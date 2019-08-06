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
			set
			{
				_position = value;
			}
		}

		private Vector2D _velocity;
		public Vector2D Velocity
		{
			get
			{
				return _velocity;
			}
			set
			{
				_velocity = value;
			}
		}

		private Vector2D _scale;
		public Vector2D Scale
		{
			get
			{
				return _scale;
			}
			set
			{
				_scale = value;
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
			_scale = new Vector2D { X = 1.0f, Y = 1.0f };
			_rotation = 0.0f;
		}

		public Transform(Vector2D position, Vector2D velocity)
		{
			_position = position;
			_velocity = velocity;
			_scale = new Vector2D { X = 1.0f, Y = 1.0f };
			_rotation = 0.0f;
		}
	}
}
