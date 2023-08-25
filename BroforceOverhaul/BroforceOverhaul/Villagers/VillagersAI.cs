using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace BroforceOverhaul.Villagers.AI
{
    /*[HarmonyPatch(typeof(FollowerMinion), "GetInput")]
    static class FollowerMinion_FollowPlayerIfNoMoreCheckpoint_Patch
    {
        static void Postfix(FollowerMinion __instance)
        {
            Traverse t = Traverse.Create(__instance);
            Unit unit = t.Field("unit").GetValue<Unit>();
            if (Main.enabled && __instance.GetComponent<PathAgent>().CurrentPath == null && t.Field("waitingForBroToCatchUp").GetValue<bool>() Map.GetNearestCheckPointToRight(unit.X - 32f,unit.Y, true) == null && t.Field("pathingDelay").GetValue<float>() < 0f)
            {
                try
                {
                    if (__instance.leader != null && __instance.leader.IsAlive())
                    {
                        __instance.GetComponent<PathAgent>().GetPath(Map.GetCollumn(__instance.leader.X) + UnityEngine.Random.Range(-2, 1), Map.GetRow(__instance.leader.Y), 10f);
                    }
                    else
                    {
                        t.Method("FindLeader").GetValue();
                    }
                }
                catch(Exception ex)
                {
                    Main.ExceptionLog(ex);
                }
            }
        }
    }*/
}
