using SharpEngine.Library.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX;

namespace SharpEngine.Library.GraphicsSystem
{
	public class GraphicsManager
	{
		private SharpDX.Direct3D11.Device d3dDevice;
		private SharpDX.Direct3D11.DeviceContext d3dContext;
		private SharpDX.Direct2D1.Device d2dDevice;
		private SharpDX.Direct2D1.DeviceContext d2dContext;
		private SwapChain swapChain;

		private Texture2D target;
		private RenderTargetView targetView;

		private Form _win;


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

			SharpDX.Direct3D11.Device defaultDevice = new SharpDX.Direct3D11.Device(
				SharpDX.Direct3D.DriverType.Hardware, DeviceCreationFlags.Debug | DeviceCreationFlags.BgraSupport);

			d3dDevice = defaultDevice.QueryInterface<SharpDX.Direct3D11.Device>();
			d3dContext = d3dDevice.ImmediateContext.QueryInterface<SharpDX.Direct3D11.DeviceContext>();

			using (SharpDX.DXGI.Device dxgi = d3dDevice.QueryInterface<SharpDX.DXGI.Device>())
			{
				SwapChainDescription scd = new SwapChainDescription
				{
					OutputHandle = win.Handle,
					BufferCount = 1,
					SampleDescription = new SampleDescription(1, 0),
					Usage = Usage.RenderTargetOutput,
					SwapEffect = SwapEffect.Discard
				};
			}

			// Reference
			// https://github.com/r2d2rigo/MyFirstDirect2D-SharpDX/blob/master/MyFirstDirect2D/MyViewProvider.cs


			/*
			SwapChainDescription scd = new SwapChainDescription
			{
				BufferCount = 1,
				Flags = SwapChainFlags.None,
				IsWindowed = true,
				ModeDescription = new ModeDescription(win.ClientSize.Width, win.ClientSize.Height, new Rational(60, 1), Format.B8G8R8A8_UNorm),
				OutputHandle = win.Handle,
				SampleDescription = new SampleDescription(1, 0),
				SwapEffect = SwapEffect.Discard,
				Usage = Usage.RenderTargetOutput,
			};
			*/


		}

		#endregion

		public void Render()
		{
			//d3dDevice.ImmediateContext.ClearRenderTargetView(targetView, SharpDX.Color.Black);

			//d2dContext.DrawRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(50, 50, 100, 100), 
				//new SharpDX.Direct2D1.SolidColorBrush(d2dContext, new SharpDX.Mathematics.Interop.RawColor4(1f, 0f, 0f, 1f)));

			//swapChain.Present(0, PresentFlags.None);
		}

		public void Dispose()
		{
			
			d3dDevice.Dispose();
			d3dContext.Dispose();
			/*
			swapChain.Dispose();
			target.Dispose();
			targetView.Dispose();
			*/
		}

	}
}
