using System;
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
        static System.Random rng = new System.Random();
        static void Postfix(Animal __instance)
        {
            if (!Main.enabled) return;
            if(rng.Next(3) == 2)
                __instance.isRotten = true;
            if(__instance.isRotten && !Main.settings.PigAreAlwaysTerror)
            {
                __instance.material.mainTexture = Utility.CreateTexFromMat("pig_animStinky.png", __instance.material);
            }
            if (Main.settings.PigAreAlwaysTerror || Map.MapData.theme == LevelTheme.Hell)
                __instance.material.mainTexture = Utility.CreateTexFromMat("Gimp_Pig_anim.png", __instance.material);
        }
    }

    // Patch Hero unlock intervals for spawn
    [HarmonyPatch(typeof(HeroUnlockController), "IsAvailableInCampaign")]
    static class HeroUnlockController_IsAvailableInCampaign_Patch
    {
        public static void Prefix(HeroType hero)
        {
            if (!Main.enabled) return;
            if (Main.settings.ChangeHeroUnlock)
            {
                Traverse.Create(typeof(HeroUnlockController)).Field("_heroUnlockIntervals").SetValue(Main.HeroDictionary);
            }
        }
    }

    // i like the steel from expendabros
    [HarmonyPatch(typeof(Map), "Start")]
    static class ChangeSteelBlockTex_Patch // Don't work
    {
        static void Postfix(Map __instance)
        {
            if (!Main.enabled) return;
            /*  try
              {
                  __instance.activeTheme.blockPrefabSteel.GetComponent<SpriteSM>().meshRender.sharedMaterial.SetTexture("_MainTex", Utility.CreateTexFromSpriteSM("Steel.png", __instance.activeTheme.blockPrefabSteel.sprite));

              }catch(Exception ex) { Main.Log(ex); }*/
        }
    }

    // The grenade for mech drop do smoke.
    [HarmonyPatch(typeof(BroBase), "ThrowMechDropGrenade")]
    static class MechDropGrenadeDoSomke_Patch
    {
        static void Prefix(BroBase __instance)
        {
            if (!Main.enabled) return;
             (ProjectileController.GetMechDropGrenadePrefab() as GrenadeAirstrike).callInTank = true;
        }
    } 
}
