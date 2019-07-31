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
		public Vector2D Center;

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
		public void CallCollisionEvent(Object obj, CollisionEventArgs e)
		{
			CollisionEvent?.Invoke(obj, e);
		}

		public CircleCollider()
		{
			Center = new Vector2D { X = 0, Y = 0 };
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
						bRetVal = HitPlane((PlaneCollider)other);
						break;
				}
			}

			return bRetVal;
		}

		public bool Hit(UObject obj)
		{
			bool bRetVal = Hit(obj.Collider);
			if(bRetVal)
			{
				CollisionEventArgs me = new CollisionEventArgs
				{
					Who = obj,
					Point = _hitPoint,
					Location = _hitLocation,
					Distance = _circleDistance
				};
				CollisionEventArgs you = new CollisionEventArgs
				{
					Who = this.Owner,
					Point = _hitPoint,
					Location = _hitLocation,
					Distance = _circleDistance
				};
				CallCollisionEvent(obj, me);
				obj.Collider.CallCollisionEvent(this.Owner, you);
			}

			return bRetVal;
		}

		// Collision object to return when collision occure
		private Vector2D _hitPoint;
		private CollisionEventArgs.HitLocation _hitLocation;

		public bool HitPlane(PlaneCollider other)
		{
			bool bRetVal = false;

			switch(other.Type)
			{
				case Collider2DType.PlaneX:
					{
						// Check where in the world space the plane is locatated
						if(World.Instance.CenterOfWorld.X < other.Tupal)
						{
							if (Position.X + (Radius * 2) > other.Tupal)
							{
								_hitPoint = new Vector2D
								{
									X = other.Tupal,
									Y = Position.Y
								};
								bRetVal = true;
								// Tell collider if it was on the left or right the collision occured
								_hitLocation = CollisionEventArgs.HitLocation.Right;
							}

						}
						else
						{
							if (Position.X < other.Tupal)
							{
								_hitPoint = new Vector2D
								{
									X = other.Tupal,
									Y = Position.Y
								};
								bRetVal = true;
								// Tell collider if it was on the left or right the collision occured
								_hitLocation = CollisionEventArgs.HitLocation.Left;
							}

						}
					}
					break;
				case Collider2DType.PlaneY:
					{
						// This is actually using Diameter and should be changed
						if(Position.Y + (Radius * 2) > other.Tupal || Position.Y < other.Tupal)
						{
							_hitPoint = new Vector2D
							{
								X = Position.X,
								Y = other.Tupal
							};
							bRetVal = true;
							// Tell collider if it was on the top or bottom the collision occured
							_hitLocation = (Position.Y + (Radius * 2) > other.Tupal) ? CollisionEventArgs.HitLocation.Bottom : CollisionEventArgs.HitLocation.Top;
						}
					}
					break;
			}

			return bRetVal;
		}

		private float _circleDistance;

		private bool HitCircle(CircleCollider other)
		{
			float x0 = Position.X + Center.X;
			float y0 = Position.Y + Center.Y;
			float x1 = other.Position.X + other.Center.X;
			float y1 = other.Position.Y + other.Center.Y;

			float doubleRadius = Square(Radius + other.Radius);
			float doubleDistance = Square(x0 - x1) + Square(y0 - y1);

			_circleDistance = doubleDistance;
			return doubleDistance <= doubleRadius;
		}

		private float Square(float v)
		{
			return v * v;
		}
	}
}
