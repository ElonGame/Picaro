using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LibraryProject;

namespace MapGeneratorProject
{
    static public class HallMaker
    {
        static private Random rand;

        static public void initialize()
        {
            rand = new Random(DateTime.Now.Second);
        }

        static public void MakeHalls(ref AreaMap new_map, ref List<List<Point>> hall_paths)
        {
            int counter = hall_paths.Count;

            for (int i = 0; i < counter; i++)
                MakeHall(ref new_map, i, ref hall_paths); // <----- set altitudes called here

            MarkHallWalls(ref new_map, ref hall_paths);
            MakeHallWalls(ref new_map);
        }

        static public void ConnectRooms(ref AreaMap new_map, ref List<List<Point>> hall_paths, int repeats)
        {
            int room_count = new_map.RoomList.Count;

            Vector4 tunnel_ends = new Vector4(); // (start X, start Y, end X, end Y)
            List<Point> path = new List<Point>();

            for (int i = 0; i < repeats; i++)
            {
                for (int curr_room = 0; curr_room < room_count; curr_room++)
                {
                    tunnel_ends = FindClosestRoom(ref new_map, curr_room, room_count);

                    if (tunnel_ends.X == -1f)
                        return;

                    path.Clear();
                    PathFinder.Reset(ref new_map, ref tunnel_ends);
                    PathFinder.Search(ref new_map, ref path, false);

                    if (path.Count != 0)
                    {
                        hall_paths.Add(new List<Point>());
                        for (int s = 0; s < path.Count; s++)
                            hall_paths[hall_paths.Count - 1].Add(path[s]);
                    }
                }
            }
        }

        static public void ConnectDCRooms(ref AreaMap new_map, ref List<List<Point>> hall_paths, int repeats)
        {
            int room_count = MapGenHelper.disconnectedRooms.Count();

            List<Point> path = new List<Point>();

            for (int i = 0; i < repeats; i++)
            {
                for (int j = 0; j < room_count; j++)
                {
                    GetClosestRoomPath(ref new_map, MapGenHelper.disconnectedRooms[j], 
                        ref MapGenHelper.disconnectedRooms, ref path);

                    if (path.Count() > 0)
                    {
                        hall_paths.Add(new List<Point>());
                        for (int s = 0; s < path.Count; s++)
                            hall_paths[hall_paths.Count - 1].Add(path[s]);
                        break;
                    }
                }
            }
        }

        // needs a list of of vector2 for the hall path(vec_path);
        static public void MakeHall(ref AreaMap new_map, int curr_path, ref List<List<Point>> paths)
        {
            int new_x = 0;
            int new_y = 0;
            int prev_x = -1;
            int prev_y = -1;
            int num_sqrs = paths[curr_path].Count;

            for (int square = 1; square < num_sqrs - 1; square++)
            {
                prev_x = paths[curr_path][square - 1].X; prev_y = paths[curr_path][square - 1].Y;
                new_x = paths[curr_path][square].X; new_y = paths[curr_path][square].Y;

                // make new hall MapSquare
                new_map.MSGrid[new_y][new_x].MSStates[(int)MSFlagIndex.BL_ST] = MSFlag.NT_BL;
                new_map.MSGrid[new_y][new_x].MSStates[(int)MSFlagIndex.WALL_RM_ENT] = MSFlag.ROOM;
                new_map.MSGrid[new_y][new_x].MSStates[(int)MSFlagIndex.PASSABLE] = MSFlag.NT_BLKD;
                MapGenHelper.AddRoomSqrTile(ref new_map, ref new_map.MSGrid[new_y][new_x], new_x, new_y);
            }

            // turn beginning and ending MapSquares to Entrances
            new_x = paths[curr_path][0].X;
            new_y = paths[curr_path][0].Y;
            new_map.MSGrid[new_y][new_x].MSStates[(int)MSFlagIndex.WALL_RM_ENT] = MSFlag.ENT;
            new_map.MSGrid[new_y][new_x].MSStates[(int)MSFlagIndex.PASSABLE] = MSFlag.NT_BLKD;
            MapGenHelper.AddRoomSqrTile(ref new_map, ref new_map.MSGrid[new_y][new_x], new_x, new_y);

            new_x = paths[curr_path][num_sqrs - 1].X;
            new_y = paths[curr_path][num_sqrs - 1].Y;
            new_map.MSGrid[new_y][new_x].MSStates[(int)MSFlagIndex.WALL_RM_ENT] = MSFlag.ENT;
            new_map.MSGrid[new_y][new_x].MSStates[(int)MSFlagIndex.PASSABLE] = MSFlag.NT_BLKD;
            MapGenHelper.AddRoomSqrTile(ref new_map, ref new_map.MSGrid[new_y][new_x], new_x, new_y);

            if (Globals.DrawHallHeightsLinear)
                MapGenHelper.SetAltitudesTest(ref new_map, curr_path, ref paths);
        }

