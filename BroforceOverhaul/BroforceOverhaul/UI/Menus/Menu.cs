using System.Collections.Generic;
using System;
using System.Linq;
using RocketLib;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;
using BroforceOverhaul.Utility;

namespace BroforceOverhaul.UI.Menus
{
    [HarmonyPatch(typeof(Menu), "RunInput")]
    static class Menu_RunInput_Patch
    {
        static void Postfix(Menu __instance)
        {
            try
            {
                if (__instance is WorldMapOrArcadeMenu)
                {
                    Traverse t = Traverse.Create(__instance);
                    Countdown cd = __instance.gameObject.GetComponent<Countdown>();
                    if (cd != null && t.Field("highlightIndex").GetValue<int>() == MenuController.arcadeCampaignMenuIndex && cd.IsFinish)
                    {
                        if (t.Field("right").GetValue<bool>() || t.Field("left").GetValue<bool>())
                        {
                            MenuController.ChangeArcadeCampaign(t.Field("right").GetValue<bool>());
                            cd.ResetTimer();
                        }
                        t.Field("items").GetValue<Localisation.MenuBarItemUI[]>()[MenuController.arcadeCampaignMenuIndex].text = MenuController.ArcadeButtonMenuText;
                    }
                }
            }
            catch(Exception ex)
            {
                Main.ExceptionLog(ex);
            }
        }
    }
}

