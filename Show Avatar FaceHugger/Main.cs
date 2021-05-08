using System;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;
using System.Reflection;

namespace Avatar_FaceHugger_Mod
{
    public static class Main
    {

        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            mod = modEntry;
            try
            {
                    modEntry.OnGUI = OnGUI;
                    modEntry.OnSaveGUI = OnSaveGUI;
                    modEntry.OnToggle = OnToggle;
                    settings = Settings.Load<Settings>(modEntry);
                    var harmony = new Harmony(modEntry.Info.Id);
                    var assembly = Assembly.GetExecutingAssembly();
                    harmony.PatchAll(assembly);
            }
            catch (Exception ex)
            {
                mod.Logger.Log(ex.ToString());
            }
            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();
            settings.facehuggerEnabled = GUILayout.Toggle(settings.facehuggerEnabled, "Enable FaceHugger", GUILayout.ExpandWidth(false));
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
        public static void Log(String str)
        {
            mod.Logger.Log(str);
        }

    }

    public class Settings : UnityModManager.ModSettings
    {
        public bool facehuggerEnabled;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

    }

    [HarmonyPatch(typeof(PlayerHUD), "ShowFaceHugger")]
    static class Avatar_FaceHugger_Patch
    {
        static void Prefix(PlayerHUD __instance)
        {
            if (!Main.settings.facehuggerEnabled) //If facehugger not enabled
                return;                           //Do nothing

            //Otherwise show the facehugger when alien on head of the Bro
            //This code is just the opposite of HideFaceHugger()
            __instance.showFaceHugger = true;
            __instance.avatar.SetLowerLeftPixel(new Vector2(__instance.faceHugger1.lowerLeftPixel.x, 1f));
            __instance.faceHugger1.gameObject.SetActive(true);
            
        }
    }
}
