using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;

namespace LibraryProject
{
    [Serializable]
    public class SavedMap
    {
        public int squareSize;
        public int mapWidth;
        public int mapHeight;

        public string squareTextureNameList;

        public string notBlankSquareGrid;
        public string roomIDGrid;
        public string altitudeGrid;

        public string tex2DIndices;

        public string BGtiles;
        public string FGtiles;

        public string MsStates;

        public string StartPnt;

        public SavedMap()
        { }
    }

    //[Serializable]
    //public struct AreaMapGeneralInfo
    //{
    //    public int mapWidth;
    //    public int mapHeight;
    //    public string squareTextureNameList;

    //    public AreaMapGeneralInfo(int map_width, int map_height, ref string square_texture_name_list)
    //    {
    //        mapWidth = map_width;
    //        mapHeight = map_height;
    //        squareTextureNameList = square_texture_name_list;
    //    }

    //    //public string roomIDs;
    //    //public string altitues;

    //    //public int numNonBlankSquares;
    //    //public string nonBlankSquares;
    //}

    //public struct CondensedString
    //{
    //    public string data_str;

    //    public CondensedString(ref string str)
    //    {
    //        data_str = str;
    //    }
    //}

    //public struct MapSquareStruct
    //{
    //    public int xIndex;
    //    public int yIndex;
    //    public int squareSize;

    //    public int tex2DIdx;
    //    public TileStruct BGTile;
    //    public TileStruct FGTile;

    //    public float altitude;

    //    public int roomID;

    //    public List<MSFlag> states;

    //    public MapSquareStruct(int x, int y, int s, int i, TileStruct BG, TileStruct FG, float a, int r, List<MSFlag> sts)
    //    {
    //        xIndex = x;
    //        yIndex = y;
    //        squareSize = s;
    //        tex2DIdx = i;
    //        BGTile = BG;
    //        FGTile = FG;
    //        altitude = a;
    //        roomID = r;
    //        states = sts;
    //    }
    //}

    //public struct TileStruct
    //{
    //    public int Layer;
    //    public int xSource;
    //    public int ySource;
    //    public int sourceSize;

    //    public bool IsAnimated;
    //    public int FrameCount;
    //    public float FrameLength;

    //    public TileStruct(int l, int xs, int ys, int ss, bool a, int fc, float fl)
    //    {
    //        Layer = l;
    //        xSource = xs;
    //        ySource = ys;
    //        sourceSize = ss;
    //        IsAnimated = a;
    //        FrameCount = fc;
    //        FrameLength = fl;         
    //    }
    //}
}
