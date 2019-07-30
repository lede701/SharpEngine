using SharpEngine.Library.Controller;
using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Math;
using SharpEngine.Library.Objects;
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

		public ShipBlaster(bool debug = false)
		{
			_key = Guid.NewGuid().ToString();
			_transform = new Transform();
			_collider = new CircleCollider();
			_collider.Radius = 3.0f;
			_collider.Position = Position;
			_debug = debug;
			Collider.CollisionEvent += OnCollision;
			dr = 0;
			dg = 200;
			db = 0;
		}

		private int dr;
		private int dg;
		private int db;

		private void OnCollision(object sender, EventArgs e)
		{
			dr = 255;
			dg = 0;
		}

		public void Render(IGraphics g)
		{
			g.FillEllipse(Position.X, Position.Y, 2.0f, 25.0f, Color.FromArgb(180, 252, 119, 3));
			if(Debug)
			{
				CircleCollider cc = (CircleCollider)Collider;
				RectangleF rect = new RectangleF
				{
					X = Position.X,
					Y = Position.Y,
					Width = cc.Radius * 2,
					Height = cc.Radius * 2
				};
				g.FillEllipse(rect, Color.FromArgb(80, dr, dg, db));
				g.DrawEllipse(rect, Color.FromArgb(120, dr, dg, db));
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
		}

		public void Update(float deltaTime)
		{
			Position.X += Velocity.X;
			Position.Y += Velocity.Y;
			// Check if bolt needs to be auto destroyed
			if(Position.Y < -10)
			{
				SceneManager.Instance.Scene.Remove(this, 4);
			}
		}

		public void Dispose()
		{
			// Nothing to dispose just yet
		}
	}
}
