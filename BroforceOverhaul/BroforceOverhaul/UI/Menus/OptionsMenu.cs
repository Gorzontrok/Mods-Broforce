using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace BroforceOverhaul.UI.Menus.OptionsMenu0
{
    [HarmonyPatch(typeof(OptionsMenu), "InstantiateItems")]
    static class OptionsMenu_AddLanguageMenu_Patch
    {
        static void Prefix(OptionsMenu __instance)
        {
            if (Main.enabled)
            {
                try
                {
                    Traverse trav = Traverse.Create(__instance);
                    MenuBarItem[] masterItems = trav.Field("masterItems").GetValue() as MenuBarItem[];
                    List<MenuBarItem> list = new List<MenuBarItem>(masterItems);

                    list.Insert(list.Count - 2, new MenuBarItem
                    {
                        color = Color.white,
                        size = __instance.characterSizes,
                        localisedKey = "MENU_OPTIONS_LANGUAGE",
                        name = "LANGUAGE_2",
                        invokeMethod = "GoToLanguageMenu"
                    });
                    trav.Field("masterItems").SetValue(list.ToArray());
                }
                catch(Exception ex)
                {
                    Main.ExceptionLog(ex);
                }
                
            }
        }
    }
}

