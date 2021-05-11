using System;
using System.Collections.Generic;
using System.Reflection;
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

        public static Component comp;
        public static BroforceObject BO;
        public static TestVanDammeAnim TVDA;
        public static NetworkedUnit NU;

        public static string progressionPath = "./Mods/TrophyManagerMod/";

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            mod = modEntry;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            //modEntry.OnUpdate = OnUpdate;
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

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            var styleT_Name = GUI.skin.textField;
            styleT_Name.fontSize = 20;
            styleT_Name.fontStyle = FontStyle.Bold;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset", GUILayout.Width(100)))
            {
                ResetTrophy();
            }
            if (GUILayout.Button("Check Trophy", GUILayout.Width(100)))
            {
                CheckTrophy();
            }
            if (GUILayout.Button("Check Trophy File", GUILayout.Width(100)))
            {
                GetTrophyFile();
            }
            GUILayout.EndHorizontal();


            //--------'Who turn of the light' trophy
            GUILayout.BeginHorizontal();
            GUILayout.Label(CheckTrophyDoneForImage("Who Turn Off The Light"), GUILayout.Width(86), GUILayout.Height(86));
            GUILayout.BeginVertical();
            GUILayout.TextField("Who Turn Off The Light !?", styleT_Name);
            GUILayout.TextArea("Decapitate " + TrophyDico.trophyMax["Who Turn Off The Light"] + " enemies\n\nenemies decapitate: " + settings.decapitatedCount + " times", GUILayout.ExpandWidth(true));
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            //--------------------------------------
            //--------'Next Time Try With Two Eyes' trophy
            GUILayout.BeginHorizontal();
            GUILayout.Label(CheckTrophyDoneForImage("Do You Like My Muscle"), GUILayout.Width(86), GUILayout.Height(86));
            GUILayout.BeginVertical();
            GUILayout.TextField("Do you like my muscle ?", styleT_Name);
            GUILayout.TextArea("Make " + TrophyDico.trophyMax["Do you like my muscle ?"] + " enemies blind.\n\nenemies blind: " + settings.blindCount + " times", GUILayout.ExpandWidth(true));
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            //--------------------------------------
            //--------'Next Time Try With Two Eyes' trophy
            GUILayout.BeginHorizontal();
            GUILayout.Label(CheckTrophyDoneForImage("*BOOM* You Are Now Invisible."), GUILayout.Width(86), GUILayout.Height(86));
            GUILayout.BeginVertical();
            GUILayout.TextField("*BOOM* you are now invisible.", styleT_Name);
            GUILayout.TextArea("Make " + TrophyDico.trophyMax["*BOOM* you are now invisible."] + " enemies explode.\n\nenemies blow up: " + settings.explodeCount + " times", GUILayout.ExpandWidth(true));
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            //--------------------------------------
            //--------'For MURICA !' trophy
            GUILayout.BeginHorizontal();
            GUILayout.Label(CheckTrophyDoneForImage("For MURICA"), GUILayout.Width(86), GUILayout.Height(86));
            GUILayout.BeginVertical();
            GUILayout.TextField("For MURICA !", styleT_Name);
            GUILayout.TextArea("Kill " + TrophyDico.trophyMax["For MURICA !"] + " enemies.\n\nEnemies kills: " + settings.killCount, GUILayout.ExpandWidth(true));
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            //--------------------------------------
        }
        /*
        static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            if()
            {

            }
        }*/

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

        public class TrophyInfo
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string MaxCount { get; set; }
            public string Image { get; set; }
        }

        public static List<string> GetTrophyFile() //WIP for automate add trophy for avoid the shit in the OnGui()
        {                                          //Make it like the manager 🤷‍
            string trophyPath = "./Mods/TrophyManagerMod/Trophy/";
            List<string> txtLine = new List<string>();
            string line;
            try
            {
                string[] trophyFile = Directory.GetFiles(trophyPath, "t_*.txt");
                foreach (string files in trophyFile)
                {
                    StreamReader txt = new StreamReader(files);
                    while ((line = txt.ReadLine()) != null)
                    {
                        txtLine.Add(line);
                        Main.Log(line);
                    }
                }
            }catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
            Main.Log(txtLine.ToString());
            return txtLine;
        }

        public static Texture texConvert(string imgFile)
        {
            Texture2D tex;
            byte[] fileData;

            string t_imgPath = progressionPath + "/Trophy" + imgFile;

            if (File.Exists(t_imgPath))
            {
                fileData = File.ReadAllBytes(t_imgPath);

                tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
                tex.LoadImage(fileData);
                return tex;
            }
            return null;

        }

        public static Texture CheckTrophyDoneForImage(string trophy)
        {
            Texture img;
            if (TrophyDico.trophyDone.ContainsKey(trophy))
            {
                if (TrophyDico.trophyDone[trophy])
                {
                    img = texConvert("/t_m_" + trophy + ".png");
                }
                else
                {
                    img = texConvert("/t_" + trophy + ".png");
                }

                if (img == null)
                {
                    if(TrophyDico.trophyDone[trophy])
                    {
                        return texConvert("/m_imgMissing.png");

                    }
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
            foreach (KeyValuePair<string, int> Max in TrophyDico.trophyMax)//Check the max value to have for be done
            {
                foreach (KeyValuePair<string, int> Progression in TrophyDico.trophyProgress)//Check the current Progress
                {
                    foreach (KeyValuePair<string, bool> IsClaim in TrophyDico.trophyDone) // Check if trophy is already claim
                    {
                        if ((Max.Key == Progression.Key) && (Max.Value <= Progression.Value) && IsClaim.Value == false)// If the max
                        {
                            Main.Log("'" + Progression.Key + "' trophy is done !");
                            TrophyDico.trophyDone[Progression.Key] = true;
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
        public int blindCount = 0;
        public int explodeCount = 0;
        public int killCount = 0;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

    }
    [HarmonyPatch(typeof(Mook), "IsDecapitated")]
    static class WhoTurnOffTheLight_TrophyPatch
    {
        public static void Postfix(ref bool __result)
        {
            if (__result)
            {
                Main.settings.decapitatedCount++;
                Main.CheckTrophy();
            }

        }
    }

    [HarmonyPatch(typeof(Mook), "StopBeingBlind", new Type[] { })] //The function is not call so don't work
    static class DoYouLikeMyMuscle_TrophyPatch
    {
        public static void Prefix(Mook __instance)
        {
            Main.settings.blindCount++;
            if (__instance.enemyAI != null)
            {
                __instance.enemyAI.StopBeingBlind();
            }
            __instance.ForceChangeFrame();
            __instance.Stop();
            __instance.firingPlayerNum = Main.NU.playerNum;
        }
    }

    [HarmonyPatch(typeof(TestVanDammeAnim), "CreateGibs", new Type[] {typeof(float), typeof(float)})]
    static class BoomYouAreNowInvisible_TrophyPatch
    {
        public static void Prefix(ref float xI, ref float yI)
        {
            try
            {
                Main.settings.explodeCount++;
                EffectsController.CreateGibs(Main.TVDA.gibs, Main.comp.GetComponent<Renderer>().sharedMaterial, Main.BO.X, Main.BO.Y, 100f, 100f, xI * 0.25f, yI * 0.25f + 60f);
            }
            catch(Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }
    //KILL TROPHY

    [HarmonyPatch(typeof(Mook), "OnDestroy", new Type[] { })]
    static class KillTrophy_TrophyPatch
    {
        public static void Prefix(Mook __instance)
        {
            try
            {
                bool hasDied = Traverse.Create(__instance).Field("hasDied").GetValue<bool>();
                if (hasDied)
                {
                    Main.settings.killCount++;
                    MapController.currentActiveDeadMooksInScene--;
                }
                MapController.currentDeadMooksInScene++;
            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }
}