        static public void MarkHallWalls(ref AreaMap new_map, int x_loc, int y_loc)
        {
            int x_offset = 0; int y_offset = 0;

            for (int dir = 0; dir < Globals.Directions.Count(); dir += Globals.AllDirItr)
            {
                x_offset = Globals.Directions[dir].X + x_loc; y_offset = Globals.Directions[dir].Y + y_loc;

                if (!new_map.OffMap(x_offset, y_offset))
                    if (new_map.ChkMSForState(ref new_map.MSGrid[y_offset][x_offset], MSFlag.BL, MSFlagIndex.BL_ST))
                        new_map.MSGrid[y_offset][x_offset].MSStates[(int)MSFlagIndex.WALL_RM_ENT] = MSFlag.PWALL;
            }
        }

        static public void MarkHallWalls(ref AreaMap new_map, ref List<List<Point>> hall_paths)
        {
            int x_offset = 0; int y_offset = 0;

            for (int hall = 0; hall < hall_paths.Count; hall++)
            {
                for (int sqr = 0; sqr < hall_paths[hall].Count(); sqr++)
                {
                    for (int dir = 0; dir < Globals.Directions.Count(); dir += Globals.AllDirItr)
                    {
                        x_offset = Globals.Directions[dir].X + hall_paths[hall][sqr].X;
                        y_offset = Globals.Directions[dir].Y + hall_paths[hall][sqr].Y;

                        if (!new_map.OffMap(x_offset, y_offset))
                            if (new_map.ChkMSForState(ref new_map.MSGrid[y_offset][x_offset], MSFlag.BL, MSFlagIndex.BL_ST))
                                new_map.MSGrid[y_offset][x_offset].MSStates[(int)MSFlagIndex.WALL_RM_ENT] = MSFlag.PWALL;
                    }
                }
            }
        }

        static public void MakeHallWalls(ref AreaMap new_map)
        {
            int x_tile_max = new_map.widthTiles; int y_tile_max = new_map.heightTiles;

            for (int y = 0; y < y_tile_max; y++)
            {
                for (int x = 0; x < x_tile_max; x++)
                {
                    if (new_map.MSGrid[y][x].MSStates[(int)MSFlagIndex.WALL_RM_ENT] == MSFlag.PWALL)
                    {
                        new_map.MSGrid[y][x].MSStates[(int)MSFlagIndex.BL_ST] = MSFlag.NT_BL;
                        new_map.MSGrid[y][x].MSStates[(int)MSFlagIndex.WALL_RM_ENT] = MSFlag.WALL;
                        new_map.MSGrid[y][x].MSStates[(int)MSFlagIndex.PASSABLE] = MSFlag.BLKD;
                        MapGenHelper.AddRoomSqrTile(ref new_map, ref new_map.MSGrid[y][x], x, y);
                    }
                }
            }
        }

        #region Misc helper methods

