using PrimordialParticleSystems.Utility;
using PrimordialParticleSystems.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.IO;

namespace PrimordialParticleSystems
{
	public class PrimordialParticleSystem
	{
		protected Random _rng;

		public PrimordialParticleSystem(Settings settings)
		{
			_rng = new Random();

			Settings = settings;

			Particles = new List<Particle>();
		}

		/// <summary>
		/// Settings for this particle system
		/// </summary>
		public Settings Settings { get; protected set; }

		/// <summary>
		/// Used to enumerate through all the particles in the system
		/// </summary>
		public List<Particle> Particles { get; protected set; }

		/// <summary>
		/// Renders all of the particles in the system
		/// </summary>
		public virtual void Render()
		{
		}

		/// <summary>
		/// Removes all particles from the system
		/// </summary>
		public virtual void Clear()
		{
			Particles.Clear();
		}

		/// <summary>
		/// Loads simulation state from the provided file
		/// </summary>
		/// <exception cref="FileNotFoundException"></exception>
		/// <param name="filePath">File path to read data from</param>
		public virtual void LoadState(string filePath)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Loads simulation state from the provided <see cref="SimulationStateSnapshot"/>
		/// </summary>
		/// <param name="stateSnapshot"><see cref="SimulationStateSnapshot"/> object to read data from</param>
		public virtual void LoadState(SimulationStateSnapshot stateSnapshot)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Saves the current simulation state into an object and optionally a file
		/// </summary>
		/// <param name="filePath">(optional) file path to save the state</param>
		/// <returns>A <see cref="SimulationStateSnapshot"/> describing the current simulation state</returns>
		public virtual SimulationStateSnapshot SaveState(string filePath = null)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Spawns the specified amount of particles onto the screen
		/// </summary>
		/// <param name="amount">Number of particles to spawn</param>
		public virtual void Spawn(int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				var position = new Point();
				position.X = _rng.NextFloat(0, Settings.Boundary.Size.X);
				position.Y = _rng.NextFloat(0, Settings.Boundary.Size.Y);
				position = Settings.Boundary.Clamp(position);

				float rotation = _rng.NextFloat(0, 360);

				var particle = new Particle(position, rotation);
				Particles.Add(particle);
			}
		}

		/// <summary>
		/// Spawns the specified amount of particles in the specified position
		/// </summary>
		/// <param name="x">The spawn position on the x axis</param>
		/// <param name="y">The spawn position on the y axis</param>
		/// <param name="amount">Number of particles to spawn</param>
		public virtual void SpawnAt(float x, float y, int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				var position = new Point();
				position.X = x;
				position.Y = y;

				float rotation = _rng.NextFloat(0, 360);

				var particle = new Particle(position, rotation);
				Particles.Add(particle);
			}
		}

		/// <summary>
		/// Toggles whether the world boundaries will be in effect
		/// </summary>
		public virtual void ToggleBoundaries()
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Progresses forward the simulation, updaing each particle
		/// </summary>
		public virtual void Update()
		{
			foreach (var particle in Particles)
			{
				UpdateParticleRotation(particle);
				UpdateParticlePosition(particle);
			}
		}

		#region Private methods
		private (int left, int right) GetNumberOfNeighboursOnEachSide(Particle particle, List<Point> neighbours)
		{
			int leftNeighbours = 0;
			int rightNeighbours = 0;

			foreach (Point neighbour in neighbours)
			{
				float xSeparation = neighbour.X - particle.Position.X;
				float ySeparation = neighbour.Y - particle.Position.Y;

				float separationAngle = (float)(Math.Atan2(ySeparation, xSeparation));

				if (xSeparation * Math.Sin(particle.RotationRadians) - ySeparation * Math.Cos(particle.RotationRadians) < 0)
				{
					rightNeighbours++;
				}
				else
				{
					leftNeighbours++;
				}
			}
			return (leftNeighbours, rightNeighbours);
		}

		private List<Point> GetParticleNeighbours(Particle particle)
		{
			var inRange = new List<Point>();

			foreach (Particle otherParticle in Particles)
			{
				// brute force for now
				// TODO: optimize with prune and sweep(?)
				if (particle != otherParticle)
				{
					double distSquared = Math.Pow(Math.Abs(particle.Position.X - otherParticle.Position.X), 2) + Math.Pow(Math.Abs(particle.Position.Y - otherParticle.Position.Y), 2);
					if (distSquared <= Settings.ReactionRadiusSquared)
					{
						inRange.Add(particle.Position);
					}
				}
			}

			return inRange;
		}

		private void UpdateParticlePosition(Particle particle)
		{
			float movementX = Settings.ParticleSpeed * (float)Math.Cos(particle.RotationRadians);
			float movementY = Settings.ParticleSpeed * (float)Math.Sin(particle.RotationRadians);
			particle.Position = new Point
			{
				X = particle.Position.X + movementX,
				Y = particle.Position.Y + movementY,
			};

			if(Settings.BoundariesEnabled)
			{
				particle.Position = Settings.Boundary.Clamp(particle.Position);
			}
		}

		private void UpdateParticleRotation(Particle particle)
		{
			List<Point> neighbours = GetParticleNeighbours(particle);

			(int left, int right) = GetNumberOfNeighboursOnEachSide(particle, neighbours);
			particle.NeighbourCount = left + right;

			float reactionRotation = Settings.Beta * Math.Sign(right - left) * particle.NeighbourCount;

			particle.Rotation += Settings.Alpha + reactionRotation;
		}
		#endregion
	}
}
