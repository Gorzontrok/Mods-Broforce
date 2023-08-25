using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace BroforceOverhaul.Doodads.AlarmSystem0
{
    [HarmonyPatch(typeof(AlarmSystem), "CallInParatroopers")]
    static class AlarmSystem_MoreFriendlyDrop_Patch
    {
        static bool Prefix(AlarmSystem __instance)
        {
            if(Main.enabled)
            {
				try
				{
					Traverse t = Traverse.Create(__instance);
					float x = __instance.transform.position.x;
					float num = SortOfFollow.GetScreenMaxY() + 16f;
					RaycastHit raycastHit;
					if (SortOfFollow.IsItSortOfVisible(__instance.transform.position, 48f, 64f) && !Physics.Raycast(__instance.transform.position, Vector3.up, out raycastHit, 256f, Map.groundLayer) && !Physics.Raycast(__instance.transform.position - Vector3.right * 16f, Vector3.up, out raycastHit, 256f, Map.groundLayer) && !Physics.Raycast(__instance.transform.position + Vector3.right * 16f, Vector3.up, out raycastHit, 256f, Map.groundLayer))
					{
						int activatedPlayerNum = t.Field("activatedPlayerNum").GetValue<int>();
						if (activatedPlayerNum < 0 && MapController.currentActiveMooksInScene < __instance.activeMookThreshold && MapController.currentDeadMooksInScene < __instance.deadMookThreshold)
						{
							Mook mookPrefab = (Mook)Map.Instance.activeTheme.mook;
							for (int i = 0; i < 4; i++)
							{
								MapController.SpawnMook_Networked(mookPrefab, x + ((float)i - 1.5f) * 32f, num + UnityEngine.Random.value * 8f, (float)UnityEngine.Random.Range(-1, 2), 0f, false, false, true, false, false);
							}
						}
						else if (activatedPlayerNum >= 0)
						{
							DoodadsController.CallBroHQ(x, num);
							t.Field("calledVillagers").SetValue(true);
							t.Field("paratrooperCounter").SetValue(100000f);
						}
					}
					return false;
				}
				catch(Exception ex)
                {
					Main.ExceptionLog(ex);
                }
			}
			return true;
        }
    }
}
