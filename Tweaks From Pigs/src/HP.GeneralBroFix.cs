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
        static Vector3 GetVector3Position(HeroType hero)
        {
            Vector3 vector = new Vector3(0f, 0f, -0.001f);
            switch(hero)
            {
                case HeroType.Blade: vector = new Vector3(4f, 0f, -1f); break;
                case HeroType.BronanTheBrobarian: vector = new Vector3(3f, 0, -1f); break;
                case HeroType.Nebro: vector = new Vector3(4f, 0, -1f); break;
                case HeroType.TheBrolander: vector = new Vector3(3f, 0, -1f); break;
                case HeroType.HaleTheBro: vector = new Vector3(2f, 0, -1f); break;
                case HeroType.BroveHeart:
                    TestVanDammeAnim broheart = HeroController.GetHeroPrefab(hero);
                    if (!Traverse.Create(broheart).Field("disarmed").GetValue<bool>()) vector = new Vector3(5f, 4, -1f);
                    else vector = new Vector3(3, 0, -1);
                    break;
                case HeroType.BroneyRoss:
                    vector = new Vector3(2, 0, 0); break;
                case HeroType.LeeBroxmas:
                    vector = new Vector3(6, 0, -0.001f); break;
                case HeroType.TheBrode:
                    vector = new Vector3(4, 4, -1); break;
                case HeroType.Brochete:
                    vector = new Vector3(6, 0, 0.001f); break;
            }
            return vector;
        }
        static void Postfix(TestVanDammeAnim __instance)
        {
            if (!Main.enabled) return;
            float pushingTime = Traverse.Create(__instance).Field("pushingTime").GetValue<float>();
            if (__instance.fire || pushingTime <= 0)
            {
                __instance.gunSprite.transform.localScale = new Vector3(-1f, 1f, 1f);
                __instance.gunSprite.transform.localPosition = GetVector3Position(__instance.heroType);
            }
        }
    }

    [HarmonyPatch(typeof(TestVanDammeAnim), "AnimateGesture")]
    static class AnimateGesture_Patch
    {
        static bool Prefix(TestVanDammeAnim __instance)
        {
            if (!Main.enabled) return true;
            if (HeroUnlockController.IsExpendaBro(__instance.heroType)) return false;
            return true;
        }
    }
    [HarmonyPatch(typeof(TestVanDammeAnim), "AnimateInseminationFrames")]
    static class AnimateInseminationFrames_Patch
    {
        static bool Prefix(TestVanDammeAnim __instance)
        {
            if (!Main.enabled) return true; ;
            if (HeroUnlockController.IsExpendaBro(__instance.heroType))
            {
                Traverse.Create(__instance).Method("AnimateActualDeath").GetValue();
                return false;
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
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
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
            return true;
        }
    }
    // Ennemy Forget player when (s)he is in tear gas
    [HarmonyPatch(typeof(Map), "TearGasUnits")]
    static class ForgetPlayerInTearGrass_Patch
    {
        static bool Prefix(Map __instance, int playerNum, float x, float y, float range, float tearGasTime = 9f)
        {
            if (!Main.enabled) return true;
            if (Map.units == null)
            {
                return false;
            }
            for (int i = Map.units.Count - 1; i >= 0; i--)
            {
                Unit unit = Map.units[i];
                if (unit.IsHero)
                {
                    UpdateBrobase_Patch.TearGasTime = tearGasTime;
                }
                if (unit != null && !unit.invulnerable && GameModeController.DoesPlayerNumDamage(playerNum, unit.playerNum))
                {
                    float num = unit.X - x;
                    if (Mathf.Abs(num) - range < unit.width && (unit.Y != y || num != 0f))
                    {
                        float f = unit.Y + unit.height / 2f + 3f - y;
                        if (Mathf.Abs(f) - range < unit.height)
                        {
                            unit.TearGas(tearGasTime);
                        }
                    }
                }
            }
            return false;
        }
    }
}
