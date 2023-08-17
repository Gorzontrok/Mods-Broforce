using HarmonyLib;
using RocketLib;
using RocketLib.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

namespace FilteredBros
{
    public static class Main
    {
        public static bool CanUsePatch
        {
            get
            {
                if (Main.enabled)
                {
                    if (LevelEditorGUI.IsActive || Map.isEditing || LevelSelectionController.IsCustomCampaign())
                    {
                        return settings.patchInCustomsLevel;
                    }
                    return true;
                }
                return false;
            }
        }

        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        public static Dictionary<int, HeroType> originalDict = Heroes.OriginalUnlockIntervals;

        public static List<HeroType> heroList = new List<HeroType>();
        public static List<int> heroInt = new List<int>(Heroes.HeroSaveInterval);
        public static bool cheat;
        public static HeroType[] broforceBros = Heroes.CampaignBro;
        public static HeroType[] expendabrosBros = Heroes.Expendabros;
        public static HeroType[] unusedBros = Heroes.Unused;
        public static List<HeroType> broList = new List<HeroType>();

        private static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            settings = Settings.Load<Settings>(modEntry);
            mod = modEntry;

            try
            {
                var harmony = new Harmony(modEntry.Info.Id);
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);
            }
            catch (Exception ex)
            {
                mod.Logger.Log("Failed to Patch Harmony !\n" + ex.ToString());
            }
            try
            {
                cheat = Environment.UserName == "Gorzon";
                Start();
            }
            catch(Exception ex)
            {
                Main.Log(ex);
            }
            return true;
        }

        private static void Start()
        {
            try
            {
                var temp = unusedBros.ToList();
                temp.Remove(HeroType.SuicideBro);
                unusedBros = temp.ToArray();

                heroList.AddRange(broforceBros);
                heroList.AddRange(expendabrosBros);
                heroList.AddRange(unusedBros);

                BuildBroToggles(broforceBros, BroToggle.BroGroup.Broforce);
                BuildBroToggles(expendabrosBros, BroToggle.BroGroup.Expendabros);
                BuildBroToggles(unusedBros, BroToggle.BroGroup.Hide);

                if (BroToggle.BroToggles.Count == settings.brosEnable.Count)
                {
                    int i = 0;
                    foreach (BroToggle broToggle in BroToggle.BroToggles)
                    {
                        broToggle.enabled = settings.brosEnable[i];
                        i++;
                    }
                }

                foreach (BroToggle b in BroToggle.BroToggles)
                {
                    heroInt.Add(b.unlockNumber);
                }

                new GamePassword("iaminthematrix", IAmInTheMatrix);
            }
            catch(Exception ex )
            {
                Main.Log(ex);
            }
        }
        private static int _intervalMax = -1;
        private static void BuildBroToggles(HeroType[] heroArray, BroToggle.BroGroup group)
        {
            Dictionary<int, HeroType> dict = new Dictionary<int, HeroType>(originalDict);
            if(_intervalMax == -1)
                _intervalMax = originalDict.Last().Key;

            foreach (HeroType hero in heroArray)
            {
                try
                {
                    int interval = -1;
                    if (dict.ContainsValue(hero))
                    {
                        interval = dict.First(x => x.Value == hero).Key;
                    }

                    if(interval == -1)
                    {
                        interval = _intervalMax + 10;
                        _intervalMax = interval;
                    }
                    new BroToggle(hero, interval, group);
                }
                catch (Exception ex)
                {
                    Main.Log(ex);
                }
            }
        }

        private static void IAmInTheMatrix()
        {
            cheat = true;
        }

        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            var s = new GUIStyle();
            s.normal.textColor = Color.yellow;
            s.fontStyle = FontStyle.Italic;

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("I Am In The Matrix", s);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Number of bro select : " + BroToggle.brosEnable);
            if (GUILayout.Button("Select all", GUILayout.ExpandWidth(false)))
                SelectAll();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Number of bro per line : " + settings.numberOfBroPerLine);
            settings.numberOfBroPerLine = (int)GUILayout.HorizontalScrollbar(settings.numberOfBroPerLine, 0, 3, 15, GUILayout.MinWidth(200));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Only expendabros", GUILayout.ExpandWidth(false)))
                SelectOnlyExpendabros();
            if (GUILayout.Button("Select nothing", GUILayout.ExpandWidth(false)))
                SelectNothing();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Max number of lives (0 to disabled) :  ");
            int.TryParse(GUILayout.TextField(settings.maxLifeNumber.ToString(), GUILayout.Width(80)), out settings.maxLifeNumber);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            settings.ignoreForcedBros = GUILayout.Toggle(settings.ignoreForcedBros, new GUIContent("Ignore forced bros in campaigns"));
            GUILayout.FlexibleSpace();
            settings.patchInCustomsLevel = GUILayout.Toggle(settings.patchInCustomsLevel, new GUIContent("Enable in custom level"));
            GUILayout.EndHorizontal();
            int numberOfRescuesToNextUnlock = HeroUnlockController.GetNumberOfRescuesToNextUnlock();
            if (numberOfRescuesToNextUnlock != -1)
            {
                GUILayout.Label(String.Format("Next unlock in {0} saves", numberOfRescuesToNextUnlock));
            }
            GUILayout.Space(10);

            var typeStyle = new GUIStyle();
            typeStyle.normal.textColor = Color.gray;
            typeStyle.fontSize = 15;
            typeStyle.fontStyle = FontStyle.Bold;

            // Broforce Basic
            GUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(false));
            GUILayout.Label(" - Broforce :", typeStyle);
            GUILayout.EndHorizontal();
            ShowToggles(BroToggle.broTogglesBroforce);

            // - Expendabros :
            GUILayout.Space(25);
            GUILayout.BeginHorizontal("box");
            GUILayout.Label(" - Expendabros :", typeStyle);
            GUILayout.EndHorizontal();
            ShowToggles(BroToggle.broTogglesExpendabros);

            // Hide
            GUILayout.Space(25);
            GUILayout.BeginHorizontal("box");
            GUILayout.Label(" - Secrets :", typeStyle);
            GUILayout.EndHorizontal();
            ShowToggles(BroToggle.broTogglesHide);
        }

        private static void ShowToggles(List<BroToggle> broToggles)
        {
            int i = 0;
            bool horizontal = false;
            foreach(BroToggle broToggle in broToggles)
            {
                if (i == 0)
                {
                    GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
                    horizontal = true;
                }
                broToggle.Toggle();
                GUILayout.Space(10);
                i++;
                if (i == settings.numberOfBroPerLine)
                {
                    i = 0;
                    GUILayout.EndHorizontal();
                    horizontal = false;
                }
            }
            if(horizontal)
            {
                GUILayout.EndHorizontal();
            }
        }


        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            List<bool> list = new List<bool>();
            foreach(BroToggle b in BroToggle.BroToggles)
            {
                list.Add(b.enabled);
            }
            settings.brosEnable = list;
            settings.Save(modEntry);
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }

        public static void Log(object msg)
        {
            mod.Logger.Log(msg.ToString());
        }
        public static void Log(object msg, Exception ex)
        {
           Log(msg + "\n" + ex);
        }

        private static void SelectAll()
        {
            foreach(BroToggle b in BroToggle.BroToggles)
            {
                b.enabled = true;
            }

        }

        private static void SelectNothing()
        {
            foreach (BroToggle b in BroToggle.BroToggles)
            {
                b.enabled = false;
            }

        }

        private static void SelectOnlyExpendabros()
        {
            foreach (BroToggle b in BroToggle.BroToggles)
            {
                b.enabled = b.group == BroToggle.BroGroup.Expendabros;
            }

        }

        internal static Dictionary<int, HeroType> UpdateDictionary()
        {
            Dictionary<int, HeroType> broDict = new Dictionary<int, HeroType>();
            try
            {
                foreach (HeroType hero in heroList)
                {
                    BroToggle broToggle = BroToggle.GetBroToggleFromHeroType(hero);
                    if (broToggle.enabled)
                    {
                        broDict.Add(broToggle.unlockNumber, hero);
                    }
                }
            }
            catch (Exception ex)
            {
                Main.Log("Failed to update dictionary", ex);
            }
            return broDict;
        }
    }

    public class Settings : UnityModManager.ModSettings
    {
        public int numberOfBroPerLine = 8;
        public int maxLifeNumber = 0;
        public bool patchInCustomsLevel = false;
        public bool ignoreForcedBros = false;

        public List<bool> brosEnable;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}