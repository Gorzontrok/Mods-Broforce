using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerroristC4Programs.Patches.Snake
{
    [HarmonyPatch(typeof(AlienFaceHugger), "Start")]
    static class SnakeAreTerroristSide_Patch
    {
        static void Postfix(AlienFaceHugger alien)
        {
            alien.playerNum = -1;
        }
    }
}
