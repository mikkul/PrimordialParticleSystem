using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Properties;
using PPSMonoGame.Rendering;
using PPSMonoGame.Utility;
using PPSMonoGame.Utility.Myra;
using PrimordialParticleSystems.Boundaries;
using System;

namespace PPSMonoGame
{
    public class Main : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        Desktop _desktop;
        MonoGamePrimordialParticleSystem _pps;
        int _windowWidth = 1200;
        int _windowHeight = 800;
        int _minWindowWidth = 600;
        int _minWindowHeight = 300;
        RenderTarget2D _renderTarget1;
        RenderTarget2D _renderTarget2;
        BloomFilter _bloomFilter;
        Texture2D _whitePixelTexture;
        bool _isPaused;

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = _windowWidth;
            _graphics.PreferredBackBufferHeight = _windowHeight;
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        protected override void Initialize()
        {
            _renderTarget1 = new RenderTarget2D(GraphicsDevice, (int)(_windowWidth * 0.75f), _windowHeight, false, SurfaceFormat.Vector4, DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);
            _renderTarget2 = new RenderTarget2D(GraphicsDevice, (int)(_windowWidth * 0.75f), _windowHeight);

            var settings = new PPSSettings
            {
                Boundary = new RectBoundary(_windowWidth * 0.75f, _windowHeight),
                Alpha = 180,
                Beta = 17,
                ParticleSize = 2,
                ParticleSpeed = 7,
                ReactionRadius = 150,
                BoundariesEnabled = true,
                MouseForceMultiplier = 250,
                MouseForceRadius = 150,
            };
            _pps = new MonoGamePrimordialParticleSystem(settings, Content);
            _pps.Spawn(175);

            MyraEnvironment.Game = this;

            var mainGrid = new Grid();
            mainGrid.ColumnsProportions.Add(new Proportion(ProportionType.Part, 3));
            mainGrid.ColumnsProportions.Add(new Proportion(ProportionType.Part, 1));
            mainGrid.RowsProportions.Add(new Proportion());

            var sidebar = new VerticalStackPanel
            {
                Spacing = 10,
                Padding = new Myra.Graphics2D.Thickness(5, 10),
            };

            var sidebarScrollViewer = new ScrollViewer
            {
                Content = sidebar,
                GridColumn = 1,
                GridRow = 0,
            };

            //

            var windowWidthInput = new LabelledInput
            {
                Text = "Window width:",
            };

            var windowHeightInput = new LabelledInput
            {
                Text = "Window height:",
            };

            var applyWindowSizeButton = new TextButton
            {
                Text = "Apply",
            };
            applyWindowSizeButton.Click += (s, e) =>
            {
                bool isValidWidth = int.TryParse(windowWidthInput.Value, out int newWidth);
                bool isValidHeight = int.TryParse(windowHeightInput.Value, out int newHeight);

                if(!isValidWidth || !isValidHeight)
				{
                    return;
				}

                newWidth = Math.Max(newWidth, _minWindowWidth);
                newHeight = Math.Max(newHeight, _minWindowHeight);

                _pps.Settings.Boundary.Size = new PrimordialParticleSystems.Utility.Point(newWidth, newHeight);

                // TODO: remove boilerplate

                _renderTarget1 = new RenderTarget2D(GraphicsDevice, (int)(newWidth * 0.75f), newHeight, false, SurfaceFormat.Vector4, DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);
                _renderTarget2 = new RenderTarget2D(GraphicsDevice, (int)(newWidth * 0.75f), newHeight);

                _bloomFilter.Load(GraphicsDevice, Content, (int)(newWidth * 0.75f), newHeight);
                _bloomFilter.BloomPreset = BloomFilter.BloomPresets.Focussed;
                _bloomFilter.BloomStreakLength = 1;
                _bloomFilter.BloomThreshold = 0f;
                _bloomFilter.BloomStrengthMultiplier = 1.75f;

                int screenWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
                int screenHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;

                int x = screenWidth / 2 - newWidth / 2;
                int y = screenHeight / 2 - newHeight / 2;

                _graphics.PreferredBackBufferWidth = newWidth;
                _graphics.PreferredBackBufferHeight = newHeight;
                _graphics.ApplyChanges();

                _windowWidth = newWidth;
                _windowHeight = newHeight;

                Window.Position = new Point(x, y);
            };

            //

            var particleCountInput = new LabelledInput
            {
                Text = "Number of particles:",
            };

            var spawnParticlesButton = new TextButton
            {
                Text = "Spawn",
            };
            spawnParticlesButton.Click += (s, e) =>
            {
                bool isANumber = int.TryParse(particleCountInput.Value, out int amount);
                if(isANumber)
				{
                    _pps.Spawn(amount);
                }
            };

            var clearParticlesButton = new TextButton
            {
                Text = "Clear",
            };
            clearParticlesButton.Click += (s, e) =>
            {
                _pps.Clear();
            };

            var pauseResumeSimulationButton = new TextButton
            {
                Text = "Pause",
            };
            pauseResumeSimulationButton.Click += (s, e) =>
            {
                _isPaused ^= true;
                pauseResumeSimulationButton.Text = _isPaused ? "Resume" : "Pause";
            };

            //

            var propGrid = new PropertyGrid
            {
                Object = settings,
            };

            //

            sidebar.Widgets.Add(windowWidthInput);
            sidebar.Widgets.Add(windowHeightInput);
            sidebar.Widgets.Add(applyWindowSizeButton);
            sidebar.Widgets.Add(new HorizontalSeparator());
            sidebar.Widgets.Add(particleCountInput);
            sidebar.Widgets.Add(spawnParticlesButton);
            sidebar.Widgets.Add(clearParticlesButton);
            sidebar.Widgets.Add(pauseResumeSimulationButton);
            sidebar.Widgets.Add(new HorizontalSeparator());
            sidebar.Widgets.Add(propGrid);

            mainGrid.Widgets.Add(sidebarScrollViewer);

            _desktop = new Desktop();
            _desktop.Widgets.Add(mainGrid);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _pps.SpriteBatch = _spriteBatch;

            // Load the BloomFilter
            _bloomFilter = new BloomFilter();
            _bloomFilter.Load(GraphicsDevice, Content, (int)(_windowWidth * 0.75f), _windowHeight);

            _bloomFilter.BloomPreset = BloomFilter.BloomPresets.Focussed;
            _bloomFilter.BloomStreakLength = 1;
            _bloomFilter.BloomThreshold = 0f;
            _bloomFilter.BloomStrengthMultiplier = 1.75f;

            //
            _whitePixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            _whitePixelTexture.SetData(new Color[] { Color.White });
        }

