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
		private float _starSpeed;
		private String _key = Guid.NewGuid().ToString();
		public string Key
		{
			get
			{
				return _key;
			}
		}

		public UniverseStar(System.Drawing.Rectangle range)
		{
			starShape = new System.Drawing.Rectangle
			{
				X = RandomManager.Instance.Next(range.Left, range.Right),
				Y = RandomManager.Instance.Next(range.Top, range.Bottom),
				Width = 1,
				Height = 1
			};

			color = System.Drawing.Color.FromArgb(RandomManager.Instance.Next(50, 200), 255, 255, 255);
			_starSpeed = (float)(RandomManager.Instance.Next(50, 200) / 100f);
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
