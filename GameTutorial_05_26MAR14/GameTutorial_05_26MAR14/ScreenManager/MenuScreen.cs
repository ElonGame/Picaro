using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LibraryProject;

namespace GameTutorial_05_26MAR14
{
    /// <summary>
    /// Base class for screens that contain a menu of options. The user can
    /// move up and down to select an entry, or cancel to back out of the screen.
    /// </summary>
    /// <remarks>
    /// Similar to a class found in the Game State Management sample on the 
    /// XNA Creators Club Online website (http://creators.xna.com).
    /// </remarks>
    abstract class MenuScreen : GameScreen
    {
        #region Fields

        public Color titleColor;
        public Color entriesColor;
        public Color selectColor;
        public SpriteFont spriteFont;

        public Vector2 position;

        List<MenuEntry> menuEntries = new List<MenuEntry>();
        protected int selectedEntry = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of menu entries, so derived classes can add
        /// or change the menu contents.
        /// </summary>
        protected IList<MenuEntry> MenuEntries
        {
            get { return menuEntries; }
        }

        protected MenuEntry SelectedMenuEntry
        {
            get
            {
                if ((selectedEntry < 0) || (selectedEntry >= menuEntries.Count))
                    return null;
                return menuEntries[selectedEntry];
            }
        }

        #endregion


        #region Initialization

        public MenuScreen(Color title_color, SpriteFont sf, Vector2 start_positiion)//, ref ScreenManager sm)
        {
            //base.Initialize(ref sm);

            transOnTime = TimeSpan.FromSeconds(0.5);
            transOffTime = TimeSpan.FromSeconds(0.5);

            titleColor = title_color;
            entriesColor = new Color(titleColor.R + Globals.menuEntriesColorShift,
                titleColor.G + Globals.menuEntriesColorShift,
                titleColor.B + Globals.menuEntriesColorShift,
                titleColor.A + Globals.menuEntriesColorShift);
            selectColor = new Color(titleColor.R + Globals.selectedEntryColorShift,
                titleColor.G + Globals.selectedEntryColorShift,
                titleColor.B + Globals.selectedEntryColorShift,
                titleColor.A + Globals.selectedEntryColorShift);

            spriteFont = sf;
            position = start_positiion;
        }


        #endregion

        #region Handle Input

        /// <summary>
        /// Responds to user input, changing the selected entry and accepting
        /// or cancelling the menu.
        /// </summary>
        public override void HandleInput()
        {
            int oldSelectedEntry = selectedEntry;

            // Move to the previous menu entry?
            if (InputManager.IsActionTriggered(InputManager.Action.CursorUp))
            {
                selectedEntry--;
                if (selectedEntry < 0)
                    selectedEntry = menuEntries.Count - 1;
            }

            // Move to the next menu entry?
            if (InputManager.IsActionTriggered(InputManager.Action.CursorDown))
            {
                selectedEntry++;
                if (selectedEntry >= menuEntries.Count)
                    selectedEntry = 0;
            }

            // Accept or cancel the menu?
            if (InputManager.IsActionTriggered(InputManager.Action.Ok))
            {
                //AudioManager.PlayCue("Continue");
                OnSelectEntry(selectedEntry);
            }
            else if (InputManager.IsActionTriggered(InputManager.Action.Back) ||
                InputManager.IsActionTriggered(InputManager.Action.ExitGame))
            {
                OnCancel();
            }
            else if (selectedEntry != oldSelectedEntry)
            {
                //AudioManager.PlayCue("MenuMove");
            }
        }

        /// <summary>
        /// Handler for when the user has chosen a menu entry.
        /// </summary>
        protected virtual void OnSelectEntry(int entryIndex)
        {
            menuEntries[selectedEntry].OnSelectEntry();
        }

        /// <summary>
        /// Handler for when the user has cancelled the menu.
        /// </summary>
        protected virtual void OnCancel()
        {
            ExitScreen();
        }

        /// <summary>
        /// Helper overload makes it easy to use OnCancel as a MenuEntry event handler.
        /// </summary>
        protected void OnCancel(object sender, EventArgs e)
        {
            OnCancel();
        }


        #endregion


        #region Update and Draw

        /// <summary>
        /// Updates the menu.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            for (int i = 0; i < menuEntries.Count; i++) // Update each nested MenuEntry object.
            {
                bool isSelected = IsActive && (i == selectedEntry);
                menuEntries[i].Update(this, isSelected, gameTime);
            }
        }


        /// <summary>
        /// Draws the menu.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.spriteBatch;

            spriteBatch.Begin();

            for (int i = 0; i < menuEntries.Count; i++) // Draw each menu entry in turn
            {
                MenuEntry menuEntry = menuEntries[i];
                bool isSelected = IsActive && (i == selectedEntry);
                menuEntry.Draw(this, isSelected, gameTime, false);
            }

            spriteBatch.End();
        }

        #endregion
    }
}
