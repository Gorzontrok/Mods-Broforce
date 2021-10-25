using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;
using RocketLib0;

namespace OnlyPockettedSpecialAmmo
{
    static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        internal static BroforceMod bmod;

        private static System.Random rnd = new System.Random();

        static List<PockettedSpecialAmmoType> pockettedList = new List<PockettedSpecialAmmoType>();

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

            try
            {
                pockettedList.AddRange(RocketLib._HeroUnlockController.ListPockettedSpecialAmmoTypes);
                pockettedList.Remove(PockettedSpecialAmmoType.Standard);
            }catch(Exception ex) { mod.Logger.Log("Failed while modifying the pockettedList !\n" + ex); }

            try
            {
                bmod = new BroforceMod(mod, true);
            }catch(Exception ex) { mod.Logger.Log("Failed to create Broforce Mod\n" + ex.ToString()); }
            return true;
        }
        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Infinity = GUILayout.Toggle(settings.Infinity, "Infinity mode");
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }

        internal static void Log(object str, RLogType type = RLogType.Log)
        {
            bmod.Log(str, type);
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }

        internal static PockettedSpecialAmmoType GetRandomPockettedSpecialAmmo()
        {
            return pockettedList[rnd.Next(pockettedList.Count)];
        }
    }

    public class Settings : UnityModManager.ModSettings
    {
        public bool Infinity;
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
                Traverse.Create(__instance).Field("_specialAmmo").SetValue(0);

                __instance.PickupPockettableAmmo(Main.GetRandomPockettedSpecialAmmo());
            }
            catch(Exception ex)
            {
                Main.Log("Failed to assign Pocketted special ammo !\n" + ex, RLogType.Exception);
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
                if (Main.settings.Infinity)
                    __instance.PickupPockettableAmmo(Main.GetRandomPockettedSpecialAmmo());
            }
            catch (Exception ex) { Main.Log("Failed to add special ammo on reset !\n" + ex, RLogType.Exception); }

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
                if(Main.settings.Infinity)
                    __instance.PickupPockettableAmmo(Main.GetRandomPockettedSpecialAmmo());
            }
            catch (Exception ex) { Main.Log("Failed to add special ammo on reset for The Brolander !\n" + ex, RLogType.Exception); }

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
                Traverse.Create(__instance).Field("_specialAmmo").SetValue(0);

                __instance.PickupPockettableAmmo(Main.GetRandomPockettedSpecialAmmo());
            }
            catch(Exception ex)
            {
                Main.Log("Failed to assign Pocketted special ammo for The Brolander !\n" + ex, RLogType.Exception);
            }
        }
    }
}
