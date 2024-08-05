using System;
using System.Reflection;
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
        internal static RLogger logger;

        static bool Load(UnityModManager.ModEntry modEntry)
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
            var assembly = Assembly.GetExecutingAssembly();
            harmony.PatchAll(assembly);

            logger = new RLogger();

            try
            {
                RocketLib.Main.logger = logger;

                RMain.Load(mod);
                RMain.showManagerLog = settings.showManagerLog;
                RMain.showLogOnScreen = settings.onScreenLog;
                RMain.logTimer = settings.logTimer;

                bmod = new BroforceMod();
                bmod.Load(mod);
            }
            catch (Exception ex)
            {
                logger.Exception("Error while Loading RocketLib:", ex);
            }

            try
            {
                Mod.Load();
            }
            catch (Exception e)
            {
                logger.Exception(e);
            }
            return true;
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
            if (!LevelEditorGUI.IsActive)
                ShowMouseController.ShowMouse = false;
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

    internal class RLogger : RocketLib.ILogger
    {
        public void Log(object message)
        {
            Log(message.ToString());
        }

        public void Log(string message)
        {
            UnityModManager.Logger.Log(message, "[RocketLib] ");
        }

        public void Warning(string message)
        {
            UnityModManager.Logger.Log(message, "[RocketLib] [Warning] ");
        }

        public void Error(string message)
        {
            UnityModManager.Logger.Error(message, "[RocketLib] [Error] ");
        }

        public void Exception(Exception exception)
        {
            Exception(null, exception);
        }
        public void Exception(string message, Exception exception)
        {
            UnityModManager.Logger.LogException(message, exception, "[RocketLib] [Exception] ");
        }

        public void Debug(string message)
        {
            UnityModManager.Logger.Log(message, "[RocketLib] [Debug]");
        }
    }
}

