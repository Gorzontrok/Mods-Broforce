using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace FilteredBros.Patches
{
    [HarmonyPatch(typeof(PlayerProgress))]
    public static class PlayerProgressP
    {
        [HarmonyPatch("PostLoadProcess")]
        [HarmonyPrefix]
        private static bool PostLoadProcess(PlayerProgress __instance)
        {
            // 'PlayerProgress.freedBros' get the max value of heroUnlockIntervals
            // Which cause some bros to be marked as '???' on Filtered Bros UI even if they are already unlocked.
            // Skip the original method
            return !Mod.CanUsePatch;
        }

        [HarmonyPatch("IsHeroUnlocked")]
        [HarmonyPrefix]
        private static void IsHeroUnlocked(PlayerProgress __instance) // Patch Hero unlock intervals for spawn
        {
            if (!Mod.CanUsePatch || !Mod.ShouldUpdateUnlockIntervals)
                return;

            Mod.UpdateCurrentUnlockIntervals();
            if (Mod.CurrentUnlockIntervals.Count < 0 /*|| !Mod.CurrentUnlockIntervals.ContainsKey(0)*/)
            {
                Main.Log("You have selected 0 bro, please select at least one. (The one who are name \"???\" don't count)");
                __instance.unlockedHeroes = HeroUnlockController.heroUnlockIntervals.Values.ToList();
                Mod.ShouldUpdateUnlockIntervals = false;
                return;
            }

            // test to see if the patch needs it to works
            Traverse.Create(typeof(HeroUnlockController)).Field("_heroUnlockIntervals").SetValue(Mod.CurrentUnlockIntervals);

            __instance.unlockedHeroes.Clear();
            foreach (HeroType heroType in Mod.CurrentUnlockIntervals.Values)
            {
                __instance.unlockedHeroes.Add(heroType);
            }

            // Make sure that a bro which needs to show its cutscene and isn't selected don't spawned.
            HeroType[] yetToBePlayedUnlockedHeroes = __instance.yetToBePlayedUnlockedHeroes.ToArray();
            foreach (HeroType hero in yetToBePlayedUnlockedHeroes)
            {
                if (!Mod.CurrentUnlockIntervals.ContainsValue(hero))
                    __instance.yetToBePlayedUnlockedHeroes.Remove(hero);
            }
        }
    }

    [HarmonyPatch(typeof(LevelEditorGUI))]
    public static class LevelEditorP
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void Start(LevelEditorGUI __instance)
        {
            if (!Main.enabled || !Main.settings.mod.useInLevelEditor)
            {
                LevelEditorGUI.broChangeHeroTypes = Mod.OriginalUnlockIntervals.Values.ToList();
                __instance.SetFieldValue("heroTypes", LevelEditorGUI.broChangeHeroTypes);
            }
        }

    }

    [HarmonyPatch(typeof(Player), "AddLife")]
    static class LifeNumberLimited_Patch
    {
        static void Postfix(Player __instance)
        {
            if (!Mod.CanUsePatch || Main.settings.mod.maxLifeNumber <= 0)
                return;
            if (__instance.Lives > Main.settings.mod.maxLifeNumber)
                __instance.Lives = Main.settings.mod.maxLifeNumber;
        }
    }

    [HarmonyPatch(typeof(MapData), "Deserialize")]
    static class MapData_Deserialize_Patch
    {
        static void Postfix(MapData __instance)
        {
            if (!Mod.CanUsePatch || !Main.settings.mod.ignoreForcedBros)
                return;

            __instance.forcedBro = HeroType.Random;
            __instance.forcedBros = new List<HeroType>();
        }
    }
}
