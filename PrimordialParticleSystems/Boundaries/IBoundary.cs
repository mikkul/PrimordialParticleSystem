using PrimordialParticleSystems.Utility;

namespace PrimordialParticleSystems.Boundaries
{
	public interface IBoundary
	{
		/// <summary>
		/// 
		/// </summary>
		Point Size { get; }

		/// <summary>
		/// Clamps the given point to a position where it's not outside of the boundary
		/// </summary>
		/// <param name="point">Point to be clamped</param>
		/// <returns>If the original point lies outside of the boundary, returns a new point clamped to be inside the boundary; otherwise returns the original point unchanged</returns>
		Point Clamp(Point point);

		/// <summary>
		/// Checks whether a given point is inside the boundary
		/// </summary>
		/// <param name="point">The point to be checked</param>
		/// <returns><see langword="true"/> if the point lies inside or is on the boundary; othrwise, <see langword="false"/></returns>
		bool IsInBoundary(Point point);
	}
}
