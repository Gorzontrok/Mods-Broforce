using System;
using UnityEngine;
using HarmonyLib;
using ReskinMod.Skins;

namespace ReskinMod.Patch
{
    /*
     SkinCollection skinCollection = SkinCollection.GetSkinCollection(__instance.GetType().Name.ToLower());
     if(skinCollection != null)
     {
         Skin decapitated = skinCollection.GetSkin(SkinType.Decapitated);
         if(decapitated != null)
         {
             __instance.decapitatedMaterial.mainTexture = decapitated.texture;
         }
     }
     */
    [HarmonyPatch(typeof(MookTrooper), "Awake")]
    static class MookTrooper_reskin_Patch
    {
        static void Postfix(MookTrooper __instance)
        {
            try
            {
                SkinCollection skinCollection = SkinCollection.GetSkinCollection(__instance.GetType().Name.ToLower());
                if(skinCollection != null)
                {
                    Skin decapitated = skinCollection.GetSkin(SkinType.Decapitated, 0);
                    if(decapitated != null)
                    {
                        __instance.decapitatedMaterial.mainTexture = decapitated.texture;
                    }
                }
            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
        }
    }
    [HarmonyPatch(typeof(MookSuicide), "Start")]
    static class MookSuicide_Reskin_Patch
    {
        static void Prefix(MookSuicide __instance)
        {
            try
            {

                SkinCollection skinCollection = SkinCollection.GetSkinCollection(__instance.GetType().Name.ToLower());
                if (skinCollection != null)
                {
                    Skin decapitated = skinCollection.GetSkin(SkinType.Decapitated, 0);
                    Skin char2 = skinCollection.GetSkin(SkinType.Character, 1);
                    if(decapitated != null)
                    {
                        __instance.decapitatedMaterial.mainTexture = decapitated.texture;
                    }

                    if(char2 != null)
                    {
                        __instance.warningMaterial.mainTexture = char2.texture;
                    }
                }
            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
        }
    }
    [HarmonyPatch(typeof(MookBigGuy), "Awake")]
    static class BigGuy_Reskin_Patch
    {
        static void Postfix(MookBigGuy __instance)
        {
            try
            {
                SkinCollection skinCollection = SkinCollection.GetSkinCollection(__instance.GetType().Name.ToLower());
                if (skinCollection != null)
                {
                    Skin decapitated = skinCollection.GetSkin(SkinType.Decapitated, 0);
                    if(decapitated != null)
                    {
                        __instance.decapitatedMaterial.mainTexture = decapitated.texture;
                    }
                }
            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
        }
    }
    [HarmonyPatch(typeof(SkinnedMook), "Start")]
    static class SkinnedMook_Reskin_Patch
    {
        static void Prefix(SkinnedMook __instance)
        {
            try
            {
                SkinCollection skinCollection = SkinCollection.GetSkinCollection(__instance.GetType().Name.ToLower());
                if (skinCollection != null)
                {
                    Skin decapitated = skinCollection.GetSkin(SkinType.Decapitated, 0);
                    if (decapitated != null)
                    {
                        __instance.decapitatedMaterial.mainTexture = decapitated.texture;
                    }
                }
            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
        }
    }

    [HarmonyPatch(typeof(MookHellBoomer), "Awake")]
    static class MookHellBoomer_Reskin_Patch
    {
        static void Postfix(MookHellBoomer __instance)
        {
            try
            {
                SkinCollection skinCollection = SkinCollection.GetSkinCollection(__instance.GetType().Name.ToLower());
                if (skinCollection != null)
                {
                    Skin character2 = skinCollection.GetSkin(SkinType.Character, 1);
                    if (character2 != null)
                    {
                        __instance.warningMaterial.mainTexture = character2.texture;
                    }
                }
            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
        }
    }

    [HarmonyPatch(typeof(MookSuicideUndead), "Start")]
    static class MookSuicideUndead_Reskin_Patch
    {
        static void Prefix(MookSuicideUndead __instance)
        {
            try
            {
                SkinCollection skinCollection = SkinCollection.GetSkinCollection(__instance.GetType().Name.ToLower());
                if (skinCollection != null)
                {
                    Skin character2 = skinCollection.GetSkin(SkinType.Character, 1);
                    if (character2 != null)
                    {
                        __instance.warningMaterial.mainTexture = character2.texture;
                    }
                }
            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
        }
    }

    [HarmonyPatch(typeof(ScoutMook), "Awake")]
    static class ScoutMook_Reskin_Patch
    {
        static void Postfix(ScoutMook __instance)
        {
            try
            {
                SkinCollection skinCollection = SkinCollection.GetSkinCollection(__instance.GetType().Name.ToLower());
                if (skinCollection != null)
                {
                    Skin character2 = skinCollection.GetSkin(SkinType.Character, 1);
                    if (character2 != null)
                    {
                        __instance.disarmedMaterial.mainTexture = character2.texture;
                    }
                }
            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
        }
    }
    [HarmonyPatch(typeof(SatanMiniboss), "Start")]
    static class SatanMiniboss_Reskin_Patch
    {
        static void Prefix(SatanMiniboss __instance)
        {
            try
            {
                SkinCollection skinCollection = SkinCollection.GetSkinCollection(__instance.GetType().Name.ToLower());
                if (skinCollection != null)
                {
                    Skin character2 = skinCollection.GetSkin(SkinType.Character, 1);
                    if (character2 != null)
                    {
                        __instance.satanStage2Material.mainTexture = character2.texture;
                    }
                }
            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
        }
    }

    [HarmonyPatch(typeof(MookArmouredGuy), "Start")]
    static class MookArmouredGuy_Reskin_Patch
    {
        static void Postfix(MookArmouredGuy __instance)
        {
            try
            {
                SkinCollection skinCollection = SkinCollection.GetSkinCollection(__instance.GetType().Name.ToLower());
                if (skinCollection != null)
                {
                    Skin character = skinCollection.GetSkin(SkinType.Character, 0);
                    if (character != null)
                    {
                       Traverse.Create(__instance).Field("originalMaterial").GetValue<Material>().mainTexture = character.texture;
                    }
                    character = skinCollection.GetSkin(SkinType.Character, 1);
                    if (character != null)
                    {
                        __instance.hurtMaterial.mainTexture = character.texture;
                    }
                    character = skinCollection.GetSkin(SkinType.Character, 2);
                    if (character != null)
                    {
                        __instance.americaOriginalMaterial.mainTexture = character.texture;
                    }
                    character = skinCollection.GetSkin(SkinType.Character, 3);
                    if (character != null)
                    {
                        __instance.americaHurtMaterial.mainTexture = character.texture;
                    }

                    Skin gun = skinCollection.GetSkin(SkinType.Gun, 0);
                    if (gun != null)
                    {
                        Traverse.Create(__instance).Field("originalGunMaterial").GetValue<Material>().mainTexture = gun.texture;
                    }
                    gun = skinCollection.GetSkin(SkinType.Gun, 1);
                    if (gun != null)
                    {
                        __instance.hurtGunMaterial.mainTexture = gun.texture;
                    }
                    gun = skinCollection.GetSkin(SkinType.Gun, 2);
                    if (gun != null)
                    {
                        __instance.americaOriginalGunMaterial.mainTexture = gun.texture;
                    }
                    gun = skinCollection.GetSkin(SkinType.Gun, 3);
                    if (gun != null)
                    {
                        __instance.americaHurtGunMaterial.mainTexture = gun.texture;
                    }
                }
            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
        }
    }
}
