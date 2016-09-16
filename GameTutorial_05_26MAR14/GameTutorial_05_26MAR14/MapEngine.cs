using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LibraryProject;
using MapGeneratorProject;

namespace GameTutorial_05_26MAR14
{
    static public class MapEngine
    {
        static private ContentManager mapEngineContentManager;

        static public List<Texture2D> curMTex2D; // current map textures
        static public List<string> curMTex2DNames;
        static private Texture2D blankBG;
        static public Texture2D shadowMasks;

        static public AreaMap currMap;

        static public Vector2 mDrawOrg; // map draw origin
        static public Viewport MapEngineViewport; // MapEngine Viewport
        static private Point minMEngVportBounds;
        static private Point maxMEngVportBounds;
        static public Vector2 mEngVportWLoc; // MapEngine Viewport World Location

        static private Point mouseLoc;
        static private Point mouseWorldLoc;
        static public Point curMouseSqr;

        static public bool ShowBlankStates;

        /////
        static public Texture2D tempHUD;
        static public Texture2D bgRect;

        static public SpriteFont tempFont;

        static public Rectangle leftBGRectDrawRect;
        static public Rectangle rightBGRectDrawRect;
        static public Rectangle tileRect;

        static public Point startDrawPosBG;

        static public int leftHUDwidth = 550;
        static public int rightBGwidth = 880;
        static public int screenDrawHeight = 880;

        static public List<Texture2D> CurrentMapTextures
        {
            get { return curMTex2D; }
            set { curMTex2D = value; }
        }

        static public List<string> CurrentMapTexturesNames
        {
            get { return curMTex2DNames; }
            set { curMTex2DNames = value; }
        }

        static public AreaMap CurrentMap
        {
            get { return currMap; }
            set { currMap = value; }
        }

        static public Point MinMapEngineViewportBounds
        {
            get { return minMEngVportBounds; }
            set { minMEngVportBounds = value; }
        }

        static public Point MaxMapEngineViewportBounds
        {
            get { return maxMEngVportBounds; }
            set { maxMEngVportBounds = value; }
        }

        static public void Initialize(GraphicsDeviceManager graphics)//ref GraphicsDeviceManager graphics)//ContentManager content_manager)
        {
            //mapEngineContentManager = content_manager;           

            // Background temp stuff initialization
            startDrawPosBG = new Point(85, 10);
            leftBGRectDrawRect = new Rectangle(startDrawPosBG.X, startDrawPosBG.Y, leftHUDwidth, screenDrawHeight);
            rightBGRectDrawRect = new Rectangle(startDrawPosBG.X + leftHUDwidth, startDrawPosBG.Y, rightBGwidth, screenDrawHeight);
            tileRect = new Rectangle(0, 0, 55, 55);

            // Viewport initialization
            MinMapEngineViewportBounds = new Point(635, 10); // (550 + 85, 10)
            SetMapEngineViewportDimensions(880, 880);

            MapEngineViewport.X = minMEngVportBounds.X;
            MapEngineViewport.Y = minMEngVportBounds.Y;
            mEngVportWLoc = Vector2.Zero; //  new Vector2(0, 10); 
            mDrawOrg = new Vector2(550 + 85, 10);//(550 + 85, 10);
            ShowBlankStates = false;

            // Texture initialization
            curMTex2D = new List<Texture2D>();
            curMTex2DNames = new List<string>();
            //LoadContent(content_manager);

            // List of enum identifiers for TileSets to be used for room creation
            List<TSNamesIndex> tile_sets = new List<TSNamesIndex>(); // <---------- tile set thing
            tile_sets.Add(TSNamesIndex.TEST_TILE_SET_NEW); // <---------------------------- tile set thing
            
            //InfoWriter.Initialize(ref tempFont);
            //InfoWriter.BLOutputData2 = discRooms.Count().ToString();
            //InfoWriter.TLOutputData = MapGenHelper.roomCounter.ToString();
            mouseLoc = Point.Zero;
            curMouseSqr = Point.Zero;
            MapEngineViewport = graphics.GraphicsDevice.Viewport;
        }

        static public void LoadContent(ContentManager contentManager)
        {
            mapEngineContentManager = contentManager; 

            blankBG = mapEngineContentManager.Load<Texture2D>(@"MapTextures\blankBG");
            shadowMasks = mapEngineContentManager.Load<Texture2D>(@"MapTextures\shadow_masks");
            bgRect = mapEngineContentManager.Load<Texture2D>(@"MapTextures\blankBG");
            tempHUD = mapEngineContentManager.Load<Texture2D>(@"Misc\HUD_game_screen_temp");
            tempFont = mapEngineContentManager.Load<SpriteFont>(@"Fonts\temp_font_v3");
            //string str = "test";
            //int fontHeight = (int)tempFont.MeasureString(str).Y;
        }

        static public void UnloadContent()
        {
            blankBG = null;
            bgRect = null;
            tempHUD = null;
            tempFont = null;
        }

