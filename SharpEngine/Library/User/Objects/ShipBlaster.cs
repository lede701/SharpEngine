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
	public class ShipBlaster : GObject
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

		public ShipBlaster()
		{
			_key = Guid.NewGuid().ToString();
			_transform = new Transform();
		}

		public void Render(Graphics g)
		{
			using (SolidBrush brush = new SolidBrush(Color.FromArgb(180, 252, 119, 3)))
			{
				g.FillEllipse(brush, Position.X, Position.Y, 3.0f, 25.0f);
			}
		}

		public void Update(float deltaTime)
		{
			Position.X += Velocity.X;
			Position.Y += Velocity.Y;
			// Check if bolt needs to be auto destroyed
			if(Position.Y < -10)
			{
				SceneManager.Instance.Scene.Remove(this);
			}
		}
	}
}
