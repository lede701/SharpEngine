using SharpEngine.Library.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Box2D.Collision.Shapes
{
	public enum ShapeType
	{
		E_CIRCLE = 0,
		E_EDGE = 1,
		E_POLYGON = 2,
		E_CHAIN = 3,
		E_TYPECOUNT = 4,
		E_UNDEFINED = 5
	}

	public interface b2IShape
	{
		ShapeType Type { get; }
		b2IShape Clone();
		int ChildCount { get; }
		bool TestPoint(Transform xf, Vector2D p);
		bool RayCast(out b2RayCastOutput castOutput, b2RayCastInput input, Transform transform, int childIndex);
		b2AABB ComputeAABB(b2AABB aabb, Transform xf, int childIndex);
		void ComputeMass(b2MassData massData, float density);
		float Radius { get; set; }

	}
}
