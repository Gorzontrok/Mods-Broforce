using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;

namespace TweaksFromPigs
{
    public static class Ref
    {
        public static NetworkedUnit networkedUnit;
        public static Map map;
        public static Menu menu;
    }

    [HarmonyPatch(typeof(NetworkedUnit), "Awake")]
    static class AssignNetworkedUnit_Patch
    {
        static void Postfix(NetworkedUnit __instance)
        {
            Ref.networkedUnit = __instance;
        }
    }

    [HarmonyPatch(typeof(Map), "Awake")]
    static class AssignMap_Patch
    {
        static void Postfix(Map __instance)
        {
            Ref.map = __instance;
        }
    }
    [HarmonyPatch(typeof(Menu), "Awake")]
    static class AssignMenu_Patch
    {
        static void Postfix(Menu __instance)
        {
            Ref.menu = __instance;
        }
    }
}
