using HarmonyLib;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using World.LevelEdit;

namespace LevelEditorExtended.Patches.DoodadMenu
{
    [HarmonyPatch(typeof(LevelEditorGUI), "ShowDoodadMenu")]
    static class ShowDoodadMenu_Patch
    {
        static bool Prefix(LevelEditorGUI __instance)
        {
            if (!Main.enabled) return true;
            try
            {
                UI.Initialize();

                if (UI.HasMenu(UI.CurDoodadType))
                {
                    UI.ShowMenu(UI.CurDoodadType);
                }
                else
                {
                    UI.OriginalMenu();
                }
                return false;
            }
            catch(Exception e)
            {
                Main.Log(e);
            }
            return true;
        }
    }

    internal static class UI
    {
        public static LevelEditorGUI Inst
        {
            get
            {
                return LevelEditorGUI.Instance;
            }
        }
        public static ThemeHolder ActiveTheme
        {
            get
            {
                return Map.Instance.activeTheme;
            }
        }
        public static SharedLevelObjectsHolder SharedObjectsReference
        {
            get
            {
                return Map.Instance.sharedObjectsReference.Asset;
            }
        }
        public static DoodadType CurDoodadType
        {
            get
            {
                return Inst.get_curDoodadType();
            }
            set
            {
                Inst.set_curDoodadType(value);
            }
        }
        public static int CurDoodadVariation
        {
            get
            {
                return Inst.GetInt("curDoodadVariation");
            }
            set
            {
                Inst.SetFieldValue("curDoodadVariation", value);
            }
        }
        public static DoodadInfo HoverDoodadInfo
        {
            get
            {
                return Inst.GetFieldValue<DoodadInfo>("hoverDoodadInfo");
            }
            set
            {
                Inst.SetFieldValue("hoverDoodadInfo", value);
            }
        }
        public static Vector2 ScrollPos
        {
            get
            {
                return Traverse.Create(typeof(LevelEditorGUI)).Field("scrollPos").GetValue<Vector2>();
            }
            set
            {
                Traverse.Create(typeof(LevelEditorGUI)).Field("scrollPos").SetValue(value);
            }
        }
        public static bool ShowAllDoodad
        {
            get
            {
                return Traverse.Create(typeof(LevelEditorGUI)).Field("showAllDoodads").GetValue<bool>();
            }
            set
            {
                Traverse.Create(typeof(LevelEditorGUI)).Field("showAllDoodads").SetValue(value);
            }
        }

        private static Vector2 _customScrollPos = Vector2.zero;

        private static Dictionary<DoodadType, Dictionary<int, string>> _typeWithMenu = new Dictionary<DoodadType, Dictionary<int, string>>();

