using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace Surprise
{
    // Patch the basic mook
    [HarmonyPatch(typeof(Mook), "FireWeapon")]
    static class ShotgunMook_Patch
    {
        static void Postfix(Mook __instance, float x, float y, float xSpeed, float ySpeed)
        {
            try
            {
                ProjectileController.SpawnProjectileLocally(__instance.projectile, __instance, x, y, xSpeed * 0.83f, ySpeed + 40f + UnityEngine.Random.value * 35f, __instance.firingPlayerNum);
                ProjectileController.SpawnProjectileLocally(__instance.projectile, __instance, x, y, xSpeed * 0.9f, ySpeed + 2f + UnityEngine.Random.value * 15f, __instance.firingPlayerNum);
                ProjectileController.SpawnProjectileLocally(__instance.projectile, __instance, x, y, xSpeed * 0.9f, ySpeed - 2f - UnityEngine.Random.value * 15f, __instance.firingPlayerNum);
                ProjectileController.SpawnProjectileLocally(__instance.projectile, __instance, x, y, xSpeed * 0.85f, ySpeed - 40f - UnityEngine.Random.value * 35f, __instance.firingPlayerNum);
                ProjectileController.SpawnProjectileLocally(__instance.projectile, __instance, x, y, xSpeed * 0.85f, ySpeed - 50f + UnityEngine.Random.value * 80f, __instance.firingPlayerNum);
            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }

    //Patch the grenadier
    [HarmonyPatch(typeof(MookGrenadier), "DropDeathGrenade")]
    static class MoreDeathGrenade_Patch
    {
        static void Postfix(MookGrenadier __instance)
        {
            if (__instance.IsMine)
            {
                for (int i = 0; i < 6; i++)
                {
                    ProjectileController.SpawnGrenadeOverNetwork(__instance.specialGrenade, __instance, __instance.X, __instance.Y + 4f, 0.001f, 0.011f, __instance.xI * 0.3f / __instance.specialGrenade.weight, __instance.yI * 0.5f / __instance.specialGrenade.weight + 110f, __instance.playerNum);
                    
                }
            }
        }
    }
    [HarmonyPatch(typeof(MookGrenadier), "AnimateThrowingHeldObject")]
    static class MoreGrenade_Patch
    {
        static void Postfix(MookGrenadier __instance)
        {
            if (__instance.IsMine)
            {
                float num2 = 128f;
                float num3 = 32f;
                float num4 = 130f;
                float num5 = 130f;

                float num6 = Mathf.Clamp((__instance.grenadeTossDistanceSpeedMinValue + num2 * __instance.grenadeTossXRangeM + num3 * __instance.grenadeTossYRangeM) * __instance.grenadeTossDistanceSpeedM, 0.5f, 1.5f);

                num6 = num6 * (1f - __instance.grenadeTossV / 2f) + __instance.grenadeTossV * UnityEngine.Random.value;
                num4 *= num6;
                num5 *= num6;

                ProjectileController.SpawnGrenadeOverNetwork(__instance.longFuseGrenade, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 8f, __instance.Y + 24f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * num4, num5, __instance.playerNum);
            }
        }
    }

    //Patch MookDog
    [HarmonyPatch(typeof(MookDog), "TransformIntoMegaDog")]
    static class MookDog_MegaTransformation_Patch
    {
        static void Postfix(MookDog __instance)
        {
            if(Main.HardMode)
            {
                __instance.GetComponent<Renderer>().sharedMaterial = MookDogUpdate_Patch.normalDog;
            }
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

    // Patch Bigguy
    [HarmonyPatch(typeof(MookBigGuy), "FireWeapon")]
    static class MookBigGuy_FireWeapon_Patch
    {
        static void Postfix(MookBigGuy __instance, float x, float y, float xSpeed, float ySpeed)
        {

        }
    }

    //Patch MookGeneral
    [HarmonyPatch(typeof(MookGeneral), "SpawnMook")]
    static class MookGeneral_SpawnMook_Patch
    {
        static bool Prefix(MookGeneral __instance, Mook prefab, float x, float y)
        {
            int i = 3;
            if (prefab != null)
            {
                if(Main.HardMode)
                {
                    i = 5;
                }

                int j = 0;
                while(j<i)
                {
                    MapController.SpawnMook_Networked(prefab, x, y, 0f, 0f, false, false, true, false, true);
                    j++;
                }
            }

            return true;
        }
    }

    // Patch MookRiotShield
    [HarmonyPatch(typeof(MookRiotShield), "BackSomersault")]
    static class MookRiotShield_BackSomersault_Patch
    {
        static bool Prefix(MookRiotShield __instance, bool forceTumble)
        {
            __instance.hasShield = true;
            __instance.BackSomersault(forceTumble);
            return true;
        }
    }
}
