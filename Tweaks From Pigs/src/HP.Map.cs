using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using HarmonyLib;

namespace TweaksFromPigs
{
    [HarmonyPatch(typeof(Map), "PlaceGround")]
    static class ChangeProbabilitySpawn_Patch
    {
        static void Prefix(Map __instance, GroundType placeGroundType, int x, int y, ref Block[,] newBlocks, bool addToRegistry = true)
        {
            if (!Main.enabled) return;
            if (Main.settings.UseAcidBarrel)
            {
                if(GameState.Instance.hardMode)
                {
                    Map.MapData.oilBarrelSpawnProbability = 0.4f;
                    Map.MapData.acidBarrelSpawnProbability = 0.6f;
                }
                /* try
                 {


                     Traverse travAcid = Traverse.Create(typeof(BarrelAcidBlock));
                     Material origMat = travAcid.Field("originalMaterial").GetValue<Material>();
                     origMat.mainTexture = Utility.CreateTexFromMat("Acidbarrel.png", origMat);
                     travAcid.Field("originalMaterial").SetValue(origMat as Material);

                 }
                 catch (Exception ex) { /*Main.Log(ex); }*/
            }
        }
    }
}
