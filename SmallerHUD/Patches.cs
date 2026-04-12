using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallerHUD.Patches
{
    [HarmonyPatch(typeof(PlayerHUD), "Start")]
    static class PlayerHUD_Start_Patch
    {
        static void Postfix(PlayerHUD __instance)
        {
            try
            {
                if (Mod.CanUsePatch)
                    __instance.gameObject.transform.localScale = Mod.LevelVector();
            }
            catch(Exception e)
            {
                Main.Log(e);
            }
        }
    }
}
