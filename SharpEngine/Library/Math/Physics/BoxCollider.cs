using SharpEngine.Library.Data.Trees;
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

		private PhysicsWorld _pworld;
		public PhysicsWorld PWorld
		{
			get
			{
				return _pworld;
			}
			set
			{
				_pworld = value;
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
				return Vector2D.Zero;
			}
		}

		public TRectangle NodeRectangle
		{
			get
			{
				// TODO: Need to update collider so there is a height and width
				TRectangle rect = new TRectangle
				{
					LeftTop = new Vector2D(Position.X, Position.Y),
					RightBottom = new Vector2D(20f, 20f)
				};
				return rect;
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

		public void Dispose()
		{
			if (PWorld != null)
			{
				PWorld.Remove(this);
			}
		}
	}
}
