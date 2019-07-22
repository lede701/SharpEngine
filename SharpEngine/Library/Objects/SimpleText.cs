using SharpEngine.Library.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Objects
{
	public class SimpleText : GObject
	{
		private String _key;
		public String Key
		{
			get
			{
				return _key;
			}
		}

		private String _text;
		public String Text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;
			}
		}

		private Transform _transform;
		public Transform Transform
		{
			get
			{
				return _transform;
			}
		}

		public Vector2D Position
		{
			get
			{
				return Transform.Position;
			}
		}

		private Font _font;
		public Font Font
		{
			get
			{
				return _font;
			}
			set
			{
				_font = value;
			}
		}

		public float Size
		{
			get
			{
				return Font.Size;
			}
			set
			{
				String fontName = "Arial";
				if(Font != null)
				{
					fontName = Font.Name;
					Font.Dispose();
				}
				Font = new Font(fontName, value);
			}
		}

		public SimpleText(String text)
		{
			_text = text;
			_transform = new Transform();
			_key = Guid.NewGuid().ToString();
			Font = new Font("Arial", 12);

		}

		public void Render(Graphics g)
		{
			Brush brush = Brushes.White;
			g.DrawString(Text, Font, brush, Position.X, Position.Y);
		}

		public void Update(float deltaTime)
		{
			
		}
	}
}
