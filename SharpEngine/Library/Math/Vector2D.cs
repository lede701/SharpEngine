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
	}
}
