using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.GraphicsSystem
{
	public interface IGraphics
	{
		void BeginDraw();
		void EndDraw();
		void Render();
		void Clear(System.Drawing.Color color);
		void DrawEllipse(int x, int y, int width, int height, System.Drawing.Color clr);
		void DrawEllipse(System.Drawing.Rectangle rect, System.Drawing.Color clr);
		void DrawEllipse(System.Drawing.RectangleF rect, System.Drawing.Color clr);
		void DrawEllipse(float x, float y, float width, float height, System.Drawing.Color clr);
		void FillEllipse(System.Drawing.RectangleF rect, System.Drawing.Color clr);
		void FillEllipse(System.Drawing.Rectangle rect, System.Drawing.Color clr);
		void FillEllipse(int x, int y, int width, int height, System.Drawing.Color clr);
		void FillEllipse(float x, float y, float width, float height, System.Drawing.Color clr);
		void DrawLine(float x0, float y0, float x1, float y1, System.Drawing.Color clr);
		void DrawRectangle(System.Drawing.Rectangle rect, System.Drawing.Color clr);
		void DrawRectangle(float x, float y, float width, float height, System.Drawing.Color clr);
		void DrawRectangle(int x, int y, int width, int height, System.Drawing.Color clr);
		void FillRectangle(float x, float y, float width, float height, System.Drawing.Color clr);
		void FillRectangle(int x, int y, int width, int height, System.Drawing.Color clr);
		void DrawImage(Object image, System.Drawing.Rectangle srcRect, System.Drawing.Rectangle destRect);
		void DrawImage(Object image, System.Drawing.RectangleF srcRect, System.Drawing.RectangleF destRect);
		void DrawText(String message, String fontFamily, float fontSize, System.Drawing.Color clr, System.Drawing.Rectangle area);

		void Translate(float x, float y);
	}
}
