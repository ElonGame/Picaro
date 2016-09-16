using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace GameTutorial_05_26MAR14
{
    static public class Fonts
    {
        #region Fonts

        static public SpriteFont normFont;
        //static public SpriteFont headerFont;
        //static public SpriteFont playerNameFont;
        //static public SpriteFont debugFont;
        //static public SpriteFont buttonNamesFont;
        //static public SpriteFont descriptionFont;
        //static public SpriteFont gearInfoFont;
        //static public SpriteFont damageFont;
        //static public SpriteFont playerStatisticsFont;
        //static public SpriteFont hudDetailFont;
        //static public SpriteFont captionFont;

        #endregion


        #region Font Colors

        //public static readonly Color CountColor = new Color(79, 24, 44);
        //public static readonly Color TitleColor = new Color(59, 18, 6);
        //public static readonly Color CaptionColor = new Color(228, 168, 57);
        //public static readonly Color HighlightColor = new Color(223, 206, 148);
        //public static readonly Color DisplayColor = new Color(68, 32, 19);
        //public static readonly Color DescriptionColor = new Color(0, 0, 0);
        //public static readonly Color RestrictionColor = new Color(0, 0, 0);
        //public static readonly Color ModifierColor = new Color(0, 0, 0);
        //public static readonly Color MenuSelectedColor = new Color(248, 218, 127);

        #endregion


        #region Initialization

        /// <summary>
        /// Load the fonts from the content pipeline.
        /// </summary>
        public static void LoadContent(ContentManager contentManager)
        {
            // check the parameters
            if (contentManager == null)
                throw new ArgumentNullException("contentManager");

            // load each font from the content pipeline
            normFont = contentManager.Load<SpriteFont>("Fonts/temp_font_v3");

            //buttonNamesFont = contentManager.Load<SpriteFont>("Fonts/ButtonNamesFont");
            //captionFont = contentManager.Load<SpriteFont>("Fonts/CaptionFont");
            //damageFont = contentManager.Load<SpriteFont>("Fonts/DamageFont");
            //debugFont = contentManager.Load<SpriteFont>("Fonts/DebugFont");
            //descriptionFont = contentManager.Load<SpriteFont>("Fonts/DescriptionFont");
            //gearInfoFont = contentManager.Load<SpriteFont>("Fonts/GearInfoFont");
            //headerFont = contentManager.Load<SpriteFont>("Fonts/HeaderFont");
            //hudDetailFont = contentManager.Load<SpriteFont>("Fonts/HudDetailFont");
            //playerNameFont = contentManager.Load<SpriteFont>("Fonts/PlayerNameFont");
            //playerStatisticsFont =
            //    contentManager.Load<SpriteFont>("Fonts/PlayerStatisticsFont");
        }


        /// <summary>
        /// Release all references to the fonts.
        /// </summary>
        public static void UnloadContent()
        {
            normFont = null;

            //buttonNamesFont = null;
            //captionFont = null;
            //damageFont = null;
            //debugFont = null;
            //descriptionFont = null;
            //gearInfoFont = null;
            //headerFont = null;
            //hudDetailFont = null;
            //playerNameFont = null;
            //playerStatisticsFont = null;
        }


        #endregion


        #region Text Helper Methods

        /// <summary>
        /// Adds newline characters to a string so that it fits within a certain size.
        /// </summary>
        /// <param name="text">The text to be modified.</param>
        /// <param name="maximumCharactersPerLine">
        /// The maximum length of a single line of text.
        /// </param>
        /// <param name="maximumLines">The maximum number of lines to draw.</param>
        /// <returns>The new string, with newline characters if needed.</returns>
        public static string BreakTextIntoLines(string text, int maximumCharactersPerLine, int maximumLines)
        {
            if (maximumLines <= 0)
                throw new ArgumentOutOfRangeException("maximumLines");

            if (maximumCharactersPerLine <= 0)
                throw new ArgumentOutOfRangeException("maximumCharactersPerLine");

            if (String.IsNullOrEmpty(text)) // if the string is trivial, then this is really easy
                return String.Empty;

            if (text.Length < maximumCharactersPerLine) // if the text is short enough to fit on one line, then this is still easy
                return text;

            // construct a new string with carriage returns
            StringBuilder stringBuilder = new StringBuilder(text);
            int currentLine = 0;
            int newLineIndex = 0;
            while (((text.Length - newLineIndex) > maximumCharactersPerLine) && (currentLine < maximumLines))
            {
                text.IndexOf(' ', 0);
                int nextIndex = newLineIndex;
                while ((nextIndex >= 0) && (nextIndex < maximumCharactersPerLine))
                {
                    newLineIndex = nextIndex;
                    nextIndex = text.IndexOf(' ', newLineIndex + 1);
                }
                stringBuilder.Replace(' ', '\n', newLineIndex, 1);
                currentLine++;
            }

            return stringBuilder.ToString();
        }


        /// <summary>
        /// Adds new-line characters to a string to make it fit.
        /// </summary>
        /// <param name="text">The text to be drawn.</param>
        /// <param name="maximumCharactersPerLine">
        /// The maximum length of a single line of text.
        /// </param>
        public static string BreakTextIntoLines(string text, int maximumCharactersPerLine)
        {
            if (maximumCharactersPerLine <= 0) // check the parameters
                throw new ArgumentOutOfRangeException("maximumCharactersPerLine");

            if (String.IsNullOrEmpty(text)) // if the string is trivial, then this is really easy
                return String.Empty;

            if (text.Length < maximumCharactersPerLine)  // if the text is short enough to fit on one line, then this is still easy
                return text;

            // construct a new string with carriage returns
            StringBuilder stringBuilder = new StringBuilder(text);
            int currentLine = 0;
            int newLineIndex = 0;
            while (((text.Length - newLineIndex) > maximumCharactersPerLine))
            {
                text.IndexOf(' ', 0);
                int nextIndex = newLineIndex;
                while ((nextIndex >= 0) && (nextIndex < maximumCharactersPerLine))
                {
                    newLineIndex = nextIndex;
                    nextIndex = text.IndexOf(' ', newLineIndex + 1);
                }
                stringBuilder.Replace(' ', '\n', newLineIndex, 1);
                currentLine++;
            }

            return stringBuilder.ToString();
        }


        /// <summary>
        /// Break text up into separate lines to make it fit.
        /// </summary>
        /// <param name="text">The text to be broken up.</param>
        /// <param name="font">The font used ot measure the width of the text.</param>
        /// <param name="rowWidth">The maximum width of each line, in pixels.</param>
        public static List<string> BreakTextIntoList(string text, SpriteFont font, int rowWidth)
        {
            // check parameters
            if (font == null)
                throw new ArgumentNullException("font");
            if (rowWidth <= 0)
                throw new ArgumentOutOfRangeException("rowWidth");

            List<string> lines = new List<string>(); // create the list

            if (String.IsNullOrEmpty("text")) // check for trivial text
            {
                lines.Add(String.Empty);
                return lines;
            }

            if (font.MeasureString(text).X <= rowWidth) // check for text that fits on a single line
            {
                lines.Add(text);
                return lines;
            }

            string[] words = text.Split(' '); // break the text up into words

            // add words until they go over the length
            int currentWord = 0;
            while (currentWord < words.Length)
            {
                int wordsThisLine = 0;
                string line = String.Empty;
                while (currentWord < words.Length)
                {
                    string testLine = line;

                    if (testLine.Length < 1)
                        testLine += words[currentWord];
                    else if ((testLine[testLine.Length - 1] == '.') || (testLine[testLine.Length - 1] == '?') || (testLine[testLine.Length - 1] == '!'))
                        testLine += "  " + words[currentWord];
                    else
                        testLine += " " + words[currentWord];

                    if ((wordsThisLine > 0) && (font.MeasureString(testLine).X > rowWidth))
                        break;

                    line = testLine;
                    wordsThisLine++;
                    currentWord++;
                }
                lines.Add(line);
            }
            return lines;
        }


        /// <summary>
        /// Returns a properly-formatted gold-quantity string.
        /// </summary>
        public static string GetGoldString(int gold)
        {
            return String.Format("{0:n0}", gold);
        }


        #endregion


        #region Drawing Helper Methods


        /// <summary>
        /// Draws text centered at particular position.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch object used to draw.</param>
        /// <param name="font">The font used to draw the text.</param>
        /// <param name="text">The text to be drawn</param>
        /// <param name="position">The center position of the text.</param>
        /// <param name="color">The color of the text.</param>
        public static void DrawCenteredText(ref SpriteBatch spriteBatch, SpriteFont font,
            string text, Vector2 position, Color color)
        {
            // check the parameters
            if (spriteBatch == null)
                throw new ArgumentNullException("spriteBatch");

            if (font == null)
                throw new ArgumentNullException("font");

            // check for trivial text
            if (String.IsNullOrEmpty(text))
                return;

            // calculate the centered position
            Vector2 textSize = font.MeasureString(text);
            Vector2 centeredPosition = new Vector2(
                position.X - (int)textSize.X / 2,
                position.Y - (int)textSize.Y / 2);

            // draw the string
            spriteBatch.DrawString(font, text, centeredPosition, color, 0f,
                Vector2.Zero, 1f, SpriteEffects.None, 1f);//1f - position.Y / 720f);
        }


        #endregion

    }
}
