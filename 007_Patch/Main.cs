using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace DoubleBroSevenTrained
{
    internal static class Main
    {
        internal static UnityModManager.ModEntry mod;
        internal static bool enabled;
        internal static Settings settings;
        internal static Mod bfMod;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            settings = Settings.Load<Settings>(modEntry);
            mod = modEntry;
            try
            {
                var harmony = new Harmony(modEntry.Info.Id);
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);
            }
            catch (Exception ex)
            {
                mod.Logger.Log(ex.ToString());
            }

            bfMod = new Mod();

            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Draw(modEntry);

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset Vanilla Configs to default", GUILayout.ExpandWidth(false)))
                settings.vanillaConfigs = new VanillaConfigs();
            if (GUILayout.Button("Reset Mod Configs to default", GUILayout.ExpandWidth(false)))
                settings.modConfigs = new ModConfigs();
            GUILayout.EndHorizontal();
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }

        public static void Log(object str)
        {
            mod.Logger.Log(str.ToString());
        }
    }
}
