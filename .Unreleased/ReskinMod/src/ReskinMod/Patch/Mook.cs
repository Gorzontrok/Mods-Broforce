using System;
using HarmonyLib;

namespace ReskinMod.Patch
{
    [HarmonyPatch(typeof(MookTrooper), "Awake")]
    static class MookTrooper_reskin_Patch
    {
        static void Postfix(MookTrooper __instance)
        {
            try
            {
                var cInfo = Utility.GetMookReskinInfo(__instance);
                cInfo.CheckTex(__instance);

                MookTrooper mook = __instance;
                if (__instance as MookBazooka)
                {
                    mook = __instance as MookBazooka;
                }
                mook.decapitatedMaterial.mainTexture = cInfo.GetDecapitated();
            }
            catch (Exception ex) { Main.bmod.ExceptionLog(ex); }
        }
    }
    [HarmonyPatch(typeof(MookJetpack), "Start")]
    static class MookJetPack_Reskin_Patch
    {
        static void Prefix(MookJetpack __instance)
        {
            try
            {
                var cInfo = Utility.GetMookReskinInfo(__instance);
                cInfo.CheckTex(__instance);

                __instance.decapitatedMaterial.mainTexture = cInfo.GetDecapitated();
            }
            catch (Exception ex) { Main.bmod.ExceptionLog(ex); }
        }
    }
    [HarmonyPatch(typeof(UndeadTrooper), "Awake")]
    static class UndeadTrooper_Reskin_Patch
    {
        static void Postfix(UndeadTrooper __instance)
        {
            try
            {
                var cInfo = Utility.GetMookReskinInfo(__instance);
                cInfo.CheckTex(__instance);

                __instance.decapitatedMaterial.mainTexture = cInfo.GetDecapitated();
            }
            catch (Exception ex) { Main.bmod.ExceptionLog(ex); }
        }
    }
    [HarmonyPatch(typeof(MookSuicide), "Start")]
    static class MookSuicide_Reskin_Patch
    {
        static void Prefix(MookSuicide __instance)
        {
            try
            {
                var cInfo = Utility.GetMookReskinInfo(__instance);
                cInfo.CheckTex(__instance);
                cInfo.CheckSecondTex(__instance.warningMaterial);

                __instance.decapitatedMaterial.mainTexture = cInfo.GetDecapitated();
                __instance.warningMaterial.mainTexture = cInfo.GetCharacterTex2(__instance.warningMaterial);
            }
            catch (Exception ex) { Main.bmod.ExceptionLog(ex); }
        }
    }
    [HarmonyPatch(typeof(MookBigGuy), "Awake")]
    static class BigGuy_Reskin_Patch
    {
        static void Postfix(MookBigGuy __instance)
        {
            try
            {
                var cInfo = Utility.GetMookReskinInfo(__instance);
                cInfo.CheckTex(__instance);

                MookBigGuy mook = __instance;
                if (__instance as MookHellBigGuy)
                {
                    mook = __instance as MookHellBigGuy;
                }
                else if (__instance as MookHellArmouredBigGuy)
                {
                    mook = __instance as MookHellArmouredBigGuy;
                }
                mook.decapitatedMaterial.mainTexture = cInfo.GetDecapitated();
            }
            catch (Exception ex) { Main.bmod.ExceptionLog(ex); }
        }
    }

    [HarmonyPatch(typeof(MookArmouredGuy), "Awake")]
    static class MookArmouredGuy_Reskin_Patch
    {
        static void Postfix(MookArmouredGuy __instance)
        {
            try
            {
                var cInfo = Utility.GetMookReskinInfo(__instance);
                cInfo.CheckTex(__instance);

                __instance.decapitatedMaterial.mainTexture = cInfo.GetDecapitated();
            }
            catch (Exception ex) { Main.bmod.ExceptionLog(ex); }
        }
    }
    [HarmonyPatch(typeof(SkinnedMook), "Start")]
    static class SkinnedMook_Reskin_Patch
    {
        static void Prefix(SkinnedMook __instance)
        {
            try
            {
                var cInfo = Utility.GetMookReskinInfo(__instance);
                cInfo.CheckTex(__instance);
                //cInfo.CheckSecondTex(__instance.warningMaterial); = null

                __instance.decapitatedMaterial.mainTexture = cInfo.GetDecapitated();
                //__instance.warningMaterial.mainTexture = cInfo.GetCharacterTex2(__instance.warningMaterial);
            }
            catch (Exception ex) { Main.bmod.ExceptionLog(ex); }
        }
    }

    [HarmonyPatch(typeof(MookHellBoomer), "Awake")]
    static class MookHellBoomer_Reskin_Patch
    {
        static void Postfix(MookHellBoomer __instance)
        {
            try
            {
                var cInfo = Utility.GetMookReskinInfo(__instance);
                cInfo.CheckTex(__instance);
                cInfo.CheckSecondTex(__instance.warningMaterial);

                MookHellBoomer mook = __instance;
                if (__instance as MookHellSoulCatcher)
                {
                    mook = __instance as MookHellSoulCatcher;
                }
                mook.warningMaterial.mainTexture = cInfo.GetCharacterTex2(mook.warningMaterial);
            }
            catch (Exception ex) { Main.bmod.ExceptionLog(ex); }
        }
    }

    [HarmonyPatch(typeof(MookSuicideUndead), "Start")]
    static class MookSuicideUndead_Reskin_Patch
    {
        static void Prefix(MookSuicideUndead __instance)
        {
            try
            {
                var cInfo = Utility.GetMookReskinInfo(__instance);
                cInfo.CheckTex(__instance);
                cInfo.CheckSecondTex(__instance.warningMaterial);
                __instance.warningMaterial.mainTexture = cInfo.GetCharacterTex2(__instance.warningMaterial);
            }
            catch (Exception ex) { Main.bmod.ExceptionLog(ex); }
        }
    }

    [HarmonyPatch(typeof(ScoutMook), "Awake")]
    static class ScoutMook_Reskin_Patch
    {
        static void Postfix(ScoutMook __instance)
        {
            try
            {
                var cInfo = Utility.GetMookReskinInfo(__instance);
                cInfo.CheckTex(__instance);
                cInfo.CheckSecondTex(__instance.disarmedMaterial/*, __instance.disarmedGunMaterial*/);

                __instance.disarmedMaterial.mainTexture = cInfo.GetCharacterTex2(__instance.disarmedMaterial);
                //__instance.disarmedGunMaterial.mainTexture = cInfo.GetGunTex2(__instance.disarmedGunMaterial);
            }
            catch (Exception ex) { Main.bmod.ExceptionLog(ex); }
        }
    }
}
