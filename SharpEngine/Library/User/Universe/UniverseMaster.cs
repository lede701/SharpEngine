using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Math;
using SharpEngine.Library.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.User.Universe
{
	public class UniverseMaster : GObject
	{
		public Vector2D WorldSize;
		public Vector2D BlockSize;
		public Vector2D PlayerPosition;

		public UniverseMaster()
		{
		
		}

		private String _key = Guid.NewGuid().ToString();
		public string Key
		{
			get
			{
				return _key;
			}
		}

		public void Dispose()
		{
			
		}

		public void Render(IGraphics g)
		{
			
		}

		public void Update(float deltaTime)
		{
			
		}
	}
}
