using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace Surprise
{
    // Patch AlienXenomorph
    [HarmonyPatch(typeof(AlienXenomorph), "Start")]
    static class AlienXenomorph_Update_Patch
    {
        static void Postfix(AlienXenomorph __instance)
        {
            __instance.health = 8;

            if (Main.HardMode)
            {
                __instance.speed = 100;
                __instance.hearingRangeX = 150;
                __instance.health = 8;
                __instance.canBeAssasinated = false;
                __instance.willPanicWhenOnFire = false;
                __instance.canBeCoveredInAcid = false;
            }
        }
    }
}
