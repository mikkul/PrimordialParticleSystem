using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;
using PPSMonoGame.PPS;
using PPSMonoGame.Rendering;
using PPSMonoGame.UI;
using PPSMonoGame.Utility;
using PrimordialParticleSystems.Boundaries;
using System;

namespace PPSMonoGame
{
    public class Main : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        WindowManager _windowManager;
        Desktop _desktop;
        MonoGamePrimordialParticleSystem _pps;
        RenderTarget2D _renderTarget1;
        RenderTarget2D _renderTarget2;
        BloomFilter _bloomFilter;
        Texture2D _whitePixelTexture;

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        protected override void Initialize()
        {
            _windowManager = new WindowManager(Window, _graphics, 500, 300);
            _windowManager.WindowSizeChanged += WindowSizeChanged;

            var settings = new PPSSettings
            {
                Boundary = new RectBoundary(0, 0),
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

            MyraEnvironment.Game = this;

            var mainGrid = new Grid();
            mainGrid.ColumnsProportions.Add(new Proportion(ProportionType.Part, 3));
            mainGrid.ColumnsProportions.Add(new Proportion(ProportionType.Part, 1));
            mainGrid.RowsProportions.Add(new Proportion());

            var sidebar = new Sidebar(_pps, _windowManager, Content.RootDirectory);

            var sidebarScrollViewer = new ScrollViewer
            {
                Content = sidebar,
                GridColumn = 1,
                GridRow = 0,
            };

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

            //
            _whitePixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            _whitePixelTexture.SetData(new Color[] { Color.White });

            //
            _windowManager.WindowWidth = 1200;
            _windowManager.WindowHeight = 800;
            _windowManager.ApplyChanges();
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

            _pps.Update();
            HandleInput();

			base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_renderTarget1);

            _spriteBatch.Begin();

            if (_pps.Settings.EnableParticleTrace)
			{
                _spriteBatch.Draw(_whitePixelTexture, new Rectangle(0, 0, (int)(_windowManager.WindowWidth * 0.75f), _windowManager.WindowHeight), new Color(Color.Black, 0.1f));
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
				bloom = _bloomFilter.Draw(_renderTarget1, (int)(_windowManager.WindowWidth * 0.75f), _windowManager.WindowHeight);
			}

			GraphicsDevice.SetRenderTarget(null);

			_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

			_spriteBatch.Draw(_renderTarget1, new Rectangle(0, 0, (int)(_windowManager.WindowWidth * 0.75f), _windowManager.WindowHeight), Color.White);

			if (_pps.Settings.EnableParticleGlow)
			{
				_spriteBatch.Draw(bloom, new Rectangle(0, 0, (int)(_windowManager.WindowWidth * 0.75f), _windowManager.WindowHeight), Color.White);
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

        private void WindowSizeChanged(int width, int height)
        {
            _renderTarget1 = new RenderTarget2D(GraphicsDevice, (int)(width * 0.75f), height, false, SurfaceFormat.Vector4, DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);
            _renderTarget2 = new RenderTarget2D(GraphicsDevice, (int)(width * 0.75f), height);

            _bloomFilter.Load(GraphicsDevice, Content, (int)(width * 0.75f), height);
            _bloomFilter.BloomPreset = BloomFilter.BloomPresets.Focussed;
            _bloomFilter.BloomStreakLength = 1;
            _bloomFilter.BloomThreshold = 0f;
            _bloomFilter.BloomStrengthMultiplier = 1.75f;

            _pps.Settings.Boundary.Size = new PrimordialParticleSystems.Utility.Point(width, height);
        }
    }
}
