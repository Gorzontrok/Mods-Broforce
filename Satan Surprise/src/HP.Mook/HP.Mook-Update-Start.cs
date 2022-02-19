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

            if (Main.HardMode && Main.enabled)
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
            if (!Main.enabled) return;
            __instance.range = 60;
            __instance.blastForce = 100f;

            __instance.speed = 150f;
            Main.NewMookForMecha = __instance;
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
            if (!Main.enabled) return;
            __instance.awareMegaRunSpeed = 190f;
            __instance.awareRunSpeed = 175;

            if (!__instance.isMegaDog)
                normalDog = __instance.GetComponent<Renderer>().sharedMaterial;

            if (Main.HardMode)
            {
                __instance.canBeAssasinated = false;
                __instance.willPanicWhenOnFire = false;
                __instance.canBeCoveredInAcid = false;

                if(__instance.isMegaDog)
                {
                    /*Traverse.Create(typeof(MookDog)).Field("spritePixelWidth").SetValue(32);
                    Traverse.Create(typeof(MookDog)).Field("spritePixelHeight").SetValue(32);
                    Traverse.Create(typeof(MookDog)).Field("halfWidth").SetValue(6);
                    Traverse.Create(typeof(MookDog)).Field("feetWidth").SetValue(4);
                    Traverse.Create(typeof(MookDog)).Field("standingHeadHeight").SetValue(18);
                    SpriteSM sprite = Traverse.Create(typeof(MookDog)).Field("sprite").GetValue() as SpriteSM;
                    Vector3 spriteOffset = Traverse.Create(typeof(MookDog)).Field("spriteOffset").GetValue<Vector3>();

                    int spritePixelWidth = Traverse.Create(typeof(MookDog)).Field("spritePixelWidth").GetValue<int>();
                    int spritePixelHeight = Traverse.Create(typeof(MookDog)).Field("spritePixelHeight").GetValue<int>();

                    sprite.RecalcTexture();
                    sprite.SetPixelDimensions(32, 32);
                    sprite.SetSize(32, 32);
                    spriteOffset.y = sprite.offset.y;
                    sprite.SetLowerLeftPixel((float)(15 * spritePixelWidth), (float)(spritePixelHeight * 2));*/
                }

            }
        }
    }

    //Patch BigGuy
    [HarmonyPatch(typeof(MookBigGuy), "Update")]
    static class MookBigGuyUpdate_Patch
    {
        static void Postfix(MookBigGuy __instance)
        {
            if (!Main.enabled) return;
            //__instance.health = 75;
            __instance.willPanicWhenOnFire = false;
            __instance.canBeCoveredInAcid = false;

            __instance.canBeRevived = false;
            Traverse.Create(typeof(MookBigGuy)).Field("hurtStumbleFireDelay").SetValue(0);

            if (Main.HardMode)
            {
                //__instance.health = 85;
                __instance.immuneToPlasmaShock = true;
                __instance.showElectrifiedFrames = false;
                __instance.canBeAssasinated = false;
            }
        }
    }

    // Patch the grenadier mook
    [HarmonyPatch(typeof(MookGrenadier), "Update")]
    static class GrenadierMookUpdate_Patch
    {
        static void Postfix(MookGrenadier __instance)
        {
            if (!Main.enabled) return;
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
            if (!Main.enabled) return;
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
            if (!Main.enabled) return;
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
            if (!Main.enabled) return;
            __instance.health = 100;
            __instance.tankSpeed = 150;
            __instance.mooksToSpawn = 15;
            Traverse.Create(typeof(MookTruck)).Field("fireDelay").SetValue(0f);
            if(Main.HardMode)
            {
                __instance.mooksToSpawn = 25;
            }
        }
    }

    // Patch MookDoor
    [HarmonyPatch(typeof(MookDoor), "Start")]
    static class MookDoorStart_Patch
    {
        static void Postfix(MookDoor __instance)
        {
            if (!Main.enabled) return;
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
            if (!Main.enabled) return;
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
            if (Main.HardMode && Main.enabled)
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
            if (!Main.enabled) return;
            __instance.runSpeed = 175f;
        }
    }
    [HarmonyPatch(typeof(ScoutMook), "Awake")]
    static class ScoutMook_Awake_Patch
    {
        static void Postfix(ScoutMook __instance)
        {
            if (!Main.enabled) return;
            __instance.runSpeed = 175f;
            __instance.health = 10;
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

    // Patch MookJetpack
    [HarmonyPatch(typeof(MookJetpack), "Start")]
    static class MookJetpack_Start_Patch
    {
        static void Postfix(MookJetpack __instance)
        {
            if (!Main.enabled) return;
            __instance.speed = 80;
            __instance.fireRate = 0.05f;
            __instance.explosionRange = 70;
            __instance.blastForce = 40f;

            if (Main.HardMode)
            {
                __instance.health = 5;
                __instance.canBeAssasinated = false;
                __instance.willPanicWhenOnFire = false;
                __instance.canBeCoveredInAcid = false;
            }
        }
    }

    // Patch CR-666
    [HarmonyPatch(typeof(DolphLundrenSoldier), "Start")]
    static class DolphLundrenSoldier_Start_Patch
    {
        static void Postfix(DolphLundrenSoldier __instance)
        {
            if (!Main.enabled) return;
            __instance.health = 200;
            __instance.healthReviveAmount = 200;
            __instance.speed = 110;
            __instance.willPanicWhenOnFire = false;
            __instance.immuneToPlasmaShock = true;
            __instance.showElectrifiedFrames = false;

            if (Main.HardMode)
            {
                __instance.health = 300;
                __instance.healthReviveAmount = 300;
                __instance.canBeAssasinated = false;
                __instance.canBeCoveredInAcid = false;
            }
        }
    }

    //Patch Mookopter
    [HarmonyPatch(typeof(Mookopter), "Awake")]
    static class Mookopter_Awake_Patch
    {
        static MookopterPolyAI mkpAI;
        static void Postfix(Mookopter __instance)
        {
            if (!Main.enabled) return;
            __instance.health = 300;
            __instance.verticalSpeed = 200;
            __instance.tankSpeed = 150;

            mkpAI.attackTime = 5;
            if (Main.HardMode)
            {
                __instance.health = 400;
                mkpAI.maxIdleWaitDuration = 0.5f;
            }
        }
    }

    // Patch TankBig
    [HarmonyPatch(typeof(TankBig), "Start")]
    static class TankBig_Start_Patch
    {
        static void Postfix(TankBig __instance)
        {
            if (!Main.enabled) return;
            __instance.health = 500;
            __instance.tankSpeed = 80;

            if(Main.HardMode)
            {
                __instance.health = 600;
            }
        }
    }

    // patch Mecha
    [HarmonyPatch(typeof(MookArmouredGuy), "Start")]
    static class MookArmouredGuy_Start_Patch
    {
        static void Postfix(MookArmouredGuy __instance)
        {
            if (!Main.enabled) return;
            __instance.health = 50;
            __instance.speed = 15;
            try
            {
                __instance.jetPackFuelConsumption *= 2;
                __instance.pilotUnit = Main.NewMookForMecha;
            }
            catch(Exception ex)
            {
                Main.Log(ex);
            }

        }
    }

    // Patch Mamoth kopter
    [HarmonyPatch(typeof(MammothKopter), "Start")]
    static class MammothKopter_Start_Patch
    {
        static void Postfix(MammothKopter __instance)
        {
            if (!Main.enabled) return;
            __instance.health = 12000;
            __instance.tankSpeed = 110;
            __instance.verticalSpeed = 110;
            int NbrHitPropane = 8;
            if (Main.HardMode) NbrHitPropane = 10;
            Traverse.Create(__instance).Field("propaneHitsLeft").SetValue(NbrHitPropane);

        }
    }
}

