using System;

namespace PrimordialParticleSystems.Utility
{
	public static class MathHelper
	{
		private const float _toRadians = (float)(Math.PI / 180);
		private const float _toDegrees = (float)(180 / Math.PI);

		public static float ToDegrees(float angle)
		{
			return angle * _toDegrees;
		}

		public static float ToRadians(float angle)
		{
			return angle * _toRadians;
		}

		public static float Clamp(float value, float min, float max)
		{
			return Math.Max(0, Math.Min(max, value));
		}
	}
}
