using System;
using HarmonyLib;
using UnityEngine;

namespace TweaksFromPigs._HarmonyPatch.BroFix
{
    [HarmonyPatch(typeof(IndianaBrones), "AnimateMelee")]
    static class IndianaBrones_AchievementRework_Patch
    {
        static void Prefix(IndianaBrones __instance)
        {
            if (!Main.enabled) return;
            try
            {
                if (Main.settings.fixIndianaAchievement)
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
            catch(Exception ex) { Main.bmod.logger.ExceptionLog("Failed to fix Indiana achievement" + ex); }
        }
    }

    // DoubleBroSeven Patch
    [HarmonyPatch(typeof(DoubleBroSeven), "Awake")]
    static class DoubleBroSeven_Awake_Patch
    {
        static void Postfix(DoubleBroSeven __instance)
        {
            if (!Main.enabled || Compatibility._007_Patch.i.IsEnabled) return;
            try
            {
                if (Main.settings.useFifthBondSpecial)
                {
                    __instance.originalSpecialAmmo = 5;
                    __instance.SpecialAmmo = 5;
                }
            }
            catch(Exception ex)
            {
                Main.bmod.logger.ExceptionLog("Failed to add 5th bond's special.", ex);
            }
        }
    }
   [HarmonyPatch(typeof(DoubleBroSeven), "UseSpecial")]
    static class DoubleBroSeven_TearGasAtFeet_Patch
    {
        static bool Prefix(DoubleBroSeven __instance)
        {
            if (!Main.enabled || Compatibility._007_Patch.i.IsEnabled) return true;
            try
            {
                if (Main.settings.teargasAtFeet)
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
            }
            catch(Exception ex) { Main.bmod.logger.ExceptionLog("Failed to patch teargas at feet", ex); }
            return true;
        }
    }
    [HarmonyPatch(typeof(DoubleBroSeven), "FireWeapon")]
    static class LessAccurateWhenDrunk_Patch
    {
        static bool Prefix(DoubleBroSeven __instance, float x, float y, float xSpeed, float ySpeed)
        {
            if (!Main.enabled) return true;
            try
            {
                if (Main.settings.lessAccurateDrunkSeven)
                {
                    if (Traverse.Create(__instance).Field("martinisDrunk").GetValue<int>() > 2)
                    {
                        int randY = UnityEngine.Random.Range(-25, 25);
                        __instance.gunSprite.SetLowerLeftPixel((float)(32 * 3), 32f);
                        EffectsController.CreateMuzzleFlashEffect(x, y, -25f, xSpeed * 0.01f, ySpeed * 0.01f, __instance.transform);
                        ProjectileController.SpawnProjectileLocally(__instance.projectile, __instance, x, y, xSpeed, ySpeed + randY, __instance.playerNum);
                        return false;
                    }
                }
            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog("Failed to make 007 more drunk", ex); }
            return true;
        }
    }

    // BroneyRoss Patch
    [HarmonyPatch(typeof(BroneyRoss), "Awake")]
    static class BroneyRoss_Awake_Patch
    {
        static void Postfix(BroneyRoss __instance)
        {
            if (!Main.enabled || (Compatibility.ExpendablesBros.i.IsEnabled)) return;
            try
            {
                if (Main.settings.fixExpendabros)
                {
                    TestVanDammeAnim broHard = HeroController.GetHeroPrefab(HeroType.BroHard);
                    __instance.soundHolder.attackSounds = broHard.soundHolder.attackSounds;
                }
            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog("Failed to patch Broney Ross", ex); }
        }
    }

