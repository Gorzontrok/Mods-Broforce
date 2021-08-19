using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace Surprise
{
    // Patch the basic mook
    [HarmonyPatch(typeof(Mook), "Update")]
    static class MookUpdate_Patch
    {
        static void Postfix(Mook __instance)
        {

            if (Main.HardMode)
            {
                __instance.canBeAssasinated = false;
                __instance.willPanicWhenOnFire = false;
                __instance.canBeCoveredInAcid = false;
            }
        }
    }

    //Patch suicide mook
    [HarmonyPatch(typeof(MookSuicide), "Update")]
    static class MookSuicideUpdate_Patch
    {
        static void Postfix(MookSuicide __instance)
        {
            __instance.range = 100f;
            __instance.blastForce = 100f;

            __instance.speed = 150f;
            if (Main.HardMode)
            {
                __instance.canBeAssasinated = false;
                __instance.willPanicWhenOnFire = false;
                __instance.canBeCoveredInAcid = false;
            }
        }
    }

    //Patch dog
    [HarmonyPatch(typeof(MookDog), "Update")]
    static class MookDogUpdate_Patch
    {
        public static Material normalDog;
        static void Postfix(MookDog __instance)
        {
            __instance.awareMegaRunSpeed = 175f;
            __instance.awareRunSpeed = 150f;

            if (!__instance.isMegaDog)
                normalDog = __instance.GetComponent<Renderer>().sharedMaterial;
            if (Main.HardMode)
            {
                __instance.canBeAssasinated = false;
                __instance.willPanicWhenOnFire = false;
                __instance.canBeCoveredInAcid = false;
            }
        }
    }

    //Patch BigGuy
    [HarmonyPatch(typeof(MookBigGuy), "Update")]
    static class MookBigGuyUpdate_Patch
    {
        static void Postfix(MookBigGuy __instance)
        {
            //__instance.health = 75;
            __instance.willPanicWhenOnFire = false;
            __instance.canBeCoveredInAcid = false;

            __instance.canBeRevived = false;
            Traverse.Create(typeof(MookBigGuy)).Field("hurtStumbleFireDelay").SetValue(0);

            if (Main.HardMode)
            {
                //__instance.health = 85;
                __instance.canBeAssasinated = false;
                __instance.canBeCoveredInAcid = false;
            }
        }
    }

    // Patch the grenadier mook
    [HarmonyPatch(typeof(MookGrenadier), "Update")]
    static class GrenadierMookUpdate_Patch
    {
        static void Postfix(MookGrenadier __instance)
        {
            if (Main.HardMode)
            {
                __instance.canBeAssasinated = false;
                __instance.willPanicWhenOnFire = false;
                __instance.canBeCoveredInAcid = false;
            }
        }
    }

    // Patch Satan
    [HarmonyPatch(typeof(Satan), "Start")]
    static class SatanStart_Patch
    {
        static void Postfix(Satan __instance)
        {
            __instance.health = 300000;
            __instance.canBeAssasinated = false;
            __instance.willPanicWhenOnFire = false;
            __instance.canBeCoveredInAcid = false;
            __instance.canBeRevived = false;
            __instance.canBeStrungUp = false;
            __instance.canBeSetOnFire = false;
            __instance.canBeTearGased = false;

            __instance.speed = 150f;
            __instance.bloodCountAmount = 150;
            __instance.immuneToPlasmaShock = true;
            __instance.showElectrifiedFrames = false;
            __instance.blindTime = 0f;
        }
    }

    // Patch drill carrier
    [HarmonyPatch(typeof(DrillCarrier), "Start")]
    static class DrillCarrierStart_Patch
    {
        static void Postfix(DrillCarrier __instance)
        {
            __instance.health = 100;
            __instance.tankSpeed = 150;
            __instance.mooksToSpawn = 15;
        }
    }

    // Patch MookTruck
    [HarmonyPatch(typeof(MookTruck), "Start")]
    static class MookTruckStart_Patch
    {
        static void Postfix(MookTruck __instance)
        {
            __instance.health = 100;
            __instance.tankSpeed = 150;
            __instance.mooksToSpawn = 15;
            Traverse.Create(typeof(MookTruck)).Field("fireDelay").SetValue(0f);
        }
    }

    // Patch MookDoor
    [HarmonyPatch(typeof(MookDoor), "Start")]
    static class MookDoorStart_Patch
    {
        static void Postfix(MookDoor __instance)
        {
            __instance.maxMookCount = 5;
            __instance.maxMooksOnCollapse = 15;
            __instance.maxAlarmedMooksCount = 10;

            if(Main.HardMode)
            {
                __instance.maxMooksOnCollapse = 20;
                __instance.maxMookCount = 7;
                __instance.spawningRate = 0.5f;
            }
        }
    }

    // Patch MookDoorSliding
    [HarmonyPatch(typeof(MookDoorSliding), "Start")]
    static class MookDoorSlidingStart_Patch
    {
        static void Postfix(MookDoorSliding __instance)
        {
            __instance.maxMookCount = 5;
            __instance.maxMooksOnCollapse = 15;
            __instance.maxAlarmedMooksCount = 10;

            if (Main.HardMode)
            {
                __instance.maxMooksOnCollapse = 20;
                __instance.maxMookCount = 7;
                __instance.spawningRate = 0.5f;
            }
        }
    }

    // Patch MookRiotShield
    [HarmonyPatch(typeof(MookRiotShield), "Update")]
    static class MookRiotShield_Update_Patch
    {
        static void Postfix(MookRiotShield __instance)
        {
            if (Main.HardMode)
            {
                __instance.canBeAssasinated = false;
                __instance.willPanicWhenOnFire = false;
                __instance.canBeCoveredInAcid = false;
            }
        }
    }

    //Patch ScoutMook
    [HarmonyPatch(typeof(ScoutMook),"Update")]
    static class ScoutMook_Update_Patch
    {
        static void Postfix(ScoutMook __instance)
        {
            __instance.runSpeed = 175f;
        }
    }

    [HarmonyPatch(typeof(ScoutMook), "Awake")]
    static class ScoutMook_Awake_Patch
    {
        static void Postfix(ScoutMook __instance)
        {
            __instance.runSpeed = 175f;
            __instance.health = 20;
            __instance.hearingRangeX = 450;
            __instance.immuneToPlasmaShock = true;

            if(Main.HardMode)
            {
                __instance.hearingRangeX = 500f;
                __instance.hearingRangeY = 250f;
                __instance.canBeAssasinated = false;
                __instance.willPanicWhenOnFire = false;
                __instance.canBeCoveredInAcid = false;
            }
        }
    }
}
