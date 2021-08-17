using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace CustomHeroName
{
using RocketLib;
    static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            mod = modEntry;

            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;

            settings = Settings.Load<Settings>(modEntry);
            try
            {
                var harmony = new Harmony(modEntry.Info.Id);
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);
            }
            catch (Exception ex)
            {
                mod.Logger.Log("Failed to patch with Harmony !\n" + ex.ToString());
            }


            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("View name"))
            {
                foreach(HeroType hero in RocketLib._HeroUnlockController.HeroTypeFullList)
                    Main.Log(GetBasicNameInTxt(hero)+"\t"+GetNewName(hero));
            }
            GUILayout.FlexibleSpace();
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

        public static void Log(object str)
        {
            mod.Logger.Log(str.ToString());
        }

        static string GetBasicNameInTxt(HeroType Hero)
        {
            switch (Hero)
            {
                case HeroType.AshBrolliams: return "AshBrolliams";
                case HeroType.BaBroracus: return "B.A.Broracus";
                case HeroType.Blade: return "Brade";
                case HeroType.BoondockBros: return "BoondockBros";
                case HeroType.Brobocop: return "Brobocop";
                case HeroType.Brochete: return "Brochete";
                case HeroType.BrodellWalker: return "BrodellWalker";
                case HeroType.Broden: return "Broden";
                case HeroType.BroDredd: return "BroDredd";
                case HeroType.BroHard: return "BroHard";
                case HeroType.BroLee: return "BroLee";
                case HeroType.BroMax: return "BroMax";
                case HeroType.Brominator: return "Brominator";
                case HeroType.Brommando: return "Brommando";
                case HeroType.BronanTheBrobarian: return "BronanTheBrobarian";
                case HeroType.BroniversalSoldier: return "BroniversalSoldier";
                case HeroType.Brononymous: return "BroInBlack";
                case HeroType.BroveHeart: return "BroHeart";
                case HeroType.CherryBroling: return "CherryBroling";
                case HeroType.ColJamesBroddock: return "Col.JamesBroddock";
                case HeroType.DirtyHarry: return "DirtyBrody";
                case HeroType.DoubleBroSeven: return "DoubleBroSeven";
                case HeroType.EllenRipbro: return "EllenRipbro";
                case HeroType.IndianaBrones: return "IndianaBrones";
                case HeroType.McBrover: return "MacBrover";
                case HeroType.Nebro: return "MrAnderbro";
                case HeroType.Predabro: return "TheBrodator";
                case HeroType.Rambro: return "Rambro";
                case HeroType.SnakeBroSkin: return "SnakeBroSkin";
                case HeroType.TankBro: return "TankBro";
                case HeroType.TheBrocketeer: return "TheBrocketeer";
                case HeroType.TheBrode: return "TheBrode";
                case HeroType.TheBrofessional: return "TheBrofessional";
                case HeroType.TheBrolander: return "TheBrolander";
                case HeroType.TimeBroVanDamme: return "TimeBro";

                //Expendabros
                case HeroType.Broc: return "BroctorDeath";
                case HeroType.BroneyRoss: return "BroneyRoss";
                case HeroType.BronnarJensen: return "BronnarJensen";
                case HeroType.HaleTheBro: return "BroCaesar";
                case HeroType.LeeBroxmas: return "LeeBroxmas";
                case HeroType.TollBroad: return "TollBroad";
                case HeroType.TrentBroser: return "TrentBroser";

                case HeroType.BrondleFly: return "BrondleFly";
            }
            return "";
        }

        public static string GetBasicNameOnNull(HeroType type)
        {
            switch (type)
            {
                case HeroType.BaBroracus:
                    return "B.A. Broracus";
                case HeroType.BrodellWalker:
                    return "Brodell Walker";
                case HeroType.Blade:
                    return "Brade";
                case HeroType.McBrover:
                    return "MacBrover";
                case HeroType.Brononymous:
                    return "Bro In Black";
                case HeroType.BroDredd:
                    return "Bro Dredd";
                case HeroType.BroHard:
                    return "Bro Hard";
                case HeroType.SnakeBroSkin:
                    return "Snake Broskin";
                case HeroType.IndianaBrones:
                    return "Indiana Brones";
                case HeroType.AshBrolliams:
                    return "Ash Brolliams";
                case HeroType.Nebro:
                    return "Mr Anderbro";
                case HeroType.BoondockBros:
                    return "Boondock Bros";
                case HeroType.BronanTheBrobarian:
                    return "Bronan The Brobarian";
                case HeroType.EllenRipbro:
                    return "Ellen Ripbro";
                case HeroType.CherryBroling:
                    return "Cherry Broling";
                case HeroType.TimeBroVanDamme:
                    return "Time Bro";
                case HeroType.ColJamesBroddock:
                    return "Col. James Broddock";
                case HeroType.BroniversalSoldier:
                    return "Broniversal Soldier";
                case HeroType.BroneyRoss:
                    return "Broney Ross";
                case HeroType.LeeBroxmas:
                    return "Lee Broxmas";
                case HeroType.BronnarJensen:
                    return "Bronnar Jensen";
                case HeroType.HaleTheBro:
                    return "Bro Caesar";
                case HeroType.TrentBroser:
                    return "Trent Broser";
                case HeroType.Broc:
                    return "Broctor Death";
                case HeroType.TollBroad:
                    return "Toll Broad";
                case HeroType.TheBrode:
                    return "The Brode";
                case HeroType.BroMax:
                    return "Bro Max";
                case HeroType.DoubleBroSeven:
                    return "Double Bro Seven";
                case HeroType.Predabro:
                    return "The Brodator";
                case HeroType.TheBrofessional:
                    return "The Brofessional";
                case HeroType.BrondleFly:
                    return "Brondle Fly";
                case HeroType.BroveHeart:
                    return "Bro Heart";
                case HeroType.TheBrocketeer:
                    return "The Brocketeer";
                case HeroType.TankBro:
                    return "Tank Bro";
                case HeroType.TheBrolander:
                    return "The Brolander";
                case HeroType.BroLee:
                    return "Bro Lee";
                case HeroType.DirtyHarry:
                    return "Dirty Brody";
            }
            return type.ToString();
        }

        public static string GetNewName(HeroType Hero)
        {
            string NameToReturn = "";

            string[] lines = File.ReadAllLines(mod.Path + "CustomHeroName.txt");
            string TxtBasicName = GetBasicNameInTxt(Hero);

            foreach(string line in lines)
            {
                string[] words = line.Split();
                if(words.Length > 1)
                {
                    if(words[0] == TxtBasicName)
                    {
                        for (int i = 1; i < words.Length; i++)
                        {
                            NameToReturn += words[i] + " ";
                        }
                    }
                }
            }
            //Main.Log(NameToReturn);
            return NameToReturn;
        }

    }

    public class Settings : UnityModManager.ModSettings
    {
        public int FileSelect;
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

    }
    
    [HarmonyPatch(typeof(HeroController), "GetHeroName")]
    static class CustomName_Patch
    {
        public static void Postfix(ref HeroType type, ref string __result)
        {
            if (Main.enabled)
            {
                try
                {
                    __result = Main.GetNewName(type);

                    foreach(HeroType hero in RocketLib._HeroUnlockController.HeroTypeFullList)
                    {
                        if(Main.GetBasicNameOnNull(hero) == __result)
                        {
                            __result = Main.GetNewName(hero);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Main.Log(ex);
                }
            }

            if (String.IsNullOrEmpty(__result)) __result = Main.GetBasicNameOnNull(type);

        }
    }
}
