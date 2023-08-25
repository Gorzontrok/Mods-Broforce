using System;
using RocketLib;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace BroforceOverhaul.Bros.Expendabros.LeeBroxmas0
{
    // Change knife texture
    [HarmonyPatch(typeof(LeeBroxmas), "Awake")]
    static class LeeBroxmas_ChangeKnifTexture_Patch
    {
        static void Postfix(LeeBroxmas __instance)
        {
            if (Main.enabled)
            {
                try
                {
                    TestVanDammeAnim blade = HeroController.GetHeroPrefab(HeroType.Blade);
                    Texture bladeKnifeTex = (blade as Blade).throwingKnife.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture;

                    __instance.projectile.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = bladeKnifeTex;
                    __instance.macheteSprayProjectile.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = bladeKnifeTex;

                }
                catch (Exception ex)
                {
                    Main.ExceptionLog("Failed to patch Lee Broxmas", ex);
                }
            }
        }
    }
    // TODO : Fix pushing
    /*[HarmonyPatch(typeof(LeeBroxmas), "AnimatePushing")]
    static class LeeBroxmas_FixPushing_Patch
    {
        static void Postfix(LeeBroxmas __instance)
        {
            if (Main.enabled)
            {
                try
                {
                    __instance.gunSprite.transform.localPosition = TFP_Utility.GetBroGunVector3PositionWhilePushing(HeroType.LeeBroxmas);
                    float pushingTime = Traverse.Create(__instance).Field("pushingTime").GetValue<float>();
                    if (__instance.fire || pushingTime <= 0)
                    {
                        __instance.gunSprite.transform.localScale = new Vector3(-1f, 1f, 1f);
                        __instance.gunSprite.transform.localPosition = TFP_Utility.GetBroGunVector3PositionWhenFinishPushing(__instance.heroType);
                    }

                }
                catch (Exception ex)
                {
                    Main.ExceptionLog("Failed to patch Lee Broxmas Pushing", ex);
                }
            }
        }
    }*/
}

