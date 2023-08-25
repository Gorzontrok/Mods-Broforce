using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace BroforceOverhaul.Projectiles.Chakram0
{

    [HarmonyPatch(typeof(Chakram), "RunChakramCatch")]
    static class Chakram_CatchOnlyOnDemand_Patch
    {
        static bool Prefix(Chakram __instance)
        {
            if(Main.enabled)
            {
                Traverse t = Traverse.Create(__instance);
                if (t.Field("canBeCaught").GetValue<bool>() && __instance.firedBy != null && __instance.firedBy.GetComponent<BroforceOverhaul.Bros.Xena.Xena_Comp>().hasCallChakram)
                {
                    Xebro xebro = __instance.firedBy as Xebro;
                    float f = xebro.X - __instance.X;
                    float f2 = xebro.Y + 10f - __instance.Y;
                    if (Mathf.Abs(f) < 9f && Mathf.Abs(f2) < 14f)
                    {
                        xebro.CatchChakram(__instance);
                        Sound.GetInstance().PlaySoundEffectAt(__instance.soundHolder.special3Sounds, 0.5f, __instance.transform.position, 1f, true, false, false, 0f);
                        t.Field("hasPlayedReturnSwooshSound").SetValue(false);
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
            if (Main.enabled)
            {
                Traverse t = Traverse.Create(__instance);
                t.Field("canBeCaught").SetValue(__instance.firedBy.GetComponent<Bros.Xena.Xena_Comp>().hasCallChakram);
            }
        }
    }
}