        protected override void UnloadContent()
        {
            _bloomFilter.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				Exit();
			}

            if(!_isPaused)
			{
                _pps.Update();
                HandleInput();
            }

			base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_renderTarget1);

            _spriteBatch.Begin();

            if (_pps.Settings.EnableParticleTrace)
			{
                _spriteBatch.Draw(_whitePixelTexture, new Rectangle(0, 0, (int)(_windowWidth * 0.75f), _windowHeight), new Color(Color.Black, 0.1f));
            }
            else
			{
                GraphicsDevice.Clear(Color.Black);
			}

            _pps.Render();

            _spriteBatch.End();

			GraphicsDevice.SetRenderTarget(_renderTarget2);

			_spriteBatch.Begin();
			_pps.Render();
			_spriteBatch.End();

			Texture2D bloom = null;
			if (_pps.Settings.EnableParticleGlow)
			{
				bloom = _bloomFilter.Draw(_renderTarget1, (int)(_windowWidth * 0.75f), _windowHeight);
			}

			GraphicsDevice.SetRenderTarget(null);

			_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

			_spriteBatch.Draw(_renderTarget1, new Rectangle(0, 0, (int)(_windowWidth * 0.75f), _windowHeight), Color.White);

			if (_pps.Settings.EnableParticleGlow)
			{
				_spriteBatch.Draw(bloom, new Rectangle(0, 0, (int)(_windowWidth * 0.75f), _windowHeight), Color.White);
			}

			_spriteBatch.End();

			_desktop.Render();

            base.Draw(gameTime);
        }

        private void HandleInput()
		{
            var mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                moveParticlesByMouseForce(1);
            }
            else if (mouseState.RightButton == ButtonState.Pressed)
            {
                moveParticlesByMouseForce(-1);
            }

            void moveParticlesByMouseForce(float multiplier)
			{
                foreach (var particle in _pps.Particles)
                {
                    var distSquared = Vector2.DistanceSquared(mouseState.Position.ToVector2(), particle.Position.ToVector2());
                    if (distSquared < _pps.Settings.ReactionRadiusSquared)
                    {
                        Vector2 force = Vector2.Normalize(mouseState.Position.ToVector2() - particle.Position.ToVector2()) * _pps.Settings.MouseForceMultiplier / (float)Math.Sqrt(distSquared);
                        force *= multiplier;
                        particle.Position = new PrimordialParticleSystems.Utility.Point(particle.Position.X + force.X, particle.Position.Y + force.Y);
                    }
                }
            }
        }
    }
}
