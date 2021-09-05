using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;

namespace TweaksFromPigs
{
    //Patch pig
    [HarmonyPatch(typeof(Animal), "Awake")]
    static class Pig_Awake_Patch
    {
        static Random rng = new Random();
        static void Postfix(Animal __instance)
        {
            if(rng.Next(3) == 2)
                __instance.isRotten = true;
            if(__instance.isRotten && !Main.settings.PigAreAlwaysTerror)
            {
                __instance.material.mainTexture = Utility.CreateTexFromMat("pig_animStinky.png", __instance.material);
            }
            if (Main.settings.PigAreAlwaysTerror || Map.MapData.theme == LevelTheme.Hell)
                __instance.material.mainTexture = Utility.CreateTexFromMat("Gimp_Pig_anim.png", __instance.material);
        }
    }

    // Patch Hero unlock intervals for spawn
    /*[HarmonyPatch(typeof(HeroUnlockController), "IsAvailableInCampaign")] //Patch for add the Bros
    static class HeroUnlockController_IsAvailableInCampaign_Patch
    {
        // After my mod FiltredBros, i have no idea how this can working..
        public static bool Prefix(ref HeroType hero)
        {
            Dictionary<int, HeroType> newHeroUnlockIntervals = Traverse.Create(typeof(HeroUnlockController)).Field("_heroUnlockIntervals").GetValue() as Dictionary<int, HeroType>;
            if (Main.enabled)
                Main.CheckExpendables(newHeroUnlockIntervals);
            return newHeroUnlockIntervals.ContainsValue(hero);
        }
    }*/
}
