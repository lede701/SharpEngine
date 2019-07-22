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
					Who = obj,
					Point = _hitPoint,
					Location = _hitLocation
				};
				CollisionEvent?.Invoke(this, e);
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
