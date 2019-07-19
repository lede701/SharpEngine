using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpEngine.Library.Controller;
using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Math;

namespace SharpEngine.Library.Objects
{
	public class USpriteObject : UObject
	{
		private Sprite _sprite;
		public Sprite Sprite
		{
			get
			{
				return _sprite;
			}
			set
			{
				_sprite = value;
			}
		}
		private IController _controller;
		public IController Controller
		{
			get
			{
				return _controller;
			}
			set
			{
				value = _controller;
			}
		}

		private ICollider _collider;
		public ICollider Collider
		{
			get
			{
				return _collider;
			}
			set
			{
				_collider = value;
			}
		}

		private Transform _transform;
		public Transform Transform
		{
			get
			{
				return _transform;
			}
		}

		public Vector2D Position
		{
			get
			{
				return Transform.Position;
			}
		}

		public Vector2D Velocity
		{
			get
			{
				return Transform.Velocity;
			}
		}

		public float Rotation
		{
			get
			{
				return Transform.Rotation;
			}
		}

		private String _key;
		public string Key
		{
			get
			{
				return _key;
			}
		}

		public USpriteObject(Sprite sprite)
		{
			_transform = new Transform();
			_key = Guid.NewGuid().ToString();
			Sprite = sprite;
			
		}

		public void Render(Graphics g)
		{
			g.TranslateTransform(Position.X, Position.Y);
			Rectangle src = Sprite.Frame;
			Rectangle dest = new Rectangle
			{
				X = src.X,
				Y = src.Y,
				Width = (int)((float)src.Width * 0.25f),
				Height = (int)((float)src.Height * 0.25f)
			};
			g.DrawImage(Sprite.SpriteSheet, dest, src, GraphicsUnit.Pixel );
			g.TranslateTransform(-Position.X, -Position.Y);
		}

		public void Update(float deltaTime)
		{
			if (Controller != null)
			{
				Position.X = (Velocity.X * deltaTime * Controller.GetValue(Input.Right)) - (Velocity.X * deltaTime * Controller.GetValue(Input.Left));
				Position.Y = (Velocity.Y * deltaTime * Controller.GetValue(Input.Down)) - (Velocity.Y * deltaTime * Controller.GetValue(Input.Up));
			}

		}
	}
}
