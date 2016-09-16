using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryProject
{
    public class PrimStats : Stats
    {
        public int strength;
        public int constitution;
        public int dexterity;
        public int agility;
        public int intelligence;
        public int spirit;
        public int luck;

        public PrimStats()
        { }

        public PrimStats(int STR, int CON, int DEX, int AGI, int INT, int SPI, int LCK)
        {
            statCount = 7;
            ResetStats(statCount);
            statList[0] = strength = STR;
            statList[1] = constitution = CON;
            statList[2] = dexterity = DEX;
            statList[3] = agility = AGI;
            statList[4] = intelligence = INT;
            statList[5] = spirit = SPI;
            statList[6] = luck = LCK;

            LoadStatStrList("STR", "CON", "DEX", "AGI", "INT", "SPI", "LCK");
        }
    }
}
