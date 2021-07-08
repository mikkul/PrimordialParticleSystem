namespace PPSMonoGame
{
	public static class Program
	{
		private static void Main()
		{
			using (var game = new Main())
			{
				game.Run();
			}
		}
	}
}
