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

		public SceneManager()
		{
			// Create new scene
			_scenes = new Stack<Scene>();
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
				_scenes.Peek().Add(obj, layer);
			}
		}

		public void Update(float deltaTime)
		{
			if(_scenes.Count > 0)
			{
				_scenes.Peek().Update(deltaTime);
			}
		}

		public void Render(Graphics g)
		{
			if(_scenes.Count > 0)
			{
				_scenes.Peek().Render(g);
			}
		}
	}
}
