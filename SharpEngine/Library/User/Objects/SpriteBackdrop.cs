﻿using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.User.Objects
{
	public class SpriteBackdrop : USpriteObject
	{
		public SpriteBackdrop(Sprite sprite) : base(sprite)
		{
			Position.Y = Sprite.Frame.Height - (int)World.Instance.WorldSize.Y;
		}

		public override void Render(Graphics g)
		{
			Rectangle rect = new Rectangle
			{
				X = (int)Position.X,
				Y = (int)Position.Y,
				Width = (int)World.Instance.WorldSize.X,
				Height = (int)World.Instance.WorldSize.Y
			};
			Rectangle dest = new Rectangle
			{
				X = 0,
				Y = 0,
				Width = rect.Width,
				Height = rect.Height
			};

			g.DrawImage(Sprite.SpriteSheet, dest, rect, GraphicsUnit.Pixel);
		}
		public override void Update(float deltaTime)
		{
			Position.Y = System.Math.Max(0, Position.Y - 0.5f);
		}
	}
}
