using System;
using RocketLib;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace BroforceOverhaul.Bros.DoubleBroSeven0
{
    // Add the Fifth Special
    [HarmonyPatch(typeof(DoubleBroSeven), "Awake")]
    static class DoubleBroSeven_FifthSpecial_Patch
    {
        static void Postfix(DoubleBroSeven __instance)
        {
            if (Main.enabled)
            {
                try
                {
                    __instance.originalSpecialAmmo = 5;
                    __instance.SpecialAmmo = 5;
                }

                catch (Exception ex)
                {
                    Main.bmod.logger.ExceptionLog("Failed to add 5th bond's special.", ex);
                }
            }
        }
    }
    // Throw teargas at feet if crouch
    [HarmonyPatch(typeof(DoubleBroSeven), "UseSpecial")]
    static class DoubleBroSeven_TearGasAtFeet_Patch
    {
        static bool Prefix(DoubleBroSeven __instance)
        {
            if (Main.enabled)
            {
                try
                {
                    Traverse t = Traverse.Create(__instance);
                    DoubleBroSevenSpecialType currentSpecialType = t.Field("currentSpecialType").GetValue<DoubleBroSevenSpecialType>();
                    if (currentSpecialType == DoubleBroSevenSpecialType.TearGas)
                    {
                        Networking.Networking.RPC<float>(PID.TargetAll, new RpcSignature<float>(__instance.PlayThrowLightSound), 0.5f, false);
                        if (__instance.IsMine)
                        {
                            if (t.Field("ducking").GetValue<bool>() && __instance.down)
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
                catch (Exception ex)
                {
                    Main.ExceptionLog("Failed to patch teargas at feet", ex);
                }
            }
            return true;
        }
    }

    // less accurate if drunk
    [HarmonyPatch(typeof(DoubleBroSeven), "FireWeapon")]
    static class DoubleBroSeven_LessAccurateIfDrunk_Patch
    {
        static bool Prefix(DoubleBroSeven __instance, float x, float y, float xSpeed, float ySpeed)
        {
            if (Main.enabled)
            {
                try
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
                catch (Exception ex)
                {
                    Main.ExceptionLog("Failed to make 007 more drunk", ex);
                }
            }
            return true;
        }
    }
}

