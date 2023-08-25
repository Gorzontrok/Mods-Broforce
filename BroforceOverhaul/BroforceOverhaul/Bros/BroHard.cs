using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace BroforceOverhaul.Bros.BroHard0
{
    [HarmonyPatch(typeof(TestVanDammeAnim), "GetSpeed", MethodType.Getter)]
    static class TestVanDammeAnim_FasterBroHardWhenDucking_Patch
    {
        static bool Prefix(TestVanDammeAnim __instance, ref float __result)
        {
            if(Main.enabled && __instance is BroHard)
            {
                bool surroundedByBarbedWire = Traverse.Create(__instance).Method("IsSurroundedByBarbedWire").GetValue<bool>();
                if (__instance.player == null)
                {
                    __result = __instance.speed * ((!surroundedByBarbedWire) ? 1f : 0.5f) * (__instance.IsDucking ? 1.2f : 1f);
                }
                __result = __instance.player.ValueOrchestrator.GetModifiedFloatValue(Rogueforce.ValueOrchestrator.ModifiableType.MovementSpeed, __instance.speed) * ((!surroundedByBarbedWire) ? 1f : 0.5f) * (__instance.IsDucking ? 2f : 1f);
                return false;
            }
            return true;
        }
    }

}
