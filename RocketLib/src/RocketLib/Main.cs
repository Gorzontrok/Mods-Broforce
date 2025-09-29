using System;
using System.IO;
using System.Reflection;
using HarmonyLib;
using UnityModManagerNet;

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
                    logger.Log("Failed to Patch Harmony : " + ex.ToString());
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
                
                // Register test menus
                RegisterTestMenus();
                
                Loaded = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }

        private static void RegisterTestMenus()
        {
            #if DEBUG
            try
            {
                // Register the basic FlexMenu example
                Menus.Core.MenuRegistry.RegisterMenu<Menus.Tests.BasicFlexMenuExample>(
                    displayText: "FLEX MENU TEST",
                    targetMenu: Menus.Core.TargetMenu.MainMenu,
                    position: Menus.Core.PositionMode.After,
                    positionReference: "OPTIONS",
                    priority: 100
                );
                
                logger.Log("Test menus registered successfully");
            }
            catch (Exception ex)
            {
                logger.Error($"Failed to register test menus: {ex}");
            }
            #endif
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

