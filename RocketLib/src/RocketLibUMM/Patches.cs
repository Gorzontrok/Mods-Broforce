using HarmonyLib;
using RocketLib.Loggers;
using RocketLib;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RocketLibUMM
{
    // Patches to avoid destroying the mod manager.
    [HarmonyPatch(typeof(PauseMenu), "ReturnToMenu")]
    static class PauseMenu_ReturnToMenu_Patch
    {
        static bool Prefix(PauseMenu __instance)
        {
            if (!Main.enabled)
            {
                return true;
            }

            PauseGameConfirmationPopup m_ConfirmationPopup = __instance.GetFieldValue<PauseGameConfirmationPopup>("m_ConfirmationPopup");

            MethodInfo dynMethod = m_ConfirmationPopup.GetType().GetMethod("ConfirmReturnToMenu", BindingFlags.NonPublic | BindingFlags.Instance);
            dynMethod.Invoke(m_ConfirmationPopup, null);

            return false;
        }

    }
    [HarmonyPatch(typeof(PauseMenu), "ReturnToMap")]
    static class PauseMenu_ReturnToMap_Patch
    {
        static bool Prefix(PauseMenu __instance)
        {
            if (!Main.enabled)
            {
                return true;
            }

            __instance.CloseMenu();
            GameModeController.Instance.ReturnToWorldMap();
            return false;
        }

    }
    [HarmonyPatch(typeof(PauseMenu), "RestartLevel")]
    static class PauseMenu_RestartLevel_Patch
    {
        static bool Prefix(PauseMenu __instance)
        {
            if (!Main.enabled)
            {
                return true;
            }

            Map.ClearSuperCheckpointStatus();

            (Traverse.Create(typeof(TriggerManager)).Field("alreadyTriggeredTriggerOnceTriggers").GetValue() as List<string>).Clear();

            if (GameModeController.publishRun)
            {
                GameModeController.publishRun = false;
                LevelEditorGUI.levelEditorActive = true;
            }
            PauseController.SetPause(PauseStatus.UnPaused);
            GameModeController.RestartLevel();

            return false;
        }
    }

    [HarmonyPatch(typeof(MainMenu))]
    public class MainMenuPatch
    {
        [HarmonyPatch("ProcessCharacter")]
        [HarmonyPostfix]
        private static void CheckCustomPassword(MainMenu __instance)
        {
            foreach (GamePassword password in GamePassword.Passwords)
            {
                try
                {
                    if (__instance.CallMethod<bool>("CheckCheatString", new object[] { password.password }))
                    {
                        Sound sound7 = Sound.GetInstance();
                        sound7.PlaySoundEffect(__instance.drumSounds.specialSounds[0], 0.75f);
                        password.action?.Invoke();
                    }
                }
                catch (Exception ex)
                {
                    ScreenLogger.Instance.ExceptionLog($"Failed to check the password: {password.password}", ex);
                }
            }
        }
    }
}
