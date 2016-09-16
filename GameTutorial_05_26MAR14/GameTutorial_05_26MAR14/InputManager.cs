using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameTutorial_05_26MAR14
{
    /// <summary>
    /// This class handles all keyboard and gamepad actions in the game.
    /// </summary>
    static public class InputManager
    {
        #region Action Enumeration

        static int elapsedTime;

        /// <summary>
        /// The actions that are possible within the game.
        /// </summary>
        public enum Action
        {
            MainMenu,
            Ok,
            Back,
            ExitGame,
            MoveCharN,
            MoveCharNE,
            MoveCharE,
            MoveCharSE,
            MoveCharS,
            MoveCharSW,
            MoveCharW,
            MoveCharNW,
            CursorUp,
            CursorDown,
            DecreaseAmount,
            IncreaseAmount,
            PageLeft,
            PageRight,
            TargetUp,
            TargetDown,
            Save,
            Load,
            New,
            ShowDebug,
            SpritePrev,
            SpriteNext,
            TotalActionCount,
        }

        // Removed:
        // CharacterManagement
        // TakeView
        // DropUnEquip,

        /// <summary>
        /// Readable names of each action.
        /// </summary>
        private static readonly string[] actionNames = 
            {
                "Main Menu",
                "Ok",
                "Back",
                "Exit Game",
                "Move Character - Up",
                "Move Character - Up + Right",
                "Move Character - Right",
                "Move Character - Down + Right",
                "Move Character - Down",
                "Move Character - Down + Left",
                "Move Character - Left",
                "Move Character - Up + Left",
                "Move Cursor - Up",
                "Move Cursor - Down",
                "Decrease Amount",
                "Increase Amount",
                "Page Screen Left",
                "Page Screen Right",
                "Select Target - Up",
                "Select Target - Down",
                "Save Map",
                "Load Map",
                "New Map",
                "Show Debug",
                "SpritePrev",
                "SpriteNext"
            };

        // Removed:
        // "Character Management"
        // "Take / View"
        // "Drop / Unequip"
        
        /// <summary>
        /// Returns the readable name of the given action.
        /// </summary>
        static public string GetActionName(Action action)
        {
            int index = (int)action;

            if ((index < 0) || (index > actionNames.Length))
                throw new ArgumentException("action");

            return actionNames[index];
        }


        #endregion

        #region Support Types

        /// <summary>
        /// A combination of gamepad and keyboard keys mapped to a particular action.
        /// </summary>
        public class ActionMap
        {
            /// <summary>
            /// List of Keyboard controls to be mapped to a given action.
            /// </summary>
            public List<Keys> keyboardKeys = new List<Keys>();
        }


        #endregion

        #region Keyboard Data

        /// <summary>
        /// The state of the keyboard as of the last update.
        /// </summary>
        private static KeyboardState currentKeyboardState;

        /// <summary>
        /// The state of the keyboard as of the last update.
        /// </summary>
        static public KeyboardState CurrentKeyboardState
        {
            get { return currentKeyboardState; }
        }


        /// <summary>
        /// The state of the keyboard as of the previous update.
        /// </summary>
        private static KeyboardState previousKeyboardState;


        /// <summary>
        /// Check if a key is pressed.
        /// </summary>
        static public bool IsKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        static public bool IsKeyPressedTimed(Keys key, int ms_delay)
        {
            if (elapsedTime > ms_delay)
                if (currentKeyboardState.IsKeyDown(key))
                {
                    elapsedTime = 0;
                    return true;
                }
            return false;
        }

        /// <summary>
        /// Check if a key was just pressed in the most recent update.
        /// </summary>
        static public bool IsKeyTriggered(Keys key)
        {
            return (currentKeyboardState.IsKeyDown(key)) && (!previousKeyboardState.IsKeyDown(key));
        }

        #endregion

        #region Action Mapping


        /// <summary>
        /// The action mappings for the game.
        /// </summary>
        private static ActionMap[] actionMaps;


        static public ActionMap[] ActionMaps
        {
            get { return actionMaps; }
        }


        /// <summary>
        /// Reset the action maps to their default values.
        /// </summary>
        private static void ResetActionMaps()
        {
            actionMaps = new ActionMap[(int)Action.TotalActionCount];

            actionMaps[(int)Action.MainMenu] = new ActionMap();
            actionMaps[(int)Action.MainMenu].keyboardKeys.Add(Keys.Tab);

            actionMaps[(int)Action.Ok] = new ActionMap();
            actionMaps[(int)Action.Ok].keyboardKeys.Add(Keys.Space);
                //Keys.Enter);

            actionMaps[(int)Action.Back] = new ActionMap();
            actionMaps[(int)Action.Back].keyboardKeys.Add(Keys.LeftControl);

            //actionMaps[(int)Action.CharacterManagement] = new ActionMap();
            //actionMaps[(int)Action.CharacterManagement].keyboardKeys.Add(
            //    Keys.Space);

            actionMaps[(int)Action.ExitGame] = new ActionMap();
            actionMaps[(int)Action.ExitGame].keyboardKeys.Add(Keys.Escape);

            //actionMaps[(int)Action.TakeView] = new ActionMap();
            //actionMaps[(int)Action.TakeView].keyboardKeys.Add(
            //    Keys.LeftControl);

            //actionMaps[(int)Action.DropUnEquip] = new ActionMap();
            //actionMaps[(int)Action.DropUnEquip].keyboardKeys.Add(
            //    Keys.D);

            actionMaps[(int)Action.MoveCharN] = new ActionMap();
            actionMaps[(int)Action.MoveCharN].keyboardKeys.Add(Keys.W);

            actionMaps[(int)Action.MoveCharNE] = new ActionMap();
            actionMaps[(int)Action.MoveCharNE].keyboardKeys.Add(Keys.E);

            actionMaps[(int)Action.MoveCharE] = new ActionMap();
            actionMaps[(int)Action.MoveCharE].keyboardKeys.Add(Keys.D);

            actionMaps[(int)Action.MoveCharSE] = new ActionMap();
            actionMaps[(int)Action.MoveCharSE].keyboardKeys.Add(Keys.C);

            actionMaps[(int)Action.MoveCharS] = new ActionMap();
            actionMaps[(int)Action.MoveCharS].keyboardKeys.Add(Keys.S);

            actionMaps[(int)Action.MoveCharSW] = new ActionMap();
            actionMaps[(int)Action.MoveCharSW].keyboardKeys.Add(Keys.Z);

            actionMaps[(int)Action.MoveCharW] = new ActionMap();
            actionMaps[(int)Action.MoveCharW].keyboardKeys.Add(Keys.A);

            actionMaps[(int)Action.MoveCharNW] = new ActionMap();
            actionMaps[(int)Action.MoveCharNW].keyboardKeys.Add(Keys.Q);

            actionMaps[(int)Action.CursorUp] = new ActionMap();
            actionMaps[(int)Action.CursorUp].keyboardKeys.Add(Keys.Up);

            actionMaps[(int)Action.CursorDown] = new ActionMap();
            actionMaps[(int)Action.CursorDown].keyboardKeys.Add(Keys.Down);

            actionMaps[(int)Action.DecreaseAmount] = new ActionMap();
            actionMaps[(int)Action.DecreaseAmount].keyboardKeys.Add(Keys.Left);

            actionMaps[(int)Action.IncreaseAmount] = new ActionMap();
            actionMaps[(int)Action.IncreaseAmount].keyboardKeys.Add(Keys.Right);

            actionMaps[(int)Action.PageLeft] = new ActionMap();
            actionMaps[(int)Action.PageLeft].keyboardKeys.Add( Keys.Left);

            actionMaps[(int)Action.PageRight] = new ActionMap();
            actionMaps[(int)Action.PageRight].keyboardKeys.Add(Keys.Right);

            actionMaps[(int)Action.TargetUp] = new ActionMap();
            actionMaps[(int)Action.TargetUp].keyboardKeys.Add(Keys.Up);

            actionMaps[(int)Action.TargetDown] = new ActionMap();
            actionMaps[(int)Action.TargetDown].keyboardKeys.Add(Keys.Down);

            actionMaps[(int)Action.Save] = new ActionMap();
            actionMaps[(int)Action.Save].keyboardKeys.Add(Keys.V);

            actionMaps[(int)Action.Load] = new ActionMap();
            actionMaps[(int)Action.Load].keyboardKeys.Add(Keys.O);

            actionMaps[(int)Action.New] = new ActionMap();
            actionMaps[(int)Action.New].keyboardKeys.Add(Keys.N);

            actionMaps[(int)Action.ShowDebug] = new ActionMap();
            actionMaps[(int)Action.ShowDebug].keyboardKeys.Add(Keys.Y);

            actionMaps[(int)Action.SpritePrev] = new ActionMap();
            actionMaps[(int)Action.SpritePrev].keyboardKeys.Add(Keys.N);

            actionMaps[(int)Action.SpriteNext] = new ActionMap();
            actionMaps[(int)Action.SpriteNext].keyboardKeys.Add(Keys.Y);
        }


        /// <summary>
        /// Check if an action has been pressed.
        /// </summary>
        static public bool IsActionPressed(Action action)
        {
            return IsActionMapPressed(actionMaps[(int)action]);
        }

        static public bool IsActionPressedTimed(Action action, int ms_delay)
        {
            return IsActionMapPressedTime(actionMaps[(int)action], ms_delay);
        }

        /// <summary>
        /// Check if an action was just performed in the most recent update.
        /// </summary>
        static public bool IsActionTriggered(Action action)
        {
            return IsActionMapTriggered(actionMaps[(int)action]);
        }

        /// <summary>
        /// Check if an action map has been pressed.
        /// </summary>
        private static bool IsActionMapPressed(ActionMap actionMap)
        {
            for (int i = 0; i < actionMap.keyboardKeys.Count; i++)
                if (IsKeyPressed(actionMap.keyboardKeys[i]))
                    return true;

            return false;
        }

        /// <summary>
        /// Check if an action map has been triggered this frame.
        /// </summary>
        private static bool IsActionMapPressedTime(ActionMap actionMap, int ms_delay)
        {
            for (int i = 0; i < actionMap.keyboardKeys.Count; i++)
                if (IsKeyPressedTimed(actionMap.keyboardKeys[i], ms_delay))
                    return true;

            return false;
        }

        /// <summary>
        /// Check if an action map has been triggered this frame.
        /// </summary>
        private static bool IsActionMapTriggered(ActionMap actionMap)
        {
            for (int i = 0; i < actionMap.keyboardKeys.Count; i++)
                if (IsKeyTriggered(actionMap.keyboardKeys[i]))
                    return true;

            return false;
        }
        #endregion


        #region Initialization


        /// <summary>
        /// Initializes the default control keys for all actions.
        /// </summary>
        static public void Initialize()
        {
            elapsedTime = 0;
            ResetActionMaps();
        }

        #endregion


        #region Updating


        /// <summary>
        /// Updates the keyboard and gamepad control states.
        /// </summary>
        static public void Update(GameTime gt)
        {
            elapsedTime += gt.ElapsedGameTime.Milliseconds;
            // update the keyboard state
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
        }


        #endregion
    }
}
