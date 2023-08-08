using HarmonyLib;
using System;
using TheGeneralsTraining.Components.Bros;

namespace TheGeneralsTraining.Patches.Bros.ChevBrolios0
{


    [HarmonyPatch(typeof(ChevBrolios), "Awake")]
    static class ChevBrolois_Awake_Patch
    {
        static void Postfix(ChevBrolios __instance)
        {
            if(Main.CanUsePatch && Main.settings.carBattery)
            {
                try
                {
                    __instance.gameObject.AddComponent<ChevBrolios_Comp>();
                }
                catch (Exception e)
                {
                    Main.Log(e);
                }
            }
        }
    }
    [HarmonyPatch(typeof(ChevBrolios), "Update")]
    static class ChevBrolois_Update_Patch
    {
        static void Postfix(ChevBrolios __instance)
        {
            if (Main.CanUsePatch && Main.settings.carBattery)
            {
                try
                {
                    ChevBrolios_Comp c = __instance.GetComponent<ChevBrolios_Comp>();
                    if (__instance.IsAlive() && __instance.GetFieldValue<bool>("isPoisoned") && c != null && c.IsOnFire())
                    {
                        Map.BurnBlocksAround(0, __instance.collumn, __instance.row, true);
                    }
                }
                catch(Exception e)
                {
                    Main.Log(e);
                }
            }
        }
    }
    [HarmonyPatch(typeof(ChevBrolios), "UseSpecial")]
    static class ChevBrolios_UseSpecial_Patch
    {
        static void Prefix(ChevBrolios __instance)
        {
            if(Main.CanUsePatch && Main.settings.carBattery)
            {
                try
                {
                    ChevBrolios_Comp c = __instance.GetComponent<ChevBrolios_Comp>();
                    if (__instance.GetFieldValue<bool>("isPoisoned") && __instance.SpecialAmmo > 0 && c != null)
                    {
                        c.UseSpecial();
                        __instance.SpecialAmmo--;
                    }
                }
                catch(Exception e)
                {
                    Main.Log(e);
                }
            }
        }
    }

    [HarmonyPatch(typeof(ChevBrolios), "UseFire")]
    static class ChevBrolios_RemoveRecoils_Patch
    {
        static void Postfix(ChevBrolios __instance)
        {
            if (Main.CanUsePatch && Main.settings.noRecoil)
            {
                __instance.xIBlast = 0;
            }
        }
    }
}
