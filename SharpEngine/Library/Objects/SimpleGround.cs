using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpEngine.Library.Controller;
using SharpEngine.Library.Math;

namespace SharpEngine.Library.Objects
{
	public class SimpleGround : UObject
	{
		public float Ground;
		private IController _controller;
		public IController Controller
		{
			get
			{
				return _controller;
			}
			set
			{
				_controller = value;
			}
		}
		private ICollider _collider;
		public ICollider Collider
		{
			get
			{
				return _collider;
			}
			set
			{
				_collider = value;
				_collider.Owner = this;
			}
		}

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
				return _transform.Position;
			}
		}

		public Vector2D Velocity
		{
			get
			{
				return _transform.Velocity;
			}
		}

		public float Rotation
		{
			get
			{
				return _transform.Rotation;
			}
		}

		private String _key;
		public String Key
		{
			get
			{
				return _key;
			}
		}

		public SimpleGround(float ground, Collider2DType type)
		{
			_controller = new NullController();
			Collider = new PlaneCollider(type, ground);
			Collider.CollisionEvent += OnCollisionHandler;
			_transform = new Transform();
			_key = Guid.NewGuid().ToString();
			Ground = ground;
		}

		private void OnCollisionHandler(object sender, EventArgs e)
		{
			// The ground has nothing to worry about so do nothing yet
		}

		public void Render(Graphics g)
		{
			Rectangle rect = new Rectangle();

			if(Collider.Type == Collider2DType.PlaneY)
			{
				rect.X = 0;
				rect.Width = 1000;
				if (Ground > World.Instance.CenterOfWorld.Y)
				{
					rect.Y = (int)Ground;
					rect.Height = (int)(World.Instance.WorldSize.Y - Ground);
				}
				else
				{
					rect.Y = 0;
					rect.Height = (int)Ground;
				}
			}
			else
			{
				rect.Y = 0;
				rect.Height = 1000;
				if(Ground > World.Instance.CenterOfWorld.X)
				{
					rect.X = (int)Ground;
					rect.Width = (int)(World.Instance.WorldSize.X - Ground);
				}else
				{
					rect.X = 0;
					rect.Width = (int)Ground;
				}
			}
			g.FillRectangle(Brushes.Wheat, rect);
		}

		public void Update(float deltaTime)
		{
			// Ground doesn't move so nothing to update! or does it ;)
		}
	}
}
