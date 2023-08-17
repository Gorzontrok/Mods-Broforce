using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FilteredBros. Patches
{
    // freedBros get the max value of heroUnlockIntervals ; Remove this so unlock number stay the same
    [HarmonyPatch(typeof(PlayerProgress), "PostLoadProcess")]
    static class PlayerProgress_PostLoadProcess_Patch
    {
        static bool Prefix(PlayerProgress __instance)
        {
            try
            {
                return false;
            }
            catch (Exception e)
            {
                Main.Log(e);
            }
            return true;
        }
    }
    // Patch Hero unlock intervals for spawn
    [HarmonyPatch(typeof(PlayerProgress), "IsHeroUnlocked")]
    internal static class PlayerProgress_IsHeroUnlocked_Patch
    {
        private static void Prefix(PlayerProgress __instance)
        {
            if (!Main.CanUsePatch) return;

            try
            {
                Dictionary<int, HeroType> heroUnlockIntervals = Main.UpdateDictionary();
                if (heroUnlockIntervals.Count > 0 || heroUnlockIntervals.ContainsKey(0))
                {
                    Traverse.Create(typeof(HeroUnlockController)).Field("_heroUnlockIntervals").SetValue(heroUnlockIntervals);
                    __instance.unlockedHeroes.Clear();
                    foreach (var heroType in heroUnlockIntervals.Values)
                    {
                        __instance.unlockedHeroes.Add(heroType);
                    }
                    var yetToBePlayedUnlockedHeroes = __instance.yetToBePlayedUnlockedHeroes.ToArray();
                    foreach (var hero in yetToBePlayedUnlockedHeroes)
                    {
                        if (!heroUnlockIntervals.ContainsValue(hero))
                            __instance.yetToBePlayedUnlockedHeroes.Remove(hero);
                    }
                }
                else
                {
                    Main.Log("You have selected 0 bro, please select at least one. (The one who are name \"???\" don't count)");
                    heroUnlockIntervals = Main.originalDict;
                }
            }
            catch (Exception ex) { Main.Log("Failed to patch the Unlock intervals", ex); }
        }
    }

    [HarmonyPatch(typeof(Player), "AddLife")]
    static class LifeNumberLimited_Patch
    {
        static void Postfix(Player __instance)
        {
            if (!Main.CanUsePatch) return;
            try
            {
                if (Main.enabled && Main.settings.maxLifeNumber != 0)
                {
                    while (__instance.Lives > Main.settings.maxLifeNumber)
                    {
                        __instance.Lives--;
                    }
                }
            }
            catch (Exception ex)
            {
                Main.Log(ex);
            }
        }
    }

    [HarmonyPatch(typeof(MapData), "Deserialize")]
    static class MapData_Deserialize_Patch
    {
        static void Postfix(MapData __instance)
        {
            if (!Main.CanUsePatch && Main.settings.ignoreForcedBros) return;

            __instance.forcedBro = HeroType.Random;
            __instance.forcedBros = new List<HeroType>();
        }
    }
}
