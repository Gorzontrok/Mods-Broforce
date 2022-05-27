using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace TweaksFromPigs.HPatch.HUD
{
    // From Show avatar FaceHugger
    [HarmonyPatch(typeof(PlayerHUD), "ShowFaceHugger")]
    static class Avatar_FaceHugger_Patch
    {
        static void Prefix(PlayerHUD __instance)
        {
            if (Main.enabled && Main.settings.showFacehuggerHUD)
            {
                __instance.showFaceHugger = true;
                __instance.faceHugger1.SetSize(Traverse.Create(__instance).Field("avatarFacingDirection").GetValue<int>() * __instance.faceHugger1.width, __instance.faceHugger1.height);
                __instance.avatar.SetLowerLeftPixel(new Vector2(__instance.faceHugger1.lowerLeftPixel.x, 1f)); //For some reason he makes the avatar transparent.
                __instance.faceHugger1.gameObject.SetActive(true);
            }
        }
    }

    [HarmonyPatch(typeof(PlayerHUD), "SetGrenadeMaterials", new Type[] { typeof(HeroType) })]
    static class AddTearGasIcon_Patch
    {
        static void Prefix(PlayerHUD __instance, HeroType type)
        {
            if(AssetsCollection.grenadeShader == null)
            {
                AssetsCollection.grenadeShader = __instance.alienPheromones.shader;
            }
            if (Main.enabled && type == HeroType.DoubleBroSeven && __instance.doubleBroGrenades.Length < 5)
            {
                List<Material> tempList = __instance.doubleBroGrenades.ToList();
                tempList.Add(AssetsCollection.Grenade_Tear_Gas);
                __instance.doubleBroGrenades = tempList.ToArray();
            }
        }
    }
}
