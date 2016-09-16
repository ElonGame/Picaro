using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryProject
{
    public class StatHelper
    {
        public int primStatCount = 7;
        public int secStatCount = 7;
        public int conResCount = 9;
        public int eleResCount = 11;

        public void Initialize()
        {

        }

        public StatHelper()
        {

        }

        #region Operator Methods

        // returns new combined stat
        public Stats GetCombinedStats(ref Stats stat1, int stat_val, int operation)
        {
            Stats resultStats = new Stats(stat1.statCount);

            switch (operation)
            {
                case -2:
                    for (int s = 0; s < resultStats.statCount; s++)
                        resultStats.statList[s] = (stat1.statList[s] / stat_val);
                    break;
                case -1:
                    for (int s = 0; s < resultStats.statCount; s++)
                        resultStats.statList[s] = (stat1.statList[s] - stat_val);
                    break;
                case 1:
                    for (int s = 0; s < resultStats.statCount; s++)
                        resultStats.statList[s] = (stat1.statList[s] + stat_val);
                    break;
            }

            return resultStats;
        }

        // returns new combined stat
        public Stats GetCombinedStats(ref Stats stat1, ref Stats stat2, int operation)
        {
            Stats resultStats = new Stats(stat1.statCount);

            switch (operation)
            {
                case -2:
                    for (int s = 0; s < resultStats.statCount; s++)
                        resultStats.statList[s] = (stat1.statList[s] / stat2.statList[s]);
                    break;
                case -1:
                    for (int s = 0; s < resultStats.statCount; s++)
                        resultStats.statList[s] = (stat1.statList[s] - stat2.statList[s]);
                    break;
                case 1:
                    for (int s = 0; s < resultStats.statCount; s++)
                        resultStats.statList[s] = (stat1.statList[s] + stat2.statList[s]);
                    break;
            }

            return resultStats;
        }

        // returns new combined stat
        public int GetCombinedStat(int stat, ref Stats stat1, int operation)
        {
            int resultstat = 0;

            switch (operation)
            {
                case -2:
                    resultstat = (stat1.statList[stat] / stat);
                    break;
                case -1:
                    resultstat = (stat1.statList[stat] - stat);
                    break;
                case 1:
                    resultstat = (stat1.statList[stat] + stat);
                    break;
                case 2:
                    resultstat = (stat1.statList[stat] * stat);
                    break;
            }

            return resultstat;
        }

        // modifies passed stat
        public void CombineAllStats(ref Stats stat1, int stat_val, int operation)
        {
            switch (operation)
            {
                case -2:
                    for (int s = 0; s < stat1.statCount; s++)
                        stat1.statList[s] = (stat1.statList[s] / stat_val);
                    break;
                case -1:
                    for (int s = 0; s < stat1.statCount; s++)
                        stat1.statList[s] = (stat1.statList[s] - stat_val);
                    break;
                case 1:
                    for (int s = 0; s < stat1.statCount; s++)
                        stat1.statList[s] = (stat1.statList[s] + stat_val);
                    break;
            }
        }

        // modifies passed stat
        public void CombineAllStats(ref Stats stat1, ref Stats stat2, int operation)
        {
            switch (operation)
            {
                case -2:
                    for (int s = 0; s < stat1.statCount; s++)
                        stat1.statList[s] = (stat1.statList[s] / stat2.statList[s]);
                    break;
                case -1:
                    for (int s = 0; s < stat1.statCount; s++)
                        stat1.statList[s] = (stat1.statList[s] - stat2.statList[s]);
                    break;
                case 1:
                    for (int s = 0; s < stat1.statCount; s++)
                        stat1.statList[s] = (stat1.statList[s] + stat2.statList[s]);
                    break;
            }
        }

        // modifies passed stat
        public void CombineStat(int stat, ref Stats stat1, int operation)
        {
            switch (operation)
            {
                case -2:
                    stat1.statList[stat] = (stat1.statList[stat] / stat);
                    break;
                case -1:
                    stat1.statList[stat] = (stat1.statList[stat] - stat);
                    break;
                case 1:
                    stat1.statList[stat] = (stat1.statList[stat] + stat);
                    break;
                case 2:
                    stat1.statList[stat] = (stat1.statList[stat] * stat);
                    break;
            }
        }

        #endregion

        //#region Operator: +

        //public Stats operator +(Stats stat1, Stats stat2)
        //{
        //    return GetCombinedStats(ref stat1, ref stat2, 1);
        //}

        //public Stats operator +(Stats stat1, int stat_val)
        //{
        //    return GetCombinedStats(ref stat1, stat_val, 1);
        //}

        //#endregion

        //#region Operator: -

        //public Stats operator -(Stats stat1, Stats stat2)
        //{
        //    return GetCombinedStats(ref stat1, ref stat2, -1);
        //}

        //public Stats operator -(Stats stat1, int stat_val)
        //{
        //    return GetCombinedStats(ref stat1, stat_val, -1);
        //}

        //#endregion

        //#region Operator: *

        //public Stats operator /(Stats stat1, int stat_val)
        //{
        //    return GetCombinedStats(ref stat1, stat_val, 2);
        //}

        //#endregion

        //#region Operator: /

        //public Stats operator /(Stats stat1, Stats stat2)
        //{
        //    return GetCombinedStats(ref stat1, ref stat2, -2);
        //}

        //public Stats operator /(Stats stat1, int stat_val)
        //{
        //    return GetCombinedStats(ref stat1, stat_val, -2);
        //}

        //#endregion

        #region Output
       
        ///// <summary>
        ///// Builds a string that describes a modifier, where non-zero stats are skipped.
        ///// </summary>
        public string GetAllStatsStr(ref Stats output_stat)
        {
            StringBuilder sb = new StringBuilder();
            bool firstStatistic = true;

            for (int s = 0; s < output_stat.statCount; s++)
            {
                if (firstStatistic)
                    firstStatistic = false;
                else
                    sb.Append("\n");

                sb.Append(output_stat.statStringList[s]);
                sb.Append(": ");
                sb.Append(output_stat.statList[s].ToString());         
            }

            return sb.ToString();
        }

        ///// <summary>
        ///// Builds a string that describes a modifier, where non-zero stats are skipped.
        ///// </summary>
        public string GetAllStatsModString(ref Stats output_stat)
        {
            StringBuilder sb = new StringBuilder();
            bool firstStatistic = true;

            for (int s = 0; s < output_stat.statCount; s++)
            {
                if (output_stat.statList[s] != 0)
                {
                    if (firstStatistic)
                        firstStatistic = false;
                    else
                        sb.Append("\n");

                    sb.Append(output_stat.statStringList[s]);
                    sb.Append(": ");
                    sb.Append(output_stat.statList[s].ToString());  
                }
            }
            return sb.ToString();
        }

        public string GetStatStr(ref Stats output_stat, int stat)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(output_stat.statStringList[stat]);
            sb.Append(": ");
            sb.Append(output_stat.statList[stat].ToString());
            
            return sb.ToString();
        }

        #endregion

    }
}
