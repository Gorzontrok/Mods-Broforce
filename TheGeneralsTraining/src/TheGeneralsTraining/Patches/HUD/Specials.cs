using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TheGeneralsTraining.Patches.HUD.Specials
{
    [HarmonyPatch(typeof(PlayerHUD), "SetGrenadeMaterials", new Type[] { typeof(HeroType) })]
    static class AddTearGasIcon_Patch
    {
        static void Prefix(PlayerHUD __instance, HeroType type)
        {
            if (Main.CanUsePatch && Main.settings.fifthBondSpecial)
            {
                if (type == HeroType.DoubleBroSeven && __instance.doubleBroGrenades.Length < 5)
                {
                    List<Material> tempList = __instance.doubleBroGrenades.ToList();
                    tempList.Add(ResourcesController.GetMaterialResource("sharedtextures:GrenadeTearGas", ResourcesController.Unlit_DepthCutout));
                    __instance.doubleBroGrenades = tempList.ToArray();
                }
            }
        }
        static void Postfix(PlayerHUD __instance, HeroType type)
        {
            if (type == HeroType.CaseyBroback)
            {
                Material material = ResourcesController.GetMaterialResource("pigGrenade.png", ResourcesController.Unlit_DepthCutout);
                for (int i = 0; i < __instance.grenadeIcons.Length; i++)
                {
                    __instance.grenadeIcons[i].GetComponent<Renderer>().material = material;
                }
            }
        }
    }
}
