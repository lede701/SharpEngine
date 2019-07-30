using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Math;
using SharpEngine.Library.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.User.Objects
{
	public class SpriteAsteroid : USpriteObject
	{
		public SpriteAsteroid(Sprite sprite) : base(sprite, true)
		{
			sprite.Frames.Add(new System.Drawing.Rectangle
			{
				X = 0,
				Y = 0,
				Width = 125,
				Height = 125
			});
			CircleCollider cldr = new CircleCollider();

			cldr.Radius = 25;
			cldr.Position = Position;
			Collider = cldr;
			Type = ObjectType.ENEMY;
		}

		public override void OnCollision(object sender, EventArgs e)
		{
			// Set to figure out what hit this object
			String message = e.ToString();
		}

		public override void Render(IGraphics g)
		{
			base.Render(g);
		}
	}

}
