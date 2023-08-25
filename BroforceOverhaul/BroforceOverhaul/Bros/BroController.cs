using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace BroforceOverhaul.Bros
{
    public static class BroController
    {
        public static bool BroIsStronger(BroBase bro)
        {
            Traverse t = Traverse.Create(bro);
            if(t.Field("performanceEnhanced").GetValue<bool>())
            {
                return true;
            }
            if(bro is BroniversalSoldier && Traverse.Create(bro as BroniversalSoldier).Field("serumFrenzy").GetValue<bool>())
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
