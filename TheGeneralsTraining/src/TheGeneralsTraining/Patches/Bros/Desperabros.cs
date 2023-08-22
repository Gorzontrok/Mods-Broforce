using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace TheGeneralsTraining.Patches.Bros.Desperabro0
{
    [HarmonyPatch(typeof(Desperabro), "Update")]
    static class MariachisDoMusicIfNoEnemies_Patch
    {
        static void Postfix(Desperabro __instance)
        {
            if(Main.CanUsePatch && __instance.mariachiBroType != Desperabro.MariachiBroType.Desperabro && !__instance.GetFieldValue<Desperabro>("desperabro").GetBool("isSerenading"))
            {
                try
                {
                    if (__instance.GetBool("isSerenading"))
                    {
                        int direction = __instance.GetInt("mariachiDirection");
                        if (Map.IsEnemyUnitNearby(__instance.playerNum, __instance.X, __instance.Y + 6f, direction, 160f, 16f, false, true))
                        {
                            __instance.ForceFaceDirection(direction);
                            __instance.SetFieldValue("gunFightFireTimer", 1.5f);
                            __instance.CallMethod("StopSerenading");
                        }
                        else if (Map.IsEnemyUnitNearby(__instance.playerNum, __instance.X, __instance.Y + 6f, -direction, 160f, 16f, false, true))
                        {
                            __instance.ForceFaceDirection(-direction);
                            __instance.SetFieldValue("gunFightFireTimer", 1.5f);
                            __instance.CallMethod("StopSerenading");
                        }
                    }
                    else if (__instance.GetFloat("gunFightFireTimer") <= 0f)
                    {
                        __instance.CallMethod("StartSerenading");
                    }
                }
                catch (Exception e)
                {
                    Main.ExceptionLog(e);
                }
            }
        }
    }
}
