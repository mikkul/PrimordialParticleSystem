using Microsoft.Xna.Framework;
using PrimordialParticleSystems;
using System.Collections.Generic;

namespace PPSMonoGame
{
	class PPSSettings : Settings
	{
		/// <summary>
		/// 
		/// </summary>
		public bool EnableParticleTrace { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool EnableParticleGlow { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public float MouseForceMultiplier { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public float MouseForceRadius { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public float MouseForceRadiusSquared => MouseForceRadius * MouseForceRadius;

		/// <summary>
		/// 
		/// </summary>
		public Dictionary<int, Color> NeighbourThresholdColours { get; set; }

		/// <summary>
		/// Get or sets the particle radius in pixels
		/// </summary>
		public float ParticleSize { get; set; }
	}
}
