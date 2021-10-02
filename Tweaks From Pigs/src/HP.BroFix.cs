using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace TweaksFromPigs
{
    [HarmonyPatch(typeof(IndianaBrones), "AnimateMelee")]
    static class IndianaBrones_AchievementRework_Patch
    {
        static void Prefix(IndianaBrones __instance)
        {
            if (!Main.enabled) return;
            if(Main.settings.FixIndianaAchievement)
            {
                Traverse instTrav = Traverse.Create(__instance);
                TestVanDammeAnim nearbyMook = instTrav.Field("nearbyMook").GetValue<TestVanDammeAnim>();
                if (instTrav.Field("meleeFrame").GetValue<int>() == 2 && nearbyMook != null && nearbyMook.CanBeThrown() && instTrav.Field("highFive").GetValue<bool>())
                {
                    instTrav.Method("CancelMelee").GetValue();
                    instTrav.Method("ThrowBackMook", new object[] { nearbyMook }).GetValue();

                    Transform parentedToTransform = Traverse.Create(__instance).Field("nearbyMook").GetValue<TestVanDammeAnim>().GetParentedToTransform();
                    if (parentedToTransform != null && parentedToTransform.name.ToUpper().Contains("BOSS"))
                    {
                        SteamController.UnlockAchievement(SteamAchievement.noticket);
                    }
                }
            }
        }
    }

    // DoubleBroSeven Patch
    [HarmonyPatch(typeof(DoubleBroSeven), "Awake")]
    static class DoubleBroSeven_Awake_Patch
    {
        static void Postfix(DoubleBroSeven __instance)
        {
            if (!Main.enabled) return;

            if (Main.settings.UseFifthBondSpecial)
            {
                Traverse.Create(typeof(DoubleBroSeven)).Field("_specialAmmo").SetValue(5);
                __instance.SpecialAmmo = 5;
                __instance.originalSpecialAmmo = 5;
            }
        }
    }
   [HarmonyPatch(typeof(DoubleBroSeven), "UseSpecial")]
    static class DoubleBroSeven_TearGasAtFeet_Patch
    {
        static bool Prefix(DoubleBroSeven __instance)
        {
            if (!Main.enabled) return true;
            if(Main.settings.TeargasAtFeet)
            {
                DoubleBroSevenSpecialType currentSpecialType = Traverse.Create(__instance).Field("currentSpecialType").GetValue<DoubleBroSevenSpecialType>();
                if (currentSpecialType == DoubleBroSevenSpecialType.TearGas)
                {
                    Networking.Networking.RPC<float>(PID.TargetAll, new RpcSignature<float>(__instance.PlayThrowLightSound), 0.5f, false);
                    if (__instance.IsMine)
                    {
                        if (Traverse.Create(__instance).Field("ducking").GetValue<bool>() && __instance.down)
                        {
                            ProjectileController.SpawnGrenadeOverNetwork(__instance.tearGasGrenade, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 3f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 30f, 70f, __instance.playerNum);
                        }
                        else
                        {
                            ProjectileController.SpawnGrenadeOverNetwork(__instance.tearGasGrenade, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 150f, __instance.playerNum);
                        }
                    }
                    __instance.SpecialAmmo--;
                    return false;
                }
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(DoubleBroSeven), "FireWeapon")]
    static class LessAccurateWhenDrunk_Patch
    {
        static bool Prefix(DoubleBroSeven __instance, float x, float y, float xSpeed, float ySpeed)
        {
            if (!Main.enabled) return true;
            if(Main.settings.LessAccurateDrunkSeven)
            {
                if (Traverse.Create(__instance).Field("martinisDrunk").GetValue<int>() > 2)
                {
                    int randY = Utility.rand.Next(-25, 25);
                    __instance.gunSprite.SetLowerLeftPixel((float)(32 * 3), 32f);
                    EffectsController.CreateMuzzleFlashEffect(x, y, -25f, xSpeed * 0.01f, ySpeed * 0.01f, __instance.transform);
                    ProjectileController.SpawnProjectileLocally(__instance.projectile, __instance, x, y, xSpeed, ySpeed + randY, __instance.playerNum);
                    return false;
                }
            }
            return true;
        }
    }

    // BrondleFly Patch
    [HarmonyPatch(typeof(BrondleFly), "Awake")]
    static class BrondleFly_Awake_Patch
    {
        static void Postfix(BrondleFly __instance)
        {
            if (!Main.enabled) return;

            TestVanDammeAnim baBroracus = HeroController.GetHeroPrefab(HeroType.BaBroracus);
            __instance.soundHolder.attackSounds = (baBroracus as BaBroracus).soundHolder.attackSounds;
        }
    }

    // BroneyRoss Patch
    [HarmonyPatch(typeof(BroneyRoss), "Awake")]
    static class BroneyRoss_Awake_Patch
    {
        static void Postfix(BroneyRoss __instance)
        {
            if (!Main.enabled) return;
            if(Main.settings.FixExpendabros)
            {
                TestVanDammeAnim broHard = HeroController.GetHeroPrefab(HeroType.BroHard);
                __instance.soundHolder.attractSounds = broHard.soundHolder.attackSounds;
            }

        }
    }

    // LeeBroxmas Patch
    [HarmonyPatch(typeof(LeeBroxmas), "Awake")]
    static class LeeBroxmas_Awake_Patch
    {
        static void Postfix(LeeBroxmas __instance)
        {
            if (!Main.enabled) return;

            if(Main.settings.FixExpendabros)
            {
                TestVanDammeAnim blade = HeroController.GetHeroPrefab(HeroType.Blade);
                Texture bladeKnifeTex = (blade as Blade).throwingKnife.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture;

                __instance.projectile.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = bladeKnifeTex;
                __instance.macheteSprayProjectile.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = bladeKnifeTex;
            }
        }
    }
    [HarmonyPatch(typeof(LeeBroxmas), "AnimatePushing")]
    static class LeeBroxmas_Pushing_Patch
    {
        static void Postfix(LeeBroxmas __instance)
        {
            if (!Main.enabled) return;
            if(Main.settings.FixPushingAnimation)
            {
                __instance.gunSprite.transform.localPosition = Utility.GetBroGunVector3PositionWhilePushing(HeroType.LeeBroxmas);
                float pushingTime = Traverse.Create(__instance).Field("pushingTime").GetValue<float>();
                if (__instance.fire || pushingTime <= 0)
                {
                    __instance.gunSprite.transform.localScale = new Vector3(-1f, 1f, 1f);
                    __instance.gunSprite.transform.localPosition = Utility.GetBroGunVector3PositionWhenFinishPushing(__instance.heroType);
                }
            }
        }
    }

    // Patch Bronnar Jensen
    [HarmonyPatch(typeof(BronnarJensen), "UseFire")]
    static class BronnarJensen_FixShoot_Patch
    {
        static bool Prefix(BronnarJensen __instance)
        {
            if (!Main.enabled) return true;
            if(Main.settings.FixExpendabros)
            {
                if (__instance.IsMine)
                {
                    if (Traverse.Create(__instance).Field("ducking").GetValue<bool>() && __instance.down)
                    {
                        Traverse.Create(__instance).Method("PlayAttackSound", new object[] { 0.4f }).GetValue();
                        Traverse.Create(__instance).Method("FireWeapon", new object[] { __instance.X + __instance.transform.localScale.x * 6f, __instance.Y + 7f, __instance.transform.localScale.x * (__instance.shootGrenadeSpeedX * 0.3f) + __instance.xI * 0.45f, 25f + ((__instance.yI <= 0f) ? 0f : (__instance.yI * 0.3f)) }).GetValue();
                    }
                    else
                    {
                        Traverse.Create(__instance).Method("PlayAttackSound", new object[] { 0.4f }).GetValue();
                        Traverse.Create(__instance).Method("FireWeapon", new object[] { __instance.X + __instance.transform.localScale.x * 6f, __instance.Y + 10f, __instance.transform.localScale.x * __instance.shootGrenadeSpeedX + __instance.xI * 0.45f, __instance.shootGrenadeSpeedY + ((__instance.yI <= 0f) ? 0f : (__instance.yI * 0.3f)) }).GetValue();
                    }
                }
                Map.DisturbWildLife(__instance.X, __instance.Y, 60f, __instance.playerNum);
                __instance.fireDelay = 0.6f;

                return false;
            }
            return true;
        }
    }

    // TrentBroser Patch
    [HarmonyPatch(typeof(TrentBroser), "Awake")]
    static class TrentBroser_Awake_Patch
    {
        static void Postfix(TrentBroser __instance)
        {
            if (!Main.enabled) return;

            if (Main.settings.FixExpendabros)
            {
                TestVanDammeAnim broDredd = HeroController.GetHeroPrefab(HeroType.BroDredd);
                __instance.soundHolder.attackSounds = broDredd.soundHolder.attackSounds;
            }
        }
    }

    // patch Brochete
    [HarmonyPatch(typeof(Brochete), "AnimatePushing")]
    static class Brochete_Pushing_Patch
    {
        static void Postfix(Brochete __instance)
        {
            if (!Main.enabled) return;
            if(Main.settings.FixPushingAnimation)
            {
                __instance.gunSprite.transform.localPosition = Utility.GetBroGunVector3PositionWhilePushing(HeroType.Brochete);
                float pushingTime = Traverse.Create(__instance).Field("pushingTime").GetValue<float>();
                if (__instance.fire || pushingTime <= 0)
                {
                    __instance.gunSprite.transform.localScale = new Vector3(-1f, 1f, 1f);
                    __instance.gunSprite.transform.localPosition = Utility.GetBroGunVector3PositionWhenFinishPushing(__instance.heroType);
                }
            }
        }
    }

    // Patch Broniversal Soldier
    [HarmonyPatch(typeof(BroniversalSoldier), "Update")]
    static class AutoReviveAndWaitForSpecial_Patch
    {
        public static float TimeBeforeSpecial = 0f;
        static void Postfix(BroniversalSoldier __instance)
        {
            if (!Main.enabled) return;
            if(Main.settings.LessOPBroniversalRevive)
            {
                if (TimeBeforeSpecial > 0) TimeBeforeSpecial -= 0.1f;
            }
            if(Main.settings.BroniversalAutoRevive)
            {
                if (Time.time - Traverse.Create(__instance).Field("deathTime").GetValue<float>() < Traverse.Create(__instance).Field("deathGracePeriod").GetValue<float>() && __instance.SpecialAmmo > 0)
                {
                    __instance.Revive(__instance.playerNum, true, __instance);
                }
            }
        }
    }
    [HarmonyPatch(typeof(BroniversalSoldier), "Revive")]
    static class DecreaseSpecialAmmoOnRevive_Patch
    {
        static void Postfix(BroniversalSoldier __instance, ref bool __result)
        {
            if (!Main.enabled) return;
            if(Main.settings.LessOPBroniversalRevive)
            {
                if(__result) __instance.SpecialAmmo--;
            }
        }
    }
    [HarmonyPatch(typeof(BroniversalSoldier), "PressSpecial")]
    static class WaitBeforeReuseSpecial_Patch
    {
        static bool Prefix(BroniversalSoldier __instance)
        {
            if (!Main.enabled) return true;
            if(Main.settings.LessOPBroniversalRevive)
            {
                if (AutoReviveAndWaitForSpecial_Patch.TimeBeforeSpecial <= 0)
                {
                    AutoReviveAndWaitForSpecial_Patch.TimeBeforeSpecial = 2f;
                    return true;
                }
                return false;
            }
            return true;
        }
    }
}
