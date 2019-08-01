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
	}
}
