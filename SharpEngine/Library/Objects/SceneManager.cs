using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Objects
{
	public class SceneManager
	{
		private static SceneManager _instance;
		public static SceneManager Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new SceneManager();
				}

				return _instance;
			}
		}

		private Dictionary<int, Dictionary<String, GObject>> _scene;

		public SceneManager(int maxLayers = 10)
		{
			// Create new scene
			_scene = new Dictionary<int, Dictionary<string, GObject>>();
			// Create scene layers
			for(int i=0; i<maxLayers;++i)
			{
				_scene[i] = new Dictionary<string, GObject>();
			}
		}

		public void Add(GObject obj)
		{
			Add(obj, 0);
		}

		public void Add(GObject obj, int layer)
		{
			if(_scene.ContainsKey(layer))
			{
				_scene[layer][obj.Key] = obj;
			}
		}

		public void Update(float deltaTime)
		{
			foreach(Dictionary<String, GObject> layer in _scene.Values)
			{
				foreach(GObject obj in layer.Values)
				{
					obj.Update(deltaTime);
				}
			}
		}

		public void Render(Graphics g)
		{
			foreach (Dictionary<String, GObject> layer in _scene.Values)
			{
				foreach (GObject obj in layer.Values)
				{
					obj.Render(g);
				}
			}

		}
	}
}
