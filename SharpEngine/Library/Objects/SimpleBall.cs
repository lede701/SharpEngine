using SharpEngine.Library.Controller;
using SharpEngine.Library.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Objects
{
	public class SimpleBall : UObject
	{
		private String _key;
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
			}
		}

		public String Key
		{
			get
			{
				return _key;
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
				return Transform.Velocity;
			}
		}

		public float Rotation
		{
			get
			{
				return Transform.Rotation;
			}
		}

		private int _radius;
		private Rectangle _boundary;
		private bool _isMoving;

		public SimpleBall(int radius, Vector2D position, Rectangle boundary)
		{
			// Create a new tranform object
			_transform = new Transform(position, new Vector2D { X = 0.0f, Y = 0.0f });
			_radius = radius;
			_key = Guid.NewGuid().ToString();
			// TODO: move to the collider for testing instead
			_boundary = boundary;
			_isMoving = true;
		}

		public void Render(Graphics g)
		{
			g.FillEllipse(Brushes.Red, Position.X, Position.Y, _radius, _radius);
		}

		public void Update(float deltaTime)
		{
			if(_isMoving)
			{

				if(_controller != null)
				{
					Position.X += Velocity.X * deltaTime * Controller.GetValue(Input.Right);
					Position.X -= Velocity.X * deltaTime * Controller.GetValue(Input.Left);
					Position.Y += Velocity.Y * deltaTime * Controller.GetValue(Input.Down);
					Position.Y -= Velocity.Y * deltaTime * Controller.GetValue(Input.Up);
				}
				else
				{
					Position.X += Velocity.X * deltaTime;
					Position.Y += Velocity.Y * deltaTime;
				}

				if (Position.X < _boundary.X || Position.X > _boundary.Width)
				{
					Position.X = Position.X < _boundary.X ? _boundary.X : _boundary.Width;
				}
				if (Position.Y < _boundary.Y || Position.Y > _boundary.Height)
				{
					Position.Y = Position.Y < _boundary.Y ? _boundary.Y : _boundary.Height;
					Velocity.Y *= -0.6f;
					if (System.Math.Abs(Velocity.Y) < 0.2f)
					{
						_isMoving = false;
					}
				}else if(Controller == null)
				{
					// Apply gravity to the object
					Velocity.Y += 0.2f;
				}

			}
		}
	}
}
