using SharpEngine.Library.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.GraphicsSystem
{
	public class Sprite
	{
		public Bitmap SpriteSheet;
		public Vector2D CenterPoint;
		public List<Rectangle> Frames;
		public int CurrentFrame;
		public bool AutoAdvance;

		public Sprite(String filename)
		{
			SpriteSheet = new Bitmap(filename);
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
				}

				return rect;
			}
		}
	}
}
