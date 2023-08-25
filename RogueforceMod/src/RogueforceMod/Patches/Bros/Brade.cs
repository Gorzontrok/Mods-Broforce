using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using World.Generation.MapGenV4;

namespace RogueforceMod.Patches.Bros.Brade
{
    [HarmonyPatch(typeof(Blade), "UseFire")]
    static class UseFire_Patch
    {
        static bool Prefix(Blade __instance)
        {
            try
            {
                __instance.GetFieldValue<List<Unit>>("alreadyHit").Clear();
                __instance.SetFieldValue("hasHitWithWall", false);
                __instance.SetFieldValue("hasHitWithSlice", false);
                __instance.SetFieldValue("hasTriedToSpawnProjectile", false);
                __instance.SetFieldValue("gunFrame", 6);
                __instance.SetFieldValue("hasPlayedAttackHitSound", false);
                __instance.CallMethod("FireWeapon", __instance.X + __instance.transform.localScale.x * 10f, __instance.Y + 6.5f, __instance.transform.localScale.x * (float)(250 + ((!Demonstration.bulletsAreFast) ? 0 : 150)), (float)(UnityEngine.Random.Range(0, 40) - 20) * ((!Demonstration.bulletsAreFast) ? 0.2f : 1f));
                __instance.CallMethod("PlayAttackSound");
                Map.DisturbWildLife(__instance.X, __instance.Y, 60f, __instance.playerNum);
                if (ProcGenGameMode.UseProcGenRules)
                {
                    __instance.SetFieldValue("speedBoostTime", 0.09f);
                    __instance.SetFieldValue("pauseTime", 0f);
                    switch (HeroController.GetPrimaryFireLevel(__instance.playerNum))
                    {
                        case 0:
                            __instance.SetFieldValue("speedBoostM", 1.25f);
                            break;
                        case 1:
                            __instance.SetFieldValue("speedBoostM", 1.4f);
                            break;
                        case 2:
                            __instance.SetFieldValue("speedBoostM", 1.6f);
                            break;
                        case 3:
                            __instance.SetFieldValue("speedBoostM", 1.75f);
                            break;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                Main.Log(e);
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(Blade), "DealDamageToGround")]
    static class DealDamageToGround_Patch
    {
        static bool Prefix(Blade __instance, float x, float y, Vector3 point, Collider hitCollider)
        {
            try
            {
                __instance.CallMethod("MakeEffects", point.x, point.y);
                bool flag = hitCollider.GetComponent<Cage>();
                int num = __instance.groundSwordDamage;
                if (ProcGenGameMode.UseProcGenRules)
                {
                    int primaryFireLevel = HeroController.GetPrimaryFireLevel(__instance.playerNum);
                    num = Mathf.Clamp(-1 + 2 * primaryFireLevel, 1, 100);
                }
                MapController.Damage_Local(__instance, hitCollider.gameObject, num + ((!flag) ? 0 : 5), DamageType.Bullet, __instance.xI, 0f, x, y);
                if (!__instance.GetBool("hasHitWithWall"))
                {
                    SortOfFollow.Shake(0.15f);
                }
                __instance.SetFieldValue("hasHitWithWall", true);
                return false;
            }
            catch(Exception e)
            {
                Main.Log(e);
            }
            return true;
        }
    }
}
