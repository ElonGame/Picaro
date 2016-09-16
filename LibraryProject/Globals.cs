using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LibraryProject
{
    static public class Globals
    {
        // misc
        static public StatHelper statHelper;

        // temp debugging flags
        static public bool DrawHallHeightsLinear = true;
        static public bool GetMapData = false;

        static public Random rand;

        // Map
        static public Vector2 mDrawOrg = new Vector2(550 + 85, 10);
        static public int tileSize = 55;

        // heights;
        static public List<float> Heights;
        static public int HeightMax = 0;
        static private float minus5 = 0f;
        static private float minus4 = 0.2f;
        static private float minus3 = 0.4f;
        static private float minus2 = 0.6f;
        static private float minus1 = 0.8f;
        static private float ground = 1.0f;
        static private float plus1 = 15.0f;
        static private float plus2 = 50.0f;
        static private float plus3 = 100.0f;
        static private float plus4 = 150.0f;
        static private float plus5 = 200.0f;
        static private float plus6 = 250.0f;

        //static public List<float> Heights;
        //static public int HeightMax = 0;
        //static private float minus4 = -1.0f;
        //static private float minus3 = -0.8f;
        //static private float minus2 = -0.6f;
        //static private float minus1 = -0.3f;
        //static private float ground = 0.0f;
        //static private float plus1 = 1.0f;
        //static private float plus2 = 20.0f;
        //static private float plus3 = 50.0f;
        //static private float plus4 = 100.0f;
        //static private float plus5 = 150.0f;
        //static private float plus6 = 200.0f;
        //static private float plus7 = 250.0f;        

        // sizes
        static public List<int> sizeRanges;

        static private int tinyLow = 3;
        static private int tinyHigh = 6;
        static private int smallLow = 7;
        static private int smallHigh = 10;
        static private int medLow = 11;
        static private int medHigh = 16;
        static private int largeLow = 17;
        static private int largeHigh = 22;
        static private int hugeLow = 21;
        static private int hugeHigh = 26;

        static public int mapMult = 5;

        // map algorithm
        static public int maxMakeRoomFails = 100;
        static public bool roomFail = false;

        static public int maxRoomCount = 15;

        // directions (y,x)
        static public List<Point> Directions;

        static public Point N = new Point(0, -1);
        static public Point NE = new Point(1, -1);
        static public Point E = new Point(1, 0);
        static public Point SE = new Point(1, 1);
        static public Point S = new Point(0, 1);
        static public Point SW = new Point(-1, 1);
        static public Point W = new Point(-1, 0);
        static public Point NW = new Point(-1, -1);

        static public int AllDirItr = 1;
        static public int NESWItr = 2;
        static public int NumDirects = 8;

        static public void Initialize()
        {
            statHelper = new StatHelper();

            rand = new Random();

            Heights = new List<float>(12);
            Heights.Add(minus5);
            Heights.Add(minus4);
            Heights.Add(minus3);
            Heights.Add(minus2);
            Heights.Add(minus1);
            Heights.Add(ground);
            Heights.Add(plus1);
            Heights.Add(plus2);
            Heights.Add(plus3);
            Heights.Add(plus4);
            Heights.Add(plus5);
            Heights.Add(plus6);
            //Heights.Add(plus7);
            HeightMax = (Heights.Count() - 1);

            sizeRanges = new List<int>(10);
            sizeRanges.Add(tinyLow);
            sizeRanges.Add(tinyHigh);
            sizeRanges.Add(smallLow);
            sizeRanges.Add(smallHigh);
            sizeRanges.Add(medLow);
            sizeRanges.Add(medHigh);
            sizeRanges.Add(largeLow);
            sizeRanges.Add(largeHigh);
            sizeRanges.Add(hugeLow);
            sizeRanges.Add(hugeHigh);

            Directions = new List<Point>(8);
            Directions.Add(N);
            Directions.Add(NE);
            Directions.Add(E);
            Directions.Add(SE);
            Directions.Add(S);
            Directions.Add(SW);
            Directions.Add(W);
            Directions.Add(NW);


        }

        static public Point GetSizeRange(MakeTarget target, Sizes size)
        {
            Point range = new Point();

            switch (size)
            {
                case Sizes.TINY:
                    range.X = Globals.tinyLow;
                    range.Y = Globals.tinyHigh;
                    break;
                case Sizes.SMALL:
                    range.X = Globals.smallLow;
                    range.Y = Globals.smallHigh;
                    break;
                case Sizes.MEDIUM:
                    range.X = Globals.medLow;
                    range.Y = Globals.medHigh;
                    break;
                case Sizes.LARGE:
                    range.X = Globals.largeLow;
                    range.Y = Globals.largeHigh;
                    break;
                case Sizes.HUGE:
                    range.X = Globals.hugeLow;
                    range.Y = Globals.hugeHigh;
                    break;
            }

            if (target == MakeTarget.MAP)
            {
                range.X *= Globals.mapMult;
                range.Y *= Globals.mapMult;
            }

            return range;
        }

        //static public int GetRoomDistance(Distances dist, ref Point size_pnt)//Sizes roomSize)//, ref Point keep_size)
        //{
        //    int distance = 0;
        //    Point size_range = Point.Zero;

        //    distance = GetIntFromRange(size_pnt);

        //    switch(dist)
        //    {
        //        case Distances.TOUCHING:
        //            distance = 1;
        //            break;
        //        case Distances.CLOSE:
        //            distance *= 1;
        //            break;
        //        case Distances.MIDDLE:
        //            distance *= 2;
        //            break;
        //        case Distances.FAR:
        //            distance *= 3;
        //            break;
        //        case Distances.VERY_FAR:
        //            distance *= 4;
        //            break;
        //    }


        //    return distance;
        //}

        static public int GetIntFromRange(Point rang)
        {
            int magnitude = 0;

            magnitude = rand.Next(rang.X, rang.Y);

            return magnitude;
        }

        static public Point GetPointFromRange(Point rang)
        {
            Point range = new Point();

            range.X = rand.Next(rang.X, rang.Y);
            range.Y = rand.Next(rang.X, rang.Y);

            return range;
        }

        static public Point GetPointFromRange(int min, int max)
        {
            Point range = new Point();

            range.X = rand.Next(min, max);
            range.Y = rand.Next(min, max);

            return range;
        }

        static public Point AddPoints(ref Point pt1, ref Point pt2)
        {
            return new Point(pt1.X + pt2.X, pt1.Y + pt2.Y);
        }

        #region Graphics Stuff

        static public int menuEntriesColorShift = -50;
        static public int selectedEntryColorShift = 50;

        #endregion
    }
}
