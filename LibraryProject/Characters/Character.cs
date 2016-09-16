using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LibraryProject
{
    //public abstract class Character : WorldObject
    public class Character : WorldObject
    {
        #region Character Data

        public PrimStats pStats;
        public SecStats sStats;
        public ConditionRes rStats;
        public ElementalRes eStats;

        #endregion

        public Character()
        {

        }

        public Character(string character_name, Point map_pos, Directions direct)
        {
            Name = character_name;
            mapPosition = map_pos;
            Direction = direct;
        }

        #region Character State

        /// <summary>
        /// The state of a character.
        /// </summary>
        public enum CharacterState
        {
            /// <summary>
            /// Ready to perform an action, and playing the idle animation
            /// </summary>
            Idle,

            /// Dead, but still playing the dying animation.
            /// </summary>
            Dying,

            /// <summary>
            /// Dead, with the dead animation.
            /// </summary>
            Dead,

            Moving

            /// <summary>
            /// Ready to perform an action, and playing the idle animation
            /// </summary>
            ///Idle,
            ///
            /// <summary>
            /// Walking in the world.
            /// </summary>
            /// Walking
            ///
            ///
            /// <summary>
            /// In defense mode
            /// </summary>
            ///Defending,
            ///
            /// <summary>
            /// Performing Dodge Animation
            /// </summary>
            ///Dodging,
            ///
            /// <summary>
            /// Performing Hit Animation
            /// </summary>
            ///Hit,
            ///
            /// <summary>
            /// Dead, but still playing the dying animation.
            /// </summary>
            ///Dying,
            ///
            /// <summary>
            /// Dead, with the dead animation.
            /// </summary>
            ///Dead,
        }


        /// <summary>
        /// The state of this character.
        /// </summary>
        public CharacterState state = CharacterState.Idle;

        /// <summary>
        /// Returns true if the character is dead or dying.
        /// </summary>
        public bool IsDeadOrDying
        {
            get
            {
                return ((state == CharacterState.Dying) ||
                    (state == CharacterState.Dead));             
            }
        }


        #endregion

        #region Map Data

        public Point mapSize;
        public Point mapPosition;
        public Point tilePosition
        {
            get { return new Point(mapPosition.X / Globals.tileSize, mapPosition.Y / Globals.tileSize); }
        }
        public Rectangle mapRect
        {
            get
            {
                return new Rectangle(mapPosition.X, mapPosition.Y,
                    MapSprite.FrameDimensions.X, MapSprite.FrameDimensions.Y);
            }
        }


        public Directions Direction;

        /*virtual*/public bool SetMapPosition(int x_offset, int y_offset, int max_x, int max_y) 
        {
            int new_x_loc_pixel = mapPosition.X + x_offset;
            int new_y_loc_pixel = mapPosition.Y + y_offset;

            if ((new_x_loc_pixel < 0) || (new_x_loc_pixel >= (max_x * Globals.tileSize)) ||
                    (new_y_loc_pixel < 0) || (new_y_loc_pixel >= (max_y * Globals.tileSize)))
                return false;
            else
            {
                mapPosition.X = new_x_loc_pixel;
                mapPosition.Y = new_y_loc_pixel;
                return true;
            }
        }

        #endregion


        #region Graphics Data

        public AnimatedSprite MapSprite;

        /// <summary>
        /// The animating sprite for the map view of this character as it walks.
        /// </summary>
        /// <remarks>
        /// If this object is null, then the animations are taken from MapSprite.
        /// </remarks>
        //private AnimatedSprite walkingSprite;

        /// <summary>
        /// The animating sprite for the map view of this character as it walks.
        /// </summary>
        /// <remarks>
        /// If this object is null, then the animations are taken from MapSprite.
        /// </remarks>
        //[ContentSerializer(Optional = true)]
        //public AnimatedSprite WalkingSprite
        //{
        //    get { return walkingSprite; }
        //    set { walkingSprite = value; }
        //}


        /// <summary>
        /// Reset the animations for this character.
        /// </summary>
        public virtual void ResetAnimation(bool isMoving)
        {
            //state = isWalking ? CharacterState.Moving : CharacterState.Idle;
            if (MapSprite != null)
            {
                //if (isWalking && mapSprite["Walk" + Direction.ToString()] != null)
                //{
                //    mapSprite.PlayAnimation("Walk", Direction);
                //}
                //else
                //{
                MapSprite.PlayAnimation("Idle", Direction);
                //}
            }
            //if (walkingSprite != null)
            //{
            //    if (isWalking && walkingSprite["Walk" + Direction.ToString()] != null)
            //    {
            //        walkingSprite.PlayAnimation("Walk", Direction);
            //    }
            //    else
            //    {
            //        walkingSprite.PlayAnimation("Idle", Direction);
            //    }
            //}
        }


        /// <summary>
        /// The small blob shadow that is rendered under the characters.
        /// </summary>
        //private Texture2D shadowTexture;

        /// <summary>
        /// The small blob shadow that is rendered under the characters.
        /// </summary>
        //[ContentSerializerIgnore]
        //public Texture2D ShadowTexture
        //{
        //    get { return shadowTexture; }
        //    set { shadowTexture = value; }
        //}

        #endregion


        #region Standard Animation Data

        /// <summary>
        /// The default idle-animation interval for the animating map sprite.
        /// </summary>
        private int mapIdleAnimationInterval = 200;

        /// <summary>
        /// The default idle-animation interval for the animating map sprite.
        /// </summary>
        [ContentSerializer(Optional = true)]
        public int MapIdleAnimationInterval
        {
            get { return mapIdleAnimationInterval; }
            set { mapIdleAnimationInterval = value; }
        }


        /// <summary>
        /// Add the standard character idle animations to this character.
        /// </summary>
        //private void AddStandardCharacterIdleAnimations(int col_index, int row_index, int frame_count)
        //{
        //    if (mapSprite != null)
        //    {
        //        mapSprite.AddAnimation(new Animation("Idle", col_index, row_index, frame_count,
        //            MapIdleAnimationInterval, true));

        //        //mapSprite.AddAnimation(new Animation("IdleSouth", 1, 6,
        //        //    MapIdleAnimationInterval, true));
        //        //mapSprite.AddAnimation(new Animation("IdleSouthwest", 7, 12,
        //        //    MapIdleAnimationInterval, true));
        //        //mapSprite.AddAnimation(new Animation("IdleWest", 13, 18,
        //        //    MapIdleAnimationInterval, true));
        //        //mapSprite.AddAnimation(new Animation("IdleNorthwest", 19, 24,
        //        //    MapIdleAnimationInterval, true));
        //        //mapSprite.AddAnimation(new Animation("IdleNorth", 25, 30,
        //        //    MapIdleAnimationInterval, true));
        //        //mapSprite.AddAnimation(new Animation("IdleNortheast", 31, 36,
        //        //    MapIdleAnimationInterval, true));
        //        //mapSprite.AddAnimation(new Animation("IdleEast", 37, 42,
        //        //    MapIdleAnimationInterval, true));
        //        //mapSprite.AddAnimation(new Animation("IdleSoutheast", 43, 48,
        //        //    MapIdleAnimationInterval, true));
        //    }
        //}


        /// <summary>
        /// The default walk-animation interval for the animating map sprite.
        /// </summary>
        //private int mapWalkingAnimationInterval = 80;

        /// <summary>
        /// The default walk-animation interval for the animating map sprite.
        /// </summary>
        //[ContentSerializer(Optional = true)]
        //public int MapWalkingAnimationInterval
        //{
        //    get { return mapWalkingAnimationInterval; }
        //    set { mapWalkingAnimationInterval = value; }
        //}


        /// <summary>
        /// Add the standard character walk animations to this character.
        /// </summary>
        //private void AddStandardCharacterWalkingAnimations()
        //{
        //    AnimatedSprite sprite = (walkingSprite == null ? mapSprite : walkingSprite);
        //    if (sprite != null)
        //    {
        //        sprite.AddAnimation(new Animation("WalkSouth", 1, 6,
        //            MapWalkingAnimationInterval, true));
        //        sprite.AddAnimation(new Animation("WalkSouthwest", 7, 12,
        //            MapWalkingAnimationInterval, true));
        //        sprite.AddAnimation(new Animation("WalkWest", 13, 18,
        //            MapWalkingAnimationInterval, true));
        //        sprite.AddAnimation(new Animation("WalkNorthwest", 19, 24,
        //            MapWalkingAnimationInterval, true));
        //        sprite.AddAnimation(new Animation("WalkNorth", 25, 30,
        //            MapWalkingAnimationInterval, true));
        //        sprite.AddAnimation(new Animation("WalkNortheast", 31, 36,
        //            MapWalkingAnimationInterval, true));
        //        sprite.AddAnimation(new Animation("WalkEast", 37, 42,
        //            MapWalkingAnimationInterval, true));
        //        sprite.AddAnimation(new Animation("WalkSoutheast", 43, 48,
        //            MapWalkingAnimationInterval, true));
        //    }
        //}


        #endregion

        #region Content Type Reader


        /// <summary>
        /// Reads a Character object from the content pipeline.
        /// </summary>
        //public class CharacterReader : ContentTypeReader<Character>
        //{
        //    /// <summary>
        //    /// Reads a Character object from the content pipeline.
        //    /// </summary>
        //    protected override Character Read(ContentReader input,
        //        Character existingInstance)
        //    {
        //        Character character = existingInstance;
        //        if (character == null)
        //        {
        //            throw new ArgumentNullException("existingInstance");
        //        }

        //        input.ReadRawObject<WorldObject>(character as WorldObject);

        //        character.MapIdleAnimationInterval = input.ReadInt32();
        //        character.MapSprite = input.ReadObject<AnimatedSprite>();
        //        if (character.MapSprite != null)
        //        {
        //            character.MapSprite.SourceOffset =
        //                new Vector2(
        //                character.MapSprite.SourceOffset.X - 32,
        //                character.MapSprite.SourceOffset.Y - 32);
        //        }
        //        //character.AddStandardCharacterIdleAnimations();

        //        //character.MapWalkingAnimationInterval = input.ReadInt32();
        //        //character.WalkingSprite = input.ReadObject<AnimatedSprite>();
        //        //if (character.WalkingSprite != null)
        //        //{
        //        //    character.WalkingSprite.SourceOffset =
        //        //        new Vector2(
        //        //        character.WalkingSprite.SourceOffset.X - 32,
        //        //        character.WalkingSprite.SourceOffset.Y - 32);
        //        //}
        //        //character.AddStandardCharacterWalkingAnimations();

        //        character.ResetAnimation(false);

        //        //character.shadowTexture = input.ContentManager.Load<Texture2D>(
        //        //    @"Textures\Characters\CharacterShadow");

        //        return character;
        //    }
        //}


        #endregion
    }
}
