using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LibraryProject.Map
{
    public class Tile
    {
        private string textureName;
        protected Rectangle sourceRect;

        /// <summary>
        /// String name for Texture2D that contains Tile graphic
        /// this name should be connected to a file path in 
        /// TextureManager.textureNameToPathDictionary
        /// </summary>
        public string TextureName
        {
            get { return textureName; }
            set { textureName = value; }
        }

        /// <summary>
        /// Source rectangle for Tile graphic on Texture2D that
        /// field textureName is linked to
        /// </summary>
        public Rectangle SourceRect
        {
            get { return sourceRect; }
            set { sourceRect = value; }
        }

        public Tile()
        {
            textureName = "blank";
            sourceRect = new Rectangle(0, 0, 0, 0);        
        }

        public Tile(string texture_name, Rectangle source_rectangle)
        {
            textureName = texture_name;
            sourceRect = source_rectangle;
        }

        public virtual void Update(GameTime gt)
        {

        }
    }
}
