using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LibraryProject
{
    public class Tile
    {
        public Rectangle SourceRect;
        public bool IsAnimated;

        private int currentFrame;
        public int FrameCount; // Maximum # of frames in AnimatedTile
        public float ElapsedMilliseconds;
        public float MillisecondsPerFrame;

        ///// <summary>
        ///// String name for Texture2D that contains Tile graphic
        ///// this name should be connected to a file path in 
        ///// TextureManager.textureNameToPathDictionary
        ///// </summary>
        //public string TextureName;        
        //public Rectangle SourceRect
        //{
        //    get { return sourceRect; }
        //    set { sourceRect = value; }
        //}

        public Tile(int tileSize)
        {
            //TextureName = "blank";
            SourceRect = new Rectangle(0, 0, tileSize, tileSize);
            IsAnimated = false;

            FrameCount = 0;
            MillisecondsPerFrame = 0f;
            ElapsedMilliseconds = 0;
            currentFrame = 0;
        }

        public Tile(int xSource, int ySource, int tileSize)//string texture_name, Rectangle source_rectangle)
        {
            //TextureName = texture_name;
            IsAnimated = false;
            SourceRect = new Rectangle(xSource, ySource, tileSize, tileSize);

            FrameCount = 0;
            MillisecondsPerFrame = 0f;
            ElapsedMilliseconds = 0;
            currentFrame = 0;
        }

        public Tile(int xSource, int ySource, int tileSize, int frame_count, int frame_length)//string texture_name, Rectangle source_rectangle)
        {
            //TextureName = texture_name;
            IsAnimated = true;
            SourceRect = new Rectangle(xSource, ySource, tileSize, tileSize);

            FrameCount = frame_count;
            MillisecondsPerFrame = frame_length;
            ElapsedMilliseconds = 0;
            currentFrame = 0;
        }

        public virtual void Update(GameTime gt)
        {
            if(IsAnimated)
            {
                ElapsedMilliseconds += gt.ElapsedGameTime.Milliseconds;

                if (ElapsedMilliseconds >= MillisecondsPerFrame)
                {
                    currentFrame++;
                    if (currentFrame >= (FrameCount))
                    {
                        currentFrame = 0;
                        SourceRect.X -= (SourceRect.Width * (FrameCount - 1));
                    }
                    else
                        SourceRect.X += SourceRect.Width;

                    ElapsedMilliseconds = 0;
                }
            }
        }
    }
}
