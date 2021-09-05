using System;
using UnityEngine;
using HarmonyLib;

namespace TweaksFromPigs
{
    // MookSuicide patch
    [HarmonyPatch(typeof(MookSuicide), "Start")]
    static class MookSuicide_Patch
    {
        static void Postfix(MookSuicide __instance)
        {
            if (Main.settings.TeargasEveryone) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }
    [HarmonyPatch(typeof(SachelPack), "StickToUnit")]
    static class SuicideMookDontPanicWhenHaveDynamiteOnThem_Patch
    {
        static bool Prefix(SachelPack __instance, Unit unit, Vector3 stucklocalPos)
        {
            if(Main.settings.SuicideDontPanicWithDynamiteOnThem)
            {
                Traverse instTrav = Traverse.Create(__instance);
                if (unit == null)
                {
                    return false;
                }
                instTrav.Field("stuckToUnit").SetValue(unit);
                instTrav.Field("stuckTolocalPos").SetValue(stucklocalPos);
                if (unit.enemyAI != null)
                {

                    if (unit.enemyAI.IsAlerted() && unit.GetMookType() != (MookType.Suicide | MookType.UndeadSuicide))
                    {
                        instTrav.Field("stuckToUnit").GetValue<Unit>().Panic((int)Mathf.Sign(__instance.xI), __instance.life + 0.2f, false);
                    }
                    else
                    {
                        PolymorphicAI enemyAI = instTrav.Field("stuckToUnit").GetValue<Unit>().enemyAI;
                        Traverse.Create(enemyAI).Field("ShowQuestionBubble").GetValue();
                    }
                }
                return false;
            }
            return true;
        }
    }

    // Patch MookBigGuy
    [HarmonyPatch(typeof(MookBigGuy), "Awake")]
    static class MookBigGuy_Patch
    {
        static void Postfix(MookBigGuy __instance)
        {
            if (Main.settings.TeargasEveryone) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }

    // Patch mook dog
    [HarmonyPatch(typeof(MookDog), "Update")]
    static class MookDog_Patch
    {
        static void Postfix(MookDog __instance)
        {
            if (Main.settings.TeargasEveryone) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }

    // Patch Grenadier
    [HarmonyPatch(typeof(MookGrenadier), "Start")]
    static class MookGrenadier_Patch
    {
        static void Postfix(MookGrenadier __instance)
        {
            if (Main.settings.TeargasEveryone) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }

    // Patch MookJetpack
    [HarmonyPatch(typeof(MookJetpack), "Start")]
    static class MookJetpack_Patch
    {
        static void Postfix(MookJetpack __instance)
        {
            if (Main.settings.TeargasEveryone) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }

    //Patch Ninja
    [HarmonyPatch(typeof(MookNinja), "Start")]
    static class MookNinja_Patch
    {
        static void Postfix(MookNinja __instance)
        {
            if (Main.settings.TeargasEveryone) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }

    // Patch Shield
    [HarmonyPatch(typeof(MookRiotShield), "Update")]
    static class MookRiotShield_Patch
    {
        static void Postfix(MookRiotShield __instance)
        {
            if (Main.settings.TeargasEveryone) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }

    // patch trooper
    [HarmonyPatch(typeof(MookTrooper), "Start")]
    static class MookTrooper_Patch
    {
        static void Postfix(MookTrooper __instance)
        {
            if (Main.settings.TeargasEveryone) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }

    // Patch scout
    [HarmonyPatch(typeof(ScoutMook), "Awake")]
    static class ScoutMook_Patch
    {
        static void Postfix(ScoutMook __instance)
        {
            if (Main.settings.TeargasEveryone) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }

    // Patch general
    [HarmonyPatch(typeof(MookGeneral), "IsEvil")]
    static class MookGeneral_Patch
    {
        static void Postfix(MookGeneral __instance)
        {
            if (Main.settings.TeargasEveryone) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }

    // Patch alien
    [HarmonyPatch(typeof(Alien), "Start")]
    static class Alien_Patch
    {
        static void Postfix(Alien __instance)
        {
            if (Main.settings.TeargasEveryone) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }

    // Patch facehugger
    [HarmonyPatch(typeof(AlienFaceHugger), "Start")]
    static class AlienFaceHugger_Patch
    {
        static void Postfix(AlienFaceHugger __instance)
        {
            if (Main.settings.TeargasEveryone) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }

    // patch Melter
    [HarmonyPatch(typeof(AlienMelter), "Start")]
    static class AlienMelter_Patch
    {
        static void Postfix(AlienMelter __instance)
        {
            if (Main.settings.TeargasEveryone) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }

    // patch Mosquitto
    [HarmonyPatch(typeof(AlienMosquito), "Start")]
    static class AlienMosquito_Patch
    {
        static void Postfix(AlienMosquito __instance)
        {
            if (Main.settings.TeargasEveryone) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }

    // Patch xenomorphe
    [HarmonyPatch(typeof(AlienXenomorph), "Start")]
    static class AlienXenomorph_Patch
    {
        static void Postfix(AlienXenomorph __instance)
        {
            if (Main.settings.TeargasEveryone) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }

    //Patch MookSuicideUndead
    [HarmonyPatch(typeof(MookSuicideUndead), "Start")]
    static class MookSuicideUndead_Patch
    {
        static void Postfix(MookSuicideUndead __instance)
        {
            if (Main.settings.TeargasEveryone) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }

    // Patch Hell dog
    [HarmonyPatch(typeof(HellDog), "Awake")]
    static class HellDog_Patch
    {
        static void Postfix(HellDog __instance)
        {
            if (Main.settings.TeargasEveryone) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }

    // Patch undead trooper
    [HarmonyPatch(typeof(UndeadTrooper), "Start")]
    static class UndeadTrooper_Patch
    {
        static void Postfix(UndeadTrooper __instance)
        {
            if (Main.settings.TeargasEveryone) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }
}