        /// <summary>
        /// Converts texture names from current AreaMap and adds them as Texture2D types to currentMapTextures
        /// </summary>
        static public void LoadMapTextures()
        {
            foreach (string textureName in currMap.TextureNameList)
                curMTex2D.Add(TextureManager.TextureNameToPathDictionary[textureName]);
        }

        static public void NewMap()
        {
            int dc = 0;
            int rc = 0;

            if (!IsMapNull())
                ClearMap();

            if (!MapGenerator.isInitialized)
                MapGenerator.Initialize(ref mapEngineContentManager, ref dc, ref rc);
            else
                MapGenerator.GenerateMap(ref mapEngineContentManager, ref dc, ref rc);

            InfoWriter.BLOutputData2 = dc.ToString();
            InfoWriter.TLOutputData = rc.ToString();
        }

        static public void ResetMap()
        {

        }

        static public void ClearMap()
        {
            currMap = new AreaMap();
            curMTex2D.Clear();
        }

        static public void LoadMap()//ref string file_name)
        {
            string testStr = "test.xml";
            MapLoader.SavedMapToMap(ref testStr, out currMap);

            maxMEngVportBounds.X = currMap.widthTiles * Globals.tileSize;
            maxMEngVportBounds.Y = currMap.heightTiles * Globals.tileSize;
            mEngVportWLoc = Vector2.Zero;

            SetMapEngineViewportDimensions(880, 880);
            
            LoadMapTextures();

            InfoWriter.ReInitialize();
        }

        static public void SaveMap()
        {
            string testStr = "test.xml";
            MapLoader.MapToSavedMap(ref MapEngine.currMap, ref testStr);
            MapEngine.ClearMap();
        }

        static public bool IsMapNull()
        {
            if (currMap == null)
                return true;
            else if (currMap.widthTiles == 0)
                return true;

            return false;
        }

        /// <summary>
        /// Updates all MapSquares in current AreaMap; intended to update AnimatedTiles
        /// </summary>
        /// <param name="gt"></param>
        static public void Update(GameTime gt)
        {
            mouseWorldLoc.X = (int)mEngVportWLoc.X + Mouse.GetState().X - 635;//465;
            mouseWorldLoc.Y = (int)mEngVportWLoc.Y + Mouse.GetState().Y;// +10;
            InfoWriter.TLOutputData3 = mouseWorldLoc.ToString();
            //curMouseSqr = new Point(-1, -1);

            for (int row = 0; row < currMap.heightTiles; row++)
            {
                for (int column = 0; column < currMap.widthTiles; column++)
                {
                    currMap.MSGrid[row][column].Update(gt);

                    if (CheckVisibility(ref currMap.MSGrid[row][column].WorldRect))
                        if (currMap.MSGrid[row][column].WorldRect.Contains(mouseWorldLoc))
                        {
                            curMouseSqr = new Point(column, row);
                            if (Globals.GetMapData)
                            {
                                curMouseSqr = new Point(column, row);

                                if (!(currMap.MSGrid[row][column].MSStates[(int)MSFlagIndex.BL_ST] == MSFlag.BL))
                                {
                                    if (currMap.MSGrid[row][column].RoomID == -1)
                                    {
                                        //InfoWriter.showBLOutput = false;
                                        InfoWriter.TLOutputData4 = "\n";
                                        for (int i = 0; i < currMap.MSGrid[row][column].MSStates.Count(); i++)
                                        {
                                            InfoWriter.TLOutputData4 += currMap.MSGrid[row][column].MSStates[i].ToString();
                                            InfoWriter.TLOutputData4 += "\n";
                                        }
                                        InfoWriter.TLOutputData4 += currMap.MSGrid[row][column].Altitude.ToString();
                                        InfoWriter.BLOutputData = "-";
                                    }
                                    else
                                    {
                                        //InfoWriter.showBLOutput = true;
                                        InfoWriter.BLOutputData = currMap.MSGrid[row][column].RoomID.ToString();
                                        InfoWriter.TLOutputData4 = "\n";
                                        for (int i = 0; i < currMap.MSGrid[row][column].MSStates.Count(); i++)
                                        {
                                            InfoWriter.TLOutputData4 += currMap.MSGrid[row][column].MSStates[i].ToString();
                                            InfoWriter.TLOutputData4 += "\n";
                                        }
                                        InfoWriter.TLOutputData4 += currMap.MSGrid[row][column].Altitude.ToString();
                                    }
                                }
                                else
                                {
                                    InfoWriter.TLOutputData4 = string.Empty;
                                    InfoWriter.BLOutputData = string.Empty;
                                }
                            }
                        }

                }
            }
        }

