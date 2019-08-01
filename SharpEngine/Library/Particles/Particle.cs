using SharpEngine.Library.Math;
using SharpEngine.Library.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Particles
{
	public class Particle
	{
		public static readonly int MAX_LIFE = 10000;

		private Vector2D _position;
		private Vector2D _velocity;

		private int _life;
		private System.Drawing.Color _color;

		public float R { get; set; }

		#region Ctor's
		public Particle() : this(Vector2D.Zero, Vector2D.Zero, System.Drawing.Color.White)
		{

		}

		public Particle(Vector2D pos, Vector2D vel, System.Drawing.Color clr)
		{
			_position = pos;
			_velocity = vel;
			_color = clr;
			_life = 0;
			R = 1.0f;
		}

		#endregion

		public bool Update(float deltaTime)
		{
			_velocity.X -= (World.Instance.Gravity.X + World.Instance.Wind.X) * deltaTime;
			_velocity.Y -= (World.Instance.Gravity.Y + World.Instance.Wind.Y) * deltaTime;

			_position.X += _velocity.X * deltaTime;
			_position.Y += _velocity.Y * deltaTime;

			bool agedOut = _life > MAX_LIFE;
			return agedOut;
		}

		public Vector2D Position
		{
			get
			{
				return _position;
			}
		}

		public Vector2D Velocity
		{
			get
			{
				return _velocity;
			}
		}

		public int Life
		{
			get
			{
				return _life;
			}
		}

		public void LifePlus(int val)
		{
			_life += val;
		}

		public System.Drawing.Color Color
		{
			get
			{
				return _color;
			}
		}

	}
}
