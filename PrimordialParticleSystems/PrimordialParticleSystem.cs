using PrimordialParticleSystems.Utility;
using PrimordialParticleSystems.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
			if(Settings.IsPaused)
			{
				return;
			}

			if(Particles.Count > 1)
			{
				SweepAndPrune();
			}

			foreach (var particle in Particles)
			{
				UpdateParticleRotation(particle);
				UpdateParticlePosition(particle);
			}
		}

		#region Private methods
		private void UpdateParticleRotation(Particle particle)
		{
			float reactionRotation = Settings.Beta * Math.Sign(particle.NeigbhourRatio) * particle.NeighbourCount;

			particle.Rotation += Settings.Alpha + reactionRotation;
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

		private void SweepAndPrune()
		{
			Dictionary<Particle, (float left, float right)> endpoints = new Dictionary<Particle, (float left, float right)>();
			Particles = Particles.OrderBy(p => p.Position.X).ToList();
			foreach (var particle in Particles)
			{
				endpoints[particle] = (particle.Position.X - Settings.ReactionRadius, particle.Position.X + Settings.ReactionRadius);
				particle.NeigbhourRatio = 0;
				particle.NeighbourCount = 0;
			}
			var activeList = new List<Particle>();
			activeList.Add(Particles[0]);
			for (int i = 1; i < Particles.Count; i++)
			{
				for (int j = 0; j < activeList.Count; j++)
				{
					if (endpoints[Particles[i]].left > endpoints[activeList[j]].right)
					{
						activeList.RemoveAt(j);
						j--;
						continue;
					}

					float dx = Particles[i].Position.X - activeList[j].Position.X;
					float dy = Particles[i].Position.Y - activeList[j].Position.Y;
					double distSquared = dx * dx + dy * dy;
					if (distSquared <= Settings.ReactionRadiusSquared)
					{
						float xSeparation = activeList[j].Position.X - Particles[i].Position.X;
						float ySeparation = activeList[j].Position.Y - Particles[i].Position.Y;

						float separationAngle = (float)Math.Atan2(ySeparation, xSeparation);

						if (xSeparation * Math.Sin(Particles[i].RotationRadians) - ySeparation * Math.Cos(Particles[i].RotationRadians) < 0)
						{
							Particles[i].NeigbhourRatio++;
						}
						else
						{
							Particles[i].NeigbhourRatio--;
						}
						Particles[i].NeighbourCount++;
					}
				}
				activeList.Add(Particles[i]);
			}
		}
		#endregion
	}
}
