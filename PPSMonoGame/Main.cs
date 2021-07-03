using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Properties;
using PPSMonoGame.Rendering;
using PrimordialParticleSystems.Boundaries;

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
        RenderTarget2D _renderTarget;
        BloomFilter _bloomFilter;

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
            _renderTarget = new RenderTarget2D(GraphicsDevice, (int)(_windowWidth * 0.75f), _windowHeight);

            var settings = new PPSSettings
            {
                Boundary = new RectBoundary(_windowWidth * 0.75f, _windowHeight),
                Alpha = 180,
                Beta = 17,
                ParticleSize = 2,
                ParticleSpeed = 7,
                ReactionRadius = 150,
                BoundariesEnabled = true,
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

			base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_renderTarget);

            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            _pps.Render();
            _spriteBatch.End();

            Texture2D bloom = null;
            if (_pps.Settings.EnableParticleGlow)
			{
                bloom = _bloomFilter.Draw(_renderTarget, (int)(_windowWidth * 0.75f), _windowHeight);
            }

            GraphicsDevice.SetRenderTarget(null);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

            _pps.Render();

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
