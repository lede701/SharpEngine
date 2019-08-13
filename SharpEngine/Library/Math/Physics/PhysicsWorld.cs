using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Math.Physics
{
	public class PhysicsWorld
	{
		private Dictionary<String, ICollider> _world;
		private Object _lock;

		public PhysicsWorld(Vector2D worldSize)
		{
			_world = new Dictionary<String, ICollider>();
			_lock = new Object();
		}

		public void Update(float deltaTime)
		{
			List<ICollider> colliders;
			lock(_lock)
			{
				colliders = new List<ICollider>(_world.Values);
			}
			// TODO: Revamp the physics data structure to improve performance of this loop
			for (int i = 0; i < colliders.Count; ++i)
			{
				if (i + 1 < colliders.Count)
				{
					for (int j = i + 1; j < colliders.Count; ++j)
					{
						 colliders[i].Hit(colliders[j]);
					}
				}
			}
		}

		public void Add(ICollider collider)
		{
			lock (_lock)
			{
				_world[collider.Key] = collider;
				collider.PWorld = this;
			}
		}
		
		public void Remove(ICollider collider)
		{
			Remove(collider.Key);
		}

		public void Remove(String key)
		{
			_world.Remove(key);
		}
	}
}
