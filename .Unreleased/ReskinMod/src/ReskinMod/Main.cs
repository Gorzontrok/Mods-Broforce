using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;
using RocketLib0;

using ReskinMod.Skins;

namespace ReskinMod
{
    static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
       // public static Settings settings;

        public static string assetsFolderPath;
        internal static BroforceMod bmod;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnGUI = OnGUI;
            //modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            //settings = Settings.Load<Settings>(modEntry);
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

            bmod = new BroforceMod(mod);
            Start();
            return true;
        }

        static void Start()
        {
            /*List<string> FolderList = new List<string>();
            Utility.Mook_Folder = mod.Path + "Assets\\Mook\\";
            Utility.Bro_Folder = mod.Path + "Assets\\Bro\\";
            Utility.HUD_Folder = mod.Path + "Assets\\Interface\\HUD\\";
            Utility.Interface_Folder = mod.Path + "Assets\\Interface\\";
            Utility.Other_Character_Folder = mod.Path + "Assets\\Other_Character\\";

            FolderList.AddRange(new string[] { Utility.Mook_Folder, Utility.Bro_Folder, Utility.Interface_Folder, Utility.HUD_Folder, Utility.Other_Character_Folder });*/
            /* foreach (string folder in FolderList)
             {
             }*/
            assetsFolderPath = Path.Combine(mod.Path, "assets");
            if (!Directory.Exists(assetsFolderPath))
            {
                Directory.CreateDirectory(assetsFolderPath);
            }
            SkinCollection.Init();
            /*foreach(SkinCollection s in SkinCollection.skinCollections)
            {
                Main.bmod.Log(s.name + " " + s.skins.Count);
                foreach(Skin skin in s.skins)
                {
                    Main.bmod.Log("\t"+skin);
                }
            }*/
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            if(GUILayout.Button(new GUIContent("Reload Mod"), GUILayout.Width(200)))
            {
                SkinCollection.Init();
            }
        }

       /*static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }*/

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }

        public static void ErrorLog(object msg)
        {
            bmod.logger.ErrorLog(msg);
        }

        public static void WarningLog(object msg)
        {
            bmod.logger.WarningLog(msg);
        }
    }

   /* public class Settings : UnityModManager.ModSettings
    {
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }*/
}
