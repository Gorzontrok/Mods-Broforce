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
                    Main.Wait(20);
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
                __instance.health = 50;
               /*
                __instance.GetComponent<Renderer>().sharedMaterial = MookDogUpdate_Patch.normalDog;

                Traverse.Create(typeof(MookDog)).Field("spritePixelWidth").SetValue(32);
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
            ProjectileController.SpawnProjectileLocally(__instance.projectile, __instance, x, y, xSpeed * 0.9f, ySpeed + 2f + UnityEngine.Random.value * 15f, __instance.firingPlayerNum);
            ProjectileController.SpawnProjectileLocally(__instance.projectile, __instance, x, y, xSpeed * 0.9f, ySpeed - 2f - UnityEngine.Random.value * 15f, __instance.firingPlayerNum);
            ProjectileController.SpawnProjectileLocally(__instance.projectile, __instance, x, y, xSpeed * 0.85f, ySpeed - 40f - UnityEngine.Random.value * 35f, __instance.firingPlayerNum);
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
                    Main.Wait(2);
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
            return false;
        }
    }

    // Patch CR-666
    [HarmonyPatch(typeof(DolphLundrenSoldier), "UseSpecial")]
    static class DolphLundrenSoldier_UseSpecial_Patch
    {
        static void Postfix(DolphLundrenSoldier __instance)
        {
            for (int i = 0; i < 5; i++)
            {
                float num = 128f;
                float num2 = 32f;
                bool playerRange = __instance.enemyAI.GetPlayerRange(ref num, ref num2);
                __instance.PlaySpecialAttackSound(0.35f);
                float num3 = 100f + UnityEngine.Random.value * 10f;
                float num4 = 180f;
                if (playerRange)
                {
                    float num5 = Mathf.Clamp((50f + num * 0.7f + num2 * 0.33f) * __instance.grenadeTossDistanceSpeedM, 0.5f, 1.5f);
                    num3 *= num5;
                    num4 *= num5;
                }
                if (__instance.IsMine)
                {
                    ProjectileController.SpawnGrenadeOverNetwork(__instance.specialGrenade, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 18f, __instance.Y + 22f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * num3, num4, __instance.playerNum);
                }
            }
        }
    }
    [HarmonyPatch(typeof(DolphLundrenSoldier), "FireWeapon")]
    static class DolphLundrenSoldier_FireWeapon_Patch
    {
        static  void Postfix(DolphLundrenSoldier __instance, float x, float y, float xSpeed, float ySpeed)
        {
            for(int i = 0; i<3; i++)
            {
                EffectsController.CreateMuzzleFlashEffect(x, y, -25f, xSpeed * 0.01f, ySpeed * 0.01f, __instance.transform);
                EffectsController.CreateShrapnel(__instance.bulletShell, x + __instance.transform.localScale.x * -6f, y, 1f, 30f, 1f, -__instance.transform.localScale.x * 55f, 130f);
                ProjectileController.SpawnProjectileLocally(__instance.projectile, __instance, x, y, xSpeed, ySpeed, __instance.firingPlayerNum);
            }
        }
    }
    [HarmonyPatch(typeof(DolfLundgrenAI), "RunBombardment")]
    static class DolfLundgrenAI_RunBombardment_Patch
    {
        static void Postfix(DolfLundgrenAI __instance)
        {
            for(int i =0; i<2; i++)
            {
                Main.Wait(10);
                List<Vector3> bombardmentPositions = Traverse.Create(typeof(DolfLundgrenAI)).Field("bombardmentPositions").GetValue() as List<Vector3>;
                Vector3 vector = bombardmentPositions[0];
                ProjectileController.SpawnProjectileLocally(ProjectileController.instance.shellBombardment, SingletonMono<MapController>.Instance, vector.x + 300f, vector.y + 450f, -187.5f, -281.25f, -1);
            }
            
        }
    }

    // Patch Mecha
    [HarmonyPatch(typeof(MookArmouredGuy), "PressHighFiveMelee")]
    static class MookArmouredGuy_PressHighFiveMelee_Patch
    {
        static bool Prefix(MookArmouredGuy __instance, bool forceHighFive = false)
        {
            if (Main.HardMode && __instance.pilotUnit.IsMine || __instance.hasBeenPiloted)
                return false;

            return true;
        }
    }
    [HarmonyPatch(typeof(MookArmouredGuy), "FireWeapon")]
    static class MookArmouredGuy_FireWeapon_Patch
    {
        static void Postfix(MookArmouredGuy __instance , float x, float y, float xSpeed, float ySpeed)
        {
            ProjectileController.SpawnProjectileLocally(__instance.projectile, __instance, x, y, xSpeed * 0.9f, ySpeed + 2f + UnityEngine.Random.value * 15f, __instance.firingPlayerNum);
            ProjectileController.SpawnProjectileLocally(__instance.projectile, __instance, x, y, xSpeed * 0.9f, ySpeed - 2f - UnityEngine.Random.value * 15f, __instance.firingPlayerNum);
        }
    }
}
