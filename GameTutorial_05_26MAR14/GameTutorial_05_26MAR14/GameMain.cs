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
using LibraryProject;

namespace GameTutorial_05_26MAR14
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameMain : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        static public Viewport VP;
        static public Vector2 VPCenter
        {
            get { return new Vector2(VP.Width / 2, VP.Height / 2); }
        }

        ContentManager mapGenContentManager;
        static public ContentManager contentMan;

        public GameMain()
        {
            graphics = new GraphicsDeviceManager(this);
            mapGenContentManager = this.Content;
            contentMan = this.Content;
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            //graphics.IsFullScreen = true;
            this.IsMouseVisible = true;
            Content.RootDirectory = "Content";

            VP = new Viewport(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
           // elapsedTime = 0;
            //millisecondsBetweenInput = 100;
            
            //screenManager = new ScreenManager(graphics, mapGenContentManager, this);
            //Components.Add(screenManager);
        }

        //static public ScreenManager ScreenManager
        //{
        //    //get { return (screenManager == null ? null : GameMain.screenManager); }
        //    get { return screenManager; }
        //}

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Globals.Initialize();
            InputManager.Initialize();            
            ScreenManager.ScreenManagerInit(graphics, mapGenContentManager, this);
            CharacterLoader.Initialize(contentMan);
            base.Initialize();

            ScreenManager.AddScreen(new MainMenuStartScreen(Color.Blue, Fonts.normFont, VPCenter));         
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //normalmapEffect = Content.Load<Effect>("normalmap");
            Fonts.LoadContent(mapGenContentManager);
            ScreenManager.LoadContent();
            base.LoadContent();
            //MapEngine.LoadContent(mapGenContentManager);
            //InfoWriter.LoadContent();

            //
            //screenManager.AddScreen(new TempStartScreen(Color.Blue, Fonts.normFont, new Vector2(600, 400)));
            //MapEngine.NewMap();
            //MapEngine.LoadMap();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            Fonts.UnloadContent();
            MapEngine.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            InputManager.Update(gameTime);
            ScreenManager.Update(gameTime);
            // TODO: move these to an input manager class
            //elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            //oldkey = newKey;
            //newKey = Keyboard.GetState();

            //if (newKey.IsKeyDown(Keys.Escape))
            //    Exit();

            //// reset map
            //if ((newKey.IsKeyDown(Keys.Enter)) && elapsedTime > millisecondsBetweenInput)
            //{
            //    MapEngine.MapEngineFirstRoom();
            //    elapsedTime = 0;
            //}

            //// move mapEngineViewport up
            //if ((newKey.IsKeyDown(Keys.Up)) && elapsedTime > millisecondsBetweenInput)
            //{
            //    MapEngine.SetMapEngineViewportLocation(0, -55);
            //    elapsedTime = 0;
            //}
            //// move mapEngineViewport right
            //if ((newKey.IsKeyDown(Keys.Right)) && elapsedTime > millisecondsBetweenInput)
            //{
            //    MapEngine.SetMapEngineViewportLocation(55, 0);
            //    elapsedTime = 0;
            //}
            //// move mapEngineViewport down
            //if ((newKey.IsKeyDown(Keys.Down)) && elapsedTime > millisecondsBetweenInput)
            //{
            //    MapEngine.SetMapEngineViewportLocation(0, 55);
            //    elapsedTime = 0;
            //}
            //// move mapEngineViewport left
            //if ((newKey.IsKeyDown(Keys.Left)) && elapsedTime > millisecondsBetweenInput)
            //{
            //    MapEngine.SetMapEngineViewportLocation(-55, 0);
            //    elapsedTime = 0;
            //}

            //if ((newKey.IsKeyDown(Keys.Space)) && elapsedTime > millisecondsBetweenInput)
            //{
            //    MapEngine.ShowBlankStates = true;
            //    elapsedTime = 0;
            //}

            //// save
            //if ((newKey.IsKeyDown(Keys.S)) && oldkey.IsKeyUp(Keys.S))
            //{
            //    MapEngine.SaveMap();
            //}

            //// load
            //if ((newKey.IsKeyDown(Keys.L)) && oldkey.IsKeyUp(Keys.L))
            //{
            //    MapEngine.LoadMap();
            //}

            //// new
            //if ((newKey.IsKeyDown(Keys.N)) && oldkey.IsKeyUp(Keys.N))
            //{
            //    MapEngine.NewMap();
            //    MapEngine.LoadMap();
            //}

            //if (!MapEngine.IsMapNull())
            //    MapEngine.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            ScreenManager.Draw(gameTime);

            //spriteBatch.Begin();

            //if (!MapEngine.IsMapNull())
            //    MapEngine.Draw(spriteBatch);

            //spriteBatch.End();

            //// draw altitude tiles
            //if (!MapEngine.IsMapNull())
            //    MapEngine.Draw(spriteBatch, ref graphics, ref normalmapEffect);

            base.Draw(gameTime);
        }
    }
}
