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
using SharpEngine.Library.Math;

namespace SharpEngine.Library.GraphicsSystem
{

	using Matrix = SharpDX.Matrix3x2;

	public class GraphicsManager : IGraphics
	{
		private SharpDX.Direct3D11.Device d3dDevice;
		private SharpDX.Direct3D11.DeviceContext d3dContext;
		private SwapChain swapChain;
		private Surface surface;

		private SharpDX.Direct3D12.Device d3dDevice12;

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

            // Check for available settings
            Factory1 fac = new Factory1();
            Adapter adapter = fac.GetAdapter(0);
            Output mon = adapter.GetOutput(0);

            ModeDescription[] modeList =  mon.GetDisplayModeList(Format.B8G8R8A8_UNorm, DisplayModeEnumerationFlags.Interlaced);
            Rational rational = new Rational(0, 1);
            foreach(ModeDescription mode in modeList)
            {
                if(mode.Width == win.ClientSize.Width)
                {
                    rational = new Rational(mode.RefreshRate.Numerator, mode.RefreshRate.Numerator);
                }
            }

			/*
            SwapChainDescription1 scd1 = new SwapChainDescription1
            {
                BufferCount = 2,
				Flags = SwapChainFlags.None,
				SampleDescription = new SampleDescription(1, 0),
				SwapEffect = SwapEffect.Discard
            };
			*/
			

			// Create swap chain descirpiom object
			SwapChainDescription scd = new SwapChainDescription
			{
				BufferCount = 2,
				Flags = SwapChainFlags.None,
				IsWindowed = true,
				ModeDescription = new ModeDescription(win.ClientSize.Width, win.ClientSize.Height, rational, Format.R8G8B8A8_UNorm),
				OutputHandle = win.Handle,
				SampleDescription = new SampleDescription(1, 0),
				SwapEffect = SwapEffect.FlipSequential,
				Usage = Usage.RenderTargetOutput
			};

