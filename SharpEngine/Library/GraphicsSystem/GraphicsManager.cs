using SharpDX.Direct2D1;
using SharpDX.Direct3D11;
using SharpEngine.Library.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpEngine.Library.GraphicsSystem
{
	public class GraphicsManager
	{
		private SharpDX.Direct3D11.Device d3dDevice;
		private SharpDX.Direct2D1.Device d2dDevice;
		private SharpDX.Direct2D1.DeviceContext d2dContext;

		private WindowRenderTarget renderer;
		private SharpDX.Direct2D1.Factory factory;

		private SolidColorBrush clrColor;

		private static GraphicsManager _instance;
		public static GraphicsManager Instance
		{
			get
			{
				return _instance;
			}
		}
		public GraphicsManager(IntPtr win)
		{
			if(GraphicsManager._instance == null)
			{
				GraphicsManager._instance = this;
			}

			factory = new SharpDX.Direct2D1.Factory(SharpDX.Direct2D1.FactoryType.SingleThreaded);
			HwndRenderTargetProperties hwndProperties = new HwndRenderTargetProperties
			{
				Hwnd = win,
				PixelSize = new SharpDX.Size2((int)World.Instance.WorldSize.X, (int)World.Instance.WorldSize.Y),
				PresentOptions = PresentOptions.None
			};

			RenderTargetProperties rendProperties = new RenderTargetProperties(new PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied));
			renderer = new WindowRenderTarget(factory, rendProperties, hwndProperties);
			renderer.AntialiasMode = AntialiasMode.PerPrimitive;

			clrColor = new SolidColorBrush(renderer, new SharpDX.Mathematics.Interop.RawColor4(0f, 0f, 0f, 1f));
		}

		public void Render()
		{
			renderer.BeginDraw();
			renderer.Clear(clrColor.Color);

			renderer.FillRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(10, 10, 100, 100), new SharpDX.Direct2D1.SolidColorBrush(renderer, new SharpDX.Mathematics.Interop.RawColor4(1f, 0f, 0f, 1f)));

			renderer.Flush();
			renderer.EndDraw();
		}

		public void Dispose()
		{
			renderer.Dispose();
		}

	}
}
