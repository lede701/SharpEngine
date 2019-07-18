using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpEngine.Library.Controller;
using SharpEngine.Library.Math;

namespace SharpEngine.Library.Objects
{
	public class USpriteObject : UObject
	{
		private Bitmap _spriteSheet;
		public Bitmap SpriteSheet
		{
			get
			{
				return _spriteSheet;
			}
			set
			{
				_spriteSheet = value;
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

		public USpriteObject()
		{
			_transform = new Transform();
			_key = Guid.NewGuid().ToString();
		}

		public void Render(Graphics g)
		{
			
		}

		public void Update(float deltaTime)
		{
			
		}
	}
}
