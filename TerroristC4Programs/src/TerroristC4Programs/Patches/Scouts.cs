using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerroristC4Programs.Patches.Scouts
{
    [HarmonyPatch(typeof(ScoutMook), "Awake")]
    static class Awake_Patch
    {
        static void Prefix(ScoutMook __instance)
        {

        }
    }
}
