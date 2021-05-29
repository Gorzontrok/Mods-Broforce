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

        //Need to declare them for the patches with Harmony
        public static Component comp;
        public static BroforceObject BO;
        public static TestVanDammeAnim TVDA;
        public static NetworkedUnit NU;

        //Path needed in some function
        public static string modPath = "./Mods/TrophyManagerMod/";
        public static string trophyFolderPath = modPath + "Trophy/";


        static bool Load(UnityModManager.ModEntry modEntry)
        {
            mod = modEntry;

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
            return true;
        }

        public class TrophyInfo
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string FolderPath { get; set; }
            public string ImagePath { get; set; }
            public int MaxCount { get; set; }
            public int Progression { get; set; }
            public bool IsComplete { get; set; }
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
            GUILayout.EndHorizontal();

            try
            {       //Draw automatically all of the trophy
                var trophy = new TrophyInfo();//Don't know if i really need it but i made it
                foreach (KeyValuePair<string, object[]> Trophy in TrophyDico.trophyIntObjective)
                {
                    string Name = Trophy.Key;
                    object[] info = Trophy.Value;

                        trophy.Description = info[0].ToString();
                        trophy.FolderPath = info[1].ToString();
                        trophy.ImagePath = info[2].ToString();
                        trophy.IsComplete = (bool)info[3];
                        trophy.MaxCount = (int)info[4];
                        trophy.Progression = (int)info[5];

                    info[3] = CheckTrophy(trophy.Progression, trophy.MaxCount, trophy.IsComplete, Name);
                    trophy.IsComplete = CheckTrophy(trophy.Progression, trophy.MaxCount, (bool)info[3], Name);

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(CheckTrophyDoneForImage(trophy.ImagePath, trophy.FolderPath, trophy.IsComplete), GUILayout.Width(86), GUILayout.Height(86)); // Show the image of the trophy
                    GUILayout.BeginVertical();
                    GUILayout.TextField(Name, styleT_Name);// Show the name of the Trophy
                    GUILayout.TextArea(trophy.Description + "\n\nProgression : " + WhatIntToShow(trophy.Progression, trophy.MaxCount) + "/" + trophy.MaxCount, GUILayout.ExpandWidth(true));// Show the progression in the description field
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

        public static void Log(String str)
        {
            mod.Logger.Log(str);
        }


        // START OF FUNCTION FOR THE MOD
        // -----------------------------
        private static Texture texConvert(string imgFile) //Convert the image of the trophy to a Texture2D
        {
            Texture2D texture;
            byte[] fileData;

            fileData = File.ReadAllBytes(imgFile);

            texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            texture.LoadImage(fileData);
            return texture;
        }

        private static Texture CheckTrophyDoneForImage(string ImagePath, string folderPath, bool IsDone)
        {
            Texture image;
            string imgTrophyDone = "m_";
            try
            {
                if (IsDone)
                {
                    if (File.Exists(folderPath + imgTrophyDone + ImagePath))
                    {
                        image = texConvert(folderPath + imgTrophyDone + ImagePath); //Get the done image
                    }
                    else
                    {
                        image = texConvert(trophyFolderPath + imgTrophyDone + "imgMissing.png"); //otherwise get the image missing
                    }
                }
                else
                {
                    if (File.Exists(folderPath + ImagePath))
                    {
                        image = texConvert(folderPath + ImagePath);// Get the normal image

                    }
                    else
                    {
                        image = texConvert(trophyFolderPath + "imgMissing.png");//Otherwise get the image Missing
                    }
                }

                if (image == null)
                {
                    image = texConvert(trophyFolderPath + "error.png"); //if it show we have a problem 👏
                    Main.Log("Error");
                }
            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
                image = texConvert(folderPath + "error.png");
            }
            return image;
        }

        public static bool CheckTrophy(int Progression, int Objective, bool IsClaim, string Name)//Function to check trophy and return the stat of it
        {
            if (IsClaim)
                return true;

            if (!IsClaim)
            {
                if (Progression >= Objective)// if he can be claim
                {
                    Main.Log("'" + Name + "' trophy is done !");
                    return true;
                }
                return false;
            }

            return false;
        }

        private static int WhatIntToShow(int Progression, int Objective)
        {
            if (Progression >= Objective) return Objective;
            return Progression;
        }

        private static void ResetTrophy() //Doesn't work anymore
        {
            //This function delete the Settings.xml, where the Progression is save.
            string[] directoryFiles_xml = Directory.GetFiles(modPath, "*.xml");

            foreach (string file in directoryFiles_xml)
            {
                if (file == modPath + "Settings.xml")
                {
                    try
                    {
                        File.Delete(file);
                        Main.Log("Deleted file :" + file);
                    }
                    catch (FileNotFoundException ex)
                    {
                        Main.Log(ex.ToString() + " already delete.");
                    }
                    catch (Exception ex)
                    {
                        Main.Log(ex.ToString());
                    }
                }
            }
        }
        // -----------------------------
    }


    public class Settings : UnityModManager.ModSettings
    {
        public int decapitatedCount = 0;
        public int blindCount = 0;
        public int explodeCount = 0;
        public int killCount = 0;
        public int villagerCount = 0;
        public int ennemiOnRopeCount = 0;
        public int doorKillCount = 0;
        public int shieldThrowCount = 0;
        public int recoverInseminationCount = 0;
        public int assassinationCount = 0;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
