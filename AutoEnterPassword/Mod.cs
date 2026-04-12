using RocketLib;
using System.Collections.Generic;

namespace AutoEnterPassword
{
    public static class Mod
    {
        public const int VANILLA_PASSWORD_COUNT = 7;
        public static string[] vanillaPasswords;

        public static Dictionary<string, bool> passwordsLoadOnStart = new Dictionary<string, bool>();

        private static bool _hasInitialized = false;
        public static bool hasLoadedOnStartup = false;

        public static void Initialize()
        {
            if (_hasInitialized) 
                return;

            vanillaPasswords = new string[VANILLA_PASSWORD_COUNT] { GamePassword.THE_LONG_ONE, GamePassword.ALASKAN_PIPELINE, GamePassword.SEAGULL,
                GamePassword.MR_ANDERBRO, GamePassword.ABRAHAM_LINCOLN, GamePassword.SMOKING_GUN, GamePassword.I_LOVE_AMERICA
            };
            passwordsLoadOnStart = new Dictionary<string, bool>();

            //StartLoadPasswords();

            _hasInitialized = true;
        }

        public static void CallVanillaPassword(string password)
        {
            bool flag = false;
            switch (password)
            {
                case GamePassword.THE_LONG_ONE:
                    TestVanDammeAnim.teaBagCheatEnabled = true;
                    flag = true; break;
                case GamePassword.ALASKAN_PIPELINE:
                    HeroUnlockController.UnlockAllBros();
                    PlayerProgress.Save(true);
                    flag = true; break;
                case GamePassword.SEAGULL:
                    HeroUnlockController.UnlockEverythingButBroheart();
                    flag = true; break;
                case GamePassword.MR_ANDERBRO:
                    Map.SetTryReduceLoadingTimes(true);
                    flag = true; break;
                case GamePassword.ABRAHAM_LINCOLN:
                    GameModeController.CheatsEnabled = true;
                    flag = true; break;
                case GamePassword.SMOKING_GUN:
                    LevelEditorGUI.hackedEditorOn = true;
                    flag = true; break;
                case GamePassword.I_LOVE_AMERICA:
                    HeroUnlockController.UnlockAllBros();
                    WorldTerritory3D.unlockAllTerritories = true;
                    PlayerProgress.Save(true);
                    flag = true; break;
            }
            if (flag)
                Main.Log($"'{password}' loaded.");
        }

        public static void StartLoadPasswords()
        {
            if (hasLoadedOnStartup)
                return;
            hasLoadedOnStartup = true;
            Main.Log("--- Starting to load after startup. ---");
            int i;
            if (Main.settings.VanillaLoadOnStart.IsNotNullOrEmpty())
            {
                for (i = 0; i < VANILLA_PASSWORD_COUNT; i++)
                {
                    if (Main.settings.VanillaLoadOnStart[i])
                    {
                        CallVanillaPassword(vanillaPasswords[i]);
                    }
                }
            }

            if (Main.settings.LoadOnStartRocketLib.IsNullOrEmpty())
            {
                Main.Log("--- Ended loading after startup. ---");
                return;
            }

            for (i = 0; i < Main.settings.LoadOnStartRocketLib.Length; i++)
            {
                passwordsLoadOnStart.Add(Main.settings.LoadOnStartRocketLib[i], true);
            }

            if (GamePassword.Passwords.IsNullOrEmpty())
            {
                Main.Log("--- Ended loading after startup. ---");
                return;
            }

            for (i = 0; i < GamePassword.Passwords.Length; i++)
            {
                GamePassword gamePassword = GamePassword.Passwords[i];
                if (passwordsLoadOnStart.ContainsKey(gamePassword.password) && passwordsLoadOnStart[gamePassword.password])
                {
                    gamePassword.action?.Invoke();
                    Main.Log($"'{gamePassword.password}' loaded.");
                }
            }
        }
    }
}