        private static readonly Dictionary<int, string> _mooks = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "Trooper" },
            { 1, "Suicide" },
            { 2, "Riot Shield" },
            { 3, "Bruiser" },
            { 4, "Scout" },
            { 5, "Dog" },
            { 6, "Mech" },
            { 7, "Grenadier" },
            { 8, "Bazooka" },
            { 9, "Ninja" },
            { 10, "Jetpack" },
            { 11, "Xenomorph Brainbox" },
            { 12, "Skinned" },
            { 13, "Machine Gun" },
            { 14, "Jetpack Bazooka" },
            { 15, "Snake" },
            { 16, "Alarmist" },
            { 17, "Strong Trooper" },
            { 18, "Treasure Mook" },
            { 19, "Bruiser Strong" },
            { 20, "Bruiser Elite" },
            { 21, "Scientist" },
            { 22, "Mech Brown" },
            { 23, "Bruiser Suicide" },
        };
        private static readonly Dictionary<int, string> _aliens = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "Face Hugger" },
            { 1, "Xenomorph" },
            { 2, "Brute" },
            { 3, "Melter" },
            { 4, "Mosquito" },
        };
        private static readonly Dictionary<int, string> _vehicles = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "Tank" },
            { 1, "Mook Launcher" },
            { 2, "Truck" },
            { 3, "Kopter" },
            { 4, "Drill Carrier" },
            { 5, "Artillery Truck" },
            { 6, "Clone Pod" },
            { 7, "Blimp" },
            { 8, "Rhino" },
            { 9, "Motor Bike" },
            { 10, "Motor Bike Nuclear" },
            { 11, "Dump Truck" },
            { 12, "Random Truck" },
        };
        private static readonly Dictionary<int, string> _animals = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "Pig" },
            { 1, "Seagull" },
            { 2, "Pig Rotten" },
        };
        private static readonly Dictionary<int, string> _miniBosses = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "Mammoth Tank" },
            { 1, "Kopter" },
            { 2, "GR666" },
            { 3, "Kopter Mammoth" },
            { 4, "Goliath Mech" },
        };
        private static readonly Dictionary<int, string> _traps = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "Spike" },
            { 1, "Mine Field" },
            { 2, "Saw Blade" },
            { 3, "Saw Blade Reverse" },
            { 4, "Spikes Long" },
            { 5, "Spikes Hell" },
            { 6, "Crushing Wall Hell" },
            { 7, "Crushing Wall Spiked Hell" },
            { 8, "Lost Souls Door" },
            { 9, "Oil fire UP" },
            { 10, "Oil fire DOWN" },
            { 11, "Oil fire LEFT" },
            { 12, "Oil fire RIGHT" },
            { 13, "Rolling Boulder LEFT" },
            { 14, "Rolling Boulder RIGHT" },
        };
        private static readonly Dictionary<int, string> _alienSpawner = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "Xenomorph" },
            { 1, "Face Hugger" },
            { 2, "Snake Infinity" },
            { 3, "5 Snake" },
        };
        private static readonly Dictionary<int, string> _alienTraps = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "Protocol Spike" },
            { 1, "Protocol Spike Rotated" },
            { 2, "Wind Sack Suck Right" },
            { 3, "Wind Sack Suck Up" },
            { 4, "Earth Quake" },
            { 5, "Mouth" },
        };
        private static readonly Dictionary<int, string> _ammoCrate = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "Ammo" },
            { 1, "Time Slower" },
            { 2, "RC Car" },
            { 3, "Airstrike Grenade" },
            { 4, "Mech Drop" },
            { 5, "Alien Pheromones" },
            { 6, "Steroids" },
            { 7, "Pig" },
            { 8, "Flex Power" },
            { 9, "Dollars" },
            { 10, "Perk Revive" },
            { 11, "Perk Damage" },
        };
        private static readonly Dictionary<int, string> _alienTurrets = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "Slime" },
            { 1, "Mosquito Hive" },
            { 2, "Alien Mouth" },
            { 3, "Alien Mouth Small" },
            { 4, "Mosquito Hive Downard" },
        };
        private static readonly Dictionary<int, string> _alienBoss = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "Slug" },
            { 1, "Heart" },
        };
        private static readonly Dictionary<int, string> _effects = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "Water Drip Ceiling" },
            { 1, "Slime Drip Ceiling" },
            { 2, "Vision Point Attractor" },
        };
        private static readonly Dictionary<int, string> _quickTriggerAliens = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "Face Hugger" },
            { 1, "Melter Climb" },
            { 2, "Melter Charge" },
            { 3, "Call Worm Right" },
            { 4, "Call Worm Left" },
            { 5, "12 Melter Charge Right" },
            { 6, "8 Melter Charge Left" },
            { 7, "4 Melter Charge Right" },
            { 8, "4 Melter Charge Left" },
        };
        private static readonly Dictionary<int, string> _hellBoss = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "Satan" },
            { 1, "Satan: True Form" },
        };
        private static readonly Dictionary<int, string> _hellEnemies = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "Dog" },
            { 1, "Undead" },
            { 2, "Undead Start Dead" },
            { 3, "Warlock" },
            { 4, "Boomer" },
            { 5, "Undead Suicide" },
            { 6, "Bruiser" },
            { 7, "Fire Bone Worm" },
            { 8, "Lost Soul" },
            { 9, "Lost Soul" },
            { 10, "Souls Boomer" },
            { 11, "Armoured Bruiser" },
            { 12, "Bone Worm Left" },
            { 13, "Bone Worm Right" },
        };
        private static readonly Dictionary<int, string> _quickHellTriggers = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "Undead Trap" },
            { 1, "Undead Trap Single" },
            { 2, "Boomer Trap" },
            { 3, "Fire Ball" },
            { 4, "Fire Ball Right" },
            { 5, "Fire Ball Vertical" },
            { 6, "Fire Ball Vertical Right" },
            { 7, "Fire Ball Vertical Left" },
            { 8, "Fire Ball Vertical Far Right" },
            { 9, "Satan" },
            { 10, "Satan Right" },
            { 11, "Satan Left" },
            { 12, "Satan Left Delay" },
            { 13, "Satan Right Delay" },
            { 14, "Satan Far Right Delay" },
            { 15, "Satan Right Near" },
            { 16, "Fire Ball Vertical Far Far Right" },
            { 17, "Fire Ball Vertical Far Far Far Right" },
        };
        private static readonly Dictionary<int, string> _hellTurret = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "Lost Soul Up" },
            { 1, "Lost Soul Down" },
            { 2, "Lost Soul Up Reversed" },
            { 3, "Lost Soul Down Reversed" },
        };
        private static readonly Dictionary<int, string> _pureEvil = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "Satan" },
            { 1, "Conrad Brones Banks" },
            { 2, "Mook General" },
        };
        private static readonly Dictionary<int, string> _wallOfGuns = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "Rocket Up" },
            { 1, "Rocket Down" },
            { 2, "Rocket Extra Up" },
            { 3, "Rocket Extra Down" },
            { 4, "Door Of Guns" },
            { 5, "Saw Down" },
            { 6, "Saw No Engine" },
            { 7, "Saw No Engine Reversed" },
            { 8, "Rocket No Engine" },
            { 9, "Rocket No Engine Reversed" },
        };
        private static readonly Dictionary<int, string> _shopProps = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "Perk Purchase Switch" },
            { 1, "Money Crate" },
            { 2, "Money Safe" },
            { 3, "Bro Purchase Switch" },
            { 4, "Bro Choice Switch" },
            { 5, "Switch Three Bro Choice" },
            { 6, "Gym Switch" },
            { 7, "Chapel Building" },
            { 8, "Chapel Cross" },
            { 9, "Chapel Room" },
            { 10, "Nicolas Cage" },
            { 11, "Nicolas Cage Billboard" },
            { 12, "President Billboard" },
            { 13, "Agent Free Perk" },
            { 14, "McDonalds Counter" },
            { 15, "McDonalds Billboard" },
            { 16, "Farmer" },
            { 17, "Agent Perk Purchase" },
            { 18, "Hunter Perk Purchase" },
            { 19, "Hunter Billboard" },
            { 20, "Ammo Crate Purchase Switch" },
            { 21, "Gym Billboard" },
            { 22, "Crashed Plane" },
            { 23, "Chapel Respawn" },
            { 24, "Switch Damage Or Revive" },
        };
        private static readonly Dictionary<int, string> _alienGiantSandWorm = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "Miniboss" },
            { 1, "Face Hugger Spitter" },
            { 2, "Face Hugger Spitter Reversed" },
            { 3, "Giant Sandworm" },
            { 4, "Giant Sandworm Reversed" },
            { 5, "Giant Sandworm Boss" },
        };
        private static readonly Dictionary<int, string> _quickTrigger = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "Bombardement" },
            { 1, "Bombardement Right" },
            { 2, "Bombardement Far Right" },
            { 3, "Paradrop 3 Trooper Right" },
            { 4, "Paradrop 2 Suicide Right" },
            { 5, "Paradrop 2 Trooper" },
            { 6, "Suicide Charge" },
            { 7, "Paradrop Crate" },
            { 8, "Paradrop Cage" },
            { 9, "2 Dog Charge" },
            { 10, "Paradrop  Pig" },
            { 11, "Paradrop 3 Strong Trooper Right" },
            { 12, "Paradrop 2 Strong Trooper Left" },
            { 13, "Gib Area" },
        };
        private static readonly Dictionary<int, string> _tutorialObjects = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "Level Editor Welcome Sign" },
            { 1, "Arrow Sign Left" },
            { 2, "Arrow Sign Right" },
            { 3, "Arrow Sign Up" },
            { 4, "Arrow Sign Down" },
            { 5, "Arrow Sign Diag Up Left" },
            { 6, "Arrow Sign Diag Up Right" },
            { 7, "Arrow Sign Diag Down Left" },
            { 8, "Arrow Sign Diag Down Right" },
            { 9, "Arrow Sign Up Left" },
            { 10, "Arrow Sign Up Right" },
            { 11, "Arrow Sign Diag Down Left" },
            { 12, "Arrow Sign Diag Down Right" },
            { 13, "Alien Campaign Disclaimer Sign" },
            { 14, "To Be Continued Sign" },
            { 15, "Crashed Helicopter" },
            { 16, "Green Screen" },
            { 17, "Helicopter" },
            { 18, "General Briefing" },
            { 19, "President Billboard" },
            { 20, "President Billboard Choice" },
            { 21, "Arrow Sign Down Left" },
            { 22, "Arrow Sign Down Right" },
        };
        private static readonly Dictionary<int, string> _checkpointAirdrop = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "High" },
            { 1, "Low" },
        };
        private static readonly Dictionary<int, string> _sandstormDebris = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "Boulder" },
            { 1, "Boulder Spawner" },
            { 2, "Crate Spawner" },
            { 3, "Mook Spawner" },
            { 4, "Tire Spawner" },
            { 5, "Tumble Weed Spawner" },
        };
        private static readonly Dictionary<int, string> _extraExplosives = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "Petrol Tank" },
            { 1, "Petrol Tank Small" },
        };
        private static readonly Dictionary<int, string> _agents = new Dictionary<int, string>()
        {
            { -1, "Random" },
            { 0, "Agent Friendly" },
            { 1, "Agent Enemy" },
        };


        private static bool _hasInitialized = false;
        public static void Initialize()
        {
            if (_hasInitialized) return;

            _hasInitialized = true;
            _typeWithMenu = new Dictionary<DoodadType, Dictionary<int, string>>();
            _typeWithMenu.Add(DoodadType.Mook, _mooks);
            _typeWithMenu.Add(DoodadType.Alien, _aliens);
            _typeWithMenu.Add(DoodadType.Vehicle, _vehicles);
            _typeWithMenu.Add(DoodadType.Animal, _animals);
            _typeWithMenu.Add(DoodadType.Miniboss, _miniBosses);
            //_typeWithMenu.Add( DoodadType.Trap, _traps);
            _typeWithMenu.Add(DoodadType.AlienSpawner, _alienSpawner);
            _typeWithMenu.Add(DoodadType.TrapAlien, _alienTraps);
            _typeWithMenu.Add(DoodadType.AmmoCrate, _ammoCrate);
            _typeWithMenu.Add(DoodadType.AlienTurrets, _alienTurrets);
            _typeWithMenu.Add(DoodadType.AlienBoss, _alienBoss);
            _typeWithMenu.Add(DoodadType.Effects, _effects);
            _typeWithMenu.Add(DoodadType.QuickTriggerAliens, _quickTriggerAliens);
            _typeWithMenu.Add(DoodadType.HellBoss, _hellBoss);
            _typeWithMenu.Add(DoodadType.HellEnemy, _hellEnemies);
            _typeWithMenu.Add(DoodadType.QuickTriggerHell, _quickHellTriggers);
            _typeWithMenu.Add(DoodadType.HellTurrets, _hellTurret);
            _typeWithMenu.Add(DoodadType.PureEvil, _pureEvil);
            _typeWithMenu.Add(DoodadType.ShopProps, _shopProps);
            _typeWithMenu.Add(DoodadType.WallOfGuns, _wallOfGuns);
            _typeWithMenu.Add(DoodadType.AlienGiantSandWorm, _alienGiantSandWorm);
            _typeWithMenu.Add(DoodadType.QuickTrigger, _quickTrigger);
            _typeWithMenu.Add(DoodadType.TutorialObject, _tutorialObjects);
            _typeWithMenu.Add(DoodadType.CheckPointAirdrop, _checkpointAirdrop);
            _typeWithMenu.Add(DoodadType.SandstormDebris, _sandstormDebris);
            _typeWithMenu.Add(DoodadType.ExtraExplosives, _extraExplosives);
            _typeWithMenu.Add(DoodadType.Agents, _agents);
        }

        public static void OriginalMenu()
        {
            // Variation
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (UI.HoverDoodadInfo != null)
            {
                if (GUILayout.Button("R", (UI.CurDoodadVariation != -1) ? Inst.skin.button : Inst.skin.customStyles[3], new GUILayoutOption[0]) || Input.GetKeyDown(KeyCode.R))
                {
                    Inst.PlayClickSound();
                    UI.HoverDoodadInfo.variation = -1;
                    UI.CurDoodadVariation = -1;
                    Inst.PlaceHoverDoodad();
                }
                for (int i = 0; i < Map.Instance.GetDoodadVariationAmount(CurDoodadType); i++)
                {
                    if (GUILayout.Button(i.ToString(), (UI.CurDoodadVariation != i) ? Inst.skin.button : Inst.skin.customStyles[3], new GUILayoutOption[0]) || Input.GetKeyDown(KeyCode.Alpha0 + i))
                    {
                        Inst.PlayClickSound();
                        UI.HoverDoodadInfo.variation = i;
                        UI.CurDoodadVariation = i;
                        Inst.PlaceHoverDoodad();
                    }
                }
            }
            GUILayout.EndHorizontal();

            UI.ScrollPos = GUILayout.BeginScrollView(UI.ScrollPos, Inst.skin.scrollView);
            // All Doodad
            GUILayout.BeginHorizontal();
            GUILayout.Label("Filter by");
            string doodadSearch = GUILayout.TextField(Inst.GetFieldValue<string>("doodadSearch"));
            Inst.SetFieldValue("doodadSearch", doodadSearch);
            GUILayout.EndHorizontal();

            List<DoodadType> list = new FuzzySearcher<DoodadType>().FuzzySearch(Inst.get_doodadTypes(), doodadSearch);
            ShowAllDoodad = (GUILayout.Toggle(ShowAllDoodad, "Show All Doodads"));
            var doodadFilter = Inst.GetFieldValue("doodadFilter") as Dictionary<DoodadType, bool>;
            foreach (DoodadType doodadType in list)
            {
                GUILayout.BeginHorizontal();
                doodadFilter[doodadType] = GUILayout.Toggle(doodadFilter[doodadType], string.Empty, new GUILayoutOption[]
                {
                    GUILayout.Width(40f)
                });
                if (GUILayout.Button(doodadType.ToString(), (UI.CurDoodadType != doodadType) ? LevelEditorGUI.GetGuiSkin().customStyles[0] : LevelEditorGUI.GetGuiSkin().customStyles[1]))
                {
                    if (UI.CurDoodadType != doodadType)
                    {
                        UI.HoverDoodadInfo = new DoodadInfo(new GridPoint(Inst.C(), Inst.R()), UI.CurDoodadType, UI.CurDoodadVariation);
                        UI.CurDoodadType = doodadType;
                        UI.CurDoodadVariation = 0;
                        Inst.PlayClickSound();
                    }
                }
                GUILayout.EndHorizontal();
            }
            Inst.SetFieldValue("doodadFilter", doodadFilter);

            GUILayout.EndScrollView();
        }

        public static bool HasMenu(DoodadType doodadType)
        {
            return _typeWithMenu.ContainsKey(doodadType) || doodadType != DoodadType.Empty;
        }

        public static void ShowMenu(DoodadType doodadType)
        {
            Back();
            _customScrollPos = GUILayout.BeginScrollView(_customScrollPos, Inst.skin.scrollView);
            if(_typeWithMenu.ContainsKey(doodadType) && false)
                VariationMenu(_typeWithMenu[doodadType]);
            else
            {
                VariationMenu(Mod.GetDoodadArray(doodadType));
                /*switch(doodadType)
                {
                    case DoodadType.MookDoor:
                        VariationMenu(ActiveTheme.mookDoorPrefabs); break;
                    case DoodadType.OutdoorDoodad:
                        VariationMenu(ActiveTheme.outdoorDoodads); break;
                    case DoodadType.IndoorDoodad:
                        VariationMenu(ActiveTheme.indoorDoodads); break;
                    case DoodadType.TreeBushes:
                        VariationMenu(ActiveTheme.treeBushes); break;
                    case DoodadType.Tree:
                        VariationMenu(ActiveTheme.trees); break;
                    case DoodadType.Parallax3:
                        VariationMenu(ActiveTheme.parallax3); break;
                    case DoodadType.Parallax2:
                        VariationMenu(ActiveTheme.parallax2); break;
                    case DoodadType.Parallax1:
                        VariationMenu(ActiveTheme.parallax1); break;
                    case DoodadType.Cloud:
                        VariationMenu(ActiveTheme.parallaxCloudDoodads); break;
                    case DoodadType.TreeBackground:
                        VariationMenu(ActiveTheme.treesBackground); break;
                    case DoodadType.TreeBushBackground:
                        VariationMenu(ActiveTheme.treeBushesBackground); break;
                    case DoodadType.Trap:
                        VariationMenu(ActiveTheme.trapDoodads); break;
                    case DoodadType.Fence:
                        VariationMenu(ActiveTheme.fenceDoodads); break;
                    case DoodadType.Pole:
                        VariationMenu(ActiveTheme.poleDoodads); break;
                    case DoodadType.Switch:
                        VariationMenu(ActiveTheme.switchPrefab); break;
                    case DoodadType.Alarm:
                        VariationMenu(ActiveTheme.alarmDoodads); break;
                    case DoodadType.Door:
                        VariationMenu(ActiveTheme.doorDoodads); break;
                    case DoodadType.Scaffolding:
                        VariationMenu(ActiveTheme.scaffoldingForegroundDoodads); break;
                    case DoodadType.ScaffoldingBackground:
                        VariationMenu(ActiveTheme.scaffoldingBackgroundDoodads); break;
                    case DoodadType.HangingDoodads:
                        VariationMenu(ActiveTheme.hangingDoodads); break;
                    case DoodadType.TerrainRotators:
                        VariationMenu(ActiveTheme.terrainHelpers); break;
                    case DoodadType.WaterSource:
                        VariationMenu(ActiveTheme.waterSourceDoodads); break;
                    case DoodadType.PureEvilDecor:
                        VariationMenu(ActiveTheme.pureEvilDecor); break;
                    case DoodadType.TreeFoliageBackground:
                        VariationMenu(ActiveTheme.foliageBackground); break;
                    case DoodadType.Villager:
                        VariationMenu(ActiveTheme.villager1); break;
                    case DoodadType.Zipline:
                        VariationMenu(ActiveTheme.ziplineDoodads); break;
                    case DoodadType.Crate:
                        VariationMenu(ActiveTheme.crateDoodads); break;
                    case DoodadType.WallOfGuns:
                        VariationMenu(ActiveTheme.wallOfGuns); break;
                    case DoodadType.Elevator:
                        VariationMenu(ActiveTheme.elevators); break;
                    case DoodadType.BackgroundWindowFactory:
                        VariationMenu(ActiveTheme.backgroundwindowsFactory); break;
                    case DoodadType.BackgroundWindowStone:
                        VariationMenu(ActiveTheme.backgroundwindowsStone); break;
                    case DoodadType.TreeBushForeground:
                        VariationMenu(ActiveTheme.treeBushesForeground); break;
                    case DoodadType.QuickTrigger:
                        VariationMenu(SharedObjectsReference.triggerObjects); break;
                    case DoodadType.TutorialObject:
                        VariationMenu(SharedObjectsReference.tutorialObjects); break;
                    case DoodadType.VillagerCaptured:
                        VariationMenu(ActiveTheme.capturedVillagers); break;
                    case DoodadType.TrapAlien:
                        VariationMenu(ActiveTheme.alienTrapDoodads); break;
                    case DoodadType.CaveParallax1:
                        VariationMenu(ActiveTheme.caveParallax1); break;
                    case DoodadType.CaveParallax2:
                        VariationMenu(ActiveTheme.caveParallax2); break;
                    case DoodadType.CaveParallax3:
                        VariationMenu(ActiveTheme.caveParallax3); break;
                    case DoodadType.Boulder:
                        VariationMenu(ActiveTheme.boulders); break;
                    case DoodadType.AlienFronds:
                        VariationMenu(ActiveTheme.alienFronds); break;
                    case DoodadType.AlienTurrets:
                        VariationMenu(ActiveTheme.alienTurrets); break;
                    case DoodadType.AlienBackgroundDecoration:
                        VariationMenu(ActiveTheme.alienBackgroundDecorations); break;
                    case DoodadType.AlienDoor:
                        VariationMenu(ActiveTheme.alienDoorPrefabs); break;
                    case DoodadType.AlienTerrainDecoration:
                        VariationMenu(ActiveTheme.decorationAlienTerrain); break;
                    case DoodadType.Effects:
                        VariationMenu(SharedObjectsReference.effects); break;
                    case DoodadType.QuickTriggerAliens:
                        VariationMenu(SharedObjectsReference.triggerAlienObjects); break;
                    case DoodadType.HellEnemy:
                        VariationMenu(SharedObjectsReference.hellEnemies); break;
                    case DoodadType.BackgroundWindowVault:
                        VariationMenu(SharedObjectsReference.backgroundwindowsVault); break;
                    case DoodadType.VentDoodad:
                        VariationMenu(SharedObjectsReference.ventDoodads); break;
                    case DoodadType.BackgroundPiecesAlien:
                        VariationMenu(SharedObjectsReference.backgroundPiecesAlien); break;
                    case DoodadType.Fog:
                        VariationMenu(SharedObjectsReference.caveFog); break;
                    case DoodadType.HellDecoration:
                        VariationMenu(ActiveTheme.hellDecorations); break;
                    case DoodadType.ParallaxDecoration:
                        VariationMenu(ActiveTheme.parallaxDecorations); break;
                    case DoodadType.QuickTriggerHell:
                        VariationMenu(SharedObjectsReference.triggerHellObjects); break;
                    case DoodadType.HellFog:
                        VariationMenu(SharedObjectsReference.hellFog); break;
                    case DoodadType.HellTurrets:
                        VariationMenu(SharedObjectsReference.hellTurrets); break;
                    case DoodadType.BackgroundWindowCity:
                        VariationMenu(SharedObjectsReference.backgroundwindowsCity); break;
                    case DoodadType.LightFog:
                        VariationMenu(SharedObjectsReference.lightFog); break;
                    case DoodadType.SandstormDebris:
                        VariationMenu(SharedObjectsReference.sandstormDebris); break;
                    case DoodadType.CityBackgroundProp:
                        VariationMenu(SharedObjectsReference.cityBackgroundProps); break;
                    case DoodadType.RefugeeProps:
                        VariationMenu(SharedObjectsReference.refugeeProps); break;
                    case DoodadType.ShopProps:
                        VariationMenu(SharedObjectsReference.shopProps); break;
                    case DoodadType.ExtraExplosives:
                        VariationMenu(SharedObjectsReference.extraExplosives); break;
                    case DoodadType.DesertParallax:
                        VariationMenu(SharedObjectsReference.desertParallax); break;
                    case DoodadType.DesertParallaxFiller:
                        VariationMenu(SharedObjectsReference.desertParallaxFiller); break;
                    case DoodadType.BackgroundTrench:
                        VariationMenu(SharedObjectsReference.backgroundTrench); break;
                    case DoodadType.Agents:
                        VariationMenu(SharedObjectsReference.agents); break;
                    case DoodadType.ZoomAreas:
                        VariationMenu(SharedObjectsReference.shopZoomAreas); break;
                    case DoodadType.BackgroundTemple:
                        VariationMenu(SharedObjectsReference.backgroundTemple); break;
                    case DoodadType.DesertFog:
                        VariationMenu(SharedObjectsReference.desertFog); break;
                }*/
            }

            /*switch (doodadType)
            {
                case DoodadType.Mook:
                    VariationMenu(_mooks);
                    break;
                case DoodadType.Alien:
                    VariationMenu(_aliens);
                    break;
                case DoodadType.Vehicle:
                    VariationMenu(_vehicles);
                    break;
                case DoodadType.Animal:
                    VariationMenu(_animals);
                    break;
                case DoodadType.Miniboss:
                    VariationMenu(_miniBosses);
                    break;
                case DoodadType.Trap:
                    VariationMenu(_traps);
                    break;
                case DoodadType.AlienSpawner:
                    VariationMenu(_alienSpawner);
                    break;
                case DoodadType.TrapAlien:
                    VariationMenu(_alienTraps);
                    break;
                case DoodadType.AmmoCrate:
                    VariationMenu(_ammoCrate);
                    break;
                case DoodadType.AlienTurrets:
                    VariationMenu(_alienTurrets);
                    break;
                case DoodadType.AlienBoss:
                    VariationMenu(_alienBoss);
                    break;
                case DoodadType.Effects:
                    VariationMenu(_effects);
                    break;
                case DoodadType.QuickTriggerAliens:
                    VariationMenu(_quickTriggerAliens);
                    break;
                case DoodadType.HellBoss:
                    VariationMenu(_hellBoss);
                    break;
                case DoodadType.HellEnemy:
                    VariationMenu(_hellEnemies);
                    break;
                case DoodadType.QuickTriggerHell:
                    VariationMenu(_quickHellTriggers);
                    break;
                case DoodadType.HellTurrets:
                    VariationMenu(_hellTurret);
                    break;
                case DoodadType.PureEvil:
                    VariationMenu(_pureEvil);
                    break;
                case DoodadType.ShopProps:
                    VariationMenu(_shopProps);
                    break;
                case DoodadType.WallOfGuns:
                    VariationMenu(_wallOfGuns);
                    break;
            }*/
            GUILayout.EndScrollView();
        }



        private static void VariationMenu(Dictionary<int, string> variations)
        {
            int i = 0;
            foreach (var kvp in variations)
            {
                if (GUILayout.Button($"{kvp.Key}: {kvp.Value}", (CurDoodadVariation != kvp.Key) ? Inst.skin.button : Inst.skin.customStyles[3], new GUILayoutOption[0]) || (kvp.Key == -1 ? Input.GetKeyDown(KeyCode.R) : Input.GetKeyDown(KeyCode.Alpha0 + kvp.Key)))
                {
                    Inst.PlayClickSound();
                    UI.HoverDoodadInfo.variation = kvp.Key;
                    UI.CurDoodadVariation = kvp.Key;
                    Inst.PlaceHoverDoodad();
                }
                i++;
            }

        }

        private static void VariationMenu(GameObject[] array)
        {
            try
            {
                if (GUILayout.Button("Random", (CurDoodadVariation != -1) ? Inst.skin.button : Inst.skin.customStyles[3], new GUILayoutOption[0]) || Input.GetKeyDown(KeyCode.R))
                {
                    Inst.PlayClickSound();
                    UI.HoverDoodadInfo.variation = -1;
                    UI.CurDoodadVariation = -1;
                    Inst.PlaceHoverDoodad();
                }

                for (int i = 0; i < array.Length; i++)
                {
                    GameObject go = array[i];
                    if (GUILayout.Button($"{i}: {go.name}", (CurDoodadVariation != i) ? Inst.skin.button : Inst.skin.customStyles[3], new GUILayoutOption[0]) || (Input.GetKeyDown(KeyCode.Alpha0 + i)))
                    {
                        Inst.PlayClickSound();
                        UI.HoverDoodadInfo.variation = i;
                        UI.CurDoodadVariation = i;
                        Inst.PlaceHoverDoodad();
                    }
                }
            }
            catch(Exception ex)
            {
                Main.Log(ex);
                _customScrollPos = Vector2.zero;
                CurDoodadType = DoodadType.Empty;
            }

        }

        private static void Back()
        {
            if (GUILayout.Button("Back", LevelEditorGUI.GetGuiSkin().customStyles[0]))
            {
                _customScrollPos = Vector2.zero;
                CurDoodadType = DoodadType.Empty;
            }
            GUILayout.Space(15);
        }

        private static string GetDoodadTypeName(DoodadType doodadType)
        {
            switch (doodadType)
            {
                default:
                    return doodadType.ToString();
            }
        }
    }
}
