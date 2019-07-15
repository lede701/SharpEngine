using SharpEngine.Library.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Objects
{
	public interface UObject : GObject
	{
		IController Controller { get; }
	}
}
