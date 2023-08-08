using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using TheGeneralsTraining.Components.Bros;

namespace TheGeneralsTraining.Patches.Projectiles.Chakram0
{

    [HarmonyPatch(typeof(Chakram), "RunChakramCatch")]
    static class Chakram_CatchOnlyOnDemand_Patch
    {
        static bool Prefix(Chakram __instance)
        {
            if(Main.CanUsePatch && Main.settings.betterChakram)
            {
                Traverse t = Traverse.Create(__instance);
                if (t.GetFieldValue<bool>("canBeCaught") && __instance.firedBy != null && __instance.firedBy.GetComponent<Xena_Comp>().hasCallChakram)
                {
                    Xebro xebro = __instance.firedBy as Xebro;
                    float f = xebro.X - __instance.X;
                    float f2 = xebro.Y + 10f - __instance.Y;
                    if (Mathf.Abs(f) < 9f && Mathf.Abs(f2) < 14f)
                    {
                        xebro.CatchChakram(__instance);
                        Sound.GetInstance().PlaySoundEffectAt(__instance.soundHolder.special3Sounds, 0.5f, __instance.transform.position, 1f, true, false, false, 0f);
                        t.SetFieldValue("hasPlayedReturnSwooshSound", false);
                        UnityEngine.Object.Destroy(__instance.gameObject);
                    }
                }
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(Chakram), "RunProjectile")]
    static class Chakram_CatchOnlyOnDemand_Patch2
    {
        static void Prefix(Chakram __instance)
        {
            if (Main.CanUsePatch && Main.settings.betterChakram)
            {
                Traverse t = Traverse.Create(__instance);
                __instance.SetFieldValue("canBeCaught", __instance.firedBy.GetComponent<Xena_Comp>().hasCallChakram);
            }
        }
    }
}
