using SharpEngine.Library.Events;
using SharpEngine.Library.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Math
{
	public class PlaneCollider : ICollider
	{
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

		public event EventHandler CollisionEvent;

		private Collider2DType _type;
		public Collider2DType Type
		{
			get
			{
				return _type;
			}
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
					break;
				case Collider2DType.PlaneY:
					bRetVal = other.Position.Y + other.Radius > _tupal;
					break;
			}
			return bRetVal;
		}
	}
}
