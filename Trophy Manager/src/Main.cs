using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace TrophyManager
{
    public class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        public static string trophyFolderPath;

        static Trophy WhoTurnOffTheLight ;
        //static Trophy DoYouLikeMyMuscle;
        static Trophy BOOMYouAreNowInvisible;
        static Trophy ForMURICA;
        static Trophy JesusWillBeProud;
        static Trophy Guerrilla;
        //static Trophy DoorKill;
        static Trophy BeQuiet;
        static Trophy IsThisTheEnd;
        static Trophy IThoughtItWasTheEnd;
        static Trophy TheLastMeat;


        static bool Load(UnityModManager.ModEntry modEntry)
        {
            mod = modEntry;

            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            modEntry.OnUpdate = OnUpdate;
            settings = Settings.Load<Settings>(modEntry);

            var harmony = new Harmony(modEntry.Info.Id);
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);

                trophyFolderPath = mod.Path + "Trophy/";
                CreateTrophy();

                TrophyShower.Load();
            }
            catch (Exception ex)
            {
                mod.Logger.Log(ex.ToString());
            }

            return true;
        }
        static void CreateTrophy()
        {
            WhoTurnOffTheLight = new Trophy("Who Turn Off The Light ?!", 150, "Decapitate 150 enemies", trophyFolderPath + "WhoTurnOffTheLight.png", trophyFolderPath + "m_WhoTurnOffTheLight.png");
            ForMURICA = new Trophy("For MURICA !", 1000, "Kill 1000 enemies.", trophyFolderPath + "ForMURICA.png", trophyFolderPath + "m_ForMURICA.png");
            JesusWillBeProud = new Trophy("Jesus will be proud", 50000, "Kill 50 000 enemies.", trophyFolderPath + "JesusWillBeProud.png", trophyFolderPath + "m_JesusWillBeProud.png");
            BOOMYouAreNowInvisible = new Trophy("*BOOM* you are now invisible", 1500, "Explode 1500 enemies.", trophyFolderPath + "BOOMYouAreNowInvisible.png", trophyFolderPath + "m_BOOMYouAreNowInvisible.png");
            Guerrilla = new Trophy("Guerrilla", 50, "Give gun to 50 villagers.", trophyFolderPath + "Guerrilla.png", trophyFolderPath + "m_Guerrilla.png");
            BeQuiet = new Trophy("Be quiet !", 150, "Assasinate 150 enemies.", trophyFolderPath + "BeQuiet.png", trophyFolderPath + "m_BeQuiet.png");
            IsThisTheEnd = new Trophy("Is This The End ?", 1, "Kill Satan.", trophyFolderPath + "IsThisTheEnd.png", trophyFolderPath + "m_IsThisTheEnd.png");
            IThoughtItWasTheEnd = new Trophy("I Thought It Was The End !", 10, "Kill 10 times Satan.", trophyFolderPath + "IThoughtItWasTheEnd.png", trophyFolderPath + "m_IThoughtItWasTheEnd.png");
            TheLastMeat = new Trophy("The Last Meat", 1, "Make the sandworm alien miniboss swallow a turkey.", trophyFolderPath + "TheLastMeat.png", trophyFolderPath + "m_TheLastMeat.png");
        }
        static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            TrophyDico.CheckIsDone();
            //Decapitation
            WhoTurnOffTheLight.UpdateProgression(settings.DecapitatedCount);
            //Kill
            ForMURICA.UpdateProgression(settings.KillCount);
            JesusWillBeProud.UpdateProgression(settings.KillCount);
            //Explolsion
            BOOMYouAreNowInvisible.UpdateProgression(settings.ExplodeCount);
            //villager
            Guerrilla.UpdateProgression(settings.VillagerArmedCount);
            //Assasination
            BeQuiet.UpdateProgression(settings.AssasinationCount);
            //Satan kill
            IsThisTheEnd.UpdateProgression(settings.SatanFinalBossKill);
            IThoughtItWasTheEnd.UpdateProgression(settings.SatanFinalBossKill);
            //Swallow
            TheLastMeat.UpdateProgression(settings.SwallowAlienCount);
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            //Bold the name of the trophy
            var styleT_Name = GUI.skin.textField;
            styleT_Name.fontSize = 20;
            styleT_Name.fontStyle = FontStyle.Bold;
            //-------

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset", GUILayout.Width(100)))
            {
                ResetTrophy();//Like the function doesn't work, he doesn't work either
            }
            settings.Notif = GUILayout.Toggle(settings.Notif, "Screen Notification");
            GUILayout.EndHorizontal();

            try
            {       //Draw automatically all of the trophy
                foreach(Trophy trophy in TrophyDico.AllTrophyList)
                {
                    GUILayout.BeginHorizontal("box");
                    GUILayout.Space(10);
                    GUILayout.Label(trophy.ImageTex, GUILayout.Width(86), GUILayout.Height(86)); // Show the image of the trophy
                    GUILayout.BeginVertical();
                    GUILayout.TextField(trophy.Name, styleT_Name);// Show the name of the Trophy
                    GUILayout.TextArea(trophy.Description + "\n\nProgression : " + trophy.IntToShow() + "/" + trophy.Objective, GUILayout.ExpandWidth(true));// Show the progression in the description field
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }
            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
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

        private static void ResetTrophy() //Doesn't work anymore
        {
            TrophyDico.Reset();
            settings.DecapitatedCount = 0;
            settings.BlindCount = 0;
            settings.ExplodeCount = 0;
            settings.KillCount = 0;
            settings.VillagerArmedCount = 0;
            settings.EnemiesOnRopeCount = 0;
            settings.DoorKillCount = 0;
            settings.ShieldThrowCount = 0;
            settings.RecoverFromInseminationCount = 0;
            settings.AssasinationCount = 0;
        }
    }


    public class Settings : UnityModManager.ModSettings
    {
        public bool Notif;
        public bool LockTrophyDontHaveImage;
        // Count for Trophy
        public int DecapitatedCount = 0;
        public int BlindCount = 0;
        public int ExplodeCount = 0;
        public int KillCount = 0;
        public int VillagerArmedCount = 0;
        public int EnemiesOnRopeCount = 0;
        public int DoorKillCount = 0;
        public int ShieldThrowCount = 0;
        public int RecoverFromInseminationCount = 0;
        public int AssasinationCount = 0;
        public int SwallowAlienCount = 0;
        public int SatanFinalBossKill = 0;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
