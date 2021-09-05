using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace Surprise
{
    // Patch mine
    [HarmonyPatch(typeof(Mine), "Update")]
    static class StartMine_Patch // don't work
    {
        static void Prefix(Mine __instance)
        {
            try
            {
                Traverse.Create(typeof(Mine)).Field("detonationTime").SetValue(0f);
                Traverse.Create(typeof(Mine)).Field("range").SetValue(80f);
            }catch(Exception ex) { Main.Log(ex); }
            
        }
    }

    // Patch Barrel
    [HarmonyPatch(typeof(BarrelBlock), "Update")]
    static class BarrelBlockUpdate_Patch
    {
        static void Prefix(BarrelBlock __instance)
        {
            __instance.range = 80f;
            //__instance.delayExplosionTime = 0.12f;
            if(Main.HardMode)
            {
                __instance.delayExplosionTime = 0f;
            }
        }
    }
    
    // Patch Propane
    [HarmonyPatch(typeof(PropaneBlock), "Update")]
    static class PropaneBlock_Update_Patch
    {
        static void Prefix(PropaneBlock __instance)
        {
            __instance.range = 80f;
            __instance.delayExplosionTime = 1.0f;
            //__instance.dropDirt = false; do nothing
            if(Main.HardMode)
            {
                __instance.delayExplosionTime = 0.5f;
            }
        }
    }

    // Patch Checkpoint
    [HarmonyPatch(typeof(CheckPoint), "ActivateInternal")]
    static class CheckpointCantWork_Patch
    {
        static bool Prefix(CheckPoint __instance)
        {
            bool isFinal = Traverse.Create(__instance).Field("isFinal").GetValue<bool>();

            if (__instance.activated)
            {
                return false;
            }
            SpatialSound component = __instance.GetComponent<SpatialSound>();
            if (component)
            {
                component.Play();
            }
            StatisticsController.NotifyCaptureCheckPoint();

            __instance.activated = false;
            __instance.flag.gameObject.SetActive(false);

            if (isFinal || !Main.HardMode)
            {
                Main.Log("Checkpoint");
                __instance.activated = true;
                __instance.flag.gameObject.SetActive(true);
                HeroController.SetCheckPoint(new Vector2(__instance.transform.position.x, __instance.transform.position.y + 16f), __instance.checkPointID);
            }

            if (isFinal)
            {
                if (Map.MapData.theme == LevelTheme.Hell)
                {
                    Map.CreateExitPortal(new Vector2(__instance.X, __instance.Y));
                }
                else
                {
                    Networking.Networking.RPC<Vector2, float>(PID.TargetAll, new RpcSignature<Vector2, float>(Map.newestHelicopter.Enter), new Vector2(__instance.X - 10f, __instance.Y), 0f, false);
                }
            }
            if (__instance.flagRaiseAudioClip != null)
            {
                Sound.GetInstance().PlaySoundEffectAt(__instance.flagRaiseAudioClip, 0.6f, __instance.transform.position, 1f, true, false, false, 0f);
            }
            

            return false;
        }
    }
    [HarmonyPatch(typeof(CheckPoint), "ReactivateInternal")]
    static class CheckpointCantWork2_Patch
    {
        static bool Prefix(CheckPoint __instance)
        {
            bool isFinal = Traverse.Create(__instance).Field("isFinal").GetValue<bool>();

            if (isFinal || Main.HardMode) return false;

            return true;
        }
    }

    // Patch Alarm spawner
    [HarmonyPatch(typeof(AlarmSystem), "CallInParatroopers")]
    static class AlarmSystem_LessVillager_MoreEnnemies_Patch
    {
        static bool Prefix(AlarmSystem __instance)
        {
            int activatedPlayerNum = Traverse.Create(__instance).Field("activatedPlayerNum").GetValue<int>();

            int MooksToSpawn = 8;
            int VillagerToSpawns = 4;
            if (Main.HardMode)
            { MooksToSpawn = 10; VillagerToSpawns = 2; }

            float x = __instance.transform.position.x;
            float num = SortOfFollow.GetScreenMaxY() + 16f;
            RaycastHit raycastHit;
            if (SortOfFollow.IsItSortOfVisible(__instance.transform.position, 48f, 64f) && !Physics.Raycast(__instance.transform.position, Vector3.up, out raycastHit, 256f, Map.groundLayer) && !Physics.Raycast(__instance.transform.position - Vector3.right * 16f, Vector3.up, out raycastHit, 256f, Map.groundLayer) && !Physics.Raycast(__instance.transform.position + Vector3.right * 16f, Vector3.up, out raycastHit, 256f, Map.groundLayer))
            {
                if (activatedPlayerNum < 0 && MapController.currentActiveMooksInScene < __instance.activeMookThreshold && MapController.currentDeadMooksInScene < __instance.deadMookThreshold)
                {
                    Mook mookPrefab = (Mook)Map.Instance.activeTheme.mook;
                    for (int i = 0; i < MooksToSpawn; i++)
                    {
                        MapController.SpawnMook_Networked(mookPrefab, x + ((float)i - 1.5f) * 32f, num + UnityEngine.Random.value * 8f, (float)UnityEngine.Random.Range(-1, 2), 0f, false, false, true, false, false);
                    }
                }
                else if (activatedPlayerNum >= 0)
                {
                    for (int j = 0; j < VillagerToSpawns; j++)
                    {
                        TestVanDammeAnim[] villager = Map.Instance.activeTheme.villager1;
                        int max = villager.Length;
                        Villager villager2 = villager[UnityEngine.Random.Range(0, max)] as Villager;
                        if (villager2 != null)
                        {
                            MapController.SpawnVillager_Networked(villager2, x + ((float)j - 1.5f) * 32f, num + UnityEngine.Random.value * 8f, (float)UnityEngine.Random.Range(-1, 2), 0f, false, false, true, false, false);
                        }
                    }
                    Traverse.Create(__instance).Field("calledVillagers").SetValue(true);
                    Traverse.Create(__instance).Field("paratrooperCounter").SetValue(100000f);
                }
            }

            return false;
        }
    }
}
