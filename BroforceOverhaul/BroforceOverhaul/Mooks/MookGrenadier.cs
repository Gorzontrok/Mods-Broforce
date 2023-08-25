using System;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace BroforceOverhaul.Mooks.MookGrenadier0
{
    [HarmonyPatch(typeof(MookGrenadier), "Start")]
    static class MookGrenadier_DontStrungUp_Patch
    {
        static void Prefix(Mook __instance)
        {
            if (Main.enabled)
            {
                __instance.skinnedPrefab = null;
            }
        }
    }

    [HarmonyPatch(typeof(Unit), "IsHeavy")]
    static class Unit_GrenadierIsHeavy_Patch
    {
        static bool Prefix(Unit __instance, ref bool __result)
        {
            if (Main.enabled && __instance is MookGrenadier)
            {
                __result = true;
                return false;
            }
            return true;
        }
    }
}
