using SharpEngine.Library.Events;
using SharpEngine.Library.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Math
{
	public class CircleCollider : ICollider
	{
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
		public float Radius;
		public Vector2D Position;

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
				return Collider2DType.Circle;
			}
		}

		public event EventHandler CollisionEvent;


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
					bRetVal = HitPlane((PlaneCollider)other);
					break;
			}

			return bRetVal;
		}

		public bool Hit(UObject obj)
		{
			bool bRetVal = Hit(obj.Collider);
			if(bRetVal)
			{
				CollisionEventArgs e = new CollisionEventArgs
				{
					Who = obj
				};
				CollisionEvent?.Invoke(this, e);
			}

			return bRetVal;
		}

		private bool HitPlane(PlaneCollider other)
		{
			bool bRetVal = false;

			switch(other.Type)
			{
				case Collider2DType.PlaneX:
					{

					}
					break;
				case Collider2DType.PlaneY:
					{
						// This is actually using Diameter and should be changed
						bRetVal = Position.Y + Radius > other.Tupal;
					}
					break;
			}

			return bRetVal;
		}

		private bool HitCircle(CircleCollider other)
		{
			float doubleRadius = Square(Radius + other.Radius);
			float doubleDistance = Square(System.Math.Abs(other.Position.X - Position.X)) + Square(System.Math.Abs(other.Position.Y - Position.Y));

			return doubleDistance <= doubleRadius;
		}

		private float Square(float v)
		{
			return v * v;
		}
	}
}
