using System;
using System.Reflection;
using System.Collections.Generic;
using HarmonyLib;
using UnityModManagerNet;

namespace DresserMod
{
    static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        public static bool CanPatch
        {
            get
            {
                return Main.enabled;
            }
        }

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
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

            Start();
            return true;
        }

        static void Start()
        {
            StorageRoom.Init();
            ModUI.Initialize();
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            try
            {
                ModUI.MainGUI();
            }
            catch(Exception ex)
            {
                Main.Log(ex.ToString());
            }
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

        public static void Log(object msg)
        {
            mod.Logger.Log(msg.ToString());
        }
    }

    public class Settings : UnityModManager.ModSettings
    {
        public bool canUseDefaultSkin;
        public List<string> unactiveFiles = new List<string>();
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
