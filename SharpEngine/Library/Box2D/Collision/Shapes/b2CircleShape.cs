using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpEngine.Library.Box2D.Common;
using SharpEngine.Library.Math;

namespace SharpEngine.Library.Box2D.Collision.Shapes
{
	class b2CircleShape : b2IShape
	{

		public Vector2D m_p;

		public b2CircleShape()
		{
			m_p = Vector2D.Zero;
		}

		public ShapeType Type
		{
			get
			{
				return ShapeType.E_CIRCLE;
			}
		}

		public int ChildCount
		{
			get
			{
				return 1;
			}
		}

		private float _radius = 0.0f;
		public float Radius
		{
			get
			{
				return _radius;
			}
			set
			{
				_radius = value;
			}
		}

		public b2IShape Clone()
		{
			b2CircleShape clone = new b2CircleShape
			{
				Radius = Radius,
				m_p = new Vector2D(m_p)
			};

			return clone;
		}

		public b2AABB ComputeAABB(b2AABB aabb, Transform xf, int childIndex)
		{
			Vector2D p = xf.Position + b2Mat22.Mul(xf.Rotation, m_p);
			aabb.LowerBound = new Vector2D(p.X - Radius, p.Y - Radius);
			aabb.UpperBound = new Vector2D(p.X + Radius, p.Y + Radius);
			return aabb;
		}

		public void ComputeMass(b2MassData massData, float density)
		{
			massData.Mass = density * (float)System.Math.PI * Radius * Radius;
			massData.Center = m_p;

			// Inertia about the local origin
			massData.I = massData.Mass * (0.5f * Radius * Radius + Vector2D.Dot(m_p, m_p));
		}

		public bool RayCast(out b2RayCastOutput castOutput, b2RayCastInput input, Transform transform, int childIndex)
		{
			Vector2D position = transform.Position + b2Mat22.Mul(transform.Rotation, m_p);
			Vector2D s = input.p1 - position;
			float b = Vector2D.Dot(s, s) - Radius * Radius;

			castOutput.fraction = 0.0f;
			castOutput.Normal = null;

			// Solve quadratic equation
			Vector2D r = input.p2 - input.p1;
			float c = Vector2D.Dot(s, r);
			float rr = Vector2D.Dot(r, r);
			float sigma = c * c - rr * b;
			if(sigma < 0.0f || rr < float.Epsilon)
			{
				return false;
			}

			float a = -(float)(c + System.Math.Sqrt(sigma));
			if(0.0f <= a && a<= input.maxFraction * rr)
			{
				a /= rr;
				castOutput.fraction = a;
				castOutput.Normal = s + r * a;
				castOutput.Normal = castOutput.Normal.Normal();
				return true;
			}

			return false;
		}

		public bool TestPoint(Transform xf, Vector2D p)
		{
			Vector2D center = xf.Position + b2Mat22.Mul(xf.Rotation, m_p);
			Vector2D d = p - center;

			return Vector2D.Dot(d, d) <= Radius * Radius;
		}
	}
}
