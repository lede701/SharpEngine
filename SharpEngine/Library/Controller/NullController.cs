using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Controller
{
	public class NullController : IController
	{
		public void Dispose()
		{
			// Nothing to dispose so we are good
		}

		public bool Get(Input type)
		{
			return false;
		}

		public float GetValue(Input type)
		{
			return 0.0f;
		}
	}
}
