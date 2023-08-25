using System;
using UnityEngine;
using HarmonyLib;
namespace BroforceOverhaul.Animals.Pig
{
    [HarmonyPatch(typeof(Animal), "Awake")]
    static class Pig_Awake_Patch
    {
        static void Postfix(Animal __instance)
        {
            if (Main.enabled)
            {
                try
                {
                    Material material = ResourcesController.GetMaterialResource("Animals.PigBalaclava_anim.png", ResourcesController.Unlit_DepthCutout);
                    if (Map.MapData.theme == LevelTheme.Hell && !__instance.isRotten && material != null)
                    {
                        __instance.gameObject.GetComponent<SpriteSM>().meshRender.sharedMaterial = material;
                        __instance.material = material;
                    }
                }
                catch (Exception ex)
                {
                    Main.ExceptionLog("Failed to Patch Pigs\n", ex);
                }
            }
        }
    }
}