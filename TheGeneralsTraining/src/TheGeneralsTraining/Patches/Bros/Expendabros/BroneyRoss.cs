using System;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace TheGeneralsTraining.Patches.Bros.Expendabros.BroneyRoss0
{
    // Fix attacks sound
    [HarmonyPatch(typeof(BroneyRoss), "Awake")]
    static class BroneyRoss_FixAttackSounds_Patch
    {
        static void Postfix(BroneyRoss __instance)
        {
            if (Main.CanUsePatch)
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

