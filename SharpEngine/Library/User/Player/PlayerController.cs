using SharpEngine.Library.Controller;
using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Math;
using SharpEngine.Library.Objects;
using SharpEngine.Library.User.Universe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.User.Player
{
	public class PlayerController : GObject
	{
		private IController _controller;

		public UObject Player;
		public UniverseMaster Universe;

		private String _key = Guid.NewGuid().ToString();
		public string Key
		{
			get
			{
				return _key;
			}
		}

		public PlayerController(IController controller)
		{
			_controller = controller;
			_prevVelocity = Vector2D.Zero;
			_prevRotationSpeed = 0.0f;
		}

		private Vector2D _prevVelocity;
		private float _prevRotationSpeed;
		private float _frameCnt;

		public void Update(float deltaTime)
		{
			float rotSpeed = _controller.Get(Input.RightShift) ? 0.08f : 0.02f;
			float thrustSpeed = _controller.Get(Input.RightShift) ? 8.0f : 5.0f;

			float nextRotSpeed = ((rotSpeed * _controller.GetValue(Input.Right)) - (rotSpeed * _controller.GetValue(Input.Left))) * deltaTime;
			float framePer = 1.0f;

			// Change ship rotation
			Player.Transform.Rotation += Vector2D.Lerp(nextRotSpeed, _prevRotationSpeed, framePer);
			// Check if the booster is on
			if (_controller.Get(Input.Up))
			{
				// Calculate the thrust of ship
				float up = _controller.GetValue(Input.Up) * deltaTime;
				float vx = up * thrustSpeed * (float)System.Math.Sin(Player.Rotation);
				float vy = up * thrustSpeed * (float)System.Math.Cos(Player.Rotation);
				World.Instance.WorldPosition.X += vx;
				World.Instance.WorldPosition.Y -= vy;
				Player.Position.X += vx;
				Player.Position.Y -= vy;
			}

			Player.Update(deltaTime);
			Universe.Update(deltaTime);
		}

		public void Render(IGraphics g)
		{
			Universe.Render(g);
			Player.Render(g);
		}

		public void Dispose()
		{
			Player.Dispose();
			Universe.Dispose();
		}
	}
}
