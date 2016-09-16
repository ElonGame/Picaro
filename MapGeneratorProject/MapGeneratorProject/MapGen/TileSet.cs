using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LibraryProject;

namespace MapGeneratorProject
{
    public enum TileGroup { BG_WALL_SOURCES, FG_WALL_SOURCES, BG_ROOM_SOURCES, FG_ROOM_SOURCES }
    /// <summary>
    /// Class that contains data about a set of Tiles
    /// TileSetManager extracts data from .txt files and stores them in this class type
    /// </summary>
    public class TileSet
    {
        private string tileSetName;
        private string textureName;

        private List<List<int>> tileSources;
        // each pos_vec represents (<index of animated tile in respective group>, <#frames>)
        private List<List<Vector3>> animatedTileIndices; 

        public List<List<int>> TSrcs
        {
            get { return tileSources; }
            set { tileSources = value; }
        }

        public List<List<Vector3>> ATIndices
        {
            get { return animatedTileIndices; }
            set { animatedTileIndices = value; }
        }

        #region properties
        public string TileSetName
        {
            get { return tileSetName; }
            set { tileSetName = value; }
        }

        /// <summary>
        /// Name of the graphic file that the tiles are in
        /// </summary>
        public string TextureName
        {
            get { return textureName; }
            set { textureName = value; }
        }

        ///// <summary>
        ///// List of coordinate pairs for the origins of each (Bg)Tile in the set
        ///// </summary>
        //public List<Point> BgTileSourcesWall
        //{
        //    get { return bgTileSourcesWall; }
        //    set { bgTileSourcesWall = value; }
        //}

        ///// <summary>
        ///// List of coordinate pairs for the origins of each (Bg)Tile in the set
        ///// </summary>
        //public List<Point> BgTileSourcesRoom
        //{
        //    get { return bgTileSourcesRoom; }
        //    set { bgTileSourcesRoom = value; }
        //}

        ///// <summary>
        ///// List of Vector3 containing data about (Bg)AnimatedTiles:
        ///// (index of (Bg) AnimatedTile in BgTileSources, frameCount, frameLength)
        ///// </summary>
        //public List<Vector3> BgAnimatedTileIndicesWall
        //{
        //    get { return bgAnimatedTileIndicesWall; }
        //    set { bgAnimatedTileIndicesWall = value; }
        //}

        ///// <summary>
        ///// List of Vector3 containing data about (Bg)AnimatedTiles:
        ///// (index of (Bg) AnimatedTile in BgTileSources, frameCount, frameLength)
        ///// </summary>
        //public List<Vector3> BgAnimatedTileIndicesRoom
        //{
        //    get { return bgAnimatedTileIndicesRoom; }
        //    set { bgAnimatedTileIndicesRoom = value; }
        //}

        ///// <summary>
        ///// List of coordinate pairs for the origins of each (Fg)Tile in the set
        ///// </summary>
        //public List<Point> FgTileSourcesWall
        //{
        //    get { return fgTileSourcesWall; }
        //    set { fgTileSourcesWall = value; }
        //}

        ///// <summary>
        ///// List of coordinate pairs for the origins of each (Fg)Tile in the set
        ///// </summary>
        //public List<Point> FgTileSourcesRoom
        //{
        //    get { return fgTileSourcesRoom; }
        //    set { fgTileSourcesRoom = value; }
        //}

        ///// <summary>
        ///// List of Vector3 containing data about (Fg)AnimatedTiles:
        ///// (index of (Fg) AnimatedTile in FgTileSources, frameCount, frameLength)
        ///// </summary>
        //public List<Vector3> FgAnimatedTileIndicesWall
        //{
        //    get { return fgAnimatedTileIndicesWall; }
        //    set { fgAnimatedTileIndicesWall = value; }
        //}

        ///// <summary>
        ///// List of Vector3 containing data about (Fg)AnimatedTiles:
        ///// (index of (Fg) AnimatedTile in FgTileSources, frameCount, frameLength)
        ///// </summary>
        //public List<Vector3> FgAnimatedTileIndicesRoom
        //{
        //    get { return fgAnimatedTileIndicesRoom; }
        //    set { fgAnimatedTileIndicesRoom = value; }
        //}

#endregion

        public TileSet()
        {
            Initialize();
        }

        public void Initialize()
        {
            tileSetName = "default_tile_set_name";
            textureName = "default_texture_name";

            tileSources = new List<List<int>>();
            animatedTileIndices = new List<List<Vector3>>();

            for(int i = 0; i < 4; i++)
                tileSources.Add(new List<int>());

            for (int i = 0; i < 4; i++)
                animatedTileIndices.Add(new List<Vector3>());
        }

        /// <summary>
        /// Returns index of Vector3 containing AnimatedTile data from TileSet.fgAnimatedTileIndices or TileSet.bgAnimatedTileIndices
        /// Used by MapGenerator to set MapSquare animated FG/BGTile data frameCount and frameLength
        /// </summary>
        /// <param name="checked_index">index from bgTileSources or fgTileSources of AnimatedTile</param>
        /// <param name="animated_tile_index_list">either bgAnimatedTileIndices or fgAnimatedTileIndices</param>
        /// <returns></returns>
        public int GetATIndex(TileSet tile_set, int checked_index, int tile_group)
        {
            int max_size = tile_set.animatedTileIndices[tile_group].Count();

            for (int index = 0; index < max_size; index++)
                if (tile_set.animatedTileIndices[tile_group][index].X == checked_index)
                    return index;

            return -1;
        }
    }
}
