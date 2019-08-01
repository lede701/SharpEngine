using SharpEngine.Library.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.GraphicsSystem
{
	public class Sprite
	{
		public Object SpriteSheet;
		public Vector2D CenterPoint;
		public List<Rectangle> Frames;
		public int CurrentFrame;
		public bool AutoAdvance;
		public bool AutoDispose;

		public Sprite(String filename)
		{
			if (File.Exists(filename))
			{
				SpriteSheet = GraphicsManager.Instance.LoadImage(filename);
			}
			CenterPoint = new Vector2D();
			Frames = new List<Rectangle>();
			CurrentFrame = 0;
		}

		public Sprite(Object spriteSheet)
		{
			SpriteSheet = spriteSheet;
			CenterPoint = new Vector2D();
			Frames = new List<Rectangle>();
			CurrentFrame = 0;
		}


		public Rectangle Frame
		{
			get
			{
				Rectangle rect = new Rectangle();
				if(Frames.Count > 0)
				{
					rect = Frames[CurrentFrame];
					// Check if auto advance is turned on and we have enough frames
					if(AutoAdvance && Frames.Count > 1)
					{
						// Incrament to next frame and make sure we don't create an invalide index for the list
						CurrentFrame = (CurrentFrame + 1) % Frames.Count;
					}
				}else
				{
					rect = new Rectangle
					{
						X = 0,
						Y = 0,
						Width = 0,
						Height = 0
					};
				}

				return rect;
			}
		}

		public void Dispose()
		{
			if(SpriteSheet is IDisposable)
			{
				((IDisposable)SpriteSheet).Dispose();
			}
		}
	}
}