        static public Vector4 FindClosestRoom(ref AreaMap new_map, int start_room_index, int room_count)
        {
            int shortest, distance, start_room_id, stop_room_id, end_room_index;
            shortest = distance = start_room_id = stop_room_id = end_room_index = -1;

            Point endRoomPT = new Point();
            Point curRoomWSqrPT = new Point(); Point endRoomWSqrPT = new Point();
            Point startWsqrPT = new Point(); Point endWsqrPT = new Point();

            List<Point> possible_starts = new List<Point>();
            List<Point> possible_ends = new List<Point>();

            List<int> ignored_rm_index = new List<int>();

            int wall_square_count = new_map.RoomList[start_room_index].WallSquares.Count();
            int otherRoomWSqrLst_count = 0;

            ignored_rm_index.Clear();
            ignored_rm_index.Add(start_room_index);

            // check each room
            for (int room = 0; room < room_count; room++)
            {
                if (!ignored_rm_index.Contains(room))
                {
                    if (AreRoomsConnected(ref new_map, room, start_room_index))
                    {
                        ignored_rm_index.Add(room);
                        continue;
                    }
                    // check each wall square in this room for path start square
                    for (int wall_square = 0; wall_square < wall_square_count; wall_square++)
                    {
                        curRoomWSqrPT = new_map.RoomList[start_room_index].WallSquares[wall_square];

                        // check if this first something
                        if (shortest < 0)
                        {
                            int rndEndRoomWSqrIndex = rand.Next(1, new_map.RoomList[room].WallSquares.Count - 1);
                            endRoomPT = new_map.RoomList[room].WallSquares[rndEndRoomWSqrIndex];

                            //while (new_map.RoomList[room].IsCorner(rndEndRoomWSqrIndex))
                            while (!CheckForGoodPathEnds(ref new_map, room, rndEndRoomWSqrIndex, ref endRoomPT))
                            {
                                rndEndRoomWSqrIndex = rand.Next(1, new_map.RoomList[room].WallSquares.Count - 1);
                                endRoomPT = new_map.RoomList[room].WallSquares[rndEndRoomWSqrIndex];
                            }
                         
                            shortest = PathFinder.StepDistance(curRoomWSqrPT, ref endRoomPT);
                        }

                        // check if start square is a corner or already an entrance
                        if (!CheckForGoodPathEnds(ref new_map, start_room_index, wall_square, ref curRoomWSqrPT))
                            continue;

                        otherRoomWSqrLst_count = new_map.RoomList[room].WallSquares.Count();

                        // check wall squares in other room
                        for (int other_square = 0; other_square < otherRoomWSqrLst_count; other_square++)
                        {
                            endRoomWSqrPT = new_map.RoomList[room].WallSquares[other_square];

                            // check if end square is a corner or already an entrance
                            if (!CheckForGoodPathEnds(ref new_map, room, other_square, ref endRoomWSqrPT))
                                continue;

                            // get distance from start_room[wall_square] to room[other_square]
                            distance = PathFinder.StepDistance(curRoomWSqrPT, ref endRoomWSqrPT);

                            if (distance <= shortest)
                            {
                                if (distance < shortest)
                                {
                                    possible_ends.Clear();
                                    possible_starts.Clear();
                                }

                                possible_ends.Add(endRoomWSqrPT);
                                possible_starts.Add(curRoomWSqrPT);

                                shortest = distance;
                                end_room_index = room;
                            }
                        }
                    }
                    if (possible_ends.Count == 0)
                        ignored_rm_index.Add(room);
                }
            }

            if (shortest == 1)
            {
                int square_index = rand.Next(0, possible_starts.Count() - 1);
                startWsqrPT = possible_starts[square_index];
                endWsqrPT = possible_ends[square_index];
            }
            else
            {
                if (possible_ends.Count != 0)
                {
                    startWsqrPT = possible_starts[rand.Next(0, possible_starts.Count() - 1)];
                    endWsqrPT = possible_ends[rand.Next(0, possible_ends.Count() - 1)];
                }
                else
                    return new Vector4(-1f, -1f, -1f, -1f);
            }

            start_room_id = new_map.MSGrid[(int)startWsqrPT.Y][(int)startWsqrPT.X].RoomID;
            stop_room_id = new_map.MSGrid[(int)endWsqrPT.Y][(int)endWsqrPT.X].RoomID;

            new_map.RoomList[start_room_id].OutConnections.Add(stop_room_id);
            new_map.RoomList[stop_room_id].InConnections.Add(start_room_id);

            new_map.MSGrid[(int)startWsqrPT.Y][(int)startWsqrPT.X].MSStates[(int)MSFlagIndex.WALL_RM_ENT] = MSFlag.ENT;
            new_map.MSGrid[(int)endWsqrPT.Y][(int)endWsqrPT.X].MSStates[(int)MSFlagIndex.WALL_RM_ENT] = MSFlag.ENT;

            return new Vector4(startWsqrPT.X, startWsqrPT.Y, endWsqrPT.X, endWsqrPT.Y);
        }

