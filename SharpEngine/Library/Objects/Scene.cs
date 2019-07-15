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
			// Create 15 layers for the scene
			for(int i=0; i<15; ++i)
			{
				_gameScene[i] = new Layer();
			}
		}

		public void Render(Graphics g)
		{
			foreach(Layer layer in _gameScene.Values)
			{
				layer.Render(g);
			}
		}

		public void Update(float deltaTime)
		{
			foreach (Layer layer in _gameScene.Values)
			{
				layer.Update(deltaTime);
			}
		}

		public void Add(GObject obj)
		{
			Add(obj, DefaultLayer);
		}

		public void Add(GObject obj, int layer)
		{
			// Check if there is a valid layer
			if (_gameScene.ContainsKey(layer))
			{
				_gameScene[layer].Add(obj);
			}else
			{
				// Report invalid key for layer
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
				// Call the layer objects remove method
				bRetVal = _gameScene[layer].Remove(obj);
			}

			return bRetVal;
		}
	}
}
