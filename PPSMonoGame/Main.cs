using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Properties;
using PPSMonoGame.Rendering;
using PPSMonoGame.Utility;
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
        int _windowWidth = 1000;
        int _windowHeight = 600;
        RenderTarget2D _renderTarget1;
        RenderTarget2D _renderTarget2;
        BloomFilter _bloomFilter;
        Texture2D _whitePixelTexture;

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

            var propGrid = new PropertyGrid
            {
                Object = settings,
                GridColumn = 1,
                GridRow = 0,
            };

            mainGrid.Widgets.Add(propGrid);

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

            var mouseState = Mouse.GetState();

            if(mouseState.LeftButton == ButtonState.Pressed)
			{
				foreach (var particle in _pps.Particles)
				{
                    var distSquared = Vector2.DistanceSquared(mouseState.Position.ToVector2(), particle.Position.ToVector2());
                    if(distSquared < 1)
					{
                        continue;
					}
                    if(distSquared < _pps.Settings.ReactionRadiusSquared)
					{
                        Vector2 force = Vector2.Normalize(mouseState.Position.ToVector2() - particle.Position.ToVector2()) * _pps.Settings.MouseForceMultiplier / (float)Math.Sqrt(distSquared);
                        particle.Position = new PrimordialParticleSystems.Utility.Point(particle.Position.X + force.X, particle.Position.Y + force.Y);
                    }
                }
			}
            else if(mouseState.RightButton == ButtonState.Pressed)
			{
                foreach (var particle in _pps.Particles)
                {
                    var distSquared = Vector2.DistanceSquared(mouseState.Position.ToVector2(), particle.Position.ToVector2());
                    if (distSquared < _pps.Settings.ReactionRadiusSquared)
                    {
                        Vector2 force = Vector2.Normalize(particle.Position.ToVector2() - mouseState.Position.ToVector2()) * _pps.Settings.MouseForceMultiplier / (float)Math.Sqrt(distSquared);
                        particle.Position = new PrimordialParticleSystems.Utility.Point(particle.Position.X + force.X, particle.Position.Y + force.Y);
                    }
                }
            }

            _pps.Update();

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
    }
}
