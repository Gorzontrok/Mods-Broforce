using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelEditorExtended.Patches
{
    [HarmonyPatch(typeof(LevelEditorGUI), "Start")]
    static class Start_Patch
    {
        static LevelEditorGUI inst;
        static bool hadError = false;
        static bool Prefix(LevelEditorGUI __instance)
        {
            if (!Main.enabled) return true;
            try
            {
                inst = __instance;
                ShowMouseController.SetCursorToArrow(true);
                Indicators();

                // Types
                inst.SetFieldValue("terrainTypes", Mod.GetAllOf<TerrainType>());
                inst.SetFieldValue("doodadTypes", Mod.GetAllOf<DoodadType>());
                inst.SetFieldValue("gameplayTags", Mod.GetAllOf<GameplayWorkshopTag>());
                inst.SetFieldValue("triggerTypes", Mod.GetAllOf<TriggerType>());
                inst.SetFieldValue("actionTypes", Mod.GetAllOf<TriggerActionType>());
                inst.SetFieldValue("characterCommandTypes", Mod.GetAllOf<CharacterCommandType>());
                inst.SetFieldValue("enemyActionTypes", Mod.GetAllOf<EnemyActionType>());
                inst.SetFieldValue("gameModes", Mod.GetAllOf<GameMode>());
                inst.SetFieldValue("levelThemes", Mod.GetAllOf<LevelTheme>());
                inst.SetFieldValue("heroSpawnModes", Mod.GetAllOf<HeroSpawnMode>());
                inst.SetFieldValue("musicTypes", Mod.GetAllOf<MusicType>());
                inst.SetFieldValue("weatherTypes", Mod.GetAllOf<WeatherType>());
                inst.SetFieldValue("ambienceTypes", Mod.GetAllOf<AmbienceType>());
                inst.SetFieldValue("flexPowersTypes", Mod.GetAllOf<FlexPowerMapType>());
                inst.SetFieldValue("flexPowersTypesHidden",new List<FlexPowerMapType>() { FlexPowerMapType.Default, FlexPowerMapType.Other });
                // Cameras
                var cameras = Mod.GetAllOf<CameraFollowMode>();
                inst.SetFieldValue("cameraFollowModes", new List<CameraFollowMode>() { CameraFollowMode.ForcedHorizontal, CameraFollowMode.Horizontal, CameraFollowMode.ForcedVertical, CameraFollowMode.Vertical});
                inst.SetFieldValue("deathmatchCameraModes", new List<CameraFollowMode>() { CameraFollowMode.Normal, CameraFollowMode.SingleScreen, CameraFollowMode.Horizontal, CameraFollowMode.Vertical});
                inst.SetFieldValue("raceCameraFollowModes", new List<CameraFollowMode>() { CameraFollowMode.Horizontal, CameraFollowMode.Vertical});
                inst.SetFieldValue("cameraFollowModesCampaign", new List<CameraFollowMode>() { CameraFollowMode.Normal, CameraFollowMode.MapExtents }) ;

                // Hero Types
                var heroTypes = Mod.GetAllOf<HeroType>();
                heroTypes.Remove(HeroType.None);
                heroTypes.Remove(HeroType.Final);
                heroTypes.Remove(HeroType.Random);
                heroTypes.Remove(HeroType.MadMaxBrotansky);
                inst.SetFieldValue("heroTypes", heroTypes);
                LevelEditorGUI.broChangeHeroTypes = new List<HeroType>(heroTypes);
                LevelEditorGUI.broChangeHeroTypes.Insert(0, HeroType.None);


                DoodadFilter();
                inst.SetFieldValue("mouseInfo", GameObjectUtils.FindOrCreateGameObject<LevelEditorMouseInfo>());
                inst.CallMethod("RefreshFiles");
                inst.SetFieldValue("mapData", Map.MapData);
                inst.fileName = PlayerOptions.Instance.LastCustomLevel;
                Map.isEditing = true;
                if (Map.MapData != null)
                {
                    inst.SetFieldValue("newMapWidth", Map.MapData.Width);
                    inst.SetFieldValue("newMapHeight", Map.MapData.Height);
                }

                inst.SetFieldValue("gridClicked", new bool[inst.GetInt("newMapWidth"), inst.GetInt("newMapHeight")]);
                if (inst.CallMethod<bool>("CanEditChunks") && GameState.Instance.loadMode != MapLoadMode.Generated)
                {
                    inst.GetTraverse().Field("levelChunkEditor").CallMethod("Initialise", inst.GetFieldValue<LevelEditorMouseInfo>("mouseInfo"));
                }
                return hadError;
            }
            catch(Exception e)
            {
                Main.Log(e);
            }
            return true;
        }
       /* static void Postfix()
        {
            if (!Main.enabled) return;

            if (!LevelEditorGUI.hasShownDisclaimer && inst.fileName.IsNotNullOrEmpty())
            {
                LevelEditorGUI.hasShownDisclaimer = true;
                inst.LoadCampaign();
            }
        }*/

        static void Indicators()
        {
            try
            {
                var targetSelectionIndicator = inst.GetFieldValue<SelectionIndicator>("targetSelectionIndicator");
                targetSelectionIndicator = UnityEngine.Object.Instantiate<SelectionIndicator>(inst.SelectionIndicatorPrefab);
                targetSelectionIndicator.enabled = false;
                targetSelectionIndicator.name = "TargetSelectionIndicator";
                inst.SetFieldValue("targetSelectionIndicator", targetSelectionIndicator);

                var targetSelectionIndicatorOthers = new SelectionIndicator[16];
                for (int i = 0; i < targetSelectionIndicatorOthers.Length; i++)
                {
                    targetSelectionIndicatorOthers[i] = UnityEngine.Object.Instantiate<SelectionIndicator>(inst.SelectionIndicatorPrefab);
                    targetSelectionIndicatorOthers[i].enabled = false;
                }
                inst.SetFieldValue("targetSelectionIndicatorOthers", targetSelectionIndicatorOthers);

                if (inst.LevelBoundsIndicatorPrefab != null)
                {
                    var levelBoundsIndicator = UnityEngine.Object.Instantiate<SpriteSelectionIndicator>(inst.LevelBoundsIndicatorPrefab);
                    if (levelBoundsIndicator != null)
                    {
                        levelBoundsIndicator.gameObject.SetActive(false);
                    }
                    inst.SetFieldValue("levelBoundsIndicator", levelBoundsIndicator);
                }

                var activeSelectionIndicator = UnityEngine.Object.Instantiate<SelectionIndicator>(inst.SelectionIndicatorPrefab);
                activeSelectionIndicator.enabled = false;
                inst.SetFieldValue("activeSelectionIndicator", activeSelectionIndicator);

                var mousePosSelectionIndicator = UnityEngine.Object.Instantiate<SelectionIndicator>(inst.SingleSelectionIndicatorPrefab);
                mousePosSelectionIndicator.name = "Mouse Selection Indicator";
                inst.SetFieldValue("mousePosSelectionIndicator", mousePosSelectionIndicator);
            }
            catch (Exception e)
            {
                Main.Log(e);
                hadError = true;
            }
        }
        static void DoodadFilter()
        {
            try
            {
                var doodadFilter = new Dictionary<DoodadType, bool>();
                foreach (DoodadType key in inst.GetFieldValue<List<DoodadType>>("doodadTypes"))
                {
                    doodadFilter.Add(key, false);
                }
                inst.SetFieldValue("doodadFilter", doodadFilter);
            }
            catch (Exception e)
            {
                hadError = true;
                Main.Log(e);
            }
        }
    }
}
