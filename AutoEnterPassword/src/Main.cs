using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;
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
            mod = modEntry;

            modEntry.OnGUI = ModUI.OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;

            // Load Settings
            settings = Settings.Load<Settings>(modEntry);
            if (settings.VanillaLoadOnStart.IsNullOrEmpty())
                settings.VanillaLoadOnStart = new bool[Mod.VANILLA_PASSWORD_COUNT];

            try
            {
                Mod.Initialize();
                var harmony = new Harmony(modEntry.Info.Id);
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);
            }
            catch (Exception ex)
            {
                Main.Log("Failed Mod Initialization\n" + ex);
            }

            return true;
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.LoadOnStartRocketLib = Mod.passwordsLoadOnStart.Where(kv => kv.Value).Select(kv => kv.Key).ToArray();
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
        public bool[] VanillaLoadOnStart;
        public string[] LoadOnStartRocketLib;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }

    [HarmonyPatch(typeof(UnityModManager.UI), "Awake")]
    public static class AfterLoadedMods_Patch
    {
        static void Prefix()
        {
            if (!Main.enabled)
                return;

            Mod.StartLoadPasswords();
        }
    }
}
