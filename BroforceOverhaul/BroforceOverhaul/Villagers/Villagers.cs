using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace BroforceOverhaul.Villagers
{
    [HarmonyPatch(typeof(Villager), "Awake")]
    static class Villager_MoreVillager_Patch
    {
        static void Prefix(Villager __instance)
        {
            __instance.gameObject.AddComponent<MoreVillager_Comp>();
        }
    }
}
