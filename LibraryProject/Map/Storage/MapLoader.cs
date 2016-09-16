using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;
//using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System.Xml;

namespace LibraryProject
{
    static public class MapLoader
    {
        static public StorageDevice sDevice;
        static public StorageContainer sContainer;
        static public IAsyncResult result;

        static public int nonBlankCount;
        
        static public void MapToSavedMap(ref AreaMap map, ref string file_name)//ref string content_path, )
        {
            nonBlankCount = 0;
            XmlSerializer x_ser;
            StreamWriter m_writer;
            XmlWriterSettings settings = new XmlWriterSettings();
            
            SavedMap sMap = new SavedMap();

            settings.Indent = true;

            string test = AppDomain.CurrentDomain.BaseDirectory;
            test += "test_map.xml";         
            m_writer = new StreamWriter(test);//file_name);

            sMap.squareSize = Globals.tileSize;
            sMap.mapWidth = map.widthTiles;
            sMap.mapHeight = map.heightTiles;
            sMap.squareTextureNameList = ToStringCondenser(ref map, 0); 
            sMap.notBlankSquareGrid = ToStringCondenser(ref map, 1);
            sMap.roomIDGrid = ToStringCondenser(ref map, 2);
            sMap.altitudeGrid = ToStringCondenser(ref map, 3);
            sMap.tex2DIndices = ToStringCondenser(ref map, 4);
            sMap.BGtiles = ToStringCondenser(ref map, 5);
            sMap.FGtiles = ToStringCondenser(ref map, 6);
            sMap.MsStates = ToStringCondenser(ref map, 7);
            sMap.StartPnt = ToStringCondenser(ref map, 8);

            x_ser = new XmlSerializer(typeof(SavedMap));
            x_ser.Serialize(m_writer, sMap);
            m_writer.Close();
        }

        static public void SavedMapToMap(ref string file_name, out AreaMap new_map)
        {
            XmlSerializer x_ser;
            StreamReader m_reader;

            SavedMap lMap = new SavedMap();

            string test = AppDomain.CurrentDomain.BaseDirectory;
            test += "test_map.xml";

            x_ser = new XmlSerializer(typeof(SavedMap));
            m_reader = new StreamReader(test);
            lMap = (SavedMap)x_ser.Deserialize(m_reader);

            new_map = new AreaMap(lMap.mapWidth);
            CondensedStringToMapField(ref new_map, ref lMap, 0); // map texture names
            CondensedStringToMapField(ref new_map, ref lMap, 1); // room IDs
            CondensedStringToMapField(ref new_map, ref lMap, 2); // altitudes
            CondensedStringToMapField(ref new_map, ref lMap, 3); // tex2D indices
            CondensedStringToMapField(ref new_map, ref lMap, 4); // BG tiles
            CondensedStringToMapField(ref new_map, ref lMap, 5); // FG tiles
            CondensedStringToMapField(ref new_map, ref lMap, 6); // MS states
            CondensedStringToMapField(ref new_map, ref lMap, 7); // start point

            m_reader.Close();
        }

        #region Condensers

