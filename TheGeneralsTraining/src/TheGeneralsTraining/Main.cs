using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityModManagerNet;

namespace TheGeneralsTraining
{
    public static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        public static bool CanUsePatch
        {
            get
            {
                if (Main.enabled)
                {
                    if (LevelEditorGUI.IsActive || Map.isEditing || LevelSelectionController.IsCustomCampaign())
                    {
                        return settings.patchInCustomsLevel;
                    }
                    return true;
                }
                return false;
            }
        }
        public static void ExceptionLog(Exception ex)
        {
            Log(ex);
        }
        public static void ExceptionLog(string s, Exception ex)
        {
            Log(s + "\n" + ex);
        }
        public static void Log(object str)
        {
            mod.Logger.Log(str.ToString());
        }


        private static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            modEntry.OnUpdate = OnUpdate;

            settings = Settings.Load<Settings>(modEntry);

            mod = modEntry;

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
            GUI_UMM.Init();

            return true;
        }

        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUI_UMM.OnGUI();
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

        private static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
        }

    }
}
