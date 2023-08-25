using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace BroforceOverhaul.Bros.Brade
{
    [HarmonyPatch(typeof(Blade), "Awake")]
    static class Blade_OldGlaive_Patch
    {
        static void Postfix(Blade __instance)
        {
            if (Main.enabled)
            {
                try
                {
                    Material material = ResourcesController.GetMaterialResource("Projectiles.ThrowingGlaiveBlade.png", ResourcesController.Unlit_DepthCutout);
                    if(material)
                    {
                        __instance.throwingKnife.GetComponent<Renderer>().sharedMaterial = material;
                        __instance.throwingKnife.gameObject.AddComponent<Projectiles.RotateProjectile_Comp>();
                    }
                }

                catch (Exception ex)
                {
                    Main.bmod.logger.ExceptionLog(ex);
                }
            }
        }
    }
}
