using SharpEngine.Library.Events;
using SharpEngine.Library.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Math.Physics
{
	public class BoxCollider : ICollider
	{
		private String _key = Guid.NewGuid().ToString();
		public String Key
		{
			get
			{
				return _key;
			}
		}

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

		private bool _isActive;
		public bool IsActive
		{
			get
			{
				return _isActive;
			}
			set
			{
				_isActive = value;
			}
		}
		private UObject _owner;
		public UObject Owner
		{
			get
			{
				return _owner;
			}
			set
			{
				_owner = value;
			}
		}

		public Collider2DType Type
		{
			get
			{
				return Collider2DType.Box;
			}
		}

		public Vector2D MinVec
		{
			get
			{
				Vector2D min = new Vector2D { X = 0, Y = 0 };
				return min;
			}
		}

		public event EventHandler CollisionEvent;
		public void CallCollisionEvent(Object obj, CollisionEventArgs e)
		{
			CollisionEvent?.Invoke(obj, e);
		}

		public bool Hit(ICollider other)
		{
			bool bRetVal = false;
			if (other != null)
			{
				switch (other.Type)
				{
					case Collider2DType.Box:
						HitBox((BoxCollider)other);
						break;
					case Collider2DType.Circle:
						break;
					case Collider2DType.PlaneX:
					case Collider2DType.PlaneY:

						break;
				}
			}

			return bRetVal;
		}

		public bool Hit(UObject obj)
		{
			return Hit(obj.Collider);
		}

		private bool HitBox(BoxCollider other)
		{
			bool bRetVal = false;
			// Find smallest x,y

			// Find larget x,y

			return bRetVal;
		}
	}
}
