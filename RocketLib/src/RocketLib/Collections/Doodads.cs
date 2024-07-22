using BundleReferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RocketLib.Collections
{
    public static class Doodads
    {
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

        public static class Enemies
        {
            public static GameObject[] WallOfGuns
            {
                get
                {
                    if (_wallOfGuns.IsNullOrEmpty())
                        _wallOfGuns = ActiveTheme.wallOfGuns;
                    return _wallOfGuns;
                }
            }
            private static GameObject[] _wallOfGuns;
            public static GameObject[] MookDoors
            {
                get
                {
                    if (_mookDoors.IsNullOrEmpty())
                        _mookDoors = ActiveTheme.mookDoorPrefabs.ToGameObjects();
                    return _mookDoors;
                }
            }
            private static GameObject[] _mookDoors;
        }

        public static class Environments
        {
            public static GameObject[] TreeBushes
            {
                get
                {
                    if (_treeBushes.IsNullOrEmpty())
                        _treeBushes = ActiveTheme.treeBushes.ToGameObjects();
                    return _treeBushes;
                }
            }
            private static GameObject[] _treeBushes;
            public static GameObject[] Trees
            {
                get
                {
                    if (_trees.IsNullOrEmpty())
                        _trees = ActiveTheme.trees;
                    return _trees;
                }
            }
            private static GameObject[] _trees;
            public static GameObject[] Parallaxes1
            {
                get
                {
                    if (_parallaxes1.IsNullOrEmpty())
                        _parallaxes1 = ActiveTheme.parallax1.ToGameObjects();
                    return _parallaxes1;
                }
            }
            private static GameObject[] _parallaxes1;
            public static GameObject[] Parallaxes2
            {
                get
                {
                    if (_parallaxes2.IsNullOrEmpty())
                        _parallaxes2 = ActiveTheme.parallax2.ToGameObjects();
                    return _parallaxes2;
                }
            }
            private static GameObject[] _parallaxes2;
            public static GameObject[] Parallaxes3
            {
                get
                {
                    if (_parallaxes3.IsNullOrEmpty())
                        _parallaxes3 = ActiveTheme.parallax3.ToGameObjects();
                    return _parallaxes3;
                }
            }
            private static GameObject[] _parallaxes3;
            public static GameObject[] ParallaxCloud
            {
                get
                {
                    if (_parallaxCloud.IsNullOrEmpty())
                        _parallaxCloud = ActiveTheme.parallaxCloudDoodads.ToGameObjects();
                    return _parallaxCloud;
                }
            }
            private static GameObject[] _parallaxCloud;
            public static GameObject[] TreesBackground
            {
                get
                {
                    if (_treesBackground.IsNullOrEmpty())
                        _treesBackground = ActiveTheme.treesBackground.ToGameObjects();
                    return _treesBackground;
                }
            }
            private static GameObject[] _treesBackground;
            public static GameObject[] FoliageBackground
            {
                get
                {
                    if (_foliageBackground.IsNullOrEmpty())
                        _foliageBackground = ActiveTheme.foliageBackground.ToGameObjects();
                    return _foliageBackground;
                }
            }
            private static GameObject[] _foliageBackground;
            public static GameObject[] TreeBushesBackground
            {
                get
                {
                    if (_treeBushesBackground.IsNullOrEmpty())
                        _treeBushesBackground = ActiveTheme.treeBushesBackground.ToGameObjects();
                    return _treeBushesBackground;
                }
            }
            private static GameObject[] _treeBushesBackground;
        }


        public static class Structures
        {
            public static GameObject[] ScaffoldingsForeground
            {
                get
                {
                    if (_scaffoldingsForeground.IsNullOrEmpty())
                        _scaffoldingsForeground = ActiveTheme.scaffoldingForegroundDoodads.ToGameObjects();
                    return _scaffoldingsForeground;
                }
            }
            private static GameObject[] _scaffoldingsForeground;
            public static GameObject[] ScaffoldingsBackground
            {
                get
                {
                    if (_scaffoldingsBackground.IsNullOrEmpty())
                        _scaffoldingsBackground = ActiveTheme.scaffoldingBackgroundDoodads.ToGameObjects();
                    return _scaffoldingsBackground;
                }
            }
            private static GameObject[] _scaffoldingsBackground;
        }

        public static class Interactables
        {
            public static GameObject[] Switches
            {
                get
                {
                    if (_switches.IsNullOrEmpty())
                        _switches = ActiveTheme.switchPrefab;
                    return _switches;
                }
            }
            private static GameObject[] _switches;
            public static GameObject[] Alarms
            {
                get
                {
                    if (_alarms.IsNullOrEmpty())
                        _alarms = ActiveTheme.alarmDoodads.ToGameObjects();
                    return _alarms;
                }
            }
            private static GameObject[] _alarms;
        }

        public static class Decors
        {
            public static GameObject[] Fences
            {
                get
                {
                    if (_fences.IsNullOrEmpty())
                        _fences = ActiveTheme.fenceDoodads.ToGameObjects();
                    return _fences;
                }
            }
            private static GameObject[] _fences;
            public static GameObject[] Poles
            {
                get
                {
                    if (_poles.IsNullOrEmpty())
                        _poles = ActiveTheme.poleDoodads.ToGameObjects();
                    return _poles;
                }
            }
            private static GameObject[] _poles;
            public static GameObject[] Doors
            {
                get
                {
                    if (_doors.IsNullOrEmpty())
                        _doors = ActiveTheme.doorDoodads.ToGameObjects();
                    return _doors;
                }
            }
            private static GameObject[] _doors;
            public static GameObject[] OutdoorDoodads
            {
                get
                {
                    if (_outdoorDoodads.IsNullOrEmpty())
                        _outdoorDoodads = ActiveTheme.outdoorDoodads.ToGameObjects();
                    return _outdoorDoodads;
                }
            }
            private static GameObject[] _outdoorDoodads;
            public static GameObject[] IndoorDoodad
            {
                get
                {
                    if (_indoorDoodad.IsNullOrEmpty())
                        _indoorDoodad = ActiveTheme.indoorDoodads.ToGameObjects();
                    return _indoorDoodad;
                }
            }
            private static GameObject[] _indoorDoodad;
            public static GameObject[] HangingDoodads
            {
                get
                {
                    if (_hangingDoodads.IsNullOrEmpty())
                        _hangingDoodads = ActiveTheme.hangingDoodads.ToGameObjects();
                    return _hangingDoodads;
                }
            }
            private static GameObject[] _hangingDoodads;
            public static GameObject[] PureEvil
            {
                get
                {
                    if (_pureEvil.IsNullOrEmpty())
                        _pureEvil = ActiveTheme.pureEvilDecor.ToGameObjects();
                    return _pureEvil;
                }
            }
            private static GameObject[] _pureEvil;
        }

        public static GameObject[] TerrainRotators
        {
            get
            {
                if (_terrainRotators.IsNullOrEmpty())
                    _terrainRotators = ActiveTheme.terrainHelpers.ToGameObjects();
                return _terrainRotators;
            }
        }
        private static GameObject[] _terrainRotators;
        public static GameObject[] WaterSources
        {
            get
            {
                if (_waterSources.IsNullOrEmpty())
                    _waterSources = ActiveTheme.waterSourceDoodads.ToGameObjects();
                return _waterSources;
            }
        }
        private static GameObject[] _waterSources;
        public static GameObject[] Villagers
        {
            get
            {
                if (_villagers.IsNullOrEmpty())
                    _villagers = ActiveTheme.villager1.ToGameObjects();
                return _villagers;
            }
        }
        private static GameObject[] _villagers;
        public static GameObject[] Ziplines
        {
            get
            {
                if (_ziplines.IsNullOrEmpty())
                    _ziplines = ActiveTheme.ziplineDoodads.ToGameObjects();
                return _ziplines;
            }
        }
        private static GameObject[] _ziplines;
        public static GameObject[] Crates
        {
            get
            {
                if (_crates.IsNullOrEmpty())
                    _crates = ActiveTheme.crateDoodads.ToGameObjects();
                return _crates;
            }
        }
        private static GameObject[] _crates;
        public static GameObject[] Traps
        {
            get
            {
                if (_traps.IsNullOrEmpty())
                    _traps = ActiveTheme.trapDoodads.ToGameObjects();
                return _traps;
            }
        }
        private static GameObject[] _traps;

        public static GameObject[] GetDoodadsFromType(DoodadType doodadType)
        {
            if (ActiveTheme == null || SharedObjectsReference == null)
                return null;

            switch (doodadType)
            {
                #region Enemies
                case DoodadType.MookDoor:
                    return Enemies.MookDoors;
                case DoodadType.WallOfGuns:
                    return Enemies.WallOfGuns;
                #endregion
                #region Environments
                case DoodadType.TreeBushes:
                    return Environments.TreeBushes;
                case DoodadType.Tree:
                    return Environments.Trees;
                case DoodadType.Parallax3:
                    return Environments.Parallaxes3;
                case DoodadType.Parallax2:
                    return Environments.Parallaxes2;
                case DoodadType.Parallax1:
                    return Environments.Parallaxes1;
                case DoodadType.Cloud:
                    return Environments.ParallaxCloud;
                case DoodadType.TreeBackground:
                    return Environments.TreesBackground;
                case DoodadType.TreeBushBackground:
                    return Environments.TreeBushesBackground;
                case DoodadType.TreeFoliageBackground:
                    return Environments.FoliageBackground;
                #endregion
                #region Interactables
                case DoodadType.Switch:
                    return Interactables.Switches;
                case DoodadType.Alarm:
                    return Interactables.Alarms;
                #endregion
                #region Structures
                case DoodadType.Scaffolding:
                    return Structures.ScaffoldingsForeground;
                case DoodadType.ScaffoldingBackground:
                    return Structures.ScaffoldingsBackground;
                #endregion
                #region Decors
                case DoodadType.Fence:
                    return Decors.Fences;
                case DoodadType.Pole:
                    return Decors.Poles;
                case DoodadType.Door:
                    return Decors.Doors;
                case DoodadType.HangingDoodads:
                    return Decors.HangingDoodads;
                case DoodadType.OutdoorDoodad:
                    return Decors.OutdoorDoodads;
                case DoodadType.IndoorDoodad:
                    return Decors.IndoorDoodad;
                case DoodadType.PureEvilDecor:
                    return Decors.PureEvil;
                #endregion

                case DoodadType.Trap:
                    return Traps;
                case DoodadType.TerrainRotators:
                    return TerrainRotators;
                case DoodadType.WaterSource:
                    return WaterSources;
                case DoodadType.Villager:
                    return Villagers;
                case DoodadType.Zipline:
                    return Ziplines;
                case DoodadType.Crate:
                    return Crates;

                case DoodadType.Elevator:
                    return ActiveTheme.elevators.ToGameObjects();
                case DoodadType.BackgroundWindowFactory:
                    return ActiveTheme.backgroundwindowsFactory.ToGameObjects();
                case DoodadType.BackgroundWindowStone:
                    return ActiveTheme.backgroundwindowsStone.ToGameObjects();
                case DoodadType.TreeBushForeground:
                    return ActiveTheme.treeBushesForeground.ToGameObjects();
                case DoodadType.QuickTrigger:
                    return SharedObjectsReference.triggerObjects;
                case DoodadType.TutorialObject:
                    return SharedObjectsReference.tutorialObjects;
                case DoodadType.VillagerCaptured:
                    return ActiveTheme.capturedVillagers.ToGameObjects();
                case DoodadType.TrapAlien:
                    return ActiveTheme.alienTrapDoodads.ToGameObjects();
                case DoodadType.CaveParallax1:
                    return ActiveTheme.caveParallax1.ToGameObjects();
                case DoodadType.CaveParallax2:
                    return ActiveTheme.caveParallax2.ToGameObjects();
                case DoodadType.CaveParallax3:
                    return ActiveTheme.caveParallax3.ToGameObjects();
                case DoodadType.Boulder:
                    return ActiveTheme.boulders;
                case DoodadType.AlienFronds:
                    return ActiveTheme.alienFronds.ToGameObjects();
                case DoodadType.AlienTurrets:
                    return ActiveTheme.alienTurrets;
                case DoodadType.AlienBackgroundDecoration:
                    return ActiveTheme.alienBackgroundDecorations;
                case DoodadType.AlienDoor:
                    return ActiveTheme.alienDoorPrefabs.ToGameObjects();
                case DoodadType.AlienTerrainDecoration:
                    return ActiveTheme.decorationAlienTerrain;
                case DoodadType.Effects:
                    return SharedObjectsReference.effects;
                case DoodadType.QuickTriggerAliens:
                    return SharedObjectsReference.triggerAlienObjects;
                case DoodadType.HellEnemy:
                    return SharedObjectsReference.hellEnemies;
                case DoodadType.BackgroundWindowVault:
                    return SharedObjectsReference.backgroundwindowsVault.ToGameObjects();
                case DoodadType.VentDoodad:
                    return SharedObjectsReference.ventDoodads.ToGameObjects();
                case DoodadType.BackgroundPiecesAlien:
                    return SharedObjectsReference.backgroundPiecesAlien.ToGameObjects();
                case DoodadType.Fog:
                    return SharedObjectsReference.caveFog;
                case DoodadType.HellDecoration:
                    return ActiveTheme.hellDecorations;
                case DoodadType.ParallaxDecoration:
                    return ActiveTheme.parallaxDecorations.ToGameObjects();
                case DoodadType.QuickTriggerHell:
                    return SharedObjectsReference.triggerHellObjects;
                case DoodadType.HellFog:
                    return SharedObjectsReference.hellFog;
                case DoodadType.HellTurrets:
                    return SharedObjectsReference.hellTurrets;
                case DoodadType.BackgroundWindowCity:
                    return SharedObjectsReference.backgroundwindowsCity.ToGameObjects();
                case DoodadType.LightFog:
                    return SharedObjectsReference.lightFog;
                case DoodadType.SandstormDebris:
                    return SharedObjectsReference.sandstormDebris.ToGameObjects();
                case DoodadType.CityBackgroundProp:
                    return SharedObjectsReference.cityBackgroundProps.ToGameObjects();
                case DoodadType.RefugeeProps:
                    return SharedObjectsReference.refugeeProps;
                case DoodadType.ShopProps:
                    return SharedObjectsReference.shopProps;
                case DoodadType.ExtraExplosives:
                    return SharedObjectsReference.extraExplosives;
                case DoodadType.DesertParallax:
                    return SharedObjectsReference.desertParallax.ToGameObjects();
                case DoodadType.DesertParallaxFiller:
                    return SharedObjectsReference.desertParallaxFiller.ToGameObjects();
                case DoodadType.BackgroundTrench:
                    return SharedObjectsReference.backgroundTrench.ToGameObjects();
                case DoodadType.Agents:
                    return SharedObjectsReference.agents.ToGameObjects();
                case DoodadType.ZoomAreas:
                    return SharedObjectsReference.shopZoomAreas;
                case DoodadType.BackgroundTemple:
                    return SharedObjectsReference.backgroundTemple.ToGameObjects();
                case DoodadType.DesertFog:
                    return SharedObjectsReference.desertFog;

                // Single
                case DoodadType.Cage:
                    return new GameObject[] { ActiveTheme.blockPrefabCage.gameObject };
                case DoodadType.CageEmpty:
                    return new GameObject[] { ActiveTheme.blockPrefabCageEmpty.gameObject };
                case DoodadType.CheckPoint:
                    return new GameObject[] { ActiveTheme.checkPointPrefab.gameObject };
                case DoodadType.VerticalCheckPoint:
                    return new GameObject[] { ActiveTheme.verticalCheckpointPrefab.gameObject };
                case DoodadType.HorizontalCheckPoint:
                    return new GameObject[] { ActiveTheme.horizontalCheckpointPrefab.gameObject };
                case DoodadType.Signpost:
                    return new GameObject[] { ActiveTheme.signPostDoodad.gameObject };
                case DoodadType.PetrolTanker:
                    return new GameObject[] { ActiveTheme.petrolTanker.gameObject };
                case DoodadType.ExperimentalTerrainBoss:
                    return new GameObject[] { ActiveTheme.verticalBossMachine.gameObject };
                case DoodadType.AlienCage:
                    return new GameObject[] { ActiveTheme.blockPrefabAlienCage.gameObject };
                case DoodadType.ForceFieldHell:
                    return new GameObject[] { SharedObjectsReference.forceFieldWorldDestroyer.gameObject };
                case DoodadType.SetpieceBackground:
                    return new GameObject[] { SharedObjectsReference.finalBossBackgroundPortal };
                case DoodadType.SafeArea:
                    return new GameObject[] { SharedObjectsReference.safeAreaDoodad.gameObject };

                // Fractionned
                case DoodadType.PureEvil:
                    return new TestVanDammeAnim[] { ActiveTheme.satan, ActiveTheme.conradBroneBanks, ActiveTheme.mookGeneral }.ToGameObjects();
                case DoodadType.Mook:
                    return new Unit[] { ActiveTheme.mook, ActiveTheme.mookSuicide, ActiveTheme.mookRiotShield, ActiveTheme.mookBigGuy,
                        ActiveTheme.mookScout, ActiveTheme.mookScout, ActiveTheme.mookDog, ActiveTheme.mookArmoured, ActiveTheme.mookGrenadier,
                        ActiveTheme.mookBazooka, ActiveTheme.mookNinja, ActiveTheme.mookJetpack, ActiveTheme.mookXenomorphBrainbox,
                        ActiveTheme.skinnedMook, ActiveTheme.mookJetpackBazooka, ActiveTheme.snake, ActiveTheme.mookAlarmist, ActiveTheme.mookStrong,
                        SharedObjectsReference.treasureMook, SharedObjectsReference.mookBigGuyStrong, ActiveTheme.mookBigGuyElite, ActiveTheme.mookScientist,
                        SharedObjectsReference.mechBrown, ActiveTheme.mookSuicideBigGuy, ActiveTheme.sandbag
                    }.ToGameObjects();
                case DoodadType.Alien:
                    return new TestVanDammeAnim[] { ActiveTheme.alienFaceHugger, ActiveTheme.alienXenomorph, ActiveTheme.alienBrute, ActiveTheme.alienBaneling, ActiveTheme.alienMosquito }.ToGameObjects();
                case DoodadType.Vehicle:
                    return new BroforceObject[] { ActiveTheme.mookTankRockets, ActiveTheme.mookTankMookLauncher, ActiveTheme.mookTruck, ActiveTheme.mookKopter,
                        ActiveTheme.mookDrillCarrier, ActiveTheme.mookArtilleryTruck, ActiveTheme.mookClonePod, ActiveTheme.mookBlimp, ActiveTheme.mookTankCannon,
                        SharedObjectsReference.mookMotorBike, SharedObjectsReference.mookMotorBikeNuclear, SharedObjectsReference.mookDumpTruck
                    }.ToGameObjects();
                case DoodadType.Miniboss:
                    return new Unit[] { ActiveTheme.mookMammothTank, ActiveTheme.mookKopterMiniBoss, ActiveTheme.mookDolfLundgren, ActiveTheme.mookKopterMammoth, ActiveTheme.goliathMech }.ToGameObjects();
                case DoodadType.SpawnPoint:
                    return new SpawnPoint[] { Map.Instance.spawnPointPrefabReference.Asset, Map.Instance.spawnPointInvisiblePrefabReference.Asset }.ToGameObjects();
                case DoodadType.CheckPointRunHorizontal:
                    return new CheckPointExplosionRun[] { ActiveTheme.checkPointRunHorizontalPrefab, ActiveTheme.checkPointRunHorizontalFastPrefab }.ToGameObjects();
                case DoodadType.CheckPointRunVertical:
                    return new CheckPointExplosionRun[] { ActiveTheme.checkPointRunVerticalPrefab, ActiveTheme.checkPointRunVerticalFastPrefab }.ToGameObjects();
                case DoodadType.CheckPointRunDescent:
                    return new CheckPointExplosionRun[] { ActiveTheme.checkPointRunDescentPrefab, ActiveTheme.checkPointRunDescentFastPrefab }.ToGameObjects();
                case DoodadType.AlienGiantSandWorm:
                    return new Unit[] { SharedObjectsReference.alienMinibossSandWorm, SharedObjectsReference.alienSandWormFacehuggerSpitter, SharedObjectsReference.alienSandWormFacehuggerSpitterBehind,
                        SharedObjectsReference.alienGiantSandWorm, SharedObjectsReference.alienGiantSandWorm, SharedObjectsReference.alienGiantSandWormBoss
                    }.ToGameObjects();
                case DoodadType.ExpendabrosBoss:
                    return new GameObject[] { ActiveTheme.expendabrosBoss, ActiveTheme.expendabrosBossStation.gameObject, ActiveTheme.expendabrosBossRailHorizontal, ActiveTheme.expendabrosExplosionChase,
                        ActiveTheme.expendabrosBossRailVertical
                    };
                case DoodadType.AmmoCrate:
                    return new CrateBlock[] { ActiveTheme.crateAmmo, SharedObjectsReference.crateTimeCop, SharedObjectsReference.crateRCCar, SharedObjectsReference.crateAirstrike,
                        SharedObjectsReference.crateMechDrop, SharedObjectsReference.crateAlienPheromonesDrop, SharedObjectsReference.crateSteroids, SharedObjectsReference.cratePiggy,
                        SharedObjectsReference.cratePerk, SharedObjectsReference.crateDollars, SharedObjectsReference.crateRevive, SharedObjectsReference.crateDamage
                    }.ToGameObjects();
                case DoodadType.AlienBoss:
                    return new GameObject[] { SharedObjectsReference.alienSlugBoss, SharedObjectsReference.alienHeartBoss };
                case DoodadType.HellBoss:
                    return new GameObject[] { SharedObjectsReference.satanMiniboss.gameObject, SharedObjectsReference.satanMegaBoss };
                case DoodadType.CheckPointAirdrop:
                    return new CheckPoint[] { SharedObjectsReference.checkPointAirdropHighPrefab, SharedObjectsReference.checkPointAirdropLowPrefab }.ToGameObjects();
            }
            return null;
        }
    }
}

