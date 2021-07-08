namespace PPSMonoGame.Utility
{
	internal static class MonoGameExtensions
	{
		public static Microsoft.Xna.Framework.Vector2 ToVector2(this PrimordialParticleSystems.Utility.Point point)
		{
			return new Microsoft.Xna.Framework.Vector2(point.X, point.Y);
		}
	}
}
