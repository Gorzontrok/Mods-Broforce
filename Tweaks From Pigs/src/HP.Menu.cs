using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Utility;
using UnityEngine.SceneManagement;

namespace TweaksFromPigs
{
    // Playing multiple Arcade Campaign
    [HarmonyPatch(typeof(LevelSelectionController), "ResetLevelAndGameModeToDefault")]
    static class ArcadeCampaign_Patch
    {
        static void Postfix()
        {
            if (!Main.enabled) return;
            string c = Main.CurentArcade;
            if (c == "Hell Arcade")
                LevelSelectionController.DefaultCampaign = LevelSelectionController.HellArcade;
            else if (c == "Expendabros")
                LevelSelectionController.DefaultCampaign = LevelSelectionController.ExpendabrosCampaign;
            else if (c == "TWITCHCON")
                LevelSelectionController.DefaultCampaign = "VIETNAM_EXHIBITION_TWITCHCON";
            else if (c == "Alien Demo")
                LevelSelectionController.DefaultCampaign = "AlienExhibition";
            else if (c == "Boss Rush")
                LevelSelectionController.DefaultCampaign = "BossRushCampaign";
            else
                LevelSelectionController.DefaultCampaign = LevelSelectionController.OfflineCampaign;

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

    /* [HarmonyPatch(typeof(OptionsMenu), "InstantiateItems")]
     static class d
     {
         static bool Prefix(OptionsMenu __instance)
         {
             Traverse trav = Traverse.Create(__instance);
             trav.Field("playingInSteam").SetValue(trav.Type("SteamController").Method("IsSteamEnabled").GetValue<bool>());

            // __instance.playOnlineWithFnetIndex = __instance.FindIndexOf("PLAY ONLINE WITH NON STEAM USERS");
             //__instance.playerNameIndex = __instance.FindIndexOf("PLAYERNAME");
             Ref.menu.InstantiateItems();
             //base.SetMenuItemColor(__instance.playerNameIndex, __instance.playingInSteam ? Color.gray : Color.white);
             trav.Method("SetPlayOnlineWithFnet").GetValue();
             __instance.SetPlayerNameText();

             return false;
         }
     }*/
    /*[HarmonyPatch(typeof(OptionsMenu), "SetPlayerName")]
    static class d4
    {
        static bool Prefix(OptionsMenu __instance)
        {
            Traverse trav = Traverse.Create(__instance);

            trav.Field("settingPlayerName").SetValue(true);

            return false;
        }
    }*/

}
