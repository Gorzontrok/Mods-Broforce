using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TweaksFromPigs.HPatch.GeneralBroFix
{
    // Patch animation
    [HarmonyPatch(typeof(TestVanDammeAnim), "AnimatePushing")]
    static class AnimatePushing_Patch
    {
        internal static bool __instanceHeroTypePushingBug(HeroType hero)
        {
            List<HeroType> heroBuggy = new List<HeroType>() { HeroType.BroneyRoss, HeroType.Blade, HeroType.BronanTheBrobarian, HeroType.Nebro, HeroType.TheBrolander, HeroType.HaleTheBro, HeroType.TheBrode, HeroType.BroveHeart };
            foreach (HeroType heroBug in heroBuggy)
            {
                if (hero == heroBug) return true;
            }
            return false;
        }
        static void Postfix(TestVanDammeAnim __instance)
        {
            try
            {
                if (Main.enabled && Main.settings.fixPushingAnimation)
                {
                    if (__instanceHeroTypePushingBug(__instance.heroType))
                    {
                        __instance.gunSprite.transform.localPosition = TFP_Utility.GetBroGunVector3PositionWhilePushing(__instance.heroType);
                    }

                    float pushingTime = Traverse.Create(__instance).Field("pushingTime").GetValue<float>();
                    if (__instance.fire || pushingTime <= 0)
                    {
                        __instance.gunSprite.transform.localScale = new Vector3(-1f, 1f, 1f);
                        __instance.gunSprite.transform.localPosition = TFP_Utility.GetBroGunVector3PositionWhenFinishPushing(__instance.heroType);
                        if (__instanceHeroTypePushingBug(__instance.heroType))
                            Traverse.Create(HeroController.GetHeroPrefab(__instance.heroType)).Method("SetGunPosition", new object[] { 0, 0 });
                    }
                }
            }
            catch (Exception ex)
            {
                Main.ExceptionLog(ex);
            }
        }
    }
    [HarmonyPatch(typeof(TestVanDammeAnim), "AnimateGesture")]
    static class AnimateGesture_Patch
    {
        static bool Prefix(TestVanDammeAnim __instance)
        {
            return Main.enabled && Main.settings.fixExpendabros && HeroUnlockController.IsExpendaBro(__instance.heroType);
        }
    }
    [HarmonyPatch(typeof(TestVanDammeAnim), "AnimateInseminationFrames")]
    static class AnimateInseminationFrames_Patch
    {
        static bool Prefix(TestVanDammeAnim __instance)
        {
            try
            {
                if (Main.enabled && Main.settings.fixExpendabros && HeroUnlockController.IsExpendaBro(__instance.heroType))
                {
                     Traverse.Create(__instance).Method("AnimateActualDeath").GetValue();
                     return false;
                }
            }
            catch(Exception ex)
            {
                Main.ExceptionLog(ex);
            }
            return true;
        }
    }

    // Patch BroBase
    [HarmonyPatch(typeof(BroBase), "AnimateMeleeCommon")]
    static class UpdateBroBase_Patch
    {
        static bool Prefix(BroBase __instance)
        {
            if (Main.enabled && Main.settings.steroidsThrowEveryone)
            {
                Traverse t = Traverse.Create(__instance);
                t.Method("SetSpriteOffset", new object[] { 0f, 0f }).GetValue();
                t.Field("rollingFrames").SetValue(0);
                if (__instance.frame == 1)
                {
                    __instance.counter -= 0.0334f;
                }
                if (__instance.frame == 6 && t.Field("meleeFollowUp").GetValue<bool>())
                {
                    __instance.counter -= 0.08f;
                    __instance.frame = 1;
                    t.Field("meleeFollowUp").SetValue(false);
                    t.Method("ResetMeleeValues").GetValue();
                }
                t.Field("frameRate").SetValue(0.025f);
                Mook nearbyMook = t.Field("nearbyMook").GetValue<Mook>();
                if (__instance.frame == 2 && nearbyMook != null && (nearbyMook.CanBeThrown() || __instance.IsPerformanceEnhanced) && t.Field("highFive").GetValue<bool>())
                {
                    t.Method("CancelMelee").GetValue();
                    t.Method("ThrowBackMook", new object[] { nearbyMook }).GetValue();
                    nearbyMook = null;
                }
                return false;
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(BroBase), "Awake")]
    static class BroBase_Awake_Patch
    {
        static void Postfix(BroBase __instance)
        {
            try
            {
                __instance.useNewPushingFrames = Main.enabled && Main.settings.usePushingFrame && !AnimatePushing_Patch.__instanceHeroTypePushingBug(__instance.heroType);
                __instance.useNewLadderClimbingFrames = Main.enabled && Main.settings.useNewLadderFrame;
            }
            catch(Exception ex)
            {
                Main.ExceptionLog(ex);
            }
        }
    }
    [HarmonyPatch(typeof(BroBase), "Start")]
    static class BroBase_Start_Patch
    {
        static void Postfix(BroBase __instance)
        {
            if (!Main.enabled) return;
            try
            {
            }
            catch (Exception ex)
            {
                Main.ExceptionLog(ex);
            }
        }
    }

    // Fix Hide in grass
    [HarmonyPatch(typeof(Map), "IsInSubstance")]
    static class HideInGrassRework_Patch
    {
        static bool Prefix(Map __instance, ref Doodad __result, float x, float y, float range)
        {
            try
            {
                if (Main.enabled && Main.settings.fixHidingInGrass)
                {
                    Extensions.DrawCircle(x, y, range, Color.magenta, 0f);
                    for (int i = 0; i < Map.grassAndBlood.Count; i++)
                    {
                        if (i >= 0 && Map.grassAndBlood[i] != null && Map.grassAndBlood[i].SubMergesUnit() && Mathf.Abs(Map.grassAndBlood[i].centerX - x) <= range + Map.grassAndBlood[i].width / 2f && Mathf.Abs(Map.grassAndBlood[i].centerY - y) <= range + Map.grassAndBlood[i].height / 2f)
                        {
                            __result = Map.grassAndBlood[i];
                            return false;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Main.bmod.logger.ExceptionLog("Failed to hide player in grass", ex);
            }
            return true;
        }
    }

    // Remember pocketed special on bro swap.
    [HarmonyPatch(typeof(Player), "SpawnHero")]
    static class RememberPockettedSpecial_Patch
    {
        static List<PockettedSpecialAmmoType> listp = new List<PockettedSpecialAmmoType>();
        static void Prefix(Player __instance)
        {
            if(Main.enabled && Main.settings.rememberPockettedSpecial)
            {
                try
                {
                    if (__instance.character != null && __instance.character.IsAlive())
                    {
                        BroBase bro = __instance.character as BroBase;
                        if (bro)
                        {
                            listp = bro.pockettedSpecialAmmo;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Main.bmod.logger.ExceptionLog("Failed while remembering pocketed special list.", ex);
                }
            }
        }
        static void Postfix(Player __instance)
        {
            try
            {
                if(Main.enabled && Main.settings.rememberPockettedSpecial)
                {
                    BroBase bro = __instance.character as BroBase;
                    if (bro)
                    {
                        bro.pockettedSpecialAmmo = listp;
                        Traverse.Create(bro).Method("SetPlayerHUDAmmo").GetValue();
                        listp = new List<PockettedSpecialAmmoType>();
                    }
                }
            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog("Failed while assign pocketed special list.", ex); }
        }
    }
}
