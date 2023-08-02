using System;
using UnityEngine;
using UnityModManagerNet;

namespace AutoEnterPassword
{
    static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            settings = Settings.Load<Settings>(modEntry);
            mod = modEntry;

            try
            {
                if(settings.autoLoad.IsNullOrEmpty())
                    settings.autoLoad = new string[0];
                Mod.Initialize();
            }
            catch (Exception ex)
            {
                Main.Log("Failed Mod Initialization\n" + ex);
            }

            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.Label("Vanilla Password :");
            GUILayout.BeginHorizontal();

            foreach(var vanilla in Mod.vanillaPasswords)
            {
                Mod.GamePasswordUI(vanilla);
            }

            GUILayout.EndHorizontal();

            if (Mod.passwords.Count <= 0) return;

            GUILayout.Label("RocketLib Password :");
            for (int i = 0; i < Mod.passwords.Count; i++)
            {
                if (i == 0)
                {
                    GUILayout.BeginHorizontal();
                }

                Mod.GamePasswordUI(Mod.passwords[i]);

                if (i % 5 == 0 || i == Mod.passwords.Count)
                {
                    GUILayout.EndHorizontal();
                }
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

        public static void Log(object str)
        {
            mod.Logger.Log(str.ToString());
        }
    }

    public class Settings : UnityModManager.ModSettings
    {
        public string[] autoLoad;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            autoLoad = Mod.autoLoadSession.ToArray();
            Save(this, modEntry);
        }
    }
}
