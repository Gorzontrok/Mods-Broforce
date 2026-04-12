using HarmonyLib;
using System;

namespace DresserMod.Patches.BroforceObjects
{
    [HarmonyPatch(typeof(CheckPoint), "Awake")]
    static class CheckPoint_Awake_Patch
    {
        static void Postfix(CheckPoint __instance)
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

    [HarmonyPatch(typeof(Block), "FirstFrame")]
    static class Block_FirstFrame_Patch
    {
        static void Postfix(Block __instance)
        {
            if (!Main.CanPatch) return;
            if (__instance.GetBool("hasDoneFirstFrame")) return;

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

    [HarmonyPatch(typeof(Doodad), "Start")]
    static class Doodad_Start_Patch
    {
        static void Postfix(Doodad __instance)
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

    [HarmonyPatch(typeof(FollowingObject), "Start")]
    static class FollowingObject_Start_Patch
    {
        static void Postfix(FollowingObject __instance)
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

    [HarmonyPatch(typeof(Helicopter), "Start")]
    static class Helicopter_Start_Patch
    {
        static void Postfix(Helicopter __instance)
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

    [HarmonyPatch(typeof(Mine), "Start")]
    static class Mine_Start_Patch
    {
        static void Postfix(Mine __instance)
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

    [HarmonyPatch(typeof(Projectile), "Start")]
    static class Projectile_Start_Patch
    {
        static void Postfix(Projectile __instance)
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

    [HarmonyPatch(typeof(Shrapnel), "Start")]
    static class Shrapnel_Start_Patch
    {
        static void Postfix(Shrapnel __instance)
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

    [HarmonyPatch(typeof(Parachute), "Start")]
    static class Parachute_Start_Patch
    {
        static void Postfix(Parachute __instance)
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
