using RocketLib0;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace TweaksFromPigs
{
    // Enable vSync on exit for avoid high useless fps when start the game.
    [HarmonyPatch(typeof(MainMenu), "ExitGame")]
    static class UMM_OnDestroy_Patch
    {
        static void Prefix()
        {
            if (!Main.enabled) return;
            QualitySettings.vSyncCount = 1;
            PlayerOptions.Instance.vSync = true;
            Main.Log("Enable vSync On Exit");
        }
    }

    //Patch pig
    [HarmonyPatch(typeof(Animal), "Awake")]
    static class Pig_Awake_Patch
    {
        static void Postfix(Animal __instance)
        {
            if (!Main.enabled) return;
            if(Main.settings.EnabledSickPigs)
            {
                if (Utility.rand.Next(3) == 2)
                    __instance.isRotten = true;
                if (__instance.isRotten && !Main.settings.PigAreAlwaysTerror)
                {
                    __instance.material.mainTexture = Utility.CreateTexFromMat("pig_animStinky.png", __instance.material);
                }
                if (Main.settings.PigAreAlwaysTerror || Map.MapData.theme == LevelTheme.Hell)
                    __instance.material.mainTexture = Utility.CreateTexFromMat("Gimp_Pig_anim.png", __instance.material);
            }
        }
    }

    // Patch Hero unlock intervals for spawn
    [HarmonyPatch(typeof(HeroUnlockController), "IsAvailableInCampaign")]
    static class HeroUnlockController_IsAvailableInCampaign_Patch
    {
        public static void Prefix()
        {
            if (!Main.enabled) return;
            if (Main.settings.ChangeHeroUnlock && Main.HeroDictionary.Count > 0)
            {
                Traverse.Create(typeof(HeroUnlockController)).Field("_heroUnlockIntervals").SetValue(Main.HeroDictionary);
                PlayerProgress.Instance.yetToBePlayedUnlockedHeroes = new List<HeroType>();
            }
        }
    }

    // The grenade for mech drop do smoke.
    [HarmonyPatch(typeof(BroBase), "ThrowMechDropGrenade")]
    static class MechDropGrenadeDoSomke_Patch
    {
        static void Prefix(BroBase __instance)
        {
            if (!Main.enabled) return;
            if(Main.settings.MechDropDoesFumiginene)
            {
                (ProjectileController.GetMechDropGrenadePrefab() as GrenadeAirstrike).callInTank = true;
            }
        }
    }
}
