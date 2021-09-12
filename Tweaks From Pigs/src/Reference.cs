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
    }

    [HarmonyPatch(typeof(NetworkedUnit), "Awake")]
    static class AssignNetworkedUnit_Patch
    {
        static void Postfix(NetworkedUnit __instance)
        {
            Ref.networkedUnit = __instance;
        }
    }
}
