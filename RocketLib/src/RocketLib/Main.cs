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

        private static int TabSelected = 0;

        private static GUIStyle LogStyle = new GUIStyle();

        internal static Harmony harmony;
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            string origCustomRequirements = "Broforce";
            string CustomRequirements = string.Empty;
            for (int i = 0; i < 3; i++)
            {
                if (i == 0)
                {
                    CustomRequirements = "<color=\"#1C59FE\">";
                }
                CustomRequirements += origCustomRequirements[i];
                if (i == 2)
                {
                    CustomRequirements += "</color>";
                }
            }
            for (int i = 3; i < origCustomRequirements.Length; i++)
            {
                if (i % 2 == 0)
                {
                    CustomRequirements += "<color=\"red\">" + origCustomRequirements[i] + "</color>";
                }
                else
                {
                    CustomRequirements += "<color=\"white\">" + origCustomRequirements[i] + "</color>";
                }
            }
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGui;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnUpdate = OnUpdate;
            //modEntry.CustomRequirements = "<color=\"#1C59FE\">The</color><color=\"red\"> G</color><color=\"white\">a</color><color=\"red\">m</color><color=\"white\">e</color>";
            modEntry.CustomRequirements = CustomRequirements;

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
                bmod = new BroforceMod();
                bmod.Load(mod);
            }
            catch (Exception ex)
            {
                Main.Log("Error while Loading RocketLib :\n" + ex.ToString());
            }

            FirstLaunch();

            return true;
        }

        internal static bool GorzonBuild
        {
            get
            {
                return Environment.UserName == "Gorzon";
            }
        }

        private static string sceneStr;
        static void OnGui(UnityModManager.ModEntry modEntry)
        {
            GUIStyle testBtnStyle = new GUIStyle("button");
            testBtnStyle.normal.textColor = Color.yellow;

            if (GorzonBuild && GUILayout.Button("TEST", testBtnStyle, new GUILayoutOption[] { GUILayout.Width(150) }))
            {
                try
                {
                    bmod.logger.Log("TEST");
                }
                catch (Exception ex)
                {
                    bmod.logger.Log(ex, RLogType.Exception);

                }
            }
            string[] tabs = new string[] { "<color=\"yellow\">Main</color>", "Screen Logger", "Scene Loader", "Log" };
            TabSelected = RocketLib.RGUI.Tab(tabs, TabSelected, 10, 110);

            if (TabSelected == 0)
            {
                GUILayout.Space(30);
                MainGUI();
            }
            else if (TabSelected == 1)
            {
                GUILayout.Space(30);
                ScreenLoggerGUI();
            }
            else if (TabSelected == 2)
            {
                GUILayout.Space(30);
                LoadSceneGUI();
            }
            else if (TabSelected == tabs.Length - 1)
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
                ScreenLogger.Instance.Clear();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            bmod.logger.UseDebugLog = GUILayout.Toggle(bmod.logger.UseDebugLog, "Enable Debug log");
            settings.ShowManagerLog = GUILayout.Toggle(settings.ShowManagerLog, new GUIContent("Show Unity Mod Manager Log"));

            GUILayout.BeginHorizontal();
            GUILayout.Label("Time before log disappear : " + settings.logTimer.ToString(), GUILayout.ExpandWidth(false));
            settings.logTimer = (int)GUILayout.HorizontalScrollbar(settings.logTimer, 1f, 1f, 11f, GUILayout.MaxWidth(200));
            GUILayout.EndHorizontal();

            GUI.Label(ToolTipRect, GUI.tooltip);
            GUILayout.EndVertical();
        }

        static void LoadSceneGUI()
        {
            GUILayout.BeginHorizontal();
            sceneStr = GUILayout.TextField(sceneStr, GUILayout.Width(200));
            GUILayout.Space(10);
            if (GUILayout.Button("Load Scene", new GUILayoutOption[] { GUILayout.Width(150) }))
            {
                try
                {
                    Utility.SceneLoader.LoadScene(sceneStr);
                }
                catch (Exception ex)
                {
                    bmod.logger.Log(ex, RLogType.Exception);

                }
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            if (GUILayout.Button("Load Main Menu", new GUILayoutOption[] { GUILayout.Width(150) }))
            {
                try
                {
                    Utility.SceneLoader.LoadScene("MainMenu");
                }
                catch (Exception ex)
                {
                    bmod.logger.Log(ex, RLogType.Exception);

                }
            }
        }


        private static Vector2 scrollViewVector;
        private static void LogGUI()
        {
            GUILayout.BeginVertical("box");
            scrollViewVector = GUILayout.BeginScrollView(scrollViewVector, GUILayout.Height(250));
            foreach (string log in ScreenLogger.Instance.FullLogList)
            {
                LogStyle.normal.textColor = ScreenLogger.WhichColor(log);
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

            foreach (BroforceMod mod in BroforceModController.Get_BroforceModList())
            {
                mod.OnUpdate();
            }
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

        static void FirstLaunch()
        {
            if (!settings.HaveItFirstLaunch)
            {
                settings.logTimer = 3;
                settings.OnScreenLog = true;
                settings.ShowManagerLog = true;

                settings.HaveItFirstLaunch = true;
                bmod.logger.InformationLog("Finish RocketLib first launch.");
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

