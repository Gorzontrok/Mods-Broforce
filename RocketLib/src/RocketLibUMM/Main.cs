using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityModManagerNet;
using HarmonyLib;
using RocketLib;
using RMain = RocketLib.Main;
using RocketLib.Loggers;

namespace RocketLibUMM
{
    public class Main
    {
        public static bool enabled;
        public static Harmony harmony;
        public static UnityModManager.ModEntry mod;
        public static Settings settings;

        internal static BroforceMod bmod;

        public static void ExceptionLog(object str)
        {
            bmod.logger.ExceptionLog(str);
        }
        public static void ExceptionLog(Exception exception)
        {
            bmod.logger.ExceptionLog(exception);
        }
        public static void ExceptionLog(object str, Exception exception)
        {
            bmod.logger.ExceptionLog(str.ToString(), exception);
        }

        public static void Log(object str, RLogType type = RLogType.Log)
        {
            bmod.logger.Log(str, type);
        }

        public static void LogUMM(object str)
        {
            mod.Logger.Log(str.ToString());
        }

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                mod = modEntry;

                modEntry.OnToggle = OnToggle;
                modEntry.OnGUI = ModUI.OnGui;
                modEntry.OnSaveGUI = OnSaveGUI;
                modEntry.OnUpdate = OnUpdate;
                modEntry.CustomRequirements = MakeUSAColorOnBroforce();

                settings = Settings.Load<Settings>(modEntry);
                ScreenLogger.fontSize = settings.fontSize;

                harmony = new Harmony(modEntry.Info.Id);
                try
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    harmony.PatchAll(assembly);
                }
                catch (Exception ex)
                {
                    Main.LogUMM("Failed to Patch Harmony :\n" + ex);
                }

                try
                {
                    RMain.Load(mod);
                    RMain.showManagerLog = settings.showManagerLog;
                    RMain.showLogOnScreen = settings.onScreenLog;
                    RMain.logTimer = settings.logTimer;

                    bmod = new BroforceMod();
                    bmod.Load(mod);

                }
                catch (Exception ex)
                {
                    Main.LogUMM("Error while Loading RocketLib  :\n" + ex.ToString());
                }


                try
                {
                    Mod.Load();
                }
                catch (Exception e)
                {
                    Main.LogUMM(e.ToString());
                }

                return true;
            }
            catch(Exception ex)
            {
                Main.LogUMM(ex);
            }
            return false;
        }

        static string MakeUSAColorOnBroforce()
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
            return CustomRequirements;
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.onScreenLog = RMain.showLogOnScreen;
            settings.showManagerLog = RMain.showManagerLog;
            settings.logTimer = RMain.logTimer;
            settings.Save(modEntry);
        }

        static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            if (!LevelEditorGUI.IsActive) ShowMouseController.ShowMouse = false;
            Cursor.lockState = CursorLockMode.None;

            Mod.Update();
        }
        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }
    }

    public class Settings : UnityModManager.ModSettings
    {
        // ScreenLogger Option
        public bool onScreenLog = false;
        public bool showManagerLog = true;
        public float logTimer = 3;
        public int fontSize = 13;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Mod.save.Save();
            Save(this, modEntry);
        }
    }

    // Patch to don't destroy the mod manager. Need to find the one of Cutscene.
    [HarmonyPatch(typeof(PauseMenu), "ReturnToMenu")]
    static class PauseMenu_ReturnToMenu_Patch
    {
        static bool Prefix(PauseMenu __instance)
        {
            if (!Main.enabled)
            {
                return true;
            }

            PauseGameConfirmationPopup m_ConfirmationPopup = __instance.GetFieldValue<PauseGameConfirmationPopup>("m_ConfirmationPopup");

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

