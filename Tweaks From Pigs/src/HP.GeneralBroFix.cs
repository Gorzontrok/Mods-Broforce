using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace TweaksFromPigs
{
    // Patch animation
    [HarmonyPatch(typeof(TestVanDammeAnim), "AnimatePushing")]
    static class AnimatePushing_Patch
    {
        public static bool ThisHeroTypePushingBug(HeroType hero)
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
            if (!Main.enabled) return;
            if(Main.settings.FixPushingAnimation)
            {
                if (ThisHeroTypePushingBug(__instance.heroType))
                {
                    __instance.gunSprite.transform.localPosition = Utility.GetBroGunVector3PositionWhilePushing(__instance.heroType);
                }

                float pushingTime = Traverse.Create(__instance).Field("pushingTime").GetValue<float>();
                if (__instance.fire || pushingTime <= 0)
                {
                    __instance.gunSprite.transform.localScale = new Vector3(-1f, 1f, 1f);
                    __instance.gunSprite.transform.localPosition = Utility.GetBroGunVector3PositionWhenFinishPushing(__instance.heroType);
                    if (ThisHeroTypePushingBug(__instance.heroType))
                        Traverse.Create(HeroController.GetHeroPrefab(__instance.heroType)).Method("SetGunPosition", new object[] { 0, 0 });
                }
            }
        }
    }
    [HarmonyPatch(typeof(TestVanDammeAnim), "AnimateGesture")]
    static class AnimateGesture_Patch
    {
        static bool Prefix(TestVanDammeAnim __instance)
        {
            if (!Main.enabled) return true;
            if(Main.settings.FixExpendabros)
            {
                if (HeroUnlockController.IsExpendaBro(__instance.heroType)) return false;
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(TestVanDammeAnim), "AnimateInseminationFrames")]
    static class AnimateInseminationFrames_Patch
    {
        static bool Prefix(TestVanDammeAnim __instance)
        {
            if (!Main.enabled) return true;
            if(Main.settings.FixExpendabros)
            {
                if (HeroUnlockController.IsExpendaBro(__instance.heroType))
                {
                    Traverse.Create(__instance).Method("AnimateActualDeath").GetValue();
                    return false;
                }
            }
            return true;
        }
    }

    // Patch BroBase
    [HarmonyPatch(typeof(BroBase))]
    [HarmonyPatch("Update")]
    static class UpdateBrobase_Patch
    {
        public static float TearGasTime;
        static void Postfix(BroBase __instance)
        {
            if (!Main.enabled) return;
            if (TearGasTime >= 0)
            {
                TearGasTime -= Time.deltaTime;
                Map.ForgetPlayer(__instance.playerNum, false, false);
            }
            if (__instance.down || Traverse.Create(__instance).Field("ducking").GetValue<bool>())
            {
                __instance.gunSprite.gameObject.SetActive(true);
            }
        }
    }
    [HarmonyPatch(typeof(BroBase), "Awake")]
    static class BroBase_Awake_Patch
    {
        static void Postfix(BroBase __instance)
        {
            if (!Main.enabled) return;
            if (Main.settings.UsePushingFrame && !AnimatePushing_Patch.ThisHeroTypePushingBug(__instance.heroType)) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // Fix Hide in grass
    [HarmonyPatch(typeof(Map), "IsInSubstance")]
    static class HideInGrassRework_Patch
    {
        static bool Prefix(Map __instance, ref Doodad __result, float x, float y, float range)
        {
            if (!Main.enabled) return true;
            if(Main.settings.FixHidingInGrass)
            {
                Extensions.DrawCircle(x, y, range, Color.magenta, 0f);
                for (int i = 0; i < Map.grassAndBlood.Count; i++)
                {
                    if (i >= 0 && Map.grassAndBlood[i] != null && Map.grassAndBlood[i].SubMergesUnit())
                    {
                        if (Mathf.Abs(Map.grassAndBlood[i].centerX - x) <= range + Map.grassAndBlood[i].width / 2f)
                        {
                            if (Mathf.Abs(Map.grassAndBlood[i].centerY - y) <= range + Map.grassAndBlood[i].height / 2f)
                            {
                                __result = Map.grassAndBlood[i];
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
