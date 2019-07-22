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
	public class SpriteShip : USpriteObject
	{
		private Rectangle _boundary;
		public SpriteShip(Sprite sprite) : base(sprite)
		{
			_boundary = new Rectangle
			{
				X = 10,
				Y = 50,
				Height = (int)World.Instance.WorldSize.Y - 50,
				Width = (int)World.Instance.WorldSize.X - 10
			};
			Scale.X = Scale.Y = 0.20f;
			_canFire = true;
		}

		private bool _canFire;
		private int _nextFire;
		private bool _canon;

		public override void Update(float deltaTime)
		{
			// Call the base update method
			base.Update(deltaTime);
			float radius = ((CircleCollider)Collider).Radius;
			// Check if sprite has gone beoyond boundary
			if (Position.X + (radius * 2) > _boundary.Width)
			{
				Position.X = _boundary.Width - (radius *2); ;
			}
			else if (Position.X < _boundary.X)
			{
				Position.X = _boundary.X;
			}
			if(Position.Y + radius > _boundary.Height)
			{
				Position.Y = _boundary.Height - radius;
			}else if(Position.Y < _boundary.Y)
			{
				Position.Y = _boundary.Y;
			}
			if(Controller.Get(Input.Fire) && _canFire)
			{
				// Spawn blaster object
				ShipBlaster bolt = new ShipBlaster();
				int offset = 15;
				bolt.Position.X = Position.X + (_canon ? offset : Sprite.Frame.Width * Scale.X - offset);
				_canon = !_canon;
				bolt.Position.Y = Position.Y - 5.0f;
				bolt.Velocity.Y = -20.0f;
				SceneManager.Instance.Scene.Add(bolt, 4);
				_canFire = false;
			}
			// Check if it is time to reset fire routines
			if(_nextFire++ > 8)
			{
				_nextFire = 0;
				_canFire = true;
			}
		}
	}
}
