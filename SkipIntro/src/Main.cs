using System;
using UnityEngine;
using System.Reflection;
using HarmonyLib;
using UnityModManagerNet;

namespace SkipIntroMod
{
    static class Main
    {
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                var harmony = new Harmony(modEntry.Info.Id);
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);
            }
            catch (Exception ex)
            {
                modEntry.Logger.Log("Failed to Patch Harmony !\n" + ex.ToString());
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(Startup), "Update")]
    static class SkipIntro_Patch
    {
        static bool Prefix(Startup __instance)
        {
            GameState.LoadLevel("MainMenu");
            return false;
        }
    }
}
