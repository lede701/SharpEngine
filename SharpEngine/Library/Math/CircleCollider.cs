using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Math
{
	public class CircleCollider : ICollider
	{
		public float Radius;
		public Vector2D Position;

		public Collider2DType Type
		{
			get
			{
				return Collider2DType.Circle;
			}
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
					break;
				case Collider2DType.PlaneX:
				case Collider2DType.PlaneY:
					HitPlane((PlaneCollider)other);
					break;
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
						bRetVal = Position.Y + Radius > other.Tupal;
					}
					break;
			}

			return bRetVal;
		}
	}
}
