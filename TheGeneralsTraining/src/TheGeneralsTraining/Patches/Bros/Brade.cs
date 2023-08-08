using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace TheGeneralsTraining.Patches.Bros.Brade
{
    [HarmonyPatch(typeof(Blade), "Awake")]
    static class Blade_OldGlaive_Patch
    {
        static void Postfix(Blade __instance)
        {
            if (Main.CanUsePatch && Main.settings.bradeGlaive && __instance is Blade)
            {
                try
                {
                    if (__instance.throwingKnife == null) return;

                    Material material = ResourcesController.GetMaterialResource("ThrowingGlaive.png", ResourcesController.Unlit_DepthCutout);
                    if(material)
                    {
                        __instance.throwingKnife.GetComponent<Renderer>().sharedMaterial = material;
                        __instance.throwingKnife.gameObject.AddComponent<Projectiles.RotateProjectile_Comp>();
                    }
                }

                catch (Exception ex)
                {
                    Main.ExceptionLog(ex);
                }
            }
        }
    }
}
