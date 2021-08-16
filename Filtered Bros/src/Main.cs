using System;
using System.Collections.Generic;
using UnityEngine;
using UnityModManagerNet;

namespace FilteredBros
{
    using RocketLib;
    public class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        public static List<HeroType> HeroList = RocketLib._HeroUnlockController.HeroTypeFullList;
        public static List<int> heroInt = new List<int> { 1, 3, 5, 8, 11, 15, 20, 25, 37, 42, 46, 52, 56, 62, 65, 72, 75, 82, 87, 92, 99, 102, 115, 123, 132, 145, 160, 175, 193, 209, 222, 249, 274, 300, 326, 350, 374, 400, 425, 445, 465, 485, 520 };


        static bool Load(UnityModManager.ModEntry modEntry)
        {
            mod = modEntry;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            modEntry.OnUpdate = OnUpdate;
            settings = Settings.Load<Settings>(modEntry);

            if (!settings.getFirstLaunch)
                firstLaunch();

            return true;
        }

        static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            settings.numberOfBro = 0;
            foreach(HeroType hero in HeroList)
            {
                if (GetBroBool(hero)) settings.numberOfBro++;
            }

            Dictionary<int, HeroType> NewDictionary = Main.UpdateList();
            if (NewDictionary.Count > 0) { RocketLib._HeroUnlockController.SetHeroUnlockIntervals(NewDictionary); }
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Number of bro select : " + settings.numberOfBro);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Select all", GUILayout.ExpandWidth(false)))
                SelectAllBasic();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Remove Brolander", GUILayout.ExpandWidth(false)))
                RemoveBrolander();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Select nothing", GUILayout.ExpandWidth(false)))
                DeselectAllBasic();
            GUILayout.EndHorizontal();

            var typeStyle = new GUIStyle();
            typeStyle.normal.textColor = Color.gray;
            typeStyle.fontSize = 15;
            typeStyle.fontStyle = FontStyle.Bold;

            // Broforce Basic
            GUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(false));
            GUILayout.Label(" - Broforce :", typeStyle);
            GUILayout.EndHorizontal(); GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(true));
            settings.Rambro = GUILayout.Toggle(settings.Rambro, HeroController.GetHeroName(HeroType.Rambro));
            settings.Brommando = GUILayout.Toggle(settings.Brommando, HeroController.GetHeroName(HeroType.Brommando));
            settings.BaBroracus = GUILayout.Toggle(settings.BaBroracus, HeroController.GetHeroName(HeroType.BaBroracus));
            settings.BrodellWalker = GUILayout.Toggle(settings.BrodellWalker, HeroController.GetHeroName(HeroType.BrodellWalker));
            settings.BroHard = GUILayout.Toggle(settings.BroHard, HeroController.GetHeroName(HeroType.BroHard));
            settings.McBrover = GUILayout.Toggle(settings.McBrover, HeroController.GetHeroName(HeroType.McBrover));
            settings.Blade = GUILayout.Toggle(settings.Blade, HeroController.GetHeroName(HeroType.Blade));
            settings.BroDredd = GUILayout.Toggle(settings.BroDredd, HeroController.GetHeroName(HeroType.BroDredd));
            settings.Brononymous = GUILayout.Toggle(settings.Brononymous, HeroController.GetHeroName(HeroType.Brononymous));
            settings.DirtyHarry = GUILayout.Toggle(settings.DirtyHarry, HeroController.GetHeroName(HeroType.DirtyHarry));
            settings.Brominator = GUILayout.Toggle(settings.Brominator, HeroController.GetHeroName(HeroType.Brominator));
            settings.Brobocop = GUILayout.Toggle(settings.Brobocop, HeroController.GetHeroName(HeroType.Brobocop));
            settings.IndianaBrones = GUILayout.Toggle(settings.IndianaBrones, HeroController.GetHeroName(HeroType.IndianaBrones));
            settings.AshBrolliams = GUILayout.Toggle(settings.AshBrolliams, HeroController.GetHeroName(HeroType.AshBrolliams));
            GUILayout.EndHorizontal(); GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));

            settings.Nebro = GUILayout.Toggle(settings.Nebro, HeroController.GetHeroName(HeroType.Nebro), GUILayout.ExpandWidth(false));
            settings.BoondockBros = GUILayout.Toggle(settings.BoondockBros, HeroController.GetHeroName(HeroType.BoondockBros), GUILayout.ExpandWidth(false));
            settings.Brochete = GUILayout.Toggle(settings.Brochete, HeroController.GetHeroName(HeroType.Brochete), GUILayout.ExpandWidth(false));
            settings.BronanTheBrobarian = GUILayout.Toggle(settings.BronanTheBrobarian, HeroController.GetHeroName(HeroType.BronanTheBrobarian), GUILayout.ExpandWidth(false));
            settings.EllenRipbro = GUILayout.Toggle(settings.EllenRipbro, HeroController.GetHeroName(HeroType.EllenRipbro), GUILayout.ExpandWidth(false));
            settings.TheBrocketeer = GUILayout.Toggle(settings.TheBrocketeer, HeroController.GetHeroName(HeroType.TheBrocketeer), GUILayout.ExpandWidth(false));
            settings.TimeBroVanDamme = GUILayout.Toggle(settings.TimeBroVanDamme, HeroController.GetHeroName(HeroType.TimeBroVanDamme), GUILayout.ExpandWidth(false));
            settings.BroniversalSoldier = GUILayout.Toggle(settings.BroniversalSoldier, HeroController.GetHeroName(HeroType.BroniversalSoldier), GUILayout.ExpandWidth(false));
            settings.ColJamesBroddock = GUILayout.Toggle(settings.ColJamesBroddock, HeroController.GetHeroName(HeroType.ColJamesBroddock), GUILayout.ExpandWidth(false));
            settings.CherryBroling = GUILayout.Toggle(settings.CherryBroling, HeroController.GetHeroName(HeroType.CherryBroling), GUILayout.ExpandWidth(false));
            settings.BroMax = GUILayout.Toggle(settings.BroMax, HeroController.GetHeroName(HeroType.BroMax), GUILayout.ExpandWidth(false));
            settings.DoubleBroSeven = GUILayout.Toggle(settings.DoubleBroSeven, HeroController.GetHeroName(HeroType.DoubleBroSeven), GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal(); GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));

            settings.TheBrode = GUILayout.Toggle(settings.TheBrode, HeroController.GetHeroName(HeroType.TheBrode), GUILayout.ExpandWidth(false));
            settings.Predabro = GUILayout.Toggle(settings.Predabro, HeroController.GetHeroName(HeroType.Predabro), GUILayout.ExpandWidth(false));
            settings.BroveHeart = GUILayout.Toggle(settings.BroveHeart, HeroController.GetHeroName(HeroType.BroveHeart), GUILayout.ExpandWidth(false));
            settings.TheBrofessional = GUILayout.Toggle(settings.TheBrofessional, HeroController.GetHeroName(HeroType.TheBrofessional), GUILayout.ExpandWidth(false));
            settings.Broden = GUILayout.Toggle(settings.Broden, HeroController.GetHeroName(HeroType.Broden), GUILayout.ExpandWidth(false));
            settings.TheBrolander = GUILayout.Toggle(settings.TheBrolander, HeroController.GetHeroName(HeroType.TheBrolander), GUILayout.ExpandWidth(false));
            settings.SnakeBroSkin = GUILayout.Toggle(settings.SnakeBroSkin, HeroController.GetHeroName(HeroType.SnakeBroSkin), GUILayout.ExpandWidth(false));
            settings.TankBro = GUILayout.Toggle(settings.TankBro, HeroController.GetHeroName(HeroType.TankBro), GUILayout.ExpandWidth(false));
            settings.BroLee = GUILayout.Toggle(settings.BroLee, HeroController.GetHeroName(HeroType.BroLee), GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal(); GUILayout.Space(25);

            // - Expendabros :
            GUILayout.BeginHorizontal("box");
            GUILayout.Label(" - Expendabros :", typeStyle);
            GUILayout.EndHorizontal(); GUILayout.BeginHorizontal();
            settings.BroneyRoss = GUILayout.Toggle(settings.BroneyRoss, HeroController.GetHeroName(HeroType.BroneyRoss), GUILayout.ExpandWidth(false));
            settings.LeeBroxmas = GUILayout.Toggle(settings.LeeBroxmas, HeroController.GetHeroName(HeroType.LeeBroxmas), GUILayout.ExpandWidth(false));
            settings.BronnarJensen = GUILayout.Toggle(settings.BronnarJensen, HeroController.GetHeroName(HeroType.BronnarJensen), GUILayout.ExpandWidth(false));
            settings.HaleTheBro = GUILayout.Toggle(settings.HaleTheBro, HeroController.GetHeroName(HeroType.HaleTheBro), GUILayout.ExpandWidth(false));
            settings.TrentBroser = GUILayout.Toggle(settings.TrentBroser, HeroController.GetHeroName(HeroType.TrentBroser), GUILayout.ExpandWidth(false));
            settings.Broc = GUILayout.Toggle(settings.Broc, HeroController.GetHeroName(HeroType.Broc), GUILayout.ExpandWidth(false));
            settings.TollBroad = GUILayout.Toggle(settings.TollBroad, HeroController.GetHeroName(HeroType.TollBroad), GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal(); GUILayout.Space(25);

            // - The ? :
            GUILayout.BeginHorizontal("box");
            GUILayout.Label(" - The ? :", typeStyle);
            GUILayout.EndHorizontal(); GUILayout.BeginHorizontal();
            settings.BrondleFly = GUILayout.Toggle(settings.BrondleFly, HeroController.GetHeroName(HeroType.BrondleFly), GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();
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

        public static void Log(object str, RLogType type = RLogType.Log)
        {
            RocketLib.ScreenLogger.Log(str, type);
        }

        static void firstLaunch()
        {
            settings.Rambro = true;
            settings.Brommando = true;
            settings.BaBroracus = true;
            settings.BrodellWalker = true;
            settings.BroHard = true;
            settings.McBrover = true;
            settings.Blade = true;
            settings.BroDredd = true;
            settings.Brononymous = true;
            settings.SnakeBroSkin = true;
            settings.Brominator = true;
            settings.Brobocop = true;
            settings.IndianaBrones = true;
            settings.AshBrolliams = true;
            settings.Nebro = true;
            settings.BoondockBros = true;
            settings.Brochete = true;
            settings.BronanTheBrobarian = true;
            settings.EllenRipbro = true;
            settings.TimeBroVanDamme = true;
            settings.BroniversalSoldier = true;
            settings.ColJamesBroddock = true;
            settings.CherryBroling = true;
            settings.BroMax = true;
            settings.TheBrode = true;
            settings.DoubleBroSeven = true;
            settings.Predabro = true;
            settings.TheBrocketeer = true;
            settings.BroveHeart = true;
            settings.TheBrofessional = true;
            settings.Broden = true;
            settings.TheBrolander = true;
            settings.DirtyHarry = true;
            settings.TankBro = true;
            settings.BroLee = true;

            // Expendabros
            settings.BroneyRoss = false;
            settings.LeeBroxmas = false;
            settings.BronnarJensen = false;
            settings.HaleTheBro = false;
            settings.TrentBroser = false;
            settings.Broc = false;
            settings.TollBroad = false;

            // The ?
            settings.BrondleFly = false;

            settings.getFirstLaunch = true;
        }

        static void RemoveBrolander()
        {
            settings.TheBrolander = false;
        }
        static void SelectAllBasic()
        {
            settings.Rambro = true;
            settings.Brommando = true;
            settings.BaBroracus = true;
            settings.BrodellWalker = true;
            settings.BroHard = true;
            settings.McBrover = true;
            settings.Blade = true;
            settings.BroDredd = true;
            settings.Brononymous = true;
            settings.SnakeBroSkin = true;
            settings.Brominator = true;
            settings.Brobocop = true;
            settings.IndianaBrones = true;
            settings.AshBrolliams = true;
            settings.Nebro = true;
            settings.BoondockBros = true;
            settings.Brochete = true;
            settings.BronanTheBrobarian = true;
            settings.EllenRipbro = true;
            settings.TimeBroVanDamme = true;
            settings.BroniversalSoldier = true;
            settings.ColJamesBroddock = true;
            settings.CherryBroling = true;
            settings.BroMax = true;
            settings.TheBrode = true;
            settings.DoubleBroSeven = true;
            settings.Predabro = true;
            settings.TheBrocketeer = true;
            settings.BroveHeart = true;
            settings.TheBrofessional = true;
            settings.Broden = true;
            settings.TheBrolander = true;
            settings.DirtyHarry = true;
            settings.TankBro = true;
            settings.BroLee = true;
        }
        static void DeselectAllBasic()
        {
            settings.Rambro = false;
            settings.Brommando = false;
            settings.BaBroracus = false;
            settings.BrodellWalker = false;
            settings.BroHard = false;
            settings.McBrover = false;
            settings.Blade = false;
            settings.BroDredd = false;
            settings.Brononymous = false;
            settings.SnakeBroSkin = false;
            settings.Brominator = false;
            settings.Brobocop = false;
            settings.IndianaBrones = false;
            settings.AshBrolliams = false;
            settings.Nebro = false;
            settings.BoondockBros = false;
            settings.Brochete = false;
            settings.BronanTheBrobarian = false;
            settings.EllenRipbro = false;
            settings.TimeBroVanDamme = false;
            settings.BroniversalSoldier = false;
            settings.ColJamesBroddock = false;
            settings.CherryBroling = false;
            settings.BroMax = false;
            settings.TheBrode = false;
            settings.DoubleBroSeven = false;
            settings.Predabro = false;
            settings.TheBrocketeer = false;
            settings.BroveHeart = false;
            settings.TheBrofessional = false;
            settings.Broden = false;
            settings.TheBrolander = false;
            settings.DirtyHarry = false;
            settings.TankBro = false;
            settings.BroLee = false;
        }

        public static Dictionary<int, HeroType> UpdateList()
        {
            Dictionary<int, HeroType> BroDico = new Dictionary<int, HeroType>();
            try
            {
                int i = 0;
                foreach (HeroType hero in HeroList)
                {
                    bool broBool = GetBroBool(hero);
                    if(broBool & !BroDico.ContainsValue(hero))
                    {
                        BroDico.Add(heroInt[i], hero);
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                Main.Log(ex);
            }
            return BroDico;
        }

        static bool GetBroBool(HeroType broName)
        {
            switch (broName)
            {
                case HeroType.AshBrolliams: return settings.AshBrolliams;
                case HeroType.BaBroracus: return settings.BaBroracus;
                case HeroType.Blade: return settings.Blade;
                case HeroType.BoondockBros: return settings.BoondockBros;
                case HeroType.Brobocop: return settings.Brobocop;
                case HeroType.Broc: return settings.Broc;
                case HeroType.Brochete: return settings.Brochete;
                case HeroType.BrodellWalker: return settings.BrodellWalker;
                case HeroType.Broden: return settings.Broden;
                case HeroType.BroDredd: return settings.BroDredd;
                case HeroType.BroHard: return settings.BroHard;
                case HeroType.BroLee: return settings.BroLee;
                case HeroType.BroMax: return settings.BroMax;
                case HeroType.Brominator: return settings.Brominator;
                case HeroType.Brommando: return settings.Brommando;
                case HeroType.BronanTheBrobarian: return settings.BronanTheBrobarian;
                case HeroType.BrondleFly: return settings.BrondleFly;
                case HeroType.BroneyRoss: return settings.BroneyRoss;
                case HeroType.BroniversalSoldier: return settings.BroniversalSoldier;
                case HeroType.BronnarJensen: return settings.BronnarJensen;
                case HeroType.Brononymous: return settings.Brononymous;
                case HeroType.BroveHeart: return settings.BroveHeart;
                case HeroType.CherryBroling: return settings.CherryBroling;
                case HeroType.ColJamesBroddock: return settings.ColJamesBroddock;
                case HeroType.DirtyHarry: return settings.DirtyHarry;
                case HeroType.DoubleBroSeven: return settings.DoubleBroSeven;
                case HeroType.EllenRipbro: return settings.EllenRipbro;
                case HeroType.HaleTheBro: return settings.HaleTheBro;
                case HeroType.IndianaBrones: return settings.IndianaBrones;
                case HeroType.LeeBroxmas: return settings.LeeBroxmas;
                case HeroType.McBrover: return settings.McBrover;
                case HeroType.Nebro: return settings.Nebro;
                case HeroType.Predabro: return settings.Predabro;
                case HeroType.Rambro: return settings.Rambro;
                case HeroType.SnakeBroSkin: return settings.SnakeBroSkin;
                case HeroType.TimeBroVanDamme: return settings.TimeBroVanDamme;
                case HeroType.TollBroad: return settings.TollBroad;
                case HeroType.TankBro: return settings.TankBro;
                case HeroType.TheBrocketeer: return settings.TheBrocketeer;
                case HeroType.TheBrode: return settings.TheBrode;
                case HeroType.TheBrofessional: return settings.TheBrofessional;
                case HeroType.TheBrolander: return settings.TheBrolander;
                case HeroType.TrentBroser: return settings.TrentBroser;
            }
            return false;
        }
    }

    public class Settings : UnityModManager.ModSettings
    {
        public bool Brommando;
        public bool BaBroracus;
        public bool BrodellWalker;
        public bool BroHard;
        public bool McBrover;
        public bool Blade;
        public bool BroDredd;
        public bool Brononymous;
        public bool SnakeBroSkin;
        public bool Brominator;
        public bool Brobocop;
        public bool IndianaBrones;
        public bool AshBrolliams;
        public bool Nebro;
        public bool BoondockBros;
        public bool Brochete;
        public bool BronanTheBrobarian;
        public bool EllenRipbro;
        public bool TimeBroVanDamme;
        public bool BroniversalSoldier;
        public bool ColJamesBroddock;
        public bool CherryBroling;
        public bool BroMax;
        public bool TheBrode;
        public bool DoubleBroSeven;
        public bool Predabro;
        public bool TheBrocketeer;
        public bool BroveHeart;
        public bool TheBrofessional;
        public bool Broden;
        public bool TheBrolander;
        public bool DirtyHarry;
        public bool TankBro;
        public bool BroLee;
        public bool Rambro;

        // Expendabros
        public bool BroneyRoss;
        public bool LeeBroxmas;
        public bool BronnarJensen;
        public bool HaleTheBro;
        public bool TrentBroser;
        public bool Broc;
        public bool BrondleFly;
        public bool TollBroad;

        public bool getFirstLaunch;
        public int numberOfBro;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