        static public string ToStringCondenser(ref AreaMap map, int field)
        {
            int mHeight = map.heightTiles;
            int mWidth = map.widthTiles;
            int numLineBreaks = mHeight;
            StringBuilder map_str_builder;
            int str_builder_capacity = 0;

            string str_end = "STR_END ";
            int str_end_l = str_end.Length;

            switch(field)
            {
                case 0: // map texture names
                    str_builder_capacity = 50;
                    break;
                case 1: // non-blank tiles
                    str_builder_capacity = (((1 + 1) * (map.widthTiles * map.heightTiles)) + str_end_l + mHeight + 10);
                    break;
                case 2: // roomIDS
                    str_builder_capacity = (((3 + 1) * nonBlankCount) + str_end_l + mHeight + 10);
                    break;
                case 3: // altitudes
                    str_builder_capacity = (((3 + 1) * nonBlankCount) + str_end_l + mHeight + 10);
                    break;
                case 4: // tex2D indices
                    str_builder_capacity = (((1 + 1) * nonBlankCount) + str_end_l + mHeight + 10);
                    break;
                case 5: // (BG tile (x,y) source)-(FrameCount)'(FrameLength))
                    str_builder_capacity = (((17) * nonBlankCount) + str_end_l + mHeight + 10);
                    break;
                case 6: // (FG tile (x,y) source)-(FrameCount)'(FrameLength))
                    str_builder_capacity = (((17) * nonBlankCount) + str_end_l + mHeight + 10);
                    break;
                case 7: // mapSquare states (state1,state2,state, etc...)
                    str_builder_capacity = (((12) * nonBlankCount) + str_end_l + mHeight + 10);
                    break;
                case 8: // start point
                    str_builder_capacity = 25;
                    break;
                default:
                    break;
            }

            map_str_builder = new StringBuilder(str_builder_capacity);

            switch(field)
            {
                case 0: // map texture names
                    mHeight = map.TextureNameList.Count();

                    for (int i = 0; i < mHeight; i++)
                    {
                        map_str_builder.Append(map.TextureNameList[i]);
                        map_str_builder.Append(" ");
                    }
                    break;
                case 1: // non-blank tiles
                    for (int row = 0; row < mHeight; row++)
                    {
                        for (int col = 0; col < mWidth; col++)
                            if (map.MSGrid[row][col].MSStates[(int)MSFlagIndex.BL_ST] != MSFlag.BL)
                            {
                                map_str_builder.Append("1 ");
                                nonBlankCount++;
                            }
                            else
                                map_str_builder.Append("0 ");

                        map_str_builder.Append("\n");
                    }
                    break;
                case 2: // room IDs
                    for (int row = 0; row < mHeight; row++)
                    {
                        for (int col = 0; col < mWidth; col++)
                        {
                            if (map.MSGrid[row][col].MSStates[(int)MSFlagIndex.BL_ST] != MSFlag.BL)
                            {
                                map_str_builder.Append(map.MSGrid[row][col].RoomID.ToString());
                                map_str_builder.Append(" ");
                            }
                        }
                        map_str_builder.Append("\n");
                    }
                    break;
                case 3: // altitudes
                    for (int row = 0; row < mHeight; row++)
                    {
                        for (int col = 0; col < mWidth; col++)
                        {
                            if (map.MSGrid[row][col].MSStates[(int)MSFlagIndex.BL_ST] != MSFlag.BL)
                            {
                                map_str_builder.Append(map.MSGrid[row][col].Altitude.ToString());
                                map_str_builder.Append(" ");
                            }
                        }
                        map_str_builder.Append("\n");
                    }
                    break;
                case 4: // tex2D indices
                    for (int row = 0; row < mHeight; row++)
                    {
                        for (int col = 0; col < mWidth; col++)
                        {
                            if (map.MSGrid[row][col].MSStates[(int)MSFlagIndex.BL_ST] != MSFlag.BL)
                            {
                                map_str_builder.Append(map.MSGrid[row][col].MapTexIndex.ToString());
                                map_str_builder.Append(" ");
                            }
                        }
                        map_str_builder.Append("\n");
                    }
                    break;
                case 5: // (BG tile (x,y) source)-(FrameCount)'(FrameLength))
                    for (int row = 0; row < mHeight; row++)
                    {
                        for (int col = 0; col < mWidth; col++)
                        {
                            if (map.MSGrid[row][col].MSStates[(int)MSFlagIndex.BL_ST] != MSFlag.BL)
                            {
                                map_str_builder.Append(map.MSGrid[row][col].BgTile.SourceRect.X.ToString());
                                map_str_builder.Append(",");
                                map_str_builder.Append(map.MSGrid[row][col].BgTile.SourceRect.Y.ToString());
                                map_str_builder.Append("-");
                                map_str_builder.Append(map.MSGrid[row][col].BgTile.FrameCount.ToString());
                                map_str_builder.Append("`");
                                map_str_builder.Append(map.MSGrid[row][col].BgTile.MillisecondsPerFrame.ToString());
                                map_str_builder.Append(" "); 
                            }
                        }
                        map_str_builder.Append("\n");
                    }
                    break;
                case 6: // (FG tile (x,y) source)-(FrameCount)'(FrameLength))
                    for (int row = 0; row < mHeight; row++)
                    {
                        for (int col = 0; col < mWidth; col++)
                        {
                            if (map.MSGrid[row][col].MSStates[(int)MSFlagIndex.BL_ST] != MSFlag.BL)
                            {
                                map_str_builder.Append(map.MSGrid[row][col].FgTile.SourceRect.X.ToString());
                                map_str_builder.Append(",");
                                map_str_builder.Append(map.MSGrid[row][col].FgTile.SourceRect.Y.ToString());
                                map_str_builder.Append("-");
                                map_str_builder.Append(map.MSGrid[row][col].FgTile.FrameCount.ToString());
                                map_str_builder.Append("`");
                                map_str_builder.Append(map.MSGrid[row][col].FgTile.MillisecondsPerFrame.ToString());
                                map_str_builder.Append(" ");
                            }
                        }
                        map_str_builder.Append("\n");
                    }
                    break;
                case 7: // mapSquare states (state1,state2,state3, etc...)
                    for (int row = 0; row < mHeight; row++)
                    {
                        for (int col = 0; col < mWidth; col++)
                        {
                            if (map.MSGrid[row][col].MSStates[(int)MSFlagIndex.BL_ST] != MSFlag.BL)
                            {
                                for (int state = 0; state < map.MSGrid[row][col].MSStates.Count() - 1; state++)
                                {
                                    map_str_builder.Append((map.MSGrid[row][col].MSStates[state]).ToString());
                                    map_str_builder.Append(",");
                                }

                                map_str_builder.Append((map.MSGrid[row][col].MSStates[map.MSGrid[row][col].MSStates.Count() - 1]).ToString());
                                map_str_builder.Append(" ");
                            }
                        }
                        map_str_builder.Append("\n");
                    }
                    break;
                case 8: // start point
                    map_str_builder.Append(map.startPoint.X.ToString());
                    map_str_builder.Append(",");
                    map_str_builder.Append(map.startPoint.Y.ToString());
                    map_str_builder.Append(" ");
                    break;
                default:
                    break;
            }
            map_str_builder.Append(str_end);
            return map_str_builder.ToString();
        }

