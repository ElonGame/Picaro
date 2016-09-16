using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Graphics;
using LibraryProject;

namespace MapGeneratorProject
{
    /// <summary>
    /// Loads TileSets from .txt files, also contains list of filepaths for TileSet .txt files
    /// </summary>
    static public class TileSetManager
    {
        static private List<string> tileSetsNames;
        static private string line;
        //static private string lineStop = "***";

        static public List<string> TileSetsNames
        {
            get { return tileSetsNames; }
            set { tileSetsNames = value; }
        }

        static private string[] tileGroupNames;
        static private string[] animatedTileGroupNames;

        static public void Initialize()
        {
            LoadTileGroups();
            LoadTileSetNames();
        }

        static private void LoadTileGroups()
        {
            tileGroupNames = new string[4];
            tileGroupNames[0] = "BG_WALL_SOURCES";
            tileGroupNames[1] = "FG_WALL_SOURCES";
            tileGroupNames[2] = "BG_ROOM_SOURCES";
            tileGroupNames[3] = "FG_ROOM_SOURCES";

            animatedTileGroupNames = new string[4];
            animatedTileGroupNames[0] = "BG_WALL_ANIMATED_TILES";
            animatedTileGroupNames[1] = "FG_WALL_ANIMATED_TILES";
            animatedTileGroupNames[2] = "BG_ROOM_ANIMATED_TILES";
            animatedTileGroupNames[3] = "FG_ROOM_ANIMATED_TILES";
        }

        /// <summary>
        /// These names are the literal file paths/names of a TileSet .txt file. They are loaded by using an enum identifier
        /// that corresponds to the TileSet filepaths' index in TileSetManager.tileSetsNames
        /// </summary>
        static private void LoadTileSetNames()
        {
            tileSetsNames = new List<string>();
            tileSetsNames.Add(@"Content\\Maps\\TileSets\\blank.txt");
            tileSetsNames.Add(@"Content\\Maps\\TileSets\\testTileSetNew.txt");
        }

        /// <summary>
        /// Adds TileSet from .txt file to List<TileSet> MapGenerator.LoadedTileSets
        /// </summary>
        /// <param name="tile_set_name">Enum used to identify which index to use when retrieving the 
        /// file path for the particular TileSet from (this class) List<string> tileSetNames
        /// </param>
        /// <param name="tile_set">index of List<TileSet> MapGeneratorHelper.LoadedTileSets
        /// that the loaded tile set will be stored at</param>
        static public void LoadTileSet(TSNamesIndex tile_set_name, int tile_set) 
        {
            MapGenHelper.LoadedTileSets.Add(new TileSet()); // <--- new tileSet object that .txt file is loading into
            line = string.Empty;
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream(tileSetsNames[(int)tile_set_name]); // <-- this is where the TileSetsNamesIndices enum is used
                System.IO.StreamReader tile_set_txt_file = new System.IO.StreamReader(stream);

                // <-------- int tile_set is index MapGeneratorHelper.LoadedTileSets this tileSet is being added to
                if ((line = tile_set_txt_file.ReadLine()) == "START")
                {
                    GetName(false, tile_set, "TILE_SET_NAME", "***", ref tile_set_txt_file); // <----- adds tileSet name to tileSet object

                    GetName(true, tile_set, "TEXTURE_FILE_NAME", "***", ref tile_set_txt_file); // <-- adds file name of textured2D to tileSet object

                    for (int tile_group = 0; tile_group < tileGroupNames.Count(); tile_group++) // <-- adds tile sources to tileSet object
                        GetTileSource(tile_group, tile_set, tileGroupNames[tile_group], "***", ref tile_set_txt_file);

                    // <-- adds animated tile indices to tileSet object
                    for (int anim_tile_group = 0; anim_tile_group < animatedTileGroupNames.Count(); anim_tile_group++)
                        GetAnimatedTileIndices(anim_tile_group, tile_set, animatedTileGroupNames[anim_tile_group], "***", ref tile_set_txt_file);                         
                }
                
                tile_set_txt_file.Close();
            }
            catch (System.IO.FileNotFoundException)
            {
                //temp_game.Exit();
            }
        }

