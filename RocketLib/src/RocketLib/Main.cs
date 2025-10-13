using System;
using System.IO;
using System.Reflection;
using HarmonyLib;
using UnityModManagerNet;
using RocketLib.Menus.Core;
using RocketLib.Menus.Tests;

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

                Loaded = true;

                // Uncomment to enable test menus:
                // RegisterTestMenus();
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }


        private static void RegisterTestMenus()
        {
            MenuRegistry.RegisterMenu<BasicFlexMenuExample>(
                displayText: "Basic Flex Menu Test",
                targetMenu: TargetMenu.MainMenu,
                positionReference: "OPTIONS"
            );

            MenuRegistry.RegisterMenu<VanillaSubmenuExample>(
                displayText: "Vanilla Submenu Test",
                targetMenu: TargetMenu.MainMenu,
                positionReference: "OPTIONS"
            );

            MenuRegistry.RegisterMenu<ModOptionsExample>(
                displayText: "Test Mod Options",
                targetMenu: TargetMenu.ModOptions
            );

            MenuRegistry.RegisterMenu<GridLayoutExample>(
                displayText: "Grid Layout Test",
                targetMenu: TargetMenu.MainMenu,
                positionReference: "OPTIONS"
            );

            Main.logger.Log("Test menus registered successfully");
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

