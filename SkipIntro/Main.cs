using System;
using System.Reflection;
using HarmonyLib;
using TFBGames.Systems;
using UnityModManagerNet;

namespace SkipIntroMod
{
    static class Main
    {
        static bool Load( UnityModManager.ModEntry modEntry )
        {
            try
            {
                var harmony = new Harmony( modEntry.Info.Id );
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll( assembly );
            }
            catch ( Exception ex )
            {
                modEntry.Logger.Log( "Failed to Patch Harmony !\n" + ex.ToString() );
            }
            return true;
        }
    }

    [HarmonyPatch( typeof( Startup ), "Update" )]
    static class SkipIntro_Patch
    {
        static bool Prefix( Startup __instance )
        {
            // Wait for main menu assets to load
            if ( !GameSystems.ResourceManager.HasAssetsToEnterScene( "MainMenu" ) )
            {
                return false;
            }

            // Skip intro and go straight to main menu
            GameState.LoadLevel( "MainMenu" );
            return false;
        }
    }
}
