using System;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace TheGeneralsTraining.Patches.Bros.Brochete0
{
    // TODO : Fix pushing
    /*[HarmonyPatch(typeof(Brochete), "AnimatePushing")]
    static class Brochete_Pushing_Patch
    {
        static void Postfix(Brochete __instance)
        {
            if (Main.enabled)
            {
                try
                {
                    __instance.gunSprite.transform.localPosition = TFP_Utility.GetBroGunVector3PositionWhilePushing(HeroType.Brochete);
                    float pushingTime = Traverse.Create(__instance).Field("pushingTime").GetValue<float>();
                    if (__instance.fire || pushingTime <= 0)
                    {
                        __instance.gunSprite.transform.localScale = new Vector3(-1f, 1f, 1f);
                        __instance.gunSprite.transform.localPosition = TFP_Utility.GetBroGunVector3PositionWhenFinishPushing(__instance.heroType);
                    }

                }
                catch (Exception ex)
                {
                    Main.bmod.logger.ExceptionLog("Failed to patch Brochete pushing", ex);
                }
            }
        }
    }*/

    [HarmonyPatch(typeof(Brochete), "Awake")]
    static class Brochete_Awake_Patch
    {
        static void Postfix(Brochete __instance)
        {
            if (Main.CanUsePatch)
            {
                try
                {
                    __instance.SetFieldValue("test6Frames", Main.settings.alternateSpecialAnim);
                }
                catch(Exception e)
                {
                    Main.Log(e);
                }
            }
        }
    }
}

