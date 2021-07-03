using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PrimordialParticleSystems;

namespace PPSMonoGame
{
	class MonoGamePrimordialParticleSystem : PrimordialParticleSystem
	{
		private readonly Texture2D _circleTexture;
		private readonly PPSSettings _settings;

		public MonoGamePrimordialParticleSystem(PPSSettings settings, ContentManager content) : base(settings)
		{
			_settings = settings;
			_circleTexture = content.Load<Texture2D>("circleTexture");
		}

		public SpriteBatch SpriteBatch { get; set; }

		/// <inheritdoc/>
		public override void Render()
		{
			foreach (var particle in Particles)
			{
				var rect = new Rectangle((int)(particle.Position.X - _settings.ParticleSize), (int)(particle.Position.Y - _settings.ParticleSize), (int)(_settings.ParticleSize * 2), (int)(_settings.ParticleSize * 2));
				SpriteBatch.Draw(_circleTexture, rect, Color.DarkGreen);
			}
		}
	}
}
