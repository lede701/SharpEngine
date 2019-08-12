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
using SharpEngine.Library.Math.Physics;


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
				_collider.Owner = this;
			}
		}

		public ObjectType Type { get; set; }

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
								Position.Y = ce.Points[0].Y;
							}
							else
							{
								Position.Y = ce.Points[0].Y - (((CircleCollider)Collider).Radius * 2);
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

		public int Layer { get; set; }

		private bool _debug;
		public bool Debug
		{
			get
			{
				return _debug;
			}
			set
			{
				_debug = value;
			}
		}

		public USpriteObject(Sprite sprite, bool debug = true)
		{
			_transform = new Transform();
			_key = Guid.NewGuid().ToString();
			Sprite = sprite;
			_debug = debug;
		}

		public virtual void Render(IGraphics g)
		{
			g.Translate(Transform, Sprite.CenterPoint);

			Rectangle src = Sprite.Frame;
			Rectangle dest = new Rectangle
			{
				X = 0,
				Y = 0,
				Width = src.Width,
				Height = src.Height
			};
			// TODO: Write image drawing utility
			g.DrawImage(Sprite.SpriteSheet, src, dest);
			if(Debug)
			{
				DebugRender(g, dest);
			}
			RenderAddons(g);
			g.TranslateReset();
		}

		public virtual void RenderAddons(IGraphics g)
		{

		}

		public void DebugRender(IGraphics g, Rectangle rect)
		{
			g.DrawRectangle(rect, Color.FromArgb(255, 255, 0, 0));
			if (Collider != null)
			{
				switch(Collider.Type)
				{
					case Collider2DType.Circle:
						CircleCollider cc = (CircleCollider)Collider;
						Rectangle cldrCircle = new Rectangle
						{
							X = (int)(rect.X + cc.Center.X),
							Y = (int)(rect.Y + cc.Center.Y),
							Width = (int)cc.Radius,
							Height = (int)cc.Radius
						};
						g.FillEllipse(cldrCircle, Color.FromArgb(25, 0, 180, 0));
						g.FillEllipse(cldrCircle.X, cldrCircle.Y, 5, 5, Color.FromArgb(120, 0, 180, 0));
						g.DrawEllipse(cldrCircle, Color.FromArgb(120, 0, 255, 0));
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

		public virtual void Dispose()
		{
			if (Sprite.AutoDispose)
			{
				Sprite.Dispose();
			}
		}
	}
}
