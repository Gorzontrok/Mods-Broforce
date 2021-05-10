using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using UnityModManagerNet;

namespace TrophyManager
{
    public class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        public static string progressionPath = "./Mods/TrophyManagerMod/";
        public static Rect windowRect = new Rect(20, 20, 120, 50);
        
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
            }
            catch(Exception ex)
            {
                mod.Logger.Log(ex.ToString());
            }
            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            var styleT_Name = GUI.skin.textField;
            styleT_Name.fontSize = 20;
            styleT_Name.fontStyle = FontStyle.Bold;

            windowRect = GUILayout.Window(0, windowRect, trophyWindows, "My Window");

            if (GUILayout.Button("Reset", GUILayout.Width(100)))
            {
                ResetTrophy();
            }
            
            GUILayout.BeginHorizontal();
            if(CheckTrophyDoneForImage("Decapitated") != null)
            {
                
                GUILayout.Label(CheckTrophyDoneForImage("Decapitated"), GUILayout.Width(86), GUILayout.Height(86));
                GUILayout.BeginVertical();
                GUILayout.TextField("Who turn off the light !?", styleT_Name);
                GUILayout.TextArea("Decapitate 50 Ennemy\n\nEnnemy decapitate: " + settings.decapitatedCount + " times",GUILayout.ExpandWidth(true));

                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            CheckTrophy();
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

        public static void trophyWindows(int windowID)
        {
            if (GUILayout.Button("Hello World"))
            {
                Main.Log("Got a click");
            }
        }

        public static Texture texConvert(string imgFile)
        {
            Texture2D tex;
            byte[] fileData;

            string t_imgPath = progressionPath + "/TrophyImg" + imgFile;

            if (File.Exists(t_imgPath))
            {
                fileData = File.ReadAllBytes(t_imgPath);

                tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
                tex.LoadImage(fileData);
                return tex;
            }
            else
            {
                //Main.Log("No such file :" + t_imgPath);
            }
            return null;

        }

        public static Texture CheckTrophyDoneForImage(string trophy)
        {
            Texture img;
            string trophyName = trophy.ToLower();
            if (TrophyDico.trophyDone.ContainsKey(trophy))
            {
                if (TrophyDico.trophyDone[trophy])
                {
                    img = texConvert("/t_m_" + trophyName + ".png");
                }
                else
                {
                    img = texConvert("/t_" + trophyName + ".png");
                }
                if(img == null)
                {
                    return texConvert("/imgMissing.png");
                }
                return img;
            }
            else
            {
                return texConvert("/imgMissing.png");
            }
            
        }

        public static void CheckTrophy()//Function to check trophy
        {
            Dictionary<String, int> trophyProgress = new Dictionary<String, int>()
            {
                {"Decapitated", settings.decapitatedCount}
            };

            foreach (KeyValuePair<string, int> trophyMaxCount in TrophyDico.trophyMax)//Check the max value to have for be done
            {
                foreach (KeyValuePair<string, int> trophyProgressC in trophyProgress)
                {
                    foreach (KeyValuePair<string, bool> trophy in TrophyDico.trophyDone) // Check if trophy can be check
                    {
                        if ((trophyMaxCount.Key == trophyProgressC.Key) && (trophyMaxCount.Value <= trophyProgressC.Value) && !trophy.Value)
                        {
                            Main.Log(trophyProgressC.Key + " trophy is done !");
                            TrophyDico.trophyDone[trophyProgressC.Key] = true;
                        }
                    }
                }
            }
        }

        public static void ResetTrophy()
        {
            string[] directoryFiles = Directory.GetFiles(progressionPath, "*.xml");

            foreach (string file in directoryFiles)
            {
                if (file == progressionPath + "Settings.xml")
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

    }


    public class Settings : UnityModManager.ModSettings
    {
        public int decapitatedCount = 0;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

    }
    [HarmonyPatch(typeof(Mook), "IsDecapitated")]
    static class Decapitate_AchievementPatch
    {
        public static void Postfix(ref bool __result)
        {
           if(__result)
            {
                Main.settings.decapitatedCount++;
            }

        }
    }
}
