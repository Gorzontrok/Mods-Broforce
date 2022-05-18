using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace TweaksFromPigs.HPatch
{
    // Enable vSync on exit for avoid high useless fps when start the game.
    [HarmonyPatch(typeof(MainMenu), "ExitGame")]
    static class UMM_OnDestroy_Patch
    {
        static void Prefix()
        {
            if (!Main.enabled) return;
            try
            {
                QualitySettings.vSyncCount = 1;
                PlayerOptions.Instance.vSync = true;
                Main.bmod.Log("Active vSync On Exit", RLogType.Information);
            }
            catch(Exception ex) { Main.bmod.logger.ExceptionLog("Failed to active V-Sync on exit.\n" + ex); }
        }
    }

    //Patch pig
    [HarmonyPatch(typeof(Animal), "Awake")]
    static class Pig_Awake_Patch
    {
        static void Postfix(Animal __instance)
        {
            if (!Main.enabled) return;
            try
            {
                if(AssetsCollection.characterShader == null)
                {
                    AssetsCollection.characterShader = __instance.material.shader;
                }
                if (Main.settings.pigAreAlwaysTerror || Map.MapData.theme == LevelTheme.Hell)
                {
                    //__instance.material.mainTexture = TFP_Utility.CreateTexFromMat("Gimp_Pig_anim.png", __instance.material);
                    __instance.material = AssetsCollection.Gimp_Pig_anim;
                }
            }
            catch(Exception ex)
            {
                Main.bmod.logger.ExceptionLog("Failed to Patch Pigs\n" + ex);
            }
        }
    }

    // Patch Hero unlock intervals for spawn
    [HarmonyPatch(typeof(HeroUnlockController), "IsAvailableInCampaign")]
    static class HeroUnlockController_IsAvailableInCampaign_Patch
    {
        public static void Prefix()
        {
            try
            {
                Settings sett = Main.settings;
                if (!Main.enabled || (Compatibility.FilteredBros.i.IsEnabled) || (Compatibility.ExpendablesBros.i.IsEnabled) || (Compatibility.ForBralef.i.IsEnabled)) return;

                if (Main.settings.changeHeroUnlock && Main.heroDictionary.Count > 0)
                {
                    Traverse.Create(typeof(HeroUnlockController)).Field("_heroUnlockIntervals").SetValue(Main.heroDictionary);
                   PlayerProgress.Instance.unlockedHeroes.AddRange(PlayerProgress.Instance.yetToBePlayedUnlockedHeroes);
                }
            }
            catch(Exception ex) { Main.bmod.logger.ExceptionLog("Failed to patch Unlock Intervals.\n" + ex); }
        }
    }

    [HarmonyPatch(typeof(HeroUnlockController), "IsDeathMatchBro")]
    static class MoreBroInDeathMatch_Patch
    {
        static bool Prefix(HeroType nextHeroType, ref bool __result)
        {
            if (!Main.enabled) return true;

            Main.settings.moreBroInDeathMatch = true;

            if(Main.settings.moreBroInDeathMatch)
            {
                switch(nextHeroType)
                {
                    case HeroType.BrondleFly:
                    case HeroType.TheBrofessional:
                    case HeroType.Predabro:
                    case HeroType.CherryBroling:
                    case HeroType.BoondockBros:
                        __result = true;
                        return false;
                }
            }
            return true;

        }
    }

    // The grenade for mech drop do smoke.
    [HarmonyPatch(typeof(BroBase), "ThrowMechDropGrenade")]
    static class MechDropGrenadeDoSomke_Patch
    {
        static void Prefix(BroBase __instance)
        {
            if (!Main.enabled) return;
            try
            {
                    (ProjectileController.GetMechDropGrenadePrefab() as GrenadeAirstrike).callInTank = Main.settings.mechDropDoesSmoke;
            }
            catch(Exception ex) { Main.bmod.logger.ExceptionLog("Failed to add pink smoke to Mech Drop.\n" + ex); }
        }
    }

    [HarmonyPatch(typeof(Utility.Platforms.Platform), "GetPrimaryUserName")]
    static class GetCustomPlayerName_Patch
    {
        static bool Prefix(ref string __result)
        {
            if (!Main.enabled) return true;
            if(!string.IsNullOrEmpty(Main.settings.customPlayerName))
            {
                __result = Main.settings.customPlayerName;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(CutsceneIntroRoot), "ContainsCutscene")]
    static class NoCutscene_Patch
    {
        static bool Prefix(ref bool __result)
        {
            CutsceneController.holdPlayersStill = false;
            CutsceneController.Instance.CloseAllCutscenes();
            __result = false;
            return false;
        }
    }
}
