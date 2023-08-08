using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace TheGeneralsTraining.Patches.Projectiles
{
    [HarmonyPatch(typeof(Projectile), "RunProjectile")]
    static class Projectile_RotateProjectile_Patch
    {
        static void Postfix(Projectile __instance, float t)
        {
            if(Main.CanUsePatch && __instance.GetComponent<RotateProjectile_Comp>() != null)
            {
                float num = 140f + Mathf.Abs(__instance.xI) * 0.5f;
                __instance.transform.Rotate(0f, 0f, num * 400 * t, Space.Self);
            }

        }
    }

    [HarmonyPatch(typeof(Projectile), "MakeEffects")]
    static class Projectile_RotateProjectile2_Patch
    {
        static void Postfix(Projectile __instance)
        {
            if (Main.CanUsePatch && __instance.GetComponent<RotateProjectile_Comp>() != null)
            {
                __instance.transform.rotation = Quaternion.identity;
            }

        }
    }
}
