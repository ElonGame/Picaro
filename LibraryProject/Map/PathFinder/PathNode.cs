using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LibraryProject
{
    public class PathNode
    {
        public PathNode parent;

        public Point position;
        public Point pathEnd;
        public int path_cost;
        public int step_cost;
        public int heur_cost;

        public PathNode(ref PathNode parent_node, Point grid_pos, Point end)
        {
            parent = parent_node;
            position = grid_pos;
            pathEnd = end;
        }

        //public bool ScanLinEnd(ref AreaMap map)
        //{
        //    int start_x = position.X;
        //    int start_y = position.Y;
        //    int end_x = pathEnd.X;
        //    int end_y = pathEnd.Y;
        //    int x_step = map.GetManStep(start_x, end_x);
        //    int y_step = map.GetManStep(start_y, end_y);

        //    if (start_x == end_x)
        //    {
        //        for(int y = start_y; y <= end_y; y++)
        //        {
        //            y += y_step;
        //            if (map.MSGrid[y][start_x].MSStates[(int)MSFlagIndex.BL_ST] != MSFlag.BL)
        //                return false;
        //        }
        //    }
        //    else if (start_y == end_y)
        //    {
        //        for (int x = start_x; x <= end_x; x++)
        //        {
        //            x += x_step;
        //            if (map.MSGrid[x][start_y].MSStates[(int)MSFlagIndex.BL_ST] != MSFlag.BL)
        //                return false;
        //        }
        //    }

        //    return true;
        //}
         
    }
}
