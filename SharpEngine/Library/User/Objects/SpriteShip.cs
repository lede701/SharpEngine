using SharpEngine.Library.Controller;
using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Math;
using SharpEngine.Library.Objects;
using SharpEngine.Library.User.Interfaces;
using SharpEngine.Library.User.Player;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.User.Objects
{
	public class SpriteShip : USpriteObject, ITakeDamage
	{
		public PlayerStatistics PlayerStats;
		private Rectangle _boundary;
		public SpriteShip(Sprite sprite) : base(sprite, false)
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
			// Create collider for ship
			CircleCollider cldr = new CircleCollider()
			{
				Radius = (int)230 * Scale.X,
				Position = Position
			};
			cldr.Center.X = (Width / 2) - 9;
			cldr.Center.Y = (Height / 2) - 9;

			Collider = cldr;
		}

		public float Width
		{
			get
			{
				return Sprite.Frame.Width * Scale.X;
			}
		}

		public float Height
		{
			get
			{
				return Sprite.Frame.Height * Scale.Y;
			}
		}

		public UObject DebugObject { get; set; }

		private bool _canon;

		public override void Render(IGraphics g)
		{
			base.Render(g);
			// Render player stats
			float x = 10;
			float y = World.Instance.WorldSize.Y - 25;
			float maxWidth = (World.Instance.WorldSize.X - 20);
			float wWidth = maxWidth * (PlayerStats.WeaponEnergy / PlayerStats.MaxWeaponEnergy);
			float sWidth = maxWidth * (PlayerStats.ShieldEnergy / PlayerStats.MaxShieldEnergy);

			// Draw energy levels
			g.FillRectangle(x, y, wWidth, 5, Color.FromArgb(120, 252, 119, 3));
			g.DrawRectangle(x, y, maxWidth, 5, Color.FromArgb(120, 252, 119, 3));
			// Draw shield levels
			g.FillRectangle(x, y + 7, sWidth, 5, Color.FromArgb(120, 60, 177, 255));
			g.DrawRectangle(x, y + 7, maxWidth, 5, Color.FromArgb(120, 60, 177, 255));
			// Draw shields
			float shieldsLevel = (float)(PlayerStats.ShieldEnergy / PlayerStats.MaxShieldEnergy);
			int shieldsAlpha = (int)(100f * shieldsLevel);
			float radius = ((CircleCollider)Collider).Radius;
			RectangleF shieldsRect = new RectangleF
			{
				X = Position.X + (Width / 2),
				Y = Position.Y + (Height / 2),
				Width = radius,
				Height = radius
			};
			List<System.Drawing.Color> colors = new List<Color>();
			colors.Add(Color.FromArgb(5, 80, 80, 200));
			colors.Add(Color.FromArgb(10, 80, 80, 200));
			colors.Add(Color.FromArgb(shieldsAlpha, 8, 152, 255));
			g.FillGradientEllipse(shieldsRect, colors.ToArray());
			g.DrawEllipse(shieldsRect, Color.FromArgb((int)(210 * shieldsLevel), 175, 219, 250));
			/*
			g.FillEllipse(shieldsRect, Color.FromArgb(shieldsAlpha, 128, 128, 255));
			g.DrawEllipse(shieldsRect, Color.FromArgb(shieldsAlpha, 128, 128, 255));
			g.FillEllipse(shieldsRect.X, shieldsRect.Y, 5, 5, Color.FromArgb(200, 0, 0, 255));
			*/
		}

		public override void Update(float deltaTime)
		{
			// Call the base update method
			base.Update(deltaTime);
			Rectangle src = Sprite.Frame;
			Rectangle dest = new Rectangle
			{
				X = (int)Position.X,
				Y = (int)Position.Y,
				Width = (int)((float)src.Width * Scale.X),
				Height = (int)((float)src.Height * Scale.Y)
			};
			// Check if sprite has gone beoyond boundary
			if (Position.X + dest.Width > _boundary.Width)
			{
				Position.X = _boundary.Width - dest.Width;
			}
			else if (Position.X < _boundary.X)
			{
				Position.X = _boundary.X;
			}
			if(Position.Y + dest.Height > _boundary.Height)
			{
				Position.Y = _boundary.Height - dest.Height;
			}else if(Position.Y < _boundary.Y)
			{
				Position.Y = _boundary.Y;
			}
			if(Controller.Get(Input.Fire) && PlayerStats.CanFire)
			{
				// Spawn blaster object
				ShipBlaster bolt = new ShipBlaster(false);
				int offset = (int)(90 * Scale.X);
				bolt.Position.X = Position.X + (_canon ? offset : Sprite.Frame.Width * Scale.X - offset);
				_canon = !_canon;
				bolt.Position.Y = Position.Y - 5.0f;
				bolt.Velocity.Y = -4.0f;
				bolt.DebugObject = DebugObject;
				bolt.Type = ObjectType.MISSLE;
				SceneManager.Instance.Scene.Add(bolt, 4);
				float damage = PlayerStats.Fire;
			}

			PlayerStats.Update(deltaTime);
		}

		public float TakeDamage(float damage)
		{
			PlayerStats.ShieldEnergy -= damage;
			return PlayerStats.TotalLife;
		}
	}
}
