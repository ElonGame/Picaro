using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameTutorial_05_26MAR14
{
    static public class ScreenManager// : DrawableGameComponent
    {
        #region Fields

        static List<GameScreen> screens = new List<GameScreen>();
        static List<GameScreen> screensToUpdate = new List<GameScreen>();

        // A default SpriteBatch shared by all the screens. This saves
        // each screen having to bother creating their own local instance.
        static public SpriteBatch spriteBatch;
        static public GraphicsDeviceManager graphicDevMan;
        static public ContentManager contentMan;
        static public Game thisGame;

        static bool isInitialized;
        static bool traceEnabled;

        #endregion


        #region Properties

        /// <summary>
        /// If true, the manager prints out a list of all the screens
        /// each time it is updated. This can be useful for making sure
        /// everything is being added and removed at the right times.
        /// </summary>
        static public bool TraceEnabled
        {
            get { return traceEnabled; }
            set { traceEnabled = value; }
        }


        #endregion


        #region Initialization
        // constructor
        static public void ScreenManagerInit(GraphicsDeviceManager gdman, ContentManager cman, Game game)//, Game game)// : base(game)
        {
            graphicDevMan = gdman;
            contentMan = cman;
            thisGame = game;
            isInitialized = true;
        }
        //static public override void Initialize()
        static public void Initialize()
        {

            //base.Initialize();
            //isInitialized = true;
        }
        //static protected override void LoadContent()
        static public void LoadContent()
        {
            //ContentManager content = contentMan.//Game.Content; // Load content belonging to the screen manager.
            spriteBatch = new SpriteBatch(graphicDevMan.GraphicsDevice);

            foreach (GameScreen screen in screens) // Tell each of the screens to load their content.
                screen.LoadContent();
        }
        //static protected override void UnloadContent()
        static public void UnloadContent()
        {
            foreach (GameScreen screen in screens) // Tell each of the screens to unload their content.
                screen.UnloadContent();
        }

        #endregion


        #region Update and Draw
        //static public override void Update(GameTime gameTime)
        static public void Update(GameTime gameTime)
        {
            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others.
            screensToUpdate.Clear();

            foreach (GameScreen screen in screens)
                screensToUpdate.Add(screen);

            bool otherScreenHasFocus = !thisGame.IsActive; // cheat: set to false
            bool coveredByOtherScreen = false;

            // Loop as long as there are screens waiting to be updated.
            while (screensToUpdate.Count > 0)
            {
                // Pop the topmost screen off the waiting list.
                GameScreen screen = screensToUpdate[screensToUpdate.Count - 1];

                screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

                // Update the screen.
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.screenState == ScreenState.TransitionOn || screen.screenState == ScreenState.Active)
                {
                    // If this is the first active screen we came across,
                    // give it a chance to handle input.
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput();
                        otherScreenHasFocus = true;
                    }

                    // If this is an active non-popup, inform any subsequent
                    // screens that they are covered by it.
                    if (!screen.isPopup)
                        coveredByOtherScreen = true;
                }
            }

            // Print debug trace?
            if (traceEnabled)
                TraceScreens();
        }


        /// <summary>
        /// Prints a list of all the screens, for debugging.
        /// </summary>
        static public void TraceScreens()
        {
            List<string> screenNames = new List<string>();

            foreach (GameScreen screen in screens)
                screenNames.Add(screen.GetType().Name);

#if WINDOWS
            //Trace.WriteLine(string.Join(", ", screenNames.ToArray()));
#endif
        }
        //static public override void Draw(GameTime gameTime)
        static public void Draw(GameTime gameTime)
        {         
            foreach (GameScreen screen in screens)
            {
                if (screen.screenState == ScreenState.Hidden)
                    continue;
                screen.Draw(gameTime);
            }
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Adds a new screen to the screen manager.
        /// </summary>
        static public void AddScreen(GameScreen screen)
        {
            //screen.ScreenManager = this;
            screen.IsExiting = false;

            if (isInitialized) // If we have a graphics device, tell the screen to load content.
                screen.LoadContent();

            screens.Add(screen);
        }

        /// <summary>
        /// Removes a screen from the screen manager. You should normally
        /// use GameScreen.ExitScreen instead of calling this directly, so
        /// the screen can gradually transition off rather than just being
        /// instantly removed.
        /// </summary>
        static public void RemoveScreen(GameScreen screen)
        {
            if (isInitialized) // If we have a graphics device, tell the screen to unload content.
                screen.UnloadContent();

            screens.Remove(screen);
            screensToUpdate.Remove(screen);
        }


        /// <summary>
        /// Expose an array holding all the screens. We return a copy rather
        /// than the real master list, because screens should only ever be added
        /// or removed using the AddScreen and RemoveScreen methods.
        /// </summary>
        static public GameScreen[] GetScreens()
        {
            return screens.ToArray();
        }

        #endregion
    }
}
