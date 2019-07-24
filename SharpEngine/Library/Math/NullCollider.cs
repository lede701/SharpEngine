using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpEngine.Library.Objects;

namespace SharpEngine.Library.Math
{
	public class NullCollider : ICollider
	{
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
