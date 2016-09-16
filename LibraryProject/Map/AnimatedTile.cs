using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LibraryProject.Map
{
    public class AnimatedTile : Tile
    {
        private int currentFrame;
        private int frameCount;
        private int elapsedMilliseconds;
        private int millisecondsPerFrame;

        /// <summary>
        /// Maximum # of frames in AnimatedTile
        /// </summary>
        public int FrameCount
        {
            get { return frameCount; }
            set { frameCount = value; }
        }

        public int MillisecondsPerFrame
        {
            get { return millisecondsPerFrame; }
            set { millisecondsPerFrame = value; }
        }

        public AnimatedTile() : base()
        {
            currentFrame = 0;
            frameCount = 1;
            elapsedMilliseconds = 0;
            millisecondsPerFrame = 0;
        }

        public AnimatedTile(int frame_count, int frame_length, 
            string texture_name, Rectangle source_rectangle)
            : base(texture_name, source_rectangle)
        {
            currentFrame = 0;
            frameCount = frame_count;
            elapsedMilliseconds = 0;
            millisecondsPerFrame = frame_length;
        }

        public override void Update(GameTime gt)
        {
            elapsedMilliseconds += gt.ElapsedGameTime.Milliseconds;

            if(elapsedMilliseconds >= millisecondsPerFrame)
            {
                if(currentFrame >= (frameCount - 1))
                {
                    currentFrame = 0;
                    sourceRect.X -= (sourceRect.Width * (frameCount - 1));
                }
                else
                {
                    currentFrame++;
                    sourceRect.X += sourceRect.Width;
                }
                elapsedMilliseconds = 0;
            }
        }
    }
}
