using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Properties;
using PrimordialParticleSystems.Boundaries;

namespace PPSMonoGame
{
    public class Main : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        Desktop _desktop;
        MonoGamePrimordialParticleSystem _pps;
        int _windowWidth;
        int _windowHeight;
        
        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _windowWidth = 1000;
            _windowHeight = 600;
            _graphics.PreferredBackBufferWidth = _windowWidth;
            _graphics.PreferredBackBufferHeight = _windowHeight;
        }

        protected override void Initialize()
        {
            var settings = new PPSSettings
            {
                Boundary = new RectBoundary(_windowWidth * 0.75f, _windowHeight),
                Alpha = 180,
                Beta = 17,
                ParticleSize = 3,
                ParticleSpeed = 3,
                ReactionRadius = 50,
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
        }

        protected override void UnloadContent()
        {
            
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
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            _pps.Render();
            _spriteBatch.End();

            _desktop.Render();

            base.Draw(gameTime);
        }
    }
}
