using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LibraryProject.Map
{
    static public class MapGenerator
    {
        static private string line;
        static private string lineStop = "***";

        static public AreaMapType areaMapType;

        static private List<string> mapSetNames;
        static private List<TileSet> loadedTileSets;

        static public List<TileSet> LoadedTileSets
        {
            get { return loadedTileSets; }
            set { loadedTileSets = value; }
        }

        static private TileSet currentTileSet;

        static public TileSet CurrentTileSet
        {
            get { return currentTileSet; }
            set { currentTileSet = value; }
        }

        #region Initialization

        static public void Initialize()
        {
            LoadMapSetNames();
            MapGenHelper.Initialize();
        }

        static private void LoadMapSetNames() //-------------------- ALL map set path names are added here
        {
            mapSetNames = new List<string>();
            mapSetNames.Add(@"Content\Maps\MapSets\TestMapSet.txt");
        }

        #endregion

        static public void GenerateMap(ref AreaMap current_map, ref List<string> currMapTexNames, MapSets map_set, Sizes map_size,
            ref List<int> dcRoomList)
        {
            areaMapType = new AreaMapType();
            LoadMapSet(map_set, ref areaMapType);
            MapGenHelper.MakeMap(ref current_map, ref areaMapType, ref currMapTexNames, map_size);
            MapGenHelper.GetMapData(ref dcRoomList);
        }

        static public void LoadMapSet(MapSets map_set, ref AreaMapType map_template)
        {
            line = string.Empty;
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream(mapSetNames[(int)map_set]); // <------ this is where the MapSets enum is used
                System.IO.StreamReader map_set_txt_file = new System.IO.StreamReader(stream);

                if ((line = map_set_txt_file.ReadLine()) == "START")
                {
                    GetName(ref map_template, "MAP_SET_NAME", ref map_set_txt_file);
                    GetMapType(ref map_template, "MAP_TYPE", ref map_set_txt_file);
                    GetRoomTypes(ref map_template, "ROOM_TYPES", ref map_set_txt_file);
                    GetRoomTypeProbabilities(ref map_template,"ROOM_PROBABILITIES", ref map_set_txt_file);
                    GetRoomSizesRange(ref map_template, "ROOM_SIZES_RANGE", ref map_set_txt_file);
                    GetRoomBarriers(ref map_template, "ROOM_BARRIER_SIZES", ref map_set_txt_file);
                    GetTileSets(ref map_template, "TILE_SETS", ref map_set_txt_file);
                }

                map_set_txt_file.Close();
            }
            catch (System.IO.FileNotFoundException)
            {
                //temp_game.Exit();
            }
        }

        static private void GetName(ref AreaMapType template, string start, ref System.IO.StreamReader reader)
        {
            line = reader.ReadLine();

            if (line == start)
                while ((line = reader.ReadLine()) != lineStop)
                    if (CheckLine(ref line) == TileSetFlags.OK)
                        template.name = line;
        }

        static private void GetMapType(ref AreaMapType template, string start, ref System.IO.StreamReader reader)
        {
            line = reader.ReadLine();

            if (line == start)
                while ((line = reader.ReadLine()) != lineStop)
                    if (CheckLine(ref line) == TileSetFlags.OK)
                        template.mapType = StringToMapTypes(line);
        }

        static private void GetRoomTypes(ref AreaMapType template, string start, ref System.IO.StreamReader reader)
        {
            line = reader.ReadLine();

            if (line == start)
                while ((line = reader.ReadLine()) != lineStop)
                    if (CheckLine(ref line) == TileSetFlags.OK)
                        template.roomTypes.Add(StringToRoomTypes(line));
        }

        static private void GetRoomTypeProbabilities(ref AreaMapType template, string start, ref System.IO.StreamReader reader)
        {
            line = reader.ReadLine();

            if (line == start)
            {
                int prev_prob = 0;          int curr_prob = 0;

                while ((line = reader.ReadLine()) != lineStop)
                {
                    if (CheckLine(ref line) == TileSetFlags.OK)
                    {
                        curr_prob = Convert.ToInt32(line);
                        curr_prob += prev_prob;

                        template.roomTypeProbs.Add(curr_prob);
                        prev_prob = curr_prob;
                    }
                }
            }
        }

        static private void GetRoomSizesRange(ref AreaMapType template, string start, ref System.IO.StreamReader reader)
        {
            line = reader.ReadLine();
            int try_room;
            int room = 0;

            if (line == start)
            {
                while ((line = reader.ReadLine()) != lineStop)
                {
                    if (CheckLine(ref line) == TileSetFlags.OK)
                    {
                        if (Int32.TryParse(line, out try_room)) // get room # 
                        {
                            room = try_room;
                            template.roomSizesRange.Add(new List<Sizes>());
                        }
                        else
                            template.roomSizesRange[room].Add(StringToRoomSizesRange(line));
                    }
                }
            }
        }

        static private void GetRoomBarriers(ref AreaMapType template, string start, ref System.IO.StreamReader reader)
        {
            line = reader.ReadLine();

            if (line == start)
                while ((line = reader.ReadLine()) != lineStop)
                    if (CheckLine(ref line) == TileSetFlags.OK)
                        template.roomBarrierSizes.Add(Convert.ToInt32(line));
        }

        static private void GetTileSets(ref AreaMapType template, string start, ref System.IO.StreamReader reader)
        {
            line = reader.ReadLine();

            if (line == start)
                while ((line = reader.ReadLine()) != lineStop)
                    if (CheckLine(ref line) == TileSetFlags.OK)
                        template.tsNameIndices.Add(StringToTSNameIndex(line));
        }

        static private TileSetFlags CheckLine(ref string line)
        {
            if (line[0] == '#')
                return TileSetFlags.COMMENT;
            else
                return TileSetFlags.OK;
        }

#region StringToEnums

        static public MapTypes StringToMapTypes(string line)
        {
            switch (line)
            {
                case "rooms":
                    return MapTypes.ROOMS;
                default:
                    return MapTypes.NULL;
            }
        }

        static public RoomTypes StringToRoomTypes(string line)
        {
            switch (line)
            {
                case "rectangle":
                    return RoomTypes.RECTANGLE;
                default:
                    return RoomTypes.NULL;
            }
        }

        static public Sizes StringToRoomSizesRange(string line)
        {
            switch (line)
            {
                case "tiny":
                    return Sizes.TINY;
                case "small":
                    return Sizes.SMALL;
                case "medium":
                    return Sizes.MEDIUM;
                case "large":
                    return Sizes.LARGE;
                case "huge":
                    return Sizes.HUGE;
                default:
                    return Sizes.NULL;
            }
        }

        static public TSNamesIndex StringToTSNameIndex(string line)
        {
            switch (line)
            {
                case "test_tile_set_new":
                    return TSNamesIndex.TEST_TILE_SET_NEW;
                default:
                    return TSNamesIndex.BLANK;
            }
        }

#endregion
    }
}
