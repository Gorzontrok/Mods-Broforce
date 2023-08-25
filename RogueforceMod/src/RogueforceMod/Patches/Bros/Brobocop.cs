using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using World.Generation.MapGenV4;

namespace RogueforceMod.Patches.Bros.Brobocop0
{
    /* [HarmonyPatch(typeof(Brobocop), "Update")]
     static class Update_Patch
     {
         static bool Prefix(Brobocop __instance)
         {
             if (__instance.specialCooldownTimer < __instance.specialCooldownDelay)
             {
                 __instance.specialCooldownTimer += Time.deltaTime;
             }
             __instance.Update();
             if (__instance.usingTargetingSystem && __instance.targetSystem != null)
             {
                 Unit nextClosestUnit = Map.GetNextClosestUnit(__instance.playerNum, __instance.targetSystem.TravelDirection, __instance.scanningRange, __instance.scanningRange, __instance.targetSystem.transform.position.x, __instance.targetSystem.transform.position.y, __instance.currentTargetingUnitStreak);
                 if (nextClosestUnit != null)
                 {
                     __instance.targettedUnits.Add(nextClosestUnit);
                     __instance.currentTargetingUnitStreak.Add(nextClosestUnit);
                     __instance.currentTargetingStreakTime = 0f;
                     Networking.RPC<Unit>(PID.TargetAll, new RpcSignature<Unit>(__instance.CreateTargetOnUnit), nextClosestUnit, false);
                 }
                 else
                 {
                     TargetableObject nextClosestTargetableObject = Map.GetNextClosestTargetableObject(__instance.playerNum, __instance.targetSystem.TravelDirection, __instance.scanningRange, __instance.scanningRange, __instance.targetSystem.transform.position.x, __instance.targetSystem.transform.position.y, __instance.currentTargetingObjectStreak);
                     if (nextClosestTargetableObject != null)
                     {
                         __instance.targettedUnits.Add(nextClosestTargetableObject);
                         __instance.currentTargetingObjectStreak.Add(nextClosestTargetableObject);
                         __instance.currentTargetingStreakTime = 0f;
                         Networking.RPC<TargetableObject>(PID.TargetAll, new RpcSignature<TargetableObject>(__instance.CreateTargetOnObject), nextClosestTargetableObject, false);
                     }
                 }
             }
             if (__instance.shooting && __instance.health > 0 && (__instance.burstDelay -= __instance.t) <= 0f)
             {
                 __instance.burstDelay = 0.033334f;
                 int num = __instance.bulletRoundSize;
                 if (ProcGenGameMode.isEnabled || ProcGenGameMode.ProcGenTestBuild)
                 {
                     int primaryFireLevel = HeroController.GetPrimaryFireLevel(__instance.playerNum);
                     if (primaryFireLevel == 0)
                     {
                         num = __instance.bulletRoundSize - 1;
                     }
                     else if (primaryFireLevel == 3)
                     {
                         num = __instance.bulletRoundSize + 1;
                     }
                 }
                 for (int i = 0; i < __instance.bulletRoundSize; i++)
                 {
                     if (__instance.bulletsToFire > 0)
                     {
                         __instance.bulletVariation = Mathf.Clamp(__instance.bulletVariation + 0.04f + __instance.bulletVariation * 0.15f, 0f, 2.5f);
                         __instance.FireWeapon(__instance.X + __instance.transform.localScale.x * 15f, __instance.Y + 10.5f, __instance.transform.localScale.x * 300f + 50f * (UnityEngine.Random.value - 0.5f) * __instance.bulletVariation, UnityEngine.Random.Range(-50f, 50f) * __instance.bulletVariation);
                         __instance.PlayAttackSound();
                         Map.DisturbWildLife(__instance.X, __instance.Y, 80f, __instance.playerNum);
                         __instance.bulletsToFire--;
                     }
                     else
                     {
                         __instance.shooting = false;
                         __instance.fireDelay = 0.25f;
                     }
                 }
             }
         }
     }*/

    [HarmonyPatch(typeof(Brobocop), "UseFIre")]
    static class UseFire_Patch
    {
        static bool Prefix(Brobocop __instance)
        {
            try
            {
                __instance.SetFieldValue("shooting", true);
                var chargeTime = __instance.GetFloat("chargeTime");
                chargeTime += 0.3f;
                chargeTime = Mathf.Clamp(chargeTime, 0.01f, 2.625f);
                float num = chargeTime / 2.625f;
                float num2 = __instance.GetFloat("chargeTimePerBulletFired");
                if (ProcGenGameMode.UseProcGenRules)
                {
                    int primaryFireLevel = HeroController.GetPrimaryFireLevel(__instance.playerNum);
                    num2 *= 1.6600001f - 0.3f * (float)primaryFireLevel;
                }
                float num3 = chargeTime / num2 * num * 0.5f + chargeTime / num2 * 0.5f;
                var bulletsToFire = (int)num3;
                bulletsToFire = Mathf.Clamp(bulletsToFire, 4, 35);
                __instance.SetFieldValue("bulletRoundSize", Mathf.Clamp(bulletsToFire / 5, 1, 4));
                __instance.SetFieldValue("bulletsToFire", bulletsToFire);
                __instance.SetFieldValue("bulletVariation", 0f);
                return false;
            }
            catch (Exception e)
            {
                Main.Log(e);
            }
            return true;
        }
    }
}
