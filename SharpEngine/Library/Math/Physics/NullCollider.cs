using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpEngine.Library.Events;
using SharpEngine.Library.Objects;

namespace SharpEngine.Library.Math.Physics
{
	public class NullCollider : ICollider
	{
		private String _key = Guid.NewGuid().ToString();
		public String Key
		{
			get
			{
				return _key;
			}
		}
		public Vector2D Position { get; set; }
		public bool IsActive{
			get
			{
				return false;
			}
			set
			{
			}
		}
		private UObject _owner;
		public UObject Owner
		{
			get
			{
				return _owner;
			}
			set
			{
				_owner = value;
			}
		}

		public Collider2DType Type
		{
			get
			{
				return Collider2DType.None;
			}
		}

		public event EventHandler CollisionEvent;
		public void CallCollisionEvent(Object obj, CollisionEventArgs e)
		{
			CollisionEvent?.Invoke(obj, e);
		}

		public bool Hit(ICollider other)
		{
			return false;
		}

		public bool Hit(UObject obj)
		{
			return false;
		}
	}
}
