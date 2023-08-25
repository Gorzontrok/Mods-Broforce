using System;
using System.Collections.Generic;
using System.Linq;
using RocketLib;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace BroforceOverhaul.UI.Menus.WorldMapOrArcadeMenu0
{
    [HarmonyPatch(typeof(WorldMapOrArcadeMenu), "SetupItems")]
    static class Mook_Awake_Patch
    {
        static void Postfix(WorldMapOrArcadeMenu __instance)
        {
            try
            {
                __instance.gameObject.AddComponent<Utility.Countdown>();
                Traverse t = Traverse.Create(__instance);
                MenuBarItem[] masterItems = t.Field("masterItems").GetValue() as MenuBarItem[];
                List<MenuBarItem> list = new List<MenuBarItem>(masterItems);
                list.Insert(list.Count - 2, new MenuBarItem
                {
                    color = list[0].color,
                    size = 6f,
                    name = MenuController.ArcadeButtonMenuText
                });
                t.Field("masterItems").SetValue(list.ToArray());
                MenuController.arcadeCampaignMenuIndex = t.Method("FindIndexOf", new object[] { MenuController.ArcadeButtonMenuText }).GetValue<int>();
            }
            catch(Exception ex)
            {
                Main.ExceptionLog(ex);
            }
        }
    }

}

