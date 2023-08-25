using System;
namespace BroforceOverhaul.UI.Menus
{
    public static class MenuController
    {
        public static int arcadeCampaignMenuIndex = 4;

		public static string ArcadeButtonMenuText
		{
			get
			{
				return "< " + Levels.LevelController.arcadeCampaigns[Levels.LevelController.arcadeCampaignIndex] + " >";
			}
		}

		public static void ChangeArcadeCampaign(bool Increase)
		{
			if (Increase)
			{
				Levels.LevelController.arcadeCampaignIndex++;
				if (Levels.LevelController.arcadeCampaignIndex > Levels.LevelController.arcadeCampaigns.Length - 1)
				{
					Levels.LevelController.arcadeCampaignIndex = 0;
					return;
				}
			}
			else
			{
				Levels.LevelController.arcadeCampaignIndex--;
				if (Levels.LevelController.arcadeCampaignIndex < 0)
				{
					Levels.LevelController.arcadeCampaignIndex = Levels.LevelController.arcadeCampaigns.Length - 1;
				}
			}
		}
	}
}

