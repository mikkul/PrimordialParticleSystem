using PrimordialParticleSystems.Utility;
using System.Collections.Generic;

namespace PrimordialParticleSystems
{
	public class Particle
	{
		private float _rotationDegrees;
		private float _rotationRadians;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="position">A point containing the initial X and Y position</param>
		/// <param name="rotation">Angle in degrees</param>
		public Particle(Point position, float rotation)
		{
			Position = position;
			Rotation = rotation;
		}

		/// <summary>
		/// A vector containing the X and Y positions of the particle
		/// </summary>
		public Point Position { get; set; }

		/// <summary>
		/// Current rotation in radians
		/// </summary>
		public float Rotation 
		{ 
			get => _rotationDegrees;
			set
			{
				_rotationDegrees = value;
				_rotationRadians = MathHelper.ToRadians(value);
			}
		}

		/// <summary>
		/// Rotations expressed in radians
		/// </summary>
		public float RotationRadians => _rotationRadians;

		/// <summary>
		/// Total number of neighbours in reaction radius
		/// </summary>
		public int NeighbourCount { get; set; }

		/// <summary>
		/// Ratio of left-side neighbours to the right-side neighbours
		/// </summary>
		public int NeigbhourRatio { get; set; }
	}
}
