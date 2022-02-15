using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace TrophyManager
{
    static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        internal static string trophyFolderPath;

        static Trophy WhoTurnOffTheLight ;
        static Trophy DoYouLikeMyMuscle;
        static Trophy BOOMYouAreNowInvisible;
        static Trophy ForMURICA;
        static Trophy JesusWillBeProud;
        static Trophy Guerrilla;
        static Trophy DoorKill;
        static Trophy BeQuiet;
        static Trophy IsThisTheEnd;
        static Trophy IThoughtItWasTheEnd;
        static Trophy TheLastMeat;


        static bool Load(UnityModManager.ModEntry modEntry)
        {

            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            modEntry.OnUpdate = OnUpdate;
            settings = Settings.Load<Settings>(modEntry);

            mod = modEntry;

            var harmony = new Harmony(modEntry.Info.Id);
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);

                trophyFolderPath = mod.Path + "Trophy/";

                TrophyShower.Load();
                Trophy.errorTexture = TrophyController.CreateTextureFromPath(Main.trophyFolderPath + "Error.png");
                Trophy.hideTexture = TrophyController.CreateTextureFromPath(Main.trophyFolderPath + "Hide.png");
                Trophy.achieveFrame = TrophyController.CreateTextureFromPath(Main.trophyFolderPath + "achieveFrame.png");

                CreateTrophy();
            }
            catch (Exception ex)
            {
                mod.Logger.Log(ex.ToString());
            }

            return true;
        }
        static void CreateTrophy()
        {
            WhoTurnOffTheLight = new Trophy("Who Turn Off The Light ?!", 150, settings.DecapitatedCount, "Decapitate 150 enemies", TrophyController.CreateTextureFromPath(trophyFolderPath + "WhoTurnOffTheLight.png"));
            ForMURICA = new Trophy("For MURICA !", 1000, settings.KillCount, "Kill 1000 enemies.", TrophyController.CreateTextureFromPath(trophyFolderPath + "ForMURICA.png"));
            JesusWillBeProud = new Trophy("Jesus will be proud", 50000, settings.KillCount, "Kill 50 000 enemies.", TrophyController.CreateTextureFromPath(trophyFolderPath + "JesusWillBeProud.png"));
            BOOMYouAreNowInvisible = new Trophy("*BOOM* you are now invisible", 1500, settings.ExplodeCount, "Explode 1500 enemies.", TrophyController.CreateTextureFromPath(trophyFolderPath + "BOOMYouAreNowInvisible.png"));
            Guerrilla = new Trophy("Guerrilla", 50, settings.VillagerArmedCount, "Give gun to 50 villagers.", TrophyController.CreateTextureFromPath(trophyFolderPath + "Guerrilla.png"));
            BeQuiet = new Trophy("Be quiet !", 150, settings.AssasinationCount, "Assassinate 150 enemies.", TrophyController.CreateTextureFromPath(trophyFolderPath + "BeQuiet.png"));
            IsThisTheEnd = new Trophy("Is This The End ?", 1, settings.SatanFinalBossKill, "Kill Satan.", TrophyController.CreateTextureFromPath(trophyFolderPath + "IsThisTheEnd.png"));
            IThoughtItWasTheEnd = new Trophy("I Thought It Was The End !", 10,settings.SatanFinalBossKill, "Kill 10 times Satan.", TrophyController.CreateTextureFromPath(trophyFolderPath + "IThoughtItWasTheEnd.png"));
            TheLastMeat = new Trophy("The Last Meat", 1, settings.SwallowAlienCount, "Make the Sandworm alien Miniboss swallow a turkey.", TrophyController.CreateTextureFromPath(trophyFolderPath + "TheLastMeat.png"));
            //DoorKill = new Trophy("D-D-D-DOOR KILL !", 1, "Kill Someone with a door", trophyFolderPath + "DoorKill.png", trophyFolderPath + "m_DoorKill.png");
           // DoYouLikeMyMuscle = new Trophy("Do you like my muscle ?", 50, "Make 50 enemies blind.", trophyFolderPath + "DoYouLikeMyMuscle.png", trophyFolderPath + "m_DoYouLikeMyMuscle.png");
        }
        static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            TrophyController.CheckIsDone();
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
        private static Vector2 scrollViewVector;
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
            settings.LockedTrophyDontHaveImage = GUILayout.Toggle(settings.LockedTrophyDontHaveImage, "No image when locked");
            GUILayout.EndHorizontal();

            try
            {       //Draw automatically all of the trophy
                scrollViewVector = GUILayout.BeginScrollView(scrollViewVector, GUILayout.Height(500));
                foreach (Trophy trophy in TrophyController.AllTrophyList)
                {
                    GUILayout.BeginHorizontal("box");
                    GUILayout.Space(10);
                    //var rect = GUILayoutUtility.GetLastRect();
                    GUILayout.Label(trophy.TrophyTex, GUILayout.Width(86), GUILayout.Height(86)); // Show the image of the trophy
                    //GUI.Label(rect, doneTrophy);
                    GUILayout.BeginVertical();
                    GUILayout.TextField(trophy.Name, styleT_Name);// Show the name of the Trophy
                    GUILayout.TextArea(trophy.Description + "\n\nProgression : " + trophy.IntToShow() + "/" + trophy.Objective, GUILayout.ExpandWidth(true));// Show the progression in the description field
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
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

        internal static void Log(object str)
        {
            mod.Logger.Log(str.ToString());
        }

        private static void ResetTrophy() //Doesn't work anymore
        {
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
            TrophyController.Reset();
        }
    }


    public class Settings : UnityModManager.ModSettings
    {
        public bool Notif;
        public bool LockedTrophyDontHaveImage;
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
