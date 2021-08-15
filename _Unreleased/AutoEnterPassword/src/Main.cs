using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace AutoEnterPassword
{
    class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;


        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            settings = Settings.Load<Settings>(modEntry);
            var harmony = new Harmony(modEntry.Info.Id);
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);
            }
            catch (Exception ex)
            {
                mod.Logger.Log(ex.ToString());
            }

            mod = modEntry;

            try { AutoLoad(); } catch (Exception ex) { Main.Log("Failed to AutoLoad the cheats !\n" + ex); }

            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            if (GUILayout.Button("IThinkPuttingMyTesticalsInSomeoneElseFaceWithoutTheirConsentIsOkay"))
                Cheats.IThinkPuttingMyTesticalsInSomeoneElseFaceWithoutTheirConsentIsOkay();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            settings.IThinkPuttingMyTesticalsInSomeoneElseFaceWithoutTheirConsentIsOkay = GUILayout.Toggle(settings.IThinkPuttingMyTesticalsInSomeoneElseFaceWithoutTheirConsentIsOkay, "AutoLoad");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            if (GUILayout.Button("alaskanpipeline"))
                Cheats.alaskanpipeline();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            settings.alaskanpipeline = GUILayout.Toggle(settings.alaskanpipeline, "AutoLoad");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            if (GUILayout.Button("seagull"))
                Cheats.seagull();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            settings.seagull = GUILayout.Toggle(settings.seagull, "AutoLoad");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            if (GUILayout.Button("mranderbro"))
                Cheats.mranderbro();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            settings.mranderbro = GUILayout.Toggle(settings.mranderbro, "AutoLoad");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            if (GUILayout.Button("abrahamlincoln"))
                Cheats.abrahamlincoln();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            settings.abrahamlincoln = GUILayout.Toggle(settings.abrahamlincoln, "AutoLoad");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            if (GUILayout.Button("smokinggun"))
                Cheats.smokinggun();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            settings.smokinggun = GUILayout.Toggle(settings.smokinggun, "AutoLoad");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
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

        static void AutoLoad()
        {
            if (settings.IThinkPuttingMyTesticalsInSomeoneElseFaceWithoutTheirConsentIsOkay)
                Cheats.IThinkPuttingMyTesticalsInSomeoneElseFaceWithoutTheirConsentIsOkay();

            if (settings.alaskanpipeline)
                Cheats.alaskanpipeline();

            if (settings.seagull)
                Cheats.seagull();

            if (settings.mranderbro)
                Cheats.mranderbro();

            if (settings.abrahamlincoln)
                Cheats.abrahamlincoln();

            if (settings.smokinggun)
                Cheats.smokinggun();
        }
    }

    public static class Cheats
    {
        public static void IThinkPuttingMyTesticalsInSomeoneElseFaceWithoutTheirConsentIsOkay()
        {
            TestVanDammeAnim.teaBagCheatEnabled = true;
            Main.Log("'IThinkPuttingMyTesticalsInSomeoneElseFaceWithoutTheirConsentIsOkay' loaded !");
        }
        public static void alaskanpipeline()
        {
            HeroUnlockController.UnlockAllBros();
            PlayerProgress.Save(true);
            Main.Log("'alaskanpipeline' loaded !");
        }
        public static void seagull()
        {
            HeroUnlockController.UnlockEverythingButBroheart(); // False ! Unlock all until The Brode !
            PlayerProgress.Save(true);
            Main.Log("'seagull' loaded !");
        }
        public static void mranderbro()
        {
            Map.SetTryReduceLoadingTimes(true);
            Main.Log("'mranderbro' loaded !");
        }

        public static void abrahamlincoln()
        {
            GameModeController.CheatsEnabled = true;
            Main.Log("'abrahamlincoln' loaded !");
        }

        public static void smokinggun()
        {
            LevelEditorGUI.hackedEditorOn = true;
            Main.Log("'smokinggun' loaded !");
        }

    }
    public class Settings : UnityModManager.ModSettings
    {
        public bool IThinkPuttingMyTesticalsInSomeoneElseFaceWithoutTheirConsentIsOkay;
        public bool alaskanpipeline;
        public bool seagull;
        public bool mranderbro;
        public bool abrahamlincoln;
        public bool smokinggun;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
