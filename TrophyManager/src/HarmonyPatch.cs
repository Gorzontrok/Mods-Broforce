using System;
using HarmonyLib;

namespace TrophyManager
{
    //Who turn off the light TROPHY
    [HarmonyPatch(typeof(Mook), "IsDecapitated")]
    static class WhoTurnOffTheLight_TrophyPatch
    {
        public static void Postfix(ref bool __result)
        {
            if (__result)
            {
                Main.settings.decapitatedCount++;
                //Main.CheckTrophy();
            }

        }
    }
    //DoYouLikeMyMuscle TROPHY
    [HarmonyPatch(typeof(Mook), "StopBeingBlind", new Type[] { })]//Work 👍
    static class DoYouLikeMyMuscle_TrophyPatch
    {
        public static void Postfix()
        {
            Main.settings.blindCount++;
            //Main.CheckTrophy();
        }
    }
    //Explode TROPHY
    [HarmonyPatch(typeof(TestVanDammeAnim), "CreateGibs", new Type[] { typeof(float), typeof(float) })]//Work 👍
    static class BoomYouAreNowInvisible_TrophyPatch
    {
        public static void Postfix()
        {
            try
            {
                Main.settings.explodeCount++;
                // Main.CheckTrophy();
            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }

    //KILL TROPHY
    [HarmonyPatch(typeof(Mook), "OnDestroy", new Type[] { })]//Work 👍
    static class KillTrophy_TrophyPatch
    {
        public static void Postfix()
        {
            try
            {
                Main.settings.killCount++;
                // Main.CheckTrophy();

            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }

    //Guerrilla TROPHY
    [HarmonyPatch(typeof(VillagerAI), "EnterMinionMode", new Type[] { })]
    static class GuerillaTrophy_TrophyPatch
    {
        public static void Postfix()
        {
            try
            {
                Main.settings.villagerCount++;
                //Main.CheckTrophy();

            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }

    //Predabro TROPHY
    [HarmonyPatch(typeof(PredabroRope), "SetUp", new Type[] { typeof(Unit) })]
    static class predabroTrophy_TrophyPatch
    {
        public static void Postfix()
        {
            try
            {
                Main.settings.ennemiOnRopeCount++;
                //Main.CheckTrophy();

            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }
    //door kill TROPHY
    [HarmonyPatch(typeof(DoorDoodad), "MakeEffectsDeath", new Type[] { })]
    static class doorKillTrophy_TrophyPatch
    {
        public static void Postfix()
        {
            try
            {
                Main.settings.doorKillCount++;
                //Main.CheckTrophy();

            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }

    //shield TROPHY
    [HarmonyPatch(typeof(MookRiotShield), "DisarmShield", new Type[] { typeof(float) })]
    static class shieldThrowTrophy_TrophyPatch
    {
        public static void Postfix()
        {
            try
            {
                Main.settings.shieldThrowCount++;
                //Main.CheckTrophy();

            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }
    //anti-insemination TROPHY
    [HarmonyPatch(typeof(TestVanDammeAnim), "RecoverFromInsemination", new Type[] { })]
    static class InsecticidTrophy_TrophyPatch
    {
        public static void Postfix()
        {
            try
            {
                Main.settings.recoverInseminationCount++;
                //Main.CheckTrophy();

            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }
    //assassination TROPHY
    [HarmonyPatch(typeof(Mook), "AnimateAssasinated", new Type[] { })]
    static class AssassinationTrophy_TrophyPatch
    {
        public static void Postfix()
        {
            try
            {
                Main.settings.assassinationCount++;
                //Main.CheckTrophy();

            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }
}
