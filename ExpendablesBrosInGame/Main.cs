using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;
using System.Reflection;

namespace ExpendablesBrosInGame
{
    static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;

        private static Harmony _harmony;

        private static Dictionary<int, HeroType> _expendablesBros = new Dictionary<int, HeroType>()
        {
            { 610, HeroType.BroneyRoss },
            { 620, HeroType.LeeBroxmas },
            { 630, HeroType.BronnarJensen },
            { 640, HeroType.HaleTheBro },
            { 650, HeroType.TrentBroser },
            { 660, HeroType.Broc },
            { 670, HeroType.TollBroad }
        };

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnToggle = OnToggle;
            mod = modEntry;

            try
            {
                _harmony = new Harmony(modEntry.Info.Id);
                var assembly = Assembly.GetExecutingAssembly();
                _harmony.PatchAll(assembly);
            }
            catch (Exception e)
            {
                mod.Logger.Log(e.ToString());
            }

            try
            {
                CheckIfFilteredBrosIsHere();
            }
            catch(Exception ex)
            {
                mod.Logger.Log(ex.ToString());
            }

            return true;
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }

        internal static void Log(object str)
        {
            mod.Logger.Log(str.ToString());
        }

        internal static void CheckIfFilteredBrosIsHere()
        {
            bool isFilteredBrosPresent = UnityModManager.FindMod("FilteredBrosMod") != null;

            if (isFilteredBrosPresent)
            {
                _harmony.Unpatch(typeof(HeroUnlockController).GetMethod("IsAvailableInCampaign"), HarmonyPatchType.Prefix, mod.Info.Id);
                Main.Log("Mod unpatched to avoid conflict with FilteredBros");
            }
        }

        internal static Dictionary<int, HeroType> CheckExpendables(Dictionary<int, HeroType> brosInGame)
        {
            try
            {
                foreach (KeyValuePair<int, HeroType> bros in _expendablesBros)
                {
                    // remove if mod disabled
                    if (!Main.enabled && brosInGame.Contains(bros))
                    {
                        Main.Log("Remove " + bros.Value + ".....");
                        brosInGame.Remove(bros.Key);
                    }
                    // Add if mod enabled
                    else if (Main.enabled && !brosInGame.Contains(bros))
                    {
                        Main.Log(bros.Value + " is missing ! Adding....");
                        brosInGame.Add(bros.Key, bros.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                Main.Log(ex);
            }
            return brosInGame;
        }
    }

    [HarmonyPatch(typeof(HeroUnlockController), "IsAvailableInCampaign")] //Patch to add the Bros
    static class HeroUnlockController_IsAvailableInCampaign_Patch
    {
        public static void Prefix()
        {
            var traverse = Traverse.Create(typeof(HeroUnlockController));
            var _heroUnlockIntervals = traverse.Field("_heroUnlockIntervals");

            Dictionary<int, HeroType> newHeroUnlockIntervals = _heroUnlockIntervals.GetValue() as Dictionary<int, HeroType>;
            _heroUnlockIntervals.SetValue(Main.CheckExpendables(newHeroUnlockIntervals));
        }
    }

    // EXPENDABROS FIX
    //===================

    [HarmonyPatch(typeof(TestVanDammeAnim), "AnimateGesture")]
    static class AnimateGesture_Patch
    {
        static bool Prefix(TestVanDammeAnim __instance)
        {
            if (!Main.enabled) return true;

            if (HeroUnlockController.IsExpendaBro(__instance.heroType))
                return false;
            return true;
        }
    }
    [HarmonyPatch(typeof(TestVanDammeAnim), "AnimateInseminationFrames")]
    static class AnimateInseminationFrames_Patch
    {
        static bool Prefix(TestVanDammeAnim __instance)
        {
            if (!Main.enabled) return true;

            if (HeroUnlockController.IsExpendaBro(__instance.heroType))
            {
                Traverse.Create(__instance).Method("AnimateActualDeath").GetValue();
                return false;
            }
            return true;
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
            __instance.soundHolder.attackSounds = broHard.soundHolder.attackSounds;
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
            Texture bladeKnifeTex = (blade as Blade).throwingKnife.GetComponent<MeshRenderer>().sharedMaterial.mainTexture;

            __instance.projectile.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = bladeKnifeTex;
            __instance.macheteSprayProjectile.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = bladeKnifeTex;
        }
    }

    // Patch Bronnar Jensen
    [HarmonyPatch(typeof(BronnarJensen), "UseFire")]
    static class BronnarJensen_FixShoot_Patch
    {
        static bool Prefix(BronnarJensen __instance)
        {
            if (!Main.enabled) return true;

            if (__instance.IsMine)
            {
                var traverse = Traverse.Create(__instance);
                if (traverse.Field("ducking").GetValue<bool>() && __instance.down)
                {
                    traverse.Method("PlayAttackSound", new object[] { 0.4f }).GetValue();
                    traverse.Method("FireWeapon", new object[] { __instance.X + __instance.transform.localScale.x * 6f, __instance.Y + 7f, __instance.transform.localScale.x * (__instance.shootGrenadeSpeedX * 0.3f) + __instance.xI * 0.45f, 25f + ((__instance.yI <= 0f) ? 0f : (__instance.yI * 0.3f)) }).GetValue();
                }
                else
                {
                    Traverse.Create(__instance).Method("PlayAttackSound", new object[] { 0.4f }).GetValue();
                    traverse.Method("FireWeapon", new object[] { __instance.X + __instance.transform.localScale.x * 6f, __instance.Y + 10f, __instance.transform.localScale.x * __instance.shootGrenadeSpeedX + __instance.xI * 0.45f, __instance.shootGrenadeSpeedY + ((__instance.yI <= 0f) ? 0f : (__instance.yI * 0.3f)) }).GetValue();
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