using System;
using System.Reflection;
using System.IO;
using UnityModManagerNet;
using HarmonyLib;

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

        internal static ILogger logger;

        public static void Load(UnityModManager.ModEntry _mod)
        {
            try
            {
                if (Loaded)
                {
                    logger.Log("Cancel Load, already Started ");
                    return;
                }

                mod = _mod;

                harmony = new Harmony("RocketLib-NotUMM");
                try
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    harmony.PatchAll(assembly);
                }
                catch (Exception ex)
                {
                    logger.Exception("Failed to Patch Harmony :\n", ex);
                }


                // Load Newtonsoft
                try
                {
                    Assembly.LoadFile(Path.Combine(mod.Path, NEWTONSOFT_ASSEMBLY_NAME));
                }
                catch (Exception ex)
                {
                    logger.Exception("Error while loading Newtonsoft.Json", ex);
                }
                Loaded = true;
            }
            catch(Exception ex)
            {
               logger.Error(ex);
            }
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

