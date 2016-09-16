using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LibraryProject
{
    /// <summary>
    /// Class that links texture/TileSet names to texture file paths
    /// </summary>
    static public class TextureManager
    {
        static private Dictionary<string, Texture2D> textureNameToPathDictionary;
        static private ContentManager textureContentManager;

        static public Dictionary<string, Texture2D> TextureNameToPathDictionary
        {
            get { return textureNameToPathDictionary; }
            set { textureNameToPathDictionary = value; }
        }

        static public void Initialize(ContentManager content_manager)
        {
            textureContentManager = content_manager;
            textureNameToPathDictionary = new Dictionary<string, Texture2D>();
            LoadTextureNameDictionary();
        }

        /// <summary>
        /// Add new texture/TileSet names and their file paths here
        /// </summary>
        static private void LoadTextureNameDictionary()
        {
            textureNameToPathDictionary.Add("blank",
                textureContentManager.Load<Texture2D>(@"MapTextures\\blankBG"));
            textureNameToPathDictionary.Add("newTestSet",
                textureContentManager.Load<Texture2D>(@"MapTextures\\newTestSet2"));
        }
    }
}
