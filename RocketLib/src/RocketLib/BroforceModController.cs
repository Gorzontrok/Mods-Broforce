using System;
using System.Collections.Generic;
using UnityEngine;
using RocketLib.Loggers;
using HarmonyLib;

namespace RocketLib
{
    /// <summary>
    ///
    /// </summary>
    public static class BroforceModController
    {

        private static List<BroforceMod> BroforceMod_List = new List<BroforceMod>();

        public static bool AddBroforceMod(BroforceMod bmod)
        {
            if (!ID_Already_Taken(bmod.ID))
            {
                BroforceMod_List.Add(bmod);
                return true;
            }
            return false;
        }

        public static List<BroforceMod> Get_BroforceModList()
        {
            return new List<BroforceMod>(BroforceMod_List);
        }

        private static bool ID_Already_Taken(string id)
        {
            foreach (BroforceMod mod in BroforceMod_List)
            {
                if (mod.ID == id) return true;
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(GameModeController), "LevelFinish", typeof(LevelResult))]
    static class OnLevelFinish_Patch
    {
        static void Prefix(LevelResult result)
        {
            if (GameModeController.LevelFinished)
            {
                foreach (var bmod in BroforceModController.Get_BroforceModList())
                {
                    try
                    {
                        if (bmod.OnLevelFinished != null)
                            bmod.OnLevelFinished();
                    }
                    catch (Exception ex) { ScreenLogger.Instance.ExceptionLog("Failed to load OnLevelFinished from: " + bmod.ID, ex); }
                }
            }
        }
    }

    [HarmonyPatch(typeof(MainMenu), "ExitGame")]
    static class OnExit_Patch
    {
        static void Prefix()
        {
            foreach (var bmod in BroforceModController.Get_BroforceModList())
            {
                try
                {
                    if (bmod.OnExitGame != null)
                        bmod.OnExitGame.Invoke();
                }
                catch (Exception ex) { ScreenLogger.Instance.ExceptionLog("Failed to load OnExitGame from: " + bmod.ID, ex); }
            }
        }
    }

    /*[HarmonyPatch(typeof(UnityModManager), "Start")]
    static class OnAfterLoadMod_Patch
    {
        static void Postfix(UnityModManager __instance)
        {
            if(!Traverse.Create(__instance).Field("started").GetValue<bool>())
            {
                Main.mod.Logger.Log("ze");
                foreach(var bmod in BroforceModController.Get_BroforceModList())
                {
                    try
                    {
                        if(bmod.OnAfterLoadMod != null)
                        {
                            bmod.OnAfterLoadMod.Invoke();
                        }
                    }catch(Exception ex) { Main.bmod.logger.Log("Failed to load OnAfterLoadMod from: " + bmod.ID, ex); }
                }
            }
        }
    }*/
    [HarmonyPatch(typeof(MainMenu), "Start")]
    static class OnAfterLoadMod_Patch
    {
        private static bool LoadMods;
        static void Prefix()
        {
            if (!LoadMods)
            {
                foreach (var bmod in BroforceModController.Get_BroforceModList())
                {
                    try
                    {
                        if (bmod.OnAfterLoadMods != null)
                        {
                            bmod.OnAfterLoadMods.Invoke();
                        }
                    }
                    catch (Exception ex) { ScreenLogger.Instance.ExceptionLog("Failed to load OnAfterLoadMod from: " + bmod.ID, ex); }
                }
                LoadMods = true;
            }
        }
    }
}
