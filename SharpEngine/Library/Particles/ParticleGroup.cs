using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Math;
using SharpEngine.Library.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Particles
{
	public abstract class ParticleGroup : GObject
	{
		protected ArrayList _particles = new ArrayList();
		protected bool _regenerate = false;
		protected Vector2D _position;
		protected System.Drawing.Color _color;

		protected abstract Particle GenerateParticle();

		public abstract void Update(float deltaTime);

		public virtual void Render(IGraphics g)
		{
			Particle part;
			float intense;
			for(int i=0;i<Count; ++i)
			{
				part = this[i];
				intense = System.Math.Max(1.0f - (float)part.Life / (float)PARTICLES_MAX_LIFE, 0f);
				System.Drawing.Color clr = System.Drawing.Color.FromArgb((int)(255f * intense), part.Color.R, part.Color.G, part.Color.B);
				g.FillEllipse(part.Position.X, part.Position.Y, part.R, part.R, clr);
				part.LifePlus(1);
			}
		}

		public void Dispose()
		{
			// Nothing to dispose of year
		}

		public int Count
		{
			get
			{
				return _particles.Count;
			}
		}

		public virtual int PARTICLES_MAX_LIFE
		{
			get
			{
				return 600;
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

		public Particle this[int idx]
		{
			get
			{
				return (Particle)_particles[idx];
			}
		}
	}
}
