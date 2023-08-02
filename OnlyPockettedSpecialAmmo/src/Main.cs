using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;
using System.Linq;

namespace OnlyPockettedSpecialAmmo
{
    static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        static List<PockettedSpecialAmmoType> pockettedList = RocketLib.Collections.PockettedSpecial.SpecialAmmo.ToList();

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;

            settings = Settings.Load<Settings>(modEntry);

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
        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            settings.infinity = GUILayout.Toggle(settings.infinity, "Infinity mode");
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }

        internal static void Log(object str)
        {
            mod.Logger.Log(str.ToString());
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }

        internal static PockettedSpecialAmmoType GetRandomPockettedSpecialAmmo()
        {
            return pockettedList.RandomElement();
        }
    }

    public class Settings : UnityModManager.ModSettings
    {
        public bool infinity;
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

    }

    [HarmonyPatch(typeof(BroBase), "Start")]
    class SetSpecialAmmo_Patch
    {
        static void Postfix(BroBase __instance)
        {
            if (!Main.enabled) return;

            try
            {
                __instance.SpecialAmmo = 0;
                __instance.originalSpecialAmmo = 0;
                __instance.SetFieldValue("_specialAmmo", 0);
                __instance.PickupPockettableAmmo(Main.GetRandomPockettedSpecialAmmo());
            }
            catch(Exception ex)
            {
                Main.Log("Failed to assign Pocketted special ammo !\n" + ex);
            }
        }
    }

    [HarmonyPatch(typeof(TestVanDammeAnim), "ResetSpecialAmmo")]
    class AddPockettedSpecialOnAmmoCrate_Patch
    {
        static void Postfix(TestVanDammeAnim __instance)
        {
            if (!Main.enabled) return;
            try
            {
                if (Main.settings.infinity)
                    __instance.PickupPockettableAmmo(Main.GetRandomPockettedSpecialAmmo());
            }
            catch (Exception ex)
            {
                Main.Log("Failed to add special ammo on reset !\n" + ex);
            }

        }
    }


    [HarmonyPatch(typeof(TheBrolander), "ResetSpecialAmmo")]
    class AddPockettedSpecialOnAmmoCrateFor_TheBrolander_Patch
    {
        static void Postfix(TheBrolander __instance)
        {
            if (!Main.enabled) return;
            try
            {
                if(Main.settings.infinity)
                    __instance.PickupPockettableAmmo(Main.GetRandomPockettedSpecialAmmo());
            }
            catch (Exception ex)
            {
                Main.Log("Failed to add special ammo on reset for The Brolander !\n" + ex);
            }

        }
    }

    [HarmonyPatch(typeof(TheBrolander), "Start")]
    class SetSpecialAmmoFor_TheBrolander_Patch
    {
        static void Postfix(TheBrolander __instance)
        {
            if (!Main.enabled) return;

            try
            {
                __instance.maxSpecialAmmo = 0;
                __instance.SpecialAmmo = 0;
                __instance.originalSpecialAmmo = 0;
                __instance.SetFieldValue("_specialAmmo", 0);

                __instance.PickupPockettableAmmo(Main.GetRandomPockettedSpecialAmmo());
            }
            catch(Exception ex)
            {
                Main.Log("Failed to assign Pocketted special ammo for The Brolander !\n" + ex);
            }
        }
    }
}