        static public void GetClosestRoomPath(ref AreaMap new_map, int start_room_index, ref List<int> includedRooms, ref List<Point> path)
        {
            int shortest, distance, start_room_id, stop_room_id;
            shortest = distance = start_room_id = stop_room_id = -1; 
            bool path_found = false;
            int room_count = new_map.RoomList.Count();
            Point curRoomWSqrPT = new Point(); Point endRoomWSqrPT = new Point();
            Point startWsqrPT = new Point(); Point endWsqrPT = new Point();

            List<int> ignored_rm_index = new List<int>();

            List<List<Point>> paths_list = new List<List<Point>>();
            int path_index = 0;
            Vector4 srt_n_fsh = new Vector4();

            int wall_square_count = new_map.RoomList[start_room_index].WallSquares.Count();
            int otherRoomWSqrLst_count = 0;

            ignored_rm_index.Clear();
            ignored_rm_index.Add(start_room_index);
            for (int r = 0; r < includedRooms.Count; r++) 
                ignored_rm_index.Add(includedRooms[r]);   

            // check for a room to connect with
            for (int room = 0; room < room_count; room++)
            {
                if (path_found)
                    break;

                if (!ignored_rm_index.Contains(room))
                {
                    if (AreRoomsConnected(ref new_map, room, start_room_index))
                    {
                        ignored_rm_index.Add(room);
                        continue;
                    }
                    // check each wall square in this room for path start square
                    for (int wall_square = 0; wall_square < wall_square_count; wall_square++)
                    {
                        if (path_found)
                            break;

                        curRoomWSqrPT = new_map.RoomList[start_room_index].WallSquares[wall_square];

                        if (!CheckForGoodPathEnds(ref new_map, start_room_index, wall_square, ref curRoomWSqrPT))
                            continue;

                        otherRoomWSqrLst_count = new_map.RoomList[room].WallSquares.Count();

                        // check wall squares in other room
                        for (int other_square = 0; other_square < otherRoomWSqrLst_count; other_square++)
                        {
                            if (path_found)
                                break;

                            endRoomWSqrPT = new_map.RoomList[room].WallSquares[other_square];

                            // check if end square is a corner or already an entrance
                            if (!CheckForGoodPathEnds(ref new_map, room, other_square, ref endRoomWSqrPT))
                                continue;

                            srt_n_fsh = new Vector4(curRoomWSqrPT.X, curRoomWSqrPT.Y, endRoomWSqrPT.X, endRoomWSqrPT.Y);

                            path.Clear();
                            PathFinder.Reset(ref new_map, ref srt_n_fsh);
                            PathFinder.Search(ref new_map, ref path, true);

                            // get distance from start_room[wall_square] to room[other_square]
                            distance = path.Count();

                            if (path.Count() <= 1)
                                continue;
                            else if (shortest < 0 || distance <= shortest)
                            {
                                if (distance < shortest)
                                {
                                    paths_list.Clear();
                                    path_index = 0;
                                }

                                paths_list.Add(new List<Point>());
                                for (int i = 0; i < path.Count(); i++)
                                    paths_list[path_index].Add(new Point(path[i].X, path[i].Y));

                                path_index++;
                                shortest = distance;
                                path_found = true;
                            }
                        }
                    }
                    if (paths_list.Count() == 0)
                        ignored_rm_index.Add(room);
                }
            }

            path.Clear();
            if (paths_list.Count() != 0)
            {
                path_index = rand.Next(0, paths_list.Count() - 1);
                startWsqrPT = paths_list[path_index][0];
                endWsqrPT = paths_list[path_index][paths_list[path_index].Count() - 1];
            }
            else
                return;

            start_room_id = new_map.MSGrid[(int)startWsqrPT.Y][(int)startWsqrPT.X].RoomID;
            stop_room_id = new_map.MSGrid[(int)endWsqrPT.Y][(int)endWsqrPT.X].RoomID;

            new_map.RoomList[start_room_id].OutConnections.Add(stop_room_id);
            new_map.RoomList[stop_room_id].InConnections.Add(start_room_id);

            new_map.MSGrid[(int)startWsqrPT.Y][(int)startWsqrPT.X].MSStates[(int)MSFlagIndex.WALL_RM_ENT] = MSFlag.ENT;
            new_map.MSGrid[(int)endWsqrPT.Y][(int)endWsqrPT.X].MSStates[(int)MSFlagIndex.WALL_RM_ENT] = MSFlag.ENT;

            for (int i = 0; i < paths_list[path_index].Count(); i++)
                path.Add(new Point(paths_list[path_index][i].X, paths_list[path_index][i].Y));
        }

