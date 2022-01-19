using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace Proc_Gen_Mod
{
    static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnGUI = OnGUI;
            modEntry.OnToggle = OnToggle;

            mod = modEntry;

            var harmony = new Harmony(modEntry.Info.Id);
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);
            }
            catch (Exception ex)
            {
                mod.Logger.Log("Failed to Patch Harmony !\n" + ex.ToString());
            }

            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            int.TryParse(GUILayout.TextField(Seed.ToString(), GUILayout.Width(100)), out Seed);
            if(GUILayout.Button("Load"))
            {
               TFBGames.Systems.GameSystems.ResourceManager.LoadPersistentAssetsAsync();
                LevelSelectionController.ResetLevelAndGameModeToDefault();
                GameState gameState = new GameState();
                gameState.campaignName = "Random";
                gameState.gameMode = GameMode.Campaign;
                gameState.loadMode = MapLoadMode.Generated;
                LevelSelectionController.levelFileNameToLoad = LevelSelectionController.DefaultCampaign;
                gameState.sceneToLoad = LevelSelectionController.CampaignScene;
                gameState.levelEditorActive = false;
                gameState.useRandomMapGenSeed = false;
                gameState.mapGenSeed = Seed == 0 ? UnityEngine.Random.Range(int.MinValue, int.MaxValue) : Seed;
                JoinScreen.EditorTestJoin();
                MainMenu.wasShown = true;
                gameState.Apply();
                GameState.LoadLevel(gameState.sceneToLoad);
            }

            GUILayout.Space(20);
            if(GUILayout.Button("Invincible"))
            {

                for (int j = 0; j < 4; j++)
                {
                    if (HeroController.players[j] != null && HeroController.players[j].IsAlive())
                    {
                        HeroController.players[j].character.health = 10000;
                    }
                }
            }
        }


        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }

        public static void Log(object str)
        {
            mod.Logger.Log(str.ToString());
        }
        public static void Log(IEnumerable<object> str)
        {
            mod.Logger.Log(str.ToString());
        }

        public static int Seed = 0;
    }
}
