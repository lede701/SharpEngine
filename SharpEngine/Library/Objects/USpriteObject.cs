using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpEngine.Library.Controller;
using SharpEngine.Library.Events;
using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Math;

namespace SharpEngine.Library.Objects
{
	public class USpriteObject : UObject
	{
		private Sprite _sprite;
		public Sprite Sprite
		{
			get
			{
				return _sprite;
			}
			set
			{
				_sprite = value;
			}
		}
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
				// Make sure old collider is remove form the event handler to this object
				if(_collider != null)
				{
					_collider.CollisionEvent -= OnCollision;
				}
				_collider = value;
				// Connect collider to event handler
				_collider.CollisionEvent += OnCollision;
			}
		}

		public virtual void OnCollision(object sender, EventArgs e)
		{
			if(e is CollisionEventArgs)
			{
				CollisionEventArgs ce = (CollisionEventArgs)e;
				switch(ce.Who.Collider.Type)
				{
					case Collider2DType.PlaneY:
						{
							PlaneCollider pc = (PlaneCollider)ce.Who.Collider;
							if (ce.Location == CollisionEventArgs.HitLocation.Top)
							{
								Position.Y = ce.Point.Y;
							}
							else
							{
								Position.Y = ce.Point.Y - (((CircleCollider)Collider).Radius * 2);
							}
						}
						break;
					case Collider2DType.PlaneX:
						{
							PlaneCollider pc = (PlaneCollider)ce.Who.Collider;
							if(ce.Location == CollisionEventArgs.HitLocation.Left)
							{
								Position.X = pc.Tupal;
							}else
							{
								Position.X = pc.Tupal - (((CircleCollider)Collider).Radius * 2);
							}
						}
						break;
				}
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

		public USpriteObject(Sprite sprite)
		{
			_transform = new Transform();
			_key = Guid.NewGuid().ToString();
			Sprite = sprite;
			
		}

		public virtual void Render(Graphics g)
		{
			g.TranslateTransform(Position.X, Position.Y);
			Rectangle src = Sprite.Frame;
			Rectangle dest = new Rectangle
			{
				X = src.X,
				Y = src.Y,
				Width = (int)((float)src.Width * Scale.X),
				Height = (int)((float)src.Height * Scale.Y)
			};
			g.DrawImage(Sprite.SpriteSheet, dest, src, GraphicsUnit.Pixel );
			g.TranslateTransform(-Position.X, -Position.Y);
#if DEBUG
			DebugRender(g);
#endif
		}

		public void DebugRender(Graphics g)
		{
			if(Collider != null)
			{
				switch(Collider.Type)
				{
					case Collider2DType.Circle:
						CircleCollider cc = (CircleCollider)Collider;
						Brush diag = new SolidBrush(Color.FromArgb(15, 0, 255, 0));
						Rectangle cldrCircle = new Rectangle
						{
							X = (int)cc.Position.X,
							Y = (int)cc.Position.Y,
							Width = (int)cc.Radius * 2,
							Height = (int)cc.Radius * 2
						};
						g.DrawEllipse(Pens.LightGreen, cldrCircle);
						g.FillEllipse(Brushes.LightGreen, cc.Position.X + cc.Radius, cc.Position.Y + cc.Radius, 4, 4);
						g.FillEllipse(diag, cldrCircle);
						break;
				}
			}
		}

		public virtual void Update(float deltaTime)
		{
			if (Controller != null)
			{
				Position.X += (Velocity.X * deltaTime * Controller.GetValue(Input.Right)) - (Velocity.X * deltaTime * Controller.GetValue(Input.Left));
				Position.Y += (Velocity.Y * deltaTime * Controller.GetValue(Input.Down)) - (Velocity.Y * deltaTime * Controller.GetValue(Input.Up));
			}

		}
	}
}
