using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Graphics;

namespace LibraryProject.Map
{
    /// <summary>
    /// Loads TileSets from .txt files, also contains list of filepaths for TileSet .txt files
    /// </summary>
    static public class TileSetManager
    {
        static private List<string> tileSetsNames;
        static private string line;

        static public List<string> TileSetsNames
        {
            get { return tileSetsNames; }
            set { tileSetsNames = value; }
        }

        static public void Initialize()
        {
            tileSetsNames = new List<string>();
            tileSetsNames.Add(@"Content\\Maps\\TileSets\\blank.txt");
            tileSetsNames.Add(@"Content\\Maps\\TileSets\\testTileSet.txt");
            tileSetsNames.Add(@"Content\\Maps\\TileSets\\testTileSet2.txt");
        }

        static public void LoadTileSet(TileSetsNamesIndices tile_set_name, int tile_set)
        {
            MapGenerator.LoadedTileSets.Add(new TileSet());
            line = string.Empty;
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream(tileSetsNames[(int)tile_set_name]);
                System.IO.StreamReader tile_set_txt_file = new System.IO.StreamReader(stream);

                if ((line = tile_set_txt_file.ReadLine()) == "START")
                {
                    GetTileName(ref tile_set_txt_file, tile_set);
                    GetTextureName(ref tile_set_txt_file, tile_set);
                    GetBgSources(ref tile_set_txt_file, tile_set);
                    GetBgAnimatedTiles(ref tile_set_txt_file, tile_set);
                    GetFgSources(ref tile_set_txt_file, tile_set);
                    GetFgAnimatedTiles(ref tile_set_txt_file, tile_set);
                }
                tile_set_txt_file.Close();
            }
            catch (System.IO.FileNotFoundException)
            {
                //temp_game.Exit();
            }
        }

        static void GetTileName(ref System.IO.StreamReader reader, int tile_set)
        {
            line = reader.ReadLine();
            while (line != "TEXTURE_FILE_NAME")
            {
                line = reader.ReadLine();
                MapGenerator.LoadedTileSets[tile_set].TileSetName = line;
                line = reader.ReadLine();
            }
        }

        static void GetTextureName(ref System.IO.StreamReader reader, int tile_set)
        {
            while (line != "BG_SOURCES")
            {
                line = reader.ReadLine();
                MapGenerator.LoadedTileSets[tile_set].TextureName = line;
                line = reader.ReadLine();
            }
        }

        static void GetBgSources(ref System.IO.StreamReader reader, int tile_set)
        {
            Point temp_point = new Point(0, 0);

            line = reader.ReadLine();
            while (line != "BG_ANIMATED_TILES")
            {
                temp_point.X = Convert.ToInt32(line);
                line = reader.ReadLine();
                temp_point.Y = Convert.ToInt32(line);
                MapGenerator.LoadedTileSets[tile_set].BgTileSources.Add(temp_point);
                line = reader.ReadLine();
            }
        }

        static void GetBgAnimatedTiles(ref System.IO.StreamReader reader, int tile_set)
        {
            Vector3 temp_vec = new Vector3(0, 0, 0);

            line = reader.ReadLine();
            while (line != "FG_SOURCES")
            {
                temp_vec.X = Convert.ToInt32(line);
                line = reader.ReadLine();
                temp_vec.Y = Convert.ToInt32(line);
                line = reader.ReadLine();
                temp_vec.Z = Convert.ToInt32(line);
                MapGenerator.LoadedTileSets[tile_set].BgAnimatedTileIndices.Add(temp_vec);
                line = reader.ReadLine();
            }
        }

        static void GetFgSources(ref System.IO.StreamReader reader, int tile_set)
        {
            Point temp_point = new Point(0, 0);

            line = reader.ReadLine();
            while (line != "FG_ANIMATED_TILES")
            {
                temp_point.X = Convert.ToInt32(line);
                line = reader.ReadLine();
                temp_point.Y = Convert.ToInt32(line);
                MapGenerator.LoadedTileSets[tile_set].FgTileSources.Add(temp_point);
                line = reader.ReadLine();
            }
        }

        static void GetFgAnimatedTiles(ref System.IO.StreamReader reader, int tile_set)
        {
            Vector3 temp_vec = new Vector3(0, 0, 0);

            line = reader.ReadLine();
            while (line != "STOP")
            {
                temp_vec.X = Convert.ToInt32(line);
                line = reader.ReadLine();
                temp_vec.Y = Convert.ToInt32(line);
                line = reader.ReadLine();
                temp_vec.Z = Convert.ToInt32(line);
                MapGenerator.LoadedTileSets[tile_set].FgAnimatedTileIndices.Add(temp_vec);
                line = reader.ReadLine();
            }
        }
    }
}

    


