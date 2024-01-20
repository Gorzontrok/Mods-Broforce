using HarmonyLib;
using System;

namespace DresserMod.Patches.NetworkedUnits
{
    [HarmonyPatch(typeof(NetworkedUnit), "Awake")]
    static class NetworkedUnit_Awake_Patch
    {
        static void Postfix(NetworkedUnit __instance)
        {
            if (!Main.CanPatch) return;
            if (__instance as TestVanDammeAnim) return;

            string name = Wearers.GetWearerName(__instance);
            if (StorageRoom.Wardrobes.ContainsKey(name))
            {
                try
                {
                    StorageRoom.Wardrobes[name].SetRandomAttire(__instance);
                }
                catch(Exception e)
                {
                    Main.Log(e);
                }
            }
        }
    }

    [HarmonyPatch(typeof(TestVanDammeAnim), "Start")]
    static class TestVanDammeAnim_Start_Patch
    {
        static void Prefix(NetworkedUnit __instance)
        {
            if (!Main.CanPatch) return;

            string name = Wearers.GetWearerName(__instance);
            if (StorageRoom.Wardrobes.ContainsKey(name))
            {
                try
                {
                    StorageRoom.Wardrobes[name].SetRandomAttire(__instance);
                }
                catch (Exception e)
                {
                    Main.Log(e);
                }
            }
        }
    }
}
