using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace LevelEditorExtended
{
    public static class LevelEditorGUIExtensions
    {
        public static void PlayClickSound(this LevelEditorGUI gui)
        {
            typeof(LevelEditorGUI).CallMethod("PlayClickSound");
        }
        public static void PlaceHoverDoodad(this LevelEditorGUI gui)
        {
            gui.CallMethod("PlaceHoverDoodad");
        }

        #region Fields
        public static int C(this LevelEditorGUI gui)
        {
            return gui.GetFieldValue<int>("c");
        }
        public static int R(this LevelEditorGUI gui)
        {
            return gui.GetFieldValue<int>("r");
        }
        public static List<DoodadType> get_doodadTypes(this LevelEditorGUI gui)
        {
            return gui.GetFieldValue< List<DoodadType> >("doodadTypes");
        }
        public static void set_doodadTypes(this LevelEditorGUI gui, List<DoodadType> list)
        {
            gui.SetFieldValue("doodadTypes", list);
        }
        public static DoodadType get_curDoodadType(this LevelEditorGUI gui)
        {
            return gui.GetFieldValue<DoodadType>("curDoodadType");
        }
        public static void set_curDoodadType(this LevelEditorGUI gui, DoodadType type)
        {
            gui.SetFieldValue("curDoodadType", type);
        }
        #endregion

        #region Static Fields
        public static Vector2 get_scrollPos(this LevelEditorGUI gui)
        {
            return Traverse.Create(typeof(LevelEditorGUI)).Field("scrollPos").GetValue<Vector2>();
        }
        public static void set_scrollPos(this LevelEditorGUI gui, Vector2 value)
        {
            Traverse.Create(typeof(LevelEditorGUI)).Field("scrollPos").SetValue(value);
        }

        public static bool get_showAllDoodads(this LevelEditorGUI gui)
        {
            return Traverse.Create(typeof(LevelEditorGUI)).Field("showAllDoodads").GetValue<bool>();
        }
        public static void set_showAllDoodads(this LevelEditorGUI gui, bool value)
        {
            Traverse.Create(typeof(LevelEditorGUI)).Field("showAllDoodads").SetValue(value);
        }
        #endregion
    }
}
