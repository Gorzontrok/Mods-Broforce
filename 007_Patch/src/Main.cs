using System;
using System.Reflection;
using HarmonyLib;
using UnityModManagerNet;

namespace _007_Patch
{
    static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnToggle = OnToggle;
            var harmony = new Harmony(modEntry.Info.Id);
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);
            }
            catch (Exception ex)
            {
                mod.Logger.Log(ex.ToString());
            }

            mod = modEntry;

            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }

        public static void Log(object str)
        {
            mod.Logger.Log(str.ToString());
        }
    }

    [HarmonyPatch(typeof(DoubleBroSeven), "Awake")]
    static class AddMoreSpecial_Patch
    {
        static void Postfix(DoubleBroSeven __instance)
        {
            if (!Main.enabled) return;
            Traverse.Create(typeof(DoubleBroSeven)).Field("_specialAmmo").SetValue(5);
            __instance.SpecialAmmo = 5;
            __instance.originalSpecialAmmo = 5;
        }
    }
    /*[HarmonyPatch(typeof(DoubleBroSeven), "Update")]
    static class TearGas_Patch
    {
        static void Postfix(DoubleBroSeven __instance)
        {
            if (__instance.health > 0)
            {
                if (!Main.usedTeargas)
                {
                    Main.usedTeargas = true;
                    Traverse.Create(typeof(DoubleBroSeven)).Field("_specialAmmo").SetValue(5);
                    __instance.SpecialAmmo = 5;
                }
            }
        }
    }*/

    // Mook patch
    [HarmonyPatch(typeof(MookSuicide), "Start")]
    static class MookSuicide_Patch
    {
        static void Postfix(MookSuicide __instance)
        {
            __instance.canBeTearGased = true;
        }
    }
    [HarmonyPatch(typeof(MookBigGuy), "Awake")]
    static class MookBigGuy_Patch
    {
        static void Postfix(MookBigGuy __instance)
        {
            __instance.canBeTearGased = true;
        }
    }
    [HarmonyPatch(typeof(MookDog), "Update")]
    static class MookDog_Patch
    {
        static void Postfix(MookDog __instance)
        {
            __instance.canBeTearGased = true;
        }
    }
    [HarmonyPatch(typeof(MookGrenadier), "Start")]
    static class MookGrenadier_Patch
    {
        static void Postfix(MookGrenadier __instance)
        {
            __instance.canBeTearGased = true;
        }
    }
    [HarmonyPatch(typeof(MookJetpack), "Start")]
    static class MookJetpack_Patch
    {
        static void Postfix(MookJetpack __instance)
        {
            __instance.canBeTearGased = true;
        }
    }
    [HarmonyPatch(typeof(MookNinja), "Start")]
    static class MookNinja_Patch
    {
        static void Postfix(MookNinja __instance)
        {
            __instance.canBeTearGased = true;
        }
    }
    [HarmonyPatch(typeof(MookRiotShield), "Update")]
    static class MookRiotShield_Patch
    {
        static void Postfix(MookRiotShield __instance)
        {
            __instance.canBeTearGased = true;
        }
    }
    [HarmonyPatch(typeof(MookTrooper), "Start")]
    static class MookTrooper_Patch
    {
        static void Postfix(MookTrooper __instance)
        {
            __instance.canBeTearGased = true;
        }
    }
    [HarmonyPatch(typeof(ScoutMook), "Awake")]
    static class ScoutMook_Patch
    {
        static void Postfix(ScoutMook __instance)
        {
            __instance.canBeTearGased = true;
        }
    }
    [HarmonyPatch(typeof(MookGeneral), "IsEvil")]
    static class MookGeneral_Patch
    {
        static void Postfix(MookGeneral __instance)
        {
            __instance.canBeTearGased = true;
        }
    }

    // Patch alien
    [HarmonyPatch(typeof(Alien), "Start")]
    static class Alien_Patch
    {
        static void Postfix(Alien __instance)
        {
            __instance.canBeTearGased = true;
        }
    }
    [HarmonyPatch(typeof(AlienFaceHugger), "Start")]
    static class AlienFaceHugger_Patch
    {
        static void Postfix(AlienFaceHugger __instance)
        {
            __instance.canBeTearGased = true;
        }
    }
    [HarmonyPatch(typeof(AlienMelter), "Start")]
    static class AlienMelter_Patch
    {
        static void Postfix(AlienMelter __instance)
        {
            __instance.canBeTearGased = true;
        }
    }
    [HarmonyPatch(typeof(AlienMosquito), "Start")]
    static class AlienMosquito_Patch
    {
        static void Postfix(AlienMosquito __instance)
        {
            __instance.canBeTearGased = true;
        }
    }
    [HarmonyPatch(typeof(AlienXenomorph), "Start")]
    static class AlienXenomorph_Patch
    {
        static void Postfix(AlienXenomorph __instance)
        {
            __instance.canBeTearGased = true;
        }
    }

    //Patch Hell
    [HarmonyPatch(typeof(MookSuicideUndead), "Start")]
    static class MookSuicideUndead_Patch
    {
        static void Postfix(MookSuicideUndead __instance)
        {
            __instance.canBeTearGased = true;
        }
    }
    [HarmonyPatch(typeof(HellDog), "Awake")]
    static class HellDog_Patch
    {
        static void Postfix(HellDog __instance)
        {
            __instance.canBeTearGased = true;
        }
    }
    [HarmonyPatch(typeof(UndeadTrooper), "Start")]
    static class UndeadTrooper_Patch
    {
        static void Postfix(UndeadTrooper __instance)
        {
            __instance.canBeTearGased = true;
        }
    }

}
