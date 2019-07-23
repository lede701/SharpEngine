using SharpEngine.Library.Controller;
using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Math;
using SharpEngine.Library.Objects;
using SharpEngine.Library.User.Player;
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
		public PlayerStatistics PlayerStats;
		private Rectangle _boundary;
		public SpriteShip(Sprite sprite) : base(sprite)
		{
			_boundary = new Rectangle
			{
				X = 10,
				Y = 50,
				Height = (int)World.Instance.WorldSize.Y - 80,
				Width = (int)World.Instance.WorldSize.X - 10
			};
			Scale.X = Scale.Y = 0.20f;
			PlayerStats = new PlayerStatistics();
		}

		private bool _canon;

		public override void Render(Graphics g)
		{
			base.Render(g);
			// Render player stats
			float x = 10;
			float y = World.Instance.WorldSize.Y - 25;
			float maxWidth = (World.Instance.WorldSize.X - 20);
			float wWidth = maxWidth * (PlayerStats.WeaponEnergy / PlayerStats.MaxWeaponEnergy);
			float sWidth = maxWidth * (PlayerStats.ShieldEnergy / PlayerStats.MaxShieldEnergy);
			using (SolidBrush brush = new SolidBrush(Color.FromArgb(120, 255, 0, 0)))
			{
				g.FillRectangle(brush, x, y, wWidth, 5);
			}
			using (Pen p = new Pen(Color.FromArgb(120, 255, 0, 0)))
			{
				g.DrawRectangle(p, x, y, maxWidth, 5);
			}
			using (SolidBrush brush = new SolidBrush(Color.FromArgb(120, 60, 177, 255)))
			{
				g.FillRectangle(brush, x, y + 7, sWidth, 5);
			}
			using (Pen p = new Pen(Color.FromArgb(120, 60, 177, 255)))
			{
				g.DrawRectangle(p, x, y+7, maxWidth, 5);
			}
		}

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
			if(Controller.Get(Input.Fire) && PlayerStats.CanFire)
			{
				// Spawn blaster object
				ShipBlaster bolt = new ShipBlaster();
				int offset = 15;
				bolt.Position.X = Position.X + (_canon ? offset : Sprite.Frame.Width * Scale.X - offset);
				_canon = !_canon;
				bolt.Position.Y = Position.Y - 5.0f;
				bolt.Velocity.Y = -20.0f;
				SceneManager.Instance.Scene.Add(bolt, 4);
				float damage = PlayerStats.Fire;
			}

			PlayerStats.Update(deltaTime);
		}
	}
}
