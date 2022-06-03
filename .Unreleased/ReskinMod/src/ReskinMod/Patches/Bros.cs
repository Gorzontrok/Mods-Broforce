using System;
using UnityEngine;
using HarmonyLib;
using ReskinMod.Skins;

namespace ReskinMod.Patches.Bros
{
    [HarmonyPatch(typeof(Brominator), "Start")]
    static class Brominator_Reskin_Patch
    {
        static void Postfix(Brominator __instance)
        {
            try
            {
                SkinCollection skinCollection = SkinCollection.GetSkinCollection(__instance.GetType().Name.ToLower());
                if (skinCollection != null)
                {
                    Skin characterSkin = skinCollection.GetSkin(SkinType.Character, 0);
                    Skin character2 = skinCollection.GetSkin(SkinType.Character, 1);

                    Skin avatarSkin = skinCollection.GetSkin(SkinType.Avatar, 0);
                    Skin avatar2 = skinCollection.GetSkin(SkinType.Avatar, 1);

                    Skin gun = skinCollection.GetSkin(SkinType.Gun, 0);
                    Skin gun2 = skinCollection.GetSkin(SkinType.Gun, 1);
                    if(characterSkin != null)
                    {
                        __instance.humanBrominator.mainTexture = character2.texture;
                    }
                    if (character2 != null)
                    {
                        __instance.metalBrominator.mainTexture = character2.texture;
                    }
                    if (avatarSkin != null)
                    {
                        __instance.brominatorHumanAvatar.mainTexture = avatarSkin.texture;
                    }
                    if (avatar2 != null)
                    {
                        __instance.brominatorRobotAvatar.mainTexture = avatar2.texture;
                    }
                    if (gun != null)
                    {
                        __instance.humanGunBrominator.mainTexture = gun.texture;
                        __instance.humanGunBrominator.SetTexture("_MainTex", gun.texture);
                    }
                    if (gun2 != null)
                    {
                        __instance.metalGunBrominator.mainTexture = gun2.texture;
                    }
                }
            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
        }
    }
    [HarmonyPatch(typeof(Nebro), "Start")]
    static class Nebro_Reskin_Patch
    {
        static void Postfix(Nebro __instance)
        {
            try
            {
                SkinCollection skinCollection = SkinCollection.GetSkinCollection(__instance.GetType().Name.ToLower());
                if (skinCollection != null)
                {
                    Skin characterSkin = skinCollection.GetSkin(SkinType.Character, 0);
                    Skin armless = skinCollection.GetSkin(SkinType.Armless, 0);
                    if(armless != null)
                    {
                        __instance.materialArmless.mainTexture = armless.texture;
                    }
                    Traverse.Create(__instance).Field("materialNormal").GetValue<Material>().mainTexture = characterSkin.texture;
                }
            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
        }
    }
    [HarmonyPatch(typeof(AshBrolliams), "Start")]
    static class AshBrolliams_Reskin_Patch
    {
        static void Postfix(AshBrolliams __instance)
        {
            try
            {
                SkinCollection skinCollection = SkinCollection.GetSkinCollection(__instance.GetType().Name.ToLower());
                if (skinCollection != null)
                {
                    Skin avatar2 = skinCollection.GetSkin(SkinType.Avatar, 1);
                    if(avatar2 != null)
                    {
                        __instance.bloodyAvatar.mainTexture = avatar2.texture;
                    }
                }
            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
        }
    }
    [HarmonyPatch(typeof(BroveHeart), "Awake")]
    static class BroveHeart_Reskin_Patch
    {
        static void Postfix(BroveHeart __instance)
        {
            try
            {
                SkinCollection skinCollection = SkinCollection.GetSkinCollection(__instance.GetType().Name.ToLower());
                if (skinCollection != null)
                {
                    Skin character = skinCollection.GetSkin(SkinType.Character, 0);
                    Skin character2 = skinCollection.GetSkin(SkinType.Character, 1);
                    if (character != null)
                    {
                        __instance.defaultGunMaterial.mainTexture = character.texture;
                    }
                    if (character2 != null)
                    {
                        __instance.disarmedGunMaterial.mainTexture = character2.texture;
                    }
                }
            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
        }
    }

    [HarmonyPatch(typeof(BoondockBro), "Awake")]
    static class BoondockBro_Leader_Reskin_Patch
    {
        static void Postfix(BoondockBro __instance)
        {
            try
            {
                SkinCollection skinCollection = SkinCollection.GetSkinCollection(__instance.GetType().Name.ToLower());
                if (skinCollection != null)
                {
                    if (__instance.isLeadBro)
                    {
                        Skin character = skinCollection.GetSkin(SkinType.Character, 0);
                        Skin character2 = skinCollection.GetSkin(SkinType.Character, 1);
                        if (character != null)
                        {
                            __instance.gameObject.GetComponent<SpriteSM>().meshRender.sharedMaterial.SetTexture("_MainTex", character.texture);
                        }
                        if (character2 != null)
                        {
                            __instance.secondBroMaterial.mainTexture = character2.texture;
                        }
                        Skin gun = skinCollection.GetSkin(SkinType.Gun, 0);
                        if (gun != null)
                        {
                            __instance.gunSprite.GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex", gun.texture);
                        }
                    }

                    Skin avatar = skinCollection.GetSkin(SkinType.Avatar, 0);
                    Skin avatar2 = skinCollection.GetSkin(SkinType.Avatar, 1);
                    if(avatar != null)
                    {
                        __instance.avatar1.mainTexture = avatar.texture;
                    }
                    if (avatar2 != null)
                    {
                        __instance.avatar2.mainTexture = avatar2.texture;
                    }
                }
            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
        }
    }
    [HarmonyPatch(typeof(BoondockBro), "SetUpTrailingBro")]
    static class BoondockBro_TrailingBro_Reskin_Patch
    {
        static void Postfix(BoondockBro __instance)
        {
            SkinCollection skinCollection = SkinCollection.GetSkinCollection(__instance.GetType().Name.ToLower());
            if (skinCollection != null)
            {
                Skin gun2 = skinCollection.GetSkin(SkinType.Gun, 1);
                if (gun2 != null)
                {
                    __instance.gunSprite.GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex", gun2.texture);
                }
            }
        }
    }
}
