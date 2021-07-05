using PrimordialParticleSystems.Boundaries;
using PrimordialParticleSystems.Utility;
using System.ComponentModel;

namespace PrimordialParticleSystems
{
	public class Settings
	{
		private float _reactionRadius;
		private float _reactionRadiusSquared;
		private float _alphaDegrees;
		private float _alphaRadians;
		private float _betaDegrees;
		private float _betaRadians;

		/// <summary>
		/// Angle in degrees a particle rotates each frame
		/// </summary>
		[Category("Simulation")]
		[DisplayName("Alpha (degrees)")]
		public float Alpha
		{
			get => _alphaDegrees;
			set
			{
				_alphaDegrees = value;
				_alphaRadians = MathHelper.ToRadians(value);
			}
		}

		/// <summary>
		/// Alpha value in radians
		/// </summary>
		[Category("Readonly")]
		[DisplayName("Alpha (radians)")]
		public float AlphaRadians => _alphaRadians;

		/// <summary>
		/// Angle in degrees used in the motion formula
		/// </summary>
		[Category("Simulation")]
		[DisplayName("Beta (degrees)")]
		public float Beta { 
			get => _betaDegrees;
			set
			{
				_betaDegrees = value;
				_betaRadians = MathHelper.ToRadians(value);
			}
		}

		/// <summary>
		/// Beta value in radians
		/// </summary>
		[Category("Readonly")]
		[DisplayName("Beta (radians)")]
		public float BetaRadians => _betaRadians;

		/// <summary>
		/// Describes whether the particles will be limited by the boundaries
		/// </summary>
		[Category("Interaction")]
		[DisplayName("Enable boundaries")]
		public bool BoundariesEnabled { get; set; }

		/// <summary>
		/// A boundary describing the area which the particles will be limited to
		/// </summary>
		[Category("Readonly")]
		[DisplayName("Boundary")]
		public IBoundary Boundary { get; set; }

		/// <summary>
		/// Number of pixels a particle can traverse in a straight line in one frame
		/// </summary>
		[Category("Simulation")]
		[DisplayName("Particle speed")]
		public float ParticleSpeed { get; set; }

		/// <summary>
		/// Number of pixels specifying the reaction radius of a particle
		/// </summary>
		[Category("Simulation")]
		[DisplayName("Reaction radius")]
		public float ReactionRadius
		{
			get => _reactionRadius;
			set
			{
				_reactionRadius = value;
				_reactionRadiusSquared = value * value;
			}
		}

		/// <summary>
		/// Used for calculations
		/// </summary>
		[Category("Readonly")]
		[DisplayName("Reaction radius sq")]
		public float ReactionRadiusSquared => _reactionRadiusSquared;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static Settings FromFile(string filePath)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filePath"></param>
		public void SaveToFile(string filePath)
		{
			throw new System.NotImplementedException();
		}
	}
}
