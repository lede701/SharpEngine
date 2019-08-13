using SharpEngine.Library.Events;
using SharpEngine.Library.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Math.Physics
{
	public class CircleCollider : ICollider
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
		public Vector2D Position { get; set; }
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
			_hitPoint = new List<Vector2D>();
		}


		public bool Hit(ICollider other)
		{
			bool bRetVal = false;
			_hitPoint.Clear();
			if (other != null)
			{
				switch (other.Type)
				{
					case Collider2DType.Box:
						//HitBox((BoxCollider)other);
						break;
					case Collider2DType.Circle:
						{
							bRetVal = HitCircle((CircleCollider)other);
							if (bRetVal)
							{
								CalcCircleHits((CircleCollider)other);
							}
						}
						break;
					case Collider2DType.PlaneX:
					case Collider2DType.PlaneY:
						bRetVal = HitPlane((PlaneCollider)other);
						break;
				}
			}
			if (bRetVal)
			{
				CollisionEventArgs ce = new CollisionEventArgs
				{
					Who = other.Owner,
					Source = this.Owner,
					Points = _hitPoint,
					Location = _hitLocation,
					Distance = _circleDistance
				};
				CallCollisionEvent(other.Owner, ce);
				other.CallCollisionEvent(this.Owner, ce);
			}

			return bRetVal;
		}

		public bool Hit(UObject obj)
		{
			return Hit(obj.Collider);
		}

		// Collision object to return when collision occure
		private List<Vector2D> _hitPoint;
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
								_hitPoint.Add(new Vector2D { X = other.Tupal, Y = Position.Y });
								bRetVal = true;
								// Tell collider if it was on the left or right the collision occured
								_hitLocation = CollisionEventArgs.HitLocation.Right;
							}

						}
						else
						{
							if (Position.X < other.Tupal)
							{
								_hitPoint.Add(new Vector2D { X = other.Tupal, Y = Position.Y });
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
							_hitPoint.Add(new Vector2D { X = Position.X, Y = other.Tupal });
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
			Vector2D diff = Position - other.Position;
			float x0 = Position.X + Center.X;
			float y0 = Position.Y + Center.Y;
			float x1 = other.Position.X + other.Center.X;
			float y1 = other.Position.Y + other.Center.Y;

			float doubleRadius = Square(Radius + other.Radius);
			float doubleDistance = Square(x0 - x1) + Square(y0 - y1);

			_circleDistance = doubleDistance;
			return doubleDistance <= doubleRadius;
		}

		private void CalcCircleHits(CircleCollider other)
		{
			// Set small value for validating collider values
			float EPS = 0.0000001f;
			float r, R, d, dx, dy, cx, cy, Cx, Cy;
			if(Radius < other.Radius)
			{
				r = Radius;
				R = other.Radius;
				cx = Position.X;
				cy = Position.Y;
				Cx = other.Position.X;
				Cy = other.Position.Y;
			}else
			{
				R = Radius;
				r = other.Radius;
				Cx = Position.X;
				Cy = Position.Y;
				cx = other.Position.X;
				cy = other.Position.Y;
			}

			dx = cx - Cx;
			dy = cy - Cy;

			d = (float)System.Math.Sqrt(Square(dx) + Square(dy));
			// Check for no intersections and circles are outside
			if (d < EPS && System.Math.Abs(R - r) < EPS) return;
			else if (d < EPS) return;

			Vector2D p = new Vector2D { X = (dx / d) * R + Cx, Y = (dy / d) * R + Cy };
			// Check if colliders are touching on one point
			if (System.Math.Abs((R + r) - d) < EPS || System.Math.Abs(R - (r + d)) < EPS)
			{
				_hitPoint.Add(p);
				return;
			}
			// Check if collider is within collider
			if ((d + r) < R || R + r < d) return;

			Vector2D C = new Vector2D { X = Cx, Y = Cy };
			float angle = (float) SafeACOS(Square(r) - Square(d) - Square(R) / (-2.0 * d * R));
			_hitPoint.Add(C.RotateVector(p, +angle));
			_hitPoint.Add(C.RotateVector(p, -angle));
		}

		public float SafeACOS(double val)
		{
			float v = (float)val;
			if (v >= 1.0f) return 0f;
			if (v <= -1.0f) return (float)System.Math.PI;
			return (float)System.Math.Acos(val);
		}

		private float Square(float v)
		{
			return v * v;
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
