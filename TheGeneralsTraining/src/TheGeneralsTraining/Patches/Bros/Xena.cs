using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using TheGeneralsTraining.Components.Bros;

namespace TheGeneralsTraining.Patches.Bros.Xena
{
    [HarmonyPatch(typeof(Xebro), "Awake")]
    static class Xebro_Awake_Patch
    {
        static void Postfix(Xebro __instance)
        {
            if(Main.CanUsePatch && Main.settings.betterChakram)
            {
                __instance.gameObject.AddComponent<Xena_Comp>();
            }
        }
    }

    [HarmonyPatch(typeof(Xebro), "CatchChakram")]
    static class Xebro_CatchOnlyOnDemand_Patch
    {
        static bool Prefix(Xebro __instance, Chakram chakram)
        {
            if(Main.CanUsePatch && Main.settings.betterChakram && __instance.GetComponent<Xena_Comp>() != null)
            {
                try
                {
                    Traverse t = Traverse.Create(__instance);
                    Xena_Comp comp = __instance.GetComponent<Xena_Comp>();
                    if (comp.hasCallChakram)
                    {
                        __instance.meleeType = BroBase.MeleeType.Punch;
                        __instance.SpecialAmmo++;
                        List<Chakram> list = __instance.GetFieldValue< List<Chakram> >("thrownChakram");
                        list.Remove(chakram);
                        __instance.SetFieldValue("thrownChakram", list);

                    }
                    return false;
                }
                catch(Exception e)
                {
                    Main.Log(e);
                }
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(Xebro), "UseSpecial")]
    static class Xebro_CallBackChakram_Patch
    {
        static void Prefix(Xebro __instance)
        {
            if(Main.CanUsePatch && Main.settings.betterChakram)
            {
                __instance.GetComponent<Xena_Comp>().hasCallChakram = __instance.SpecialAmmo <= 0;
            }
        }
    }
}

