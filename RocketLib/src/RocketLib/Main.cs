using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using UnityModManagerNet;
using HarmonyLib;
using RocketLib.Loggers;

namespace RocketLib
{
    public static class Main
    {
        public const string NEWTONSOFT_ASSEMBLY_NAME = "Newtonsoft.Json.dll";

        /// <summary>
        /// Is RocketLib is Loaded
        /// </summary>
        public static bool Loaded { get; private set; } = false;

        public static UnityModManager.ModEntry mod;
        internal static Harmony harmony;

        public static float logTimer = 3f;
        public static bool showLogOnScreen = true;
        public static bool showManagerLog = true;

        public static void Load(UnityModManager.ModEntry _mod)
        {
            try
            {
                if (!Loaded)
                {
                    mod = _mod;

                    harmony = new Harmony("RocketLib-NotUMM");
                    try
                    {
                        var assembly = Assembly.GetExecutingAssembly();
                        harmony.PatchAll(assembly);
                    }
                    catch (Exception ex)
                    {
                        mod.Logger.Log("Failed to Patch Harmony :\n" + ex);
                    }


                    // Load Newtonsoft
                    try
                    {
                        Assembly.LoadFile(Path.Combine(mod.Path, NEWTONSOFT_ASSEMBLY_NAME));
                    }
                    catch (Exception ex)
                    {
                        mod.Logger.Log("Error while loading Newtonsoft.Json" + ex);
                    }
                    KeyBindingForPlayers.keyBindingForPlayers = new Dictionary<string, List<KeyBindingForPlayers>>();
                    Loaded = true;
                }
                else
                {
                    mod.Logger.Log("Cancel Load, already Started ");
                }
            }
            catch(Exception ex)
            {
                mod.Logger.Log(ex.ToString());
            }
        }

        internal static void Log(object msg)
        {
            mod.Logger.Log(msg.ToString());
        }

        public static bool TestBuild
        {
            get
            {
                return Environment.UserName == "Gorzon";
            }
        }
    }
}

