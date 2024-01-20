using System;
using System.Collections.Generic;
using System.IO;
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
        private static IAttire _selectedAttire = null;

        private static bool _wardrobeError = false;
        private static bool _attireError = false;
        private static bool _clothesError = false;

        private static string _fileName = string.Empty;

        public static void Initialize()
        {
            _tabs = new string[] { "Storage Browser", "Settings" };
            _tabsAction = new Action[] { StorageBrowser, SettingGUI };
            if(StorageRoom.Wardrobes.Count > 0)
            {
                _selectedWardrobe = StorageRoom.Wardrobes.First().Value;
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
                StorageRoom.Initialize();
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

            // Wardrobes
            try
            {
                if (_wardrobeError)
                    return;
                GUILayout.BeginVertical("Wardrobes", GUI.skin.box, GUILayout.Width(300), GUILayout.Height(250));
                GUILayout.Space(15);
                _scrollViewVectorWardrobes = GUILayout.BeginScrollView(_scrollViewVectorWardrobes, GUILayout.Height(250));
                foreach (KeyValuePair<string, Wardrobe> pair in StorageRoom.Wardrobes)
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

            // Attires
            try
            {
                if (_attireError || _selectedWardrobe == null)
                    return;
                GUILayout.Space(20);
                GUILayout.BeginVertical(_selectedWardrobe.wearers, GUI.skin.box, GUILayout.Width(300), GUILayout.Height(250));
                GUILayout.Space(15);
                _scrollViewVectorAttires = GUILayout.BeginScrollView(_scrollViewVectorAttires, GUILayout.Height(250));
                foreach (IAttire attire in _selectedWardrobe.attires)
                {
                    if (GUILayout.Button(attire.Name))
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

            // Clothes
            try
            {
                if (_clothesError || _selectedAttire == null)
                    return;
                GUILayout.Space(20);
                GUILayout.BeginVertical(_selectedAttire.Name, GUI.skin.box, GUILayout.Width(500), GUILayout.Height(250));
                GUILayout.Space(15);
                _scrollViewVectorClothes = GUILayout.BeginScrollView(_scrollViewVectorClothes, GUILayout.Height(250));
                if (GUILayout.Button(_selectedAttire.Enabled ? "Enabled" : "Disabled"))
                {
                    _selectedAttire.Enabled = !_selectedAttire.Enabled;
                    if (!_selectedAttire.Enabled)
                    {
                        Main.settings.unactiveFiles.Add(_selectedAttire.Id);
                    }
                    else if (Main.settings.unactiveFiles.Contains(_selectedAttire.Id))
                    {
                        Main.settings.unactiveFiles.Remove(_selectedAttire.Id);
                    }

                }
                GUILayout.Label("Path: \n" + PathWithBroforceAtFirst(_selectedAttire.Directory));
                GUILayout.Space(10);
                foreach (KeyValuePair<string, string> pair in _selectedAttire.Clothes)
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
            Settings.useStats = GUILayout.Toggle(Settings.useStats, "Use Attires Stats");
            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            _fileName = GUILayout.TextField(_fileName, GUILayout.Width(300), GUILayout.ExpandWidth(false));
            if(GUILayout.Button("Create 'Attire' JSON File", GUILayout.ExpandWidth(false)))
            {
                StorageRoom.CreateFuturisticAttireJsonFile(_fileName, StorageRoom.WardrobesDirectory);
            }
            if (GUILayout.Button("Create 'Attire Collection' JSON File", GUILayout.ExpandWidth(false)))
            {
                StorageRoom.CreateAttireCollectionJsonFile(_fileName, StorageRoom.WardrobesDirectory);
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        public static string PathWithBroforceAtFirst(string path)
        {
            int startIndex = Directory.GetParent(Directory.GetCurrentDirectory()).FullName.Length;
            return path.Substring(startIndex, path.Length - startIndex);
        }
    }
}
