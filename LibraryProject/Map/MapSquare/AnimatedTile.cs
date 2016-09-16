using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LibraryProject
{
    //public class AnimatedTile : Tile
    //{
        //private int currentFrame;
        //public int FrameCount; // Maximum # of frames in AnimatedTile
        //public float ElapsedMilliseconds;
        //public float MillisecondsPerFrame;

        //public AnimatedTile() : base()
        //{
        //    currentFrame = 0;
        //    FrameCount = 1;
        //    ElapsedMilliseconds = 0;
        //    MillisecondsPerFrame = 0;
        //}

        //public AnimatedTile(int frame_count, int frame_length, string texture_name, Rectangle source_rectangle)
        //    : base(texture_name, source_rectangle)
        //{
        //    currentFrame = 0;
        //    FrameCount = frame_count;
        //    ElapsedMilliseconds = 0;
        //    MillisecondsPerFrame = (float)frame_length;
        //}

        //public override void Update(GameTime gt)
        //{
        //    ElapsedMilliseconds += gt.ElapsedGameTime.Milliseconds;

        //    if (ElapsedMilliseconds >= MillisecondsPerFrame)
        //    {
        //        currentFrame++;
        //        if (currentFrame >= (FrameCount))
        //        {
        //            currentFrame = 0;
        //            sourceRect.X -= (sourceRect.Width * (FrameCount - 1));
        //        }
        //        else               
        //            sourceRect.X += sourceRect.Width;

        //        ElapsedMilliseconds = 0;
        //    }
        //}
    //}
}
