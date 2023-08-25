using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace BroforceOverhaul.Bros.Xena
{
    public class Xena_Comp : MonoBehaviour
    {
        public bool hasCallChakram;
    }

    [HarmonyPatch(typeof(Xebro), "Awake")]
    static class Xebro_Awake_Patch
    {
        static void Postfix(Xebro __instance)
        {
            __instance.gameObject.AddComponent<Xena_Comp>();
        }
    }

    [HarmonyPatch(typeof(Xebro), "CatchChakram")]
    static class Xebro_CatchOnlyOnDemand_Patch
    {
        static bool Prefix(Xebro __instance, Chakram chakram)
        {
            if(Main.enabled && __instance.GetComponent<Xena_Comp>() != null)
            {
                Traverse t = Traverse.Create(__instance);
                 Xena_Comp comp = __instance.GetComponent<Xena_Comp>();
                if(comp.hasCallChakram)
                {
                    __instance.meleeType = BroBase.MeleeType.Punch;
                    __instance.SpecialAmmo++;
                    Traverse tc = t.Field("thrownChakram");
                    List<Chakram> list = tc.GetValue<List<Chakram>>();
                    list.Remove(chakram);
                    tc.SetValue(list);

                }
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(Xebro), "UseSpecial")]
    static class Xebro_CallBackChakram_Patch
    {
        static void Prefix(Xebro __instance)
        {
            if(Main.enabled)
            {
                __instance.GetComponent<Xena_Comp>().hasCallChakram = __instance.SpecialAmmo <= 0;

            }
        }
    }
}

