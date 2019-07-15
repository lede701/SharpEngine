using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Math
{
	public class BoxCollider : ICollider
	{
		public Collider2DType Type
		{
			get
			{
				return Collider2DType.Box;
			}
		}

		public Vector2D MinVec
		{
			get
			{
				Vector2D min = new Vector2D { X = 0, Y = 0 };
				return min;
			}
		}

		public bool Hit(ICollider other)
		{
			bool bRetVal = false;
			switch(other.Type)
			{
				case Collider2DType.Box:
					HitBox((BoxCollider)other);
					break;
				case Collider2DType.Circle:
					break;
				case Collider2DType.PlaneX:
				case Collider2DType.PlaneY:

					break;
			}

			return bRetVal;
		}

		private bool HitBox(BoxCollider other)
		{
			bool bRetVal = false;
			// Find smallest x,y

			// Find larget x,y

			return bRetVal;
		}
	}
}
