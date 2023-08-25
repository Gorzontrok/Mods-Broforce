using System;
using RocketLib;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace BroforceOverhaul.Bros.Expendabros.BronnarJensen0
{
    // Shoot at feet if crouch
    [HarmonyPatch(typeof(BronnarJensen), "UseFire")]
    static class BronnarJensen_ShootAtFeet_Patch
    {
        static bool Prefix(BronnarJensen __instance)
        {
            if (Main.enabled)
            {
                try
                {
                    Traverse t = Traverse.Create(__instance);
                    if (__instance.IsMine)
                    {
                        if (t.Field("ducking").GetValue<bool>() && __instance.down)
                        {
                            t.Method("FireWeapon", new object[] { __instance.X + __instance.transform.localScale.x * 6f, __instance.Y + 7f, __instance.transform.localScale.x * (__instance.shootGrenadeSpeedX * 0.3f) + __instance.xI * 0.45f, 25f + ((__instance.yI <= 0f) ? 0f : (__instance.yI * 0.3f)) }).GetValue();
                        }
                        else
                        {
                            t.Method("FireWeapon", new object[] { __instance.X + __instance.transform.localScale.x * 6f, __instance.Y + 10f, __instance.transform.localScale.x * __instance.shootGrenadeSpeedX + __instance.xI * 0.45f, __instance.shootGrenadeSpeedY + ((__instance.yI <= 0f) ? 0f : (__instance.yI * 0.3f)) }).GetValue();
                        }
                        t.Method("PlayAttackSound", new object[] { 0.4f }).GetValue();
                    }
                    Map.DisturbWildLife(__instance.X, __instance.Y, 60f, __instance.playerNum);
                    __instance.fireDelay = 0.6f;
                    return false;
                }

                catch (Exception ex)
                {
                    Main.ExceptionLog("Failed to shoot at Lee Broxmas's foot", ex);
                }
            }
            return true;
        }
    }
}

