using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace LibraryProject
{
    static public class CharacterLoader
    {
        static public string line;
        static private string lineStop = "***";

        static private string pathStr;
        static private string loadStr;

        static private StringBuilder sb;
        static public ContentManager cman;

        static public void Initialize(ContentManager contentMan)
        {
            sb = new StringBuilder();
            cman = contentMan;
            pathStr = @"Content\CharacterData";
        }

        static public Character LoadCharacter(ref ContentManager contentMan, string char_name, string char_type)//, ref Character character)
        {
            Character character = new Character();

            line = string.Empty;
            loadStr = string.Empty;

            sb.Clear();
            
            sb.Append(pathStr);
            sb.Append(@"\");
            sb.Append(char_type);
            sb.Append("Data");
            sb.Append(@"\");
            sb.Append(char_name);
            sb.Append(@".txt");
            loadStr = sb.ToString();

            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream(loadStr);
                System.IO.StreamReader char_txt_file = new System.IO.StreamReader(stream);

                if ((line = char_txt_file.ReadLine()) == "START")
                {
                    LoadAnimations(ref character, ref char_txt_file);                 
                    //GetName(ref map_template, "MAP_SET_NAME", ref map_set_txt_file);
                    //GetMapType(ref map_template, "MAP_TYPE", ref map_set_txt_file);
                    //GetRoomTypes(ref map_template, "ROOM_TYPES", ref map_set_txt_file);
                    //GetRoomTypeProbabilities(ref map_template, "ROOM_PROBABILITIES", ref map_set_txt_file);
                    //GetRoomSizesRange(ref map_template, "ROOM_SIZES_RANGE", ref map_set_txt_file);
                    //GetRoomBarriers(ref map_template, "ROOM_BARRIER_SIZES", ref map_set_txt_file);
                    //GetTileSets(ref map_template, "TILE_SETS", ref map_set_txt_file);
                }

                char_txt_file.Close();
            }
            catch(System.IO.FileNotFoundException)
            {

            }

            return character;
        }

        static public void LoadAnimations(ref Character character, ref System.IO.StreamReader reader)
        {
            int s_count = 0;

            string animation_name = string.Empty;
            string tex_name = string.Empty;
            int col_idx = 0;
            int row_idx = 0;
            int frame_count = 0;
            int interval = 0;
            bool is_loop = false;

            while (line != "TEX2D_NAME")
                line = reader.ReadLine();

            tex_name = @"Content\";
            tex_name = reader.ReadLine();
            
            character.MapSprite = new AnimatedSprite();
            //character.MapSprite.texture = cman.Load<Texture2D>(tex_name);

            character.MapSprite.textureName = tex_name;

            while(line != "SPRITE_COUNT")
                line = reader.ReadLine();

            s_count = Convert.ToInt32(reader.ReadLine());

            for(int count = 0; count < s_count; count++)
            {
                while(line != "ANIMATION_NAME")
                    line = reader.ReadLine();

                animation_name = reader.ReadLine();

                while (line != "COLUMN_INDEX")
                    line = reader.ReadLine();

                col_idx = Convert.ToInt32(reader.ReadLine());

                while (line != "ROW_INDEX")
                    line = reader.ReadLine();

                row_idx = Convert.ToInt32(reader.ReadLine());

                while (line != "FRAME_COUNT")
                    line = reader.ReadLine();

                frame_count = Convert.ToInt32(reader.ReadLine());

                while (line != "INTERVAL")
                    line = reader.ReadLine();

                interval = Convert.ToInt32(reader.ReadLine());

                while (line != "ISLOOP")
                    line = reader.ReadLine();

                line = reader.ReadLine();

                if (line == "true")
                    is_loop = true;
                else
                    is_loop = false;

                character.MapSprite.AddAnimation(new Animation(animation_name, col_idx, row_idx, frame_count, interval, is_loop));
            }

            
        }
    }
}
