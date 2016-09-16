using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryProject
{
    public class ConditionRes : Stats
    {
        public int poisionRes;
        public int diseaseRes;
        public int paralyzeRes;
        public int mutationRes;
        public int blindRes;
        public int silenceRes;
        public int charmRes;
        public int confuseRes;
        public int fearRes;

        public ConditionRes()
        { }

        public ConditionRes(int PO_RES, int DIS_RES, int PAR_RES, int MUT_RES, int BLI_RES,
            int SIL_RES, int CHA_RES, int CONF_RES, int FEAR_RES)
        {
            ResetStats(Globals.statHelper.conResCount);
            statList[0] = poisionRes = PO_RES;
            statList[1] = diseaseRes = DIS_RES;
            statList[2] = paralyzeRes = PAR_RES;
            statList[3] = mutationRes = MUT_RES;
            statList[4] = blindRes = BLI_RES;
            statList[5] = silenceRes = SIL_RES;
            statList[6] = charmRes = CHA_RES;
            statList[7] = confuseRes = CONF_RES;
            statList[8] = fearRes = FEAR_RES;

            statStringList[0] = "POIS RES";
            statStringList[1] = "DISE RES";
            statStringList[2] = "PARA RES";
            statStringList[3] = "MUTA RES";
            statStringList[4] = "BLIN RES";
            statStringList[5] = "SILN RES";
            statStringList[6] = "CHAM RES";
            statStringList[7] = "CONF RES";
            statStringList[8] = "FEAR RES";
        }
    }
}
