using HarmonyLib;
using System.Reflection;
using UnityModManagerNet;

namespace ActivateModsOnline
{
    internal static class Main
    {
        const string NAME_DISABLED = "<color=red>ONLINE MODS DEACTIVATED</color>";
        const string NAME_ENABLED = "<color=\"#00ff00\">ONLINE MODS ACTIVATED</color>";

        public static UnityModManager.ModEntry mod;
        public static bool enabled;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnToggle = OnToggle;
            mod = modEntry;

            modEntry.Info.DisplayName = modEntry.Enabled ? NAME_ENABLED : NAME_DISABLED;

            var harmony = new Harmony(modEntry.Info.Id);
            var assembly = Assembly.GetExecutingAssembly();
            harmony.PatchAll(assembly);

            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            modEntry.Info.DisplayName = enabled ? NAME_ENABLED : NAME_DISABLED;
            return true;
        }

    }

    [HarmonyPatch(typeof(Connect), "BypassNetworkLayer", MethodType.Getter)]
    internal static class Patch
    {
        public static bool Prefix(ref bool __result)
        {
            if (!Main.enabled)
                return true;

            __result = true;
            return false;
        }
    }
}

