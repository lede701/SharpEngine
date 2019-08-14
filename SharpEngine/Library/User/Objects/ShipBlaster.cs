using SharpEngine.Library.Controller;
using SharpEngine.Library.Events;
using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Math;
using SharpEngine.Library.Math.Physics;
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

		private String _key;
		public String Key
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
					_collider.CollisionEvent += OnCollision;
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
			_debug = debug;
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
			Vector2D blasterCenter = new Vector2D(1.5f, 1.5f);
			g.Translate(Transform, blasterCenter);
			g.FillGradientEllipse(0, 0, 3.0f, 25.0f, 0f, -12f, blasterTexture.ToArray());
			g.TranslateReset();

			if (Debug)
			{
				Transform tran = new Transform(Position, Velocity);
				tran.Rotation = 0f;
				g.Translate(tran);
				CircleCollider cc = (CircleCollider)Collider;
				RectangleF rect = new RectangleF
				{
					X = 1.5f,
					Y = 1.5f,
					Width = cc.Radius,
					Height = cc.Radius
				};
				g.FillEllipse(rect, Color.FromArgb(80, 0, 200, 0));
				g.DrawEllipse(rect, Color.FromArgb(120, 0, 200, 0));
				if(DebugObject != null)
				{
					Color debugClr = Color.FromArgb(200, 0, 200, 0);
					CircleCollider acc = (CircleCollider)DebugObject.Collider;
					Vector2D destPos = (acc.Position - cc.Position) + acc.Center;

					if(destPos.Length < acc.Radius + cc.Radius)
					{
						debugClr = Color.FromArgb(200, 255, 0, 0);
					}

					int len = (int)destPos.Length; 
					// Draw a line between these two items
					g.DrawLine(1.5f, 1.5f, destPos.X, destPos.Y, debugClr);
					g.DrawText(len.ToString(), "Ariel", 10f, Color.White, new Rectangle{
						X = (int)(destPos.X / 2),
						Y = (int)(destPos.Y / 2),
						Width = 50,
						Height = 25
					});
				}
				g.TranslateReset();
			}
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
			if (Collider != null)
			{
				// Clean up the physics world
				Collider.Dispose();
			}
		}
	}
}
