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

namespace MapGeneratorProject
{
    static public class MapEngine
    {
        static public List<int> discRooms;

        static private ContentManager mapEngineContentManager;

        static private List<Texture2D> curMTex2D; // current map textures
        static private List<string> curMTex2DNames;
        static private Texture2D blankBG;

        static public AreaMap currMap;

        static private Vector2 mDrawOrg; // map draw origin
        static private Viewport mEngVport; // MapEngine Viewport
        static private Point minMEngVportBounds;
        static private Point maxMEngVportBounds;
        static public Vector2 mEngVportWLoc; // MapEngine Viewport World Location

        static private Point mouseLoc;
        static private Point mouseWorldLoc;

        static public bool ShowBlankStates;

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

        static public Viewport MapEngineViewport
        {
            get { return mEngVport; }
            set
            {
                mEngVport = value;
                //viewportCenter = new Vector2(
                //    mapEngineViewport.X + mapEngineViewport.Width / 2f,
                //    mapEngineViewport.Y + mapEngineViewport.Height / 2f);
            }
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

        static public void Initialize(ContentManager content_manager)
        {
            mapEngineContentManager = content_manager;

            mEngVport.X = minMEngVportBounds.X;
            mEngVport.Y = minMEngVportBounds.Y;
            mEngVportWLoc = Vector2.Zero;
            mDrawOrg = new Vector2(550 + 85, 10);
            ShowBlankStates = false;

            curMTex2D = new List<Texture2D>();
            curMTex2DNames = new List<string>();
            LoadContent(content_manager);

            // List of enum identifiers for TileSets to be used for room creation
            List<TSNamesIndex> tile_sets = new List<TSNamesIndex>(); // <---------- tile set thing
            tile_sets.Add(TSNamesIndex.TEST_TILE_SET_NEW); // <---------------------------- tile set thing

            discRooms = new List<int>();
            MapGenerator.GenerateMap(ref currMap, ref curMTex2DNames, MapSets.TEST_MAP_SET, Sizes.LARGE, ref discRooms); // <----- map gen

            //string testStr = "test.xml";
            //MapLoader.MapToSavedMap(ref currMap, ref MapGeneratorMain.contentRoot, ref testStr);

            InfoWriter.BLOutputData2 = discRooms.Count().ToString();

            maxMEngVportBounds.X = currMap.widthTiles * Globals.tileSize;
            maxMEngVportBounds.Y = currMap.heightTiles * Globals.tileSize;

            LoadMapTextures(); // <---------------------------------------------------------------- tile texture names

            InfoWriter.TLOutputData = MapGenHelper.roomCounter.ToString();
            mouseLoc = Point.Zero;
        }

        static private void LoadContent(ContentManager contentManager)
        {
            blankBG = mapEngineContentManager.Load<Texture2D>(@"MapTextures\blankBG");
        }

        /// <summary>
        /// Converts texture names from current AreaMap and adds them as Texture2D types to currentMapTextures
        /// </summary>
        static public void LoadMapTextures()
        {
            foreach (string textureName in currMap.TextureNameList)
                curMTex2D.Add(TextureManager.TextureNameToPathDictionary[textureName]);
        }

        /// <summary>
        /// Doesn't work right now, dont use
        /// </summary>
        static public void ResetMap()
        {

        }

        static public void ClearMap()
        {
            currMap = new AreaMap();
            curMTex2D.Clear();
        }

        static public void LoadMap(ref string file_name)
        {
            MapLoader.SavedMapToMap(ref file_name, out currMap);

            //InfoWriter.BLOutputData2 = discRooms.Count().ToString();
            //InfoWriter.TLOutputData = MapGenHelper.roomCounter.ToString();

            maxMEngVportBounds.X = currMap.widthTiles * Globals.tileSize;
            maxMEngVportBounds.Y = currMap.heightTiles * Globals.tileSize;

            LoadMapTextures();

            InfoWriter.ReInitialize();
        }

        static public bool IsMapNull()
        {
            if (currMap.widthTiles == 0)
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
            mouseWorldLoc.Y = (int)mEngVportWLoc.Y + Mouse.GetState().Y;
            InfoWriter.TLOutputData3 = mouseWorldLoc.ToString();

            for (int row = 0; row < currMap.heightTiles; row++)
            {
                for (int column = 0; column < currMap.widthTiles; column++)
                {
                    currMap.MSGrid[row][column].Update(gt);

                    if (CheckVisibility(currMap.MSGrid[row][column].WorldRect))
                        if (currMap.MSGrid[row][column].WorldRect.Contains(mouseWorldLoc))
                            if(Globals.GetMapData)
                            {
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

        /// <summary>
        /// Calls Draw() for each MapSquare that is in Viewport
        /// </summary>
        /// <param name="sb"></param>
        static public void Draw(SpriteBatch sb)
        {
            Rectangle destinationRectangle = new Rectangle(0, 0, Globals.tileSize, Globals.tileSize);

            for (int row = 0; row < currMap.heightTiles; row++)
            {
                for (int column = 0; column < currMap.widthTiles; column++)
                {
                    if (CheckVisibility(currMap.MSGrid[row][column].WorldRect))
                    {
                        if (currMap.MSGrid[row][column].MSStates[(int)MSFlagIndex.BL_ST] == MSFlag.BL)
                        {
                            if (MapEngine.ShowBlankStates == true)
                                currMap.MSGrid[row][column].TileColor = Color.Aqua;

                            currMap.MSGrid[row][column].Draw(sb, blankBG, mDrawOrg, mEngVportWLoc);
                        }                        
                        else
                            currMap.MSGrid[row][column].Draw(sb, curMTex2D[currMap.MSGrid[row][column].MTex2DIndx], mDrawOrg, mEngVportWLoc);
                    }
                }
            }
        }

        static public void Draw(SpriteBatch sb, ref GraphicsDeviceManager gd_man, ref Effect effect)
        {
            Rectangle destinationRectangle = new Rectangle(0, 0, Globals.tileSize, Globals.tileSize);

            for (int row = 0; row < currMap.heightTiles; row++)
            {
                for (int column = 0; column < currMap.widthTiles; column++)
                {
                    destinationRectangle.X = column * Globals.tileSize;
                    destinationRectangle.Y = row * Globals.tileSize;

                    if (CheckVisibility(destinationRectangle))
                        if (currMap.MSGrid[row][column].Altitude != 0)
                            currMap.MSGrid[row][column].DrawHeights(ref sb, curMTex2D[currMap.MSGrid[row][column].MTex2DIndx],
                                mDrawOrg, mEngVportWLoc, ref gd_man, ref effect);
                }
            }
        }

        /// <summary>
        /// Checks if current Rectangle is in the Viewport
        /// </summary>
        /// <param name="screenRectangle"></param>
        /// <returns></returns>
        static public bool CheckVisibility(Rectangle screenRectangle)
        {
            return ((screenRectangle.X > mEngVportWLoc.X - screenRectangle.Width) &&
                (screenRectangle.Y > mEngVportWLoc.Y - screenRectangle.Height) &&
                (screenRectangle.X < mEngVportWLoc.X + mEngVport.Width) &&
                (screenRectangle.Y < mEngVportWLoc.Y + mEngVport.Height));
        }

        /// <summary>
        /// Sets width and height of Viewport
        /// </summary>
        /// <param name="map_engine_viewport_width"></param>
        /// <param name="map_engine_viewport_height"></param>
        static public void SetMapEngineViewportDimensions(int map_engine_viewport_width,
            int map_engine_viewport_height)
        {
            mEngVport.Width = map_engine_viewport_width;
            mEngVport.Height = map_engine_viewport_height;
        }

        /// <summary>
        /// Moves Viewport, checking if it is in bounds before doing so
        /// </summary>
        /// <param name="x_offset"></param>
        /// <param name="y_offset"></param>
        static public void SetMapEngineViewportLocation(int x_offset, int y_offset)
        {
            int new_x_loc_pixel = (int)mEngVportWLoc.X + x_offset;
            int new_y_loc_pixel = (int)mEngVportWLoc.Y + y_offset;
            int new_max_x_loc_pixel = new_x_loc_pixel + mEngVport.Width;
            int new_max_y_loc_pixel = new_y_loc_pixel + mEngVport.Height;

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

        static public void MapEngineFirstRoom()
        {
            mEngVportWLoc.X = currMap.RoomList[0].XOrigin * 55;
            mEngVportWLoc.Y = currMap.RoomList[0].YOrigin * 55;
        }
    }
}
