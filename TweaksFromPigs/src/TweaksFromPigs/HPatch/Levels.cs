﻿using System;
using HarmonyLib;
using UnityEngine;

namespace TweaksFromPigs.HPatch.Levels
{
    [HarmonyPatch(typeof(MapData), "Deserialize")]
    static class ChangeProbabilitySpawn_Patch
    {
        static void Postfix(MapData __instance)
        {
            if (Main.enabled && TFP_Utility.CanChangeMapValue && Main.settings.useAcidBarrel)
            {
                __instance.acidBarrelSpawnProbability = Main.settings.acidBarrelSpawnProbability;
            }
        }
    }
}