            /* Reference
			* https://stackoverflow.com/questions/26220964/sharpdxhow-to-place-sharpdx-window-in-winforms-window
			* https://github.com/sharpdx/SharpDX-Samples/blob/master/Desktop/Direct2D1/MiniRect/Program.cs
			* https://github.com/r2d2rigo/MyFirstDirect2D-SharpDX/blob/master/MyFirstDirect2D/MyViewProvider.cs
			*/
            try
            {
                // Create our DirectX 11 device object and swap chain
                SharpDX.Direct3D11.Device.CreateWithSwapChain(
                    SharpDX.Direct3D.DriverType.Hardware,
                    DeviceCreationFlags.Debug | DeviceCreationFlags.BgraSupport,
                    scd,
                    out d3dDevice, out swapChain);
            }catch(Exception e)
            {
                MessageBox.Show(e.ToString());
                return;
            }
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
				new SharpDX.Direct2D1.PixelFormat(scd.ModeDescription.Format, SharpDX.Direct2D1.AlphaMode.Premultiplied)
			));

			// Initiallize the Graphic Manager internal parameters
			_inRender = false;
			_lock = new Object();
			// Set initial tranformation of graphics system
			d2dRenderTarget.Transform = Identity;
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

		public void Invalidate()
		{
		}

		public bool WaitingRender
		{
			get
			{
				return true;
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

		public void DrawEllipse(System.Drawing.RectangleF rect, System.Drawing.Color clr)
		{
			DrawEllipse(rect.X, rect.Y, rect.Width, rect.Height, clr);
		}

		public void DrawEllipse(float x, float y, float width, float height, System.Drawing.Color clr)
		{
			SharpDX.Mathematics.Interop.RawVector2 center = new SharpDX.Mathematics.Interop.RawVector2(x, y);
			SharpDX.Direct2D1.SolidColorBrush brush = new SharpDX.Direct2D1.SolidColorBrush(d2dRenderTarget, ToColor(clr));
			SharpDX.Direct2D1.Ellipse ellipse = new SharpDX.Direct2D1.Ellipse(center, width, height);
			d2dRenderTarget.DrawEllipse(ellipse, brush);

			brush.Dispose();

		}

		public void FillGradientEllipse(float x, float y, float width, float height, System.Drawing.Color[] colors)
		{
			// Create the radial gradient brush properties object
			SharpDX.Direct2D1.RadialGradientBrushProperties radProp = new SharpDX.Direct2D1.RadialGradientBrushProperties
			{
				RadiusX = width,
				RadiusY = height,
				Center = new SharpDX.Mathematics.Interop.RawVector2(x, y)
			};
			// Create a list of gratiend stops
			List<SharpDX.Direct2D1.GradientStop> stops = new List<SharpDX.Direct2D1.GradientStop>();
			// TODO: Create a color collection that also stores the color position
			// Auto calulate color position
			for (int i = 0; i < colors.Length; ++i)
			{
				SharpDX.Direct2D1.GradientStop stop = new SharpDX.Direct2D1.GradientStop
				{
					Color = ToColor(colors[i]),
					Position = (float)(1.0 / colors.Length) * (i + 1)
				};
				stops.Add(stop);
			}

			SharpDX.Direct2D1.GradientStopCollection radStops = new SharpDX.Direct2D1.GradientStopCollection(d2dRenderTarget, stops.ToArray());
			SharpDX.Direct2D1.RadialGradientBrush rgBrush = new SharpDX.Direct2D1.RadialGradientBrush(d2dRenderTarget, ref radProp, radStops);
			SharpDX.Mathematics.Interop.RawVector2 center = new SharpDX.Mathematics.Interop.RawVector2(x, y);
			SharpDX.Direct2D1.Ellipse ellipse = new SharpDX.Direct2D1.Ellipse(center, width, height);
			d2dRenderTarget.FillEllipse(ellipse, rgBrush);

			radStops.Dispose();
			rgBrush.Dispose();
		}

		public void FillGradientEllipse(float x, float y, float width, float height, float cx, float cy, System.Drawing.Color[] colors)
		{
			// Create the radial gradient brush properties object
			SharpDX.Direct2D1.RadialGradientBrushProperties radProp = new SharpDX.Direct2D1.RadialGradientBrushProperties
			{
				RadiusX = width,
				RadiusY = height,
				Center = new SharpDX.Mathematics.Interop.RawVector2(x, y)
			};
			// Create a list of gratiend stops
			List<SharpDX.Direct2D1.GradientStop> stops = new List<SharpDX.Direct2D1.GradientStop>();
			// TODO: Create a color collection that also stores the color position
			// Auto calulate color position
			for (int i = 0; i < colors.Length; ++i)
			{
				SharpDX.Direct2D1.GradientStop stop = new SharpDX.Direct2D1.GradientStop
				{
					Color = ToColor(colors[i]),
					Position = (float)(1.0 / colors.Length) * (i + 1)
				};
				stops.Add(stop);
			}

			SharpDX.Direct2D1.GradientStopCollection radStops = new SharpDX.Direct2D1.GradientStopCollection(d2dRenderTarget, stops.ToArray());
			SharpDX.Direct2D1.RadialGradientBrush rgBrush = new SharpDX.Direct2D1.RadialGradientBrush(d2dRenderTarget, ref radProp, radStops);
			rgBrush.GradientOriginOffset = new SharpDX.Mathematics.Interop.RawVector2 { X = cx, Y = cy };
			SharpDX.Mathematics.Interop.RawVector2 center = new SharpDX.Mathematics.Interop.RawVector2(x, y);
			SharpDX.Direct2D1.Ellipse ellipse = new SharpDX.Direct2D1.Ellipse(center, width, height);
			d2dRenderTarget.FillEllipse(ellipse, rgBrush);

			radStops.Dispose();
			rgBrush.Dispose();
		}

		public void FillGradientEllipse(System.Drawing.RectangleF rect, System.Drawing.Color[] colors)
		{
			FillGradientEllipse(rect.X, rect.Y, rect.Width, rect.Height, colors);
		}

		public void FillEllipse(System.Drawing.Rectangle rect, System.Drawing.Color clr)
		{
			FillEllipse((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height, clr);
		}
		public void FillEllipse(System.Drawing.RectangleF rect, System.Drawing.Color clr)
		{
			FillEllipse(rect.X, rect.Y, rect.Width, rect.Height, clr);
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

		public void DrawLine(float x0, float y0, float x1, float y1, System.Drawing.Color clr)
		{
			SharpDX.Mathematics.Interop.RawVector2 p0 = new SharpDX.Mathematics.Interop.RawVector2 { X = x0, Y = y0 };
			SharpDX.Mathematics.Interop.RawVector2 p1 = new SharpDX.Mathematics.Interop.RawVector2 { X = x1, Y = y1 };
			SharpDX.Direct2D1.SolidColorBrush brush = new SharpDX.Direct2D1.SolidColorBrush(d2dRenderTarget, ToColor(clr));
			d2dRenderTarget.DrawLine(p0, p1, brush);

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
			brush.Dispose();
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
			brush.Dispose();
		}

		public void FillRectangle(int x, int y, int width, int height, System.Drawing.Color clr)
		{
			FillRectangle((float)x, (float)y, (float)width, (float)height, clr);
		}

		public void DrawImage(Object image, System.Drawing.Rectangle srcRect, System.Drawing.Rectangle destRect)
		{
			System.Drawing.RectangleF fsrc = new System.Drawing.RectangleF
			{
				X = srcRect.X,
				Y = srcRect.Y,
				Width = srcRect.Width,
				Height = srcRect.Height
			};
			System.Drawing.RectangleF fdest = new System.Drawing.RectangleF
			{
				X = destRect.X,
				Y = destRect.Y,
				Width = destRect.Width,
				Height = destRect.Height
			};

			DrawImage(image, fsrc, fdest);
		}

		public void DrawImage(Object image, System.Drawing.RectangleF srcRect, System.Drawing.RectangleF destRect)
		{
			if (image is SharpDX.Direct2D1.Bitmap)
			{
				SharpDX.Direct2D1.Bitmap bImg = (SharpDX.Direct2D1.Bitmap)image;
				if (!bImg.IsDisposed)
				{

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
				}// Endif image is not disposed
			}// Endif image is DirectX Bitmap
		}

		public void DrawText(String message, String fontFamily, float fontSize, System.Drawing.Color clr, System.Drawing.Rectangle area)
		{

			SharpDX.DirectWrite.TextFormat fmt = new SharpDX.DirectWrite.TextFormat(dwFactory, fontFamily, fontSize);
			SharpDX.Mathematics.Interop.RawRectangleF rect = ToRectangle(area);
			SharpDX.Direct2D1.SolidColorBrush brush = new SharpDX.Direct2D1.SolidColorBrush(d2dRenderTarget, ToColor(clr));
			d2dRenderTarget.DrawText(message, fmt, rect, brush);
			fmt.Dispose();
			brush.Dispose();
		}

		public void Translate(Transform transform)
		{
			Translate(transform, Vector2D.Zero);
		}
		public void Translate(Transform transform, Vector2D center)
		{
			Vector2D pos = World.Instance.ToScreen(transform.Position);
			Matrix tran = Matrix.Translation(pos.X, pos.Y);
			Matrix rot = Matrix.Rotation(transform.Rotation.Angle, new SharpDX.Vector2(center.X, center.Y));
			Matrix sca = Matrix.Scaling(transform.Scale.X, transform.Scale.Y);
			d2dRenderTarget.Transform = rot * sca * tran;
		}

		public void Translate(float x, float y, float r = 0f, Math.Vector2D scale = null)
		{
			if(scale == null)
			{
				scale = new Math.Vector2D { X = 1f, Y = 1f };
			}
			Matrix tran = Matrix.Translation(x, y);
			Matrix rot = Matrix.Rotation(r);
			Matrix sca = Matrix.Scaling(scale.X, scale.Y);
			d2dRenderTarget.Transform = sca * rot * tran;
		}

		public void Translate(Matrix m)
		{
			d2dRenderTarget.Transform = m;
		}

		public void TranslateReset()
		{
			d2dRenderTarget.Transform = Identity;
		}

		public Matrix Identity
		{
			get
			{
				return Matrix.Identity;
			}
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

		#region Sprite Tools

		public Sprite LoadSpriteFromImagePath(String filename)
		{
			return new Sprite(LoadImage(filename));
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
