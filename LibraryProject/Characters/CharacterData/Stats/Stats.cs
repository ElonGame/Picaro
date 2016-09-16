using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryProject
{
    //public enum Stat { PRIMARY, SECONDARY, CONDITION, ELEMENTAL}

    public class Stats
    {
        public int statCount;
        public List<int> statList;
        public List<string> statStringList;

        public Stats()
        { }

        public Stats(int stat_count)
        {
            ResetStats(stat_count);
        }

        public void ResetStats(int stat_count)
        {
            statCount = stat_count;
            statList = new List<int>(statCount);
            for (int s = 0; s < stat_count; s++)
                statList.Add(0);
            statStringList = new List<string>(statCount);
            for (int s = 0; s < stat_count; s++)
                statStringList.Add(string.Empty);
        }

        public void LoadStatStrList(params string[] stat_strs)
        {
            for (int s = 0; s < stat_strs.Length; s++)
                statStringList[s] = stat_strs[s];
        }

        //#region Content Type Reader

        ////public class StatisticsValueReader : ContentTypeReader<StatisticsValue>
        ////{
        ////    protected override StatisticsValue Read(ContentReader input,
        ////        StatisticsValue existingInstance)
        ////    {
        ////        StatisticsValue output = new StatisticsValue();

        ////        output.HealthPoints = input.ReadInt32();
        ////        output.MagicPoints = input.ReadInt32();
        ////        output.PhysicalOffense = input.ReadInt32();
        ////        output.PhysicalDefense = input.ReadInt32();
        ////        output.MagicalOffense = input.ReadInt32();
        ////        output.MagicalDefense = input.ReadInt32();

        ////        return output;
        ////    }
        ////}


        //#endregion
    }
}
