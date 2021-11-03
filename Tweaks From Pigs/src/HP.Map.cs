using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using HarmonyLib;

namespace TweaksFromPigs
{
    [HarmonyPatch(typeof(Map), "PlaceGround")]
    static class ChangeProbabilitySpawn_Patch
    {
        static bool Prefix(Map __instance, Block __result, GroundType placeGroundType, int x, int y, ref Block[,] newBlocks, bool addToRegistry = true)
        {
            Ref.map = __instance;
            if (!Main.enabled || (Compatibility.MapDataController.i.IsHere && Main.settings.MapDataController_Compatibility)) return true;
            if (Main.settings.UseAcidBarrel)
            {
                Map.MapData.acidBarrelSpawnProbability = Main.settings.AcidBarrelSpawnChance;
            }
            return true;
        }
    }
}
