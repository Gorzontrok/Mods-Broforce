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

        private static SkinCollection _selectedSkinCollection;
        private static Vector2 _scrollViewVector;
        private static Vector2 _scrollViewVectorUnused;

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
            assetsFolderPath = Path.Combine(mod.Path, "assets");
            if (!Directory.Exists(assetsFolderPath))
            {
                Directory.CreateDirectory(assetsFolderPath);
            }

            SkinCollection.Init();
            if(SkinCollection.skinCollections.Count>0)
            {
                _selectedSkinCollection = SkinCollection.skinCollections[0];
            }
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            if(GUILayout.Button(new GUIContent("Reload Mod"), GUILayout.Width(200)))
            {
                SkinCollection.Init();
            }

            GUILayout.Space(15);
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical("Skins' Name", GUI.skin.box, GUILayout.Width(200), GUILayout.Height(250));
            GUILayout.Space(15);
            _scrollViewVector = GUILayout.BeginScrollView(_scrollViewVector, GUILayout.Height(250));
            foreach(SkinCollection skinCollection in SkinCollection.skinCollections)
            {
                if(GUILayout.Button(skinCollection.name))
                {
                    _selectedSkinCollection = skinCollection;
                }
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            GUILayout.Space(30);
            bool skinCollectionNull = _selectedSkinCollection == null;
            GUILayout.BeginVertical(skinCollectionNull ? "" : _selectedSkinCollection.name, GUI.skin.box, GUILayout.Width(500), GUILayout.Height(250));
            GUILayout.Space(5);
            _scrollViewVectorUnused = GUILayout.BeginScrollView(_scrollViewVectorUnused, GUILayout.Height(250));
            if(!skinCollectionNull)
            {
                foreach (Skin skin in _selectedSkinCollection.skins)
                {
                    GUILayout.Label(skin.ToString());
                    GUILayout.Space(7);
                }
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

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
