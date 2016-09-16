﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LibraryProject.Map
{
    static public class MapGenHelper
    {
        static public List<string> mapEngineTextureNames;
        static public int mapWidthTile;
        static public int mapHeightTile;
        static public int dcCount;

        static public int roomCounter; // keeps track of # rooms made for assigning roomIDs
        static public List<int> disconnectedRooms;
        static public List<TileSet> LoadedTileSets;
        static public TileSet CurrentTileSet;

        static public List<int> addSqrIndices;

        static public bool roomMakeFail;
        static public Random rand;

        static public void Initialize()
        {
            rand = new Random();
            addSqrIndices = new List<int>(4);

            for (int i = 0; i < 4; i++)
                addSqrIndices.Add(0);

            mapEngineTextureNames = new List<string>();
            LoadedTileSets = new List<TileSet>();
            disconnectedRooms = new List<int>();

            roomMakeFail = false;
            roomCounter = 0;
            dcCount = 0;
        }
      
        static public void MakeMap(ref AreaMap new_map, ref AreaMapType template, 
            ref List<string> currMapTexNames, Sizes size)
        {
            int prev_dc_count = 0;
            Point m_size_range = Point.Zero;
            m_size_range = Globals.GetSizeRange(MakeTarget.MAP, size);

            // get map size
            template.mapWidthTile = rand.Next(m_size_range.X, m_size_range.Y);
            template.mapHeightTile = template.mapWidthTile;

            // load possible TileSets from template to TileSetManager
            for (int set = 0; set < template.tsNameIndices.Count(); set++)
                TileSetManager.LoadTileSet(template.tsNameIndices[set], set);
            
            // load template into new AreaMap
            new_map = new AreaMap(ref template);
            
            if (new_map.areaMapType == MapTypes.ROOMS)
            {
                List<List<Point>> hall_path = new List<List<Point>>();

                RoomMaker.MakeRooms(ref new_map, ref template);
                HallMaker.ConnectRooms(ref new_map, ref hall_path, 1);
                HallMaker.MakeHalls(ref new_map, ref hall_path);

                while (!HallMaker.FloodIsConnectedCheck(ref new_map, false))            
                {
                    if (prev_dc_count > 0)
                        if (prev_dc_count <= disconnectedRooms.Count())
                            break;
                        
                    hall_path.Clear();
                    HallMaker.ConnectDCRooms(ref new_map, ref hall_path, 1);
                    HallMaker.MakeHalls(ref new_map, ref hall_path);

                    prev_dc_count = disconnectedRooms.Count();
                }
                
            }
            else if (new_map.areaMapType == MapTypes.OPEN)
            {

            }
            else if (new_map.areaMapType == MapTypes.ENCLOSED_ROOMS)
            {

            }
           
            CopyMapTexNames(ref currMapTexNames);
        }

        static public void AddRoomSqrTile(ref AreaMap current_map, ref MapSquare map_square, int x_loc, int y_loc)
        {
            int rand_TI = 0;
            int tile_count = map_square.MSTiles.Count();
            int sources = 0;
            int anim_indices = 0;

            // 0: tile_sources_bg // 1: tile_sources_fg // 2: animated_indices_bg // 3: animated_indices_fg
            for (int i = 0; i < 4; i++)
                addSqrIndices[i] = 0;
 
            map_square.TileColor = Color.White;

            if (map_square.MSStates[(int)MSFlagIndex.WALL_RM_ENT] == MSFlag.ENT ||
                map_square.MSStates[(int)MSFlagIndex.WALL_RM_ENT] == MSFlag.ROOM)
            {
                addSqrIndices[(int)MSMakeIndex.TILE_BG] = (int)TileGroup.BG_ROOM_SOURCES;
                addSqrIndices[(int)MSMakeIndex.TILE_FG] = (int)TileGroup.FG_ROOM_SOURCES; 
            }
            else if (map_square.MSStates[(int)MSFlagIndex.WALL_RM_ENT] == MSFlag.WALL)
            {
                addSqrIndices[(int)MSMakeIndex.TILE_BG] = (int)TileGroup.BG_WALL_SOURCES;
                addSqrIndices[(int)MSMakeIndex.TILE_FG] = (int)TileGroup.FG_WALL_SOURCES;
            }

            addSqrIndices[(int)MSMakeIndex.ANM_BG] = addSqrIndices[(int)MSMakeIndex.TILE_BG];
            addSqrIndices[(int)MSMakeIndex.ANM_FG] = addSqrIndices[(int)MSMakeIndex.TILE_FG];

            for (int i = 0; i < tile_count; i++)
            {
                sources = addSqrIndices[i];
                anim_indices = addSqrIndices[i + 2];

                rand_TI = rand.Next(0, CurrentTileSet.TSrcs[sources].Count());

                if (current_map.IsTileAnimated(CurrentTileSet.ATIndices[anim_indices], CurrentTileSet.TSrcs[sources][rand_TI]))
                {
                    int a_t_index = CurrentTileSet.GetATIndex(CurrentTileSet, CurrentTileSet.TSrcs[anim_indices][rand_TI], sources);
                    map_square.MSTiles[i] =
                        new AnimatedTile(
                            (int)CurrentTileSet.ATIndices[sources][a_t_index].Y,
                            (int)CurrentTileSet.ATIndices[sources][a_t_index].Z,
                            CurrentTileSet.TextureName,
                            new Rectangle(CurrentTileSet.TSrcs[sources][rand_TI] * Globals.tileSize,
                                sources * Globals.tileSize, Globals.tileSize, Globals.tileSize));
                }
                else
                {
                    map_square.MSTiles[i].TextureName = CurrentTileSet.TextureName;
                    map_square.MSTiles[i].SourceRect =
                        new Rectangle(CurrentTileSet.TSrcs[sources][rand_TI] * Globals.tileSize,
                            sources * Globals.tileSize, Globals.tileSize, Globals.tileSize);
                }
            }
        }

        static public void AddTextureName(ref AreaMap area_map, ref MapSquare map_square, string texture_name)          
        {
            if (!(mapEngineTextureNames.Contains(texture_name)))
            {
                mapEngineTextureNames.Add(texture_name);
                area_map.TextureNameList.Add(texture_name);
                // used during MapEngine.Draw() to know which Texture2D in MapEngine.currentMapTextures[]
                // to use for drawing an individual mapSquare in the current AreaMap
                map_square.AreaMapTextureIndex = (mapEngineTextureNames.Count() - 1);
            }
        }

        static public void UnmarkMap(ref AreaMap new_map)
        {
            int x_tile_max = new_map.widthTiles;      int y_tile_max = new_map.heightTiles;

            for (int y = 0; y < y_tile_max; y++)
                for (int x = 0; x < x_tile_max; x++)
                    new_map.MSGrid[y][x].MSStates[(int)MSFlagIndex.IS_MKD] = MSFlag.NT_MKD;
        }

        static public void ToggleBlankState(ref MapSquare m_sqr)
        {
            if(m_sqr.MSStates[(int)MSFlagIndex.BL_ST] == MSFlag.BL)
                m_sqr.MSStates[(int)MSFlagIndex.BL_ST] = MSFlag.NT_BL;
            else
                m_sqr.MSStates[(int)MSFlagIndex.BL_ST] = MSFlag.BL;
        }

        static public void PreToRoomOrWall(ref AreaMap new_map, Point m_sqr, int room_ID)
        {
            new_map.MSGrid[m_sqr.Y][m_sqr.X].MSStates[(int)MSFlagIndex.BL_ST] = MSFlag.NT_BL;

            if(new_map.ChkAdjacentForRoomID(ref new_map, ref m_sqr, room_ID, Globals.NESWItr, 4))
            {
                new_map.RoomList[roomCounter].RoomSquares.Add(new Point(m_sqr.X, m_sqr.Y));
                new_map.MSGrid[m_sqr.Y][m_sqr.X].MSStates[(int)MSFlagIndex.WALL_RM_ENT] = MSFlag.ROOM;
                new_map.MSGrid[m_sqr.Y][m_sqr.X].MSStates[(int)MSFlagIndex.PASSABLE] = MSFlag.NT_BLKD;
            }
            else
            {
                new_map.RoomList[roomCounter].WallSquares.Add(new Point(m_sqr.X, m_sqr.Y));
                new_map.MSGrid[m_sqr.Y][m_sqr.X].MSStates[(int)MSFlagIndex.WALL_RM_ENT] = MSFlag.WALL;
                new_map.MSGrid[m_sqr.Y][m_sqr.X].MSStates[(int)MSFlagIndex.PASSABLE] = MSFlag.BLKD;
            }
        }

        static public void CopyMapTexNames(ref List<string> currMapTexNames)
        {
            for(int i = 0; i < mapEngineTextureNames.Count(); i++)
                currMapTexNames.Add(mapEngineTextureNames[i]);
        }

        static public void GetMapData(ref List<int> dcRoomList)
        {
            if (disconnectedRooms.Count != 0)
                for (int i = 0; i < disconnectedRooms.Count; i++)
                    dcRoomList.Add(disconnectedRooms[i]);
            else
                dcRoomList.Clear();
        }

        static public void SetAltitudesTest(ref AreaMap new_map, int index, ref List<List<Point>> sqr_list)
        {
            int cur_h = 0;
            int h_change = 1;
            //int prev_row = 0;
            //int row = 0;
            //int index = 0;
            //float alt = 0f;
            //int r_count = new_map.RoomList[index].RoomSquares.Count();
            int r_count = sqr_list[index].Count();
            Point cur_sqr = Point.Zero;

            //prev_row = new_map.RoomList[index].RoomSquares[0].X;
            for(int i = 0; i < r_count; i++)
            {
                cur_sqr = sqr_list[index][i];
                //cur_sqr = new_map.RoomList[index].RoomSquares[i];
                //row = new_map.RoomList[index].RoomSquares[i].X;

                //if (prev_row != row)
                //{
                //    //if (alt < 2f)
                //    //    alt += 1f;
                //    //else if (alt >= 2f)
                //    //    alt += 50f;

                //    //if (alt > -0.6f)
                //    //    alt -= 0.3f;
                //    //else if (alt <= -0.6f)
                //    //    alt -= 0.2f;                    
                //}

                new_map.MSGrid[cur_sqr.Y][cur_sqr.X].altitude = Globals.Heights[cur_h];
                cur_h += h_change;
                if (cur_h > Globals.HeightMax || cur_h < 0)
                {
                    h_change *= -1;
                    cur_h += (2 * h_change);
                }                 

                //prev_row = row;
            }  
        }
    }
}