        /// <summary>
        /// Calls Draw() for each MapSquare that is in Viewport
        /// </summary>
        /// <param name="sb"></param>
        static public void Draw(SpriteBatch sb)
        {
            //sb.Draw(bgRect, leftBGRectDrawRect, Color.DarkGray);
            //sb.Draw(tempHUD, leftBGRectDrawRect, Color.White);
            //sb.Draw(bgRect, rightBGRectDrawRect, tileRect, Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 1f);

            for (int row = 0; row < currMap.heightTiles; row++)
                for (int column = 0; column < currMap.widthTiles; column++)
                    if (CheckVisibility(ref currMap.MSGrid[row][column].WorldRect))
                        if (currMap.MSGrid[row][column].MSStates[(int)MSFlagIndex.BL_ST] == MSFlag.BL)
                        {
                            if (MapEngine.ShowBlankStates == true)
                                currMap.MSGrid[row][column].BaseColor = Color.Aqua;

                            currMap.MSGrid[row][column].Draw(ref sb, blankBG, shadowMasks, mDrawOrg, mEngVportWLoc);
                        }
                        else
                        {
                            currMap.MSGrid[row][column].Draw(ref sb, curMTex2D[currMap.MSGrid[row][column].MapTexIndex], shadowMasks, mDrawOrg, mEngVportWLoc);

                        }

            InfoWriter.Draw(sb);
        }

        static public void Draw(SpriteBatch sb, ref GraphicsDeviceManager gd_man, ref Effect effect)
        {
            for (int row = 0; row < currMap.heightTiles; row++)
                for (int column = 0; column < currMap.widthTiles; column++)
                    if (CheckVisibility(ref currMap.MSGrid[row][column].WorldRect))
                        if (currMap.MSGrid[row][column].Altitude != -1)
                            currMap.MSGrid[row][column].DrawHeights(ref sb, curMTex2D[currMap.MSGrid[row][column].MapTexIndex], shadowMasks, mDrawOrg, mEngVportWLoc, ref gd_man, ref effect);
        }

        /// <summary>
        /// Checks if current Rectangle is in the Viewport
        /// </summary>
        /// <param name="screenRectangle"></param>
        /// <returns></returns>
        static public bool CheckVisibility(ref Rectangle screenRectangle)
        {
            if ((screenRectangle.X > mEngVportWLoc.X - screenRectangle.Width) && (screenRectangle.Y > mEngVportWLoc.Y - screenRectangle.Height))
                if ((screenRectangle.X < mEngVportWLoc.X + MapEngineViewport.Width) && (screenRectangle.Y < mEngVportWLoc.Y + MapEngineViewport.Height))
                    return true;

            return false;
        }

        /// <summary>
        /// Sets width and height of Viewport
        /// </summary>
        /// <param name="map_engine_viewport_width"></param>
        /// <param name="map_engine_viewport_height"></param>
        static public void SetMapEngineViewportDimensions(int map_engine_viewport_width, int map_engine_viewport_height)
        {
            MapEngineViewport.Width = map_engine_viewport_width;
            MapEngineViewport.Height = map_engine_viewport_height;
        }

        /// <summary>
        /// Moves Viewport, checking if it is in bounds before doing so
        /// </summary>
        /// <param name="x_offset"></param>
        /// <param name="y_offset"></param>
        static public void MoveMapEngineViewportLocation(int x_offset, int y_offset)
        {
            int new_x_loc_pixel = (int)mEngVportWLoc.X + x_offset;
            int new_y_loc_pixel = (int)mEngVportWLoc.Y + y_offset;
            int new_max_x_loc_pixel = new_x_loc_pixel + MapEngineViewport.Width;
            int new_max_y_loc_pixel = new_y_loc_pixel + MapEngineViewport.Height;

            if ((new_x_loc_pixel < 0) || (new_max_x_loc_pixel > maxMEngVportBounds.X) ||
                (new_y_loc_pixel < 0) || (new_max_y_loc_pixel > maxMEngVportBounds.Y))
                return;
            else
            {
                mEngVportWLoc.X = new_x_loc_pixel;
                mEngVportWLoc.Y = new_y_loc_pixel;
                InfoWriter.TLOutputData2 = mEngVportWLoc.ToString();
            }
        }

        static public void SetMapEngineViewportLocation(int x_pos, int y_pos)
        {
            //if ((x_pos < 0) || (x_pos + MapEngineViewport.Width > maxMEngVportBounds.X) ||
            //    (y_pos < 0) || (y_pos + MapEngineViewport.Height > maxMEngVportBounds.Y))
            //    return;
            //else
            //{

            mEngVportWLoc.X = MathHelper.Clamp((float)x_pos, 0, ((CurrentMap.widthTiles * tileRect.Width) - MapEngineViewport.Width));
            mEngVportWLoc.Y = MathHelper.Clamp((float)y_pos, 0, ((CurrentMap.heightTiles * tileRect.Height) - MapEngineViewport.Height)); 
            InfoWriter.TLOutputData2 = mEngVportWLoc.ToString();

            //}
        }

        static public void MapEngineFirstRoom()
        {
            mEngVportWLoc.X = currMap.RoomList[0].XOrigin * 55;
            mEngVportWLoc.Y = currMap.RoomList[0].YOrigin * 55;
        }

        static public void GetMouseMapSqr(ref Point ret_square)
        {
            mouseWorldLoc.X = (int)mEngVportWLoc.X + Mouse.GetState().X - 635;//465;
            mouseWorldLoc.Y = (int)mEngVportWLoc.Y + Mouse.GetState().Y;// +10;
        }
    }
}
