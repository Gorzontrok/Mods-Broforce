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

            /*try
            {
                foreach (KeyValuePair<string, object[]> Trophy in TrophyDico.trophyIntObjective)
                {
                    string Name = Trophy.Key;
                    object[] info = Trophy.Value;
                    foreach (object[] item in info)
                    {
                        string description = info[0].ToString();
                        string imgPath = info[1].ToString();
                        //bool IsMade = info[2];
                    }
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(CheckTrophyDoneForImage(Name), GUILayout.Width(86), GUILayout.Height(86));
                }

            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }*/

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

        public static void GetTrophyFile() //WIP for automate add trophy for avoid the shit in the OnGui()
        {                                          //Make it like the manager 🤷‍
            try
            {
                foreach(KeyValuePair<string, object[]> Trophy in TrophyDico.trophyIntObjective)
                {
                    foreach(object info in Trophy.Value)
                    {
                        Main.Log(info.ToString());
                    }
                }

            }catch(Exception ex)
            {
                Main.Log(ex.ToString());
            }

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
                try
                {
                    if (TrophyDico.trophyDone[trophy])
                    {
                        img = texConvert("/t_m_" + trophy + ".png");
                        if (img == null)
                            img = texConvert("/m_imgMissing.png");
                    }
                    else
                    {
                        img = texConvert("/t_" + trophy + ".png");
                        if (img == null)
                            img = texConvert("/imgMissing.png");

                    }
                    return img;

                }
                catch (Exception ex)
                {
                    Main.Log(ex.ToString());
                }
            }
            else
            {
                return texConvert("/imgMissing.png");
            }
            return null;
        }

        public static void CheckTrophy()//Function to check trophy
        {
            foreach (KeyValuePair<string, bool> IsClaim in TrophyDico.trophyDone) // Check if trophy is already claim
            {
                if (!IsClaim.Value)
                {
                    foreach (KeyValuePair<string, int> Progression in TrophyDico.trophyProgress)//Check the current Progress
                    {
                        foreach (KeyValuePair<string, int> Max in TrophyDico.trophyMax) //Check the max value to have for be done
                        {
                            if ((Max.Key == Progression.Key) && (Progression.Key == IsClaim.Key) && (Max.Value <= Progression.Value))// If the max
                            {
                                Main.Log("'" + Progression.Key + "' trophy is done !");
                                TrophyDico.trophyDone[Progression.Key] = true;
                            }
                        }
                    }
                }
            }
        }

        public static void ResetTrophy()
        {
            string[] directoryFiles_xml = Directory.GetFiles(progressionPath, "*.xml");
            string[] directoryFiles_cache = Directory.GetFiles(progressionPath, "*.cache");

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
                Main.CheckTrophy();
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
            Main.CheckTrophy();
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
                Main.CheckTrophy();
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
                Main.CheckTrophy();

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
                Main.CheckTrophy();

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
                Main.CheckTrophy();

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
                Main.CheckTrophy();

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
                Main.CheckTrophy();

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
                Main.CheckTrophy();

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
                Main.CheckTrophy();

            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }
}
