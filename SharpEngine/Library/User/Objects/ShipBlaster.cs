using SharpEngine.Library.Controller;
using SharpEngine.Library.Events;
using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Math;
using SharpEngine.Library.Objects;
using SharpEngine.Library.Particles;
using SharpEngine.Library.User.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.User.Objects
{
	public class ShipBlaster : UObject
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

		private String _key;
		public String Key
		{
			get
			{
				return _key;
			}
		}

		public float Damage { get; set; }
		public int Layer { get; set; }

		public ObjectType Type { get; set; }

		private IController _controller;
		public IController Controller {
			get
			{
				return _controller;
			}
			set
			{
				_controller = value;
			}
		}
		private CircleCollider _collider;
		public ICollider Collider {
			get
			{
				return _collider;
			}
			set
			{
				if (value is CircleCollider)
				{
					_collider = (CircleCollider)value;
					_collider.Owner = this;
				}
			}
		}

		public UObject DebugObject { get; set; }

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

		public List<System.Drawing.Color> blasterTexture;

		public ShipBlaster(bool debug = false)
		{
			_key = Guid.NewGuid().ToString();
			_transform = new Transform();
			_collider = new CircleCollider();
			_collider.Radius = 3.0f;
			_collider.Position = Position;
			_collider.Owner = this;
			_debug = debug;
			Collider.CollisionEvent += OnCollision;
			blasterTexture = new List<System.Drawing.Color>();

			blasterTexture.Add(Color.FromArgb(190, 247, 233, 203));
			blasterTexture.Add(Color.FromArgb(120, 252, 55, 0));
			blasterTexture.Add(Color.FromArgb(80, 255, 81, 0));
			Damage = 1.0f;
		}

		private void OnCollision(object sender, EventArgs e)
		{
			CollisionEventArgs ce = (CollisionEventArgs)e;
			if (ce.Who.Type == ObjectType.ENEMY)
			{
				// We hit something so let see if it gets destroyed
				if(((ITakeDamage)ce.Who).TakeDamage(Damage) <= 0.0f)
				{
					SceneManager.Instance.Scene.Remove(ce.Who, ce.Who.Layer);
				}
				SceneManager.Instance.Scene.Remove(this, Layer);
				ParticleExplosion exp = new ParticleExplosion(Position);
				SceneManager.Instance.Scene.Add(exp, 6);
			}
			if(ce.Source.Type == ObjectType.ENEMY)
			{
				// We hit something so let see if it gets destroyed
				if (((ITakeDamage)ce.Source).TakeDamage(Damage) <= 0.0f)
				{
					SceneManager.Instance.Scene.Remove(ce.Source, ce.Source.Layer);
				}
				SceneManager.Instance.Scene.Remove(this, Layer);
				ParticleExplosion exp = new ParticleExplosion(Position);
				SceneManager.Instance.Scene.Add(exp, 6);
			}
		}

		public void Render(IGraphics g)
		{
			// Draw blaster to screen
			//g.FillEllipse(Position.X, Position.Y, 2.0f, 25.0f, Color.FromArgb(180, 252, 119, 3));
			g.Translate(Transform);
			g.FillGradientEllipse(0, 0, 3.0f, 25.0f, 0f, -12f, blasterTexture.ToArray());

			if (Debug)
			{
				CircleCollider cc = (CircleCollider)Collider;
				RectangleF rect = new RectangleF
				{
					X = Position.X,
					Y = Position.Y,
					Width = cc.Radius,
					Height = cc.Radius
				};
				g.FillEllipse(rect, Color.FromArgb(80, 0, 200, 0));
				g.DrawEllipse(rect, Color.FromArgb(120, 0, 200, 0));
				if(DebugObject != null)
				{
					float distX = Position.X - DebugObject.Position.X;
					float distY = Position.Y - DebugObject.Position.Y;
					int len = (int)System.Math.Sqrt((distX * distX) + (distY * distY));
					// Draw a line between these two items
					g.DrawLine(Position.X, Position.Y, DebugObject.Position.X, DebugObject.Position.Y, Color.FromArgb(200, 0, 200, 0));
					g.DrawText(len.ToString(), "Ariel", 10f, Color.White, new Rectangle{
						X = (int)(Position.X - (distX / 2)),
						Y = (int)(Position.Y - (distY / 2)),
						Width = 50,
						Height = 25
					});
				}
			}
			g.TranslateReset();
		}
		private float _life = 0f;
		private float _maxLife = 50f;

		public void Update(float deltaTime)
		{
			Position.X += Velocity.X * deltaTime;
			Position.Y += Velocity.Y * deltaTime;
			// Check if bolt needs to be auto destroyed
			_life += 1.0f * deltaTime;
			if(_life > _maxLife)
			{
				SceneManager.Instance.Scene.Remove(this, Layer);
			}
		}

		public void Dispose()
		{
			// Nothing to dispose just yet
		}
	}
}
