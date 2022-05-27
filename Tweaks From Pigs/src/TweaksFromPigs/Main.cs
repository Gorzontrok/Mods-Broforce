using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;
using RocketLib0;

namespace TweaksFromPigs
{
    public static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        public static BroforceMod bmod;

        public static Dictionary<int, HeroType> heroDictionary = new Dictionary<int, HeroType>();
        public static bool origDicIsSet = true;
        public static bool needReload = false;

        public static string assetsFolder;

        public static Harmony harmony;

        public static string[] arcadeCampaigns = new string[] { "Normal", "Expendabros", "TWITCHCON", "Alien Demo", "Boss Rush", "Only Hell" };

        private static GUIStyle reloadStyle = new GUIStyle();

        public static string CurrentArcade
        {
            get
            {
                return arcadeCampaigns[settings.arcadeIndex];
            }
        }

        public static bool GorzonBuild
        {
            get
            {
                return Environment.UserName == "Gorzon";
            }
        }

        public static bool cheat;
        private static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            modEntry.OnUpdate = OnUpdate;
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
                mod.Logger.Log("Failed to Patch Harmony :\n" + ex.ToString());
            }

            try
            {
                bmod = new BroforceMod();
                bmod.Load(mod);
            }
            catch (Exception ex)
            {
                mod.Logger.Log("Failed create BroforceMod :\n" + ex);
            }

            try
            {
                Start();
            }
            catch (Exception ex)
            {
                mod.Logger.Log(ex.ToString());
            }

            if (!settings.firstLaunch) FirstLaunch();

            return true;
        }

        private static void Start()
        {
            UMM_GUI.Init();

            reloadStyle = new GUIStyle();
            reloadStyle.fontSize = 15;
            reloadStyle.normal.textColor = Color.yellow;

            // Set Mouse of the game
            if (settings.setCustomMouse)
            {
                ShowMouseController.SetCursorToArrow(true);
                settings.customMouseIsSet = true;
                bmod.logger.InformationLog("Custom mouse is set.", true);
            }
            else settings.customMouseIsSet = false;

            assetsFolder = Path.Combine(mod.Path, "Assets/");

            cheat = GorzonBuild;

            new GamePassword("drunkpigs", drunkpigs);
        }
        private static void drunkpigs()
        {
            cheat = true;
        }

        private static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            if (!Main.enabled) return;
            // Build Unlock Intervals
            heroDictionary = TFP_Utility.BuildHeroDictionary();

            // Set T-Bag
            TestVanDammeAnim.teaBagCheatEnabled = settings.tbagEnabled;

            //Set custom Frame rate
            if (settings.useCustomFramerate)
            {
                Application.targetFrameRate = settings.maxFramerate;
                QualitySettings.vSyncCount = 0;
            }
            else QualitySettings.vSyncCount = 1;

            needReload = TheseVarHaveChangeValue();

            // Custom Mouse change color
            if (settings.setCustomMouse)
            {
                if (!settings.customMouseIsSet)
                {
                    ShowMouseController.SetCursorToArrow(true);
                    settings.customMouseIsSet = true;
                }
                if (Input.GetMouseButton(0)) ShowMouseController.HilightCursor();
            }

            PlayerOptions.Instance.playerName = settings.customPlayerName;
        }

        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();
            if (needReload)
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("If you want change being applied, you need to reload !", reloadStyle);
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();

            UMM_GUI.GlobalGUI();
        }

        private static bool TheseVarHaveChangeValue()
        {
            if (!settings.setCustomMouse && settings.customMouseIsSet) return true;
            return false;
        }

        private static void FirstLaunch()
        {
            settings.maxFramerate = 60;
            settings.acidBarrelSpawnProbability = 0.2f;
            settings.zombieSpeedModifier = 1.2f;

            settings.firstLaunch = true;
            OnSaveGUI(mod);
        }
        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }
        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }

        public static void ExceptionLog(Exception ex)
        {
            Main.bmod.logger.Log("", ex);
        }

        public static void ExceptionLog(object obj, Exception ex)
        {
            Main.bmod.logger.Log(obj.ToString(), ex);
        }
    }
}
