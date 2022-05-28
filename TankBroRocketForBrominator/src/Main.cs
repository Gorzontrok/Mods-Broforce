using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityModManagerNet;

namespace TankBroRocketForBrominator
{
    static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnToggle = OnToggle;
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

            return true;
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
    [HarmonyPatch(typeof(Brominator), "Start")]
    class TankBroRocketForBrominator_Patch
    {
        static void Postfix(Brominator __instance)
        {
            if (Main.enabled)
            {
                try
                {
                    __instance.projectile = (HeroController.GetHeroPrefab(HeroType.TankBro) as TankBro).projectile;
                }
                catch (Exception ex) { Main.Log(ex); }
            }
        }
    }
}
