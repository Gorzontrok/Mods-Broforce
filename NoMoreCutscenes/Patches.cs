using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NoMoreCutscenes.Patches
{
    [HarmonyPatch(typeof(CutsceneIntroRoot), "ContainsCutscene")]
    static class CutsceneOntroRoot_ContainsCutscene_Patch
    {
        static bool Prefix(CutsceneIntroRoot __instance, ref bool __result)
        {
            if(Main.CanUsePatch)
            {
                CutsceneController.holdPlayersStill = false;
                __result = false;
                return false;
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(CameraMoveAction), "Start")]
    static class CameraMoveAction_Start_Patch
    {
        static void Prefix(CameraMoveAction __instance)
        {
            Traverse t = Traverse.Create(__instance);
            CameraActionInfo cai = t.Field("info").GetValue<CameraActionInfo>();
            //cai.time = 0f;
            cai.holdPlayersInCutscene = false;
            t.Field("info").SetValue(cai);

        }
    }

    [HarmonyPatch(typeof(LevelEventAction), "Start")]
    static class LevelEventAction_Start_Patch
    {
        static bool Prefix(LevelEventAction __instance)
        {
            if (Main.CanUsePatch)
            {
                LevelEventActionInfo info = (LevelEventActionInfo)__instance.Info;
                __instance.state = TriggerActionState.Done;
                switch (info.levelActionType)
                {
                    case LevelEventActionType.LevelEndSuccess:
                        GameModeController.LevelFinished = false;
                        GameModeController.LevelFinish(LevelResult.Success);
                        break;
                    case LevelEventActionType.LevelEndFail:
                        GameModeController.LevelFinished = false;
                        GameModeController.LevelFinish(LevelResult.ForcedFail);
                        break;
                    case LevelEventActionType.Cutscene:
                       // CutsceneController.LoadCutScene(info.cutscene, 0f);
                        break;
                    case LevelEventActionType.SwitchToBossMusic:
                        Sound.GetInstance().PlayBossFightMusic((BossMusicType)info.integerValue1);
                        break;
                    case LevelEventActionType.ActivateTrigger:
                        Traverse.Create(typeof(TriggerManager)).Method("ActivateTrigger", new object[] { info.triggerName }).GetValue();
                        break;
                    case LevelEventActionType.DeactivateTrigger:
                        Traverse.Create(typeof(TriggerManager)).Method("DeactivateTrigger", new object[] { info.triggerName }).GetValue();
                        break;
                    case LevelEventActionType.GoToLevelSilent:
                        GameState.Instance.ClearSuperCheckPointStatus();
                        Map.ClearSuperCheckpointStatus();
                        if (info.transitionInstantly)
                        {
                            LevelSelectionController.CurrentLevelNum = info.cutsceneNumber;
                            Fader.nextScene = LevelSelectionController.CampaignScene;
                            Brotalitometer.Reset();
                            Fader.FadeSolid(2f, true);
                        }
                        else
                        {
                            LevelSelectionController.CurrentLevelNum = info.cutsceneNumber - 1;
                        }
                        break;
                    case LevelEventActionType.LevelEndSuccessSilent:
                        GameState.Instance.ClearSuperCheckPointStatus();
                        Map.ClearSuperCheckpointStatus();
                        GameModeController.LevelFinished = false;
                        GameModeController.Instance.switchSilently = true;
                        GameModeController.LevelFinish(LevelResult.Success);
                        if (info.transitionInstantly)
                        {
                            GameModeController.MakeFinishInstant();
                        }
                        else
                        {
                            Fader.FadeSolid(2f, true);
                        }
                        break;
                    case LevelEventActionType.CallHelicopter:
                        Map.newestHelicopter.Enter(Map.GetBlockCenter(new GridPoint(info.pos.CollumnAdjusted, info.pos.RowAdjusted)), 0f);
                        break;
                    case LevelEventActionType.TimeSlowdown:
                        TimeController.StopTime(info.value2, info.value1, 1f, true, false, true);
                        break;
                    case LevelEventActionType.ShowHealthBar:
                        {
                            GameObject doodadByTag = Map.GetDoodadByTag(info.triggerName);
                            BroforceObject component = doodadByTag.GetComponent<BroforceObject>();
                            if (component != null)
                            {
                                HealthBar.Instance.SetUnit(component);
                            }
                            else
                            {
                                BossSlugMover component2 = doodadByTag.GetComponent<BossSlugMover>();
                                if (component2 != null)
                                {
                                    HealthBar.Instance.SetUnit(component2.headPiece);
                                }
                                else
                                {
                                    LevelTitle.ShowText("COULD NOT FIND UNIT FOR HEALTH BAR", 0f, false);
                                }
                            }
                            break;
                        }
                    case LevelEventActionType.FadeInMusic:
                        if (info.fadeInMusic)
                        {
                            Sound.GetInstance().FadeInLevelMusic();
                        }
                        else
                        {
                            Sound.GetInstance().FadeInMusic();
                        }
                        break;
                    case LevelEventActionType.FadeOutMusic:
                        Sound.GetInstance().FadeOutMusic();
                        break;
                    case LevelEventActionType.TextDisplaySubtitle:
                        LevelTitle.ShowText(Traverse.Create(__instance).Method("FormatString", new object[] { info.triggerName, info.displayDecimals, false }).GetValue<string>(), 0f, false);
                        break;
                    case LevelEventActionType.TextDisplayCenter:
                        InfoBar.Appear(1f, Traverse.Create(__instance).Method("FormatString", new object[] { info.triggerName, info.displayDecimals, false }).GetValue<string>(), Color.black, 1f);
                        break;
                    case LevelEventActionType.StopTriggerAction:
                        Traverse.Create(typeof(TriggerManager)).Method("StopTriggerAction", new object[] { info.triggerName }).GetValue();
                        break;
                    case LevelEventActionType.TextDisplayContinuous:
                        __instance.state = TriggerActionState.Busy;
                        break;
                    case LevelEventActionType.SwitchToOminousMusic:
                        Sound.GetInstance().PlayBossPulse((BossPulseType)info.integerValue1, info.fadeInMusic);
                        break;
                    case LevelEventActionType.ChangeImageEffectsIntensity:
                        ColorShiftController.SetBloomEffectIntensity(info.value1, info.value2);
                        break;
                    case LevelEventActionType.CallHellPortal:
                        Map.CreateExitPortal(Map.GetBlockCenter(new GridPoint(info.pos.CollumnAdjusted, info.pos.RowAdjusted)));
                        break;
                    case LevelEventActionType.CallFakeHelicopter:
                        Map.CallFakeHelicopter(Map.GetBlockCenter(new GridPoint(info.pos.CollumnAdjusted, info.pos.RowAdjusted)), 0f);
                        break;
                    case LevelEventActionType.CallLaserBeam:
                        {
                            Vector2 b = Map.GetBlockCenter(new GridPoint(info.pos.CollumnAdjusted, info.pos.RowAdjusted));
                            Vector2 a = Map.GetBlockCenter(new GridPoint(info.pos2.CollumnAdjusted, info.pos2.RowAdjusted));
                            if (info.targetHelicopter && Map.helicopterFake != null)
                            {
                                ProjectileController.SpawnLaserBeamOverNetwork(Map.Instance.sharedObjectsReference.Asset.laserBeamPrefab, null, b.x, b.y, 0f, 1f, Map.helicopterFake.transform, true, -15, false);
                            }
                            else if (info.targetHelicopter && Map.newestHelicopter != null)
                            {
                                ProjectileController.SpawnLaserBeamOverNetwork(Map.Instance.sharedObjectsReference.Asset.laserBeamPrefab, null, b.x, b.y, 0f, 1f, Map.newestHelicopter.transform, true, -15, false);
                            }
                            else if (!info.targetHelicopter)
                            {
                                Vector3 vector = a - b;
                                float angle = global::Math.GetAngle(vector.x, vector.y);
                                ProjectileController.SpawnLaserBeamLocally(Map.Instance.sharedObjectsReference.Asset.laserBeamPrefab, null, b.x, b.y, 0f, angle, 1f, 0f, -15);
                            }
                            break;
                        }
                    case LevelEventActionType.SetCameraZoom:
                        SortOfFollow.ForceSlowSnapBackZoom(info.value2, info.value1);
                        break;
                    case LevelEventActionType.DamageUnit:
                        if (!Map.DamageDoodadByTag(info.triggerName, info.integerValue1))
                        {
                            LevelTitle.ShowText("COULD NOT FIND UNIT TO DAMAGE", 0f, false);
                        }
                        break;
                    case LevelEventActionType.StartFireworks:
                        if (EffectsController.instance != null)
                        {
                            EffectsController.instance.forceFireworksOn = true;
                        }
                        break;
                    case LevelEventActionType.TeleportBros:
                        for (int i = 0; i < 4; i++)
                        {
                            if (HeroController.IsPlaying(i) && HeroController.players[i] != null && HeroController.players[i].character != null)
                            {
                                TestVanDammeAnim character = HeroController.players[i].character;
                                character.SetXY(Map.GetBlocksX(info.pos.collumn + i), Map.GetBlocksY(info.pos.row));
                                character.ClearAllInput();
                                if (character is BoondockBro)
                                {
                                    Debug.LogError("Boondocks Bros Teleport");
                                    BoondockBro boondockBro = (BoondockBro)character;
                                    if (boondockBro.trailingBro)
                                    {
                                        boondockBro.trailingBro.SetXY(Map.GetBlocksX(info.pos.collumn + i), Map.GetBlocksY(info.pos.row));
                                        boondockBro.trailingBro.ClearAllInput();
                                        boondockBro.trailingBro.ClearInputs();
                                    }
                                    if (boondockBro.connollyBro)
                                    {
                                        boondockBro.connollyBro.SetXY(Map.GetBlocksX(info.pos.collumn + i), Map.GetBlocksY(info.pos.row));
                                        boondockBro.connollyBro.ClearAllInput();
                                        boondockBro.connollyBro.ClearInputs();
                                    }
                                }
                                SortOfFollow.ForcePosition(Map.GetBlockCenter(info.pos));
                            }
                        }
                        break;
                    case LevelEventActionType.FadeFromBlack:
                        Fader.FadeFromBlack(info.value1);
                        break;
                    case LevelEventActionType.FadeToBlack:
                        Fader.FadeToBlack(info.value1);
                        break;
                    case LevelEventActionType.SilentlyFinishWorldMapCampaign:
                        WorldMapProgressController.FinishCampaign();
                        break;
                    case LevelEventActionType.RestartLevelOnHardcoreMode:
                        Map.MapData.restartOnDeathInHardcore = info.transitionInstantly;
                        break;
                    case LevelEventActionType.SatanLaugh:
                        Announcer.SatanLaugh(0.8f, 1f, 0f);
                        break;
                    case LevelEventActionType.CollapseToSteel:
                        Map.GetBlock(info.pos.CollumnAdjusted, info.pos.RowAdjusted).replaceOnCollapse = true;
                        Map.GetBlock(info.pos.CollumnAdjusted, info.pos.RowAdjusted).replacementBlockType = GroundType.Steel;
                        break;
                    case LevelEventActionType.ReleaseCamera:
                        SortOfFollow.ReturnToNormal(info.value1);
                        break;
                    case LevelEventActionType.RetreatHelicopter:
                        Map.newestHelicopter.Leave();
                        break;
                    case LevelEventActionType.TeleportHelicopter:
                        if (info.targetHelicopter || Map.helicopterFake == null)
                        {
                            Map.newestHelicopter.Teleport(new GridPoint(info.pos.CollumnAdjusted, info.pos.RowAdjusted));
                        }
                        else
                        {
                            Map.helicopterFake.Teleport(new GridPoint(info.pos.CollumnAdjusted, info.pos.RowAdjusted));
                        }
                        break;
                    case LevelEventActionType.CameraShake:
                        SortOfFollow.Shake(info.value1, info.value2);
                        break;
                }
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(CutsceneController), "GetCustsceneSceneName")]
    static class CutsceneController_GetCustsceneSceneName_Patch
    {
        static bool Prefix(CutsceneController __instance, ref string __result, CutsceneName cutscene)
        {
            if (Main.CanUsePatch)
            {
                __result = string.Empty;
                CutsceneController.holdPlayersStill = false;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(Player), "SpawnHero")]
    static class Player_SpawnHero_Patch
    {
        static bool Prefix(Player __instance, HeroType nextHeroType)
        {
            if(Main.CanUsePatch)
            {
                if (__instance.character != null && __instance.character.IsAlive())
                {
                    Networking.Networking.RPC(PID.TargetAll, new RpcSignature(__instance.character.RecallBro), false);
                }
                if (__instance.IsMine && PlayerProgress.Instance.yetToBePlayedUnlockedHeroes.Contains(nextHeroType))
                {
                    PlayerProgress.Instance.yetToBePlayedUnlockedHeroes.Remove(nextHeroType);
                    //CutsceneController.LoadCutScene((CutsceneName)nextHeroType, 0.2f);
                }
                if (HeroUnlockController.IsAvailableInCampaign(nextHeroType) && !PlayerProgress.Instance.unlockedHeroes.Contains(nextHeroType) && GameModeController.GameMode != GameMode.DeathMatch)
                {
                    PlayerProgress.Instance.unlockedHeroes.Add(nextHeroType);
                }
                Traverse.Create(__instance).Method("InstantiateHero", new object[] { nextHeroType, __instance.playerNum, __instance.controllerNum }).GetValue<TestVanDammeAnim>();
                CutsceneController.holdPlayersStill = false; 
                return false;
            }
            return true;
        }
    }
}
