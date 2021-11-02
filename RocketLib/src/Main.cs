using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityModManagerNet;
using HarmonyLib;
using RocketLib0;


namespace RocketLibLoadMod
{
    /// <summary>
    /// You don't need it. Anyway you will just have the function of the UnityModManager.
    /// </summary>
    public class Main
    {
        /// <summary>
        /// </summary>
        public static UnityModManager.ModEntry mod;
        /// <summary>
        /// </summary>
        public static bool enabled;
        /// <summary>
        /// </summary>
        public static Settings settings;

        internal static BroforceMod bmod;

        internal static List<BroforceMod> BMOD_List = new List<BroforceMod>();

        private static int TabSelected = 0;

        private static GUIStyle LogStyle = new GUIStyle();

        internal static Harmony harmony;
        static bool Load(UnityModManager.ModEntry modEntry)
        {

            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGui;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnUpdate = OnUpdate;

            settings = Settings.Load<Settings>(modEntry);

            mod = modEntry;

            harmony = new Harmony(modEntry.Info.Id);
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);
            }
            catch (Exception ex)
            {
                Main.Log("Failed to Patch Harmony :\n" + ex);
            }

            try
            {
                RocketLib.Load();
                bmod = new BroforceMod(mod);
            }
            catch (Exception ex)
            {
                Main.Log("Error while Loading RocketLib :\n" + ex.ToString());
            }

            FirstLaunch();

