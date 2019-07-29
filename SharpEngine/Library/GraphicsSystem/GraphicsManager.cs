using SharpEngine.Library.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX;

namespace SharpEngine.Library.GraphicsSystem
{
	public class GraphicsManager : IGraphics
	{
		private SharpDX.Direct3D11.Device d3dDevice;
		private SharpDX.Direct3D11.DeviceContext d3dContext;
		private SwapChain swapChain;
		private Surface surface;

		private SharpDX.DirectWrite.Factory dwFactory;

		private Texture2D target;
		private RenderTargetView targetView;
		SharpDX.Direct2D1.Factory d2dFactory;
		private SharpDX.Direct2D1.RenderTarget d2dRenderTarget;

		private Form _win;

		private bool _inRender;

		private Object _lock;


		#region Singleton

		private static GraphicsManager _instance;
		public static GraphicsManager Instance
		{
			get
			{
				return _instance;
			}
		}

		#endregion

		#region C'tors
		public GraphicsManager(Form win)
		{
			if(GraphicsManager._instance == null)
			{
				GraphicsManager._instance = this;
			}
			_win = win;

			// Create swap chain descirpiom object
			SwapChainDescription scd = new SwapChainDescription
			{
				BufferCount = 2,
				Flags = SwapChainFlags.None,
				IsWindowed = true,
				ModeDescription = new ModeDescription(win.ClientSize.Width, win.ClientSize.Height, new Rational(60, 1), Format.B8G8R8A8_UNorm),
				OutputHandle = win.Handle,
				SampleDescription = new SampleDescription(1, 0),
				SwapEffect = SwapEffect.FlipDiscard,
				Usage = Usage.RenderTargetOutput
			};

			/* Reference
			* https://stackoverflow.com/questions/26220964/sharpdxhow-to-place-sharpdx-window-in-winforms-window
			* https://github.com/sharpdx/SharpDX-Samples/blob/master/Desktop/Direct2D1/MiniRect/Program.cs
			* https://github.com/r2d2rigo/MyFirstDirect2D-SharpDX/blob/master/MyFirstDirect2D/MyViewProvider.cs
			*/

			// Create our DirectX 11 device object and swap chain
			SharpDX.Direct3D11.Device.CreateWithSwapChain(
				SharpDX.Direct3D.DriverType.Hardware,
				DeviceCreationFlags.Debug | DeviceCreationFlags.BgraSupport,
				scd,
				out d3dDevice, out swapChain );
			// Get access to the DirectX 11 context
			d3dContext = d3dDevice.ImmediateContext;

			// Create the Direct2D factory object, used in building a render target
			d2dFactory = new SharpDX.Direct2D1.Factory();
			dwFactory = new SharpDX.DirectWrite.Factory();

			// Create a texture target
			target = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
			targetView = new RenderTargetView(d3dDevice, target);

			// Create a surface from target
			surface = target.QueryInterface<Surface>();

			// Finally create the important Direct2D render target
			d2dRenderTarget = new SharpDX.Direct2D1.RenderTarget(d2dFactory, surface, new SharpDX.Direct2D1.RenderTargetProperties(
				new SharpDX.Direct2D1.PixelFormat(Format.Unknown, SharpDX.Direct2D1.AlphaMode.Premultiplied)
			));

			// Initiallize the Graphic Manager internal parameters
			_inRender = false;
			_lock = new Object();

		}

		#endregion

		#region Render Tools

		public void BeginDraw()
		{
			if (!_inRender)
			{
				lock (_lock)
				{
					_inRender = true;
					d2dRenderTarget.BeginDraw();
				}
			}
		}

		public void Clear()
		{
			Clear(_win.BackColor);
		}

		public void Clear(System.Drawing.Color color)
		{
			d2dRenderTarget.Clear(ToColor(color));
		}

		public void EndDraw()
		{
			if (_inRender)
			{
				lock (_lock)
				{
					d2dRenderTarget.EndDraw();
					_inRender = false;
				}
			}
		}

