using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Math.Physics
{
	public class PhysicsFactory
	{
		private PhysicsWorld _world;
		public PhysicsWorld World
		{
			get
			{
				return _world;
			}
		}
		public PhysicsFactory(PhysicsWorld world)
		{
			_world = world;
		}

		public ICollider CreateCircleCollider(Vector2D position, float radius)
		{
			CircleCollider collider = new CircleCollider();
			collider.Position = position;
			collider.Radius = radius;
			_world.Add(collider);

			return collider;
		}

		public ICollider CreateBoxCollider(System.Drawing.RectangleF rect)
		{
			BoxCollider collider = new BoxCollider();

			return collider;
		}
	}
}
