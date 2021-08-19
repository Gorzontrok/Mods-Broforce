using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace Surprise
{
    // Patch mine
    [HarmonyPatch(typeof(Mine), "Update")]
    static class StartMine_Patch // don't work
    {
        static void Prefix(Mine __instance)
        {
            try
            {
                Traverse.Create(typeof(Mine)).Field("detonationTime").SetValue(0f);
                Traverse.Create(typeof(Mine)).Field("range").SetValue(80f);
            }catch(Exception ex) { Main.Log(ex); }
            
        }
    }

    // Patch Barrel
    [HarmonyPatch(typeof(BarrelBlock), "Update")]
    static class BarrelBlockUpdate_Patch
    {
        static void Prefix(BarrelBlock __instance)
        {
            __instance.range = 80f;
            __instance.delayExplosionTime = 0.12f;
            if(Main.HardMode)
            {
                __instance.delayExplosionTime = 0f;
            }
        }
    }

    // Patch Checkpoint
    [HarmonyPatch(typeof(CheckPoint), "ReactivateInternal")] // Don't work
    static class CheckpointCantWork_Patch
    {
        static bool Prefix()
        {
            if (Main.HardMode)
            {
                int i = 1;
                return false;
            }
            return true;
        }
    }
}
