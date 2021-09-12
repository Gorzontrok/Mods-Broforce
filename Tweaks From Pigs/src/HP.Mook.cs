using System;
using UnityEngine;
using HarmonyLib;

namespace TweaksFromPigs
{
    // Mook patch
    [HarmonyPatch(typeof(Mook), "TearGas")]
    static class ForgetPlayerInTearGas_Patch
    {
        static void Prefix(Mook __instance)
        {
            if (!Main.enabled) return;
            __instance.ForgetPlayer(__instance.playerNum);
            //Map.ForgetPlayer(base.playerNum, true, false);
        }
    }
     [HarmonyPatch(typeof(Mook), "Awake")]
    static class Mook_Awake_Patch
    {
        static void Prefix(Mook __instance)
        {
            if (!Main.enabled) return;
            if (Main.settings.TeargasEveryone) __instance.canBeTearGased = true;
        }
    }

}
