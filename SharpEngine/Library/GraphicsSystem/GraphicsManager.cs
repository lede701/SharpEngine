using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.GraphicsSystem
{
	public class GraphicsManager
	{
		private static GraphicsManager _instance;
		public static GraphicsManager Instance
		{
			get
			{
				if(_instance == null)
				{
					_instance = new GraphicsManager();
				}
				return _instance;
			}
		}
		private GraphicsManager()
		{

		}
	}
}
