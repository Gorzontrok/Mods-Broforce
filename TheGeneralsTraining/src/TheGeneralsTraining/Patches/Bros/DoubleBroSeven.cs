using System;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace TheGeneralsTraining.Patches.Bros.DoubleBroSeven0
{
    // Add the Fifth Special
    [HarmonyPatch(typeof(DoubleBroSeven), "Awake")]
    static class DoubleBroSeven_FifthSpecial_Patch
    {
        static void Prefix(DoubleBroSeven __instance)
        {
            if (Main.CanUsePatch && Main.settings.fifthBondSpecial)
            {
                try
                {
                    __instance.originalSpecialAmmo = 5;
                }

                catch (Exception ex)
                {
                    Main.ExceptionLog("Failed to add 5th bond's special.", ex);
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
            if (Main.CanUsePatch && Main.settings.fifthBondSpecial)
            {
                try
                {
                    Traverse t = __instance.GetTraverse();
                    DoubleBroSevenSpecialType currentSpecialType = t.GetFieldValue<DoubleBroSevenSpecialType>("currentSpecialType");
                    if (currentSpecialType == DoubleBroSevenSpecialType.TearGas)
                    {
                        Networking.Networking.RPC(PID.TargetAll, new RpcSignature<float>(__instance.PlayThrowLightSound), 0.5f, false);
                        if (__instance.IsMine)
                        {
                            if (t.GetFieldValue<bool>("ducking") && __instance.down)
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
                    Main.ExceptionLog(ex);
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
            if (Main.CanUsePatch && Main.settings.drunkSeven)
            {
                try
                {
                    if (__instance.GetFieldValue<int>("martinisDrunk") > 2)
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
                    Main.ExceptionLog("Failed to make 007 drunker", ex);
                }
            }
            return true;
        }
    }
}

