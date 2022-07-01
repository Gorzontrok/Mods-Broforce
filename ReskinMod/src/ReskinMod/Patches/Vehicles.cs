using System;
using UnityEngine;
using HarmonyLib;
using ReskinMod.Skins;

namespace ReskinMod.Patches.Vehicles
{
    [HarmonyPatch(typeof(TankBroTank), "Awake")]
    static class TankBroTank_Patch
    {
        static void Postfix(TankBroTank __instance)
        {
            if (Main.CantPatch) return;
            /* SkinCollection skinCollection = SkinCollection.GetSkinCollection(__instance.GetType().Name.ToLower());
             if (skinCollection != null)
             {
                 Skin turret = skinCollection.GetSkin(SkinType.Gun, 0);
                 if (turret != null)
                 {
                     //Traverse.Create(__instance).Field("turretOriginalMaterial").GetValue<Material>().mainTexture = turret.texture;
                     __instance.turretHurtMaterial.mainTexture = turret.texture;
                     __instance.turret.turret.meshRender.sharedMaterial.mainTexture = turret.texture;
                     //__instance.turret.GetComponent<Renderer>().sharedMaterial.mainTexture = turret.texture;
                 }
                 Skin wheels = skinCollection.GetSkin(SkinType.Character, 0);
                 if(wheels != null)
                 {
                     __instance.hurtMaterial.mainTexture = wheels.texture;
                 }
                 Skin flag = skinCollection.GetSkin(SkinType.Character, 2);
                 if (flag != null)
                 {
                     __instance.tankFlag.GetComponent<SpriteSM>().meshRender.sharedMaterial.SetTexture("_MainTex", flag.texture);
                 }
                 Skin hatch = skinCollection.GetSkin(SkinType.Character, 3);
                 if (hatch != null)
                 {
                     __instance.tankHatch.GetComponent<SpriteSM>().meshRender.sharedMaterial.SetTexture("_MainTex", hatch.texture);
                 }
             }*/
        }
    }
}
