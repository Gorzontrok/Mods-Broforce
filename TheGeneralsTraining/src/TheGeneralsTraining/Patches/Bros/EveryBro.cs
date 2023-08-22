using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using TheGeneralsTraining.Components.Bros;

namespace TheGeneralsTraining.Patches.Bros.EveryBros
{
    // TODO : FIx pushing
    [HarmonyPatch(typeof(TestVanDammeAnim), "AnimatePushing")]
    static class AnimatePushing_Patch
    {
        static Vector3 FinishPushingVector3(HeroType hero)
        {
            Vector3 vector = new Vector3(0f, 0f, -0.001f);
            switch (hero)
            {
                case HeroType.Blade: vector = new Vector3(0f, 0f, -1f); break;
                case HeroType.BronanTheBrobarian: vector = new Vector3(3f, 0, -1f); break;
                case HeroType.Nebro: vector = new Vector3(4f, 0, -1f); break;
                case HeroType.TheBrolander: vector = new Vector3(3f, 0, -1f); break;
                case HeroType.HaleTheBro: vector = new Vector3(2f, 0, -1f); break;
                case HeroType.BroveHeart:
                    TestVanDammeAnim broheart = HeroController.GetHeroPrefab(hero);
                    if (!Traverse.Create(broheart).Field("disarmed").GetValue<bool>()) vector = new Vector3(5f, 4, -1f);
                    else vector = new Vector3(3, 0, -1);
                    break;
                case HeroType.BroneyRoss:
                    vector = new Vector3(0, 0, 0); break;
                case HeroType.LeeBroxmas:
                    vector = new Vector3(6, 0, -0.001f); break;
                case HeroType.TheBrode:
                    vector = new Vector3(4, 4, 1); break;
                case HeroType.Brochete:
                    vector = new Vector3(6, 0, 0.001f); break;
            }
            return vector;
        }
        static Vector3 PushingVector3(HeroType hero)
        {
            Vector3 vector = new Vector3(0f, 0f, -0.001f);
            switch (hero)
            {
                case HeroType.Blade: vector = new Vector3(-4f, 0f, -1f); break;
                case HeroType.BronanTheBrobarian: vector = new Vector3(-3f, 0, -1f); break;
                case HeroType.Nebro: vector = new Vector3(-4f, 0, -1f); break;
                case HeroType.TheBrolander: vector = new Vector3(-3f, 0, -1f); break;
                case HeroType.HaleTheBro: vector = new Vector3(-2f, 0, -1f); break;
                case HeroType.BroveHeart:
                    TestVanDammeAnim broheart = HeroController.GetHeroPrefab(hero);
                    if (!Traverse.Create(broheart).Field("disarmed").GetValue<bool>()) vector = new Vector3(-5f, 4, -1f);
                    else vector = new Vector3(-3, 0, -1);
                    break;
                case HeroType.BroneyRoss:
                    vector = new Vector3(-2, 0, 0); break;
                case HeroType.LeeBroxmas:
                    vector = new Vector3(-5, 0, -0.001f); break;
                case HeroType.TheBrode:
                    vector = new Vector3(-4, 0, -1); break;
                case HeroType.Brochete:
                    vector = new Vector3(-6, 0, 0.001f); break;
            }
            return vector;
        }
        static bool PushingBug(HeroType hero)
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
                if (Main.CanUsePatch && Main.settings.pushAnimation)
                {
                    if (PushingBug(__instance.heroType))
                    {
                        __instance.gunSprite.transform.localPosition = PushingVector3(__instance.heroType);
                    }

                    float pushingTime =__instance.GetFieldValue<float>("pushingTime");
                    if (__instance.fire || pushingTime <= 0)
                    {
                        __instance.gunSprite.transform.localScale = new Vector3(-1f, 1f, 1f);
                        __instance.gunSprite.transform.localPosition = FinishPushingVector3(__instance.heroType);
                        if (PushingBug(__instance.heroType))
                            __instance.GetTraverse().Method("SetGunPosition", new object[] { 0, 0 });
                    }
                }
            }
            catch (Exception ex)
            {
                Main.ExceptionLog(ex);
            }
        }
    }


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
            if (Main.CanUsePatch && Main.settings.strongerThrow)
            {
                try
                {
                    Traverse t = Traverse.Create(__instance);
                    t.Method("SetSpriteOffset", new object[] { 0f, 0f }).GetValue();
                    t.SetFieldValue("rollingFrames", 0);
                    if (__instance.frame == 1)
                    {
                        __instance.counter -= 0.0334f;
                    }
                    if (__instance.frame == 6 && t.GetFieldValue<bool>("meleeFollowUp"))
                    {
                        __instance.counter -= 0.08f;
                        __instance.frame = 1;
                        t.SetFieldValue("meleeFollowUp", false);
                        t.Method("ResetMeleeValues").GetValue();
                    }
                    t.SetFieldValue("frameRate", 0.025f);
                    Mook nearbyMook = t.Field("nearbyMook").GetValue<Mook>();
                    if (__instance.frame == 2 && nearbyMook != null && /**/CanThrowMook(__instance, nearbyMook)/**/ && t.GetFieldValue<bool>("highFive"))
                    {
                        t.Method("CancelMelee").GetValue();
                        t.Method("ThrowBackMook", new object[] { nearbyMook }).GetValue();
                        nearbyMook = null;
                    }
                    return false;
                }
                catch(Exception ex)
                {
                    Main.Log(ex);
                }
            }
            return true;
        }
        static void Postfix(BroBase __instance)
        {
            if (Main.CanUsePatch && __instance is DirtyHarry && Main.settings.reloadOnPunch)
            {
                try
                {
                    __instance = __instance as DirtyHarry;
                    if (__instance.GetBool("meleeHasHit"))
                    {
                        __instance.SetFieldValue("bulletCount", 0);
                    }
                    float num = 6;
                    Vector3 vector = new Vector3(__instance.X + (float)__instance.Direction * (num + 7f), __instance.Y + 8f, 0f);
                    if (Map.HitClosestUnit(__instance, __instance.playerNum, 0, DamageType.None, num, num * 2f, vector.x, vector.y, 0, 0, false, false, __instance.IsMine, false, false))
                    {
                        __instance.SetFieldValue("bulletCount", 0);
                    }

                }
                catch (Exception ex)
                {
                    Main.ExceptionLog(ex);
                }
            }
        }
    }
    // TODO : Enabled pushing animation
    [HarmonyPatch(typeof(BroBase), "Awake")]
    static class BroBase_EnabledPushingAnimation_Patch
    {
        static void Postfix(BroBase __instance)
        {
            try
            {
                __instance.useNewPushingFrames = Main.CanUsePatch && Main.settings.pushAnimation;
                __instance.useNewLadderClimbingFrames = Main.CanUsePatch && Main.settings.ladderAnimation;
            }
            catch (Exception ex)
            {
                Main.ExceptionLog(ex);
            }
        }
    }

    // Fix Hide in grass
    [HarmonyPatch(typeof(Map), "IsInSubstance")]
    static class Map_FixHideInGrass_Patch
    {
        static bool Prefix(Map __instance, ref Doodad __result, float x, float y, float range)
        {
            if (Main.CanUsePatch)
            {
                try
                {
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
            if (Main.CanUsePatch && Main.settings.rememberPockettedSpecial)
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
            if (Main.CanUsePatch && Main.settings.rememberPockettedSpecial)
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
            if(Main.CanUsePatch)
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

            }
            return true;
        }
    }

    [HarmonyPatch(typeof(BroBase), "SpecialAmmo", MethodType.Getter)]
    static class BroBase_MultiplePocketedSpecial2_Patch
    {
        static bool Prefix(BroBase __instance, ref int __result)
        {
            if (Main.CanUsePatch)
            {
                try
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
                catch (Exception ex)
                {
                    Main.Log(ex);
                }
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(TestVanDammeAnim), "ThrowBackMook")]
    static class TestVanDammeAnim_BrolanderAndBrodenZapMookOnThrow_Patch
    {
        static void Postfix(TestVanDammeAnim __instance, Mook mook)
        {
            if(Main.CanUsePatch)
            {
                try
                {
                    if ((__instance is Broden || (__instance is TheBrolander && (__instance as TheBrolander).SpecialAmmo > 2)) && Main.settings.electricThrow)
                    {
                        Traverse.Create(__instance.heldMook).Field("plasmaCounter").SetValue(1f);
                        __instance.heldMook.Damage(100, DamageType.Plasma, 0, 0, __instance.Direction, __instance, __instance.heldMook.X, __instance.heldMook.Y + 5);
                    }
                    if (__instance is ChevBrolios && Main.settings.carBattery)
                    {
                        ChevBrolios chev = __instance as ChevBrolios;
                        if (chev.GetComponent<ChevBrolios_Comp>().IsOnFire())
                        {
                            Traverse.Create(__instance.heldMook).Field("plasmaCounter").SetValue(1f);
                            __instance.heldMook.Damage(100, DamageType.Plasma, 0, 0, __instance.Direction, __instance, __instance.heldMook.X, __instance.heldMook.Y + 5);
                        }
                    }
                }
                catch (Exception e)
                {
                    Main.Log(e);
                }
            }
        }
    }

    [HarmonyPatch(typeof(TestVanDammeAnim), "IsAmmoFull")]
    static class TestVanDammeAnim_NoMoreAmmoPickupWithPocketedSpecial_Patch
    {
        static bool Prefix(TestVanDammeAnim __instance, ref bool __result)
        {
            if(Main.CanUsePatch && __instance as BroBase)
            {
                try
                {
                    Traverse t = Traverse.Create(__instance);
                    if (World.Generation.MapGenV4.ProcGenGameMode.UseProcGenRules)
                    {
                        __result = __instance.SpecialAmmo >= 6;
                    }
                    else if ((__instance as BroBase).pockettedSpecialAmmo.Count > 0)
                    {
                        __result = true;
                    }
                    else
                    {
                        __result = __instance.SpecialAmmo >= __instance.originalSpecialAmmo;
                    }

                    if(Main.settings.retrieveSwordInAmmo && __instance.As<BroveHeart>() && __instance.As<BroveHeart>().GetFieldValue<bool>("disarmed"))
                    {
                        __result = false;
                    }
                    return false;
                }
                catch (Exception e)
                {
                    Main.Log(e);
                }
            }
            return true;
        }
    }

    /* [HarmonyPatch(typeof(TestVanDammeAnim), "SetMeleeType")]
     static class TestVanDammeAnim_MeleeDownIfDownIsPress_Patch
     {
         static bool Prefix(TestVanDammeAnim __instance)
         {
             if(Main.CanUsePatch)
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

    [HarmonyPatch(typeof(BroBase), "HolyWaterRevive")]
    static class PanicUnits_OnHolyWaterRevive_Patch
    {
        static void Prefix(BroBase __instance)
        {
            if (Main.CanUsePatch && Main.settings.holyWaterPanicUnits)
            {
                Map.PanicUnits(__instance.X, __instance.Y, 64, 0.5f, true, true);
            }
        }
    }


    [HarmonyPatch(typeof(BroBase), "TriggerFlexEvent")]
    static class BroBase_TriggerFlexEvent_Patch
    {
        static bool Prefix(BroBase __instance)
        {
            if(Main.CanUsePatch && Main.settings.goldenFlexBrosProjectile)
            {
                try
                {
                    if (__instance.player.HasFlexPower(PickupType.FlexGoldenLight))
                    {
                        MuscleTempleFlexEffect flexEffect = __instance.GetFieldValue<MuscleTempleFlexEffect>("flexEffect");
                        if (flexEffect != null)
                        {
                            flexEffect.PlaySoundEffect();
                        }
                        if (__instance.IsMine)
                        {
                            int num = 8 + UnityEngine.Random.Range(0, 5);
                            for (int i = 0; i < num; i++)
                            {
                                float angle = -1.8849558f + 1.2f / (float)(num - 1) * 3.1415927f * (float)i;
                                Vector2 vector = global::Math.Point2OnCircle(angle, 1f);
                                ProjectileController.SpawnProjectileOverNetwork(__instance.projectile, __instance,
                                    __instance.X, __instance.Y + 12f,
                                    vector.x * 400f, vector.y * 400f,
                                    true, 15, false, true, -15f);
                            }
                        }
                    }
                    return false;
                }
                catch (Exception e)
                {
                    Main.Log(e);
                }
            }
            return true;

        }
    }

    [HarmonyPatch(typeof(TestVanDammeAnim), "ResetSpecialAmmo")]
    static class TestVanDammeAnim_ResetAmmo_Patch
    {
        static void Postfix(TestVanDammeAnim __instance)
        {
            if (!Main.CanUsePatch) return;

            var broveHeart = __instance.As<BroveHeart>();
            if (broveHeart)
            {
                broveHeart.CallMethod("SetDisarmed", false);
            }
        }
    }


    [HarmonyPatch(typeof(TestVanDammeAnim), "CalculateZombieInput")]
    static class FlexIfReviveSourceFlex_Patch
    {
        static void Postfix(TestVanDammeAnim __instance)
        {
            if (Main.CanUsePatch && Main.settings.flexIfReviveSourceFlex)
            {
                var reviveSource = __instance.GetFieldValue<TestVanDammeAnim>("reviveSource");
                if (reviveSource == null || !reviveSource.IsGesturing()) return;

                if (__instance as BroBase)
                {
                    __instance.SetGestureAnimation(GestureElement.Gestures.Flex);
                }
            }
        }
    }
}

