using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace TweaksFromPigs
{
    public static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        public static Dictionary<int, HeroType> HeroDictionary = new Dictionary<int, HeroType>();

        public static bool origDicIsSet = true;
        public static bool origUnpatchFilteredBros;

        public static Harmony harmony;
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            modEntry.OnUpdate = OnUpdate;
            modEntry.OnHideGUI = OnHideGUI;
            settings = Settings.Load<Settings>(modEntry);

            mod = modEntry;

            try
            {
                harmony = new Harmony(modEntry.Info.Id);
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);
            }
            catch (Exception ex)
            {
                mod.Logger.Log("Failed to Patch Harmony !\n" + ex.ToString());
            }


            try { Start(); }
            catch(Exception ex) { mod.Logger.Log(ex.ToString()); }

            if (!settings.getFirstLaunch) FirstLaunch();

            return true;
        }
        static void Start()
        {
            var IsAvailableInC_Patch = typeof(HeroUnlockController).GetMethod("IsAvailableInCampaign");
            settings.FilteredBrosIsHere = CheckIfThisModIsHereAndUnpatchPrefix(IsAvailableInC_Patch, "FilteredBrosMod", settings.UnpatchFilteredBros);

            origUnpatchFilteredBros = settings.UnpatchFilteredBros;
        }

        static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            if (!Main.enabled) return;
            HeroDictionary = Utility.BuildHeroDictionary();

            if (settings.TbagEnabled) TestVanDammeAnim.teaBagCheatEnabled = true;
            else TestVanDammeAnim.teaBagCheatEnabled = false;

            if (settings.UseCustomFramerate)
            { Application.targetFrameRate = settings.MaxFramerate; QualitySettings.vSyncCount = 0; }
            else QualitySettings.vSyncCount = 1;

            if (settings.ShowExtra) BtnExtraTxt = "Hide buggy section";
            else BtnExtraTxt = "Show buggy section";

            settings.NeedReload = ThatVarHaveChangeValue();
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            var TitleStyle = new GUIStyle();
            TitleStyle.fontStyle = FontStyle.Bold;
            TitleStyle.fontSize = 13;
            TitleStyle.normal.textColor = Color.white;

            var ReloadStyle = new GUIStyle();
            ReloadStyle.fontSize = 15;
            ReloadStyle.normal.textColor = Color.yellow;

           // if (GUILayout.Button("df")) ;
           
            GUILayout.BeginHorizontal();
            if(settings.NeedReload)
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("If you want to change to be done you need to reload !", ReloadStyle);
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            settings.UseCustomFramerate = GUILayout.Toggle(settings.UseCustomFramerate, "Use custom Framerate");
            GUILayout.EndHorizontal();
            if (settings.UseCustomFramerate)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                GUILayout.Label("Max Framerate :", GUILayout.ExpandWidth(false));
                int.TryParse(GUILayout.TextField(settings.MaxFramerate.ToString(), GUILayout.Width(100)), out settings.MaxFramerate);
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(10);
            GUILayout.Label("- Sensitive variable :", TitleStyle);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            if (settings.FilteredBrosIsHere) settings.UnpatchFilteredBros = GUILayout.Toggle(settings.UnpatchFilteredBros, "Unpatch Filtered Bros on start");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            
            GUILayout.Space(10);
            GUILayout.Label("- Animation :", TitleStyle);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            settings.UsePushingFrame = GUILayout.Toggle(settings.UsePushingFrame, "Use Pushing Frame");
            settings.UseNewLadderFrame = GUILayout.Toggle(settings.UseNewLadderFrame, "Use New Ladder Animation");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(10);
            GUILayout.Label("- Hero Unlock Controller :", TitleStyle);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            settings.ChangeHeroUnlock = GUILayout.Toggle(settings.ChangeHeroUnlock, "Use custom HeroUnlock of this mod");
            settings.SpawnWithExpendabros = GUILayout.Toggle(settings.SpawnWithExpendabros, "Spawn With Expendabros");
            settings.SpawnBrondeFly = GUILayout.Toggle(settings.SpawnBrondeFly, "Spawn With Brondle Fly");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(10);
            GUILayout.Label("- HUD :", TitleStyle);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            settings.ShowFacehuggerHUD = GUILayout.Toggle(settings.ShowFacehuggerHUD, "Show FaceHugger on HUD");
            settings.SkeletonDeadFace = GUILayout.Toggle(settings.SkeletonDeadFace, "Show skeleton Dead Face");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(10);
            GUILayout.Label("- Bro :", TitleStyle);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            settings.TbagEnabled = GUILayout.Toggle(settings.TbagEnabled, "Enabled T-Bag");
            settings.UseFifthBondSpecial = GUILayout.Toggle(settings.UseFifthBondSpecial, "Use the 5th special of 007");
            settings.MookForgetPlayerInTearGasInsteadOfChocking = GUILayout.Toggle(settings.MookForgetPlayerInTearGasInsteadOfChocking, "Enemies forget player in teargas instead of chocking");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(10);
            GUILayout.Label("- Map :", TitleStyle);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            settings.UseAcidBarrel = GUILayout.Toggle(settings.UseAcidBarrel, "Enabled Acid barrel to spawn");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(10);
            GUILayout.Label("- Other :", TitleStyle);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            settings.PigAreAlwaysTerror = GUILayout.Toggle(settings.PigAreAlwaysTerror, "Pig always have hoodie");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();



            GUILayout.Space(10);
            var ExtraStyle = new GUIStyle("button");
            ExtraStyle.fontStyle = FontStyle.Bold;
            ExtraStyle.fontSize = 15;
            ExtraStyle.normal.textColor = Color.yellow;

            GUILayout.BeginHorizontal();
            GUILayout.Space(100);
            settings.ShowExtra = GUILayout.Toggle(settings.ShowExtra, BtnExtraTxt, ExtraStyle);
            GUILayout.Space(100);
            GUILayout.EndHorizontal();
            if (settings.ShowExtra) ExtraGUI();
        }
        private static string BtnExtraTxt = "Show buggy section";

        static void ExtraGUI()
        {
            var InfoStyle = new GUIStyle();
            InfoStyle.normal.textColor = Color.yellow;

            var OptionalToggleStyle = new GUIStyle("toggle");
            GUILayout.BeginVertical("box");

            GUILayout.Label("These option are here because they are buggy.", InfoStyle);
            settings.CloseExtraOnExit = GUILayout.Toggle(settings.CloseExtraOnExit, "Close this when exit");
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            settings.TeargasEveryone = GUILayout.Toggle(settings.TeargasEveryone, "Teargas everyone", OptionalToggleStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        public static bool CheckIfThisModIsHereAndUnpatchPrefix(MethodInfo methodInfo, string HarmonyId, bool unpatchThis, HarmonyPatchType patchType = HarmonyPatchType.Prefix)
        {
            // https://harmony.pardeike.net/articles/basics.html#checking-for-existing-patches

            // retrieve all patches
            var patches = Harmony.GetPatchInfo(methodInfo);
            if (patches is null) return false; // not patched

            // get info about all Prefixes/Postfixes/Transpilers
            foreach (var patch in patches.Prefixes)
            {
                if (patch.owner == HarmonyId)
                {
                    Main.Log(HarmonyId + " here");
                    if(unpatchThis)
                    {
                        harmony.Unpatch(methodInfo, patchType, HarmonyId);
                        Main.Log(HarmonyId + "Unpatch !");
                    }
                    return true;
                }
            }
            return false;
        }

        static bool ThatVarHaveChangeValue()
        {
            if (settings.UnpatchFilteredBros != origUnpatchFilteredBros) return true;
            return false;
        }

        static void FirstLaunch()
        {
            settings.getFirstLaunch = true;

            // Bros addon/fix
            settings.UseFifthBondSpecial = true;
            settings.MookForgetPlayerInTearGasInsteadOfChocking = true;

            // HUD
            settings.ShowFacehuggerHUD = true;
            settings.SkeletonDeadFace = true;
         }

        static void OnHideGUI(UnityModManager.ModEntry modEntry)
        {
            if (settings.CloseExtraOnExit) settings.ShowExtra = false;
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
        public static void Log(object str)
        {
            mod.Logger.Log(str.ToString());
        }
    }

    public class Settings : UnityModManager.ModSettings
    {
        public bool getFirstLaunch;
        public bool NeedReload = false;

        // Animation
        public bool UsePushingFrame;
        public bool UseNewLadderFrame;

        // Bros addon/fix
        public bool TbagEnabled;
        public bool UseFifthBondSpecial;
        public bool MookForgetPlayerInTearGasInsteadOfChocking;

        // Bro Spawn
        public bool ChangeHeroUnlock;
        public bool SpawnWithExpendabros;
        public bool SpawnBrondeFly;

        // Framerate
        public bool UseCustomFramerate;
        public int MaxFramerate;

        // HUD
        public bool ShowFacehuggerHUD;
        public bool SkeletonDeadFace;

        // Map
        public bool UseAcidBarrel;

        // Other
        public bool PigAreAlwaysTerror;


        // Extra
        public bool ShowExtra;
        public bool CloseExtraOnExit;
        public bool TeargasEveryone;

        // Harmony Id
        public bool FilteredBrosIsHere = false;
        public bool UnpatchFilteredBros;


        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
