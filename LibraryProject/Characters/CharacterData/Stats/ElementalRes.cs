using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryProject
{
    public class ElementalRes : Stats
    {
        public int fireRes;
        public int iceRes;
        public int waterRes;
        public int earthRes;
        public int airRes;
        public int lightningRes;

        public int lightRes;
        public int darkRes;

        public int arcaneRes;
        public int voidRes;
        public int chaosRes;

        public ElementalRes()
        { }

        public ElementalRes(int F_RES, int I_RES, int W_RES, int E_RES, int A_RES, int LIT_RES,
            int L_RES, int D_RES, int ARC_RES, int VOID_RES, int CHAOS_RES)
        {
            ResetStats(Globals.statHelper.eleResCount);
            statList[0] = fireRes = F_RES;
            statList[1] = iceRes = I_RES;
            statList[2] = waterRes = W_RES;
            statList[3] = earthRes = E_RES;
            statList[4] = airRes = A_RES;
            statList[5] = lightningRes = LIT_RES;
            statList[6] = lightRes = L_RES;
            statList[7] = darkRes = D_RES;
            statList[8] = arcaneRes = ARC_RES;
            statList[9] = voidRes = VOID_RES;
            statList[10] = chaosRes = CHAOS_RES;

            statStringList[0] = "FIRE RES";
            statStringList[1] = "FRST RES";
            statStringList[2] = "WATR RES";
            statStringList[3] = "EART RES";
            statStringList[4] = "WIND RES";
            statStringList[5] = "LITN RES";
            statStringList[6] = "LIGH RES";
            statStringList[7] = "DARK RES";
            statStringList[8] = "ARCN RES";
            statStringList[9] = "VOID RES";
            statStringList[10] = "CAOS RES";
        }
    }
}
