using Effects;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TheGeneralsTraining.Patches.Explosions
{
    [HarmonyPatch(typeof(HolyWaterExplosion), "Update")]
    static class HolyWaterExplosion_MookToVillager_Patch
    {
        static float range = 16f;
        static float reviveRange = 24f;
        static float revivePointDuration = 1.7f;
        static bool penetrates = true;
        static bool knock = false;
        static bool ignoreDeadUnits = true;

        static void SetHeroesWillComebackToLife(List<FlashBangPoint> persistentPoints, int i)
        {
            Vector2 vector = Map.GetPosition(persistentPoints[i].collumn, persistentPoints[i].row);
            HeroController.SetHeroesWillComebackToLife(vector.x, vector.y, reviveRange, revivePointDuration);
        }

        static bool HitHellUnits(HolyWaterExplosion __instance, List<FlashBangPoint> persistentPoints, int i)
        {
            return Map.HitHellUnits(
                __instance.GetFieldValue<MonoBehaviour>("firedBy"),
                __instance.playerNum,
                __instance.GetFieldValue<int>("holyWaterDamage"),
                DamageType.Fire,
                range,
                Map.GetBlocksX(persistentPoints[i].collumn) + 8f, Map.GetBlocksY(persistentPoints[i].row) + 8f,
                0f, 0f,
                penetrates, knock, ignoreDeadUnits,
                canHeadshot: false
                );
        }

        static void SwapMookToVillager(List<FlashBangPoint> persistentPoints, int i)
        {
            if (!Main.settings.hollywaterMookToVillager) return;

            float x = Map.GetBlocksX(persistentPoints[i].collumn) + 8f;
            float y = Map.GetBlocksY(persistentPoints[i].row) + 8f;
            List<Unit> units = Map.GetUnitsInRange((int)range, x, y, !ignoreDeadUnits);
            foreach(Unit unit in units)
            {
                if (unit != null && (unit as Mook) && !unit.invulnerable && unit.IsAlive())
                {
                    if ((unit as MookTrooper && !(unit as UndeadTrooper)) || unit as MookRiotShield || unit as MookSuicide || unit as ScoutMook)
                    {
                        Villager villager = MapController.SpawnVillager_Networked(Map.Instance.activeTheme.villager1[0].GetComponent<Villager>(), unit.X, unit.Y, 0, 0, false, false, false, false, true, 0);
                        villager.Blind(0.3f);
                        UnityEngine.Object.Destroy(unit.gameObject);
                    }
                    else if (unit is MookDog && !(unit is Alien) && !(unit is HellDog) && !(unit as MookDog).isMegaDog)
                    {
                        TestVanDammeAnim tvda = MapController.SpawnTestVanDamme_Networked(Map.Instance.activeTheme.animals[2].GetComponent<TestVanDammeAnim>(), unit.X, unit.Y, 0f, 0f, false, false, false, false);
                        tvda.Blind(0.3f);
                        UnityEngine.Object.Destroy(unit.gameObject);
                    }
                }
            }
        }

        static void DoTheLoop(HolyWaterExplosion holyWaterExplosion, List<FlashBangPoint> persistentPoints, float burnTimer, float invulnerabilityTimer)
        {
            for (int i = 0; i < persistentPoints.Count; i++)
            {
                if (burnTimer >= 0.5f)
                {
                    if(!HitHellUnits(holyWaterExplosion, persistentPoints, i))
                        SwapMookToVillager(persistentPoints, i);
                }

                if (invulnerabilityTimer >= 0.2f)
                {
                    SetHeroesWillComebackToLife(persistentPoints, i);
                }
            }
        }

        static bool Prefix(HolyWaterExplosion __instance)
        {
            if (Main.CanUsePatch)
            {
                try
                {
                    List<FlashBangPoint> persistentPoints = __instance.GetFieldValue< List<FlashBangPoint> >("persistentPoints");
                    float counter = __instance.GetFieldValue<float>("counter");
                    float burnTimer = __instance.GetFieldValue<float>("burnTimer");
                    float invulnerabilityTimer = __instance.GetFieldValue<float>("invulnerabilityTimer");
                    float frameRate = __instance.GetFieldValue<float>("frameRate");

                    float maxTime = __instance.GetFieldValue<float>("maxTime");
                    float startTime = __instance.GetFieldValue<float>("startTime");

                    __instance.FirstUpdateFromPool();

                    float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
                    counter += num;
                    if (counter >= frameRate)
                    {
                        counter -= frameRate;
                        __instance.GetTraverse().Method("RunPoints").GetValue();
                    }
                    if (Time.time - startTime > maxTime)
                    {
                        __instance.EffectDie();
                    }

                    burnTimer += Time.deltaTime;
                    invulnerabilityTimer += Time.deltaTime;

                    DoTheLoop(__instance, persistentPoints, burnTimer, invulnerabilityTimer);

                    if (burnTimer >= 0.5f)
                        burnTimer -= 0.5f;
                    if (invulnerabilityTimer >= 0.2f)
                        invulnerabilityTimer -= 0.2f;

                    for (int k = persistentPoints.Count - 1; k >= 0; k--)
                    {
                        if (!Map.IsBlockSolid(persistentPoints[k].collumn, persistentPoints[k].row - 1))
                        {
                            persistentPoints.RemoveAt(k);
                        }
                    }

                    /*persistentPoints.SetThisValueToObject("persistentPoints", __instance);
                    counter.SetThisValueToObject("counter", __instance);
                    burnTimer.SetThisValueToObject("burnTimer", __instance);
                    invulnerabilityTimer.SetThisValueToObject("invulnerabilityTimer", __instance);
                    frameRate.SetThisValueToObject("frameRate", __instance);*/

                    __instance.SetFieldValue("persistentPoints", persistentPoints);
                    __instance.SetFieldValue("counter", counter);
                    __instance.SetFieldValue("burnTimer", burnTimer);
                    __instance.SetFieldValue("invulnerabilityTimer", invulnerabilityTimer);
                    __instance.SetFieldValue("frameRate", frameRate);

                    return false;
                }
                catch (Exception ex)
                {
                    Main.ExceptionLog(ex);
                }
            }
            return true;
        }
    }
}
