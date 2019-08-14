using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Math;
using SharpEngine.Library.Objects;
using SharpEngine.Library.Randomizer;
using SharpEngine.Library.User.Factories;
using SharpEngine.Library.User.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.User.Universe
{
	public class UniverseMaster : GObject
	{
		public Vector2D WorldSize;
		public Vector2D BlockSize;
		public Vector2D PlayerPosition;

		private Dictionary<String, IUniverseItem> _world;

		private UniTile[,] MapData;

		public UObject DebugObj;

		public Vector2D Position { get; set; }

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


		public World World
		{
			get
			{
				return World.Instance;
			}
		}

		public UniverseMaster()
		{
			_world = new Dictionary<String, IUniverseItem>();
			SetupTile();
		}

		private int _tileMaxX;
		private int _tileMaxY;

		private void SetupTile()
		{
			RandomManager rm = RandomManager.Instance;
			_tileMaxX = (int)(World.Instance.WorldSize.X / World.Instance.ScreenSize.X) + 1;
			_tileMaxY = (int)(World.Instance.WorldSize.Y / World.Instance.ScreenSize.Y) + 1;

			MapData = new UniTile[_tileMaxX, _tileMaxY];

			for(int x = 0; x<_tileMaxX;++x)
			{
				for(int y=0;y<_tileMaxY; ++y)
				{
					System.Drawing.Rectangle range = new System.Drawing.Rectangle
					{
						X = (int)(x * World.Instance.ScreenSize.X),
						Y = (int)(y * World.Instance.ScreenSize.Y),
						Width = (int)World.Instance.ScreenSize.X,
						Height = (int)World.Instance.ScreenSize.Y
					};
					MapData[x, y] = new UniTile();
					for (int i = 0; i < 100; ++i)
					{
						UniverseStar star = new UniverseStar(range);
						MapData[x, y].Add(star);
					}
					//*
					if(rm.Next(0,500) > 300)
					{
						float astX = rm.Next(range.Left, range.Right);
						float astY = rm.Next(range.Top, range.Bottom);
						//AddAsteroid(astX, astY);
						//MapData[x, y].Add(UniverseFactory.Instance.CreateAsteroid(astX, astY, 0f));
						//SceneManager.Instance.Add(UniverseFactory.Instance.CreateAsteroid(astX, astY, 0f));
					}
					//*/
				}
			}
			for(int i=0; i<10; ++i)
			{
				float x = rm.Next(0, (int)World.WorldSize.X);
				float y = rm.Next(0, (int)World.WorldSize.Y);
				AddAsteroid(x, y);
			}

		}

		public void AddAsteroid(float x, float y)
		{
			SpriteAsteroid asteroid = UniverseFactory.Instance.CreateAsteroid(x, y, 0f);
			int mapX = (int)(x / World.Instance.ScreenSize.X);
			int mapY = (int)(y / World.Instance.ScreenSize.Y);
			//MapData[mapX, mapY].Add(asteroid);
			SceneManager.Instance.Add(asteroid, 4);
			//DebugObj = asteroid;
		}

		public System.Drawing.Rectangle CurrentTilePosition
		{
			get
			{
				int tileX = (int)(World.WorldPosition.X / World.Instance.ScreenSize.X);
				int tileY = (int)(World.WorldPosition.Y / World.Instance.ScreenSize.Y);
				int w = System.Math.Min(3, _tileMaxX - tileX);
				int h = System.Math.Min(3, _tileMaxY - tileY);

				System.Drawing.Rectangle rect = new System.Drawing.Rectangle
				{
					X = System.Math.Max(tileX - 1, 0),
					Y = System.Math.Max(tileY - 1, 0),
					Width = w,
					Height = h
				};

				return rect;
			}
		}

		private String _key = Guid.NewGuid().ToString();
		public string Key
		{
			get
			{
				return _key;
			}
		}

		public void Dispose()
		{
			
		}

		public void Render(IGraphics g)
		{
			g.Translate(-World.WorldPosition.X, -World.WorldPosition.Y, 0f, Vector2D.One);
			System.Drawing.Rectangle rect = CurrentTilePosition;
			for (int x = rect.Left; x < rect.Right; ++x)
			{
				for (int y = rect.Top; y < rect.Bottom; ++y)
				{
					MapData[x, y].Render(g);
					String tilePos = String.Format("[{0},{1}]", x, y);
					g.DrawText(tilePos, "Ariel", 10, System.Drawing.Color.White, new System.Drawing.Rectangle
					{
						X = (int)(x * World.Instance.ScreenSize.X),
						Y = (int)(y * World.Instance.ScreenSize.Y),
						Width = 100,
						Height = 20
					});
				}
			}

			g.TranslateReset();
		}

		public void Update(float deltaTime)
		{
			foreach (IUniverseItem item in _world.Values)
			{
				item.Update(deltaTime);
			}

		}
	}
}
