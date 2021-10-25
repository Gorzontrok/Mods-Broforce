using System;
using UnityEngine;
using HarmonyLib;

namespace TrophyManager
{
    //Who turn off the light TROPHY
    [HarmonyPatch(typeof(Mook), "IsDecapitated")]
    class WhoTurnOffTheLight_TrophyPatch
    {
         static void Postfix(ref bool __result)
        {
            if (__result)
            {
                Main.settings.DecapitatedCount++;
            }

        }
    }
    //DoYouLikeMyMuscle TROPHY
    [HarmonyPatch(typeof(Mook), "StopBeingBlind", new Type[] { })]//Work partially
    class DoYouLikeMyMuscle_TrophyPatch
    {
        static void Postfix()
        {
            Main.settings.BlindCount++;
        }
    }
    //Explode TROPHY
    [HarmonyPatch(typeof(Mook), "Gib", new Type[] {typeof(DamageType), typeof(float), typeof(float) })]//Work 👍
    class BoomYouAreNowInvisible_TrophyPatch
    {
        static void Prefix(Mook __instance)
        {
            try
            {
                if(!__instance.destroyed)
                {
                    Main.settings.ExplodeCount++;
                }
            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }

    //KILL TROPHY
    [HarmonyPatch(typeof(Mook), "Death", new Type[] { typeof(float), typeof(float) , typeof(DamageObject) })]//Work 👍
    class KillTrophy_TrophyPatch
    {
        static void Prefix(Mook __instance)
        {
            try
            {
                if (!Traverse.Create(__instance).Field("hasDied").GetValue<bool>())
                {
                    Main.settings.KillCount++;
                }

            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }

    //Guerrilla TROPHY
    [HarmonyPatch(typeof(VillagerAI), "EnterMinionMode", new Type[] { })]
    class GuerillaTrophy_TrophyPatch
    {
         static void Postfix()
         {
            try
            {
                Main.settings.VillagerArmedCount++;

            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
         }
    }

    //Predabro TROPHY
    [HarmonyPatch(typeof(PredabroRope), "SetUp", new Type[] { typeof(Unit) })]
    class predabroTrophy_TrophyPatch
    {
        static void Postfix()
        {
            try
            {
                Main.settings.EnemiesOnRopeCount++;

            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }
    //door kill TROPHY
    [HarmonyPatch(typeof(Map), "HitUnits", new Type[] { typeof(MonoBehaviour), typeof(MonoBehaviour), typeof(int), typeof(int), typeof(DamageType), typeof(float), typeof(float), typeof(float), typeof(float), typeof(float), typeof(bool), typeof(bool)})]
    class doorKillTrophy_TrophyPatch
    {
        static void Postfix(ref bool __result, MonoBehaviour damageSender)
        {
            try
            {
                /*if ((damageSender.GetType() == typeof(DoorDoodad)) && __result == true)
                    Main.settings.DoorKillCount++;*/

            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }

    //shield TROPHY
    [HarmonyPatch(typeof(MookRiotShield), "DisarmShield", new Type[] { typeof(float) })]
    class shieldThrowTrophy_TrophyPatch
    {
        static void Postfix()
        {
            try
            {
                Main.settings.ShieldThrowCount++;

            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }
    //anti-insemination TROPHY
    [HarmonyPatch(typeof(TestVanDammeAnim), "RecoverFromInsemination", new Type[] { })]
    class InsecticidTrophy_TrophyPatch
    {
        static void Postfix()
        {
            try
            {
                Main.settings.RecoverFromInseminationCount++;
            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }
    //assassination TROPHY
    [HarmonyPatch(typeof(Mook), "AnimateAssasinated", new Type[] { })]
    class AssassinationTrophy_TrophyPatch
    {
        static void Postfix(Mook __instance)
        {
            try
            {
                if(__instance.assasinatedFrame == 14)
                    Main.settings.AssasinationCount++;

            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }
    // Swallow Trophy
    [HarmonyPatch(typeof(AlienMinibossSandWorm), "ExplodeWithinHead", new Type[] { typeof(SachelPack) })]
    class SwallowTrophy_TrophyPatch
    {
        static void Prefix(AlienMinibossSandWorm __instance, SachelPack sachel)
        {
            try
            {
                if (sachel is SachelPackTurkey)
                    Main.settings.SwallowAlienCount++;

            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }

    // Satan final boss kill Trophy
    [HarmonyPatch(typeof(BossSatanFloatingHead), "LoadEndCutscene", new Type[] {  })]
    class SatanFinalKillTrophy_TrophyPatch
    {
        static void Prefix(BossSatanFloatingHead __instance)
        {
            try
            {
                 Main.settings.SatanFinalBossKill++;

            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }
}
