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
                SkinCollection skinCollection = SkinCollection.GetSkinCollection(__instance.GetType().Name.ToLower());
                if (skinCollection != null)
                {
                    Skin characterSkin = skinCollection.GetSkin(Skin.SkinType.Character);
                    Skin character2 = skinCollection.GetSkin(Skin.SkinType.Character2);

                    Skin avatarSkin = skinCollection.GetSkin(Skin.SkinType.Avatar);
                    Skin avatar2 = skinCollection.GetSkin(Skin.SkinType.Avatar2);

                    Skin gun = skinCollection.GetSkin(Skin.SkinType.Gun);
                    Skin gun2 = skinCollection.GetSkin(Skin.SkinType.Gun2);
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
                    Skin characterSkin = skinCollection.GetSkin(Skin.SkinType.Character);
                    Skin armless = skinCollection.GetSkin(Skin.SkinType.Armless);
                    if(armless != null)
                    {
                        __instance.materialArmless.mainTexture = armless.texture;
                    }

                    Material materialNormal = Material.Instantiate(__instance.materialArmless);
                    materialNormal.mainTexture = characterSkin.texture;
                    Traverse.Create(__instance).Field("materialNormal").SetValue(materialNormal);
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
                    Skin avatar2 = skinCollection.GetSkin(Skin.SkinType.Avatar2);
                    if(avatar2 != null)
                    {
                        __instance.bloodyAvatar.mainTexture = avatar2.texture;
                    }
                }
            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
        }
    }
}
