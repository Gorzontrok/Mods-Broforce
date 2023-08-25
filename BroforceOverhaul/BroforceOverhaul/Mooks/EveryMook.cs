using System;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace BroforceOverhaul.Mooks.EveryMooks
{
    [HarmonyPatch(typeof(Mook), "Awake")]
    static class Mook_Awake_Patch
    {
        static void Prefix(Mook __instance)
        {
            if (Main.enabled)
            {
                __instance.canBeTearGased = MookController.CanBeTeargased(__instance);
                if(__instance is MookGeneral)
                {
                    __instance.skinnedPrefab = Map.Instance.activeTheme.mook.skinnedPrefab;
                }
            }
        }
    }

    [HarmonyPatch(typeof(TestVanDammeAnim), "ReplaceWithSkinnedInstance")]
    static class TestVanDammeAnim_SkinUnit_Patch
    {
        static void Prefix(TestVanDammeAnim __instance, ref Unit skinnedInstance)
        {
            if(Main.enabled)
            {
                try
                {
                    Material material = MookController.GetSkinlessMaterial(__instance);
                    if (material != null)
                    {
                        skinnedInstance.gameObject.GetComponent<SpriteSM>().meshRender.sharedMaterial = material;
                    }
                }
                catch(Exception ex)
                {
                    Main.ExceptionLog(ex);
                }
            }

        }
    }
}

