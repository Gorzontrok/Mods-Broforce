using System;
using System.Collections.Generic;
using System.Linq;
using RocketLib;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace BroforceOverhaul.Doodads.Cages
{
    [HarmonyPatch(typeof(Cage), "OnSpawned")]
    static class Cage_AddVillager_Patch
    {
        static void Postfix(Cage __instance)
        {
            if (Main.enabled && __instance is CageTemporary && !Map.isEditing && UnityEngine.Random.value > 0.7f)
            {
                try
                {
                    DoodadInfo doodadInfo = new DoodadInfo(new GridPoint((int)__instance.transform.position.x / 16 + UnityEngine.Random.Range(0, 1), (int)__instance.transform.position.y / 16 - 1 ), DoodadType.VillagerCaptured, DoodadsController.GetVillagerCapturedVariation());
                    GameObject gameObject = Map.Instance.PlaceDoodad(doodadInfo);
                }
                catch(Exception ex)
                {
                    Main.ExceptionLog(ex);
                }
            }
        }
    }
}