        static void GetName(bool texture_name, int tile_set, string start, string stop, ref System.IO.StreamReader reader)
        {
            string line = reader.ReadLine();

            if (line == start)
            {
                while ((line = reader.ReadLine()) != stop)
                    if (CheckLine(ref line) == TileSetFlags.OK)
                        if (texture_name)
                            MapGenHelper.LoadedTileSets[tile_set].TextureName = line;
                        else
                            MapGenHelper.LoadedTileSets[tile_set].TileSetName = line;
            }
        }

        static void GetTileSource(int tile_group, int tile_set, string start, string stop, ref System.IO.StreamReader reader)
        {
            int tile = 0;
            string line = reader.ReadLine();       

            if(line == start)
            {
                while((line = reader.ReadLine()) != stop)
                {               
                    //if (CheckLine(ref reader) == TileSetFlags.OK)
                    if (CheckLine(ref line) == TileSetFlags.OK)
                    {
                        tile = Convert.ToInt32(line);
                        MapGenHelper.LoadedTileSets[tile_set].TSrcs[tile_group].Add(tile);
                    }
                }
            }
        }

        
        static void GetAnimatedTileIndices(int tile_group, int tile_set, string start, string stop,
            ref System.IO.StreamReader reader)
        {
            int animated_tile_index = 0;
            int frame_count = 0;
            int frame_length = 0; // ms

            string temp_str = string.Empty;

            string line = reader.ReadLine();

            if (line == start)
            {
                while ((line = reader.ReadLine()) != stop)
                {
                    if (CheckLine(ref line) == TileSetFlags.OK)
                    {
                        for (int line_pos = 0; line_pos < line.Length; line_pos++)
                        {
                            if(Char.IsNumber(line[line_pos]))
                            {
                                temp_str += line[line_pos];
                            }
                            else if((line[line_pos] == ',')) // fix this to work with vectors
                            {
                                animated_tile_index = Convert.ToInt32(temp_str);
                                temp_str = string.Empty;
                            }
                            else if ((line[line_pos] == '-')) // fix this to work with vectors
                            {
                                frame_count = Convert.ToInt32(temp_str);
                                temp_str = string.Empty;
                            }
                            
                            if (line_pos == (line.Length - 1))
                            {
                                frame_length = Convert.ToInt32(temp_str);
                                temp_str = string.Empty;
                            }
                        }
                        MapGenHelper.LoadedTileSets[tile_set].ATIndices[tile_group].Add(
                            new Vector3(animated_tile_index, frame_count, frame_length));
                    }
                }
            }
        }

        static TileSetFlags CheckLine(ref string line)
        {

            if (line[0] == 'X')
                return TileSetFlags.COMMENT;
            else
                return TileSetFlags.OK;
        }

        //static TileSetFlags CheckLine(ref string line)
        //{

        //    if (line.StartsWith("X"))
        //        return TileSetFlags.COMMENT;
        //    else
        //        return TileSetFlags.OK;
        //}

        static TileSetFlags CheckLine(ref System.IO.StreamReader reader)
        {
            if (reader.Peek() == 'X')
                return TileSetFlags.COMMENT;
            else
                return TileSetFlags.OK;
        }

        static public bool CheckIfTileSetLoaded(string tile_set_name)
        {
            // @"Content\\Maps\\TileSets\\testTileSetNew.txt"

            if (MapGenHelper.LoadedTileSets != null)
            {
                for (int i = 0; i < MapGenHelper.LoadedTileSets.Count(); i++)
                    //if (MapGenHelper.LoadedTileSets[i].TileSetName == tile_set_name)
                    if (tile_set_name.Contains(MapGenHelper.LoadedTileSets[i].TileSetName))
                        return true;
            }

            return false;
        }

    }
}

    


