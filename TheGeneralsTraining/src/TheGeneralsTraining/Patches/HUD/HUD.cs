using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TheGeneralsTraining.Patches.HUD
{
    [HarmonyPatch(typeof(PlayerHUD), "ShowFaceHugger")]
    static class Avatar_FaceHugger_Patch
    {
        static void Prefix(PlayerHUD __instance)
        {
            if (Main.CanUsePatch && Main.settings.faceHugger)
            {
                __instance.showFaceHugger = true;
                __instance.faceHugger1.SetSize(__instance.GetFieldValue<int>("avatarFacingDirection") * __instance.faceHugger1.width, __instance.faceHugger1.height);
                __instance.avatar.SetLowerLeftPixel(new Vector2(__instance.faceHugger1.lowerLeftPixel.x, 1f)); //For some reason he makes the avatar transparent.
                __instance.faceHugger1.gameObject.SetActive(true);
            }
        }
    }
}
