using System;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace TheGeneralsTraining.Patches.Bros.IndianaBrones0
{
    // Fix No Ticket achievement
    [HarmonyPatch(typeof(IndianaBrones), "AnimateMelee")]
    static class IndianaBrones_FixNoTicketAchievement_Patch
    {
        static void Prefix(IndianaBrones __instance)
        {
            if (Main.CanUsePatch)
            {
                try
                {
                    Traverse t = Traverse.Create(__instance);
                    TestVanDammeAnim nearbyMook = t.GetFieldValue<TestVanDammeAnim>("nearbyMook");
                    if (t.GetFieldValue<int>("meleeFrame") == 2 && nearbyMook != null && nearbyMook.CanBeThrown() && t.GetFieldValue<bool>("highFive"))
                    {
                        t.Method("CancelMelee").GetValue();
                        t.Method("ThrowBackMook", new object[] { nearbyMook }).GetValue();

                        Transform parentedToTransform = t.GetFieldValue<TestVanDammeAnim>("nearbyMook").GetParentedToTransform();
                        if (parentedToTransform != null && parentedToTransform.name.ToUpper().Contains("BOSS"))
                        {
                            SteamController.UnlockAchievement(SteamAchievement.noticket);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Main.ExceptionLog("Failed to fix Indiana achievement", ex);
                }
            }
        }
    }
}

