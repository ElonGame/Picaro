using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LibraryProject;

namespace GameTutorial_05_26MAR14
{
    static public class HUDHelper
    {
        //// MAP DRAWING STUFF ///
        static public Texture2D hudBorders;
        static public Texture2D bgRect;

        static public SpriteFont writeFont;

        static public Rectangle leftBGRectDrawRect;
        static public Rectangle rightBGRectDrawRect;
        static public Rectangle tileRect = new Rectangle(0, 0, 55, 55);

        static public Point startDrawPosBG = new Point(85, 10);

        static public int leftHUDwidth = 550;
        static public int rightBGwidth = 880;
        static public int screenDrawHeight = 880;

        static public Vector2 mDrawOrg; // map draw origin

        static public void Initialize()
        {
            leftBGRectDrawRect = new Rectangle(startDrawPosBG.X, startDrawPosBG.Y, leftHUDwidth, screenDrawHeight);
            rightBGRectDrawRect = new Rectangle(startDrawPosBG.X + leftHUDwidth, startDrawPosBG.Y, rightBGwidth, screenDrawHeight);
            mDrawOrg = Globals.mDrawOrg;
            LoadContent();
        }

        static public void LoadContent()
        {
            bgRect = ScreenManager.contentMan.Load<Texture2D>(@"MapTextures\blankBG");
            hudBorders = ScreenManager.contentMan.Load<Texture2D>(@"Misc\HUD_game_screen_temp");
            writeFont = ScreenManager.contentMan.Load<SpriteFont>(@"Fonts\temp_font_v3");
        }

        static public void Draw(ref SpriteBatch sb)
        {
            sb.Draw(bgRect, rightBGRectDrawRect, tileRect, Color.Black);
            DrawHud(ref sb);
        }

        static public void DrawHud(ref SpriteBatch sb)
        {
            sb.Draw(bgRect, leftBGRectDrawRect, Color.Black);
            sb.Draw(hudBorders, leftBGRectDrawRect, Color.White);
            DrawStats(ref sb);
        }

        static public void DrawStats(ref SpriteBatch sb)
        {
            int x_pos, y_pos;
            x_pos = startDrawPosBG.X + 15;
            y_pos = startDrawPosBG.Y + 15;

            for (int s = 0; s < GameSession.curPlayer.pStats.statCount; s++)
            {
                // prime stats
                //if(s == 4)
                //    x_pos += 
                sb.DrawString(writeFont, GameSession.curPlayer.pStats.statStringList[s],
                    new Vector2(x_pos, y_pos + (s * 30)), Color.Green, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 1f);
                sb.DrawString(writeFont, GameSession.curPlayer.pStats.statList[s].ToString(),
                    new Vector2(x_pos + 90, y_pos + (s * 30)), Color.LawnGreen, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 1f);
            }

            for (int s = 0; s < GameSession.curPlayer.sStats.statCount; s++)
            {
                // secondary stats
                sb.DrawString(writeFont, GameSession.curPlayer.sStats.statStringList[s],
                    new Vector2(x_pos + 150, y_pos + (s * 30)), Color.Blue, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 1f);
                sb.DrawString(writeFont, GameSession.curPlayer.sStats.statList[s].ToString(),
                    new Vector2(x_pos + 210, y_pos + (s * 30)), Color.Cyan, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 1f);
            }
        }

        static public void MakeNameValString(List<string> names, List<int> vals, 
            ref string name_val_strs, int max_width, int max_height, 
            bool fill_space, bool compact, ref float scale, ref SpriteFont font)
        {
            //int itemCount = names.Count;
            //int vertFontSpace = font.LineSpacing;
            //int fontHeight = (int)font.MeasureString(names[0]).Y;
            //int strHeight = (itemCount - 1) * vertFontSpace;

            //if(strHeight > max_height || fill_space)
            //    scale = max_height / strHeight;


            //int max_digit, max_len, max_chars, spaces;
            //max_digit = max_chars = max_len = spaces = 0;
                       
            //Vector2 line_xy, max_str_xy, str_xy, blank_xy;
            //line_xy = str_xy = blank_xy = Vector2.Zero;
            //string space = " ";

            //GetMaxStrLen(ref names, ref max_str_xy, ref font);

            //max_digit = GetMaxDigits(ref vals);

            //if (compact)
            //    blank_xy = font.MeasureString(space);

            //StringBuilder sBuilder = new StringBuilder();
            //string temp_str = string.Empty;
            //StringBuilder finalStr = new StringBuilder();

            //for (int i = 0; i < names.Count; i++)
            //{
            //    spaces = max_chars - names[i].Length;
            //    sBuilder.Append(names[i]);
            //    for (int j = spaces; i > 0; i--)
            //        sBuilder.Append(space);

            //    spaces = max_digit - GetDigitLen(vals[i]);
            //    for (int j = spaces; i > 0; i--)
            //        sBuilder.Append(space);
            //    sBuilder.Append(vals[i]);

            //    temp_str = sBuilder.ToString();
            //    sBuilder.Clear();
            //    str_xy = font.MeasureString(temp_str);


            //    if (i == (names.Count - 1))
            //        blank_xy = Vector2.Zero;

            //    if (left_to_right)
            //    {
            //        if (str_xy.X + blank_xy.X + line_xy.X < max_width)
            //        {
            //            finalStr.Append(temp_str);
            //            if (compact)
            //                finalStr.Append(space);
            //            line_xy.X += str_xy.X + blank_xy.X;
            //        }
            //        else
            //        {
            //            finalStr.Append;
            //        }
            //    }
            //}
            //name_val_strs = sBuilder.ToString();

            //sBuilder.Clear();
        }

        static public int GetDigitLen(int num)
        {
            int num_digit = 0;

            if(num < 0)
                num_digit++;

            while(num != 0)
            {
                num /= 10;
                num_digit++;
            }

            return num_digit;
        }

        static public int GetMaxDigits(ref List<int> vals)
        {
            int max_digit = 1;
            int len = 0;

            for (int i = 0; i < vals.Count; i++)
            {
                len = GetDigitLen(vals[i]);
                if (len > max_digit)
                    max_digit = len;
            }
            
            return max_digit;
        }

        static public int GetMaxCharLen(ref List<string> strs)
        {
            int max_len = 0;
            int max_char = 1;

            for (int i = 0; i < strs.Count; i++)
                if (strs[i].Length > max_char)
                    max_len = strs[i].Length;

            while (max_len > 10)
            {
                max_len /= 10;
                max_char++;
            }
            return max_char;
        }

        static public void GetMaxStrLen(ref List<string> strs, ref Vector2 outVec, ref SpriteFont font)
        {
            Vector2 len = outVec = Vector2.Zero;
            outVec.Y = font.MeasureString(strs[0]).Y;

            for(int i = 0; i < strs.Count; i++)
            {
                len.X = font.MeasureString(strs[i]).X;
                if (len.X > outVec.X)
                    outVec.X = len.X;
            }          
        }

        static public void DrawNameValuePairColumn(ref string name_str, ref string val_str, ref Vector2 origin)
        {
            
        }
    }
}
