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

		private static GraphicsManager _instance;
		public static GraphicsManager Instance
		{
			get
			{
				return _instance;
			}
			set
			{
				_instance = value;
			}
		}
		public GraphicsManager(IntPtr win)
		{
		}

		public void Render()
		{
		}

		public Graphics Graphics
		{
			get
			{
				return null;
			}
		}
	}
}
