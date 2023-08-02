using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;
using RocketLib;

namespace CustomHeroName
{
    static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;

        public static bool shouldIgnorePatch = false;
        public static HeroType currentHero;

        static bool Load(UnityModManager.ModEntry modEntry)
        {

            mod = modEntry;

            modEntry.OnGUI = OnGUI;
            modEntry.OnToggle = OnToggle;

            try
            {
                var harmony = new Harmony(modEntry.Info.Id);
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);
            }
            catch (Exception ex)
            {
                mod.Logger.Log("Failed to patch with Harmony !\n" + ex.ToString());
            }

            try
            {
                Mod.CheckFile();
            }
            catch (Exception e)
            {
                Main.Log(e);
            }
            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            try
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Recreate File", GUILayout.ExpandWidth(false)))
                {
                    Mod.CreateFile();
                }
                if (GUILayout.Button("Reload File", GUILayout.ExpandWidth(false)))
                {
                    Mod.Deserialize();
                }
                GUILayout.EndHorizontal();
            }
            catch(Exception e)
            {
                Log(e);
            }
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
}
