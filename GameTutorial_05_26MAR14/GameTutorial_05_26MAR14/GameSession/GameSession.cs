using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using LibraryProject;

namespace GameTutorial_05_26MAR14
{
    class GameSession
    {
        public static GameSession singleton;
        public static Player curPlayer;

        bool mapLoaded = false;

        Effect normalmapEffect;

        /// <summary>
        /// Private constructor of a Session object.
        /// </summary>
        /// <remarks>
        /// The lack of public constructors forces the singleton model.
        /// </remarks>
        private GameSession()//ScreenManager screenManager, TempGamePlayScreen gameplayScreen)
        {
            MapEngine.Initialize(ScreenManager.graphicDevMan);
            InfoWriter.Initialize();
            
            // check the parameter
            //if (screenManager == null)
            //{
            //    throw new ArgumentNullException("screenManager");
            //}
            //if (gameplayScreen == null)
            //{
            //    throw new ArgumentNullException("gameplayScreen");
            //}

            // assign the parameter
            //this.screenManager = screenManager;
            //this.gameplayScreen = gameplayScreen;

            // create the HUD interface
            //this.hud = new Hud(screenManager);
            //this.hud.LoadContent();
        }

        private void LoadContent(ref ContentManager contentMan)
        {
            normalmapEffect = ScreenManager.contentMan.Load<Effect>("normalmap");

            MapEngine.LoadContent(ScreenManager.contentMan);
            InfoWriter.LoadContent();

            MapEngine.NewMap();
            MapEngine.LoadMap();
            mapLoaded = true;

            curPlayer = new Player(ref ScreenManager.contentMan, "test_hero", MapEngine.currMap.startPoint, Directions.S,
                CharacterLoader.LoadCharacter(ref ScreenManager.contentMan, "test_hero", "Player"));
            curPlayer.MapSprite.texture = ScreenManager.contentMan.Load<Texture2D>(curPlayer.MapSprite.textureName);
            curPlayer.MapSprite.FrameDimensions = new Point(55, 55);
            curPlayer.MapSprite.ResetAnimation();          
        }

        /// <summary>
        /// Start a new session based on the data provided.
        /// </summary>
        //public static void StartNewSession(GameStartDescription gameStartDescription, ScreenManager screenManager, GameplayScreen gameplayScreen)
        static public void StartNewSession(ref ContentManager contentMan)
        {
            // check the parameters
            //if (gameStartDescription == null)
            //{
            //    throw new ArgumentNullException("gameStartDescripton");
            //}
            //if (screenManager == null)
            //{
            //    throw new ArgumentNullException("screenManager");
            //}
            //if (gameplayScreen == null)
            //{
            //    throw new ArgumentNullException("gameplayScreen");
            //}

            // end any existing session
            //EndSession();

            // create a new singleton
            //singleton = new Session(screenManager, gameplayScreen);
            singleton = new GameSession();
            singleton.LoadContent(ref contentMan);

            // set up the initial map
            //ChangeMap(gameStartDescription.MapContentName, null);

            //// set up the initial party
            //ContentManager content = singleton.screenManager.Game.Content;
            //singleton.party = new Party(gameStartDescription, content);

            //// load the quest line
            //singleton.questLine = content.Load<QuestLine>(
            //    Path.Combine(@"Quests\QuestLines",
            //    gameStartDescription.QuestLineContentName)).Clone() as QuestLine;
        }

        static public void Update(GameTime gt)
        {
            if (singleton.mapLoaded)
                MapEngine.Update(gt);

            curPlayer.Update(gt);
        }

        static public void Draw(GameTime gameTime)
        {
            //SpriteBatch sb = ScreenManager.spriteBatch;

            //sb.Begin();
            //if (singleton.mapLoaded)
            //    MapEngine.Draw(sb);
            //sb.End();

            //// draw altitude tiles
            ////if (singleton.mapLoaded)
            ////    MapEngine.Draw(sb, ref ScreenManager.graphicDevMan, ref singleton.normalmapEffect);

            //sb.Begin();
            //curPlayer.Draw(ref sb, ref MapEngine.mEngVportWLoc);
            //sb.End();
        }
    }
}
