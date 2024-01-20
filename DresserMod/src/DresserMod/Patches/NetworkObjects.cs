using HarmonyLib;
using System;
namespace DresserMod.Patches.NetworkObjects
{
    [HarmonyPatch(typeof(HeroTransport), "Awake")]
    static class HeroTransport_Start_Patch
    {
        static void Postfix(HeroTransport __instance)
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

    [HarmonyPatch(typeof(AlarmSystem), "Start")]
    static class AlarmSystem_Start_Patch
    {
        static void Postfix(AlarmSystem __instance)
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
