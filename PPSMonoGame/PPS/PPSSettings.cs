using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using PrimordialParticleSystems;
using System.Collections.Generic;
using System.ComponentModel;

namespace PPSMonoGame.PPS
{
	internal class PPSSettings : Settings
	{
		private float _mouseForceRadius;
		private float _mouseForceRadiusSquared;

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
		public float MouseForceRadius 
		{ 
			get => _mouseForceRadius;
			set
			{
				_mouseForceRadius = value;
				_mouseForceRadiusSquared = value * value;
			}
		}

		/// <summary>
		/// Used in calculations
		/// </summary>
		[JsonIgnore]
		[Browsable(false)]
		public float MouseForceRadiusSquared => _mouseForceRadiusSquared;

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

	internal struct IntValue
	{
		public IntValue(int value)
		{
			Value = value;
		}

		public int Value { get; set; }
	}
}
