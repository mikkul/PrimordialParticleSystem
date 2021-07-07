using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PrimordialParticleSystems;

namespace PPSMonoGame.PPS
{
	class MonoGamePrimordialParticleSystem : PrimordialParticleSystem
	{
		private readonly Texture2D _circleTexture;

		public MonoGamePrimordialParticleSystem(PPSSettings settings, ContentManager content) : base(settings)
		{
			Settings = settings;
			_circleTexture = content.Load<Texture2D>("circleTexture");
		}

		public SpriteBatch SpriteBatch { get; set; }
		public new PPSSettings Settings { get; set; }

		/// <inheritdoc/> 
		public override void Render()
		{
			foreach (var particle in Particles)
			{
				Color color = Color.Magenta;

				for (int i = 0; i < Settings.NeighbourCountThresholds.Count; i++)
				{
					if(i >= Settings.ParticleColors.Count)
					{
						break;
					}
					if(particle.NeighbourCount >= Settings.NeighbourCountThresholds[i].Value)
					{
						color = Settings.ParticleColors[i];
					}
				}

				var rect = new Rectangle((int)(particle.Position.X - Settings.ParticleSize), (int)(particle.Position.Y - Settings.ParticleSize), (int)(Settings.ParticleSize * 2), (int)(Settings.ParticleSize * 2));
				SpriteBatch.Draw(_circleTexture, rect, color);
			}
		}
	}
}
