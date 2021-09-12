using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace TweaksFromPigs
{
    [HarmonyPatch(typeof(IndianaBrones), "AnimateMelee")]
    static class IndianaBrones_AchievementRework_Patch
    {
        static void Prefix(IndianaBrones __instance)
        {
            if (!Main.enabled) return;
            Traverse instTrav = Traverse.Create(__instance);
            TestVanDammeAnim nearbyMook = instTrav.Field("nearbyMook").GetValue<TestVanDammeAnim>();
            if (instTrav.Field("meleeFrame").GetValue<int>() == 2 && nearbyMook != null && nearbyMook.CanBeThrown() && instTrav.Field("highFive").GetValue<bool>())
            {
                instTrav.Method("CancelMelee").GetValue();
                instTrav.Method("ThrowBackMook", new object[] { nearbyMook }).GetValue() ;

                Transform parentedToTransform = Traverse.Create(__instance).Field("nearbyMook").GetValue<TestVanDammeAnim>().GetParentedToTransform();
                if (parentedToTransform != null && parentedToTransform.name.ToUpper().Contains("BOSS"))
                {
                    SteamController.UnlockAchievement(SteamAchievement.noticket);
                }
            }
                
        }
    }
    
    // DoubleBroSeven Patch
    [HarmonyPatch(typeof(DoubleBroSeven), "Awake")]
    static class DoubleBroSeven_Awake_Patch
    {
        static void Postfix(DoubleBroSeven __instance)
        {
            if (!Main.enabled) return;

            if (Main.settings.UseFifthBondSpecial)
            {
                Traverse.Create(typeof(DoubleBroSeven)).Field("_specialAmmo").SetValue(5);
                __instance.SpecialAmmo = 5;
                __instance.originalSpecialAmmo = 5;
            }
        }
    }
    
    // BrondleFly Patch
    [HarmonyPatch(typeof(BrondleFly), "Awake")]
    static class BrondleFly_Awake_Patch
    {
        static void Postfix(BrondleFly __instance)
        {
            if (!Main.enabled) return;

            TestVanDammeAnim baBroracus = HeroController.GetHeroPrefab(HeroType.BaBroracus);
            __instance.soundHolder.attackSounds = (baBroracus as BaBroracus).soundHolder.attackSounds;
        }
    }
    
    // BroneyRoss Patch
    [HarmonyPatch(typeof(BroneyRoss), "Awake")]
    static class BroneyRoss_Awake_Patch
    {
        static void Postfix(BroneyRoss __instance)
        {
            if (!Main.enabled) return;

            TestVanDammeAnim broHard = HeroController.GetHeroPrefab(HeroType.BroHard);
            __instance.soundHolder.attractSounds = broHard.soundHolder.attackSounds;
        }
    }

    // LeeBroxmas Patch
    [HarmonyPatch(typeof(LeeBroxmas), "Awake")]
    static class LeeBroxmas_Awake_Patch
    {
        static void Postfix(LeeBroxmas __instance)
        {
            if (!Main.enabled) return;

            TestVanDammeAnim blade = HeroController.GetHeroPrefab(HeroType.Blade);
            Texture bladeKnifeTex =  (blade as Blade).throwingKnife.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture;

            __instance.projectile.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = bladeKnifeTex;
            __instance.macheteSprayProjectile.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = bladeKnifeTex;
        }
    }
    [HarmonyPatch(typeof(BronnarJensen), "UseFire")]
    static class BronnarJensen_FixShoot_Patch
    {
        static bool Prefix(BronnarJensen __instance)
        {
            if (!Main.enabled) return true;
            if (__instance.IsMine)
            {
                if (Traverse.Create(__instance).Field("ducking").GetValue<bool>() && __instance.down)
                {
                    Traverse.Create(__instance).Method("PlayAttackSound", new object[] { 0.4f }).GetValue();
                    Traverse.Create(__instance).Method("FireWeapon", new object[] { __instance.X + __instance.transform.localScale.x * 6f, __instance.Y + 7f, __instance.transform.localScale.x * (__instance.shootGrenadeSpeedX * 0.3f) + __instance.xI * 0.45f, 25f + ((__instance.yI <= 0f) ? 0f : (__instance.yI * 0.3f)) }).GetValue();
                }
                else
                {
                    Traverse.Create(__instance).Method("PlayAttackSound", new object[] { 0.4f }).GetValue();
                    Traverse.Create(__instance).Method("FireWeapon", new object[] { __instance.X + __instance.transform.localScale.x * 6f, __instance.Y + 10f, __instance.transform.localScale.x * __instance.shootGrenadeSpeedX + __instance.xI * 0.45f, __instance.shootGrenadeSpeedY + ((__instance.yI <= 0f) ? 0f : (__instance.yI * 0.3f)) }).GetValue();
                }
            }
            Map.DisturbWildLife(__instance.X, __instance.Y, 60f, __instance.playerNum);
            __instance.fireDelay = 0.6f;

            return false;
        }
    }

    // TrentBroser Patch
    [HarmonyPatch(typeof(TrentBroser), "Awake")]
    static class TrentBroser_Awake_Patch
    {
        static void Postfix(TrentBroser __instance)
        {
            if (!Main.enabled) return;

            TestVanDammeAnim broDredd = HeroController.GetHeroPrefab(HeroType.BroDredd);
            __instance.soundHolder.attackSounds = broDredd.soundHolder.attackSounds;
        }
    }
}
