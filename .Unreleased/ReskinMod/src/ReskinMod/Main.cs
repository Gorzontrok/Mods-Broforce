using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;
using RocketLib0;

namespace ReskinMod
{
    static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        internal static BroforceMod bmod;

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

            bmod = new BroforceMod(mod, _UseLocalLog: false);
            Start();
            return true;
        }

        static void Start()
        {
            List<string> FolderList = new List<string>();
            Utility.Mook_Folder = mod.Path + "Assets\\Mook\\";
            Utility.Bro_Folder = mod.Path + "Assets\\Bro\\";
            Utility.HUD_Folder = mod.Path + "Assets\\Interface\\HUD\\";
            Utility.Interface_Folder = mod.Path + "Assets\\Interface\\";
            Utility.Other_Character_Folder = mod.Path + "Assets\\Other_Character\\";

            FolderList.AddRange(new string[] { Utility.Mook_Folder, Utility.Bro_Folder, Utility.Interface_Folder, Utility.HUD_Folder, Utility.Other_Character_Folder });
            foreach (string folder in FolderList)
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
            }
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Debug = GUILayout.Toggle(bmod.UseDebugLog, "Debug log");
            bmod.UseDebugLog = settings.Debug;
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
    }

    public class Settings : UnityModManager.ModSettings
    {
        public bool Debug;
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
