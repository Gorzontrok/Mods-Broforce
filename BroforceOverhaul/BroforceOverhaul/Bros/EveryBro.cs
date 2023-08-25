using System;
using System.Collections.Generic;
using RocketLib;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace BroforceOverhaul.Bros.EveryBros
{
    // TODO : FIx pushing
    /*[HarmonyPatch(typeof(TestVanDammeAnim), "AnimatePushing")]
    static class AnimatePushing_Patch
    {
        internal static bool __instanceHeroTypePushingBug(HeroType hero)
        {
            List<HeroType> heroBuggy = new List<HeroType>() { HeroType.BroneyRoss, HeroType.Blade, HeroType.BronanTheBrobarian, HeroType.Nebro, HeroType.TheBrolander, HeroType.HaleTheBro, HeroType.TheBrode, HeroType.BroveHeart };
            foreach (HeroType heroBug in heroBuggy)
            {
                if (hero == heroBug) return true;
            }
            return false;
        }
        static void Postfix(TestVanDammeAnim __instance)
        {
            try
            {
                if (Main.enabled && Main.settings.fixPushingAnimation)
                {
                    if (__instanceHeroTypePushingBug(__instance.heroType))
                    {
                        __instance.gunSprite.transform.localPosition = TFP_Utility.GetBroGunVector3PositionWhilePushing(__instance.heroType);
                    }

                    float pushingTime = Traverse.Create(__instance).Field("pushingTime").GetValue<float>();
                    if (__instance.fire || pushingTime <= 0)
                    {
                        __instance.gunSprite.transform.localScale = new Vector3(-1f, 1f, 1f);
                        __instance.gunSprite.transform.localPosition = TFP_Utility.GetBroGunVector3PositionWhenFinishPushing(__instance.heroType);
                        if (__instanceHeroTypePushingBug(__instance.heroType))
                            Traverse.Create(HeroController.GetHeroPrefab(__instance.heroType)).Method("SetGunPosition", new object[] { 0, 0 });
                    }
                }
            }
            catch (Exception ex)
            {
                Main.ExceptionLog(ex);
            }
        }
    }*/


    // Patch BroBase
    [HarmonyPatch(typeof(BroBase), "AnimateMeleeCommon")]
    static class BroBase_ThrowBruisersOnSteroids_Patch
    {
        static bool CanThrowMook(BroBase bro, Mook mook)
        {
            return mook.CanBeThrown() || (BroController.BroIsStronger(bro) && mook as MookBigGuy && !(mook as MookArmouredGuy));
        }
        static bool Prefix(BroBase __instance)
        {
            if (Main.enabled)
            {
                try
                {
                    Traverse t = Traverse.Create(__instance);
                    t.Method("SetSpriteOffset", new object[] { 0f, 0f }).GetValue();
                    t.Field("rollingFrames").SetValue(0);
                    if (__instance.frame == 1)
                    {
                        __instance.counter -= 0.0334f;
                    }
                    if (__instance.frame == 6 && t.Field("meleeFollowUp").GetValue<bool>())
                    {
                        __instance.counter -= 0.08f;
                        __instance.frame = 1;
                        t.Field("meleeFollowUp").SetValue(false);
                        t.Method("ResetMeleeValues").GetValue();
                    }
                    t.Field("frameRate").SetValue(0.025f);
                    Mook nearbyMook = t.Field("nearbyMook").GetValue<Mook>();
                    if (__instance.frame == 2 && nearbyMook != null && /**/CanThrowMook(__instance, nearbyMook)/**/ && t.Field("highFive").GetValue<bool>())
                    {
                        t.Method("CancelMelee").GetValue();
                        t.Method("ThrowBackMook", new object[] { nearbyMook }).GetValue();
                        nearbyMook = null;
                    }
                    return false;
                }
                catch(Exception ex)
                {
                    Main.ExceptionLog(ex);
                }
            }
            return true;
        }
    }
    // TODO : Enabled pushing animation
    /*[HarmonyPatch(typeof(BroBase), "Awake")]
    static class BroBase_EnabledPushingAnimation_Patch
    {
        static void Postfix(BroBase __instance)
        {
            try
            {
                __instance.useNewPushingFrames = Main.enabled;
            }
            catch (Exception ex)
            {
                Main.ExceptionLog(ex);
            }
        }
    }*/

    // Fix Hide in grass
    [HarmonyPatch(typeof(Map), "IsInSubstance")]
    static class Map_FixHideInGrass_Patch
    {
        static bool Prefix(Map __instance, ref Doodad __result, float x, float y, float range)
        {
            if (Main.enabled)
            {
                try
                {
                    Extensions.DrawCircle(x, y, range, Color.magenta, 0f);
                    for (int i = 0; i < Map.grassAndBlood.Count; i++)
                    {
                        if (i >= 0 && Map.grassAndBlood[i] != null && Map.grassAndBlood[i].SubMergesUnit() && Mathf.Abs(Map.grassAndBlood[i].centerX - x) <= range + Map.grassAndBlood[i].width / 2f && Mathf.Abs(Map.grassAndBlood[i].centerY - y) <= range + Map.grassAndBlood[i].height / 2f)
                        {
                            __result = Map.grassAndBlood[i];
                            return false;
                        }
                    }

                }
                catch (Exception ex)
                {
                    Main.ExceptionLog("Failed to hide player in grass", ex);
                }
            }
            return true;
        }
    }

    // Remember pocketed special on bro swap.
    [HarmonyPatch(typeof(Player), "SpawnHero")]
    static class Player_RememberPockettedSpecial_Patch
    {
        static List<PockettedSpecialAmmoType> listp = new List<PockettedSpecialAmmoType>();
        static void Prefix(Player __instance)
        {
            if (Main.enabled)
            {
                try
                {
                    if (__instance.character != null && __instance.character.IsAlive())
                    {
                        BroBase bro = __instance.character as BroBase;
                        if (bro)
                        {
                            listp = bro.pockettedSpecialAmmo;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Main.ExceptionLog("Failed while saving pocketed special list.", ex);
                }
            }
        }
        static void Postfix(Player __instance)
        {
            if (Main.enabled)
            {
                try
                {
                    BroBase bro = __instance.character as BroBase;
                    if (bro)
                    {
                        bro.pockettedSpecialAmmo = listp;
                        Traverse.Create(bro).Method("SetPlayerHUDAmmo").GetValue();
                        listp = new List<PockettedSpecialAmmoType>();
                    }

                }
                catch (Exception ex)
                {
                    Main.ExceptionLog("Failed while assign pocketed special list.", ex);
                }
            }
        }
    }

    // Player don't move/shoot in chat
    [HarmonyPatch(typeof(Player), "GetInput")]
    static class Player_PlayerDontMoveIfTyping_Patch
    {
        static bool Prefix(Player __instance, ref bool up, ref bool down, ref bool left, ref bool right, ref bool fire, ref bool buttonJump, ref bool special, ref bool highFive, ref bool buttonGesture, ref bool sprint)
        {
            try
            {
                Traverse t = Traverse.Create(typeof(Player));
                if (__instance.UsingBotBrain)
                {
                    __instance.GetComponent<BotBrain>().GetInput(ref up, ref down, ref left, ref right, ref fire, ref buttonJump, ref special, ref highFive);
                }
                else if (__instance.controllerNum == NewChatController.ChatControlledBy || __instance.controllerNum == PauseController.pausedByController || Traverse.Create(__instance.characterUI).Field("IsTyping").GetValue<bool>())
                {
                    up = (down = (left = (right = (fire = (buttonJump = (special = (highFive = false)))))));
                }
                else
                {
                    InputReader.GetInput(__instance.controllerNum, ref up, ref down, ref left, ref right, ref fire, ref buttonJump, ref special, ref highFive, ref buttonGesture, ref sprint, false, false);
                }
                return false;
            }
            catch(Exception ex)
            {
                Main.ExceptionLog(ex);
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(BroBase), "SpecialAmmo", MethodType.Getter)]
    static class BroBase_MultiplePocketedSpecial2_Patch
    {
        static bool Prefix(BroBase __instance, ref int __result)
        {
            if (Main.enabled)
            {
                Traverse t = Traverse.Create(__instance);
                if (__instance.pockettedSpecialAmmo.Count <= 0)
                {
                    __result = t.Field("_specialAmmo").GetValue<int>();
                }
                else
                {
                    __result = __instance.pockettedSpecialAmmo.Count >= 6 ? 6 : __instance.pockettedSpecialAmmo.Count;
                }
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(TestVanDammeAnim), "ThrowBackMook")]
    static class TestVanDammeAnim_BrolanderAndBrodenZapMookOnThrow_Patch
    {
        static void Postfix(TestVanDammeAnim __instance, Mook mook)
        {
            if(Main.enabled && (__instance is Broden || (__instance is TheBrolander && (__instance as TheBrolander).SpecialAmmo > 2)))
            {
                Traverse.Create(__instance.heldMook).Field("plasmaCounter").SetValue(1f);
                __instance.heldMook.Damage(100, DamageType.Plasma, 0, 0, __instance.Direction, __instance, __instance.heldMook.X, __instance.heldMook.Y + 5);
            }
        }
    }

    [HarmonyPatch(typeof(TestVanDammeAnim), "IsAmmoFull")]
    static class TestVanDammeAnim_NoMoreAmmoPickupWithPocketedSpecial_Patch
    {
        static bool Prefix(TestVanDammeAnim __instance, ref bool __result)
        {
            if(Main.enabled && __instance as BroBase)
            {
                Traverse t = Traverse.Create(__instance);
                if (World.Generation.MapGenV4.ProcGenGameMode.UseProcGenRules)
                {
                    __result = __instance.SpecialAmmo >= 6;
                }
                else if((__instance as BroBase).pockettedSpecialAmmo.Count > 0)
                {
                    __result = true;
                }
                else
                {
                    __result = __instance.SpecialAmmo >= __instance.originalSpecialAmmo;
                }
                return false;
            }
            return true;
        }
    }

   /* [HarmonyPatch(typeof(TestVanDammeAnim), "SetMeleeType")]
    static class TestVanDammeAnim_MeleeDownIfDownIsPress_Patch
    {
        static bool Prefix(TestVanDammeAnim __instance)
        {
            if(Main.enabled)
            {
                try
                {
                    Traverse t = Traverse.Create(__instance);
                    Traverse standingMeleeT = t.Field("standingMelee");
                    Traverse jumpingMeleeT = t.Field("jumpingMelee");
                    Traverse dashingMeleeT = t.Field("dashingMelee");

                    standingMeleeT.SetValue(false);
                    jumpingMeleeT.SetValue(false);
                    dashingMeleeT.SetValue(false);

                    if (!__instance.useNewKnifingFrames)
                    {
                        standingMeleeT.SetValue(true);
                    }
                    else if (__instance.actionState == ActionState.Jumping || __instance.Y > __instance.groundHeight + 1f)
                    {

                        if(__instance.IsPressingDown())
                        {
                            jumpingMeleeT.SetValue(true);
                        }
                        else
                        {
                            dashingMeleeT.SetValue(true);
                        }
                    }
                    else if (__instance.right || __instance.left)
                    {
                        standingMeleeT.SetValue(false);
                        jumpingMeleeT.SetValue(false);
                        dashingMeleeT.SetValue(true);
                    }
                    else
                    {
                        standingMeleeT.SetValue(true);
                        jumpingMeleeT.SetValue(false);
                        dashingMeleeT.SetValue(false);
                    }
                    return false;
                }
                catch(Exception ex)
                {
                    Main.ExceptionLog(ex);
                }
            }
            return true;
        }
    }*/
}

