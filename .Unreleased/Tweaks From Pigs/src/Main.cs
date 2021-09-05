using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace TweaksFromPigs
{
    using RocketLib;
    public static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        public static List<HeroType> HeroList = RocketLib._HeroUnlockController.HeroTypeList;
        public static List<int> HeroInt = RocketLib._HeroUnlockController.HeroUnlockIntervalsInt;
        public static Dictionary<int, HeroType> HeroDictionary = new Dictionary<int, HeroType>();

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            modEntry.OnUpdate = OnUpdate;
            modEntry.OnHideGUI = OnHideGUI;
            settings = Settings.Load<Settings>(modEntry);

            var harmony = new Harmony(modEntry.Info.Id);
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);
            }
            catch (Exception ex)
            {
                mod.Logger.Log("Failed to Patch Harmony !\n" + ex.ToString());
            }

            mod = modEntry;

            try { Start(); }
            catch(Exception ex) { mod.Logger.Log(ex.ToString()); }

            return true;
        }
        static void Start()
        {
            HeroDictionary = RocketLib._HeroUnlockController.BuildHeroUnlockIntervalsDictionary(HeroList, HeroInt);
        }

        static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            Utility.BuildHeroDictionary();
            if(settings.ChangeHeroUnlock) RocketLib._HeroUnlockController.SetHeroUnlockIntervals(HeroDictionary);

            if (settings.TbagEnabled) TestVanDammeAnim.teaBagCheatEnabled = true;
            else TestVanDammeAnim.teaBagCheatEnabled = false;

            if (settings.UseCustomFramerate)
            { Application.targetFrameRate = settings.MaxFramerate; QualitySettings.vSyncCount = 0; }
            else QualitySettings.vSyncCount = 1;

        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();
            settings.UseCustomFramerate = GUILayout.Toggle(settings.UseCustomFramerate, "Use custom Framerate");
            GUILayout.EndHorizontal();
            if (settings.UseCustomFramerate)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(10);
                GUILayout.Label("Max Framerate :", GUILayout.ExpandWidth(false));
                int.TryParse(GUILayout.TextField(settings.MaxFramerate.ToString(), GUILayout.Width(100)), out settings.MaxFramerate);
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            settings.ChangeHeroUnlock = GUILayout.Toggle(settings.ChangeHeroUnlock, "Use custom HeroUnlock of this mod");
            settings.UsePushingFrame = GUILayout.Toggle(settings.UsePushingFrame, "Use Pushing Frame");
            settings.UseNewLadderFrame = GUILayout.Toggle(settings.UseNewLadderFrame, "Use New Ladder Animation");
            settings.SpawnWithExpendabros = GUILayout.Toggle(settings.SpawnWithExpendabros, "Spawn With Expendabros");
            settings.UseFifthBondSpecial = GUILayout.Toggle(settings.UseFifthBondSpecial, "Use the 5th special of 007");
            settings.ShowFacehuggerHUD = GUILayout.Toggle(settings.ShowFacehuggerHUD, "Show FaceHugger on HUD");
            settings.TbagEnabled = GUILayout.Toggle(settings.TbagEnabled, "Enabled T-Bag");
            settings.PigAreAlwaysTerror = GUILayout.Toggle(settings.PigAreAlwaysTerror, "Pig always have hoodie");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            

            GUILayout.Space(10);

            settings.ShowExtra = GUILayout.Toggle(settings.ShowExtra, "Show extra option");
            if (settings.ShowExtra) ExtraGUI();
        }

        static void ExtraGUI()
        {
            var InfoStyle = new GUIStyle();
            InfoStyle.normal.textColor = Color.yellow;

            GUILayout.BeginVertical("box");

            GUILayout.Label("These option are here because they are optional or can cause bug.", InfoStyle);
            settings.CloseExtraOnExit = GUILayout.Toggle(settings.CloseExtraOnExit, "Close this when exit");
            GUILayout.BeginHorizontal();
            settings.TeargasEveryone = GUILayout.Toggle(settings.TeargasEveryone, "Teargas everyone");
            settings.SpawnBrondeFly = GUILayout.Toggle(settings.SpawnBrondeFly, "Spawn With Brondle Fly");
            settings.SkeletonDeadFace = GUILayout.Toggle(settings.SkeletonDeadFace, "Show skeleton Dead Face");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            settings.SuicideDontPanicWithDynamiteOnThem = GUILayout.Toggle(settings.SuicideDontPanicWithDynamiteOnThem, "Suicide terrorrist don't panic when they have a bomb on them");
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

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
        public bool UsePushingFrame;
        public bool UseNewLadderFrame;
        public bool SpawnWithExpendabros;
        public bool UseFifthBondSpecial;
        public bool ShowFacehuggerHUD;
        public bool TbagEnabled;
        public bool ChangeHeroUnlock;
        public bool PigAreAlwaysTerror;

        public bool UseCustomFramerate;
        public int MaxFramerate;

        public bool ShowExtra;
        // Extra
        public bool TeargasEveryone;
        public bool SpawnBrondeFly;
        public bool SkeletonDeadFace;
        public bool CloseExtraOnExit;
        public bool SuicideDontPanicWithDynamiteOnThem;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }

    [HarmonyPatch(typeof(UnityModManager.UI), "OnDestroy")]
    static class UMM_OnDestroy_Patch
    {
        static void Prefix()
        {
            QualitySettings.vSyncCount = 1;
            PlayerOptions.Instance.vSync = true;
            Main.Log("Enable vSync");
        }
    }
}
