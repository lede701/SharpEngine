using SharpEngine.Library.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Box2D.Common
{
	public class b2Mat22
	{
		public Vector2D ex;
		public Vector2D ey;

		public b2Mat22()
		{
			ex = Vector2D.Zero;
			ey = Vector2D.Zero;
		}

		public b2Mat22(Vector2D c1, Vector2D c2)
		{
			ex = c1;
			ey = c2;
		}

		public b2Mat22(float a11, float a12, float a21, float a22)
		{
			ex = new Vector2D(a11, a21);
			ey = new Vector2D(a12, a22);
		}

		public void Set(Vector2D c1, Vector2D c2)
		{
			ex = c1;
			ey = c2;
		}

		public void SetIdentity()
		{
			ex.X = 1.0f; ex.Y = 0.0f;
			ey.X = 0.0f; ey.Y = 1.0f;
		}

		public void SetZero()
		{
			ex = Vector2D.Zero;
			ey = Vector2D.Zero;
		}

		public b2Mat22 GetInverse()
		{
			float a = ex.X;
			float b = ey.X;
			float c = ex.Y;
			float d = ey.Y;
			float det = a * d - b * c;
			b2Mat22 B = new b2Mat22();

			if(det != 0.0f)
			{
				det = 1.0f / det;
			}
			B.ex.X = det * d; B.ey.X = -det * b;
			B.ex.Y = -det * c; B.ey.Y = det * a;
			return B;
		}

		public Vector2D Solve(Vector2D b)
		{
			float a11 = ex.X;
			float a12 = ey.X;
			float a21 = ex.Y;
			float a22 = ey.Y;
			float det = a11 * a22 - a12 * a21;
			if(det != 0.0f)
			{
				det = 1.0f / det;
			}
			Vector2D x = Vector2D.Zero;
			x.X = det * (a22 * b.X - a12 * b.Y);
			x.Y = det * (a11 * b.Y - a21 * b.X);
			return x;
		}

		public static Vector2D Mul(b2Mat22 A, Vector2D v)
		{
			return new Vector2D(A.ex.X * v.X + A.ey.X * v.Y, A.ex.Y * v.X + A.ey.Y * v.Y);
		}
		public static Vector2D Mul(Rotation q, Vector2D v)
		{
			return new Vector2D(q.c * v.X - q.s * v.Y, q.s * v.X + q.c * v.Y);
		}
	}
}
