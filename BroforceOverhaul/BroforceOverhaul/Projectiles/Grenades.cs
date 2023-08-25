using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace BroforceOverhaul.Projectiles.Grenades
{
    [HarmonyPatch(typeof(ProjectileController), "Awake")]
    static class ProjectilleController_RetextureMechDrop_Patch
    {
        static void Postfix(ProjectileController __instance)
        {
            if(Main.enabled)
            {
                try
                {
                    Material material_low = ResourcesController.GetMaterialResource("Grenades.grenade_mech_drop_low.png", ResourcesController.Unlit_DepthCutout);
                    Material material = ResourcesController.GetMaterialResource("Grenades.grenade_mech_drop.png", ResourcesController.Unlit_DepthCutout);
                    Grenade mechdrop = __instance.mechDropGrenade;
                    if(material != null)
                    {
                        mechdrop.GetComponent<Renderer>().sharedMaterial = material;
                    }
                    if(material_low)
                    {
                        mechdrop.otherMaterial = material_low;
                    }
                }
                catch(Exception ex)
                {
                    Main.ExceptionLog(ex);
                }
            }
        }
    }

    [HarmonyPatch(typeof(HolyWaterExplosion), "Update")]
    static class HolyWaterExplosion_MookToVillager_Patch
    {
        static void Postfix(HolyWaterExplosion __instance)
        {
            if (Main.enabled)
            {
                try
                {
                    float range = 16f;
                    List<FlashBangPoint> persistentPoints = Traverse.Create(__instance).Field("persistentPoints").GetValue<List<FlashBangPoint>>();
                    for (int i = 0; i < persistentPoints.Count; i++)
                    {
                        float x = Map.GetBlocksX(persistentPoints[i].collumn) + 8f;
                        float y = Map.GetBlocksY(persistentPoints[i].row) + 8f;
                        for (int j = Map.units.Count - 1; j >= 0; j--)
                        {
                            Unit unit = Map.units[j];
                            if (unit != null && !unit.invulnerable && unit.IsAlive() && ((unit as MookTrooper && !(unit as UndeadTrooper)) || unit as MookRiotShield || unit as MookSuicide || unit as ScoutMook || unit is MookDog))
                            {
                                float f = unit.X - x;
                                if (Mathf.Abs(f) - range < unit.width)
                                {
                                    float num = unit.Y + unit.height / 2f + 3f - y;
                                    if (Mathf.Abs(num) - range < unit.height && (unit.IsOnGround() && unit.actionState != ActionState.Jumping && unit.actionState != ActionState.Fallen))
                                    {
                                        if ((unit as MookTrooper && !(unit as UndeadTrooper)) || unit as MookRiotShield || unit as MookSuicide || unit as ScoutMook)
                                        {
                                            Villager villager = MapController.SpawnVillager_Networked(Map.Instance.activeTheme.villager1[0].GetComponent<Villager>(), unit.X, unit.Y, 0, 0, false, false, false, false, true, __instance.playerNum);
                                            villager.Blind(0.3f);
                                        }
                                        else if (unit is MookDog && !(unit is Alien) && !(unit is HellDog) && !(unit as MookDog).isMegaDog)
                                        {
                                            MapController.SpawnTestVanDamme_Networked(Map.Instance.activeTheme.animals[2].GetComponent<TestVanDammeAnim>(), unit.X, unit.Y, 0f, 0f, false, false, false, false);
                                        }
                                        UnityEngine.Object.Destroy(unit.gameObject);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Main.ExceptionLog(ex);
                }
            }
        }
    }
}
