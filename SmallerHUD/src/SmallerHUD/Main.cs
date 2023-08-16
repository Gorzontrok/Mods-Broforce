using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityModManagerNet;

namespace SmallerHUD
{
    internal static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            modEntry.OnUpdate = OnUpdate;
            mod = modEntry;
            settings = Settings.Load<Settings>(modEntry);

            try
            {
                var harmony = new Harmony(modEntry.Info.Id);
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);
            }
            catch (Exception ex)
            {
                mod.Logger.Log("Failed to Patch Harmony\n" + ex.ToString());
            }

            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            ModUI.OnGUI();
        }

        static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
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
        public int scaleLevel = 5;
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
