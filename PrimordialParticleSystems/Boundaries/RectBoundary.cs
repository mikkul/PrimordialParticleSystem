using PrimordialParticleSystems.Utility;

namespace PrimordialParticleSystems.Boundaries
{
	public class RectBoundary : IBoundary
	{
		public RectBoundary(float width, float height)
		{
			Size = new Point(width, height);
		}

		/// <inheritdoc/>
		public Point Size { get; set; }

		/// <inheritdoc/>
		public Point Clamp(Point point)
		{
			Point clamped = point;

			clamped.X = MathHelper.Clamp(point.X, 0, Size.X);
			clamped.Y = MathHelper.Clamp(point.Y, 0, Size.Y);

			return clamped;
		}

		/// <inheritdoc/>
		public bool IsInBoundary(Point point)
		{
			throw new System.NotImplementedException();
		}
	}
}
