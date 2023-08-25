using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace BroforceOverhaul.Bros.Desperabros
{
    [HarmonyPatch(typeof(TestVanDammeAnim), "ReduceLives")]
    static class TestVanDammeAnim_MariachiDontLooseLife_Patch
    {
        static bool Prefix(TestVanDammeAnim __instance)
        {

            Desperabro desperabro = __instance as Desperabro;
            return Main.enabled && desperabro && desperabro.mariachiBroType == Desperabro.MariachiBroType.Desperabro;
        }
    }
}
