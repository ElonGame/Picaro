using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LibraryProject.Map
{
    static public class RoomMaker
    {
        static private Random rand;

        static public void initialize()
        {
            rand = new Random(DateTime.Now.Second);
        }

        static public void MakeRooms(ref AreaMap new_map, ref AreaMapType template)
        {
            while (!Globals.roomFail)
            {
                MakeRoom(ref new_map, ref template);
                if (Globals.roomFail)
                    break;
            }
        }

        static public void MakeRoom(ref AreaMap new_map, ref AreaMapType template)
        {
            RoomTypes room_type = RoomTypes.NULL;
            // loads a random RoomTypes into local var room_typr and returns room_index;
            // room_index matches a room in List<MapRoomTypes> in AreaMap
            template.currRoomTypeIndex = GetRoomTypeProb(ref template, ref room_type);

            switch (room_type)
            {
                case RoomTypes.RECTANGLE:
                    SetRectRoomArea(ref new_map, ref template);
                    if (Globals.roomFail)
                        break;

                    foreach (Point sqr in new_map.RoomList[MapGenHelper.roomCounter].AllSquares)
                        MapGenHelper.PreToRoomOrWall(ref new_map, sqr, MapGenHelper.roomCounter);

                    SetRoomBarrier(ref new_map, ref template, MapGenHelper.roomCounter);
                    MakeRoomArea(ref new_map, ref template);
                    MapGenHelper.roomCounter++;

                    break;
            }
        }

        static public void SetRoomBarrier(ref AreaMap new_map, ref AreaMapType template, int room_ID)
        {
            if (template.roomBarrierSizes[template.currRoomTypeIndex] != 0)
            {
                int x = 0;
                int y = 0;
                int wave = 0;
                int old_wave = 1;
                int wave_size = 0; // size of PREVIOUS wave
                int wave_counter = 0; // tracker for size of CURRENT wave
                int no_make_size = template.roomBarrierSizes[template.currRoomTypeIndex] - 1;
                Point offset = Point.Zero;

                List<List<Point>> waves = new List<List<Point>>();
                waves.Add(new List<Point>());
                waves.Add(new List<Point>());

                // first iteration through wall square list, populates first "wave"
                foreach (Point sqr in new_map.RoomList[MapGenHelper.roomCounter].WallSquares)
                {
                    for (int dir = 0; dir < Globals.NumDirects; dir += Globals.AllDirItr)
                    {
                        offset = Globals.Directions[dir];
                        x = sqr.X; y = sqr.Y; x += offset.X; y += offset.Y;

                        if (!new_map.OffMap(x, y))
                        {
                            if (new_map.MSGrid[y][x].RoomID != room_ID)
                            {
                                if (new_map.MSGrid[y][x].MSStates[(int)MSFlagIndex.CAN_MAKE] != MSFlag.NO_MAKE)
                                {
                                    new_map.MSGrid[y][x].MSStates[(int)MSFlagIndex.CAN_MAKE] = MSFlag.NO_MAKE;
                                    waves[wave].Add(new Point(x, y));
                                    wave_size++;
                                    //new_map.MSGrid[y][x].TileColor = Color.Blue;
                                }
                            }
                        }
                    }
                }

                // loops back and forth between waves
                for (int wv = 0; wv < no_make_size; wv++)
                {
                    if (wave > 0)
                    {
                        wave = 0;
                        old_wave = 1;
                    }
                    else
                    {
                        wave = 1;
                        old_wave = 0;
                    }

                    for (int wav = 0; wav < wave_size; wav++)
                    {
                        for (int dir = 0; dir < Globals.NumDirects; dir += Globals.AllDirItr)
                        {
                            offset = Globals.Directions[dir];
                            x = waves[old_wave][wav].X; y = waves[old_wave][wav].Y;
                            x += offset.X; y += offset.Y;

                            if (!new_map.OffMap(x, y))
                            {
                                if (new_map.MSGrid[y][x].RoomID != room_ID)
                                {
                                    if (new_map.MSGrid[y][x].MSStates[(int)MSFlagIndex.CAN_MAKE] != MSFlag.NO_MAKE)
                                    {
                                        new_map.MSGrid[y][x].MSStates[(int)MSFlagIndex.CAN_MAKE] = MSFlag.NO_MAKE;

                                        if (wav < waves[wave].Count)
                                        {
                                            waves[wave][wav] = new Point(x, y);
                                        }
                                        else
                                        {
                                            waves[wave].Add(new Point(x, y));
                                        }

                                        wave_counter++;
                                        //if(wave == 0)
                                        //    new_map.MSGrid[y][x].TileColor = Color.Aqua;
                                        //else
                                        //    new_map.MSGrid[y][x].TileColor = Color.Green;
                                    }
                                }
                            }
                        }
                    }
                    wave_size = wave_counter;
                    wave_counter = 0;
                }
            }
        }

        static public void SetRectRoomArea(ref AreaMap new_map, ref AreaMapType template)
        {
            Rectangle room_rect = Rectangle.Empty; // origin of room to be created, in tiles

            int room_x_max = 0;
            int room_y_max = 0;

            MakeRectUntil(ref room_rect, ref new_map, ref template, 100); // <------------ rects made

            if (Globals.roomFail)
                return;

            // add room to AreaMap room list
            new_map.RoomList.Add(new Room(MapGenHelper.roomCounter, room_rect));

            room_x_max = (room_rect.X + room_rect.Width);
            room_y_max = (room_rect.Y + room_rect.Height);

            // minimum room x tile coordinate index to maximum room x tile index, inclusive
            for (int x = room_rect.X; x < room_x_max; x++)
            {
                for (int y = room_rect.Y; y < room_y_max; y++) // same, with y
                {
                    MakePreRoomSqr(ref new_map.MSGrid[y][x], MapGenHelper.roomCounter);
                    new_map.RoomList[MapGenHelper.roomCounter].AllSquares.Add(new Point(x, y));
                }
            }

            //roomMakeFail = false;
        }

        // 1) Gets random TileSet index for Room from LoadedTileSets
        // 2) Calls AddTextureName, which adds the TileSet name to this.mapEngineTextureNames 
        //      and current AreaMap.TextureNameList
        static public void MakeRoomArea(ref AreaMap new_map, ref AreaMapType template)
        {
            int rand_tile_set_index = 0;

            rand_tile_set_index = rand.Next(0, MapGenHelper.LoadedTileSets.Count());
            MapGenHelper.CurrentTileSet = MapGenHelper.LoadedTileSets[rand_tile_set_index];

            foreach (Point sqr in new_map.RoomList[MapGenHelper.roomCounter].AllSquares)
            {
                MapGenHelper.AddTextureName(ref new_map, ref new_map.MSGrid[sqr.Y][sqr.X], MapGenHelper.LoadedTileSets[rand_tile_set_index].TextureName);
                MapGenHelper.AddRoomSqrTile(ref new_map, ref new_map.MSGrid[sqr.Y][sqr.X], sqr.X, sqr.Y);
            }
        }

        #region Misc helper methods

        static public void MakePreRoomSqr(ref MapSquare m_sqr, int room_id)
        {
            m_sqr.MSStates[(int)MSFlagIndex.WALL_RM_ENT] = MSFlag.PROOM;
            m_sqr.RoomID = room_id;
        }

        static public void MakeRndRect(ref Rectangle rect, ref Point x_rng, ref Point y_rng, ref Point width_size, ref Point height_size)
        {
            rect.Width = rand.Next(width_size.X, width_size.Y);
            rect.Height = rand.Next(height_size.X, height_size.Y);
            rect.X = rand.Next(x_rng.X, x_rng.Y - rect.Width);      // ensures rect does not extend
            rect.Y = rand.Next(y_rng.X, y_rng.Y - rect.Height);     // off map
        }

        static public void MakeRect(ref Rectangle rect, ref AreaMap new_map,
            ref AreaMapType template, ref Point r_size)
        {
            MakeRndRect(ref rect, ref new_map.xRange, ref new_map.yRange, ref r_size, ref r_size);
        }

        static public float MakeRectUntil(ref Rectangle rect, ref AreaMap new_map, ref AreaMapType template, int max_fails)
        {
            int tries = 0;
            int fails = 0;
            float fail_rate = 0f;

            Sizes r_size = Sizes.NULL;
            Point r_sizep = Point.Zero;

            while (fails < max_fails)
            {
                tries++;

                r_size = GetRndRmSize(ref template, template.currRoomTypeIndex);
                template.currRoomSize = r_size;
                r_sizep = new Point(Globals.sizeRanges[(int)r_size], Globals.sizeRanges[(int)r_size + 1]);

                MakeRect(ref rect, ref new_map, ref template, ref r_sizep);

                if (!new_map.ChkRectForAllFlagged(ref new_map, ref rect, MSFlag.BL, MSFlagIndex.BL_ST) ||
                        !new_map.ChkRectForAllFlagged(ref new_map, ref rect, MSFlag.MAKE_OK, MSFlagIndex.CAN_MAKE))
                    Globals.roomFail = true;
                else
                    Globals.roomFail = false;

                if (Globals.roomFail)
                    fails++;
                else
                    break;
            }

            fail_rate = (float)tries / (float)fails;
            return fail_rate;
        }

        static public Sizes GetRndRmSize(ref AreaMapType template, int room_index)
        {
            int size_index = 0;

            size_index = rand.Next(0, template.roomSizesRange[room_index].Count() - 1);

            return template.roomSizesRange[room_index][size_index];
        }

        static public int GetRoomTypeProb(ref AreaMapType template, ref RoomTypes room_type)
        {
            int room_prob = rand.Next(0, 101); int room_index = 0;

            for (int room = 0; room < template.roomTypeProbs.Count - 1; room++)
                if (room_prob > template.roomTypeProbs[room])
                    continue;
                else
                    room_index = room;

            room_type = template.roomTypes[room_index];
            return room_index;
        }

        #endregion
    }
}
