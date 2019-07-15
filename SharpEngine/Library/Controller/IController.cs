using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Controller
{
	public interface IController : IDisposable
	{
		bool Get(Input type);
		float GetValue(Input type);
	}
}
