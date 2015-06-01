using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SimpleRPG.States;
using SimpleRPG.Windows;
using SimpleRPG.Items;
using System.Xml;

namespace SimpleRPG
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;
        
        private MapObject player;
        private Camera camera;

        private int screenWidth, screenHeight;
        private int graphicsScale;

        protected StateManager stateManager;

        private SpriteFont mainFont;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            stateManager = new StateManager(this);

            loadOptions();

            // Adjust scale based on Scale = 1 for 640x480 resolution
            int baseWidth = 640;

            graphicsScale = screenWidth / baseWidth;
        }

        private void loadOptions()
        {
            screenHeight = 720;
            screenWidth = 1280;
            try
            {
                XmlTextReader reader = new XmlTextReader("settings.xml");
                string windowStyle = "window";

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.Name == "width")
                            screenWidth = int.Parse(reader.ReadString());
                        else if (reader.Name == "height")
                            screenHeight = int.Parse(reader.ReadString());
                        else if (reader.Name == "style")
                            windowStyle = reader.ReadString();
                    }
                }

                reader.Close();

                // Create a fullscreen window
                if (windowStyle == "fullscreen")
                {
                    screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                    screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

                    graphics.IsFullScreen = true;
                }
                // Create a borderless window
                else if (windowStyle == "borderless")
                {
                    IntPtr hWnd = this.Window.Handle;
                    var control = System.Windows.Forms.Control.FromHandle(hWnd);
                    var form = control.FindForm();
                    form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                }
                // Otherwise, a regular window will be created
            }
            catch (Exception) { }

            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.PreferredBackBufferWidth = screenWidth;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            mainFont = Content.Load<SpriteFont>(@"fonts\fixedsys" + graphicsScale);

            // Initialize utilities class
            Utilities.initialize(this);

            // Initialize controller class
            Controller.initialize();
            
            // Initialize graphics helper
            GraphicsHelper.setGame(this);
            GraphicsHelper.setup();
            GraphicsHelper.setWidthHeight(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            ItemManager.initialize();
            ItemContainer playerInventory = Player.getInventory();
            //playerInventory.addItem("Small Potion",  2);
            //playerInventory.addItem("Large Potion");
            //playerInventory.addItem("Carved Wood Staff");
            playerInventory.addItem("Throwing Knife", 10);

            Player.initialize(this);
            EnemyManager.initialize();

            //MapState mapState = new MapState(this, null, stateManager, map, camera, player);
            TitleState titleState = new TitleState(this, stateManager);
            SplashScreenState splashState = new SplashScreenState(this, stateManager, "splash", titleState);
            
            stateManager.addState(splashState);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Input.update();
            Debug.update();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            if (stateManager != null)
                stateManager.update();

            if (Input.isKeyPressed(Keys.B))
            {
                stateManager.addState(new BattleState(this, null, stateManager));
            }
            else if (Input.isKeyPressed(Keys.M))
            {
                stateManager.addState(new MessageState(this, null, stateManager, "A fairly long test message"));
            }

            base.Update(gameTime);
        }

        private Color clearColor = new Color(41, 38, 52);

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);

            if (stateManager!= null)
                stateManager.draw(spriteBatch);
            
            spriteBatch.End();
            
            base.Draw(gameTime);
        }

        public virtual GameState getFirstGameState()
        {
            TileMap map = new TileMap(this, "cave");
            player = Player.getParty()[0].getMapObject();

            player.setPosition(8, 0);
            map.addObject(player);

            camera = new Camera(screenWidth, screenHeight);
            camera.setMap(map);
            camera.setFollowing(player);

            camera.setZoom(graphicsScale);

            return new MapState(this, null, stateManager, map, camera, player);
        }

        public int getWidth()
        {
            return screenWidth;
        }

        public int getHeight()
        {
            return screenHeight;
        }

        public Camera getCamera()
        {
            return camera;
        }

        public int getGraphicsScale()
        {
            return graphicsScale;
        }

        public SpriteFont getFont()
        {
            return mainFont;
        }    
    }

}
