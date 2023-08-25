using System;
using RocketLib;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace BroforceOverhaul.Bros.Expendabros.BroneyRoss0
{
    // Fix attacks sound
    [HarmonyPatch(typeof(BroneyRoss), "Awake")]
    static class BroneyRoss_FixAttackSounds_Patch
    {
        static void Postfix(BroneyRoss __instance)
        {
            if (Main.enabled)
            {
                try
                {
                    TestVanDammeAnim broHard = HeroController.GetHeroPrefab(HeroType.BroHard);
                    __instance.soundHolder.attackSounds = broHard.soundHolder.attackSounds;

                }
                catch (Exception ex)
                {
                    Main.ExceptionLog("Failed to patch Broney Ross", ex);
                }
            }
        }
    }
}

