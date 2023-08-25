using System;
namespace BroforceOverhaul.Levels
{
    public static class LevelController
    {
           public static bool ModCanEditMap
           {
                get
                {
                    return !Map.isEditing || !LevelSelectionController.loadCustomCampaign || !LevelEditorGUI.IsActive;
                }
            }

            public static float acidBarrelProbability = 0.2f;

        public  static int GetMaxAracadeLevel(string ArcadeLevel)
        {
            switch (ArcadeLevel)
            {
                    case "Hell Arcade": return 13;
                    case "Expendabros": return 11;
                    case "TWITCHCON": return 10;
                    case "Alien Demo": return 5;
                    case "Boss Rush": return 10;
                default: return 63;
            }
        }
        public static string GetCampaignName(string selectedCampaign)
        {
            switch(selectedCampaign)
            {
                case "Hell Arcade": return LevelSelectionController.HellArcade;
                case "Expendabros": return LevelSelectionController.ExpendabrosCampaign;
                case "TWITCHCON": return "VIETNAM_EXHIBITION_TWITCHCON";
                case "Alien Demo": return "AlienExhibition";
                case "Boss Rush": return "BossRushCampaign";
                default: return LevelSelectionController.OfflineCampaign;
            }
        }

        public static readonly string[] arcadeCampaigns = new string[]
        {
            "Normal",
            "Expendabros",
            "TWITCHCON",
            "Alien Demo",
            "Boss Rush",
            "Hell Arcade"
        };

        public static int arcadeCampaignIndex = 0;

    }
}

