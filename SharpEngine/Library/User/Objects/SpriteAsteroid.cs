using SharpEngine.Library.Events;
using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Math;
using SharpEngine.Library.Objects;
using SharpEngine.Library.Randomizer;
using SharpEngine.Library.User.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.User.Objects
{
	public class SpriteAsteroid : USpriteObject, ITakeDamage
	{
		public event EventHandler EventDestroyed;
		private float _life;
		private float _maxLife;
		public float Life
		{
			get
			{
				return _life;
			}
			set
			{
				_life = value;
				_maxLife = value;
			}
		}
		public SpriteAsteroid(Sprite sprite) : base(sprite, false)
		{
			sprite.Frames.Add(new System.Drawing.Rectangle
			{
				X = 0,
				Y = 0,
				Width = 125,
				Height = 125
			});
			CircleCollider cldr = new CircleCollider();

			cldr.Radius = 40;
			cldr.Position = Position;
			cldr.Center.X = 125 / 2;
			cldr.Center.Y = 125 / 2;
			Collider = cldr;
			Type = ObjectType.ENEMY;
			Life = 2.0f;
			_speedEffect = new Vector2D { X = 0f, Y = 0f };
		}

		public float TakeDamage(float damage)
		{
			_life -= damage;
			if(Life > 0f)
			{
				SpeedEffect.Y += -3f;
			}
			return Life;
		}

		public override void OnCollision(object sender, EventArgs e)
		{
			// Set to figure out what hit this object
			CollisionEventArgs ce = (CollisionEventArgs)e;
			if(ce.Source.Type == ObjectType.PLAYER)
			{
				if(((ITakeDamage)ce.Source).TakeDamage(Life) <= 0f)
				{
					// Game over event
					SceneManager.Instance.Scene.Remove(ce.Source, ce.Source.Layer);
				}
				SceneManager.Instance.Scene.Remove(this, Layer);
				EventDestroyed?.Invoke(this, e);

				Life = 0f;
			}
			if (Life <= 0) 
			{
				EventDestroyed?.Invoke(this, e);
			}
		}

		private System.Drawing.Color _newColor = System.Drawing.Color.FromArgb(255, 192, 250, 0);
		private System.Drawing.Color _dangerColor = System.Drawing.Color.FromArgb(255, 255, 0, 0);

		public override void Render(IGraphics g)
		{
			float lifeLevel = _life / _maxLife;

			base.Render(g);
			System.Drawing.Rectangle rect = Sprite.Frame;
			float lWidth = (rect.Width / 2f) * lifeLevel;
			float x = Position.X + (rect.Width / 4f);

			g.FillRectangle(x, Position.Y, lWidth, 5, BlendColor(_newColor, _dangerColor, lifeLevel, 0.4f));
			g.DrawRectangle(x, Position.Y, (rect.Width / 2f), 5, BlendColor(_newColor, _dangerColor, lifeLevel, 0.55f));
		}

		private System.Drawing.Color BlendColor(System.Drawing.Color clr1, System.Drawing.Color clr2, float blend, float alpha)
		{
			int r, g, b, a;
			float baseBlend = 1.0f - blend;
			r = (int)((clr2.R * baseBlend) + (clr1.R * blend));
			g = (int)((clr2.G * baseBlend) + (clr1.G * blend));
			b = (int)((clr2.B * baseBlend) + (clr1.B * blend));
			a = (int)(255f * alpha);
			return System.Drawing.Color.FromArgb(a, r, g, b);
		}

		private Vector2D _speedEffect;
		public Vector2D SpeedEffect
		{
			get
			{
				return _speedEffect;
			}
		}

		public override void Update(float deltaTime)
		{
			Position.X += (Velocity.X + SpeedEffect.X) * deltaTime;
			Position.Y += (Velocity.Y + SpeedEffect.Y) * deltaTime;

			// Reduce the amount of speed effect
			SpeedEffect.Y = System.Math.Min(SpeedEffect.Y + 0.1f, 0f);

			if(Position.Y > World.Instance.WorldSize.Y + 100)
			{
				Position.Y = -200;
				Position.X = RandomManager.Instance.Next(100, (int)World.Instance.WorldSize.X - 100);
			}
		}
	}

}
