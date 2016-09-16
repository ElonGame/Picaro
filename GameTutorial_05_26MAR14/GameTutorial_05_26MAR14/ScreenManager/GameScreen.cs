using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameTutorial_05_26MAR14
{
    public enum ScreenState
    { TransitionOn, Active, TransitionOff, Hidden, }

    public abstract class GameScreen
    {
        public bool isPopup = false;
        public TimeSpan transOnTime = TimeSpan.Zero;
        public TimeSpan transOffTime = TimeSpan.Zero;
        public float transPos = 1;

        bool isExiting = false;
        bool otherScreenHasFocus;

        //ScreenManager screenManager;

        /// <summary>
        /// Gets the manager that this screen belongs to.
        /// </summary>
        //public ScreenManager ScreenManager
        //{
        //    get { return screenManager; }
        //    internal set { screenManager = value; }
        //}

        public event EventHandler Exiting;

        public byte TransitionAlpha
        {
            get { return (byte)(255 - transPos * 255); }
        }

        public ScreenState screenState = ScreenState.TransitionOn;

        public bool IsExiting
        {
            get { return isExiting; }
            protected internal set
            {
                bool fireEvent = !isExiting && value;
                isExiting = value;

                if (fireEvent && (Exiting != null))
                    Exiting(this, EventArgs.Empty);
            }
        }

        public bool IsActive
        {
            get
            {
                return !otherScreenHasFocus && (screenState == ScreenState.TransitionOn || screenState == ScreenState.Active);
            }
        }

        #region Initialization

        public virtual void Initialize()//ref ScreenManager sm)
        {
            //screenManager = sm;
        }

        public virtual void LoadContent() { }

        public virtual void UnloadContent() { }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Allows the screen to run logic, such as updating the transition position.
        /// Unlike HandleInput, this method is called regardless of whether the screen
        /// is active, hidden, or in the middle of a transition.
        /// </summary>
        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            this.otherScreenHasFocus = otherScreenHasFocus;

            if (IsExiting)
            {
                // If the screen is going away to die, it should transition off.
                screenState = ScreenState.TransitionOff;

                // When the transition finishes, remove the screen.
                if (!UpdateTransition(gameTime, transOffTime, 1))
                    ScreenManager.RemoveScreen(this);

            }
            else if (coveredByOtherScreen)
            {
                // If the screen is covered by another, it should transition off.
                if (UpdateTransition(gameTime, transOffTime, 1))
                    screenState = ScreenState.TransitionOff; // Still busy transitioning.
                else
                    screenState = ScreenState.Hidden; // Transition finished!
            }
            else
            {
                // Otherwise the screen should transition on and become active.
                if (UpdateTransition(gameTime, transOnTime, -1))
                    screenState = ScreenState.TransitionOn; // Still busy transitioning.
                else
                    screenState = ScreenState.Active; // Transition finished!
            }
        }


        /// <summary>
        /// Helper for updating the screen transition position.
        /// </summary>
        bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
        {
            float transitionDelta;                   // How much should we move by?

            if (time == TimeSpan.Zero)
                transitionDelta = 1;
            else
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / time.TotalMilliseconds);

            transPos += transitionDelta * direction; // Update the transition position.

            if ((transPos <= 0) || (transPos >= 1)) // Did we reach the end of the transition?
            {
                transPos = MathHelper.Clamp(transPos, 0, 1);
                return false;
            }
            return true;                            // Otherwise we are still busy transitioning.
        }


        /// <summary>
        /// Allows the screen to handle user input. Unlike Update, this method
        /// is only called when the screen is active, and not when some other
        /// screen has taken the focus.
        /// </summary>
        public virtual void HandleInput() { }


        /// <summary>
        /// This is called when the screen should draw itself.
        /// </summary>
        public virtual void Draw(GameTime gameTime) { }


        #endregion

        #region Public Methods

        /// <summary>
        /// Tells the screen to go away. Unlike ScreenManager.RemoveScreen, which
        /// instantly kills the screen, this method respects the transition timings
        /// and will give the screen a chance to gradually transition off.
        /// </summary>
        public void ExitScreen()
        {
            IsExiting = true;                  // flag that it should transition off and then exit.
            if (transOffTime == TimeSpan.Zero) // If the screen has a zero transition time, remove it immediately.
                ScreenManager.RemoveScreen(this);
        }

        #endregion
    }    
}
