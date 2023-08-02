using RocketLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AutoEnterPassword
{
    public static class Mod
    {
        public const string THE_LONG_ONE = "IThinkPuttingMyTesticalsInSomeoneElseFaceWithoutTheirConsentIsOkay";
        public const string ALASKAN_PIPELINE = "alaskanpipeline";
        public const string SEAGULL = "seagull";
        public const string MR_ANDERBRO = "mranderbro";
        public const string ABRAHAM_LINCOLN = "abrahamlincoln";
        public const string SMOKING_GUN = "smokinggun";
        public const string I_LOVE_AMERICA = "iloveamerica";

        internal static List<string> autoLoadSession = new List<string>();
        internal static List<Password> passwords = new List<Password>();
        internal static VanillaPassword[] vanillaPasswords;

        private static bool _hasInitialized = false;

        public static void Initialize()
        {
            if (_hasInitialized) return;

            CreateVanillaPassword();
            GetRocketLibPasswords();
            AutoLoad();
        }

        public static void GamePasswordUI(Password password)
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button(password.GetName()))
            {
                password.DoAction();
            }
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            password.autoLoad = GUILayout.Toggle(password.autoLoad, "AutoLoad");
            if (password.autoLoad)
                AddPasswordToAutoLoad(password.GetName());
            else
                RemovePasswordFromAutoLoad(password.GetName());
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
        private static void AddPasswordToAutoLoad(string password)
        {
            if (!autoLoadSession.Contains(password))
                autoLoadSession.Add(password);
        }
        private static void RemovePasswordFromAutoLoad(string password)
        {
            if (autoLoadSession.Contains(password))
                autoLoadSession.Remove(password);
        }
        private static void CreateVanillaPassword()
        {
            var temp = new List<VanillaPassword>();
            temp.Add(new VanillaPassword(THE_LONG_ONE));
            temp.Add(new VanillaPassword(ALASKAN_PIPELINE));
            temp.Add(new VanillaPassword(SEAGULL));
            temp.Add(new VanillaPassword(MR_ANDERBRO));
            temp.Add(new VanillaPassword(ABRAHAM_LINCOLN));
            temp.Add(new VanillaPassword(SMOKING_GUN));
            temp.Add(new VanillaPassword(I_LOVE_AMERICA));
            vanillaPasswords = temp.ToArray();
        }

        private static void GetRocketLibPasswords()
        {
            foreach (var password in GamePasswordController.GamePasswords)
            {
                passwords.Add(new Password(password));
            }
        }

        private static void AutoLoad()
        {
            autoLoadSession = Main.settings.autoLoad.ToList();
            List<Password> passwordList = new List<Password>(passwords);
            passwordList.AddRange(vanillaPasswords);

            foreach (var password in passwordList)
            {
                if (autoLoadSession.Contains(password.GetName()))
                {
                    password.DoAction();
                }
            }
        }
    }
}
