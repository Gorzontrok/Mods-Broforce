using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;


namespace ReplaceRambroWithCasey
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

        static void Start()
        {

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

        [HarmonyPatch(typeof(HeroController), "OnAfterDeserialize")]
        static class Patch
        {
            static void Postfix(HeroController __instance)
            {
                Dictionary<HeroType, HeroController.HeroDefinition> _heroData = Traverse.Create(__instance).Field("_heroData").GetValue<Dictionary<HeroType, HeroController.HeroDefinition>>();
                _heroData[HeroType.Rambro].characterReference = _heroData[HeroType.CaseyBroback].characterReference;
                Traverse.Create(__instance).Field("_heroData").SetValue(_heroData);
            }
        }
    }
}
