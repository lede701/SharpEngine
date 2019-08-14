using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Math;
using SharpEngine.Library.Threading;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Objects
{
	public class Scene : GObject
	{
		private Dictionary<int, Layer> _gameScene;
		private List<GObject> _gameObjects;

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

		public int DefaultLayer = 5;
		public Dictionary<int, Layer> GameScene
		{
			get
			{
				return _gameScene;
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

		public int Count
		{
			get
			{
				return _gameObjects.Count;
			}
		}

		public List<GObject> GameObjects
		{
			get
			{
				return _gameObjects;
			}
		}

		public List<UObject> UserObjects
		{
			get
			{
				List<UObject> list = new List<UObject>();
				lock (SceneManager.Instance.ObjectLock)
				{
					foreach (GObject obj in _gameObjects)
					{
						if (obj is UObject)
						{
							list.Add((UObject)obj);
						}
					}
				}

				return list;
			}
		}

		public Scene()
		{
			// Create unique key for scene
			_key = Guid.NewGuid().ToString();
			// Clear and create a new scene
			Clear();
		}

		public void Clear()
		{
			// Create new scene object
			_gameScene = new Dictionary<int, Layer>();
			_gameObjects = new List<GObject>();
			// Create 15 layers for the scene
			for(int i=0; i<15; ++i)
			{
				_gameScene[i] = new Layer();
			}
		}

		public void Render(IGraphics g)
		{
			foreach(Layer layer in _gameScene.Values)
			{
				// Render each object in layer
				layer.Render(g);
			}
		}

		public void Update(float deltaTime)
		{
			foreach (Layer layer in _gameScene.Values)
			{
				// Update each object in layer
				layer.Update(deltaTime);
			}
		}

		public void Add(GObject obj)
		{
			Add(obj, DefaultLayer);
		}

		public void Add(GObject obj, int layer)
		{
			lock (SceneManager.Instance.ObjectLock)
			{
				// Check if there is a valid layer
				if (_gameScene.ContainsKey(layer))
				{
					// Check if object type can store layer information
					if(obj is UObject)
					{
						((UObject)obj).Layer = layer;
					}
					_gameScene[layer].Add(obj);
					_gameObjects.Add(obj);
				}
				else
				{
					// Report invalid key for layer
				}
			}
		}

		public bool Remove(GObject obj)
		{
			// Try and remove object from the default layer (5)
			return Remove(obj, DefaultLayer);
		}

		public bool Remove(GObject obj, int layer)
		{
			bool bRetVal = false;
			// Check if layer exists in scene
			if(_gameScene.ContainsKey(layer))
			{
				lock (SceneManager.Instance.ObjectLock)
				{
					// Call the layer objects remove method
					bRetVal = _gameScene[layer].Remove(obj);
					_gameObjects.Remove(obj);
					obj.Dispose();
				}
			}

			return bRetVal;
		}

		public void Dispose()
		{
			foreach(Layer layer in _gameScene.Values)
			{
				layer.Dispose();
			}
		}
	}
}
