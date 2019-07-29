using SharpEngine.Library.GraphicsSystem;
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

		private Stack<Scene> _scenes;

		public Scene Scene
		{
			get
			{
				Scene sc = null;
				if(_scenes.Count > 0)
				{
					sc = _scenes.Peek();
				}
				return sc;
			}
		}

		private Object _objectLock;
		public Object ObjectLock
		{
			get
			{
				if(_objectLock == null)
				{
					_objectLock = new Object();
				}

				return _objectLock;
			}
		}

		public SceneManager()
		{
			// Create new scene
			Clear();
		}

		public void Add(GObject obj)
		{
			Add(obj, 0);
		}

		public void Add(Scene scene)
		{
			_scenes.Push(scene);
		}

		public void Add(GObject obj, int layer)
		{
			if(_scenes.Count > 0)
			{
				lock (ObjectLock)
				{
					_scenes.Peek().Add(obj, layer);
				}
			}
		}

		public void Update(float deltaTime)
		{
			if(_scenes.Count > 0)
			{
				_scenes.Peek().Update(deltaTime);
			}
		}

		public void Render(IGraphics g)
		{
			if(_scenes.Count > 0)
			{
				_scenes.Peek().Render(g);
			}
		}

		public void Clear()
		{
			if(_scenes != null)
			{
				foreach(Scene scene in _scenes)
				{
					scene.Dispose();
				}
			}
			_scenes = new Stack<Scene>();
		}
	}
}
