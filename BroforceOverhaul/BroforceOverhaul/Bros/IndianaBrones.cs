using System;
using RocketLib;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace BroforceOverhaul.Bros.IndianaBrones0
{
    // Fix No Ticket achievement
    [HarmonyPatch(typeof(IndianaBrones), "AnimateMelee")]
    static class IndianaBrones_FixNoTicketAchievement_Patch
    {
        static void Prefix(IndianaBrones __instance)
        {
            if (Main.enabled)
            {
                try
                {
                    Traverse t = Traverse.Create(__instance);
                    TestVanDammeAnim nearbyMook = t.Field("nearbyMook").GetValue<TestVanDammeAnim>();
                    if (t.Field("meleeFrame").GetValue<int>() == 2 && nearbyMook != null && nearbyMook.CanBeThrown() && t.Field("highFive").GetValue<bool>())
                    {
                        t.Method("CancelMelee").GetValue();
                        t.Method("ThrowBackMook", new object[] { nearbyMook }).GetValue();

                        Transform parentedToTransform = t.Field("nearbyMook").GetValue<TestVanDammeAnim>().GetParentedToTransform();
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

