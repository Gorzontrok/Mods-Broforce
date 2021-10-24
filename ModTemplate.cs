// I use this for copy/paste it in my next mod and save me time. You can use it too.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace ModTemplate
{
    static class Main
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

            settings = Settings.Load<Settings>(modEntry);
            
            mod = modEntry;
            
            var harmony = new Harmony(modEntry.Info.Id);
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);
            }
            catch(Exception ex)
            {
                mod.Logger.Log("Failed to Patch Harmony !\n"+ex.ToString());
            }
            
            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
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
        public static void Log(IEnumerable<object> str)
        {
            mod.Logger.Log(str.ToString());
        }

    }

    public class Settings : UnityModManager.ModSettings
    {

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

    }
}
