using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace Surprise.Aliens.Patch
{
    // Patch AlienXenomorph
    [HarmonyPatch(typeof(AlienXenomorph), "Start")]
    static class AlienXenomorph_Update_Patch
    {
        static void Postfix(AlienXenomorph __instance)
        {
        }
    }

    // Patch AlienBrute
    [HarmonyPatch(typeof(AlienBrute), "Start")]
    static class AlienBrute_Update_Patch
    {
        static void Postfix(AlienXenomorph __instance)
        {
        }
    }

    // Patch AlienMosquito
    [HarmonyPatch(typeof(AlienMosquito), "Start")]
    static class AlienMosquitto_Start_Patch
    {
        static void Postfix(AlienMosquito __instance)
        {
        }
    }

    // Patch AlienWormFacehuggerLauncher
    [HarmonyPatch(typeof(AlienWormFacehuggerLauncher), "Start")]
    static class AlienWormFacehuggerLauncher_Start_Patch
    {
        static void Postfix(AlienWormFacehuggerLauncher __instance)
        {
        }
    }

}
