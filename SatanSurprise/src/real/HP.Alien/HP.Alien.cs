using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace Surprise
{
    // Patch Alien spawn after facehugger insemination
    [HarmonyPatch(typeof(TestVanDammeAnim), "BurstChest")]
    static class TestVanDammeAnim_BurstChest_Patch
    {
        static void Postfix(TestVanDammeAnim __instance)
        {
            int maxAlien = 3;
            if (Main.HardMode) maxAlien = 5;
            for(int i =0; i < maxAlien;i++)
            {
                Mook component = __instance.alienBursterPrefab.GetComponent<Mook>();
                Mook mook = MapController.SpawnMook_Networked(component, __instance.X, __instance.Y, 0f, 0f, false, false, false, false, false);
                Networking.Networking.RPC<Mook, float, float, bool, int>(PID.TargetAll, new RpcSignature<Mook, float, float, bool, int>(__instance.ReleaseAlien), mook, 0f, 0f, false, HeroController.GetNearestPlayer(__instance.X, __instance.Y, 160f, 64f), true);
                Networking.Networking.RPC(PID.TargetAll, new RpcSignature(mook.GrowFromChestBurster), true);
            }
            
        }
    }
}
