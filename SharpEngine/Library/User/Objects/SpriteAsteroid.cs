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
			if(ce.Who.Type == ObjectType.PLAYER)
			{
				if(((ITakeDamage)ce.Who).TakeDamage(Life) <= 0f)
				{
					// Game over event
					SceneManager.Instance.Scene.Remove(ce.Who, ce.Who.Layer);
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

		public override void Render(IGraphics g)
		{
			base.Render(g);
			System.Drawing.Rectangle rect = Sprite.Frame;
			float lifeLevel = _life / _maxLife;
			float lWidth = rect.Width * lifeLevel;

			g.FillRectangle(Position.X, Position.Y, lWidth, 5, System.Drawing.Color.FromArgb(100, 192, 250, 0));
			g.DrawRectangle(Position.X, Position.Y, rect.Width, 5, System.Drawing.Color.FromArgb(180, 98, 128, 0));
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
