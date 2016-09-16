using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using LibraryProject;

namespace GameTutorial_05_26MAR14
{
    static public class InfoWriter
    {
        static public SpriteFont writeFont;

        static public Vector2 writeLocationStartTL1;
        static public Vector2 writeLocationStartTL2;
        static public Vector2 writeLocationStartTL3;
        static public Vector2 writeLocationStartBL;
        static public Vector2 writeLocationStartBL2;
        static public Vector2 writeLocationStartBL3;
        static public Vector2 writeLocationStartBL4;

        static public bool showTLOutput = true;
        static public string TLOutput1 = "ROOM\nCOUNT:\n";
        static public string TLOutputData = "";
        static public string TLOutput2 = "VP LOC:\n";
        static public string TLOutputData2 = "";
        static public string TLOutput3 = "MUS WLOC:\n";
        static public string TLOutputData3 = "";
        static public string TLOutput4 = "FLAGS\nNULL:\nBlank:\nType:\nPassable:\nMarked:\nMake:\nHeight:";
        static public string TLOutputData4 = "";

        static public bool showBLOutput = false;
        static public string BLOutput1 = "ROOM#: ";
        static public string BLOutputData = "";
        static public string BLOutput2 = "Disconnected\nROOMS: ";
        static public string BLOutputData2 = "";
        static public string BLOutput3 = "xMax: ";
        static public string BLOutputData3 = "";
        static public string BLOutput4 = "yMax: ";
        static public string BLOutputData4 = "";


        static public void Initialize()//ref SpriteFont sf)
        {
            writeLocationStartTL1 = new Vector2(100, 25);
            writeLocationStartTL2 = new Vector2(100, 150);
            writeLocationStartTL3 = new Vector2(100, 250);
            writeLocationStartBL = new Vector2(100, 410);
            writeLocationStartBL2 = new Vector2(100, 465);
            writeLocationStartBL3 = new Vector2(100, 575);
            writeLocationStartBL4 = new Vector2(100, 630);

            TLOutputData2 = MapEngine.mEngVportWLoc.ToString();
        }

        static public void ReInitialize()
        {
            TLOutputData2 = MapEngine.mEngVportWLoc.ToString();
            BLOutput3 = MapEngine.CurrentMap.widthTiles.ToString();
            BLOutput4 = MapEngine.CurrentMap.heightTiles.ToString();
        }

        static public void LoadContent()
        {
            writeFont = Fonts.normFont;
        }

        static public void Clear()
        {
            TLOutputData = string.Empty;
            TLOutputData2 = string.Empty;
            TLOutputData3 = string.Empty;
            TLOutputData4 = string.Empty;
            BLOutputData = string.Empty;
            BLOutputData2 = string.Empty;
        }

        static public void Draw(SpriteBatch sb)
        {
            if (Globals.GetMapData)
            {
                if (showTLOutput)
                {
                    sb.DrawString(writeFont, TLOutput1 + TLOutputData, writeLocationStartTL1, Color.Blue);
                    //sb.DrawString(writeFont, TLOutput4, writeLocationStartTL2, Color.Purple, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);
                    //sb.DrawString(writeFont, TLOutputData4, writeLocationStartTL2 + new Vector2(165, 0), Color.Purple, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);
                    sb.DrawString(writeFont, TLOutput2 + TLOutputData2, writeLocationStartTL2, Color.Coral, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);
                    sb.DrawString(writeFont, TLOutput3 + TLOutputData3, writeLocationStartTL3, Color.Cyan, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);
                }

                //if (showBLOutput)
                //{
                sb.DrawString(writeFont, BLOutput1 + BLOutputData, writeLocationStartBL, Color.Green);
                sb.DrawString(writeFont, BLOutput2 + BLOutputData2, writeLocationStartBL2, Color.Red);
                sb.DrawString(writeFont, BLOutput3 + BLOutputData3, writeLocationStartBL3, Color.Blue);
                sb.DrawString(writeFont, BLOutput4 + BLOutputData4, writeLocationStartBL4, Color.GhostWhite);
                //}
            }
        }
    }
}
