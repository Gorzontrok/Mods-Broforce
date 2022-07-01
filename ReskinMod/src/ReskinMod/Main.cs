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
        public static Settings settings;

        public static bool CantPatch
        {
            get
            {
                return !Main.enabled || Map.isEditing;
            }
        }
        public static float actualVillagerProb;

        public static string assetsFolderPath;
        internal static BroforceMod bmod;

        private static SkinCollection _selectedSkinCollection;
        private static Vector2 _scrollViewVector;
        private static Vector2 _scrollViewVectorUnused;
        private static Vector2 _scrollViewVectorUnused2;
        private static int _selectedTab;
        private static string[] _tabs;

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

            SkinCollectionController.Init();
            if(SkinCollectionController.skinCollections.Count>0)
            {
                _selectedSkinCollection = SkinCollectionController.skinCollections[0];
            }
            _tabs = new string[] { "Main", "Conflicts", "Settings"};
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            if(GUILayout.Button(new GUIContent("Reload Mod"), GUILayout.Width(200)))
            {
                SkinCollectionController.Init();
            }

            GUILayout.Space(15);

            _selectedTab = GUILayout.SelectionGrid(_selectedTab, _tabs, _tabs.Length, GUILayout.Width(1000));

            if(_selectedTab == 0)
            {
                MainGUI();
            }
            else if(_selectedTab == 1)
            {
                ConflictGUI();
            }
            else if(_selectedTab == 2)
            {
                SettingGUI();
            }
        }

        private static void MainGUI()
        {
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical("Skins' Name", GUI.skin.box, GUILayout.Width(200), GUILayout.Height(250));
            GUILayout.Space(15);
            _scrollViewVector = GUILayout.BeginScrollView(_scrollViewVector, GUILayout.Height(250));
            foreach (SkinCollection skinCollection in SkinCollectionController.skinCollections)
            {
                if (GUILayout.Button(skinCollection.name))
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
            if (!skinCollectionNull)
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

        private static void ConflictGUI()
        {
            GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(500), GUILayout.Height(250));
            GUILayout.Space(5);
            _scrollViewVectorUnused2 = GUILayout.BeginScrollView(_scrollViewVectorUnused2, GUILayout.Height(250));
            if (SkinCollectionController.conflictsAndErrors.Count > 0)
            {
                foreach (string msg in SkinCollectionController.conflictsAndErrors)
                {
                    GUILayout.Label(msg);
                    GUILayout.Space(7);
                }
            }
            else
            {
                GUILayout.Label("No conflicts or errors");
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private static void SettingGUI()
        {
            GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(1000), GUILayout.Height(250));
            _scrollViewVectorUnused2 = GUILayout.BeginScrollView(_scrollViewVectorUnused2, GUILayout.Height(250));

            settings.citizenVillagerCanHaveDefaultSkin = GUILayout.Toggle(settings.citizenVillagerCanHaveDefaultSkin, "Citizen and Villager can have default skins");
            GUILayout.BeginHorizontal();
            GUILayout.Label("Probability :");
            int.TryParse(GUILayout.TextField(settings.citizenVillagerCanHaveDefaultSkinProb.ToString(), GUILayout.Width(150)), out settings.citizenVillagerCanHaveDefaultSkinProb);
            GUILayout.Label("%");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
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

        public static void ErrorLog(object msg)
        {
            bmod.logger.ErrorLog(msg);
        }

        public static void WarningLog(object msg)
        {
            bmod.logger.WarningLog(msg);
        }

        private static float FloatBarWithEnter(float value)
        {
            value = GUILayout.HorizontalScrollbar(value, 5, 0, 1);
            return value;
        }
    }

    public class Settings : UnityModManager.ModSettings
    {
        public float citizenVillagerProb
        {
            get
            {
                return citizenVillagerCanHaveDefaultSkinProb / 100;
            }
        }
        public bool citizenVillagerCanHaveDefaultSkin;
        public int citizenVillagerCanHaveDefaultSkinProb;
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
