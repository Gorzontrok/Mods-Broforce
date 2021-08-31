using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace Surprise
{
    // Patch Alien spawn after facehugger insemination
    [HarmonyPatch(typeof(TestVanDammeAnim), "BurstChest")]
    static class TestVanDammeAnim_BurstChest_Patch
    {
        static void Postfix(TestVanDammeAnim __instance)
        {
            int maxAlien = 3;
            if (Main.HardMode) maxAlien = 5;
            for(int i =0; i < maxAlien;i++)
            {
                Mook component = __instance.alienBursterPrefab.GetComponent<Mook>();
                Mook mook = MapController.SpawnMook_Networked(component, __instance.X, __instance.Y, 0f, 0f, false, false, false, false, false);
                Networking.Networking.RPC<Mook, float, float, bool, int>(PID.TargetAll, new RpcSignature<Mook, float, float, bool, int>(__instance.ReleaseAlien), mook, 0f, 0f, false, HeroController.GetNearestPlayer(__instance.X, __instance.Y, 160f, 64f), true);
                Networking.Networking.RPC(PID.TargetAll, new RpcSignature(mook.GrowFromChestBurster), true);
            }
            
        }
    }

    // Patch AlienMelter
    [HarmonyPatch(typeof(AlienMelter), "MakeEffects")]
    static class AlienMelter_MakeEffects_Patch
    {
        static void Postfix(AlienMelter __instance)
        {
            for(int i = 0; i< 3; i++)
            {
                EffectsController.CreateExplosionRangePop(__instance.X, __instance.Y + 6f, -1f, __instance.explodeRange * __instance.broHarmRangeM * 2.4f);
                EffectsController.CreateSlimeParticlesSpray(__instance.bloodColor, __instance.X, __instance.Y + 6f, 1f, 34, 6f, 5f, 300f, __instance.xI * 0.6f, __instance.yI * 0.2f + 150f, 0.6f);
                EffectsController.CreateSlimeExplosion(__instance.X, __instance.Y, 15f, 15f, 140f, 0f, 0f, 0f, 0f, 0, 20, 120f, 0f, Vector3.up, BloodColor.Green);
                EffectsController.CreateSlimeCover(15, __instance.X, __instance.Y + 8f, 60f, false);
            }
        }
    }

    // Patch AlienWormFacehuggerLauncher
    [HarmonyPatch(typeof(AlienWormFacehuggerLauncher), "FireProjectile")]
    static class AlienWormFacehuggerLauncher_FireProjectile_Patch
    {
        static void Postfix(AlienWormFacehuggerLauncher __instance)
        {
            for (int i = 0; i < 5; i++)
            {
                float currentFireSpeed = Traverse.Create(typeof(AlienWormFacehuggerLauncher)).Field("currentFireSpeed").GetValue<float>();
                Vector3 right = __instance.headTransform.right;
                Vector3 vector = new Vector3(right.x * currentFireSpeed * (0.85f + UnityEngine.Random.value * 0.3f), right.y * currentFireSpeed, 0f);
                Vector3 pos = __instance.headTransform.position + __instance.headTransform.right * 14f - __instance.headTransform.up * -10f;

                Mook mookPrefab = (Mook)Map.Instance.activeTheme.alienFaceHugger;

                Mook mook = MapController.SpawnMook_Networked(mookPrefab, pos.x, pos.y, 0f, 0f, false, false, false, false, false);

                Networking.Networking.RPC<float, float>(PID.TargetAll, new RpcSignature<float, float>(mook.Launch), vector.x, vector.y, false);
            }
        }
    }

    // Patch AlienMinibossSandWorm
    [HarmonyPatch(typeof(AlienMinibossSandWorm), "FireProjectile")]
    static class AlienMinibossSandWorm_FireProjectile_Patch
    {
        static void Postfix(AlienMinibossSandWorm __instance)
        {
            for(int i = 0; i< 5; i++)
            {
                float currentFireSpeed = Traverse.Create(typeof(AlienWormFacehuggerLauncher)).Field("currentFireSpeed").GetValue<float>();
                Vector3 right = __instance.headTransform.right;
                Vector3 vector = __instance.headTransform.position + __instance.headTransform.right * __instance.headFirePointOffset.x + __instance.headTransform.up * __instance.headFirePointOffset.y;
                ProjectileController.SpawnProjectileLocally(__instance.projectile, __instance, vector.x, vector.y, right.x * currentFireSpeed * (1f - __instance.projectileRandomXIFactor / 2f + UnityEngine.Random.value * __instance.projectileRandomXIFactor), right.y * currentFireSpeed, -2, false, -1f);
            }
        }
    }
}
