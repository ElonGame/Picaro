using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LibraryProject
{
    public class AreaMapTemplate
    {
        public string name;

        public int mapWidthTile;
        public int mapHeightTile;
        public int tileSize;

        public int maxRoomCount;

        public int currRoomTypeIndex;
        public Sizes currRoomSize;
        public Point currRoomSizePnt;

        public MapSets mapSet;
        public MapTypes mapType;
        public List<RoomTypes> roomTypes;
        public List<RoomDistribution> roomDistrib;

        public List<int> roomBarrierSizes;

        public List<int> roomTypeProbs;
        public List<List<Sizes>> roomSizesRange;
        public List<TSNamesIndex> tsNameIndices;

        public List<Room> RoomList;                 // list of rooms in map
        public List<string> TextureNameList;

        public AreaMapTemplate()
        {
            currRoomTypeIndex = 0;
            currRoomSize = Sizes.NULL;
            currRoomSizePnt = Point.Zero;

            roomTypes = new List<RoomTypes>(1);
            roomBarrierSizes = new List<int>();
            roomTypeProbs = new List<int>(1);
            roomSizesRange = new List<List<Sizes>>();
            tsNameIndices = new List<TSNamesIndex>(1);

            ///
            RoomList = new List<Room>();
            TextureNameList = new List<string>();
        }
    }
}
