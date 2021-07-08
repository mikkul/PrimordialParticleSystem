using Microsoft.Xna.Framework;
using System;

namespace PPSMonoGame
{
	internal class WindowManager
	{
		public event WindowSizeChangedHandler WindowSizeChanged;

		private readonly GameWindow _window;
		private readonly GraphicsDeviceManager _graphics;
		private readonly int _minWindowWidth;
		private readonly int _minWindowHeight;
		private int _windowWidth;
		private int _windowHeight;

		public WindowManager(GameWindow window, GraphicsDeviceManager graphics, int minWindowWidth, int minWindowHeight)
		{
			_window = window;
			_graphics = graphics;
			_minWindowWidth = minWindowWidth;
			_minWindowHeight = minWindowHeight;
			_windowWidth = minWindowWidth;
			_windowHeight = minWindowHeight;
		}

		public int WindowWidth
		{
			get => _windowWidth;
			set
			{
				_windowWidth = Math.Max(_minWindowWidth, value);
			}
		}
		public int WindowHeight
		{
			get => _windowHeight;
			set
			{
				_windowHeight = Math.Max(_minWindowHeight, value);
			}
		}

		public void ApplyChanges()
		{
			if (_graphics.PreferredBackBufferWidth == _windowWidth && _graphics.PreferredBackBufferHeight == _windowHeight)
			{
				return; // size hasn't changed
			}

			int screenWidth = _graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
			int screenHeight = _graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height;

			int x = screenWidth / 2 - _windowWidth / 2;
			int y = screenHeight / 2 - _windowHeight / 2;

			_graphics.PreferredBackBufferWidth = _windowWidth;
			_graphics.PreferredBackBufferHeight = _windowHeight;
			_graphics.ApplyChanges();

			_window.Position = new Point(x, y);

			WindowSizeChanged?.Invoke(_windowWidth, _windowHeight);
		}
	}

	internal delegate void WindowSizeChangedHandler(int width, int height);
}
