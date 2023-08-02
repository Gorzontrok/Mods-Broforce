using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace Surprise.Terrorist.Patch
{
    // Patch Armoured Guy
    [HarmonyPatch(typeof(MookArmouredGuy), "Awake")]
    static class MookArmouredGuy_PressHighFiveMelee_Patch
    {
        static void Prefix(MookArmouredGuy __instance)
        {
            if (Main.enabled)
            {
                __instance.pilotUnit = UnityEngine.Object.Instantiate<Unit>(Map.Instance.activeTheme.mookBigGuy);
            }
        }
    }
    [HarmonyPatch(typeof(MookArmouredGuy), "DisChargePilotRPC")] // NEED TO TEST
    static class PilotDontDie_Patch
    {
        static bool Prefix(MookArmouredGuy __instance, float disChargeYI, bool stunPilot, Unit dischargedBy)
        {
            if (Main.enabled)
            {
                Traverse trav = Traverse.Create(__instance);
                if (__instance.pilotUnit != dischargedBy)
                {
                    __instance.pilotUnit.GetComponent<Renderer>().enabled = true;
                    __instance.pilotUnit.DischargePilotingUnit(__instance.X, Mathf.Clamp(__instance.Y + 32f, -6f, 100000f), __instance.xI + ((!stunPilot) ? 0f : ((float)(UnityEngine.Random.Range(0, 2) * 2 - 1) * disChargeYI * 0.3f)), disChargeYI + 100f + ((__instance.pilotUnit.playerNum >= 0) ? 0f : (disChargeYI * 0.5f)), stunPilot);
                    trav.Method("StopPlayerBubbles").GetValue();
                    __instance.pilotUnit = null;
                    __instance.playerNum = -1;
                    trav.Field("isHero").SetValue(false);
                    __instance.firingPlayerNum = -1;
                    __instance.fire = false;
                    __instance.hasBeenPiloted = true;
                    trav.Method("DeactivateGun").GetValue();
                    if (__instance.health > 0)
                    {
                        __instance.Damage(__instance.health + 1, DamageType.SelfEsteem, 0f, 0f, 0, __instance, __instance.X, __instance.Y);
                    }
                    trav.Method("SetSyncingInternal", new object[] { false }).GetValue();
                }
                return false;
            }
            return true;
        }
    }


    // Patch MookTruck
    [HarmonyPatch(typeof(MookTruck), "Start")]
    static class MookTruckStart_Patch
    {
        static void Postfix(MookTruck __instance)
        {
            NewTruckTest newTruckTest = __instance.gameObject.AddComponent<NewTruckTest>();
            newTruckTest.maximumSpawnMook = __instance.mooksToSpawn;
        }
    }
    [HarmonyPatch(typeof(MookTruck), "FireWeapon")]
    static class BetterTruckSpawn_Patch
    {
        static bool Prefix(MookTruck __instance)
        {
            if (Main.enabled)
            {
                Traverse trav = Traverse.Create(__instance);

                int mookSpawnCount = trav.Field("mookSpawnCount").GetValue<int>();
                if (mookSpawnCount < __instance.mooksToSpawn)
                {
                    trav.Field("mookSpawnCount").SetValue(mookSpawnCount++);
                    if (Connect.IsHost)
                    {
                        Mook mook = __instance.GetComponent<NewTruckTest>().SpawnMook(__instance.X + 28f, __instance.Y + 32f, (float)(55 + mookSpawnCount % 4 * 15), 150f, __instance.enemyAI.mentalState == MentalState.Alerted);
                        if (trav.Field("stunTime").GetValue<float>() > 0f && mook != null)
                        {
                            mook.enemyAI.FullyAlert(__instance.X, __instance.Y, -1);
                            mook.Blind(8f);
                        }
                    }
                }
                return false;
            }
            return true;
        }

    }
}

