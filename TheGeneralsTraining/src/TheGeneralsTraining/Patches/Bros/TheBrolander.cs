using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace TheGeneralsTraining.Patches.Bros.TheBrolander0
{
    [HarmonyPatch(typeof(TheBrolander), "IsAmmoFull")]
    static class TheBrolander_NoMoreAmmoPickupWithPocketedSpecial_Patch
    {
        static bool Prefix(TheBrolander __instance, ref bool __result)
        {
            if (Main.CanUsePatch)
            {
                try
                {
                    if (World.Generation.MapGenV4.ProcGenGameMode.UseProcGenRules)
                    {
                        __result = __instance.SpecialAmmo >= 6;
                    }
                    else if (__instance.pockettedSpecialAmmo.Count > 0)
                    {
                        __result = true;
                    }
                    else
                    {
                        __result = __instance.SpecialAmmo >= __instance.maxSpecialAmmo;
                    }
                }
                catch(Exception e)
                {
                    Main.Log(e);
                }
                return false;
            }
            return true;
        }
    }
}
