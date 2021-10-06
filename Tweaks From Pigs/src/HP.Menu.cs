using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using Utility;
using UnityEngine.SceneManagement;

namespace TweaksFromPigs
{
    // Playing multiple Arcade Campaign
    [HarmonyPatch(typeof(LevelSelectionController), "ResetLevelAndGameModeToDefault")]
    static class ArcadeCampaign_Patch
    {
        static int GetMaxAracadeLevel(string ArcadeLevel)
        {
            if(Main.settings.MaxArcadeLevelEnabled)
            {
                switch(ArcadeLevel)
                {
                    case "Hell Arcade":
                        return 13;
                    case "Expendabros": return 11;
                    case "TWITCHCON": return 10;
                    case "Alien Demo": return 5;
                    case "Boss Rush": return 10;
                }
            }
            return 63;
        }
        static void Postfix()
        {
            if (!Main.enabled) return;
            string c = Main.CurentArcade;
            if (c == "Hell Arcade")
            {
                LevelSelectionController.DefaultCampaign = LevelSelectionController.HellArcade;
            }
            else if (c == "Expendabros")
            {
                LevelSelectionController.DefaultCampaign = LevelSelectionController.ExpendabrosCampaign;

            }
            else if (c == "TWITCHCON")
            {
                LevelSelectionController.DefaultCampaign = "VIETNAM_EXHIBITION_TWITCHCON";
            }
            else if (c == "Alien Demo")
            {
                LevelSelectionController.DefaultCampaign = "AlienExhibition";
            }
            else if (c == "Boss Rush")
            {
                LevelSelectionController.DefaultCampaign = "BossRushCampaign";
            }
            else
            {
               LevelSelectionController.DefaultCampaign = LevelSelectionController.OfflineCampaign;
            }
            GetMaxAracadeLevel(c);
        }
    }

    // Add language menu
    [HarmonyPatch(typeof(OptionsMenu), "InstantiateItems")]
    static class AddLanguageMenu_Patch
    {
        static void Prefix(OptionsMenu __instance)
        {
            if (!Main.enabled) return;
            if(Main.settings.LanguageMenuEnabled)
            {
                Traverse trav = Traverse.Create(__instance);
                MenuBarItem[] masterItems = trav.Field("masterItems").GetValue() as MenuBarItem[];
                List<MenuBarItem> list = new List<MenuBarItem>(masterItems);

                list.Insert(list.Count - 2, new MenuBarItem
                {
                    color = Color.white,
                    size = __instance.characterSizes,
                    localisedKey = "MENU_OPTIONS_LANGUAGE",
                    name = "LANGUAGE_TFP",
                    invokeMethod = "GoToLanguageMenu"
                });
                trav.Field("masterItems").SetValue(list.ToArray());
            }
        }
        static void Postfix(OptionsMenu __instance)
        {
            Traverse trav = Traverse.Create(__instance);
        }
    }
    // Patch Main Menu
    /* [HarmonyPatch(typeof(MainMenu), "SetupItems")]
     static class p_Patch
     {
         static void Postfix(MainMenu __instance)
         {

             List<MenuBarItem> list = new List<MenuBarItem>( Traverse.Create(__instance).Field("masterItems").GetValue() as MenuBarItem[]);

             list.Insert(5, new MenuBarItem
             {
                 color = list[0].color,
                 size = list[0].size,
                 name = "EXPLOSION RUN",
                 invokeMethod = "StartExplosionRun"
             });

             list.Insert(5, new MenuBarItem
             {
                 color = list[0].color,
                 size = list[0].size,
                 name = "SUICIDE HORDE",
                 invokeMethod = "StartSuicideHorde"
             });
             list.Insert(5, new MenuBarItem
             {
                 color = list[0].color,
                 size = list[0].size,
                 name = "Race",
                 invokeMethod = "StartRace"
             });

             Traverse.Create(__instance).Field("masterItems").SetValue(list.ToArray());
         }
     }
     [HarmonyPatch(typeof(MainMenu), "StartSuicideHorde")]
     static class cc
     {
         static void Prefix()
         {
             Main.Log("suicide load");
         }
     }
     [HarmonyPatch(typeof(MainMenu), "StartExplosionRun")]
     static class cch
     {
         static bool Prefix()
         {
             Main.Log("explosion run load");

             LevelSelectionController.ResetLevelAndGameModeToDefault();
             LevelSelectionController.usingOnlineDMLevelRotation = false;
             LevelSelectionController.versusRotationList.Clear();
             LevelSelectionController.defaultDeathmatchCampaignIncludedInRotation = false;
             LevelSelectionController.defaultRaceCampaignIncludedInRotation = false;
             GameModeController.publishRun = false;
             GameModeController.GameMode = GameMode.ExplosionRun;
             Traverse.Create(typeof(LevelSelectionController)).Method("LoadNextVersusCampaign").GetValue();
             GameState.Instance.loadMode = MapLoadMode.Campaign;
             LevelEditorGUI.levelEditorActive = false;
             SceneLoader.LoadScene(LevelSelectionController.JoinScene, LoadSceneMode.Single);

             return false;
         }
     }*/


}
