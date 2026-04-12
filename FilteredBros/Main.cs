using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityModManagerNet;

namespace FilteredBros
{
    public static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        public static bool cheat;

        private static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnGUI = ModUI.OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            settings = Settings.Load<Settings>(modEntry);
            mod = modEntry;

            // Patch assembly
            var harmony = new Harmony(modEntry.Info.Id);
            var assembly = Assembly.GetExecutingAssembly();
            harmony.PatchAll(assembly);

            Mod.Start();

            cheat = Environment.UserName == "Gorzon";

            return true;
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            List<bool> list = new List<bool>();
            foreach(BroToggle b in BroToggle.All)
            {
                list.Add(b.enabled);
            }
            settings.brosEnable = list;
            settings.Save(modEntry);
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }

        public static void Log(object msg)
        {
            mod.Logger.Log(msg.ToString());
        }
    }
}