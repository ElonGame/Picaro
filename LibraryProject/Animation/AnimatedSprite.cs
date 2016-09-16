using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LibraryProject
{
    public class AnimatedSprite : ContentObject
    {
        #region Constructor
        public Vector2 mapDrawOrigin;
        public AnimatedSprite()
        {
            mapDrawOrigin = Globals.mDrawOrg;
        }

        #endregion

        #region Graphics Data


        /// <summary>
        /// The content path and name of the texture for this animation.
        /// </summary>
        public string textureName;

        /// <summary>
        /// The texture for this animation.
        /// </summary>
        public Texture2D texture;


        #endregion


        #region Frame Data


        /// <summary>
        /// The dimensions of a single frame of animation.
        /// </summary>
        private Point frameDimensions;

        /// <summary>
        /// The width of a single frame of animation.
        /// </summary>
        public Point FrameDimensions
        {
            get { return frameDimensions; }
            set
            {
                frameDimensions = value;
                frameOrigin.X = frameDimensions.X;//frameDimensions.X / 2;
                frameOrigin.Y = frameDimensions.Y;//frameDimensions.Y / 2;
            }
        }


        /// <summary>
        /// The origin of the sprite, within a frame.
        /// </summary>
        private Point frameOrigin;


        /// <summary>
        /// The number of frames in a row in this sprite.
        /// </summary>
        //private int framesPerRow;

        /// <summary>
        /// The number of frames in a row in this sprite.
        /// </summary>
        //public int FramesPerRow
        //{
        //    get { return framesPerRow; }
        //    set { framesPerRow = value; }
        //}

        /// <summary>
        /// The offset of this sprite from the position it's drawn at.
        /// </summary>
        public Vector2 sourceOffset;



        #endregion


        #region Animation Data


        /// <summary>
        /// The animations defined for this sprite.
        /// </summary>
        public List<Animation> animations = new List<Animation>();

        /// <summary>
        /// Enumerate the animations on this animated sprite.
        /// </summary>
        /// <param name="animationName">The name of the animation.</param>
        /// <returns>The animation if found; null otherwise.</returns>
        public Animation this[string animationName]
        {
            get
            {
                if (String.IsNullOrEmpty(animationName))
                    return null;

                foreach (Animation animation in animations)
                    if (String.Compare(animation.Name, animationName, StringComparison.OrdinalIgnoreCase) == 0)
                        return animation;

                return null;
            }
        }


        /// <summary>
        /// Add the animation to the list, checking for name collisions.
        /// </summary>
        /// <returns>True if the animation was added to the list.</returns>
        public bool AddAnimation(Animation animation)
        {
            if ((animation != null) && (this[animation.Name] == null))
            {
                animations.Add(animation);
                return true;
            }
            return false;
        }


        #endregion


        #region Playback


        /// <summary>
        /// The animation currently playing back on this sprite.
        /// </summary>
        public Animation currentAnimation = null;

        /// <summary>
        /// The current frame in the current animation.
        /// </summary>
        private int currentFrame = 0;

        /// <summary>
        /// The elapsed time since the last frame switch.
        /// </summary>
        private float elapsedTime;


        /// <summary>
        /// The source rectangle of the current frame of animation.
        /// </summary>
        private Rectangle sourceRectangle = Rectangle.Empty;

        /// <summary>
        /// The source rectangle of the current frame of animation.
        /// </summary>
        public Rectangle SourceRectangle
        {
            get { return sourceRectangle; }
        }


        /// <summary>
        /// Play the given animation on the sprite.
        /// </summary>
        /// <remarks>The given animation may be null, to clear any animation.</remarks>
        public void PlayAnimation(Animation animation)
        {
            // start the new animation, ignoring redundant Plays
            if (animation != currentAnimation)
            {
                currentAnimation = animation;
                ResetAnimation();
            }
        }


        /// <summary>
        /// Play an animation given by index.
        /// </summary>
        public void PlayAnimation(int index)
        {
            // check the parameter
            if ((index < 0) || (index >= animations.Count))
                throw new ArgumentOutOfRangeException("index");

            PlayAnimation(this.animations[index]);
        }


        /// <summary>
        /// Play an animation given by name.
        /// </summary>
        public void PlayAnimation(string name)
        {
            // check the parameter
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            PlayAnimation(this[name]);
        }


        /// <summary>
        /// Play a given animation name, with the given direction suffix.
        /// </summary>
        /// <example>
        /// For example, passing "Walk" and Direction.South will play the animation
        /// named "WalkSouth".
        /// </example>
        public void PlayAnimation(string name, Directions direction)
        {
            // check the parameter
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            PlayAnimation(name + direction.ToString());
        }


        /// <summary>
        /// Reset the animation back to its starting position.
        /// </summary>
        public void ResetAnimation()
        {
            elapsedTime = 0f;
            if (currentAnimation != null)
            {
                currentFrame = 0;// currentAnimation.StartingFrameX;
                // calculate the source rectangle by updating the animation
                //UpdateAnimation(0f);
                sourceRectangle =
                    new Rectangle(
                        currentAnimation.xIndex * frameDimensions.X,
                        currentAnimation.yIndex * frameDimensions.Y,
                        frameDimensions.X, frameDimensions.Y);
            }
        }


        /// <summary>
        /// Advance the current animation to the final sprite.
        /// </summary>
        public void AdvanceToEnd()
        {
            if (currentAnimation != null)
            {
                currentFrame = currentAnimation.lastFrame;//currentAnimation.EndingFrameX;
                // calculate the source rectangle by updating the animation
                UpdateAnimation(0f);
            }
        }


        /// <summary>
        /// Stop any animation playing on the sprite.
        /// </summary>
        public void StopAnimation()
        {
            currentAnimation = null;
        }


        /// <summary>
        /// Returns true if playback on the current animation is complete, or if
        /// there is no animation at all.
        /// </summary>
        public bool IsPlaybackComplete
        {
            get
            {
                return ((currentAnimation == null) ||
                    (!currentAnimation.isLoop &&
                    (currentFrame > currentAnimation.lastFrame))); //> currentAnimation.EndingFrameX)));
            }
        }


        #endregion


        #region Updating


        /// <summary>
        /// Update the current animation.
        /// </summary>
        public void UpdateAnimation(float elapsed_ms)
        {
            if (IsPlaybackComplete)
                return;

            // loop the animation if needed
            if (currentAnimation.isLoop && (currentFrame > currentAnimation.lastFrame))
                currentFrame = 0;// currentAnimation.StartingFrameX;

            // update the source rectangle
            //int column = (currentFrame - 1) / framesPerRow;
            //sourceRectangle = new Rectangle(
            //    (currentFrame - 1 - (column * framesPerRow)) * frameDimensions.X,
            //    column * frameDimensions.Y,
            //    frameDimensions.X, frameDimensions.Y);
            sourceRectangle.X = (currentAnimation.startingFrame + currentFrame) * frameDimensions.X;

            // update the elapsed time
            elapsedTime += elapsed_ms;

            // advance to the next frame if ready
            if (elapsedTime > (float)currentAnimation.interval)
            {
                currentFrame++;
                //elapsedTime -= (float)currentAnimation.interval / 1000f;
                elapsedTime = 0;
            }
        }


        #endregion


        #region Drawing


        /// <summary>
        /// Draw the sprite at the given position.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch object used to draw.</param>
        /// <param name="position">The position of the sprite on-screen.</param>
        /// <param name="layerDepth">The depth at which the sprite is drawn.</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, float layerDepth)
        {
            Draw(spriteBatch, position, layerDepth, SpriteEffects.None);
        }


        /// <summary>
        /// Draw the sprite at the given position.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch object used to draw.</param>
        /// <param name="position">The position of the sprite on-screen.</param>
        /// <param name="layerDepth">The depth at which the sprite is drawn.</param>
        /// <param name="spriteEffect">The sprite-effect applied.</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, float layerDepth, SpriteEffects spriteEffect)
        {
            // check the parameters
            if (spriteBatch == null)
                throw new ArgumentNullException("spriteBatch");

            if (texture != null)
                spriteBatch.Draw(texture, position, sourceRectangle, Color.White, 0f, sourceOffset, 1f, spriteEffect,
                    MathHelper.Clamp(layerDepth, 0f, 1f));
        }


        #endregion


        #region Content Type Reader


        /// <summary>
        /// Read an AnimatingSprite object from the content pipeline.
        /// </summary>
        //public class AnimatingSpriteReader : ContentTypeReader<AnimatedSprite>
        //{
        //    /// <summary>
        //    /// Read an AnimatingSprite object from the content pipeline.
        //    /// </summary>
        //    protected override AnimatedSprite Read(ContentReader input, AnimatedSprite existingInstance)
        //    {
        //        AnimatedSprite animatingSprite = existingInstance;
        //        if (animatingSprite == null)
        //        {
        //            animatingSprite = new AnimatedSprite();
        //        }

        //        animatingSprite.AssetName = input.AssetName;

        //        animatingSprite.TextureName = input.ReadString();
        //        animatingSprite.Texture =
        //            input.ContentManager.Load<Texture2D>(System.IO.Path.Combine(@"Textures", animatingSprite.TextureName));
        //        animatingSprite.FrameDimensions = input.ReadObject<Point>();
        //        //animatingSprite.FramesPerRow = input.ReadInt32();
        //        animatingSprite.SourceOffset = input.ReadObject<Vector2>();
        //        animatingSprite.Animations.AddRange(
        //            input.ReadObject<List<Animation>>());

        //        return animatingSprite;
        //    }
        //}


        //#endregion


        //#region ICloneable Members


        ///// <summary>
        ///// Creates a clone of this object.
        ///// </summary>
        //public object Clone()
        //{
        //    AnimatedSprite animatingSprite = new AnimatedSprite();

        //    animatingSprite.animations.AddRange(animations);
        //    animatingSprite.currentAnimation = currentAnimation;
        //    animatingSprite.currentFrame = currentFrame;
        //    animatingSprite.elapsedTime = elapsedTime;
        //    animatingSprite.frameDimensions = frameDimensions;
        //    animatingSprite.frameOrigin = frameOrigin;
        //    //animatingSprite.framesPerRow = framesPerRow;
        //    animatingSprite.sourceOffset = sourceOffset;
        //    animatingSprite.sourceRectangle = sourceRectangle;
        //    animatingSprite.texture = texture;
        //    animatingSprite.textureName = textureName;

        //    return animatingSprite;
        //}


        #endregion
    }
}
