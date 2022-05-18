using HarmonyLib;
using RocketLib0;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

namespace FilteredBros
{
    public static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        public static Dictionary<int, HeroType> originalDict = RocketLib._HeroUnlockController.Original_Unlock_Intervals;
        public static BroforceMod bmod;

        public static List<HeroType> heroList = new List<HeroType>();
        public static List<int> heroInt = new List<int>(RocketLib._HeroUnlockController.Hero_Unlock_Intervals);
        public static bool cheat;
        public static HeroType[] broforceBros = RocketLib._HeroUnlockController.HeroTypes_Intervals;
        public static HeroType[] expendabrosBros = RocketLib._HeroUnlockController.Expendabros_HeroTypes_Intervals;
        public static HeroType[] otherBros = RocketLib._HeroUnlockController.Other_Bros_HeroTypes;
        public static List<HeroType> broList = new List<HeroType>();

        private static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            settings = Settings.Load<Settings>(modEntry);

            mod = modEntry;

            var harmony = new Harmony(modEntry.Info.Id);
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);
            }
            catch (Exception ex)
            {
                mod.Logger.Log("Failed to Patch Harmony !\n" + ex.ToString());
            }
            cheat = Environment.UserName == "Gorzon";
            if (!settings.firstLaunch)
                firstLaunch();
            Start();
            return true;
        }

        private static void Start()
        {
            bmod = new BroforceMod();
            bmod.Load(mod);

            heroList.AddRange(broforceBros);
            heroList.AddRange(expendabrosBros);
            heroList.AddRange(otherBros);
            BuildBroToggles(broforceBros, BroToggle.BroGroup.Broforce);
            BuildBroToggles(expendabrosBros, BroToggle.BroGroup.Expendabros);
            BuildBroToggles(otherBros, BroToggle.BroGroup.Hide);

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

            new GamePassword("iaminthematrix", InTheMatrix);
        }

        private static void BuildBroToggles(HeroType[] heroArray, BroToggle.BroGroup group)
        {
            Dictionary<int, HeroType> dict = new Dictionary<int, HeroType>(originalDict);
            foreach (HeroType hero in heroArray)
            {
                try
                {
                    int interval = -1;
                    foreach (KeyValuePair<int, HeroType> pair in dict)
                    {
                        if (pair.Value == hero)
                        {
                            interval = pair.Key;
                            dict.Remove(pair.Key);
                            break;
                        }
                    }
                    if(interval == -1)
                    {
                        interval = 490;
                        foreach (BroToggle broToggle in BroToggle.BroToggles)
                        {
                            if(broToggle.unlockNumber >= interval)
                            {
                                interval = broToggle.unlockNumber + 10;
                                while (broToggle.unlockNumber >= interval)
                                {
                                    interval += 10;
                                }
                            }
                        }
                    }
                    new BroToggle(hero, interval, group);
                }
                catch (Exception ex)
                {
                    Main.bmod.Log(ex);
                }
            }
        }

        private static void InTheMatrix()
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
            GUILayout.Label("I am in The Matrix", s);
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

            /*GUILayout.BeginHorizontal();
            GUILayout.Label("Number of bro saved : " + PlayerProgress.Instance.freedBros);
            GUILayout.EndHorizontal();*/
            int numberOfRescuesToNextUnlock = HeroUnlockController.GetNumberOfRescuesToNextUnlock();
            if (numberOfRescuesToNextUnlock != -1)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(String.Format("Next unlock in {0} saves", HeroUnlockController.GetNumberOfRescuesToNextUnlock()));
                GUILayout.EndHorizontal();
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
            if (cheat)
            {
                GUILayout.Space(25);
                GUILayout.BeginHorizontal("box");
                GUILayout.Label(" - Secrets :", typeStyle);
                GUILayout.EndHorizontal();
                ShowToggles(BroToggle.broTogglesHide);
            }
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

        private static void firstLaunch()
        {
            SelectAll();
            settings.firstLaunch = true;
            settings.numberOfBroPerLine = 8;
            settings.brosEnable = new List<bool>();
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

        internal static Dictionary<int, HeroType> UpdateDico()
        {
            Dictionary<int, HeroType> BroDico = new Dictionary<int, HeroType>();
            try
            {
                int i = 0;
                foreach (HeroType hero in heroList)
                {
                    if (GetBroBool(hero) && !BroDico.ContainsValue(hero))
                    {
                        if(IsBroUnlock(hero) || Main.cheat)
                        {
                            while(BroDico.ContainsKey(heroInt[i]))
                            {
                                i++;
                            }
                            BroDico.Add(heroInt[i], hero);
                        }
                        else
                        {
                            BroToggle b = BroToggle.GetBroToggleFromHeroType(hero);
                            if(BroDico.ContainsKey(b.unlockNumber))
                            {
                                Main.bmod.Log("Keys already exist for hero : " + hero.ToString());
                            }
                            else
                            {
                                BroDico.Add(b.unlockNumber, hero);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Main.bmod.logger.ExceptionLog("Failed to update dictionary", ex);
            }
            return BroDico;
        }

        private static bool GetBroBool(HeroType broType)
        {
            foreach(BroToggle b in BroToggle.BroToggles)
            {
                if(b.heroType == broType)
                {
                    return b.enabled;
                }
            }
            return false;
        }

        private static bool IsBroUnlock(HeroType broType)
        {
            foreach(BroToggle b in BroToggle.BroToggles)
            {
                if(b.heroType == broType)
                {
                    return b.IsBroUnlocked();
                }
            }
            return false;
        }
    }

    public class Settings : UnityModManager.ModSettings
    {
        public bool firstLaunch;
        public bool useExpandaMod;
        public int numberOfBro;
        public int numberOfBroPerLine;
        public int maxLifeNumber;

        public List<bool> brosEnable;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }

    // Patch Hero unlock intervals for spawn
    [HarmonyPatch(typeof(HeroUnlockController), "IsAvailableInCampaign")]
    internal static class HeroUnlockController_IsAvailableInCampaign_Patch
    {
        private static void Prefix()
        {
            if (!Main.enabled) return;

            try
            {
                Dictionary<int, HeroType> HeroUnlockIntervals = new Dictionary<int, HeroType>();
                if (Main.enabled)
                {
                    HeroUnlockIntervals = Main.UpdateDico();
                }
                if (HeroUnlockIntervals.Count > 0 || HeroUnlockIntervals.ContainsKey(0))
                {
                    Traverse.Create(typeof(HeroUnlockController)).Field("_heroUnlockIntervals").SetValue(HeroUnlockIntervals);
                }
                else
                {
                    Main.bmod.logger.WarningLog("You have selected 0 bro, please select at least one. (The one who are name \"???\" don't count)");
                    HeroUnlockIntervals = Main.originalDict;
                }
            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog("Failed to patch the Unlock intervals", ex); }
        }
    }

    [HarmonyPatch(typeof(Player), "AddLife")]
    static class LifeNumberLimited_Patch
    {
        static void Postfix(Player __instance)
        {
            try
            {
                if(Main.enabled && Main.settings.maxLifeNumber != 0)
                {
                    while(__instance.Lives > Main.settings.maxLifeNumber)
                    {
                        __instance.Lives--;
                    }
                }
            }
            catch(Exception ex)
            {
                Main.bmod.Log(ex);
            }
        }
    }
}