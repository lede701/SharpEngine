﻿using SharpEngine.Library.GraphicsSystem;
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
		public Vector2D Position;
		public Vector2D WorldSize;
		public Vector2D BlockSize;
		public Vector2D PlayerPosition;

		private Dictionary<String, IUniverseItem> _world;

		private UniTile[,] MapData;

		

		public UniverseMaster()
		{
			_world = new Dictionary<String, IUniverseItem>();
			Position = new Vector2D { X = RandomManager.Instance.Next(0, (int)World.Instance.WorldSize.X), Y = RandomManager.Instance.Next(0, (int)World.Instance.WorldSize.Y) };
			SetupTile();
		}

		private void SetupTile()
		{
			RandomManager rm = RandomManager.Instance;
			int tileX = (int)(World.Instance.WorldSize.X / World.Instance.ScreenSize.X) + 1;
			int tileY = (int)(World.Instance.WorldSize.Y / World.Instance.ScreenSize.Y) + 1;

			MapData = new UniTile[tileX, tileY];

			for(int x = 0; x<tileX;++x)
			{
				for(int y=0;y<tileY; ++y)
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
					if(rm.Next(0,500) > 300)
					{
						float astX = rm.Next(range.Left, range.Right);
						float astY = rm.Next(range.Top, range.Bottom);
						MapData[x, y].Add(UniverseFactory.Instance.CreateAsteroid(astX, astY, 0f));
					}
				}
			}

		}

		public System.Drawing.Rectangle CurrentTilePosition
		{
			get
			{
				int tileX = (int)(Position.X / World.Instance.ScreenSize.X);
				int tileY = (int)(Position.Y / World.Instance.ScreenSize.Y);

				System.Drawing.Rectangle rect = new System.Drawing.Rectangle
				{
					X = System.Math.Max(tileX - 1, 0),
					Y = System.Math.Max(tileY - 1, 0),
					Width = 3,
					Height = 3
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
			g.Translate(-Position.X, -Position.Y, 0f, Vector2D.One);
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
