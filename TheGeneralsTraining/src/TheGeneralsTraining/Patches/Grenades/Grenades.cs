using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using TheGeneralsTraining.Components;

namespace TheGeneralsTraining.Patches.Grenades
{

    [HarmonyPatch(typeof(Grenade), "Awake")]
    static class DontKilledIfNotVisible_Patch
    {
        static void Postfix(Grenade __instance)
        {
            if (Main.CanUsePatch)
            {
                __instance.ShouldKillIfNotVisible = Main.settings.grenadeExplodeIfNotVisible;
            }

        }
    }

    [HarmonyPatch(typeof(Grenade), "Death")]
    static class SpawnPig_Patch
    {
        static bool Prefix(Grenade __instance)
        {
            if (Main.CanUsePatch && Main.settings.pigGrenade && __instance.GetComponent<PigGrenade_Comp>() != null)
            {
                try
                {
                    __instance.GetComponent<PigGrenade_Comp>()?.Explode(__instance);
                    __instance.GetTraverse().Method("DestroyGrenade").GetValue();
                    return false;
                }
                catch(Exception ex)
                {
                    Main.Log(ex);
                }
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(Grenade), "SetupGrenade")]
    static class PigGrenadeForCasey_Patch
    {
        static void Postfix(Grenade __instance, MonoBehaviour _FiredBy)
        {
            if (Main.CanUsePatch && Main.settings.pigGrenade && _FiredBy is CaseyBroback)
            {
                try
                {
                    __instance.gameObject.AddComponent<PigGrenade_Comp>();
                }
                catch (Exception ex)
                {
                    Main.Log(ex);
                }
            }
        }
    }
}
