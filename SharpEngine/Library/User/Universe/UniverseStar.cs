using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Math;
using SharpEngine.Library.Objects;
using SharpEngine.Library.Randomizer;

namespace SharpEngine.Library.User.Universe
{
	public class UniverseStar : IUniverseItem
	{
		private System.Drawing.Rectangle starShape;
		private System.Drawing.Color color;
		private String _key = Guid.NewGuid().ToString();
		public string Key
		{
			get
			{
				return _key;
			}
		}

		public UniverseStar()
		{
			starShape = new System.Drawing.Rectangle
			{
				X = RandomManager.Instance.Next(0, (int)World.Instance.WorldSize.X),
				Y = RandomManager.Instance.Next(0, (int)World.Instance.WorldSize.X),
				Width = 1,
				Height = 1
			};

			color = System.Drawing.Color.White;
		}

		public void Dispose()
		{
		}

		public void Render(IGraphics g)
		{
			g.DrawEllipse(starShape, color);
		}

		public void Update(float deltaTime)
		{
			
		}
	}
}
