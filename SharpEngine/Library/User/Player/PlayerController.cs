using SharpEngine.Library.Controller;
using SharpEngine.Library.GraphicsSystem;
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
		}

		public void Update(float deltaTime)
		{
			float rotSpeed = 0.08f;
			float thrustSpeed = 5.0f;
			// Change ship rotation
			Player.Transform.Rotation += ((rotSpeed * _controller.GetValue(Input.Right)) - (rotSpeed * _controller.GetValue(Input.Left))) * deltaTime;
			// Check if the booster is on
			if (_controller.Get(Input.Up))
			{
				// Calculate the thrust of ship
				float up = _controller.GetValue(Input.Up) * deltaTime;
				Universe.Position.X += up * thrustSpeed * (float)System.Math.Sin(Player.Rotation);
				Universe.Position.Y -= up * thrustSpeed * (float)System.Math.Cos(Player.Rotation);
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