        static public bool CheckForGoodPathEnds(ref AreaMap new_map, int room_index, int sqr_index, ref Point sqr_point)
        {        
            if (!new_map.ChkAdjacentForRoomID(ref new_map, ref sqr_point, room_index, Globals.AllDirItr, 4)) // is corner?
                return false;
            else if (new_map.ChkMSForState(ref new_map.MSGrid[sqr_point.Y][sqr_point.X], MSFlag.ENT, MSFlagIndex.WALL_RM_ENT)) // is entrance?
                return false;
            else if (!new_map.ChkAdjacentForMState(ref new_map, ref sqr_point, MSFlag.BL, MSFlagIndex.BL_ST, Globals.NESWItr, 1)) // is blocked off?
                return false;

            return true;
        }

        static public bool AreRoomsConnected(ref AreaMap new_map, int roomID_out, int roomID_in)
        {
            int out_count = new_map.RoomList[roomID_out].OutConnections.Count();
            int in_count = new_map.RoomList[roomID_in].InConnections.Count();
            int min_count = 0;

            if (out_count <= in_count)
                min_count = out_count;
            else
                min_count = in_count;

            for (int connection = 0; connection < min_count; connection++)
                if (new_map.RoomList[roomID_out].OutConnections[connection] == roomID_in)
                    if (new_map.RoomList[roomID_in].InConnections[connection] == roomID_out)
                        return true;

            return false;
        }

        static public bool FloodIsConnectedCheck(ref AreaMap new_map, bool unmark_after)
        {
            int adjacent_x = 0; int adjacent_y = 0;
            int curr_ID = 0;
            List<int> visited_rooms = new List<int>();
            Stack<Point> squares = new Stack<Point>();
            Point cur_sqr = Point.Zero;

            visited_rooms.Add(0);
            squares.Push(new_map.RoomList[0].RoomSquares[0]);

            if (new_map.MSGrid[new_map.RoomList[0].RoomSquares[0].Y]
                [new_map.RoomList[0].RoomSquares[0].X].MSStates[(int)MSFlagIndex.IS_MKD] == MSFlag.MKD)
                MapGenHelper.UnmarkMap(ref new_map); ///

            while (squares.Count > 0)
            {
                cur_sqr = squares.Pop();

                for (int dir = 0; dir < Globals.Directions.Count(); dir += Globals.NESWItr)
                {
                    adjacent_x = cur_sqr.X + Globals.Directions[dir].X; adjacent_y = cur_sqr.Y + Globals.Directions[dir].Y;

                    if (new_map.OffMap(adjacent_x, adjacent_y))
                        continue;

                    if (!new_map.ChkMSForState(ref new_map.MSGrid[adjacent_y][adjacent_x], MSFlag.WALL, MSFlagIndex.WALL_RM_ENT)) // changed: WALL->ROOM
                    {
                        if (new_map.ChkMSForState(ref new_map.MSGrid[adjacent_y][adjacent_x], MSFlag.NT_MKD, MSFlagIndex.IS_MKD))
                        {
                            curr_ID = new_map.MSGrid[adjacent_y][adjacent_x].RoomID;
                            new_map.MSGrid[adjacent_y][adjacent_x].MSStates[(int)MSFlagIndex.IS_MKD] = MSFlag.MKD;
                            squares.Push(new Point(adjacent_x, adjacent_y));

                            if (!visited_rooms.Contains(curr_ID))
                                visited_rooms.Add(curr_ID);
                        }
                    }
                }
            }

            MapGenHelper.disconnectedRooms.Clear();
            for (int i = 0; i < new_map.RoomList.Count(); i++)
                if (!visited_rooms.Contains(i))
                    MapGenHelper.disconnectedRooms.Add(i);

            if (MapGenHelper.disconnectedRooms.Count == 0)
                MapGenHelper.UnmarkMap(ref new_map);

            MapGenHelper.dcCount = MapGenHelper.disconnectedRooms.Count();

            if (MapGenHelper.disconnectedRooms.Count > 0)
                return false;

            return true;
        }

        #endregion
    }
}
