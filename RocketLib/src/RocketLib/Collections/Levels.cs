using System.Collections.Generic;

namespace RocketLib.Collections
{
    public static class Levels
    {
        public const string ROGUEFORCE_TEST = "aaaRogueforce";

        public static string[] All
        {
            get
            {
                if (_all.IsNullOrEmpty())
                {
                    var temp = new List<string>();
                    temp.AddRange(WorldMap.All);
                    temp.AddRange(Challenges.All);
                    temp.AddRange(MuscleTemples.All);
                    temp.AddRange(Campaigns.All);
                    temp.AddRange(Defaults.All);
                    temp.AddRange(WWB.All);
                    temp.Add(ROGUEFORCE_TEST);
                    _all = temp.ToArray();
                }
                return _all;
            }
        }
        private static string[] _all;

        public static class Challenges
        {
            public const string PHEROMONES = "Challenge_Alien";
            public const string AMMO = "Challenge_Ammo";
            public const string STEROIDS = "Challenge_Ammo";
            public const string RC_CAR = "Challenge_MacBrover";
            public const string MECH_DROP = "Challenge_Mech1";
            public const string TIME_WATCH = "Challenge_TimeBro";

            public static string[] All
            {
                get
                {
                    if (_all.IsNullOrEmpty())
                        _all = new string[] { AMMO, MECH_DROP, PHEROMONES, RC_CAR, STEROIDS, TIME_WATCH };
                    return _all;
                }
            }
            private static string[] _all;
        }

        public static class MuscleTemples
        {
            public const string GOLDEN_LIGTH = "MuscleTemple_1";
            public const string INVINCIBLE = "MuscleTemple_2";
            public const string AIR_FLEX = "MuscleTemple_3";
            public const string TELEPORT = "MuscleTemple_4";

            public static string[] All
            {
                get
                {
                    if (_all.IsNullOrEmpty())
                        _all = new string[] { GOLDEN_LIGTH, INVINCIBLE, AIR_FLEX, TELEPORT };
                    return _all;
                }
            }
            private static string[] _all;
        }

        public static class WorldMap
        {
            public static string[] All
            {
                get
                {
                    if (_all.IsNullOrEmpty())
                    {
                        _all = new string[] { INTRO, MISSION_1, MISSION_2, VILLAGE, WHITEHOUSE, };
                        var temp = new List<string>(_all);
                        temp.AddRange(Aliens.All);
                        temp.AddRange(Bombardements.All);
                        temp.AddRange(City.All);
                        temp.AddRange(Kazakhstan.All);
                        _all = temp.ToArray();
                    }
                    return _all;
                }
            }
            private static string[] _all;

            public const string HELL = "WM_Hell";
            public const string INTRO = "WM_Intro(mouse)";
            public const string MISSION_1 = "WM_Mission1(mouse)";
            public const string MISSION_2 = "WM_Mission2(mouse)";
            public const string VILLAGE = "WM_Village1(mouse)";
            public const string WHITEHOUSE = "WM_Whitehouse";
            public static class Aliens
            {
                public const string FIRST = "WM_AlienMission1(mouse)";
                public const string SECOND = "WM_AlienMission2(mouse)";
                public const string THIRD = "WM_AlienMission3(mouse)";
                public const string FOURTH = "WM_AlienMission4(mouse)";

                public static string[] All
                {
                    get
                    {
                        if (_all.IsNullOrEmpty())
                            _all = new string[] { FIRST, SECOND, THIRD, FOURTH };
                        return _all;
                    }
                }
                private static string[] _all;
            }

            public static class Bombardements
            {
                public const string FIRST = "WM_Bombardment(mouse)";
                public const string SECOND = "WM_Bombardment2(mouse)";

                public static string[] All
                {
                    get
                    {
                        if (_all.IsNullOrEmpty())
                            _all = new string[] { FIRST, SECOND };
                        return _all;
                    }
                }
                private static string[] _all;
            }

            public static class City
            {
                public const string FIRST = "WM_City1(mouse)";
                public const string SECOND = "WM_City2(mouse)";

                public static string[] All
                {
                    get
                    {
                        if (_all.IsNullOrEmpty())
                            _all = new string[] { FIRST, SECOND };
                        return _all;
                    }
                }
                private static string[] _all;
            }
            public static class Kazakhstan
            {
                public const string INDUSTRIAL = "WM_KazakhstanIndustrial(mouse)";
                public const string RAINY = "WM_KazakhstanRainy(mouse)";

                public static string[] All
                {
                    get
                    {
                        if (_all.IsNullOrEmpty())
                            _all = new string[] { INDUSTRIAL, RAINY };
                        return _all;
                    }
                }
                private static string[] _all;
            }

        }

        public static class WWB
        {
            public const string ELEVATOR_ACTION = "WWB2ElevatorAction";
            public const string MOUNT_BROLYMPUS = "WWB2ElevatorAction";
            public const string SEVEN = "WWB7";
            public const string HEIGHT = "WWB8";

            public static string[] All
            {
                get
                {
                    if (_all.IsNullOrEmpty())
                        _all = new string[] { ELEVATOR_ACTION, MOUNT_BROLYMPUS, SEVEN, HEIGHT };
                    return _all;
                }
            }
            private static string[] _all;
        }

        public static class Campaigns
        {
            public const string EXPENDABROS = "Expendabros_Campaign";
            public const string TWITCHCON_EXHIBITION = "VIETNAM_EXHIBITION_TWITCHCON";
            public const string ALIEN_EXHIBITION = "AlienExhibition";
            public const string BOSS_RUSH = "BossRushCampaign";
            public const string VIETNAM = "vietnam";
            public const string VIETNAM_NETWORKED = "VietnamNetworked";

            public static string[] All
            {
                get
                {
                    if (_all.IsNullOrEmpty())
                        _all = new string[] { EXPENDABROS, TWITCHCON_EXHIBITION, ALIEN_EXHIBITION, BOSS_RUSH, VIETNAM, VIETNAM_NETWORKED };
                    return _all;
                }
            }
            private static string[] _all;
        }

        public static class Defaults
        {
            public const string BRODOWN = "DefaultBrowdown";
            public const string DEATHMATCH = "DefaultDeathmatch";
            public const string RACE = "DefaultRace";

            public static string[] All
            {
                get
                {
                    if (_all.IsNullOrEmpty())
                        _all = new string[] { BRODOWN, DEATHMATCH, RACE };
                    return _all;
                }
            }
            private static string[] _all;
        }

        public static void LoadLevel(string campaignName, bool hard = false, int levelNumber = 0, MapLoadMode loadMode = MapLoadMode.Campaign, GameMode gameMode = GameMode.Campaign)
        {
            LevelSelectionController.ResetLevelAndGameModeToDefault();
            GameState.Instance.ResetToDefault();
            GameState.Instance.campaignName = campaignName;
            GameState.Instance.loadMode = loadMode;
            GameState.Instance.gameMode = gameMode;
            GameState.Instance.sceneToLoad = LevelSelectionController.CampaignScene;
            GameState.Instance.sessionID = Connect.GetIncrementedSessionID().AsByte;
            //GameState.Instance.arcadeHardMode
            LevelSelectionController.CurrentLevelNum = levelNumber;
            HeroUnlockController.Initialize();

            Networking.Networking.AdminRPC<GameState>(PID.TargetOthers, true, new RpcSignature<GameState>(GameModeController.LoadNextSceneFade), GameState.Instance);
            GameModeController.LoadNextSceneFade(GameState.Instance);
            RPCBatcher.FlushQueue();
        }
    }
}
