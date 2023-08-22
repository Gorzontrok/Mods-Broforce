using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerroristC4Programs.Patches.Levels
{
    [HarmonyPatch(typeof(Map), "PlaceDoodad")]
    static class SwapToStronger_Patch
    {
        static void Prefix(ref DoodadInfo doodad)
        {
            if (!Mod.CanUsePatch) return;
            if (doodad.type != DoodadType.Mook) return;

            int variation = doodad.variation;
            if (variation == 0 && UnityEngine.Random.value < Mod.Sett.strongerTrooperProbability.Value)
            {
                doodad.variation = 17;
            }
            if (variation == 2 && UnityEngine.Random.value < Mod.Sett.suicideGetBigger.Value)
            {
                doodad.variation = 23;
            }
            else if (variation == 3)
            {
                if (UnityEngine.Random.value < Mod.Sett.strongerBruiserProbability.Value)
                    doodad.variation = 19;
                else if (UnityEngine.Random.value < Mod.Sett.strongerBruiserProbability.Value)
                    doodad.variation = 20;
            }
        }
    }
}
