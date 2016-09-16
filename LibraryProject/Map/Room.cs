using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LibraryProject
{
    public class Room
    {
        private int roomID;

        private int xOrigin;
        private int yOrigin;
        private int width;
        private int height;

        public int xMax;
        public int yMax;

        private int tileSize;

        private List<int> inConnections;
        private List<int> outConnections;

        private List<Point> allSquares;
        private List<Point> wallSquares;
        private List<Point> roomSquares;

        public int XOrigin
        {
            get { return xOrigin; }
            set { xOrigin = value; }
        }

        public int YOrigin
        {
            get { return yOrigin; }
            set { yOrigin = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public List<int> InConnections
        {
            get { return inConnections; }
            set { inConnections = value; }
        }

        public List<int> OutConnections
        {
            get { return outConnections; }
            set { outConnections = value; }
        }

        public List<Point> AllSquares
        {
            get { return allSquares; }
            set { allSquares = value; }
        }

        public List<Point> WallSquares
        {
            get { return wallSquares; }
            set { wallSquares = value; }
        }

        public List<Point> RoomSquares
        {
            get { return roomSquares; }
            set { roomSquares = value; }
        }

        public Room(int room_id, int tile_size)
        {
            roomID = room_id;
            tileSize = tile_size;

            inConnections = new List<int>();
            outConnections = new List<int>();
            wallSquares = new List<Point>();
            roomSquares = new List<Point>();
        }

        public Room(int room_id, Rectangle room_bounds)
        {
            roomID = room_id;
            tileSize = Globals.tileSize;

            inConnections = new List<int>();
            outConnections = new List<int>();

            wallSquares = new List<Point>();
            roomSquares = new List<Point>();
            allSquares = new List<Point>();

            xOrigin = room_bounds.X;
            yOrigin = room_bounds.Y;
            width = room_bounds.Width;
            height = room_bounds.Height;
            xMax = xOrigin + width;
            yMax = yOrigin + height;
        }

        //public Vector2 GetSquareOrigin(RoomListIndices square_type, int index)
        //{

        //    if(square_type == RoomListIndices.ROOM)
        //        return new Vector2(roomSquares[index].X, roomSquares[index].Y);
        //    else if (square_type == RoomListIndices.WALL)
        //        return new Vector2(wallSquares[index].X, wallSquares[index].Y);

        //    return Vector2.Zero;
        //}

        //public bool IsCorner(int index)
        //{
        //    if (((wallSquares[index].X == xOrigin) && (wallSquares[index].Y == yOrigin)) ||
        //        ((wallSquares[index].X == xOrigin) && (wallSquares[index].Y == (yOrigin + height - 1))) ||
        //        ((wallSquares[index].X == (xOrigin + width - 1)) && (wallSquares[index].Y == yOrigin)) ||
        //        ((wallSquares[index].X == (xOrigin + width - 1)) && (wallSquares[index].Y == (yOrigin + height - 1))))
        //        return true;

        //    return false;
        //}
    }
}
