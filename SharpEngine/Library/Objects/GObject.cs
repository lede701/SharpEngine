using SharpEngine.Library.GraphicsSystem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Objects
{
	public interface GObject
	{
		String Key { get; }

		void Render(IGraphics g);
		void Update(float deltaTime);
		void Dispose();
	}
}
