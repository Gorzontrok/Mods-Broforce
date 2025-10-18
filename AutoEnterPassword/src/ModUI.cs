using RocketLib;
using UnityEngine;
using UnityModManagerNet;

namespace AutoEnterPassword
{
    internal static class ModUI
    {
        public static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.Label("Game Passwords :");
            GUILayout.BeginHorizontal();
            for (int i = 0; i < Mod.VANILLA_PASSWORD_COUNT; i++)
            {
                DrawVanillaPasswordUI(i);
            }
            GUILayout.EndHorizontal();

            if (GamePassword.Passwords.IsNullOrEmpty())
                return;

            GUILayout.Label("RocketLib Passwords :");
            for (int i = 0; i < GamePassword.Passwords.Length; i++)
            {
                if (i == 0)
                {
                    GUILayout.BeginHorizontal();
                }

                DrawRocketLibPasswordUI(GamePassword.Passwords[i]);

                if (i % 5 == 0 || i == GamePassword.Passwords.Length)
                {
                    GUILayout.EndHorizontal();
                }
            }
        }

        public static void DrawVanillaPasswordUI(int i)
        {
            if (i >= Mod.VANILLA_PASSWORD_COUNT)
                return;
            string password = Mod.vanillaPasswords[i];
            GUILayout.BeginVertical();
            if (GUILayout.Button(password))
            {
                Mod.CallVanillaPassword(password);
            }
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            Main.settings.VanillaLoadOnStart[i] = GUILayout.Toggle(Main.settings.VanillaLoadOnStart[i], "Load On Start");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        public static void DrawRocketLibPasswordUI(GamePassword gamePassword)
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button(gamePassword.password))
            {
                gamePassword.action.Invoke();
            }
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (!Mod.passwordsLoadOnStart.ContainsKey(gamePassword.password))
                Mod.passwordsLoadOnStart.Add(gamePassword.password, false);
            Mod.passwordsLoadOnStart[gamePassword.password] = GUILayout.Toggle(Mod.passwordsLoadOnStart[gamePassword.password], "Load On Start");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
    }
}
