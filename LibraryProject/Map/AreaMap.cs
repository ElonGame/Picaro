using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LibraryProject
{
    public class AreaMap
    {
        public Point startPoint;

        public MapTypes areaMapType;

        public int widthTiles; // <---- serialize
        public int heightTiles; // <---- serialize
        //private int mapAreaTiles;

        public Point xRange;
        public Point yRange;

        public List<Room> RoomList; // list of rooms in map

        public List<string> TextureNameList; // <---- serialize

        public MapSquare[][] MSGrid; // <---- serialize

        private Random rand;

        public AreaMap()
        {
            widthTiles = 0;
        }

        public AreaMap(int map_size)
        {
            rand = new Random();

            widthTiles = map_size;
            heightTiles = map_size;

            xRange = new Point(0, widthTiles);
            yRange = new Point(0, heightTiles);

            //mapAreaTiles = widthTiles * heightTiles;

            InitializeLists();

            MSGrid = new MapSquare[map_size][];

            for (int row = 0; row < map_size; row++)
            {
                MSGrid[row] = new MapSquare[map_size];

                for (int column = 0; column < map_size; column++)
                    MSGrid[row][column] = new MapSquare(column, row);
            }
        }

        public AreaMap(ref AreaMapTemplate template)
        {
            rand = new Random();

            widthTiles = template.mapWidthTile;
            heightTiles = template.mapHeightTile;

            xRange = new Point(0, widthTiles);
            yRange = new Point(0, heightTiles);

            InitializeLists();

            LoadTemplate(ref template);

            MSGrid = new MapSquare[heightTiles][];

            for (int row = 0; row < heightTiles; row++)
            {
                MSGrid[row] = new MapSquare[widthTiles];

                for (int column = 0; column < widthTiles; column++)
                    MSGrid[row][column] = new MapSquare(column, row);
            }
        }

        #region MapSquare checking functions

        public bool IsTileAnimated(List<Vector3> tile_set_indices, int checked_index)
        {
            for (int index = 0; index < tile_set_indices.Count(); index++)
                if (tile_set_indices[index].X == checked_index)
                    return true;

            return false;
        }

        public bool OffMap(int tile_x, int tile_y)
        {
            if ((tile_x < 0) || (tile_x >= widthTiles) || (tile_y < 0) || (tile_y >= heightTiles))
                return true;

            return false;
        }

        public bool ChkMSForState(ref MapSquare map_square, MSFlag checked_state, MSFlagIndex checked_index)
        {
            if(map_square.MSStates[(int)checked_index] == checked_state)
                return true;

            return false;
        }

        public bool ChkMSForState(ref MapSquare map_square, MSFlag checked_state)
        {
            for (int i = 0; i < map_square.MSStates.Count(); i++)
                if (map_square.MSStates[i] == checked_state)
                    return true;

            return false;
        }

        public MSFlag ChkMSState(MapSquare map_square, MSFlagIndex checked_index)
        {
            if((int)checked_index >= map_square.MSStates.Count())
                return MSFlag.NULL;
            else
                return map_square.MSStates[(int)checked_index];
        }

        public bool ChkRectForAllFlagged(ref AreaMap new_map, ref Rectangle rect, MSFlag flag)
        {
            int y_max = (rect.Y + rect.Height);
            int x_max = (rect.X + rect.Width);

            for (int tile_y = rect.Y; tile_y < y_max; tile_y++)
            {
                for (int tile_x = rect.X; tile_x < x_max; tile_x++)
                {
                    if (new_map.OffMap(tile_x, tile_y))
                        return false;

                    if (!ChkMSForState(ref new_map.MSGrid[tile_y][tile_x], flag))
                        return false;
                }
            }
            return true;
        }

        public bool ChkRectForAllFlagged(ref AreaMap new_map, ref Rectangle rect, MSFlag flag, MSFlagIndex flag_index)
        {
            int y_max = (rect.Y + rect.Height);     int x_max = (rect.X + rect.Width);
            
            for (int tile_y = rect.Y; tile_y < y_max; tile_y++)
            {
                for (int tile_x = rect.X; tile_x < x_max; tile_x++)
                {
                    if (new_map.OffMap(tile_x, tile_y))
                        return false;

                    if (!ChkMSForState(ref new_map.MSGrid[tile_y][tile_x], flag, flag_index))
                        return false;
                }
            }
            return true;
        }

        public bool ChkLinearPathForFlagged(ref AreaMap new_map, ref Point path_start, Point path_end,
            Directions direction, MSFlag flag, MSFlagIndex f_index, ref Point destination)
        {
            Point next = path_start;
            Point step = Point.Zero;

            step.X = Globals.Directions[(int)direction].X; step.Y = Globals.Directions[(int)direction].Y; // <----
            
            while (next != path_end)
            {
                next = Globals.AddPoints(ref next, ref step);

                if (new_map.OffMap((int)next.X, (int)next.Y))
                    return false;
                else if (next == destination)
                    return true;
                else if (!new_map.ChkMSForState(ref new_map.MSGrid[(int)next.Y][(int)next.X], flag, f_index))
                    return false;
            }
            return true;
        }

        public Point ChkIfNextTo(ref Point pt1, ref Point pt2, int magnitude, int dir_itr)
        {
            Point compare_pt = new Point();

            for (int direction = 0; direction < Globals.Directions.Count(); direction += dir_itr) // <----
            {
                for (int i = 1; i <= magnitude; i++)
                {
                    compare_pt = new Point(Globals.Directions[direction].X * i, Globals.Directions[direction].Y * i);

                    if (Globals.AddPoints(ref pt1, ref compare_pt) == pt2)
                        return Globals.Directions[direction];
                }
            }
            return Point.Zero;
        }

        // checks immediate adjacent mapSquares (NESW or All, depending on itr passed) for a MSFlag,
        // if the number of flagged MS is >= max_states, returns true, otherwise false
        public bool ChkAdjacentForMState(ref AreaMap map, ref Point sqr, MSFlag state, MSFlagIndex state_index, int dir_itr, int max_states)
        {
            int states_found = 0;
            Point ms_chk, dir_offset;
            dir_offset = ms_chk = Point.Zero;

            for (int dir = 0; dir < Globals.NumDirects; dir += dir_itr)
            {
                ms_chk = sqr;
                dir_offset = Globals.Directions[dir];

                ms_chk = Globals.AddPoints(ref ms_chk, ref dir_offset);

                if (OffMap(ms_chk.X, ms_chk.Y))
                    continue;             
                else if (map.ChkMSForState(ref map.MSGrid[ms_chk.Y][ms_chk.X], state, state_index))
                    states_found++;
            }

            if(states_found >= max_states)
                return true;

            return false;
        }

        public bool ChkAdjacentForRoomID(ref AreaMap map, ref Point sqr, int room_ID, int dir_itr, int max_hits)
        {
            int hits_found = 0;
            Point ms_chk, dir_offset;
            dir_offset = ms_chk = Point.Zero;

            for (int dir = 0; dir < Globals.NumDirects; dir += dir_itr)
            {
                ms_chk = sqr;
                dir_offset = Globals.Directions[dir];
                ms_chk = Globals.AddPoints(ref ms_chk, ref dir_offset);

                if (OffMap(ms_chk.X, ms_chk.Y))
                    continue;
                else if (map.MSGrid[ms_chk.Y][ms_chk.X].RoomID == room_ID)
                    hits_found++;
            }

            if (hits_found >= max_hits)
                return true;

            return false;
        }

        #endregion

        #region Loading stuff

        private void InitializeLists()
        {
            //RoomSizesRange = new List<Sizes>(2);
            RoomList = new List<Room>(1);
            //hallSquaresList = new List<Vector2>();
            TextureNameList = new List<string>(1);
        }

        private void LoadTemplate(ref AreaMapTemplate template)
        {
            areaMapType = template.mapType;

            //widthTiles = template.mapWidthTile;
            //heightTiles = template.mapHeightTile;
            //mapAreaTiles = widthTiles * heightTiles;
        }

        #endregion
    }
}
