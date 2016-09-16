using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LibraryProject
{
    public class Animation : ContentObject
    {
        public int xIndex;
        public int yIndex;
        public int frameCount;

        /// <summary>
        /// The name of the animation.
        /// </summary>
        private string name;

        /// <summary>
        /// The name of the animation.
        /// </summary>
        [ContentSerializer(Optional = true)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// The first frame of the animation.
        /// </summary>
        public int startingFrame;

        /// <summary>
        /// The first frame of the animation.
        /// </summary>
        //public int StartingFrameX
        //{
        //    get { return startingFrame; }
        //    set { startingFrame = value; }
        //}


        /// <summary>
        /// The last frame of the animation.
        /// </summary>
        //private int endingFrame;
        public int lastFrame;
        /// <summary>
        /// The last frame of the animation.
        /// </summary>
        //public int EndingFrameX
        //{
        //    get { return endingFrame; }
        //    set { endingFrame = value; }
        //}
        //public int LastFrame
        //{
        //    get { return lastFrame; }
        //    set { lastFrame = value; }
        //}


        /// <summary>
        /// The interval between frames of the animation.
        /// </summary>
        public int interval;

        /// <summary>
        /// If true, the animation loops.
        /// </summary>
        public bool isLoop;

        #region Constructors


        /// <summary>
        /// Creates a new Animation object.
        /// </summary>
        public Animation() { }


        /// <summary>
        /// Creates a new Animation object by full specification.
        /// </summary>
        //public Animation(string name, int startingFrame, int endingFrame, int interval, bool isLoop)
        public Animation(string name,int col_index, int row_index, int frame_count, int interval, bool isLoop)
        {
            this.Name = name;
            this.xIndex = col_index;
            this.yIndex = row_index;
            this.frameCount = frame_count;
            this.startingFrame = col_index; // startingFrame;
            this.lastFrame = frame_count - 1;// endingFrame;
            this.interval = interval;
            this.isLoop = isLoop;
        }


        #endregion

        #region Content Type Reader


        /// <summary>
        /// Read an Animation object from the content pipeline.
        /// </summary>
        //public class AnimationReader : ContentTypeReader<Animation>
        //{
        //    /// <summary>
        //    /// Read an Animation object from the content pipeline.
        //    /// </summary>
        //    protected override Animation Read(ContentReader input,
        //        Animation existingInstance)
        //    {
        //        Animation animation = existingInstance;
        //        if (animation == null)
        //        {
        //            animation = new Animation();
        //        }

        //        animation.AssetName = input.AssetName;

        //        animation.Name = input.ReadString();
        //        animation.StartingFrameX = input.ReadInt32();
        //        //animation.EndingFrameX = input.ReadInt32();
        //        animation.Interval = input.ReadInt32();
        //        animation.IsLoop = input.ReadBoolean();

        //        return animation;
        //    }
        //}


        #endregion
    }
}
