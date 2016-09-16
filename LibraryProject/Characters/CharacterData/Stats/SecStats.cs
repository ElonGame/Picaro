using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryProject
{
    public class SecStats : Stats
    {
        public int healthPoints;
        public int manaPoints;
        public int armorClass;
        public int speedRating;
        public int evasion;
        //public int physicalDamageMin;
        //public int physicalDamageMax;

        public SecStats()
        { }

        public SecStats(int HP, int MP, int AC, int SR, int EV)//, int PHYS_MIN, int PHYS_MAX)
        {
            statCount = 5;
            ResetStats(statCount);
            statList[0] = healthPoints = HP;
            statList[1] = manaPoints = MP;
            statList[2] = armorClass = AC;
            statList[3] = speedRating = SR;
            statList[4] = evasion = EV;
            //statList[5] = physicalDamageMin = PHYS_MIN;
            //statList[6] = physicalDamageMax = PHYS_MAX;

            LoadStatStrList("HP", "MP", "AC", "SR", "EV");//, "PMIN", "PMAX");
            statStringList[0] = "HP";
            statStringList[1] = "MP";
            statStringList[2] = "AC";
            statStringList[3] = "SR";
            statStringList[4] = "EV";
            //statStringList[5] = "PMIN";
            //statStringList[6] = "PMAX";
        }
    }
}
