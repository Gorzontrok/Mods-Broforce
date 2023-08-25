using System;
using RocketLib;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace BroforceOverhaul.Bros.Expendabros.TrentBroser0
{
    // Fix Attack sounds ;  Give the special grenade of Brodell Walker
    [HarmonyPatch(typeof(TrentBroser), "Awake")]
    static class TrentBroser_Awake_Patch
    {
        static void Postfix(TrentBroser __instance)
        {
            if (Main.enabled)
            {
                try
                {
                    TestVanDammeAnim broDredd = HeroController.GetHeroPrefab(HeroType.BroDredd);
                    __instance.soundHolder.attackSounds = broDredd.soundHolder.attackSounds;

                    TestVanDammeAnim brodellWalker = HeroController.GetHeroPrefab(HeroType.BrodellWalker);
                    __instance.specialGrenade = brodellWalker.specialGrenade;

                }
                catch (Exception ex)
                {
                    Main.ExceptionLog("Failed to patch Trent Broser", ex);
                }
            }
        }
    }
}

