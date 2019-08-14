using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Objects
{
	public class Layer : GObject
	{
		private Dictionary<String, GObject> _layerObjects;
		public Dictionary<String, GObject> LayerObjects
		{
			get
			{
				return _layerObjects;
			}
		}

		public bool AlwaysRender
		{
			get
			{
				return true;
			}
			set
			{

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

		private String _key;
		public String Key
		{
			get
			{
				return _key;
			}
		}

		public Layer()
		{
			// Create unique layer id
			_key = Guid.NewGuid().ToString();
			// Create new layer objects
			Clear();
			_drawObjects = new List<GObject>();
		}

		public void Clear()
		{
			// Clear the scene by creating a new layer
			_layerObjects = new Dictionary<String, GObject>();
		}

		private List<GObject> _drawObjects;

		public void Render(IGraphics g)
		{
			// Create a list of objects within the current world position
			Vector2D start = World.Instance.WorldPosition - (World.Instance.ScreenSize * 2f);
			Vector2D end = World.Instance.WorldPosition + World.Instance.ScreenSize;
			List<GObject> drawObjects = new List<GObject>();

			// Call the render item for each game object
			lock (SceneManager.Instance.ObjectLock)
			{
				foreach (GObject obj in _layerObjects.Values)
				{
					if(obj.Position.X > start.X && obj.Position.X < end.X && obj.Position.Y > start.Y && obj.Position.Y < end.Y || obj.AlwaysRender)
					{
						drawObjects.Add(obj);
					}
				}
			}
			foreach (GObject obj in drawObjects)
			{
				obj.Render(g);
			}
		}

		public void Update(float deltaTime)
		{
			List<GObject> layerObjects;
			lock (SceneManager.Instance.ObjectLock)
			{
				layerObjects = _layerObjects.Values.ToList<GObject>();
			}
			// Call update for all game objects
			foreach (GObject obj in layerObjects)
			{
				// Make sure there is valid objeft to update
				if(obj != null)
				{
					obj.Update(deltaTime);
				}
			}
		}

		public void Add(GObject obj)
		{
			_layerObjects[obj.Key] = obj;
		}

		public bool Remove(GObject obj)
		{
			bool bRetVal = false;
			if(_layerObjects.ContainsKey(obj.Key))
			{
				bRetVal = _layerObjects.Remove(obj.Key);
			}
			return bRetVal;
		}

		public void Dispose()
		{
			foreach(GObject obj in _layerObjects.Values)
			{
				obj.Dispose();
			}
		}
	}
}