    // LeeBroxmas Patch
    [HarmonyPatch(typeof(LeeBroxmas), "Awake")]
    static class LeeBroxmas_Awake_Patch
    {
        static void Postfix(LeeBroxmas __instance)
        {
            if (!Main.enabled || (Compatibility.ExpendablesBros.i.IsEnabled)) return;

            try
            {
                if (Main.settings.fixExpendabros)
                {
                    TestVanDammeAnim blade = HeroController.GetHeroPrefab(HeroType.Blade);
                    Texture bladeKnifeTex = (blade as Blade).throwingKnife.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture;

                    __instance.projectile.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = bladeKnifeTex;
                    __instance.macheteSprayProjectile.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = bladeKnifeTex;
                }
            }
            catch(Exception ex) { Main.bmod.logger.ExceptionLog("Failed to patch Lee Broxmas", ex); }
        }
    }
    [HarmonyPatch(typeof(LeeBroxmas), "AnimatePushing")]
    static class LeeBroxmas_Pushing_Patch
    {
        static void Postfix(LeeBroxmas __instance)
        {
            if (!Main.enabled) return;
            try
            {
                if (Main.settings.fixPushingAnimation)
                {
                    __instance.gunSprite.transform.localPosition = TFP_Utility.GetBroGunVector3PositionWhilePushing(HeroType.LeeBroxmas);
                    float pushingTime = Traverse.Create(__instance).Field("pushingTime").GetValue<float>();
                    if (__instance.fire || pushingTime <= 0)
                    {
                        __instance.gunSprite.transform.localScale = new Vector3(-1f, 1f, 1f);
                        __instance.gunSprite.transform.localPosition = TFP_Utility.GetBroGunVector3PositionWhenFinishPushing(__instance.heroType);
                    }
                }
            }catch(Exception ex) { Main.bmod.logger.ExceptionLog("Failed to patch Lee Broxmas Pushing", ex); }
        }
    }

    // Patch Bronnar Jensen
    [HarmonyPatch(typeof(BronnarJensen), "UseFire")]
    static class BronnarJensen_FixShoot_Patch
    {
        static bool Prefix(BronnarJensen __instance)
        {
            if (!Main.enabled || (Compatibility.ExpendablesBros.i.IsEnabled)) return true;

            try
            {
                if (Main.settings.fixExpendabros)
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
                        Traverse.Create(__instance).Method("PlayAttackSound", new object[] { 0.4f }).GetValue();
                    }
                    Map.DisturbWildLife(__instance.X, __instance.Y, 60f, __instance.playerNum);
                    __instance.fireDelay = 0.6f;
                    return false;
                }
            }
            catch(Exception ex) { Main.bmod.logger.ExceptionLog("Failed to shoot at Lee Broxmas's foot", ex); }
            return true;
        }
    }

    // TrentBroser Patch
    [HarmonyPatch(typeof(TrentBroser), "Awake")]
    static class TrentBroser_Awake_Patch
    {
        static void Postfix(TrentBroser __instance)
        {
            if (!Main.enabled || (Compatibility.ExpendablesBros.i.IsEnabled)) return;

            try
            {
                if (Main.settings.fixExpendabros)
                {
                    TestVanDammeAnim broDredd = HeroController.GetHeroPrefab(HeroType.BroDredd);
                    __instance.soundHolder.attackSounds = broDredd.soundHolder.attackSounds;

                    TestVanDammeAnim brodellWalker = HeroController.GetHeroPrefab(HeroType.BrodellWalker);
                    __instance.specialGrenade = brodellWalker.specialGrenade;
                }
            }
            catch (Exception ex)
            {
                Main.bmod.logger.ExceptionLog("Failed to patch Trent Broser", ex);
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

            try
            {
                if (Main.settings.fixPushingAnimation)
                {
                    __instance.gunSprite.transform.localPosition = TFP_Utility.GetBroGunVector3PositionWhilePushing(HeroType.Brochete);
                    float pushingTime = Traverse.Create(__instance).Field("pushingTime").GetValue<float>();
                    if (__instance.fire || pushingTime <= 0)
                    {
                        __instance.gunSprite.transform.localScale = new Vector3(-1f, 1f, 1f);
                        __instance.gunSprite.transform.localPosition = TFP_Utility.GetBroGunVector3PositionWhenFinishPushing(__instance.heroType);
                    }
                }
            }
            catch(Exception ex) { Main.bmod.logger.ExceptionLog("Failed to patch Brochete pushing", ex); }
        }
    }
    [HarmonyPatch(typeof(Brochete), "Update")]
    static class Brochete_Update_Patch
    {
        static void Postfix(Brochete __instance)
        {
            if (!Main.enabled) return;

            Traverse.Create(__instance).Field("test6Frames").SetValue(Main.settings.brocheteAlternateSpecialAnim);
        }
    }
}
