using SharpEngine.Library.Math;
using SharpEngine.Library.Objects;
using SharpEngine.Library.Randomizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Particles
{
	public class ParticleExplosion : ParticleGroup
	{
		protected override Particle GenerateParticle()
		{
			float vx = (100f - RandomManager.Instance.Next(20, 200)) / 100f;
			float vy = (100f - RandomManager.Instance.Next(20, 200)) / 100f;

			return new Particle(new Vector2D(_position), new Math.Vector2D { X = vx, Y = vy }, _color);
		}

		public ParticleExplosion(Vector2D pos)
		{
			// Setup explosion 
			_position = pos;
			_color = System.Drawing.Color.FromArgb(255, 242, 226, 218);
			// Create explosion particles and add then to group
			for(int i=0; i<10; ++i)
			{
				_particles.Add(GenerateParticle());
			}
		}

		public override void Update(float deltaTime)
		{
			for(int i=0; i<Count;++i)
			{
				if(this[i].Update(deltaTime) || this[i].Life > PARTICLES_MAX_LIFE)
				{
					_particles.RemoveAt(i);
				}
			}

			// Check if there are any particles left
			if(Count == 0)
			{
				// Remove myselft from the scene
				SceneManager.Instance.Scene.Remove(this, 6);
			}
		}
	}
}
