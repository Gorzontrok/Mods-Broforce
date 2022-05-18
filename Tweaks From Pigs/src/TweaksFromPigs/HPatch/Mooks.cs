using System;
using UnityEngine;
using HarmonyLib;

namespace TweaksFromPigs.HPatch.Mooks
{
    [HarmonyPatch(typeof(TestVanDammeAnim), "Revive")]
    static class FasterZombie_Patch
    {
        static void Prefix(TestVanDammeAnim __instance)
        {
            if (!Main.enabled) return;
            if (Main.settings.fasterZombie)
            {
                __instance.speed *= Main.settings.zombieSpeedModifier;
            }
        }
    }

    // Mook patch
    [HarmonyPatch(typeof(Mook), "TearGas")]
    static class ForgetPlayerInTearGas_Patch
    {
        static bool Prefix(Mook __instance, float time)
        {
            if (!Main.enabled || Compatibility._007_Patch.i.IsEnabled) return true;
            if (__instance.canBeTearGased)
            {
                __instance.Stop();
                Traverse.Create(__instance).Field("stunTime").SetValue(time);
                if (__instance.mookType == MookType.Trooper) Traverse.Create(__instance).Field("tearGasChoking").SetValue(true);
                if (__instance.enemyAI != null)
                {
                    __instance.enemyAI.Blind(time + 0.5f);
                }
                Traverse.Create(__instance).Method("DeactivateGun").GetValue();

                return false;
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(Mook), "Awake")]
    static class Mook_Awake_Patch
    {
        static void Prefix(Mook __instance)
        {
            if (!Main.enabled || (Compatibility._007_Patch.i.IsEnabled)) return;

            __instance.canBeTearGased = Main.settings.canTeargasedEveryone && __instance.mookType != MookType.Devil || __instance.mookType != MookType.Vehicle;
        }
    }
    [HarmonyPatch(typeof(TestVanDammeAnim), "SkinUnit")]
    static class testSkinned
    {
        static bool Prefix(Mook __instance)
        {
            if (!Main.enabled || !Main.cheat || !Main.settings.customSkinned) return true;
            try
            {
                Traverse tr = Traverse.Create(__instance);
                if (!__instance.suspendFromRope)
                {
                    __instance.suspendFromRope = true;
                    __instance.GetComponent<SpriteSM>().SetLowerLeftPixel((float)(1 * tr.Field("spritePixelWidth").GetValue<int>()), (float)(tr.Field("spritePixelHeight").GetValue<int>() * 16));
                }
                else
                {
                    __instance.SuspendFromRope();
                }

                return false;
            }
            catch (Exception ex)
            {
                Main.ExceptionLog(ex);
            }
            return true;
        }
    }
    // Patch Satan
    [HarmonyPatch(typeof(Satan), "AnimateDeath")]
    static class IfFadeMisteriouslySatanCantBeThrow_Patch // Don't work
    {
        static void Prefix(Satan __instance)
        {
            if (!Main.enabled) return;

            __instance.canBackSomersault = false;
            __instance.canBackwardTumble = false;
            __instance.canForwardSomersault = false;
            __instance.canTumble = false;
        }
    }
}