        #endregion

        #region Decondensers

        static public void CondensedStringToMapField(ref AreaMap new_map, ref SavedMap sMap, int field)
        {
            int mHeight = new_map.heightTiles;
            int mWidth = new_map.widthTiles;
            StringBuilder map_str_builder;
            StringBuilder endLine;
            int str_builder_capacity = 20;
            string term_str = string.Empty;
            int string_pos = 0;

            string str_end = "STR_END";

            map_str_builder = new StringBuilder(str_builder_capacity);
            endLine = new StringBuilder(str_builder_capacity);
            endLine.Append(str_end);

            switch (field)
            {
                case 0: // map texture names
                    while(true)
                    {
                        if(sMap.squareTextureNameList[string_pos] != ' ')
                            map_str_builder.Append(sMap.squareTextureNameList[string_pos]);
                        else
                            if (!map_str_builder.Equals(endLine)) // are we not at the end?
                            {
                                new_map.TextureNameList.Add(map_str_builder.ToString());
                                map_str_builder.Clear();
                            }
                            else
                                break;

                        string_pos++;                     
                    }
                    break;
                case 1: // room IDs              
                    for (int row = 0; row < mHeight; row++)
                    {
                        for (int col = 0; col < mWidth; col++)
                        {
                            if (sMap.notBlankSquareGrid[StrGridIndex(mWidth, row, col)] == '1')
                            {
                                while (true)
                                {
                                    if (sMap.roomIDGrid[string_pos] != ' ') // char is not space
                                        map_str_builder.Append(sMap.roomIDGrid[string_pos]);
                                    else // char is space; we're at the end of a term
                                    {
                                        if (!map_str_builder.Equals(endLine))               
                                            new_map.MSGrid[row][col].RoomID = Convert.ToInt32(map_str_builder.ToString());

                                            map_str_builder.Clear();
                                            string_pos++;
                                            break;
                                    }
                                    string_pos++;
                                }
                            }
                        }                   
                    }
                    break;
                case 2: // altitudes
                    for (int row = 0; row < mHeight; row++)
                    {
                        for (int col = 0; col < mWidth; col++)
                        {
                            if (sMap.notBlankSquareGrid[StrGridIndex(mWidth, row, col)] == '1')
                            {
                                while (true)
                                {
                                    if (sMap.altitudeGrid[string_pos] != ' ') // char is not space
                                        map_str_builder.Append(sMap.altitudeGrid[string_pos]);
                                    else // char is space; we're at the end of a term
                                    {
                                        if (!map_str_builder.Equals(endLine))
                                            new_map.MSGrid[row][col].Altitude = (float)Convert.ToDouble(map_str_builder.ToString());

                                        map_str_builder.Clear();
                                        string_pos++;
                                        break;
                                    }
                                    string_pos++;
                                }
                            }
                        }
                    }
                    break;
                case 3: // tex2D indices
                    for (int row = 0; row < mHeight; row++)
                    {
                        for (int col = 0; col < mWidth; col++)
                        {
                            if (sMap.notBlankSquareGrid[StrGridIndex(mWidth, row, col)] == '1')
                            {
                                while (true)
                                {
                                    if (sMap.tex2DIndices[string_pos] != ' ') // char is not space
                                        map_str_builder.Append(sMap.tex2DIndices[string_pos]);
                                    else // char is space; we're at the end of a term
                                    {
                                        if (!map_str_builder.Equals(endLine))
                                            new_map.MSGrid[row][col].MapTexIndex = Convert.ToInt32(map_str_builder.ToString());

                                        map_str_builder.Clear();
                                        string_pos++;
                                        break;
                                    }
                                    string_pos++;
                                }
                            }
                        }
                    }
                    break;
                case 4: // (BG tile (x,y) source)-(FrameCount)`(FrameLength))
                    for (int row = 0; row < mHeight; row++)
                    {
                        for (int col = 0; col < mWidth; col++)
                        {
                            if (sMap.notBlankSquareGrid[StrGridIndex(mWidth, row, col)] == '1')
                            {
                                while (true)
                                {
                                    if (char.IsDigit(sMap.BGtiles[string_pos])) // char is digit
                                        map_str_builder.Append(sMap.BGtiles[string_pos]);
                                    else if(sMap.BGtiles[string_pos] == ',') // source rect.X
                                    {
                                        new_map.MSGrid[row][col].BgTile.SourceRect.X = Convert.ToInt32(map_str_builder.ToString());
                                        map_str_builder.Clear();   
                                    }
                                    else if (sMap.BGtiles[string_pos] == '-') // source rect.Y
                                    {
                                        new_map.MSGrid[row][col].BgTile.SourceRect.Y = Convert.ToInt32(map_str_builder.ToString());
                                        map_str_builder.Clear();                                    
                                    }
                                    else if (sMap.BGtiles[string_pos] == '`') // frame count
                                    {
                                        new_map.MSGrid[row][col].BgTile.FrameCount = Convert.ToInt32(map_str_builder.ToString());
                                        map_str_builder.Clear();                                     
                                    }
                                    else if (sMap.BGtiles[string_pos] == ' ') // frame length
                                    {
                                        if (!map_str_builder.Equals(endLine))
                                            new_map.MSGrid[row][col].BgTile.MillisecondsPerFrame = (float)Convert.ToInt32(map_str_builder.ToString());

                                        map_str_builder.Clear();
                                        string_pos++;

                                        new_map.MSGrid[row][col].BaseColor = Color.White;
                                        if (new_map.MSGrid[row][col].BgTile.FrameCount > 0)
                                            new_map.MSGrid[row][col].BgTile.IsAnimated = true;

                                        break;
                                    }
                                    string_pos++;
                                }
                            }
                        }
                    }
                    break;
                case 5: // (FG tile (x,y) source)-(FrameCount)`(FrameLength))
                    for (int row = 0; row < mHeight; row++)
                    {
                        for (int col = 0; col < mWidth; col++)
                        {
                            if (sMap.notBlankSquareGrid[StrGridIndex(mWidth, row, col)] == '1')
                            {
                                while (true)
                                {
                                    if (char.IsDigit(sMap.FGtiles[string_pos])) // char is digit
                                        map_str_builder.Append(sMap.FGtiles[string_pos]);
                                    else if (sMap.FGtiles[string_pos] == ',') // source rect.X
                                    {
                                        new_map.MSGrid[row][col].FgTile.SourceRect.X = Convert.ToInt32(map_str_builder.ToString());
                                        map_str_builder.Clear();
                                    }
                                    else if (sMap.FGtiles[string_pos] == '-') // source rect.Y
                                    {
                                        new_map.MSGrid[row][col].FgTile.SourceRect.Y = Convert.ToInt32(map_str_builder.ToString());
                                        map_str_builder.Clear();
                                    }
                                    else if (sMap.FGtiles[string_pos] == '`') // frame count
                                    {
                                        new_map.MSGrid[row][col].FgTile.FrameCount = Convert.ToInt32(map_str_builder.ToString());
                                        map_str_builder.Clear();
                                    }
                                    else if (sMap.FGtiles[string_pos] == ' ') // frame length
                                    {
                                        if (!map_str_builder.Equals(endLine))
                                            new_map.MSGrid[row][col].FgTile.MillisecondsPerFrame = (float)Convert.ToInt32(map_str_builder.ToString());

                                        map_str_builder.Clear();
                                        string_pos++;

                                        new_map.MSGrid[row][col].BaseColor = Color.White;
                                        if (new_map.MSGrid[row][col].BgTile.FrameCount > 0)
                                            new_map.MSGrid[row][col].BgTile.IsAnimated = true;

                                        break;
                                    }
                                    string_pos++;
                                }
                            }
                        }
                    }
                    break;
                case 6: // mapSquare states (state1,state2,state, etc...)
                    int ms_index = 0;
                    for (int row = 0; row < mHeight; row++)
                    {
                        for (int col = 0; col < mWidth; col++)
                        {
                            if (sMap.notBlankSquareGrid[StrGridIndex(mWidth, row, col)] == '1')
                            {
                                while (true)
                                {
                                    if (sMap.MsStates[string_pos] != ' ') // not at end of term
                                    {
                                        if (sMap.MsStates[string_pos] != ',') // not at intra term divider
                                            map_str_builder.Append(sMap.MsStates[string_pos]);
                                        else
                                            if (!map_str_builder.Equals(endLine))
                                            {
                                                new_map.MSGrid[row][col].MSStates[ms_index] = (((MSFlag)Enum.Parse(typeof(MSFlag), map_str_builder.ToString(), true)));
                                                ms_index++;
                                                map_str_builder.Clear();
                                            }
                                    }
                                    else // char is space, we're at end of term
                                    {
                                        if (!map_str_builder.Equals(endLine))
                                            new_map.MSGrid[row][col].MSStates[ms_index] = (((MSFlag)Enum.Parse(typeof(MSFlag), map_str_builder.ToString(), true)));

                                        ms_index = 0;
                                        map_str_builder.Clear();
                                        string_pos++;
                                        break;
                                    }
                                    string_pos++;
                                }
                            }
                        }
                    }
                    break;
                case 7: // start point
                    int cur_char = 0;
                    int x = 0;
                    int y = 0;
                    
                    map_str_builder.Append(sMap.StartPnt[cur_char]);
                    cur_char++;

                    while(true)
                    {
                        if (sMap.StartPnt[cur_char] == ',')
                        {
                            x = Convert.ToInt32(map_str_builder.ToString());
                            map_str_builder.Clear();
                        }
                        else if (sMap.StartPnt[cur_char] == ' ')
                        {
                            y = Convert.ToInt32(map_str_builder.ToString());
                            break;
                        }
                        else
                            map_str_builder.Append(sMap.StartPnt[cur_char]);

                        cur_char++;
                    }
                    new_map.startPoint = new Point(x, y);
                    break;
                default:
                    break;
            }
        }

        #endregion

        static public int StrGridIndex(int row_length, int row_index, int column_index)
        {           
            return ( ( (2 * (row_length * row_index) ) + row_index ) + (2 * column_index) );
        }
    }
}
