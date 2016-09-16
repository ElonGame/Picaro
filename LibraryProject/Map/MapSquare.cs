using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LibraryProject.Map
{
    public class MapSquare
    {
        private int xLocPixel;
        private int yLocPixel;
        private int xTileIndex;
        private int yTileIndex;
        private int mapSquareSize;

        private int roomID;
        private int areaMapTextureIndex;

        private Rectangle locationRect;
        private Rectangle sourceRect;
        private Color tileColor;

        private Tile bgTile;
        private Tile fgTile;

        // temp ?
        private string textureName;

        private List<MapSquareFlags> mapSquareStates;

        public int XLocPixel
        {
            get { return xLocPixel; }
            set { xLocPixel = value; }
        }

        public int YLocPixel
        {
            get { return yLocPixel; }
            set { yLocPixel = value; }
        }

        /// <summary>
        /// Width/Height of this MapSquare, in pixels
        /// </summary>
        public int MapSquareSize
        {
            get { return mapSquareSize; }
            set { mapSquareSize = value; }
        }

        public int RoomID
        {
            get { return roomID; }
            set { roomID = value; }
        }

        /// <summary>
        /// Index which indicates where in MapEngine.currentMapTextures[] to check
        /// to get the Texture2D for this MapSquare during MapEngine's call to Draw(); 
        /// this is set by MapGenerator when an AreaMap is made
        /// </summary>
        public int AreaMapTextureIndex
        {
            get { return areaMapTextureIndex; }
            set { areaMapTextureIndex = value; }
        }
        
        public Rectangle LocationRect
        {
            get { return locationRect; }
            set { locationRect = value; }
        }

        // temp, can be removed?
        public Rectangle SourceRect
        {
            get { return sourceRect; }
            set { sourceRect = value; }
        }

        // temp
        public Color TileColor
        {
            get { return tileColor; }
            set { tileColor = value; }
        }

        // temp?
        public String TextureName
        {
            get { return textureName; }
            set { textureName = value; }
        }

        public List<MapSquareFlags> MapSquareStates
        {
            get { return mapSquareStates; }
            set { mapSquareStates = value; }
        }

        public Tile BgTile
        {
            get { return bgTile; }
            set { bgTile = value; }
        }

        public Tile FgTile
        {
            get { return fgTile; }
            set { fgTile = value; }
        }

        /// <summary>
        /// Default MapSquare constructor
        /// </summary>
        public MapSquare()
        {
            xLocPixel = 0;
            yLocPixel = 0;
            xTileIndex = 0;
            yTileIndex = 0;
            roomID = 0;
            areaMapTextureIndex = 0;
            mapSquareSize = 55;
            locationRect = new Rectangle(xLocPixel, yLocPixel, mapSquareSize, mapSquareSize);
            bgTile = new Tile();
            fgTile = new Tile();

            // temp
            sourceRect = new Rectangle(330, 0, mapSquareSize, mapSquareSize);
            tileColor = Color.White;
            textureName = "blank";
            mapSquareStates = new List<MapSquareFlags>();
            mapSquareStates.Add(MapSquareFlags.BLANK);
        }

        public MapSquare(int xcoord, int ycoord)
        {
            mapSquareSize = 55;
            xTileIndex = xcoord;
            yTileIndex = ycoord; 
            xLocPixel = xcoord * mapSquareSize;
            yLocPixel = ycoord * mapSquareSize;
            roomID = 0;
            areaMapTextureIndex = 0;
            locationRect = new Rectangle(xLocPixel, yLocPixel, mapSquareSize, mapSquareSize);
            bgTile = new Tile();
            fgTile = new Tile();

            // temp
            sourceRect = new Rectangle(330, 0, mapSquareSize, mapSquareSize);
            tileColor = Color.White;
            textureName = "blank";
            mapSquareStates = new List<MapSquareFlags>();
            mapSquareStates.Add(MapSquareFlags.BLANK);
        }

        public void SetLocs(int x_tile_index, int y_tile_index)
        {
            xTileIndex = x_tile_index;
            yTileIndex = y_tile_index;
            xLocPixel = x_tile_index * mapSquareSize;
            yLocPixel = y_tile_index * mapSquareSize;
            locationRect = new Rectangle(xLocPixel, yLocPixel, mapSquareSize, mapSquareSize);
        }

        public void Update(GameTime gt)
        {
            fgTile.Update(gt);
            bgTile.Update(gt);
        }

        public void Draw(SpriteBatch sb, Texture2D texture, Vector2 map_draw_offset, 
            Vector2 map_engine_viewport_offset)
        {
            Vector2 offset = map_draw_offset - map_engine_viewport_offset;

            // temp form
            Color temp_color = Color.White;

            if(mapSquareStates[(int)MapSquareFlagsIndices.BLANKSTATE] == MapSquareFlags.BLANK)
            {
                temp_color = Color.Black;
            }
            else
            {
                switch (mapSquareStates[(int)MapSquareFlagsIndices.PERIMETER_OR_ROOM_OR_ENTRANCE])
                {
                    //case MapSquareFlags.ROOM :
                    //    temp_color = Color.LightGreen;
                    //    break;
                    //case MapSquareFlags.PERIMETER:
                    //    temp_color = Color.Blue;
                    //    break;
                    //case MapSquareFlags.ENTRANCE :
                    //    temp_color = Color.Red;
                    //    break;
                    case MapSquareFlags.ROOM:
                    case MapSquareFlags.PERIMETER:
                    case MapSquareFlags.ENTRANCE:
                        temp_color = Color.White;
                        break;
                }
            }

            // draw BG tile
            sb.Draw(texture, new Rectangle(
                (locationRect.X + (int)offset.X), (locationRect.Y + (int)offset.Y),
                mapSquareSize, mapSquareSize),
                bgTile.SourceRect, temp_color);

            // draw FG tile
            sb.Draw(texture, new Rectangle(
                (locationRect.X + (int)offset.X), (locationRect.Y + (int)offset.Y),
                mapSquareSize, mapSquareSize),
                fgTile.SourceRect, temp_color);
        }

    }
}
