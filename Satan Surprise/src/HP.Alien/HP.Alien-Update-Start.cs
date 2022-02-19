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
                __instance.health = 10;
                __instance.canBeAssasinated = false;
                __instance.willPanicWhenOnFire = false;
                __instance.canBeCoveredInAcid = false;
            }
        }
    }

    // Patch AlienBrute
    [HarmonyPatch(typeof(AlienBrute), "Start")]
    static class AlienBrute_Update_Patch
    {
        static void Postfix(AlienXenomorph __instance)
        {
            __instance.health = 150;

            if (Main.HardMode)
            {
                __instance.immuneToPlasmaShock = false;
                __instance.showElectrifiedFrames = false;
                __instance.willPanicWhenOnFire = false;
                __instance.canBeCoveredInAcid = false;
            }
        }
    }

    // Patch AlienMosquito
    [HarmonyPatch(typeof(AlienMosquito), "Start")]
    static class AlienMosquitto_Start_Patch
    {
        static void Postfix(AlienMosquito __instance)
        {
            __instance.diveSpeed = 300;
            Traverse.Create(typeof(AlienMosquito)).Field("diveDelay").SetValue(0.1f);
            __instance.diveDelayDuration = 0.1f;
            __instance.explodeRange = 80f;
            __instance.diveSpeedIncrease = 200;
        }
    }

    // Patch AlienWormFacehuggerLauncher
    [HarmonyPatch(typeof(AlienWormFacehuggerLauncher), "Start")]
    static class AlienWormFacehuggerLauncher_Start_Patch
    {
        static void Postfix(AlienWormFacehuggerLauncher __instance)
        {
            __instance.health *= 2;
            __instance.fireRate = 0.1f;
            __instance.riseSpeed = 300;
        }
    }

}
