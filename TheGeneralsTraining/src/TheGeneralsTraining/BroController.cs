using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace TheGeneralsTraining
{
    public static class BroController
    {
        public static bool BroIsStronger(BroBase bro)
        {
            if(bro.GetFieldValue<bool>("performanceEnhanced"))
            {
                return true;
            }
            if(bro is BroniversalSoldier &&  (bro as BroniversalSoldier).GetFieldValue<float>("serumTime") > 0)
            {
                return true;
            }
            if(bro is Brominator && (bro as Brominator).brominatorMode)
            {
                return true;
            }
            return false;
        }
    }
}
