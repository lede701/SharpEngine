using SharpEngine.Library.Controller;
using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Math;
using SharpEngine.Library.Objects;
using SharpEngine.Library.Randomizer;
using SharpEngine.Library.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
			set
			{
				Transform.Position = value;
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
				return Transform.Rotation.Angle;
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
		private bool _alwaysRender = true;
		public bool AlwaysRender
		{
			get
			{
				return _alwaysRender;
			}
			set
			{
				_alwaysRender = value;
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

		private ThreadManager.ThreadNode _node;

		public SpriteStarField()
		{
			RandomManager rm = RandomManager.Instance;
			_key = Guid.NewGuid().ToString();
			_transform = new Transform();
			_stars = new List<Star>();
			for (int i = 0; i < 100; ++i)
			{
				int r = rm.Next(1, 3);
				Star star = new Star
				{
					id = i,
					raidus = r,
					clr = Color.FromArgb(rm.Next(80, 120), rm.Next(120, 180), rm.Next(100, 160), rm.Next(120, 255)),
					pos = new Vector2D
					{
						X = rm.Next(1, (int)World.Instance.WorldSize.X),
						Y = rm.Next(1, (int)World.Instance.WorldSize.Y)
					},
					speed = (float)(rm.Next(50, 4200) / 1000.0f)
				};
				_stars.Add(star);
			}

			_node = ThreadManager.CreateThread(UpdateLoop);
			_node.Start();
		}

		public void Render(IGraphics g)
		{
			foreach(Star s in _stars)
			{
				g.FillEllipse(s.pos.X, s.pos.Y, s.raidus, s.raidus, s.clr);
			}
			//g.DrawImage(_starField, 0, 0, _starField.Width, _starField.Height);
		}

		private void UpdateLoop()
		{
			float frameTime = 1000f / 60f;
			Stopwatch timer = new Stopwatch();
			while (_node.IsRunning)
			{
				timer.Restart();
				timer.Start();
				foreach (Star s in _stars)
				{
					s.pos.Y = (s.pos.Y + s.speed);
					if (s.pos.Y > World.Instance.WorldSize.Y)
					{
						s.pos.Y = s.pos.Y % World.Instance.WorldSize.Y;
						s.pos.X = RandomManager.Instance.Next(10, (int)World.Instance.WorldSize.X);
						s.speed = (float)(RandomManager.Instance.Next(50, 4200) / 1000.0f);
					}
				}
				timer.Stop();
				float waitTime = frameTime - timer.ElapsedMilliseconds;
				if(waitTime > 0f)
				{
					ThreadManager.Sleep((int)waitTime, _node);
				}
			}

		}

		public void Update(float deltaTime)
		{
			/*
			foreach (Star s in _stars)
			{
				s.pos.Y = (s.pos.Y + (s.speed * deltaTime));
				if(s.pos.Y > World.Instance.WorldSize.Y)
				{
					s.pos.Y = s.pos.Y % World.Instance.WorldSize.Y;
					s.pos.X = RandomManager.Instance.Next(10, (int)World.Instance.WorldSize.X);
				}
			}
			*/
		}

		public void Dispose()
		{
			_node.IsRunning = false;
			_node.Stop();
		}
	}
}
