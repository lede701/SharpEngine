using SharpEngine.Library.Controller;
using SharpEngine.Library.Events;
using SharpEngine.Library.GraphicsSystem;
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
				// Connect collider to event handlers
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

		public ObjectType Type { get; set; }

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
			Collider = new CircleCollider
			{
				Radius = radius,
				Position = position
			};
			Collider.CollisionEvent += OnCollisionHandler;
		}

		private float _lastVelocity;
		private void OnCollisionHandler(object sender, EventArgs e)
		{
			UObject who = ((CollisionEventArgs)e).Who;
			switch(who.Collider.Type)
			{
				case Collider2DType.PlaneY:
					{
						PlaneCollider cldr = who.Collider as PlaneCollider;
						Position.Y = cldr.Tupal - _radius;
						Velocity.Y *= -0.8f;
						float diffVel = _lastVelocity - System.Math.Abs(Velocity.Y);
						if (System.Math.Abs(diffVel) < 0.05f)
						{
							_isMoving = false;
						}
						_lastVelocity = System.Math.Abs(Velocity.Y);
					}
					break;
				case Collider2DType.Circle:
					{
						// Need to get collision point
					}
					break;

			}
		}

		public void Render(IGraphics g)
		{
			// Updated to use Radius and cetner point
			Rectangle rect = new Rectangle
			{
				X = (int)Position.X - _radius,
				Y = (int)Position.Y - _radius,
				Width = (int)_radius * 2,
				Height = (int)_radius * 2
			};

			if (_isMoving)
			{
				g.FillEllipse(rect.X, rect.Y, rect.Width, rect.Height, Color.Red);
			}else
			{
				g.FillEllipse(rect.X, rect.Y, rect.Width, rect.Height, Color.Green);
			}
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
					Velocity.X *= -0.5f;
				}
				if (Position.Y < _boundary.Y || Position.Y > _boundary.Height - _radius)
				{
					Position.Y = Position.Y < _boundary.Y ? _boundary.Y : _boundary.Height - _radius;
					Velocity.Y *= -0.6f;
					if (System.Math.Abs(Velocity.Y) < 0.2f)
					{
						_isMoving = false;
					}
				}else if(Controller == null)
				{
					// Apply gravity to the object
					Velocity.Y += 0.005f;
				}

			}
		}

		public void Dispose()
		{
			// Nothing to dispose of yet
		}
	}
}
