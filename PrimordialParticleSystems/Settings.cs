using PrimordialParticleSystems.Boundaries;
using PrimordialParticleSystems.Utility;

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
		public float AlphaRadians => _alphaRadians;

		/// <summary>
		/// Angle in degrees used in the motion formula
		/// </summary>
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
		public float BetaRadians => _betaRadians;

		/// <summary>
		/// Describes whether the particles will be limited by the boundaries
		/// </summary>
		public bool BoundariesEnabled { get; set; }

		/// <summary>
		/// A boundary describing the area which the particles will be limited to
		/// </summary>
		public IBoundary Boundary { get; set; }

		/// <summary>
		/// Number of pixels a particle can traverse in a straight line in one frame
		/// </summary>
		public float ParticleSpeed { get; set; }

		/// <summary>
		/// Number of pixels specifying the reaction radius of a particle
		/// </summary>
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
