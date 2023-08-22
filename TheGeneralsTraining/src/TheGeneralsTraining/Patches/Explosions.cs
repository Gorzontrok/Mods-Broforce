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

        static void Swaper(List<FlashBangPoint> persistentPoints, int i, int playerNum)
        {
            if (!Main.settings.hollywaterMookToVillager) return;

            float x = Map.GetBlocksX(persistentPoints[i].collumn) + 8f;
            float y = Map.GetBlocksY(persistentPoints[i].row) + 8f;
            List<Unit> units = Map.GetUnitsInRange((int)range, x, y, !ignoreDeadUnits);
            foreach(Unit unit in units)
            {
                if (unit != null && (unit as Mook) && !unit.invulnerable && unit.IsAlive())
                {
                    if (CanSwapToMook(unit))
                    {
                        SwapMookToVillager(unit, playerNum);
                    }
                    else if (CanSwapToPig(unit))
                    {
                        SwapMookToPig(unit);
                    }
                }
            }
        }
        static bool CanSwapToMook(Unit unit)
        {
            return unit is MookTrooper || unit is UndeadTrooper || unit is MookRiotShield || unit is MookSuicide || unit is ScoutMook || unit is MookBazooka;
        }
        static void SwapMookToVillager(Unit unit, int playerNum)
        {
            Villager villager = MapController.SpawnVillager_Networked(Map.Instance.activeTheme.villager1[0].GetComponent<Villager>(), unit.X, unit.Y, 0, 0, false, false, false, false, true, playerNum);
            villager.Panic(0.3f, true);
            unit.DestroyNetworked();
        }
        static bool CanSwapToPig(Unit unit)
        {
            return (unit is MookDog && !unit.As<MookDog>().isMegaDog) && !(unit is Alien) && !(unit is HellDog) && !(unit as MookDog).isMegaDog;
        }
        static void SwapMookToPig(Unit unit)
        {
            TestVanDammeAnim tvda = MapController.SpawnTestVanDamme_Networked(Map.Instance.activeTheme.animals[2].GetComponent<TestVanDammeAnim>(), unit.X, unit.Y, 0f, 0f, false, false, false, false);
            tvda.Panic(0.3f, true);
            unit.DestroyNetworked();
        }

        static void DoTheLoop(HolyWaterExplosion holyWaterExplosion, List<FlashBangPoint> persistentPoints, float burnTimer, float invulnerabilityTimer)
        {
            for (int i = 0; i < persistentPoints.Count; i++)
            {
                if (burnTimer >= 0.5f)
                {
                    if (!HitHellUnits(holyWaterExplosion, persistentPoints, i))
                        Swaper(persistentPoints, i, holyWaterExplosion.playerNum);
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
                    float counter = __instance.GetFloat("counter");
                    float burnTimer = __instance.GetFloat("burnTimer");
                    float invulnerabilityTimer = __instance.GetFloat("invulnerabilityTimer");
                    float frameRate = __instance.GetFloat("frameRate");

                    float maxTime = __instance.GetFloat("maxTime");
                    float startTime = __instance.GetFloat("startTime");

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
