using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Math;
using SharpEngine.Library.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.User.Universe
{
	public class UniTile : GObject
	{
		public System.Drawing.Rectangle Range;
		public List<IUniverseItem> Items;

		private String _key = Guid.NewGuid().ToString();
		public string Key
		{
			get
			{
				return _key;
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

		private Vector2D _position = Vector2D.Zero;
		public Vector2D Position
		{
			get
			{
				return _position;
			}
			set
			{
				_position = value;
			}
		}

		public UniTile()
		{
			Items = new List<IUniverseItem>();
		}

		public void Add(IUniverseItem item)
		{
			Items.Add(item);
		}

		public void Dispose()
		{
		}

		public void Render(IGraphics g)
		{
			foreach(IUniverseItem item in Items)
			{
				item.Render(g);
			}
		}

		public void Update(float deltaTime)
		{
			foreach (IUniverseItem item in Items)
			{
				item.Update(deltaTime);
			}
		}
	}
}
