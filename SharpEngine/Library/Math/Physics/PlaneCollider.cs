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
	public class PlaneCollider : ICollider
	{
		#region ICollider parameters

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

		public Vector2D Position { get; set; }

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
		private float _tupal;
		public float Tupal
		{
			get
			{
				return _tupal;
			}
		}

		private Collider2DType _type;
		public Collider2DType Type
		{
			get
			{
				return _type;
			}
		}

		public TRectangle NodeRectangle
		{
			get
			{
				// TODO: Figure out how to manage this colliders rectangle based on the plane
				TRectangle rect = new TRectangle
				{
					LeftTop = new Vector2D(Position.X, Position.Y),
					RightBottom = new Vector2D(Position.X, Position.Y)
				};

				return rect;
			}
		}

		#endregion

		public event EventHandler CollisionEvent;
		public void CallCollisionEvent(Object obj, CollisionEventArgs e)
		{
			CollisionEvent?.Invoke(obj, e);
		}

		public PlaneCollider()
		{
			SetType(Collider2DType.PlaneY);
		}

		public PlaneCollider(float tupal)
		{
			SetType(Collider2DType.PlaneY);
			_tupal = tupal;
		}
		public PlaneCollider(Collider2DType type, float tupal)
		{
			SetType(type);
			_tupal = tupal;
		}

		public void SetType(Collider2DType type)
		{
			if(type == Collider2DType.PlaneX || type == Collider2DType.PlaneY)
			{
				_type = type;
			}
		}

		public void SetPlane(float tupal)
		{
			_tupal = tupal;
		}

		public bool Hit(ICollider other)
		{
			bool bRetVal = false;
			if (other != null)
			{
				switch (other.Type)
				{
					case Collider2DType.Box:
						//HitBox((BoxCollider)other);
						break;
					case Collider2DType.Circle:
						bRetVal = HitCircle((CircleCollider)other);
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
			bool bRetVal = Hit(obj.Collider);
			if (bRetVal)
			{
				CollisionEventArgs e = new CollisionEventArgs
				{
					Who = obj
				};
				CollisionEvent?.Invoke(this, e);
				obj.Collider.Hit(Owner);
			}
			return bRetVal;
		}

		private bool HitCircle(CircleCollider other)
		{
			bool bRetVal = false;
			switch(Type)
			{
				case Collider2DType.PlaneX:
					bRetVal = other.Position.X < _tupal || other.Position.X + other.Radius > _tupal;
					break;
				case Collider2DType.PlaneY:
					bRetVal = other.Position.Y + (other.Radius * 2) > _tupal;
					break;
			}
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
