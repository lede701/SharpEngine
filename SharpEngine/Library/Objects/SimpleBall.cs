using SharpEngine.Library.Controller;
using SharpEngine.Library.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Objects
{
	public class SimpleBall : GObject
	{
		private String _key;
		public String Key
		{
			get
			{
				return _key;
			}
		}

		private int _radius;
		private Vector2D _position;
		private Vector2D _velocity;
		private Rectangle _boundary;
		private bool _isResting;

		public SimpleBall(int radius, Vector2D position, Rectangle boundary)
		{
			_radius = radius;
			_key = Guid.NewGuid().ToString();
			_position = position;
			_boundary = boundary;
			_velocity = new Vector2D { X = 4.0f, Y = 4.0f };
			_isResting = false;
		}

		public void Render(Graphics g)
		{
			g.FillEllipse(Brushes.Red, _position.X, _position.Y, _radius, _radius);
		}

		public void Update(float deltaTime)
		{
			if(!_isResting)
			{
				_position.X += _velocity.X * deltaTime * KeyboardController.Instance.GetValue(Input.Right);
				_position.X -= _velocity.X * deltaTime * KeyboardController.Instance.GetValue(Input.Left);
				_position.Y += _velocity.Y * deltaTime * KeyboardController.Instance.GetValue(Input.Down);
				_position.Y -= _velocity.Y * deltaTime * KeyboardController.Instance.GetValue(Input.Up);

				if (_position.X < _boundary.X || _position.X > _boundary.Width)
				{
					_position.X = _position.X < _boundary.X ? _boundary.X : _boundary.Width;
				}
				if (_position.Y < _boundary.Y || _position.Y > _boundary.Height)
				{
					_position.Y = _position.Y < _boundary.Y ? _boundary.Y : _boundary.Height;
					
				}
			}
		}
	}
}
