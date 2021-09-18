using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;
using System.Reflection;

namespace ExpendablesBrosInGame_Mod
{
    static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        private static Harmony harmony;

        static Dictionary<int, HeroType> ExpendablesBro_dico = new Dictionary<int, HeroType>() {
            { 500, HeroType.BroneyRoss },
            { 510, HeroType.LeeBroxmas },
            { 524, HeroType.BronnarJensen },
            { 534, HeroType.HaleTheBro },
            { 540, HeroType.TrentBroser },
            { 548, HeroType.Broc },
            { 560, HeroType.TollBroad }
        };

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            settings = Settings.Load<Settings>(modEntry);
            try
            {
                harmony = new Harmony(modEntry.Info.Id);
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);
            }
            catch (Exception e)
            {
                mod.Logger.Log(e.ToString());
            }
            mod = modEntry;
            try
            {
                CheckIfFilteredBrosIsHere();
            }catch(Exception ex) { mod.Logger.Log(ex.ToString()); }

            return true;
        }
        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            var warning = new GUIStyle();
            warning.normal.textColor = Color.yellow;
            warning.fontStyle = FontStyle.Bold;
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("If you have 'FilteredBros', the modification for spawn will be disabled but fix not.", warning, GUILayout.ExpandWidth(false));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            settings.brondflyEnabled = GUILayout.Toggle(settings.brondflyEnabled, "Enable Brondfly", GUILayout.ExpandWidth(false));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }
        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }
        internal static void Log(object str)
        {
            mod.Logger.Log(str.ToString());
        }

        public static void CheckIfFilteredBrosIsHere()
        {
            // https://harmony.pardeike.net/articles/basics.html#checking-for-existing-patches

            // retrieve all patches
            var patches = Harmony.GetPatchInfo(typeof(HeroUnlockController).GetMethod("IsAvailableInCampaign"));
            if (patches is null)
            {
                Main.Log("Patches is null.");
                return; // not patched
            }
            
            // get info about all Prefixes/Postfixes/Transpilers
            foreach (var patch in patches.Prefixes)
            {
                if (patch.owner == "FilteredBrosMod")
                {
                    harmony.Unpatch(typeof(HeroUnlockController).GetMethod("IsAvailableInCampaign"), HarmonyPatchType.Prefix, mod.Info.Id);
                    Main.Log("Mod Unpatch for avoiding conflict with FilteredBros !");
                }
            }
        }

        internal static Dictionary<int, HeroType> CheckExpendables(Dictionary<int, HeroType> Bro_dico)
        {
            try
            {
                int brondflyStep = 600;
                //Add Expendable Bros
                foreach (KeyValuePair<int, HeroType> bros in ExpendablesBro_dico)
                {
                    if (!Main.enabled && Bro_dico.Contains(bros))
                    {
                        Main.Log("Remove " + bros.Value + ".....");
                        Bro_dico.Remove(bros.Key);
                    }
                    else if (Main.enabled && !Bro_dico.Contains(bros))
                    {
                        if (bros.Value == HeroType.BrondleFly & settings.brondflyEnabled)
                            Main.Log(bros.Value + " is missing ! Adding....");
                        Bro_dico.Add(bros.Key, bros.Value);
                    }
                }

                //Add or remove Brondfly depend on option
                if (Main.enabled && settings.brondflyEnabled && !Bro_dico.ContainsValue(HeroType.BrondleFly))
                {
                    Main.Log("Brondfly is missing ! Adding....");
                    Bro_dico.Add(brondflyStep, HeroType.BrondleFly);
                }
                else if ((!Main.enabled || !settings.brondflyEnabled) && Bro_dico.ContainsValue(HeroType.BrondleFly))
                {
                    Main.Log("Remove Brondfly.....");
                    Bro_dico.Remove(brondflyStep);
                }
            }
            catch (Exception ex) { Main.Log(ex); }
            return Bro_dico;
        }
    }

    [HarmonyPatch(typeof(HeroUnlockController), "IsAvailableInCampaign")] //Patch for add the Bros
    static class HeroUnlockController_IsAvailableInCampaign_Patch
    {
        // After my mod FiltredBros, i have no idea how this can working..
        public static void Prefix()
        {
            Dictionary<int, HeroType> newHeroUnlockIntervals = Traverse.Create(typeof(HeroUnlockController)).Field("_heroUnlockIntervals").GetValue() as Dictionary<int, HeroType>;
            Traverse.Create(typeof(HeroUnlockController)).Field("_heroUnlockIntervals").SetValue(Main.CheckExpendables(newHeroUnlockIntervals));
        }
    }

    public class Settings : UnityModManager.ModSettings
    {
        public bool brondflyEnabled;
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
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
            if (HeroUnlockController.IsExpendaBro(__instance.heroType)) return false;
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
            Texture bladeKnifeTex = (blade as Blade).throwingKnife.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture;

            __instance.projectile.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = bladeKnifeTex;
            __instance.macheteSprayProjectile.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = bladeKnifeTex;
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