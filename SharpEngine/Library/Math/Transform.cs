using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Math
{
	public class Transform
	{
		private Vector2D _position;
		public Vector2D Position
		{
			get
			{
				return _position;
			}
		}

		private Vector2D _velocity;
		public Vector2D Velocity
		{
			get
			{
				return _velocity;
			}
		}

		private float _rotation;
		public float Rotation
		{
			get
			{
				return _rotation;
			}
			set
			{
				_rotation = value;
			}
		}

		public Transform()
		{
			_position = new Vector2D { X = 0, Y = 0 };
			_velocity = new Vector2D { X = 0, Y = 0 };
			_rotation = 0.0f;
		}

		public Transform(Vector2D position, Vector2D velocity)
		{
			_position = position;
			_velocity = velocity;
			_rotation = 0.0f;
		}
	}
}
