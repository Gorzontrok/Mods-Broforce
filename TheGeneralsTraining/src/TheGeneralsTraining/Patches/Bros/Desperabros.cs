using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace TheGeneralsTraining.Patches.Bros.Desperabro0
{

    [HarmonyPatch(typeof(Desperabro), "SetupMariachiBro")]
    static class Desperabro_MariachiDontStealLives_Patch
    {
        static void Postfix(Desperabro __instance)
        {
            if (Main.CanUsePatch && Main.settings.mariachisNoLongerStealLivesFromDesperabro)
            {
                __instance.player = null;
                __instance.playerNum = 5;
            }
        }
    }
}
