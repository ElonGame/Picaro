using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LibraryProject
{
    public class MapSquare
    {
        public int MSSize;
        public int XIndex;
        public int YIndex;

        public int MapTexIndex; // map texture index; this is how 
        public int RoomID;

        public float Altitude;

        public Color BaseColor;
        public Color AltColor;

        public Rectangle TintRect;
        public Rectangle WorldRect;

        public Tile BgTile;
        public Tile FgTile;
        public List<Tile> MSTiles;

        public List<MSFlag> MSStates;

        public MapSquare()
        {
            MSSize = 55;

            int ms_states_cap = 7;
            InitializeMS(ms_states_cap, 0, 0);
        }

        public MapSquare(int xcoord, int ycoord)
        {
            MSSize = 55;

            int ms_states_cap = 7;
            InitializeMS(ms_states_cap, xcoord, ycoord);
        }

        public void InitializeMS(int ms_states_cap, int x, int y)
        {
            RoomID = -1;
            MapTexIndex = 0;
            Altitude = -1f;

            XIndex = x;
            YIndex = y;

            BaseColor = Color.Black;
            AltColor = Color.White;

            WorldRect = new Rectangle(XIndex * MSSize, YIndex * MSSize, MSSize, MSSize);
            TintRect = new Rectangle(330, 0, MSSize, MSSize);

            MSTiles = new List<Tile>(2);
            MSTiles.Add(new Tile(MSSize));
            MSTiles.Add(new Tile(MSSize));

            BgTile = MSTiles[(int)MSTileIndex.BG];
            FgTile = MSTiles[(int)MSTileIndex.FG];

            MSStates = new List<MSFlag>(ms_states_cap);
            for (int i = 0; i < ms_states_cap; i++)
                MSStates.Add(MSFlag.NULL);
            MSStates[(int)MSFlagIndex.BL_ST] = MSFlag.BL;

            MSStates[(int)MSFlagIndex.IS_MKD] = MSFlag.NT_MKD;
            MSStates[(int)MSFlagIndex.CAN_MAKE] = MSFlag.MAKE_OK;
            MSStates[(int)MSFlagIndex.VIS_LVL] = MSFlag.NO_VIS;
        }

        public void Update(GameTime gt)
        {
            BgTile.Update(gt);
            FgTile.Update(gt);            
        }

        public void Draw(ref SpriteBatch sb, Texture2D texture, Texture2D masksTex, 
            Vector2 map_draw_offset, Vector2 map_engine_viewport_offset)
        {
            Vector2 offset = map_draw_offset - map_engine_viewport_offset;
            int vis_alpha = 0;
            //Rectangle shadow_rect = shadow_rect = new Rectangle(110, 0, 55, 55);

            //switch(MSStates[(int)MSFlagIndex.VIS_LVL])
            //{
            //    case MSFlag.NO_VIS:
            //        vis_alpha = 255;
            //        break;
            //    case MSFlag.VLOW_VIS:
            //        //vis_alpha = 200;
            //        shadow_rect = new Rectangle(55, 0, 55, 55);
            //        break;
            //    case MSFlag.LOW_VIS:
            //        //vis_alpha = 160;
            //        shadow_rect = new Rectangle(55, 0, 55, 55);
            //        break;
            //    case MSFlag.MED_VIS:
            //        //vis_alpha = 80;
            //        shadow_rect = new Rectangle(0, 0, 55, 55);
            //        break;
            //    case MSFlag.HIGH_VIS:
            //        vis_alpha = 0;
            //        break;
            //}
            switch (MSStates[(int)MSFlagIndex.VIS_LVL])
            {
                case MSFlag.NO_VIS:
                    vis_alpha = 255;
                    break;
                case MSFlag.LOW_VIS:
                    vis_alpha = 200;
                    break;
                case MSFlag.MED_VIS:
                    vis_alpha = 120;
                    break;
                case MSFlag.HIGH_VIS:
                    vis_alpha = 0;
                    break;
            }
            
            if (MSStates[(int)MSFlagIndex.IS_MKD] == MSFlag.MKD)
            {
                // BG
                sb.Draw(texture, new Rectangle((WorldRect.X + (int)offset.X), (WorldRect.Y + (int)offset.Y), MSSize, MSSize),
                    BgTile.SourceRect, BaseColor, 0f, Vector2.Zero, SpriteEffects.None, 0.8f);
                // FG
                sb.Draw(texture, new Rectangle((WorldRect.X + (int)offset.X), (WorldRect.Y + (int)offset.Y), MSSize, MSSize),
                    FgTile.SourceRect, BaseColor, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
                // Tint tile
                sb.Draw(texture, new Rectangle((WorldRect.X + (int)offset.X), (WorldRect.Y + (int)offset.Y), MSSize, MSSize),
                    TintRect, new Color(0, 0, 0, 5), 0f, Vector2.Zero, SpriteEffects.None, 0.4f);
            }
            else
            {
                // BG
                sb.Draw(texture, new Rectangle((WorldRect.X + (int)offset.X), (WorldRect.Y + (int)offset.Y), MSSize, MSSize),
                    BgTile.SourceRect, BaseColor, 0f, Vector2.Zero, SpriteEffects.None, 0.8f);
                // FG
                sb.Draw(texture, new Rectangle((WorldRect.X + (int)offset.X), (WorldRect.Y + (int)offset.Y), MSSize, MSSize),
                    FgTile.SourceRect, BaseColor, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);

                if(AltColor != Color.White)
                    sb.Draw(texture, new Rectangle((WorldRect.X + (int)offset.X), (WorldRect.Y + (int)offset.Y), MSSize, MSSize),
                    TintRect, AltColor, 0f, Vector2.Zero, SpriteEffects.None, 0.4f);
                else
                    sb.Draw(texture, new Rectangle((WorldRect.X + (int)offset.X), (WorldRect.Y + (int)offset.Y), MSSize, MSSize),
                        TintRect, new Color(0, 0, 0, vis_alpha), 0f, Vector2.Zero, SpriteEffects.None, 0.4f);
                //sb.Draw(masksTex, new Rectangle((WorldRect.X + (int)offset.X), (WorldRect.Y + (int)offset.Y), MSSize, MSSize),
                //    shadow_rect, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.3f);
            }
        }

        public void DrawHeights(ref SpriteBatch sb, Texture2D texture, Texture2D masksTex, Vector2 map_draw_offset,
            Vector2 map_engine_viewport_offset, ref GraphicsDeviceManager graphics_dev_man, ref Effect effect)
        {
            Vector2 offset = map_draw_offset - map_engine_viewport_offset;
            Rectangle screen_rec = new Rectangle((WorldRect.X + (int)offset.X), (WorldRect.Y + (int)offset.Y), MSSize, MSSize);
            //Rectangle shadow_rect = shadow_rect = new Rectangle(110, 0, 55, 55);

            Vector3 lightDirection = new Vector3(0f, 0f, 2f);

            effect.Parameters["LightDirection"].SetValue(lightDirection);
            effect.Parameters["Height"].SetValue(Altitude);

            //// Set the normalmap texture.
            graphics_dev_man.GraphicsDevice.Textures[1] = texture;

            //// Begin the sprite batch.
            sb.Begin(0, null, null, null, null, effect);

            //// Draw the sprite.
            sb.Draw(texture, screen_rec, BgTile.SourceRect, Color.White);
            //// End the sprite batch.
            sb.End();

            int vis_alpha = 0;

            switch (MSStates[(int)MSFlagIndex.VIS_LVL])
            {
                case MSFlag.NO_VIS:
                    vis_alpha = 255;
                    break;
                case MSFlag.LOW_VIS:
                    vis_alpha = 220; // 180
                    break;
                case MSFlag.MED_VIS:
                    vis_alpha = 100; // 120
                    break;
                case MSFlag.HIGH_VIS:
                    vis_alpha = 0;
                    break;
            }


            sb.Begin();
            //sb.Draw(texture, new Rectangle((WorldRect.X + (int)offset.X), (WorldRect.Y + (int)offset.Y), MSSize, MSSize),
            //    TintRect, new Color(0, 0, 0, vis_alpha), 0f, Vector2.Zero, SpriteEffects.None, 0.4f);
            if (AltColor != Color.White)
                sb.Draw(texture, new Rectangle((WorldRect.X + (int)offset.X), (WorldRect.Y + (int)offset.Y), MSSize, MSSize),
                TintRect, AltColor, 0f, Vector2.Zero, SpriteEffects.None, 0.4f);
            else
                sb.Draw(texture, new Rectangle((WorldRect.X + (int)offset.X), (WorldRect.Y + (int)offset.Y), MSSize, MSSize),
                    TintRect, new Color(0, 0, 0, vis_alpha), 0f, Vector2.Zero, SpriteEffects.None, 0.4f);
            sb.End();
            //sb.Begin();
            //sb.Draw(masksTex, new Rectangle((WorldRect.X + (int)offset.X), (WorldRect.Y + (int)offset.Y), MSSize, MSSize),
            //    shadow_rect, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.3f);
            //sb.End();
        }
    }
}
