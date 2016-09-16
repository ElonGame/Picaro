using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LibraryProject
{
    public class Player : Character
    {
        public int vision = 5;

        public Player(ref ContentManager contentMan, string name, Point start, Directions dir, Character character)
            : base(name, start, dir)
        {
            LoadCharacter(character);
            pStats = new PrimStats(21, 17, 28, 34, 22, 19, 25);
            sStats = new SecStats(127, 10, 2, 43, 56);
            MapSprite.PlayAnimation("Idle");
        }

        public void LoadCharacter(Character character)
        {
            mapSize = character.mapSize;

            this.MapSprite = new AnimatedSprite();
            this.MapSprite.textureName = character.MapSprite.textureName;
            //this.MapSprite.texture = contentMan.Load<Texture2D>(this.MapSprite.textureName);

            for (int c = 0; c < character.MapSprite.animations.Count(); c++)
                this.MapSprite.AddAnimation(character.MapSprite.animations[c]);

        }

        public void Update(GameTime gt)
        {
            MapSprite.UpdateAnimation(gt.ElapsedGameTime.Milliseconds);
        }

        public void Draw(ref SpriteBatch sb, ref Vector2 viewport_loc)
        {
            Vector2 offset = MapSprite.mapDrawOrigin - viewport_loc;
            //MapSprite.Draw(sb, new Vector2(mapPosition.X - (int)viewport_loc.X, mapPosition.Y - (int)viewport_loc.Y), 0f);
            MapSprite.Draw(sb, new Vector2(mapPosition.X + (int)offset.X, mapPosition.Y + (int)offset.Y), 0f);
        }

        //public bool SetPlayerMapPosition(int x_offset, int y_offset, int max_x, int max_y)
        //{
        //    int new_x_loc_pixel = mapPosition.X + x_offset;
        //    int new_y_loc_pixel = mapPosition.Y + y_offset;
        //    //int new_max_x_loc_pixel = new_x_loc_pixel + (max_x * Globals.tileSize);
        //    //int new_max_y_loc_pixel = new_y_loc_pixel + (max_y * Globals.tileSize);

        //    if ((new_x_loc_pixel < 0) || (new_x_loc_pixel >= (max_x * Globals.tileSize)) ||
        //            (new_y_loc_pixel < 0) || (new_y_loc_pixel >= (max_y * Globals.tileSize)))
        //        return false;
        //    else
        //    {
        //        mapPosition.X = new_x_loc_pixel;
        //        mapPosition.Y = new_y_loc_pixel;
        //        return true;
        //    }
        //}
    }
}
