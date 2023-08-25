using System;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace BroforceOverhaul.Levels.MapData0
{
    [HarmonyPatch(typeof(MapData), "Deserialize")]
    static class MapData_ChangeProbabilitySpawn_Patch
    {
        static void Postfix(MapData __instance)
        {
            if (Main.enabled && LevelController.ModCanEditMap && __instance.musicType == MusicType.Alien)
            {
                __instance.acidBarrelSpawnProbability = LevelController.acidBarrelProbability;
            }
        }
    }
}

