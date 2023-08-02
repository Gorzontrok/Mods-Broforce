using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace Surprise.Aliens.Patch
{
    // Patch Alien spawn after facehugger insemination
    [HarmonyPatch(typeof(TestVanDammeAnim), "BurstChest")]
    static class TestVanDammeAnim_BurstChest_Patch
    {
        static bool Prefix(TestVanDammeAnim __instance)
        {
            try
            {
                if(Main.enabled)
                {
                    __instance.DisConnectFaceHugger();
                    if (__instance.alienBursterPrefab == null)
                    {
                        __instance.alienBursterPrefab = Map.Instance.activeTheme.alienXenomorph;
                    }
                    if ((__instance as MookBigGuy || UnityEngine.Random.value < 0.2f))
                    {
                        __instance.alienBursterPrefab = Map.Instance.activeTheme.alienBrute;
                    }
                    else
                    {
                        __instance.alienBursterPrefab = Map.Instance.activeTheme.alienXenomorph;
                    }
                    bool flag = false;
                    if (__instance.IsHero && __instance.IsMine)
                    {
                        flag = true;
                    }
                    else if (Traverse.Create(__instance).Field("inseminatedByLocalUnit").GetValue<bool>())
                    {
                        flag = true;
                    }
                    if (flag)
                    {
                        Mook component =  __instance.alienBursterPrefab.GetComponent<Mook>();
                        Mook mook = MapController.SpawnMook_Networked(component, __instance.X, __instance.Y, 0f, 0f, false, false, false, false, false);
                        Networking.Networking.RPC<Mook, float, float, bool, int>(PID.TargetAll, new RpcSignature<Mook, float, float, bool, int>(__instance.ReleaseAlien), mook, 0f, 0f, false, HeroController.GetNearestPlayer(__instance.X, __instance.Y, 160f, 64f), true);
                        Networking.Networking.RPC(PID.TargetAll, new RpcSignature(mook.GrowFromChestBurster), true);
                    }
                    Traverse.Create(__instance).Method("Gib", new object[] { DamageType.Crush, 0f, 100f }).GetValue();
                    __instance.PlayChestBurstSound(1f);
                }
                return false;
            }
            catch(Exception ex)
            {
                Main.Log(ex);
            }
            return true;
        }
    }

    // Patch AlienMelter
    [HarmonyPatch(typeof(AlienMelter), "Awake")]
    static class AlienMelter_Awake_Patch
    {
        static void Postfix(AlienMelter __instance)
        {
            __instance.gameObject.AddComponent<MelterMakeEffectDuringRolling>();
        }
    }
    [HarmonyPatch(typeof(AlienMelter), "Update")]
    static class AlienMelter_MakeEffects_Patch
    {
        static void Postfix(AlienMelter __instance)
        {

            if (__instance.deathCounter > 0f)
            {
                MelterMakeEffectDuringRolling action = __instance.GetComponent<MelterMakeEffectDuringRolling>();
                if(action != null)
                {
                    action.Call(__instance);
                }
            }
            if (Traverse.Create(__instance).Field("exploded").GetValue<bool>())
            {
                MelterMakeEffectDuringRolling action = __instance.GetComponent<MelterMakeEffectDuringRolling>();
                if (action != null)
                {
                    action.Stop();
                }
            }
        }
    }

    // Patch AlienWormFacehuggerLauncher
   /* [HarmonyPatch(typeof(AlienWormFacehuggerLauncher), "FireProjectile")]
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
    }*/
}
