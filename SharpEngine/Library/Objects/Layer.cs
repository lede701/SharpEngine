using SharpEngine.Library.GraphicsSystem;
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
		}

		public void Clear()
		{
			// Clear the scene by creating a new layer
			_layerObjects = new Dictionary<String, GObject>();
		}

		public void Render(IGraphics g)
		{
			// Call the render item for each game object
			lock (SceneManager.Instance.ObjectLock)
			{
				foreach (GObject obj in _layerObjects.Values)
				{
					obj.Render(g);
				}
			}
		}

		public void Update(float deltaTime)
		{
			List<GObject> layerObjects = _layerObjects.Values.ToList<GObject>();
			// Call update for all game objects
			foreach (GObject obj in layerObjects)
			{
				obj.Update(deltaTime);
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
