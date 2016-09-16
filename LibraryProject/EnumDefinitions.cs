using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryProject
{
    /* 1 */ public enum Variance { UNIFORM, LITTLE, SOME, MIDDLE, GREATLY, CHAOS}                                       // 6
    /* 2 */ public enum Sizes { NULL = -1, TINY = 0, SMALL = 1, MEDIUM = 2, LARGE = 3, HUGE = 4 }                       // 6
    /* 3 */ public enum Distances { NULL = -1, TOUCHING = 0, CLOSE = 1, MIDDLE = 2, FAR = 3, VERY_FAR = 5 }             // 6
    /* 4 */ public enum Heights { INC, PEAK, DEC, BASE, NULL }                                                          // 5

    /* 5 */ public enum MakeTarget { ROOM, MAP }                                                                        // 2

    // ALL of the different "sets" (set of map characteristics; like MapTypes, RoomTypes, possible tile sets, etc..)
    // are to be added here
    // PURPOSE: enum value matches up with index of MapGenerator.mapSetNames to link
    //      MapSet ENUM to FILE PATH of a .txt file containing data to be loaded into
    //      an AreaMapType class
    /* 6 */ public enum MapSets { NULL = -1, TEST_MAP_SET = 0, }                                                        // 2
    /* 7 */ public enum RoomDistribution { NULL = -1, RANDOM = 0, PATTERN = 1, SHAPE = 2 }                              // 4
    /* 8 */ public enum RPattern { NULL = -1 }                                                                          // 1
    /* 9 */ public enum RShape { NULL = -1, GRID = 0, CROSS = 1 }                                                       // 3

    // The basic form of the map, e.g. rooms connected by halls, rooms within one large enclosing room, etc...
    /* 10 */ public enum MapTypes { NULL, ROOMS, OPEN, ENCLOSED_ROOMS }                                                 // 4

    /// <summary>
    /// enums used for picking types of rooms to make in MapGenerator?
    /// </summary>
    /* 11 */ public enum RoomTypes { NULL, RECTANGLE, COMPOUND_RECTANGLE }                                              // 4

    /// <summary>
    /// enums used for List<List<List<Vector2>>> AreaMap.RoomSquaresList, second dimension for
    /// accessing List<List<Vector2>> of either MapSquares flagged ROOM or WALL;
    /// e.g. RoomSquareList[1][ROOM], where "1" is the second "room" in the outer List,
    /// and ROOM accesses the List<Vector2> for the MapSquare "map coordinates" 
    /// that are flagged ROOM in the second "room"... you would access this list if you
    /// wanted to check all the ROOM/WALL MapSquares of a particular room (which "room"
    /// a MapSquare belongs to can be determined by checking its roomID, as this value and the
    /// index of AreaMap.RoomSquares used to reference "rooms" are set the same by
    /// MapGenerator when the AreaMap is made)
    /// </summary>
    /* 12 */ public enum RoomListIndices { ROOM, WALL }                                                                 // 2

    // NT_MK = not marked
    // MK = marked
    // NULL = null
    // BL = blank
    // NOT_BL = not blank
    // WALL = wall
    // PWALL = pre-wall
    // PROOM = pre-room
    // MAPEDGE = not used! <----
    // BLKD = blocked
    // NT_BLKD = not blocked
    // MAKE_OK =
    // NO_MAKE =
    /* 13 */                                                                                                            // 15
    public enum MSFlag
    {
        NT_MKD, MKD, NULL, BL, NT_BL, WALL, PWALL, ROOM, PROOM, ENT, MAPEDGE, BLKD, NT_BLKD, MAKE_OK, NO_MAKE,
        NO_VIS, VLOW_VIS, LOW_VIS, MED_VIS, HIGH_VIS
    }

    /// <summary>
    /// enums that indicate what an index in MapSquare.MapSquareStates is used for
    /// </summary>
    /* 14 */ public enum MSFlagIndex { NULL, BL_ST, WALL_RM_ENT, PASSABLE, IS_MKD, CAN_MAKE, VIS_LVL }                  // 6

    /* 15 */ public enum MSTileIndex { BG, FG }                                                                         // 2

    /* 16 */ public enum MSMakeIndex { TILE_BG, TILE_FG, ANM_BG, ANM_FG }                                               // 4

    // enums that are used as identifiers for indices in TileSetManager.tileSetsName
    // 
    // PURPOSE: enum value matches up with index of TileSetManager.tileSetsNames to link 
    //      TileSetNamesIndices ENUM to FILE PATH of a TileSet .txt file
    /* 17 */ public enum TSNamesIndex { BLANK, TEST_TILE_SET_NEW }//, TEST_TILE_SET_NEW2 }                              // 2

    /* 18 */ public enum TileSetFlags { OK, COMMENT }                                                                   // 2

    // Enums indexes used to reference corresponding direction in Globals.Directions
    /* 19 */ public enum Directions { N, NE, E, SE, S, SW, W, NW }                                                      // 8

    public enum PrimStat
    { STR, CON, DEX, AGI, INT, SPI, LCK }

    public enum SecStat
    { HP, MP, AC, SPD, EV, PHYS_MIN, PHYS_MAX }

    public enum CondRes
    { POI_RES, DIS_RES, PARA_RES, MUT_RES, BLI_RES, SIL_RES, CHA_RES, CONF_RES, FEAR_RES }

    public enum EleRes
    { FIRE_RES, ICE_RES, WAT_RES, EAR_RES, AIR_RES, LIT_RES, LIGHT_RES, DARK_RES, ARC_RES, VOID_RES, CHAOS_RES }

    static public class EnumDefinitions
    {
        //static public int enumTypes;
        //static public List<List<Enum>>IntsToEnumList;


        //static public void Initialize()
        //{
        //    enumTypes = 19;
        //    IntsToEnumList = new List<List<Enum>>(enumTypes);
        //    IntsToEnumList[0] = (new List<Enum>(6));
        //    IntsToEnumList[0][0] = Variance.UNIFORM;

        //    IntsToEnumList[1] = (new List<int>(6));
        //    IntsToEnumList[2] = (new List<int>(6));
        //    IntsToEnumList[3] = (new List<int>(5));
        //    IntsToEnumList[4] = (new List<int>(2));
        //    IntsToEnumList[5] = (new List<int>(2));
        //    IntsToEnumList[6] = (new List<int>(4));
        //    IntsToEnumList[7] = (new List<int>(1));
        //    IntsToEnumList[8] = (new List<int>(3));
        //    IntsToEnumList[9] = (new List<int>(4));
        //    IntsToEnumList[10] = (new List<int>(4));
        //    IntsToEnumList[11] = (new List<int>(2));
        //    IntsToEnumList[12] = (new List<int>(15));
        //    IntsToEnumList[13] = (new List<int>(6));
        //    IntsToEnumList[14] = (new List<int>(2));
        //    IntsToEnumList[15] = (new List<int>(4));
        //    IntsToEnumList[16] = (new List<int>(2));
        //    IntsToEnumList[17] = (new List<int>(2));
        //    IntsToEnumList[18] = (new List<int>(8));
        //}

        //static public Enum IntToMSIndex(int index)
        //{
        //    Enum msIndex = Enum.;

        //    return msIndex;
        //}
    }
}
