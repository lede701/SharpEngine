﻿using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Math;
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

		Vector2D Position { get; set; }

		bool AlwaysRender { get; set; }
		void Render(IGraphics g);
		void Update(float deltaTime);
		void Dispose();
	}
}
