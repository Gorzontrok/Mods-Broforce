using System;
using HarmonyLib;

namespace ReskinMod.Patch
{
    /*
     SkinCollection skinCollection = SkinCollection.GetSkinCollection(__instance.GetType().Name.ToLower());
     if(skinCollection != null)
     {
         Skin decapitated = skinCollection.GetSkin(Skin.SkinType.Decapitated);
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
                    Skin decapitated = skinCollection.GetSkin(Skin.SkinType.Decapitated);
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
                    Skin decapitated = skinCollection.GetSkin(Skin.SkinType.Decapitated);
                    Skin char2 = skinCollection.GetSkin(Skin.SkinType.Character2);
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
                    Skin decapitated = skinCollection.GetSkin(Skin.SkinType.Decapitated);
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
                    Skin decapitated = skinCollection.GetSkin(Skin.SkinType.Decapitated);
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
                    Skin character2 = skinCollection.GetSkin(Skin.SkinType.Character2);
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
                    Skin character2 = skinCollection.GetSkin(Skin.SkinType.Character2);
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
                    Skin character2 = skinCollection.GetSkin(Skin.SkinType.Character2);
                    if (character2 != null)
                    {
                        __instance.disarmedMaterial.mainTexture = character2.texture;
                    }
                }
            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
        }
    }
}
