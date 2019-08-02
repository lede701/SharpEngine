using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Math
{
	public class Vector2D
	{
		public float X;
		public float Y;
		public static float Lerp(float v1, float v2, float t)
		{
			return (1.0f - t) * v1 + t * v1;
		}

		public Vector2D()
		{

		}
		public Vector2D(float x, float y)
		{
			X = x;
			Y = y;
		}

		public Vector2D(Vector2D pos)
		{
			X = pos.X;
			Y = pos.Y;
		}

		public static Vector2D Zero
		{
			get
			{
				return new Vector2D { X = 0, Y = 0 };
			}
		}

		public float Distance(Vector2D p1)
		{
			return (float)System.Math.Sqrt(Square(X - p1.X) + Square(Y - p1.Y));
		}

		public Vector2D Subtract(Vector2D p1)
		{
			return new Vector2D { X = X - p1.X, Y = Y - p1.Y };
		}

		public Vector2D Add(Vector2D p1)
		{
			return new Vector2D { X = X + p1.X, Y = Y + p1.Y };
		}

		public Vector2D Normal()
		{
			float length = (float)System.Math.Sqrt(Square(X) + Square(Y));
			return new Vector2D { X = X / length, Y = Y / length };
		}

		public Vector2D Scale(float s)
		{
			return new Vector2D { X = X * s, Y = Y * s };
		}

		public Vector2D MidPoint(Vector2D p1)
		{
			return new Vector2D { X = (X - p1.X) / 2, Y = (Y - p1.Y) / 2 };
		}

		public Vector2D RotateVector(Vector2D fp, Vector2D pt, float angle)
		{
			float x = pt.X - fp.X;
			float y = pt.Y - fp.Y;
			float xRot = (float)(x * System.Math.Cos(angle) + y * System.Math.Sin(angle));
			float yRot = (float)(y * System.Math.Cos(angle) - x * System.Math.Sin(angle));
			return new Vector2D { X = fp.X + xRot, Y = fp.Y + yRot };
		}

		public float Square(float val)
		{
			return val * val;
		}
	}
}
