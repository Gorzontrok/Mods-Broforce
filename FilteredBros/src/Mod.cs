using RocketLib;
using RocketLib.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FilteredBros
{
    public class Mod
    {
        public static bool CanUsePatch
        {
            get
            {
                if (Main.enabled)
                {
                    if (LevelSelectionController.IsCustomCampaign() && !LevelEditorGUI.IsActive && !Map.isEditing)
                    {
                        return Main.settings.mod.patchInCustomsLevel;
                    }
                    return true;
                }
                return false;
            }
        }

        public static Settings Settings
        {
            get => Main.settings;
        }

        public static bool ShouldUpdateUnlockIntervals
        {
            get => CurrentUnlockIntervals.IsNullOrEmpty() || _shouldUpdateUnlockIntervals;
            set => _shouldUpdateUnlockIntervals = value;
        }
        private static bool _shouldUpdateUnlockIntervals = false;

        public static Dictionary<int, HeroType> OriginalUnlockIntervals { get; } = Heroes.OriginalUnlockIntervals;
        public static Dictionary<int, HeroType> CurrentUnlockIntervals { get; private set; }
        private static int _intervalMax = -1;

        internal static void Start()
        {
            // Add the password to unlock cheat
            new GamePassword("iaminthematrix", IAmInTheMatrix);

            // Campaign Bros
            List<HeroType> campaignBros = Heroes.CampaignBro.ToList();
            campaignBros.Remove(HeroType.MadMaxBrotansky); // Remove duplicate Bro Max in CampaignBros
            CreateBroToggles(campaignBros, BroGroup.Campaign);

            // Expendabros Bros
            CreateBroToggles(Heroes.Expendabros.ToList(), BroGroup.Expendabros);

            // Unused Bros
            List<HeroType> unusedBros = Heroes.Unused.ToList();
            unusedBros.Remove(HeroType.SuicideBro); // remove Suicide Bro cause it can't be spawned anymore
            CreateBroToggles(unusedBros, BroGroup.Unused);

            // Enabled bros from last instance
            if (BroToggle.All != null && Settings.brosEnable != null)
            {
                for(int i = 0; i < Settings.brosEnable.Count; i++)
                {
                    if (i < BroToggle.All.Count && BroToggle.All[i] != null)
                    {
                        BroToggle.All[i].enabled = Settings.brosEnable[i];
                    }
                }
            }
        }

        private static void CreateBroToggles(List<HeroType> heroArray, BroGroup group)
        {
            if (_intervalMax == -1)
                _intervalMax = OriginalUnlockIntervals.Last().Key;

            foreach (HeroType hero in heroArray)
            {
                int interval = -1;
                if (OriginalUnlockIntervals.ContainsValue(hero))
                {
                    interval = OriginalUnlockIntervals.First(x => x.Value == hero).Key;
                }
                else
                {
                    interval = _intervalMax + 10;
                    _intervalMax = interval;
                }
                new BroToggle(hero, interval, group);
            }
        }

        public static void UpdateCurrentUnlockIntervals()
        {
            // make sure there is BroToggle
            if (BroToggle.All.IsNullOrEmpty())
            {
                Main.Log($"[{nameof(UpdateCurrentUnlockIntervals)}] There is no BroToggle. Can't update dictionary.");
                return;
            }
            if (CurrentUnlockIntervals == null)
                CurrentUnlockIntervals = new Dictionary<int, HeroType>();
            CurrentUnlockIntervals.Clear();
            // Build the dictionary
            foreach (BroToggle toggle in BroToggle.All)
            {
                if (toggle != null && toggle.enabled)
                {
                    CurrentUnlockIntervals.Add(toggle.unlockNumber, toggle.heroType);
                }
            }
            ShouldUpdateUnlockIntervals = false;
        }

        /// <summary>
        /// Just the password to enable cheats
        /// </summary>
        private static void IAmInTheMatrix()
        {
            Main.cheat = true;
        }
    }
}