            return true;
        }

        static void OnGui(UnityModManager.ModEntry modEntry)
        {
            GUIStyle testBtnStyle = new GUIStyle("button");
            testBtnStyle.normal.textColor = Color.yellow;
            /*if (GUILayout.Button("TEST", testBtnStyle, new GUILayoutOption[] { GUILayout.Width(150)}))
              {
                  try
                  {
                    bmod.Log("test123");
                  }
                  catch (Exception ex)
                  {
                      bmod.Log(ex, RLogType.Exception);

                  }
              }*/

            var TabStyle = new GUIStyle("button");

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Main", TabStyle, GUILayout.Width(100))) TabSelected = 0;
            GUILayout.Space(20);
            if (settings.OnScreenLog)
            {
                if (GUILayout.Button("Screen Logger", TabStyle, GUILayout.Width(110))) TabSelected = 1;
                GUILayout.Space(20);
            }
            if (GUILayout.Button("Log", TabStyle, GUILayout.Width(100))) TabSelected = -1;
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            if (TabSelected == 0)
            {
                GUILayout.Space(30);
                MainGUI();
            }
            else if (TabSelected == 1 && settings.OnScreenLog)
            {
                GUILayout.Space(30);
                ScreenLoggerGUI();
            }
            else if (TabSelected == -1)
            {
                GUILayout.Space(30);
                LogGUI();
            }
        }

        private static void MainGUI()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Show current unlock intervals")) RocketLib._HeroUnlockController.ShowHeroUnlockIntervals();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            settings.OnScreenLog = GUILayout.Toggle(settings.OnScreenLog, "Enable OnScreenLog");
        }

        private static void ScreenLoggerGUI()
        {
            Rect ToolTipRect = GUILayoutUtility.GetLastRect();
            GUILayout.BeginVertical("box");

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear Log on screen", GUILayout.Width(150)))
            {
                RocketLib.ScreenLogger.Clear();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            bmod.UseDebugLog = GUILayout.Toggle(bmod.UseDebugLog, "Enable Debug log");
            settings.ShowManagerLog = GUILayout.Toggle(settings.ShowManagerLog, new GUIContent("Show Unity Mod Manager Log"));

            GUILayout.BeginHorizontal();
            GUILayout.Label("Time before log disapear : " + settings.logTimer.ToString(), GUILayout.ExpandWidth(false));
            settings.logTimer = (int)GUILayout.HorizontalScrollbar(settings.logTimer, 1f, 1f, 11f, GUILayout.MaxWidth(200));
            GUILayout.EndHorizontal();

            GUI.Label(ToolTipRect, GUI.tooltip);
            GUILayout.EndVertical();
        }


        private static Vector2 scrollViewVector;
        private static void LogGUI()
        {
            GUILayout.BeginVertical("box");
            scrollViewVector = GUILayout.BeginScrollView(scrollViewVector, GUILayout.Height(250));
            foreach (string log in RocketLib.ScreenLogger.FullLogList)
            {
                LogStyle.normal.textColor = WhichColor(log);
                GUILayout.Label(log, LogStyle);
                GUILayout.Space(5);
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }

        static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            if (!LevelEditorGUI.IsActive) ShowMouseController.ShowMouse = false;
            Cursor.lockState = CursorLockMode.None;

            foreach(BroforceMod mod in BMOD_List)
            {
                mod.OnUpdate();
            }
        }
        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }

        private static Color WhichColor(string LogMsg)
        {
            LogMsg = LogMsg.ToLower();
            if (LogMsg.Contains("error") || LogMsg.Contains("exception"))
            {
                return Color.red;
            }
            else if (LogMsg.Contains("warning"))
            {
                return Color.yellow;
            }
            else if (LogMsg.Contains("[information]"))
            {
                return Color.blue;
            }
            else if (LogMsg.Contains("successful loaded"))
            {
                return Color.green;
            }
            else
            {
                return Color.white;
            }
        }

        internal static void Log(object str)
        {
            mod.Logger.Log(str.ToString());
        }

        static void FirstLaunch()
        {
            if (!settings.HaveItFirstLaunch)
            {
                settings.logTimer = 3;
                settings.OnScreenLog = true;
                settings.ShowManagerLog = true;

                settings.HaveItFirstLaunch = true;
                bmod.InformationLog("Finish Rocketlib first launch.");
                OnSaveGUI(mod);
            }
        }
    }

    /// <summary>
    /// Really, you don't need it. Anyway you will just have the function of the UnityModManager.
    /// </summary>
    public class Settings : UnityModManager.ModSettings
    {
        // ScreenLogger Option
        /// <summary>
        /// </summary>
        public bool OnScreenLog;
        /// <summary>
        /// </summary>
        public bool ShowScreenLogOption;
        /// <summary>
        /// </summary>
        public bool ShowManagerLog;
        /// <summary>
        /// </summary>
        public bool HaveItFirstLaunch;
        /// <summary>
        /// </summary>
        public int logTimer;

        /// <summary>
        /// </summary>
        /// <param name="modEntry"></param>
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }

    // Patch for don't destroy Mod. Need to find the one of Cutscene.
    [HarmonyPatch(typeof(PauseMenu), "ReturnToMenu")]
    static class PauseMenu_ReturnToMenu_Patch
    {
        static bool Prefix(PauseMenu __instance)
        {
            if (!Main.enabled)
            {
                return true;
            }

            PauseGameConfirmationPopup m_ConfirmationPopup = (Traverse.Create(__instance).Field("m_ConfirmationPopup").GetValue() as PauseGameConfirmationPopup);

            MethodInfo dynMethod = m_ConfirmationPopup.GetType().GetMethod("ConfirmReturnToMenu", BindingFlags.NonPublic | BindingFlags.Instance);
            dynMethod.Invoke(m_ConfirmationPopup, null);

            return false;
        }

    }
    [HarmonyPatch(typeof(PauseMenu), "ReturnToMap")]
    static class PauseMenu_ReturnToMap_Patch
    {
        static bool Prefix(PauseMenu __instance)
        {
            if (!Main.enabled)
            {
                return true;
            }

            __instance.CloseMenu();
            GameModeController.Instance.ReturnToWorldMap();
            return false;
        }

    }
    [HarmonyPatch(typeof(PauseMenu), "RestartLevel")]
    static class PauseMenu_RestartLevel_Patch
    {
        static bool Prefix(PauseMenu __instance)
        {
            if (!Main.enabled)
            {
                return true;
            }

            Map.ClearSuperCheckpointStatus();

            (Traverse.Create(typeof(TriggerManager)).Field("alreadyTriggeredTriggerOnceTriggers").GetValue() as List<string>).Clear();

            if (GameModeController.publishRun)
            {
                GameModeController.publishRun = false;
                LevelEditorGUI.levelEditorActive = true;
            }
            PauseController.SetPause(PauseStatus.UnPaused);
            GameModeController.RestartLevel();

            return false;
        }
    }
}

