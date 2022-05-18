using System;
using UnityEngine;
using HarmonyLib;

namespace ReskinMod.Patch
{
    [HarmonyPatch(typeof(Brominator), "Start")]
    static class Brominator_Reskin_Patch
    {
        static void Postfix(Brominator __instance)
        {
            try
            {
                HeroType CurrentHero = __instance.heroType;

                var cInfo = Utility.GetBroReskinInfo(CurrentHero);

                cInfo.CheckTex(CurrentHero, __instance);
                cInfo.CheckSecondTex(__instance.humanBrominator, __instance.humanGunBrominator);
                cInfo.CheckAvatarTex2(__instance.brominatorRobotAvatar);

                __instance.humanBrominator.mainTexture = cInfo.CharacterTex;
                //(__instance as Brominator).humanBrominator.SetTexture("_MainTex", CharacterTex);
                __instance.humanGunBrominator.mainTexture = cInfo.GunTex;
                __instance.humanGunBrominator.SetTexture("_MainTex", cInfo.GunTex);

                __instance.metalBrominator.mainTexture = cInfo.GetCharacterTex2(__instance.metalBrominator);
                __instance.metalGunBrominator.mainTexture = cInfo.GetGunTex2(__instance.metalGunBrominator);

                __instance.brominatorHumanAvatar.mainTexture = cInfo.AvatarTex;
                __instance.brominatorRobotAvatar.mainTexture = cInfo.GetAvatarTex2(__instance.brominatorRobotAvatar);
            }
            catch (Exception ex) { Main.bmod.ExceptionLog(ex); }
        }
    }
    [HarmonyPatch(typeof(Nebro), "Start")]
    static class Nebro_Reskin_Patch
    {
        static void Postfix(Nebro __instance)
        {
            try
            {
                HeroType CurrentHero = __instance.heroType;

                var cInfo = Utility.GetBroReskinInfo(CurrentHero);
                cInfo.CheckTex(CurrentHero, __instance);
                cInfo.CheckSecondTex(__instance.materialArmless);

                __instance.materialArmless.mainTexture = cInfo.GetCharacterTex2(__instance.materialArmless);

                Material materialNormal = Material.Instantiate(__instance.materialArmless);
                materialNormal.mainTexture = cInfo.CharacterTex;
                Traverse.Create(__instance).Field("materialNormal").SetValue(materialNormal);
            }
            catch (Exception ex) { Main.bmod.ExceptionLog(ex); }
        }
    }
    [HarmonyPatch(typeof(AshBrolliams), "Start")]
    static class AshBrolliams_Reskin_Patch
    {
        static void Postfix(AshBrolliams __instance)
        {
            try
            {
                HeroType CurrentHero = __instance.heroType;

                var cInfo = Utility.GetBroReskinInfo(CurrentHero);
                cInfo.CheckTex(CurrentHero, __instance);
                cInfo.CheckAvatarTex2(__instance.bloodyAvatar);
                __instance.bloodyAvatar.mainTexture = cInfo.GetAvatarTex2(__instance.bloodyAvatar);
            }
            catch (Exception ex) { Main.bmod.ExceptionLog(ex); }
        }
    }
}
