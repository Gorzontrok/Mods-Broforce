using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Runtime;
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
        public static string trophyFolderPath = progressionPath + "/Trophy";

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

        public class TrophyInfo
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string ImagePath { get; set; }
            public int MaxCount { get; set; }
            public int Progression { get; set; }
            public bool IsComplete { get; set; }
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

            try
            {       //Draw automatically all of the trophy
                var trophy = new TrophyInfo();
                foreach (KeyValuePair<string, object[]> Trophy in TrophyDico.trophyIntObjective)
                {
                    string Name = Trophy.Key;
                    object[] info = Trophy.Value;

                        trophy.Description = info[0].ToString();
                        trophy.ImagePath = info[1].ToString();
                        trophy.IsComplete = (bool)info[2];
                        trophy.MaxCount = (int)info[3];
                        trophy.Progression = (int)info[4];

                    info[2] = CheckTrophy(trophy.Progression, trophy.MaxCount, trophy.IsComplete, Name);
                    trophy.IsComplete = CheckTrophy(trophy.Progression, trophy.MaxCount, (bool)info[2], Name);

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(CheckTrophyDoneForImage(trophy.ImagePath, trophy.IsComplete), GUILayout.Width(86), GUILayout.Height(86));
                    GUILayout.BeginVertical();
                    GUILayout.TextField(Name, styleT_Name);
                    GUILayout.TextArea(trophy.Description + "\n\nProgression : " + trophy.Progression + "/" + trophy.MaxCount, GUILayout.ExpandWidth(true));
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
        //-----------------------------
        public static Texture texConvert(string imgFile) //Convert the image of the trophy to a Texture2D
        {
            Texture2D texture;
            byte[] fileData;

            string t_imgPath = progressionPath + "/Trophy/" + imgFile;
            fileData = File.ReadAllBytes(t_imgPath);

            texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            texture.LoadImage(fileData);
            return texture;
        }

        public static Texture CheckTrophyDoneForImage(string ImagePath, bool IsDone)
        {
            Texture image;
            string imgTrophyDone = "m_";
            try
            {
                if (IsDone)
                {
                    if (File.Exists(trophyFolderPath + imgTrophyDone + ImagePath))
                    {
                        image = texConvert(imgTrophyDone + ImagePath);
                    }
                    else
                    {
                        image = texConvert(imgTrophyDone + "imgMissing.png");
                    }
                }
                else
                {
                    if (File.Exists(trophyFolderPath + ImagePath))
                    {
                        image = texConvert(imgTrophyDone + ImagePath);
                    }
                    image = texConvert("imgMissing.png");
                }

                if (image == null)
                    image = texConvert("error.png");
            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
                image = texConvert("error.png");
            }
            return image;
        }

        public static bool CheckTrophy(int Progression, int Objective, bool IsClaim, string Name)//Function to check trophy
        {
            if (IsClaim)
                return true;

            if (!IsClaim)
            {
                if (Progression >= Objective)
                {
                    Main.Log("'" + Name + "' trophy is done !");
                    return true;
                }
                return false;
            }

            return false;
        }

        public static void ResetTrophy() //Doesn't work anymore
        {
            string[] directoryFiles_xml = Directory.GetFiles(progressionPath, "*.xml");

            foreach (string file in directoryFiles_xml)
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

    //Who turn off the light TROPHY
    [HarmonyPatch(typeof(Mook), "IsDecapitated")]
    static class WhoTurnOffTheLight_TrophyPatch
    {
        public static void Postfix(ref bool __result)
        {
            if (__result)
            {
                Main.settings.decapitatedCount++;
                //Main.CheckTrophy();
            }

        }
    }
    //DoYouLikeMyMuscle TROPHY
    [HarmonyPatch(typeof(Mook), "StopBeingBlind", new Type[] { })]//Work 👍
    static class DoYouLikeMyMuscle_TrophyPatch
    {
        public static void Postfix()
        {
            Main.settings.blindCount++;
            //Main.CheckTrophy();
        }
    }
    //Explode TROPHY
    [HarmonyPatch(typeof(TestVanDammeAnim), "CreateGibs", new Type[] {typeof(float), typeof(float)})]//Work 👍
    static class BoomYouAreNowInvisible_TrophyPatch
    {
        public static void Postfix()
        {
            try
            {
                Main.settings.explodeCount++;
               // Main.CheckTrophy();
            }
            catch(Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }

    //KILL TROPHY
    [HarmonyPatch(typeof(Mook), "OnDestroy", new Type[] { })]//Work 👍
    static class KillTrophy_TrophyPatch
    {
        public static void Postfix()
        {
            try
            {
                Main.settings.killCount++;
               // Main.CheckTrophy();

            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }

    //Guerrilla TROPHY
    [HarmonyPatch(typeof(Villager), "ActivateGun", new Type[] { })]
    static class GuerillaTrophy_TrophyPatch
    {
        public static void Postfix()
        {
            try
            {
                Main.settings.villagerCount++;
                //Main.CheckTrophy();

            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }

    //Predabro TROPHY
    [HarmonyPatch(typeof(PredabroRope), "SetUp", new Type[] { typeof(Unit)})]
    static class predabroTrophy_TrophyPatch
    {
        public static void Postfix()
        {
            try
            {
                Main.settings.ennemiOnRopeCount++;
                //Main.CheckTrophy();

            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }
    //door kill TROPHY
    [HarmonyPatch(typeof(DoorDoodad), "MakeEffectsDeath", new Type[] {  })]
    static class doorKillTrophy_TrophyPatch
    {
        public static void Postfix()
        {
            try
            {
                Main.settings.doorKillCount++;
                //Main.CheckTrophy();

            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }

    //shield TROPHY
    [HarmonyPatch(typeof(MookRiotShield), "DisarmShield", new Type[] { typeof(float) })]
    static class shieldThrowTrophy_TrophyPatch
    {
        public static void Postfix()
        {
            try
            {
                Main.settings.shieldThrowCount++;
                //Main.CheckTrophy();

            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }
    //anti-insemination TROPHY
    [HarmonyPatch(typeof(TestVanDammeAnim), "RecoverFromInsemination", new Type[] {  })]
    static class InsecticidTrophy_TrophyPatch
    {
        public static void Postfix()
        {
            try
            {
                Main.settings.recoverInseminationCount++;
                //Main.CheckTrophy();

            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }
    //assassination TROPHY
    [HarmonyPatch(typeof(Mook), "AnimateAssassinated", new Type[] {  })]
    static class AssassinationTrophy_TrophyPatch
    {
        public static void Postfix()
        {
            try
            {
                Main.settings.assassinationCount++;
                //Main.CheckTrophy();

            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }
}
