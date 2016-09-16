#region File Description
//-----------------------------------------------------------------------------
// PathFinder.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace LibraryProject
{
    #region Search Status Enum
    public enum SearchStatus
    {
        Stopped,
        Searching,
        NoPath,
        PathFound,
    }
    #endregion

    #region Search Method Enum
    public enum SearchMethod
    {
        BreadthFirst,
        BestFirst,
        AStar,
        Max,
    }
    #endregion

    static public class PathFinder
    {
        static public List<Point> OpenMapTilesLst;
        static public int lstIndex;

        #region Search Node Struct
        /// <summary>
        /// Reresents one node in the search space
        /// </summary>
        private struct SearchNode
        {
            /// <summary>
            /// Location on the map
            /// </summary>
            public Point Position;

            /// <summary>
            /// Distance to goal estimate
            /// </summary>
            public int DistanceToGoal;
            
            /// <summary>
            /// Distance traveled from the start
            /// </summary>
            public int DistanceTraveled;

            public SearchNode(
                Point mapPosition, int distanceToGoal, int distanceTraveled)
            {
                Position = mapPosition;
                DistanceToGoal = distanceToGoal;
                DistanceTraveled = distanceTraveled;
            }
        }
        #endregion

        #region Constants        

        #endregion

        #region Fields

        // Holds search nodes that are avaliable to search
        static private List<SearchNode> openList;
        // Holds the nodes that have already been searched
        static private List<SearchNode> closedList;
        // Holds all the paths we've creted so far
        static private Dictionary<Point, Point> paths;

        static private Point start;
        static private Point goal;
        static private bool ignoreHallWalls;

        #endregion
        
        #region Properties

        // Tells us if the search is stopped, started, finished or failed
        static public SearchStatus SearchStatus
        {
            get { return searchStatus; }
        }
        static private SearchStatus searchStatus;

        // Tells us which search type we're using right now
        static public SearchMethod SearchMethod
        {
            get { return searchMethod; }
        }
        static private SearchMethod searchMethod = SearchMethod.BestFirst;

        /// <summary>
        /// Toggles searching on and off
        /// </summary>
        static public bool IsSearching
        {
            get { return searchStatus == SearchStatus.Searching; }
            set 
            {
                if (searchStatus == SearchStatus.Searching)
                {
                    searchStatus = SearchStatus.Stopped;
                }
                else if (searchStatus == SearchStatus.Stopped)
                {
                    searchStatus = SearchStatus.Searching;
                }
            }
        }

        /// <summary>
        /// How many search steps have elapsed on this map
        /// </summary>
        static public int TotalSearchSteps
        {
            get { return totalSearchSteps; }
        }
        static private int totalSearchSteps = 0;

        #endregion

        #region Initialization

        static public void Initialize()
        {
            searchStatus = SearchStatus.Stopped;
            openList = new List<SearchNode>();
            closedList = new List<SearchNode>();
            paths = new Dictionary<Point, Point>();

            OpenMapTilesLst = new List<Point>();
            for (int i = 0; i < 4; i++)
                OpenMapTilesLst.Add(Point.Zero);

            lstIndex = 0;
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Search Update
        /// </summary>
        //public void Update(GameTime gameTime)
        //{
        //    //if (searchStatus == SearchStatus.Searching)
        //    //{
        //    while (searchStatus != SearchStatus.NoPath || searchStatus != SearchStatus.PathFound)
        //    {
        //        DoSearchStep();
        //    }
        //    //}
        //}       

        #endregion

        #region Methods

        /// <summary>
        /// Reset the search
        /// </summary>
        static public void Reset(ref AreaMap current_map, ref Vector4 positions)
        {
            Point start_pos = new Point((int)positions.X, (int)positions.Y);
            Point end_pos = new Point((int)positions.Z, (int)positions.W);

            searchStatus = SearchStatus.Searching;
            totalSearchSteps = 0;

            openList.Clear();
            closedList.Clear();
            paths.Clear();

            start = start_pos;
            goal = end_pos;
            ignoreHallWalls = false;
            openList.Add(new SearchNode(start_pos, StepDistance(start_pos, ref end_pos), 0));
        }

        /// <summary>
        /// Cycle through the search method to the next type
        /// </summary>
        //public void NextSearchType()
        //{
        //    searchMethod = (SearchMethod)(((int)searchMethod + 1) % 
        //        (int)SearchMethod.Max);
        //}

        /// <summary>
        /// This method find the next path node to visit, puts that node on the 
        /// closed list and adds any nodes adjacent to the visited node to the 
        /// open list.
        /// </summary>
        /// 
        static public void Search(ref AreaMap search_map, ref List<Point> returned_path, bool ignore_walls)
        {
            ignoreHallWalls = ignore_walls;
            while (searchStatus == SearchStatus.Searching)
                DoSearchStep(ref search_map, ref returned_path);
        }

        static private void DoSearchStep(ref AreaMap search_map, ref List<Point> returned_path)
        {
            SearchNode newOpenListNode;

            bool foundNewNode = SelectNodeToVisit(out newOpenListNode);
            if (foundNewNode)
            {
                Point currentPos = newOpenListNode.Position;
                GetOpenMapTiles(ref search_map, ref currentPos);

                foreach (Point pos_point in OpenMapTilesLst)
                {
                    SearchNode mapTile = new SearchNode(pos_point, 
                        StepDistanceToEnd(pos_point, ref goal), 
                        newOpenListNode.DistanceTraveled + 1);
                    if (!InList(openList, pos_point) &&
                        !InList(closedList, pos_point))
                    {
                        openList.Add(mapTile);
                        paths[pos_point] = newOpenListNode.Position;
                    }
                }
                if (currentPos == goal)
                {
                    searchStatus = SearchStatus.PathFound;
                    foreach (Point pnt in FinalPath())
                        returned_path.Add(pnt);

                }
                openList.Remove(newOpenListNode);
                closedList.Add(newOpenListNode);
            }
            else
            {
                searchStatus = SearchStatus.NoPath;
                if(returned_path.Count == 0)
                {
                    search_map.MSGrid[(int)start.Y][(int)start.X].BaseColor = Color.Blue;
                    search_map.MSGrid[(int)start.Y][(int)start.X].MSStates[(int)MSFlagIndex.WALL_RM_ENT] = MSFlag.WALL;
                    search_map.MSGrid[(int)goal.Y][(int)goal.X].BaseColor = Color.Purple;
                }
            }
        }

        /// <summary>
        /// Determines if the given Point is inside the SearchNode list given
        /// </summary>
        static private bool InList(List<SearchNode> list, Point pos_point)
        {
            bool inList = false;
            foreach (SearchNode node in list)
                if (node.Position == pos_point)
                    inList = true;

            return inList;
        }

        /// <summary>
        /// This Method looks at everything in the open list and chooses the next 
        /// path to visit based on which search type is currently selected.
        /// </summary>
        /// <param name="result">The node to be visited</param>
        /// <returns>Whether or not SelectNodeToVisit found a node to examine
        /// </returns>
        static private bool SelectNodeToVisit(out SearchNode result)
        {
            result = new SearchNode();
            bool success = false;
            float smallestDistance = float.PositiveInfinity;
            float currentDistance = 0f;
            if (openList.Count > 0)
            {
                //switch (searchMethod)
                //{
                    // Breadth first search looks at every possible path in the 
                    // order that we see them in.
                    //case SearchMethod.BreadthFirst:
                    //    totalSearchSteps++;
                    //    result = openList[0];
                    //    success = true;
                    //    break;
                    //// Best first search always looks at whatever path is closest to
                    //// the goal regardless of how long that path is.
                    //case SearchMethod.BestFirst:
                    //    totalSearchSteps++;
                    //    foreach (SearchNode node in openList)
                    //    {
                    //        currentDistance = node.DistanceToGoal;
                    //        if(currentDistance < smallestDistance){
                    //            success = true;
                    //            result = node;
                    //            smallestDistance = currentDistance;
                    //        }
                    //    }
                    //    break;
                    // A* search uses a heuristic, an estimate, to try to find the 
                    // best path to take. As long as the heuristic is admissible, 
                    // meaning that it never over-estimates, it will always find 
                    // the best path.
                    //case SearchMethod.AStar:
                        totalSearchSteps++;
                        foreach (SearchNode node in openList)
                        {
                            currentDistance = Heuristic(node);
                            // The heuristic value gives us our optimistic estimate 
                            // for the path length, while any path with the same 
                            // heuristic value is equally ‘good’ in this case we’re 
                            // favoring paths that have the same heuristic value 
                            // but are longer.
                            if (currentDistance <= smallestDistance)
                            {
                                if (currentDistance < smallestDistance)
                                {
                                    success = true;
                                    result = node;
                                    smallestDistance = currentDistance;
                                }
                                else if (currentDistance == smallestDistance &&
                                    node.DistanceTraveled > result.DistanceTraveled)
                                {
                                    success = true;
                                    result = node;
                                    smallestDistance = currentDistance;
                                }
                            }
                        }
                        //break;
                //}
            }
            return success;
        }

        /// <summary>
        /// Generates an optimistic estimate of the total path length to the goal 
        /// from the given position.
        /// </summary>
        /// <param name="location">Location to examine</param>
        /// <returns>Path length estimate</returns>
        static private float Heuristic(SearchNode location)
        {
            return location.DistanceTraveled + location.DistanceToGoal;
        }

        /// <summary>
        /// Generates the path from start to end.
        /// </summary>
        /// <returns>The path from start to end</returns>
        static public LinkedList<Point> FinalPath()
        {
            LinkedList<Point> path = new LinkedList<Point>();
            if (searchStatus == SearchStatus.PathFound)
            {
                Point curPrev = goal;
                path.AddFirst(curPrev);
                while (paths.ContainsKey(curPrev))
                {
                    curPrev = paths[curPrev];
                    path.AddFirst(curPrev);
                }
            }
            return path;
        }

        /// <summary>
        /// Returns true if the given map location exists and is not 
        /// blocked by a barrier
        /// </summary>
        /// <param name="column">column position(x)</param>
        /// <param name="row">row position(y)</param>
        static private bool IsOpen(ref AreaMap search_map, int column, int row)
        {
            if (!search_map.OffMap(column, row))
            {
                if (column == goal.X)
                    if (row == goal.Y)
                        return true;

                if (search_map.ChkMSForState(ref search_map.MSGrid[row][column], MSFlag.BL, MSFlagIndex.BL_ST))
                    return true;

                if(ignoreHallWalls)
                    if (search_map.MSGrid[row][column].RoomID == -1)
                        return true;

            }
            return false;
        }

        /// <summary>
        /// Enumerate all the map locations that can be entered from the given 
        /// map location
        /// </summary>
        static public void GetOpenMapTiles(ref AreaMap search_map, ref Point map_loc)
        {
            OpenMapTilesLst.Clear();

            if (IsOpen(ref search_map, map_loc.X, map_loc.Y + 1))
                OpenMapTilesLst.Add(new Point(map_loc.X, map_loc.Y + 1));

            if (IsOpen(ref search_map, map_loc.X, map_loc.Y - 1))
                OpenMapTilesLst.Add(new Point(map_loc.X, map_loc.Y - 1));
       
            if (IsOpen(ref search_map, map_loc.X + 1, map_loc.Y))
                OpenMapTilesLst.Add(new Point(map_loc.X + 1, map_loc.Y));
            
            if (IsOpen(ref search_map, map_loc.X - 1, map_loc.Y))
                OpenMapTilesLst.Add(new Point(map_loc.X - 1, map_loc.Y));           
        }

        /// <summary>
        /// Finds the minimum number of tiles it takes to move from Point A to 
        /// Point B if there are no barriers in the way
        /// </summary>
        /// <param name="pointA">Start position</param>
        /// <param name="pointB">End position</param>
        /// <returns>Distance in tiles</returns>
        static public int StepDistance(Point pos_A, ref Point pos_B)
        {
            int distanceX = Math.Abs(pos_A.X - pos_B.X);
            int distanceY = Math.Abs(pos_A.Y - pos_B.Y);

            return distanceX + distanceY;
        }

        /// <summary>
        /// Finds the minimum number of tiles it takes to move from the current 
        /// position to the end location on the Map if there are no barriers in 
        /// the way
        /// </summary>
        /// <param name="pos_vec">Current position</param>
        /// <returns>Distance to end in tiles</returns>
        static public int StepDistanceToEnd(Point start_pos, ref Point end_pos)
        {
            return StepDistance(start_pos, ref end_pos);
        }

        #endregion
    }
}
