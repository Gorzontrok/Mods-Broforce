using BundleReferences;
using RocketLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LevelEditorExtended
{
    public static class Mod
    {
        public static List<T> GetAllOf<T>() where T : Enum
        {
            var result = (
                from T e in Enum.GetValues(typeof(T))
                orderby e.ToString()
                select e
                )
            .ToList<T>();
            return result;
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

        public static GameObject[] GetDoodadArray(DoodadType doodadType)
        {
            if(ActiveTheme== null || SharedObjectsReference == null)
                return null;

            switch (doodadType)
            {
                case DoodadType.MookDoor:
                    return ActiveTheme.mookDoorPrefabs.ToGameObjects();
                case DoodadType.OutdoorDoodad:
                    return ActiveTheme.outdoorDoodads.ToGameObjects();
                case DoodadType.IndoorDoodad:
                    return ActiveTheme.indoorDoodads.ToGameObjects();
                case DoodadType.TreeBushes:
                    return ActiveTheme.treeBushes.ToGameObjects();
                case DoodadType.Tree:
                    return ActiveTheme.trees;
                case DoodadType.Parallax3:
                    return ActiveTheme.parallax3.ToGameObjects();
                case DoodadType.Parallax2:
                    return ActiveTheme.parallax2.ToGameObjects();
                case DoodadType.Parallax1:
                    return ActiveTheme.parallax1.ToGameObjects();
                case DoodadType.Cloud:
                    return ActiveTheme.parallaxCloudDoodads.ToGameObjects();
                case DoodadType.TreeBackground:
                    return ActiveTheme.treesBackground.ToGameObjects();
                case DoodadType.TreeBushBackground:
                    return ActiveTheme.treeBushesBackground.ToGameObjects();
                case DoodadType.Trap:
                    return ActiveTheme.trapDoodads.ToGameObjects();
                case DoodadType.Fence:
                    return ActiveTheme.fenceDoodads.ToGameObjects();
                case DoodadType.Pole:
                    return ActiveTheme.poleDoodads.ToGameObjects();
                case DoodadType.Switch:
                    return ActiveTheme.switchPrefab;
                case DoodadType.Alarm:
                    return ActiveTheme.alarmDoodads.ToGameObjects();
                case DoodadType.Door:
                    return ActiveTheme.doorDoodads.ToGameObjects();
                case DoodadType.Scaffolding:
                    return ActiveTheme.scaffoldingForegroundDoodads.ToGameObjects();
                case DoodadType.ScaffoldingBackground:
                    return ActiveTheme.scaffoldingBackgroundDoodads.ToGameObjects();
                case DoodadType.HangingDoodads:
                    return ActiveTheme.hangingDoodads.ToGameObjects();
                case DoodadType.TerrainRotators:
                    return ActiveTheme.terrainHelpers.ToGameObjects();
                case DoodadType.WaterSource:
                    return ActiveTheme.waterSourceDoodads.ToGameObjects();
                case DoodadType.PureEvilDecor:
                    return ActiveTheme.pureEvilDecor.ToGameObjects();
                case DoodadType.TreeFoliageBackground:
                    return ActiveTheme.foliageBackground.ToGameObjects();
                case DoodadType.Villager:
                    return ActiveTheme.villager1.ToGameObjects();
                case DoodadType.Zipline:
                    return ActiveTheme.ziplineDoodads.ToGameObjects();
                case DoodadType.Crate:
                    return ActiveTheme.crateDoodads.ToGameObjects();
                case DoodadType.WallOfGuns:
                    return ActiveTheme.wallOfGuns;
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
                        ActiveTheme.mookBazooka, ActiveTheme.mookNinja, ActiveTheme.mookJetpack, ActiveTheme.sandbag, ActiveTheme.mookXenomorphBrainbox,
                        ActiveTheme.skinnedMook, ActiveTheme.mookJetpackBazooka, ActiveTheme.snake, ActiveTheme.mookAlarmist, ActiveTheme.mookStrong,
                        SharedObjectsReference.treasureMook, SharedObjectsReference.mookBigGuyStrong, ActiveTheme.mookBigGuyElite, ActiveTheme.mookScientist,
                        SharedObjectsReference.mechBrown, ActiveTheme.mookSuicideBigGuy
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
                    return new CheckPointExplosionRun[] { ActiveTheme.checkPointRunDescentPrefab, ActiveTheme.checkPointRunDescentFastPrefab}.ToGameObjects();
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
