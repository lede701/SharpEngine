﻿using SharpEngine.Library.Controller;
using SharpEngine.Library.Math;
using SharpEngine.Library.Objects;
using SharpEngine.Library.Randomizer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.User.Objects
{
	public class SpriteStarField : GObject
	{

		private Transform _transform;
		public Transform Transform
		{
			get
			{
				return _transform;
			}
		}

		public Vector2D Position
		{
			get
			{
				return Transform.Position;
			}
		}

		public Vector2D Velocity
		{
			get
			{
				return Transform.Velocity;
			}
		}

		public Vector2D Scale
		{
			get
			{
				return Transform.Scale;
			}
		}

		public float Rotation
		{
			get
			{
				return Transform.Rotation;
			}
		}

		private String _key;
		public string Key
		{
			get
			{
				return _key;
			}
		}
		private class Star
		{
			public int id;
			public Vector2D pos;
			public int raidus;
			public Color clr;
			public float speed;
		}
		private List<Star> _stars;

		public SpriteStarField()
		{
			RandomManager rm = RandomManager.Instance;
			_key = Guid.NewGuid().ToString();
			_transform = new Transform();
			_stars = new List<Star>();
			for (int i = 0; i < 200; ++i)
			{
				int r = rm.Next(2, 5);
				Star star = new Star
				{
					id = i,
					raidus = r,
					clr = Color.FromArgb(rm.Next(100, 255), rm.Next(100, 255), rm.Next(100, 255), 255),
					pos = new Vector2D
					{
						X = rm.Next(1, (int)World.Instance.WorldSize.X),
						Y = rm.Next(1, (int)World.Instance.WorldSize.Y)
					},
					speed = (float)(rm.Next(50, 600) / 100.0f)
				};
				_stars.Add(star);
			}
		}

		public void Render(Graphics g)
		{
			foreach(Star s in _stars)
			{
				SolidBrush clr = new SolidBrush(s.clr);
				g.FillEllipse(clr, s.pos.X, s.pos.Y, s.raidus, s.raidus);
				// Add debug text for now
				/*
				using (Font font = new Font("Arial", 8))
				{
					using (SolidBrush brush = new SolidBrush(s.clr))
					{
						String diagTxt = String.Format("{0}", s.id);
						SizeF size = g.MeasureString(diagTxt, font);
						g.DrawString(diagTxt, font, brush, s.pos.X - (int)(size.Width / 2), s.pos.Y + 8);
					}
				}
				//*/
			}
		}

		public void Update(float deltaTime)
		{
			foreach (Star s in _stars)
			{
				s.pos.Y = (s.pos.Y + s.speed);
				if(s.pos.Y > World.Instance.WorldSize.Y)
				{
					s.pos.Y = s.pos.Y % World.Instance.WorldSize.Y;
					s.pos.X = RandomManager.Instance.Next(10, (int)World.Instance.WorldSize.X);
				}
			}
		}
	}
}