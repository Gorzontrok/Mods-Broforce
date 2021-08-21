using System;
using System.Collections.Generic;
using System.IO;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace ForBralef
{
    static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        public static List<HeroType> origHeroList = new List<HeroType> { HeroType.Rambro, HeroType.Brommando, HeroType.BaBroracus, HeroType.BrodellWalker, HeroType.BroHard, HeroType.McBrover, HeroType.Blade, HeroType.BroDredd, HeroType.Brononymous, HeroType.DirtyHarry, HeroType.Brominator, HeroType.Brobocop, HeroType.IndianaBrones, HeroType.AshBrolliams, HeroType.Nebro, HeroType.BoondockBros, HeroType.Brochete, HeroType.BronanTheBrobarian, HeroType.EllenRipbro, HeroType.TheBrocketeer, HeroType.TimeBroVanDamme, HeroType.BroniversalSoldier, HeroType.ColJamesBroddock, HeroType.CherryBroling, HeroType.BroMax, HeroType.TheBrode, HeroType.DoubleBroSeven, HeroType.Predabro, HeroType.BroveHeart, HeroType.TheBrofessional, HeroType.Broden, HeroType.TheBrolander, HeroType.SnakeBroSkin, HeroType.TankBro, HeroType.BroLee, HeroType.Broc, HeroType.BroneyRoss, HeroType.BronnarJensen, HeroType.HaleTheBro, HeroType.LeeBroxmas, HeroType.TollBroad, HeroType.TrentBroser, HeroType.BrondleFly, };

        public static List<HeroType> HeroList = new List<HeroType> { HeroType.Rambro, HeroType.Brommando, HeroType.BaBroracus, HeroType.BrodellWalker, HeroType.BroHard, HeroType.McBrover, HeroType.Blade, HeroType.BroDredd, HeroType.Brononymous, HeroType.DirtyHarry, HeroType.Brominator, HeroType.Brobocop, HeroType.IndianaBrones, HeroType.AshBrolliams, HeroType.Nebro, HeroType.BoondockBros, HeroType.Brochete, HeroType.BronanTheBrobarian, HeroType.EllenRipbro, HeroType.TheBrocketeer, HeroType.TimeBroVanDamme, HeroType.BroniversalSoldier, HeroType.ColJamesBroddock, HeroType.CherryBroling, HeroType.BroMax, HeroType.TheBrode, HeroType.DoubleBroSeven, HeroType.Predabro, HeroType.BroveHeart, HeroType.TheBrofessional, HeroType.Broden, HeroType.TheBrolander, HeroType.SnakeBroSkin, HeroType.TankBro, HeroType.BroLee, HeroType.Broc, HeroType.BroneyRoss, HeroType.BronnarJensen, HeroType.HaleTheBro, HeroType.LeeBroxmas, HeroType.TollBroad, HeroType.TrentBroser, HeroType.BrondleFly, };
        public static List<int> HeroInt = new List<int> { 0, 1, 3, 5, 8, 11, 15, 20, 25, 37, 42, 46, 52, 56, 62, 65, 72, 75, 82, 87, 92, 99, 102, 115, 123, 132, 145, 160, 175, 193, 209, 222, 249, 274, 300, 326, 350, 374, 400, 425, 445, 465, 485, 520 };
        public static Dictionary<int, HeroType> HeroDictionary = new Dictionary<int, HeroType>();

        public static bool NeedToRedoneDictionary = false;
        public static int CurrentNbrOfBro;

        private static System.Random rng = new System.Random();

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            modEntry.OnUpdate = OnUpdate;
            settings = Settings.Load<Settings>(modEntry);
            
            mod = modEntry;

            if (!settings.GetFirstLaunch) FirstLaunch();
            CurrentNbrOfBro = settings.MaxNumberOfBro;

            if (settings.RememberDictionnary) 
            { 
                HeroList = settings.HeroListSave;
                HeroInt = settings.HeroIntSave;
                BuildTheDictionary(); 
            }

            TestVanDammeAnim.teaBagCheatEnabled = true;

            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            var LabelWarning = new GUIStyle();
            LabelWarning.normal.textColor = Color.yellow;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Unlock all")) HeroUnlockController.UnlockAllBros();
            if (GUILayout.Button("Clear Unlock")) HeroUnlockController.ClearUnlocks();
            GUILayout.Space(5);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Show the current bros order in log")) ShowCurrentDictionnary();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Max number of bro : " + settings.MaxNumberOfBro.ToString(), GUILayout.ExpandWidth(false));
            settings.MaxNumberOfBro = (int)GUILayout.HorizontalScrollbar(settings.MaxNumberOfBro, 1f, 1f, origHeroList.Count + 1);
            GUILayout.Space(50);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Shuffle dictionnary")) ShuffleList();
            if (GUILayout.Button("Random Unlock Intervals.")) RandomIntervalsInt();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            if (NeedToRedoneDictionary) GUILayout.Label("NEED TO BUILD DICTIONARY", LabelWarning);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Build Dictionary")) BuildTheDictionary();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            settings.RememberDictionnary = GUILayout.Toggle(settings.RememberDictionnary, "Remember dictionary");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
        }

        static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            if (settings.MaxNumberOfBro != CurrentNbrOfBro) NeedToRedoneDictionary = true;
            if (settings.MaxNumberOfBro == CurrentNbrOfBro) NeedToRedoneDictionary = false;

            if (!settings.RememberDictionnary)
            {
                settings.HeroListSave = new List<HeroType>();
                settings.HeroIntSave = new List<int>();
            }
            if (settings.RememberDictionnary)
            {
                settings.HeroListSave = HeroList;
                settings.HeroIntSave = HeroInt;
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

        static void FirstLaunch()
        {
            settings.MaxNumberOfBro = HeroList.Count;
            settings.GetFirstLaunch = true;
        }

        static void ShuffleList() 
        { 
            HeroList.Shuffle();
            Main.Log("Hero list shuffle ! Build dictionary know !!");
            NeedToRedoneDictionary = true;
        }

        static void RandomIntervalsInt()
        {
            for(int i = 1; i< HeroInt.Count; i++)
            {
                int temp = 1;
                while(HeroInt.Contains(temp))
                {
                    temp = rng.Next(2, 1000);
                }
                HeroInt[i] = temp;
            }
            HeroInt.Sort();
            Main.Log("Intervals list shuffle ! Build dictionary know !!");
            NeedToRedoneDictionary = true;
        }

        static void BuildTheDictionary()
        {
            HeroDictionary.Clear();
            CurrentNbrOfBro = settings.MaxNumberOfBro;
            int i = 0;
            try
            {
                foreach (HeroType Hero in HeroList)
                {
                    if(i < settings.MaxNumberOfBro)
                    {
                        HeroDictionary.Add(HeroInt[i], Hero);
                        i++;
                    }
                }
            }
            catch (Exception ex) { Main.Log("Failed to build Dictionnary !\n\t" + ex); return; }
            Main.Log("Finish to create Dictionary.");

            try
            {
                Traverse.Create(typeof(HeroUnlockController)).Field("_heroUnlockIntervals").SetValue(HeroDictionary);
            }
            catch (Exception ex) { Main.Log("Failed to assign dictionnary ! \n\t" + ex); return; }

            NeedToRedoneDictionary = false;
        }
        
        static void ShowCurrentDictionnary()
        {
            if (HeroDictionary.Count < 1)
            {
                Main.Log("No shuffle dictionnary, show the one from the game !");
            }
            Dictionary<int, HeroType> HeroUnlockIntervals = Traverse.Create(typeof(HeroUnlockController)).Field("_heroUnlockIntervals").GetValue() as Dictionary<int, HeroType>;

            Main.Log("Start to show intervals....");
            string Table = "\n\n int, bro\n";
            foreach (KeyValuePair<int, HeroType> Hero in HeroUnlockIntervals)
            {
                Table += (" " + Hero.Key + ",  " + HeroController.GetHeroName(Hero.Value)+"\n");
            }
            Main.Log(Table);
            Main.Log("Finish to show unlock order");
        }

        // Add this to RocketLib
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    public class Settings : UnityModManager.ModSettings
    {
        public int MaxNumberOfBro;
        public bool GetFirstLaunch;

        public bool RememberDictionnary;
        public List<HeroType> HeroListSave;
        public List<int> HeroIntSave;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
