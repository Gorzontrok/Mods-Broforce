using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelEditorExtended.Patches
{
    [HarmonyPatch(typeof(LevelEditorGUI), "CanEditChunks", MethodType.Getter)]
    static class CanEditChunk_Patch
    {
        static bool Prefix(ref bool __result)
        {
            __result = true;
            return false;
        }
    }
}
