using Microsoft.Xna.Framework;
using PrimordialParticleSystems;
using System.Collections.Generic;
using System.ComponentModel;

namespace PPSMonoGame.PPS
{
	class PPSSettings : Settings
	{
		public PPSSettings()
		{
			NeighbourCountThresholds = new List<IntValue>
			{
				new IntValue(0),
			};
			ParticleColors = new List<Color>
			{
				Color.DarkGreen,
			};
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
		[DisplayName("Thresholds")]
		public List<IntValue> NeighbourCountThresholds { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Category("Visuals")]
		[DisplayName("Colors")]
		public List<Color> ParticleColors { get; set; }

		/// <summary>
		/// Get or sets the particle radius in pixels
		/// </summary>
		[Category("Visuals")]
		[DisplayName("Particle size")]
		public float ParticleSize { get; set; }
	}

	struct IntValue
	{
		public IntValue(int value)
		{
			Value = value;
		}

		public int Value { get; set; }
	}
}
