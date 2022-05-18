using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using RocketLib0;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace AutoEnterPassword
{
    static class Main
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
            mod = modEntry;

            var harmony = new Harmony(modEntry.Info.Id);
            try
            {
                var myAssembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(myAssembly);
            }
            catch (Exception ex)
            {
                mod.Logger.Log("Fail\n" +ex.ToString());
            }


            try { AutoLoad(); } catch (Exception ex) { Main.Log("Failed to AutoLoad the cheats !\n" + ex); }

            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();

            settings.IThinkPuttingMyTesticalsInSomeoneElseFaceWithoutTheirConsentIsOkay = InGamePasswordGUI(settings.IThinkPuttingMyTesticalsInSomeoneElseFaceWithoutTheirConsentIsOkay, "IThinkPuttingMyTesticalsInSomeoneElseFaceWithoutTheirConsentIsOkay", Cheats.IThinkPuttingMyTesticalsInSomeoneElseFaceWithoutTheirConsentIsOkay);

            settings.alaskanpipeline = InGamePasswordGUI(settings.alaskanpipeline, "alaskanpipeline", Cheats.alaskanpipeline);

            settings.seagull = InGamePasswordGUI(settings.seagull, "seagull", Cheats.seagull);

            settings.mranderbro = InGamePasswordGUI(settings.mranderbro, "mranderbro", Cheats.mranderbro);

            settings.abrahamlincoln = InGamePasswordGUI(settings.abrahamlincoln, "abrahamlincoln", Cheats.abrahamlincoln);

            settings.smokinggun = InGamePasswordGUI(settings.smokinggun, "smokinggun", Cheats.smokinggun);

            GUILayout.EndHorizontal();

            if(GamePasswordController.GamePasswords.Count > 0)
            {
                GUILayout.Label("RocketLib Password :");
            }
            for (int i = 0; i < GamePasswordController.GamePasswords.Count; i++)
            {
                if(i == 0)
                {
                    GUILayout.BeginHorizontal();
                }
                if (GUILayout.Button(GamePasswordController.GamePasswords[i].password, GUILayout.ExpandWidth(false)))
                {
                    GamePasswordController.GamePasswords[i].action();
                }
                if (i % 5 == 0 || i == GamePasswordController.GamePasswords.Count)
                {
                    GUILayout.EndHorizontal();
                }
            }
        }

        static bool InGamePasswordGUI(bool active, string text, Action action)
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button(text))
            {
                action();
            }
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            active = GUILayout.Toggle(active, "AutoLoad");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            return active;
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
        public bool iloveamerica;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
