using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameTutorial_05_26MAR14
{
    /// <summary>
    /// Helper class represents a single entry in a MenuScreen. By default this
    /// just draws the entry text string, but it can be customized to display menu
    /// entries in different ways. This also provides an event that will be raised
    /// when the menu entry is selected.
    /// </summary>
    /// <remarks>
    /// Similar to a class found in the Game State Management sample on the 
    /// XNA Creators Club Online website (http://creators.xna.com).
    /// </remarks>
    class MenuEntry
    {
        #region Fields

        public string text; // The text rendered for this entry.      
        //public SpriteFont spriteFont; // The font used for this menu item.
        public Vector2 position; // The position of this menu item on the screen.

        public string description; // A description of the function of the button.
        public Texture2D texture; // An optional texture drawn with the text, If present, the text will be centered on the texture

        #endregion

        #region Events

        /// <summary>
        /// Event raised when the menu entry is selected.
        /// </summary>
        public event EventHandler<EventArgs> Selected;

        /// <summary>
        /// Method for raising the Selected event.
        /// </summary>
        protected internal virtual void OnSelectEntry()
        {
            if (Selected != null)
                Selected(this, EventArgs.Empty);
        }

        #endregion

        #region Initialization

        public MenuEntry(string text)
        {
            this.text = text;
        }

        #endregion


        #region Update and Draw

        public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
        { }


        /// <summary>
        /// Draws the menu entry. This can be overridden to customize the appearance.
        /// </summary>
        public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime, bool centered)
        {
            // Draw the selected entry in yellow, otherwise white.
            //Color color = isSelected ? Fonts.MenuSelectedColor : Fonts.TitleColor;
            Color color = isSelected ? screen.selectColor : screen.entriesColor;

            // Draw text, centered on the middle of each line.
            //ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = ScreenManager.spriteBatch;

            if (texture != null)
            {
                spriteBatch.Draw(texture, position, Color.White);
                if ((screen.spriteFont != null) && !String.IsNullOrEmpty(text))
                {
                    Vector2 textSize = screen.spriteFont.MeasureString(text);
                    Vector2 textPosition = position + new Vector2(
                        (float)Math.Floor((texture.Width - textSize.X) / 2),
                        (float)Math.Floor((texture.Height - textSize.Y) / 2));
                    spriteBatch.DrawString(screen.spriteFont, text, textPosition, color);
                }
            }
            else if(centered)
            {
                Fonts.DrawCenteredText(ref spriteBatch, screen.spriteFont, text, position, color);
            }
            else if ((screen.spriteFont != null) && !String.IsNullOrEmpty(text))
                spriteBatch.DrawString(screen.spriteFont, text, position, color);
        }

        /// <summary>
        /// Queries how much space this menu entry requires.
        /// </summary>
        public virtual int GetHeight(MenuScreen screen)
        {
            return screen.spriteFont.LineSpacing;
        }

        #endregion
    }
}
