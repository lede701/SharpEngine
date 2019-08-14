using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Math;
using SharpEngine.Library.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.User.Objects
{
	public class ShowPoints : GObject
	{
		private String _key = Guid.NewGuid().ToString();
		public string Key
		{
			get
			{
				return _key;
			}
		}

		public System.Drawing.Color PosColor = System.Drawing.Color.White;
		public System.Drawing.Color NegColor = System.Drawing.Color.Red;
		public System.Drawing.Color Color;

		private Transform _transform;

		public Vector2D Position
		{
			get
			{
				return _transform.Position;
			}
			set
			{
				_transform.Position = value;
			}
		}

		public Vector2D Velocity
		{
			get
			{
				return _transform.Velocity;
			}
		}

		private bool _alwaysRender = true;
		public bool AlwaysRender
		{
			get
			{
				return _alwaysRender;
			}
			set
			{
				_alwaysRender = value;
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

		public int Points
		{
			set
			{
				String fmt = "+{0}";
				Color = PosColor;
				if(value < 0)
				{
					fmt = "{0}";
					Color = NegColor;
				}
				Text = String.Format(fmt, value);
			}
		}

		private int _life = 0;
		public int Life
		{
			get
			{
				return _life;
			}
		}

		private int _maxLife = 300;
		public int MaxLife
		{
			get
			{
				return _maxLife;
			}
		}

		public ShowPoints()
		{
			_transform = new Transform();
			_transform.Velocity.Y = -0.5f;
		}

		public ShowPoints(int points) : this()
		{
			Points = points;
		}

		public void Dispose()
		{
		}

		public void Render(IGraphics g)
		{
			System.Drawing.Rectangle rect = new System.Drawing.Rectangle
			{
				X = (int)Position.X,
				Y = (int)Position.Y,
				Width = 100,
				Height = 50
			};
			g.DrawText(Text, "Ariel", 12, Color, rect);
		}

		public void Update(float deltaTime)
		{
			_transform.Position.X += _transform.Velocity.X * deltaTime;
			_transform.Position.Y += _transform.Velocity.Y * deltaTime;
			float alpha = System.Math.Max(1.0f - (float)_life++ / (float)_maxLife, 0f);
			int iAlpha = (int)(255f * alpha);
			Color = System.Drawing.Color.FromArgb(iAlpha, Color.R, Color.G, Color.B);
			if(Life > MaxLife)
			{
				SceneManager.Instance.Scene.Remove(this, 6);
			}
		}
	}
}