		public void Render()
		{
			lock (_lock)
			{
				swapChain.Present(0, PresentFlags.None);
			}
		}

		#endregion

		#region Drawing Tools
		public void DrawEllipse(int x, int y, int width, int height, System.Drawing.Color clr)
		{
			DrawEllipse((float)x, (float)y, (float)width, (float)height, clr);
		}

		public void DrawEllipse(System.Drawing.Rectangle rect, System.Drawing.Color clr)
		{
			DrawEllipse((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height, clr);
		}

		public void DrawEllipse(float x, float y, float width, float height, System.Drawing.Color clr)
		{
			SharpDX.Mathematics.Interop.RawVector2 center = new SharpDX.Mathematics.Interop.RawVector2(x, y);
			SharpDX.Direct2D1.SolidColorBrush brush = new SharpDX.Direct2D1.SolidColorBrush(d2dRenderTarget, ToColor(clr));
			SharpDX.Direct2D1.Ellipse ellipse = new SharpDX.Direct2D1.Ellipse(center, width, height);
			d2dRenderTarget.DrawEllipse(ellipse, brush);

			brush.Dispose();

		}

		public void FillEllipse(System.Drawing.Rectangle rect, System.Drawing.Color clr)
		{
			FillEllipse((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height, clr);
		}
		public void FillEllipse(int x, int y, int width, int height, System.Drawing.Color clr)
		{
			FillEllipse((float)x, (float)y, (float)width, (float)height, clr);
		}

		public void FillEllipse(float x, float y, float width, float height, System.Drawing.Color clr)
		{
			SharpDX.Mathematics.Interop.RawVector2 center = new SharpDX.Mathematics.Interop.RawVector2(x, y);
			SharpDX.Direct2D1.SolidColorBrush brush = new SharpDX.Direct2D1.SolidColorBrush(d2dRenderTarget, ToColor(clr));
			SharpDX.Direct2D1.Ellipse ellipse = new SharpDX.Direct2D1.Ellipse(center, width, height);
			d2dRenderTarget.FillEllipse(ellipse, brush);

			brush.Dispose();
		}

		public void DrawRectangle(float x, float y, float width, float height, System.Drawing.Color clr)
		{
			SharpDX.Mathematics.Interop.RawRectangleF rect = new SharpDX.Mathematics.Interop.RawRectangleF
			{
				Left = x,
				Top = y,
				Right = x + width,
				Bottom = y + height
			};
			SharpDX.Direct2D1.SolidColorBrush brush = new SharpDX.Direct2D1.SolidColorBrush(d2dRenderTarget, ToColor(clr));

			d2dRenderTarget.DrawRectangle(rect, brush, 1.0f);
		}

		public void DrawRectangle(int x, int y, int width, int height, System.Drawing.Color clr)
		{
			DrawRectangle((float)x, (float)y, (float)width, (float)height, clr);
		}
		public void DrawRectangle(System.Drawing.Rectangle rect, System.Drawing.Color clr)
		{
			DrawRectangle((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height, clr);
		}

		public void FillRectangle(float x, float y, float width, float height, System.Drawing.Color clr)
		{
			SharpDX.Mathematics.Interop.RawRectangleF rect = new SharpDX.Mathematics.Interop.RawRectangleF
			{
				Left = x,
				Top = y,
				Right = x + width,
				Bottom = y + height
			};
			SharpDX.Direct2D1.SolidColorBrush brush = new SharpDX.Direct2D1.SolidColorBrush(d2dRenderTarget, ToColor(clr));

			d2dRenderTarget.FillRectangle(rect, brush);
		}

		public void FillRectangle(int x, int y, int width, int height, System.Drawing.Color clr)
		{
			FillRectangle((float)x, (float)y, (float)width, (float)height, clr);
		}

		public void DrawImage(Object image, System.Drawing.Rectangle srcRect, System.Drawing.Rectangle destRect)
		{
			if(image is SharpDX.Direct2D1.Bitmap)
			{
				SharpDX.Direct2D1.Bitmap bImg = (SharpDX.Direct2D1.Bitmap)image;

				SharpDX.Mathematics.Interop.RawRectangleF src = new SharpDX.Mathematics.Interop.RawRectangleF
				{
					Top = srcRect.Top,
					Left = srcRect.Left,
					Bottom = srcRect.Bottom,
					Right = srcRect.Width
				};

				SharpDX.Mathematics.Interop.RawRectangleF dest = new SharpDX.Mathematics.Interop.RawRectangleF
				{
					Top = destRect.Top,
					Left = destRect.Left,
					Bottom = destRect.Bottom,
					Right = destRect.Right
				};

				d2dRenderTarget.DrawBitmap(bImg, dest, 1.0f, SharpDX.Direct2D1.BitmapInterpolationMode.Linear, src);
			}
		}

		public void DrawText(String message, String fontFamily, float fontSize, System.Drawing.Color clr, System.Drawing.Rectangle area)
		{

			SharpDX.DirectWrite.TextFormat fmt = new SharpDX.DirectWrite.TextFormat(dwFactory, fontFamily, fontSize);
			SharpDX.Mathematics.Interop.RawRectangleF rect = ToRectangle(area);
			SharpDX.Direct2D1.SolidColorBrush brush = new SharpDX.Direct2D1.SolidColorBrush(d2dRenderTarget, ToColor(clr));
			d2dRenderTarget.DrawText(message, fmt, rect, brush);
		}

		public void Translate(float x, float y)
		{
			d2dRenderTarget.Transform = new SharpDX.Mathematics.Interop.RawMatrix3x2
			{
				M11 = 1,
				M12 = 0,
				M21 = 0,
				M22 = 1,
				M31 = x,
				M32 = y
			};
		}

		#endregion

		#region Image Tools
		
		public Object LoadImage(String filename)
		{
			Object image = null;
			if(File.Exists(filename))
			{
				using (System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(filename))
				{
					System.Drawing.Imaging.BitmapData bData = bmp.LockBits(
						new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
						System.Drawing.Imaging.ImageLockMode.ReadOnly,
						System.Drawing.Imaging.PixelFormat.Format32bppPArgb
					);

					SharpDX.DataStream stream = new SharpDX.DataStream(bData.Scan0, bData.Stride * bData.Height, true, false);
					SharpDX.Direct2D1.PixelFormat pFormat = new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.R8G8B8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied);
					SharpDX.Direct2D1.BitmapProperties bProps = new SharpDX.Direct2D1.BitmapProperties(pFormat);

					image = new SharpDX.Direct2D1.Bitmap(d2dRenderTarget, new Size2(bmp.Width, bmp.Height), stream, bData.Stride, bProps);
					bmp.UnlockBits(bData);
					stream.Dispose();
				}
			}

			return image;
		}

		#endregion

		#region Utilities

		public SharpDX.Mathematics.Interop.RawColor4 ToColor(System.Drawing.Color clr)
		{
			float r = (float)clr.R / 255f;
			float g = (float)clr.G / 255f;
			float b = (float)clr.B / 255f;
			float a = (float)clr.A / 255f;
			return new SharpDX.Mathematics.Interop.RawColor4(r, g, b, a);
		}

		public SharpDX.Mathematics.Interop.RawRectangleF ToRectangle(System.Drawing.Rectangle rect)
		{
			return new SharpDX.Mathematics.Interop.RawRectangleF
			{
				Top = rect.Top,
				Bottom = rect.Bottom,
				Left = rect.Left,
				Right = rect.Right
			};
		}

		#endregion

		public void Dispose()
		{
			//d3dFactory.Dispose();
			surface.Dispose();
			d2dFactory.Dispose();
			d2dRenderTarget.Dispose();
			d3dDevice.Dispose();
			d3dContext.Dispose();
			swapChain.Dispose();
			target.Dispose();
			targetView.Dispose();
		}

	}
}
