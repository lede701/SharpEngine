using SharpEngine.Library.Controller;
using SharpEngine.Library.Events;
using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Math;
using SharpEngine.Library.Objects;
using SharpEngine.Library.User.Interfaces;
using SharpEngine.Library.User.Player;
using SharpEngine.Library.User.Player.Weapons;
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
		private List<Color> _shieldTexture;

		public float Life
		{
			get
			{
				return PlayerStats.ShieldEnergy;
			}
		}

		private IWeapon _weapon;
		public IWeapon Weapon
		{
			get
			{
				return _weapon;
			}
		}

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
			_weapon = new SingleBlaster(Width);
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

			// Draw shields
			float shieldsLevel = System.Math.Max((float)(PlayerStats.ShieldEnergy / PlayerStats.MaxShieldEnergy), 0f);
			int shieldsAlpha;
			if (shieldsLevel < 1.0f)
			{
				shieldsAlpha = (int)(80f * shieldsLevel);
			}else
			{
				shieldsAlpha = 120;
			}
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
				Width = (int)Width,
				Height = (int)Height
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
				UObject bolt = Weapon.CreateBolt(Position, ref PlayerStats);
				bolt.Collider.CollisionEvent += OnBoltHit;
				SceneManager.Instance.Scene.Add(bolt);
			}

			PlayerStats.Update(deltaTime);
		}

		private void OnBoltHit(object sender, EventArgs e)
		{
			if(e is CollisionEventArgs)
			{
				CollisionEventArgs ce = (CollisionEventArgs)e;
				if(ce.Source.Type == ObjectType.ENEMY)
				{
					PlayerStats.Score += 50;
					ShowPoints points = new ShowPoints(50);
					points.Position.X = ce.Who.Position.X;
					points.Position.Y = ce.Who.Position.Y;
					SceneManager.Instance.Scene.Add(points, 6);
					if (ce.Who is ITakeDamage && ((ITakeDamage)ce.Who).Life <= 0f)
					{
						PlayerStats.WeaponEnergy += 5.0f;
					}
				}
			}
		}

		public float TakeDamage(float damage)
		{
			PlayerStats.ShieldEnergy -= damage;
			PlayerStats.Score -= (int)(damage * 108);
			return PlayerStats.TotalLife;
		}
	}
}
