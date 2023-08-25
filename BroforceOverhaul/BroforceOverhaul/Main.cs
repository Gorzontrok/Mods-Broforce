using System;
using BroMakerLib;
using System.Reflection;
using RocketLib;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;
using BroforceOverhaul;

//[]
static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        internal static BroforceMod bmod;

        internal static bool GorzonBuild
        {
            get
            {
                return Environment.UserName == "Gorzon";
            }
        }

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            modEntry.OnUpdate = OnUpdate;

            settings = Settings.Load<Settings>(modEntry);

            mod = modEntry;

            var harmony = new Harmony(modEntry.Info.Id);
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);
            }
            catch (Exception ex)
            {
                mod.Logger.Log("Failed to Patch Harmony\n" + ex.ToString());
            }

            bmod = new BroforceMod(mod);
            new BroMakerLib.NewBroInfo(typeof(BroforceOverhaul.CustomBro.Bronobi.Bronobi),"[BO] Bronobi");

            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            if(GorzonBuild && GUILayout.Button("allTerritoriesCheat"))
            {
                WorldTerritory3D.allTerritoriesCheat = true;
            }
        }

        static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }

        public static void Log(object msg)
        {
            Main.bmod.Log(msg);
        }

        public static void ExceptionLog(object msg, Exception ex)
        {
            bmod.logger.ExceptionLog(msg.ToString(),ex);
        }
        public static void ExceptionLog(Exception ex)
        {
            bmod.logger.ExceptionLog(ex);
        }
    }

    public class Settings : UnityModManager.ModSettings
    {

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

    }

   /* [HarmonyPatch(typeof(GameModeController), "LoadNextSceneFade")]
    static class test
    {
        static void Prefix(ref GameState sceneAndGameState)
        {
            try
            {
                if (sceneAndGameState.gameMode == GameMode.Campaign)
                {
                    sceneAndGameState.campaignName = "01. Deep Jungle";
                    sceneAndGameState.loadMode = MapLoadMode.LoadFromFile;
                }
            }
            catch(Exception ex)
            {
                Main.Log(ex);
            }
        }
    }*/
