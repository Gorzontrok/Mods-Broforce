using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace Surprise.Terrorist.Patch
{
    // Every Mook
    [HarmonyPatch(typeof(Mook), "Start")]
    static class StartMook_Patch
    {
        static void Prefix(Mook __instance)
        {
            if (Main.enabled)
            {
                if (Main.HardMode)
                {
                    __instance.canBeAssasinated = false;
                    __instance.willPanicWhenOnFire = false;
                    __instance.canBeCoveredInAcid = false;
                }

                // Patch Dog start
                if (__instance as MookDog)
                {
                    (__instance as MookDog).awareMegaRunSpeed = 190f;
                    (__instance as MookDog).awareRunSpeed = 175;
                }
            }
        }
    }
    [HarmonyPatch(typeof(Mook), "FireWeapon")]
    static class ShotgunMook_Patch
    {
        static bool Prefix(Mook __instance, float x, float y, float xSpeed, float ySpeed)
        {
            if (Main.enabled)
            {
                ShootGrenade shootGrenade = __instance.GetComponent<ShootGrenade>();
                if (shootGrenade != null)
                {
                    shootGrenade.shootCounter++;
                    if (shootGrenade.shootCounter % 5 == 0)
                    {
                        shootGrenade.ThrowGrenade(__instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 8f, __instance.Y + 8f, Mathf.Sign(__instance.transform.localScale.x) * 250f, 150f, __instance.playerNum);
                        return false;
                    }
                }
            }
            return true;
        }
    }

    // Basic terrorist
    [HarmonyPatch(typeof(MookTrooper), "Awake")]
    static class MookTrooper_Start_Patch
    {
        static void Postfix(MookTrooper __instance)
        {
            if(Main.enabled && !(__instance as MookJetpack) && !(__instance as MookBazooka))
            {
                __instance.gameObject.AddComponent<ShootGrenade>();
            }
        }
    }

    //Patch the grenadier
    [HarmonyPatch(typeof(MookGrenadier), "Start")]
    static class Grenadier_Start_Patch
    {
        static void Postfix(MookGrenadier __instance)
        {
            __instance.gameObject.AddComponent<GrenadorThrow2Grenade>().thrower = __instance;

        }
    }
    [HarmonyPatch(typeof(MookGrenadier), "AnimateThrowingHeldObject")]
    static class __2GrenadeThrowed_Patch
    {
        static bool Prefix(MookGrenadier __instance)
        {
            try
            {
                if(Main.enabled)
                {
                    Traverse trav = Traverse.Create(__instance);
                    if (__instance.Y > __instance.groundHeight + 1f)
                    {
                        trav.Field("usingGrenade").SetValue(false);
                        trav.Method("AnimateIdle").GetValue();
                    }
                    trav.Method("DeactivateGun").GetValue();
                    int num = 17 + Mathf.Clamp(__instance.frame, 0, 7);
                    trav.Field("frameRate").SetValue(0.045f);
                    trav.Field("sprite").GetValue<SpriteSM>().SetLowerLeftPixel((float)(num * trav.Field("spritePixelWidth").GetValue<int>()), (float)(trav.Field("spritePixelHeight").GetValue<int>() * 5));
                    if (__instance.frame == 5)
                    {
                        GrenadorThrow2Grenade grenadorThrow2Grenade = __instance.GetComponent<GrenadorThrow2Grenade>();
                        if(grenadorThrow2Grenade != null)
                        {
                            grenadorThrow2Grenade.Call(__instance);
                        }
                    }
                    if (__instance.frame >= 7)
                    {
                        trav.Field("usingGrenade").SetValue(false);
                    }
                    return false;
                }
            }
            catch(Exception ex)
            {
               Main.Log(ex);
            }
            return true;
        }
    }

    //Patch MookBazooka
    [HarmonyPatch(typeof(MookBazooka), "FireWeapon")]
    static class MookBazooka_FireWeapon_Patch
    {
        static void Postfix(MookBazooka __instance, float x, float y, float xSpeed, float ySpeed)
        {

            Projectile projectile = ProjectileController.SpawnProjectileOverNetwork(__instance.projectile, __instance, x, y, xSpeed, ySpeed, false, -1, false, true, 0f);
            int seenPlayerNum = __instance.enemyAI.GetSeenPlayerNum();
            if (seenPlayerNum >= 0)
            {
                Networking.Networking.RPC<float, float, int>(PID.TargetAll, true, false, false, new RpcSignature<float, float, int>(projectile.Target), projectile.transform.position.x + __instance.transform.localScale.x * 800f, projectile.transform.position.y, seenPlayerNum);
            }

            projectile = ProjectileController.SpawnProjectileOverNetwork(__instance.projectile, __instance, x, y, xSpeed, ySpeed, false, -1, false, true, 0f);
            seenPlayerNum = __instance.enemyAI.GetSeenPlayerNum();
            if (seenPlayerNum >= 0)
            {
                Networking.Networking.RPC<float, float, int>(PID.TargetAll, true, false, false, new RpcSignature<float, float, int>(projectile.Target), projectile.transform.position.x + __instance.transform.localScale.x * 800f, projectile.transform.position.y, seenPlayerNum);
            }


            projectile = ProjectileController.SpawnProjectileOverNetwork(__instance.projectile, __instance, x, y, xSpeed, ySpeed, false, -1, false, true, 0f);
            seenPlayerNum = __instance.enemyAI.GetSeenPlayerNum();
            if (seenPlayerNum >= 0)
            {
                Networking.Networking.RPC<float, float, int>(PID.TargetAll, true, false, false, new RpcSignature<float, float, int>(projectile.Target), projectile.transform.position.x + __instance.transform.localScale.x * 800f, projectile.transform.position.y, seenPlayerNum);
            }

            projectile = ProjectileController.SpawnProjectileOverNetwork(__instance.projectile, __instance, x, y, xSpeed, ySpeed, false, -1, false, true, 0f);
            seenPlayerNum = __instance.enemyAI.GetSeenPlayerNum();
            if (seenPlayerNum >= 0)
            {
                Networking.Networking.RPC<float, float, int>(PID.TargetAll, true, false, false, new RpcSignature<float, float, int>(projectile.Target), projectile.transform.position.x + __instance.transform.localScale.x * 800f, projectile.transform.position.y, seenPlayerNum);
            }
        }
    }

    //Patch MookGeneral
    [HarmonyPatch(typeof(MookGeneral), "UseSpecial")]
    static class MookGeneral_SpawnMook_Patch
    {
        static void Prefix(MookGeneral __instance)
        {
            Map.AlertNearbyMooks(__instance.transform.position.x, __instance.transform.position.y, 64f, 40f, __instance.enemyAI.GetSeenPlayerNum());
        }
    }

    // Patch MookRiotShield
    [HarmonyPatch(typeof(MookRiotShield), "DisarmShield")]
    static class MookRiotShield_BackSomersault_Patch
    {
        static void Postfix(MookRiotShield __instance)
        {
            __instance.specialGrenade = (Map.Instance.activeTheme.mookGrenadier as MookGrenadier).specialGrenade;
            ProjectileController.SpawnGrenadeOverNetwork(__instance.specialGrenade, __instance, __instance.X, __instance.Y + 4f, 0.001f, 0.011f, __instance.xI * 0.3f / __instance.specialGrenade.weight, __instance.yI * 0.5f / __instance.specialGrenade.weight + 110f, __instance.playerNum, 1f);
        }
    }

    // Patch JetPack
    [HarmonyPatch(typeof(MookJetpack), "Death")]
    static class DropGrenadeOnDeath_Patch
    {
        static void Postfix(MookJetpack __instance)
        {
            try
            {
                __instance.specialGrenade = (Map.Instance.activeTheme.mookGrenadier as MookGrenadier).specialGrenade;
                ProjectileController.SpawnGrenadeOverNetwork(__instance.specialGrenade, __instance, __instance.X, __instance.Y + 4f, 0.001f, 0.011f, __instance.xI * 0.3f / __instance.specialGrenade.weight, __instance.yI * 0.5f / __instance.specialGrenade.weight + 110f, __instance.playerNum, 1f);

            }catch(Exception ex)
            {
                Main.Log(ex);
            }
        }
    }
}
