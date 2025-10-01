using System;
using HarmonyLib;
using RocketLib;
using RocketLib.Utils;

namespace RocketLibUMM
{
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
                    Main.logger.Exception($"Failed to check the password: {password.password}", ex);
                }
            }
        }
    }

    // Fix UMM window disappearing
    [HarmonyPatch(typeof(UnityModManagerNet.UnityModManager.UI), "Start")]
    static class UnityModManager_UI_Start_Patch
    {
        public static void Postfix(UnityModManagerNet.UnityModManager.UI __instance)
        {
            RocketLibUtils.MakeObjectUnpausable(__instance.gameObject);
        }
    }

    // Fix RuntimeUnityEditor window disappearing
    [HarmonyPatch(typeof(Startup), "Update")]
    static class Startup_Update_Patch
    {
        public static void Prefix(Startup __instance)
        {
            if (!Main.enabled)
            {
                return;
            }

            RocketLibUtils.MakeObjectUnpausable("RuntimeUnityEditor");
        }
    }
}
