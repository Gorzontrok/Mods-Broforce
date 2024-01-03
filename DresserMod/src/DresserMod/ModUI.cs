using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DresserMod
{
    internal static class ModUI
    {
        private static Settings Settings
        {
            get { return Main.settings; }
        }

        private static Vector2 _scrollViewVectorWardrobes = Vector2.zero;
        private static Vector2 _scrollViewVectorAttires = Vector2.zero;
        private static Vector2 _scrollViewVectorClothes = Vector2.zero;

        private static int _selectedTab = 0;
        private static string[] _tabs;
        private static Action[] _tabsAction;

        private static Wardrobe _selectedWardrobe = null;
        private static Attire _selectedAttire = null;

        private static bool _wardrobeError = false;
        private static bool _attireError = false;
        private static bool _clothesError = false;

        private static string _fileName = string.Empty;

        public static void Initialize()
        {
            _tabs = new string[] { "Storage Browser", "Settings" };
            _tabsAction = new Action[] { StorageBrowser, SettingGUI };
            if(StorageRoom.wardrobes.Count > 0)
            {
                _selectedWardrobe = StorageRoom.wardrobes.First().Value;
                _selectedAttire = _selectedWardrobe[0];
            }
            _wardrobeError = false;
            _attireError = false;
            _clothesError = false;
        }

        public static void MainGUI()
        {
            if (GUILayout.Button(new GUIContent("Reload Mod"), GUILayout.Width(200)))
            {
                StorageRoom.Init();
                Initialize();
            }

            GUILayout.Space(15);
            if(_selectedWardrobe == null)
            {
                GUILayout.Label(new GUIContent("No skin in assets folder."));
                return;
            }
            _selectedTab = GUILayout.SelectionGrid(_selectedTab, _tabs, 6, GUILayout.Width(1000));

            GUILayout.BeginHorizontal();
            _tabsAction[_selectedTab].Invoke();
            GUILayout.EndHorizontal();
        }

        private static void StorageBrowser()
        {

            try
            {
                if (_wardrobeError) return;
                // Wardrobes
                GUILayout.BeginVertical("Wardrobes", GUI.skin.box, GUILayout.Width(250), GUILayout.Height(200));
                GUILayout.Space(15);
                _scrollViewVectorWardrobes = GUILayout.BeginScrollView(_scrollViewVectorWardrobes, GUILayout.Height(200));
                foreach (KeyValuePair<string, Wardrobe> pair in StorageRoom.wardrobes)
                {
                    if (GUILayout.Button(pair.Key))
                    {
                        _selectedWardrobe = pair.Value;
                        _selectedAttire = _selectedWardrobe[0];
                    }
                }
                GUILayout.EndScrollView();
                GUILayout.EndVertical();
            }
            catch(Exception e)
            {
                Main.Log("Wardrobes\n"+e);
                _wardrobeError = true;
            }

            try
            {
                if (_attireError || _selectedWardrobe == null) return;
                // Attires
                GUILayout.Space(30);
                GUILayout.BeginVertical(_selectedWardrobe.wearers, GUI.skin.box, GUILayout.Width(200), GUILayout.Height(200));
                GUILayout.Space(15);
                _scrollViewVectorAttires = GUILayout.BeginScrollView(_scrollViewVectorAttires, GUILayout.Height(200));
                foreach (Attire attire in _selectedWardrobe.attires)
                {
                    if (GUILayout.Button(attire.name))
                    {
                        _selectedAttire = attire;
                    }
                }
                GUILayout.EndScrollView();
                GUILayout.EndVertical();
            }
            catch (Exception e)
            {
                Main.Log("Attires\n" + e);
                _attireError = true;
            }

            try
            {
                if (_clothesError || _selectedAttire == null) return;
                // Clothes
                GUILayout.Space(30);
                GUILayout.BeginVertical(_selectedAttire.name, GUI.skin.box, GUILayout.Width(500), GUILayout.Height(200));
                GUILayout.Space(15);
                GUILayout.Label("Path: \n" + _selectedAttire.directory);
                GUILayout.Space(10);
                _scrollViewVectorClothes = GUILayout.BeginScrollView(_scrollViewVectorClothes, GUILayout.Height(200));
                foreach (KeyValuePair<string, string> pair in _selectedAttire.clothes)
                {
                    GUILayout.Label($"\"{pair.Key}\": \"{pair.Value}\"");
                }
                GUILayout.EndScrollView();
                GUILayout.EndVertical();
            }
            catch( Exception e)
            {
                Main.Log("Clothes\n"+e);
                _clothesError = true;
            }
        }


        private static void SettingGUI()
        {
            GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(1000), GUILayout.Height(100));

            Settings.canUseDefaultSkin = GUILayout.Toggle(Settings.canUseDefaultSkin, "Can have default skin");

            GUILayout.BeginHorizontal();
            _fileName = GUILayout.TextField(_fileName, GUILayout.Width(200), GUILayout.ExpandWidth(false));
            if(GUILayout.Button("Create JSON File", GUILayout.Width(200)))
            {
                StorageRoom.CreateJsonFile(_fileName, StorageRoom.WardrobesDirectory);
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
    }
}
