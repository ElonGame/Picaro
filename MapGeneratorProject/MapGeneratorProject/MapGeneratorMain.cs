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

namespace MapGeneratorProject
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MapGeneratorMain : Microsoft.Xna.Framework.Game
    {
        //static public string contentRoot;

        //GraphicsDeviceManager graphics;
        //ContentManager mapGenContentManager;
        //SpriteBatch spriteBatch;

        //// temp
        //Point startDrawPosBG;
        //int leftHUDwidth = 550;
        //int rightBGwidth = 880;
        //int screenDrawHeight = 880;

        //Texture2D tempHUD;
        //Texture2D bgRect;
        //Rectangle leftBGRectDrawRect;
        //Rectangle rightBGRectDrawRect;
        //Rectangle tileRect;

        //SpriteFont tempFont;
        ////

        //KeyboardState oldkey;
        //KeyboardState newKey;

        //Effect normalmapEffect;

        //int elapsedTime;
        //int millisecondsBetweenInput;

        public MapGeneratorMain()
        {
            //graphics = new GraphicsDeviceManager(this);
            //mapGenContentManager = this.Content;
            //graphics.PreferredBackBufferWidth = 1600;
            //graphics.PreferredBackBufferHeight = 900;
            //graphics.IsFullScreen = true;
            //Content.RootDirectory = "Content";
            //contentRoot = Content.RootDirectory;

            //elapsedTime = 0;
            //millisecondsBetweenInput = 100;

            //startDrawPosBG = new Point(85, 10);
            //leftBGRectDrawRect = new Rectangle(startDrawPosBG.X, startDrawPosBG.Y, leftHUDwidth, screenDrawHeight);
            //rightBGRectDrawRect = new Rectangle(startDrawPosBG.X + leftHUDwidth, startDrawPosBG.Y, rightBGwidth, screenDrawHeight);
            //tileRect = new Rectangle(0, 0, 55, 55);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //TileSetManager.Initialize();
            //TextureManager.Initialize(mapGenContentManager);
            //MapGenerator.Initialize();
            //RoomMaker.initialize();
            //HallMaker.initialize();
            //Globals.Initialize();
            //PathFinder.Initialize();

            //MapEngine.Initialize(mapGenContentManager);
            //MapEngine.MapEngineViewport = graphics.GraphicsDevice.Viewport;
            //MapEngine.MinMapEngineViewportBounds = new Point(550 + 85, 10); // 635, 10
            //MapEngine.SetMapEngineViewportDimensions(880, 880);

            //this.IsMouseVisible = true;

            //base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            ///spriteBatch = new SpriteBatch(GraphicsDevice);

            //normalmapEffect = Content.Load<Effect>("normalmap");

            //tempMapTex = mapGenContentManager.Load<Texture2D>(@"MapTextures\map1_temp");
            //bgRect = mapGenContentManager.Load<Texture2D>(@"MapTextures\blankBG");
            //tempHUD = mapGenContentManager.Load<Texture2D>(@"Misc\HUD_game_screen_temp");
            //tempFont = mapGenContentManager.Load<SpriteFont>(@"Fonts\temp_font_v3");

            //InfoWriter.Initialize(ref tempFont);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //// TODO: move these to an input manager class
            //elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            //oldkey = newKey;
            //newKey = Keyboard.GetState();

            //if (newKey.IsKeyDown(Keys.Escape))
            //    Exit();

            //// reset map
            //if ((newKey.IsKeyDown(Keys.Enter)) && elapsedTime > millisecondsBetweenInput)
            //{
            //    MapEngine.MapEngineFirstRoom();
            //    //mapEngine.ResetMap();
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

            //if ((newKey.IsKeyDown(Keys.S)) && elapsedTime > millisecondsBetweenInput)
            //{
            //    string testStr = "test.xml";
            //    MapLoader.MapToSavedMap(ref MapEngine.currMap, ref testStr);
            //    MapEngine.ClearMap();
            //}

            //if ((newKey.IsKeyDown(Keys.L)) && elapsedTime > millisecondsBetweenInput)
            //{
            //    string testStr = "test.xml";
            //    MapEngine.LoadMap(ref testStr);
            //}


            //if(!MapEngine.IsMapNull())
            //    MapEngine.Update(gameTime);

            //base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.Black);
        
            //spriteBatch.Begin();

            //spriteBatch.Draw(bgRect, leftBGRectDrawRect, Color.DarkGray);
            //spriteBatch.Draw(tempHUD, leftBGRectDrawRect, Color.White);
            //spriteBatch.Draw(bgRect, rightBGRectDrawRect, tileRect, Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 1f);

            //if (!MapEngine.IsMapNull())
            //{
            //    MapEngine.Draw(spriteBatch);
            //    InfoWriter.Draw(spriteBatch);
            //}

            //spriteBatch.End();          

            //// draw altitude tiles
            //if (!MapEngine.IsMapNull())
            //    MapEngine.Draw(spriteBatch, ref graphics, ref normalmapEffect);

            //base.Draw(gameTime);
        }
    }
}
