using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace TweaksFromPigs
{
    // Patch animation
    [HarmonyPatch(typeof(TestVanDammeAnim), "AnimatePushing")]
    static class AnimatePushing_Patch
    {
        static void Postfix(TestVanDammeAnim __instance)
        {
            float pushingTime = Traverse.Create(__instance).Field("pushingTime").GetValue<float>();
            if (__instance.fire || pushingTime <= 0)
            {
                if(__instance.heroType == HeroType.BroveHeart) __instance.gunSprite.transform.localPosition = new Vector3(5f, 4f, -1f);

                __instance.gunSprite.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }
    [HarmonyPatch(typeof(TestVanDammeAnim), "AnimateGesture")]
    static class AnimateGesture_Patch
    {
        static bool Prefix(TestVanDammeAnim __instance)
        {
            if (HeroUnlockController.IsExpendaBro(__instance.heroType)) return false;
            return true;
        }
    }
    [HarmonyPatch(typeof(TestVanDammeAnim), "AnimateInseminationFrames")]
    static class AnimateInseminationFrames_Patch 
    {
        static bool Prefix(TestVanDammeAnim __instance)
        {
            if (HeroUnlockController.IsExpendaBro(__instance.heroType)) return false;
            return true;
        }
    }

    // Rambro Patch
    [HarmonyPatch(typeof(Rambro), "Awake")]
    static class Rambro_Awake_Patch
    {
        static void Postfix(Rambro __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // Brommando Patch
    [HarmonyPatch(typeof(Brommando), "Awake")]
    static class Brommando_Awake_Patch
    {
        static void Postfix(Brommando __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // BaBroracus Patch
    [HarmonyPatch(typeof(BaBroracus), "Awake")]
    static class BaBroracus_Awake_Patch
    {
        static void Postfix(BaBroracus __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // BrodellWalker Patch
    [HarmonyPatch(typeof(BrodellWalker), "Awake")]
    static class BrodellWalker_Awake_Patch
    {
        static void Postfix(BrodellWalker __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // BroHard Patch
    [HarmonyPatch(typeof(BroHard), "Awake")]
    static class BroHard_Awake_Patch
    {
        static void Postfix(BroHard __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // McBrover Patch
    [HarmonyPatch(typeof(McBrover), "Awake")]
    static class McBrover_Awake_Patch
    {
        static void Postfix(McBrover __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // Blade Patch
    [HarmonyPatch(typeof(Blade), "Awake")]
    static class Blade_Awake_Patch
    {
        static void Postfix(Blade __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // BroDredd Patch
    [HarmonyPatch(typeof(BroDredd), "Awake")]
    static class BroDredd_Awake_Patch
    {
        static void Postfix(BroDredd __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // Bro In Black Patch
    [HarmonyPatch(typeof(Brononymous), "Awake")]
    static class Brononymous_Awake_Patch
    {
        static void Postfix(Brononymous __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // SnakeBroskin Patch
    [HarmonyPatch(typeof(SnakeBroskin), "Awake")]
    static class SnakeBroskin_Awake_Patch
    {
        static void Postfix(SnakeBroskin __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // Brominator Patch
    [HarmonyPatch(typeof(Brominator), "Awake")]
    static class Brominator_Awake_Patch
    {
        static void Postfix(Brominator __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // Brobocop Patch
    [HarmonyPatch(typeof(Brobocop), "Awake")]
    static class Brobocop_Awake_Patch
    {
        static void Postfix(Brobocop __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // IndianaBrones Patch
    [HarmonyPatch(typeof(IndianaBrones), "Awake")]
    static class IndianaBrones_Awake_Patch
    {
        static void Postfix(IndianaBrones __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }
    [HarmonyPatch(typeof(IndianaBrones), "AnimateMelee")]
    static class IndianaBrones_AchievementRework_Patch
    {
        static void Prefix(IndianaBrones __instance)
        {
            Traverse instTrav = Traverse.Create(__instance);
            TestVanDammeAnim nearbyMook = instTrav.Field("nearbyMook").GetValue<TestVanDammeAnim>();
            if (instTrav.Field("meleeFrame").GetValue<int>() == 2 && nearbyMook != null && nearbyMook.CanBeThrown() && instTrav.Field("highFive").GetValue<bool>())
            {
                instTrav.Method("CancelMelee").GetValue();
                instTrav.Method("ThrowBackMook", new object[] { nearbyMook }).GetValue() ;

                Transform parentedToTransform = Traverse.Create(__instance).Field("nearbyMook").GetValue<TestVanDammeAnim>().GetParentedToTransform();
                if (parentedToTransform != null && parentedToTransform.name.ToUpper().Contains("BOSS"))
                {
                    SteamController.UnlockAchievement(SteamAchievement.noticket);
                }
            }
                
        }
    }

    // AshBrolliams Patch
    [HarmonyPatch(typeof(AshBrolliams), "Awake")]
    static class AshBrolliams_Awake_Patch
    {
        static void Postfix(AshBrolliams __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // Mr Anderbro Patch
    [HarmonyPatch(typeof(Nebro), "Start")]
    static class Nebro_Start_Patch
    {
        static void Postfix(Nebro __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // BoondockBro Patch
    [HarmonyPatch(typeof(BoondockBro), "Awake")]
    static class BoondockBro_Awake_Patch
    {
        static void Postfix(BoondockBro __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // Brochete Patch
    [HarmonyPatch(typeof(Brochete), "Awake")]
    static class Brochete_Awake_Patch
    {
        static void Postfix(Brochete __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // BronanTheBrobarian Patch
    [HarmonyPatch(typeof(BronanTheBrobarian), "Update")]
    static class BronanTheBrobarian_Update_Patch
    {
        static void Postfix(BronanTheBrobarian __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // EllenRipbro Patch
    [HarmonyPatch(typeof(EllenRipbro), "Awake")]
    static class EllenRipbro_Awake_Patch
    {
        static void Postfix(EllenRipbro __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // TimeBroVanDamme Patch
    [HarmonyPatch(typeof(TimeBroVanDamme), "Awake")]
    static class TimeBroVanDamme_Awake_Patch
    {
        static void Postfix(TimeBroVanDamme __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // BroniversalSoldier Patch
    [HarmonyPatch(typeof(BroniversalSoldier), "Awake")]
    static class BroniversalSoldier_Awake_Patch
    {
        static void Postfix(BroniversalSoldier __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // ColJamesBrodock Patch
    [HarmonyPatch(typeof(ColJamesBrodock), "Awake")]
    static class ColJamesBrodock_Awake_Patch
    {
        static void Postfix(ColJamesBrodock __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // CherryBroling Patch
    [HarmonyPatch(typeof(CherryBroling), "Awake")]
    static class CherryBroling_Awake_Patch
    {
        static void Postfix(CherryBroling __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // TheBrode Patch
    [HarmonyPatch(typeof(TheBrode), "Update")]
    static class TheBrode_Update_Patch
    {
        static void Postfix(TheBrode __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // BroMax Patch
    [HarmonyPatch(typeof(TollBroad), "Awake")]
    static class BroMax_Awake_Patch
    {
        static void Postfix(BroMax __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // DoubleBroSeven Patch
    [HarmonyPatch(typeof(DoubleBroSeven), "Awake")]
    static class DoubleBroSeven_Awake_Patch
    {
        static void Postfix(DoubleBroSeven __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;

            if (Main.settings.UseFifthBondSpecial)
            {
                Traverse.Create(typeof(DoubleBroSeven)).Field("_specialAmmo").SetValue(5);
                __instance.SpecialAmmo = 5;
                __instance.originalSpecialAmmo = 5;
            }
        }
    }

    // Predabro Patch
    [HarmonyPatch(typeof(Predabro), "Awake")]
    static class Predabro_Awake_Patch
    {
        static void Postfix(Predabro __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // TheBrofessional Patch
    [HarmonyPatch(typeof(TheBrofessional), "Update")]
    static class TheBrofessional_Update_Patch
    {
        static void Postfix(TheBrofessional __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // BrondleFly Patch
    [HarmonyPatch(typeof(BrondleFly), "Awake")]
    static class BrondleFly_Awake_Patch
    {
        static void Postfix(BrondleFly __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // BroveHeart Patch
    [HarmonyPatch(typeof(BroveHeart), "Start")]
    static class BroveHeart_Start_Patch
    {
        static void Postfix(BroveHeart __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // TheBrocketeer Patch
    [HarmonyPatch(typeof(TheBrocketeer), "Start")]
    static class TheBrocketeer_Start_Patch
    {
        static void Postfix(TheBrocketeer __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // TheBrolander Patch
    [HarmonyPatch(typeof(TheBrolander), "Start")]
    static class TheBrolander_Start_Patch
    {
        static void Postfix(TheBrolander __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // BroLee Patch
    [HarmonyPatch(typeof(BroLee), "Awake")]
    static class BroLee_Awake_Patch
    {
        static void Postfix(BroLee __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // DirtyHarry Patch
    [HarmonyPatch(typeof(DirtyHarry), "Awake")]
    static class DirtyHarry_Awake_Patch
    {
        static void Postfix(DirtyHarry __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // TollBroad Patch
    [HarmonyPatch(typeof(TollBroad), "Awake")]
    static class TollBroad_Awake_Patch
    {
        static void Postfix(TollBroad __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // Broc Patch
    [HarmonyPatch(typeof(BrocSnipes), "Awake")]
    static class BrocSnipes_Awake_Patch
    {
        static void Postfix(BrocSnipes __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // BroneyRoss Patch
    [HarmonyPatch(typeof(BroneyRoss), "Awake")]
    static class BroneyRoss_Awake_Patch
    {
        static void Postfix(BroneyRoss __instance)
        {
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
        }
    }

    // LeeBroxmas Patch
    [HarmonyPatch(typeof(LeeBroxmas), "Awake")]
    static class LeeBroxmas_Awake_Patch
    {
        static void Postfix(LeeBroxmas __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // BronnarJensen Patch
    [HarmonyPatch(typeof(BronnarJensen), "Awake")]
    static class BronnarJensen_Awake_Patch
    {
        static void Postfix(BronnarJensen __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }
    [HarmonyPatch(typeof(BronnarJensen), "UseFire")]
    static class BronnarJensen_FixShoot_Patch
    {
        static bool Prefix(BronnarJensen __instance)
        {
            if (__instance.IsMine)
            {
                if (Traverse.Create(__instance).Field("ducking").GetValue<bool>() && __instance.down)
                {
                    Traverse.Create(__instance).Method("FireWeapon", new object[] { __instance.X + __instance.transform.localScale.x * 6f, __instance.Y + 7f, __instance.transform.localScale.x * (__instance.shootGrenadeSpeedX * 0.3f) + __instance.xI * 0.45f, 25f + ((__instance.yI <= 0f) ? 0f : (__instance.yI * 0.3f)) }).GetValue();
                    
                }
                else
                {
                    Traverse.Create(__instance).Method("FireWeapon", new object[] { __instance.X + __instance.transform.localScale.x * 6f, __instance.Y + 10f, __instance.transform.localScale.x * __instance.shootGrenadeSpeedX + __instance.xI * 0.45f, __instance.shootGrenadeSpeedY + ((__instance.yI <= 0f) ? 0f : (__instance.yI * 0.3f)) }).GetValue();
                    
                }
            }
            Traverse.Create(__instance).Method("PlayAttackSound", new object[] { 0.4f });
            Map.DisturbWildLife(__instance.X, __instance.Y, 60f, __instance.playerNum);
            __instance.fireDelay = 0.6f;

            return false;
        }
    }


    // BroCeasar Patch
    [HarmonyPatch(typeof(BroCeasar), "Awake")]
    static class BroCeasar_Awake_Patch
    {
        static void Postfix(BroCeasar __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }

    // TrentBroser Patch
    [HarmonyPatch(typeof(TrentBroser), "Awake")]
    static class TrentBroser_Awake_Patch
    {
        static void Postfix(TrentBroser __instance)
        {
            if (Main.settings.UsePushingFrame) __instance.useNewPushingFrames = true;
            if (Main.settings.UseNewLadderFrame) __instance.useNewLadderClimbingFrames = true;
        }
    }
}
