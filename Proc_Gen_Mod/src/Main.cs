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
        public static int Seed = 0;
        public static Settings settings;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnGUI = OnGUI;
            modEntry.OnToggle = OnToggle;
            modEntry.OnSaveGUI = OnSaveGUI;
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
            settings.ReplaceRambroWithCasey = GUILayout.Toggle(settings.ReplaceRambroWithCasey, "Replace Rambro To Casey");
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

        public static void Log(object str)
        {
            mod.Logger.Log(str.ToString());
        }
        public static void Log(IEnumerable<object> str)
        {
            mod.Logger.Log(str.ToString());
        }

    }
    public class Settings : UnityModManager.ModSettings
    {
        public bool ReplaceRambroWithCasey;
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }

    [HarmonyPatch(typeof(HeroController), "OnAfterDeserialize")]
    static class ReplaceRambroWithCasey_Patch
    {
        static void Postfix(HeroController __instance)
        {
            if(Main.settings.ReplaceRambroWithCasey)
            {
                Dictionary<HeroType, HeroController.HeroDefinition> _heroData = Traverse.Create(__instance).Field("_heroData").GetValue<Dictionary<HeroType, HeroController.HeroDefinition>>();
                _heroData[HeroType.Rambro].characterReference = _heroData[HeroType.CaseyBroback].characterReference;
                Traverse.Create(__instance).Field("_heroData").SetValue(_heroData);
            }
        }
    }
}
