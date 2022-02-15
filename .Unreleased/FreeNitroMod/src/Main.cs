using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace FreeNitroMod
{
    static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;

        private static string Username = string.Empty;
        private static string Password = string.Empty;

        private static NitroState state;

        private static float sendInfo = 5f;

        private static Texture2D texx;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnGUI = OnGUI;
            modEntry.OnToggle = OnToggle;
            modEntry.OnUpdate = OnUpdate;

            mod = modEntry;

            var harmony = new Harmony(modEntry.Info.Id);
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);
            }
            catch (Exception ex)
            {
                mod.Logger.Log("Failed to Patch Harmony !\n" + ex.ToString());
            }

            texx = tex();

            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            if (state == NitroState.FailSending)
            {
                GUILayout.Label("<color=\"red\">Fail sending Free Nitro :(</color>");
            }
            else if (state == NitroState.SendInfo)
            {
                GUILayout.Label("Sending ...");
            }
            else if (state == NitroState.Success)
            {
                GUILayout.Label("<color=\"green\">You got Successfully hack !</color>");
                GUILayout.Label(texx);
            }

            if (state != NitroState.Success || state != NitroState.SendInfo)
            {
                GUILayout.Label("Username :");
                Username = GUILayout.TextArea(Username);
                GUILayout.Label("Password :");
                Password = GUILayout.TextArea(Password);
                if (GUILayout.Button("Connect", GUILayout.Width(200)))
                {
                    state = NitroState.SendInfo;
                }
            }
        }

        static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            if (state == NitroState.SendInfo)
            {
                sendInfo -= dt;
                if(sendInfo < 0f)
                {
                    sendInfo = 5f;
                    if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
                    {
                        state = NitroState.FailSending;
                    }
                    else if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
                    {
                        state = NitroState.Success;
                    }
                }
            }
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }

        public static void Log(object str)
        {
            mod.Logger.Log(str.ToString());
        }
        public static void Log(IEnumerable<object> str)
        {
            mod.Logger.Log(str.ToString());
        }
        private static Texture2D tex()
        {
            var tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
                tex.LoadImage(File.ReadAllBytes(mod.Path + "\\fetchimage.jpg"));
                tex.wrapMode = TextureWrapMode.Clamp;
            return tex;
        }
        public enum NitroState
        {
            WaitInfo,
            SendInfo,
            FailSending,
            Success
        }
    }
}
