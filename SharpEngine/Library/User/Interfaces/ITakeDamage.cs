using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.User.Interfaces
{
	public interface ITakeDamage
	{
		float TakeDamage(float damage);
		float Life { get; }
	}
}
