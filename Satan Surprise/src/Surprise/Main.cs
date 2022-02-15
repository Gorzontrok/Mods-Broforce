using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace Surprise
{
    public class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        public static bool HardMode;

        public static Mook NewMookForMecha;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGui;
            modEntry.OnUpdate= OnUpdate;
            modEntry.OnSaveGUI= OnSaveGUI;
            settings = Settings.Load<Settings>(modEntry);
            var harmony = new Harmony(modEntry.Info.Id);

            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);
            }
            catch (Exception ex)
            {
                mod.Logger.Log("Failed to Patch Harmony !\n"+ex.ToString());
            }

            mod = modEntry;

            return true;
        }

        static void OnGui(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();
            settings.HardMode = GUILayout.Toggle(settings.HardMode, "Super Ultra Giga Tera SURPRISE !");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            try
            {
                HardMode = settings.HardMode;
            }
            catch (Exception ex) { mod.Logger.Log("Failed to set HardMode value !\n" + ex.ToString()); }

        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }


        public static void Log(object str)
        {
            mod.Logger.Log(str.ToString());
        }

        public static void Wait(float time)
        {
            while (time > 0)
            {
                time -= Time.deltaTime;
            }
        }
    }

    public class Settings : UnityModManager.ModSettings
    {
        public bool HardMode;
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}