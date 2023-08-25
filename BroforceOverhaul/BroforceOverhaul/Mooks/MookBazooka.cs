using System;
using HarmonyLib;
using UnityEngine;

namespace BroforceOverhaul.Mooks.MookBazooka0
{
    [HarmonyPatch(typeof(MookTrooper), "Awake")]
    static class MookTrooper_ChangeMookBazookaSprite_Patch
    {
        static void Postfix(MookTrooper __instance)
        {
            if(Main.enabled && __instance is MookBazooka)
            {
                Material material = ResourcesController.GetMaterialResource("Mooks.mookBazooka_anim.png", ResourcesController.Unlit_DepthCutout);
                if(material != null)
                {
                    __instance.gameObject.GetComponent<SpriteSM>().meshRender.sharedMaterial = material;
                }
            }
        }
    }
}
