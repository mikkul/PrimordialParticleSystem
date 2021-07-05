using Microsoft.Xna.Framework;
using PrimordialParticleSystems;
using System.Collections.Generic;
using System.ComponentModel;

namespace PPSMonoGame
{
	class PPSSettings : Settings
	{
		public PPSSettings()
		{
			// I think dictionaries aren't supported by Myra's PropertyGrid
			//NeighbourThresholdColours = new Dictionary<int, Color>
			//{
			//	{0, Color.DarkGreen },
			//};
		}

		/// <summary>
		/// 
		/// </summary>
		[Category("Visuals")]
		[DisplayName("Particle trace")]
		public bool EnableParticleTrace { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Category("Visuals")]
		[DisplayName("Particle glow")]
		public bool EnableParticleGlow { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Category("Interaction")]
		[DisplayName("Mouse force")]
		public float MouseForceMultiplier { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Category("Interaction")]
		[DisplayName("Mouse radius")]
		public float MouseForceRadius { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Category("Readonly")]
		[DisplayName("Mouse radius sq")]
		public float MouseForceRadiusSquared => MouseForceRadius * MouseForceRadius;

		/// <summary>
		/// 
		/// </summary>
		[Category("Visuals")]
		[DisplayName("Threshold colors")]
		public Dictionary<int, Color> NeighbourThresholdColours { get; set; }

		/// <summary>
		/// Get or sets the particle radius in pixels
		/// </summary>
		[Category("Visuals")]
		[DisplayName("Particle size")]
		public float ParticleSize { get; set; }
	}
}
