using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace CustomHeroName
{
    public static class Mod
    {
        public static string FilePath
        {
            get
            {
                return Path.Combine(Main.mod.Path, "Names.json");
            }
        }

        public static Dictionary<HeroType, HeroIntro> names = null;

        private static bool _deserializeError = false;

        public static void CheckFile()
        {
            try
            {
                if (!File.Exists(FilePath))
                    File.Create(FilePath);
            }
            catch (Exception e)
            {
                Main.Log(e);
            }
        }

        public static void Deserialize()
        {
            if (_deserializeError) return;
            try
            {
                CheckFile();

                names = new Dictionary<HeroType, HeroIntro>();
                Dictionary<string, HeroIntro> temp = JsonConvert.DeserializeObject<Dictionary<string, HeroIntro>>(File.ReadAllText(FilePath));
                foreach (KeyValuePair<string, HeroIntro> pair in temp)
                {
                    pair.Value.type = GetHeroType(pair.Key);
                    names.Add(GetHeroType(pair.Key), pair.Value);

                }
            }
            catch(Exception e)
            {
                Main.Log(e);
                _deserializeError = true;
            }
        }

        public static void CreateFile()
        {
            try
            {
                CheckFile();
                Dictionary<string, HeroIntro> temp = new Dictionary<string, HeroIntro>();
                foreach (var hero in RocketLib.Collections.Heroes.Playables)
                {
                    temp.Add(GetHeroKey(hero), new HeroIntro(hero));
                }
                string json = JsonConvert.SerializeObject(temp, Formatting.Indented);
                File.WriteAllText(FilePath, json);

            }
            catch (Exception ex)
            {
                Main.Log(ex);
            }
        }

        public static HeroIntro GetHeroIntro(HeroType type)
        {
            try
            {
                if (names.IsNullOrEmpty())
                    Deserialize();

                names.TryGetValue(type, out HeroIntro hero);
                return hero;
            }
            catch (Exception e)
            {
                Main.Log(e);
            }
            return null;
        }

        public static string GetHeroKey(HeroType Hero)
        {
            switch (Hero)
            {
                case HeroType.Blade: return "Brade";
                case HeroType.Brononymous: return "BroInBlack";
                case HeroType.BroveHeart: return "BroHeart";
                case HeroType.DirtyHarry: return "DirtyBrody";
                case HeroType.Nebro: return "MrAnderbro";
                case HeroType.Predabro: return "TheBrodator";
                case HeroType.TimeBroVanDamme: return "TimeBro";

                //Expendabros
                case HeroType.Broc: return "BroctorDeath";
                case HeroType.HaleTheBro: return "BroCaesar";

                default:
                    return Hero.ToString();
            }
        }

        public static HeroType GetHeroType(string name)
        {
            switch (name)
            {
                case "Brade": return HeroType.Blade;
                case "BroInBlack": return HeroType.Brononymous;
                case "BroHeart": return HeroType.BroveHeart;
                case "DirtyBrody": return HeroType.DirtyHarry;
                case "MrAnderbro": return HeroType.Nebro;
                case "TheBrodator": return HeroType.Predabro;
                case "TimeBro": return HeroType.TimeBroVanDamme;

                //Expendabros
                case "BroctorDeath": return HeroType.Broc;
                case "BroCaesar" : return HeroType.HaleTheBro;

                default:
                    return (HeroType)Enum.Parse(typeof(HeroType), name, true);
            }
        }

        // Original Method
        public static string GetHeroNameOriginal(HeroType type)
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
                    return "Seth Brondle";
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
                case HeroType.CaseyBroback:
                    return "Casey Broback";
                case HeroType.Xebro:
                    return "Xebro";
                case HeroType.ScorpionBro:
                    return "The Scorpion Bro";
                case HeroType.Broffy:
                    return "Broffy the Vampire Slayer";
                case HeroType.Desperabro:
                    return "Desperabro";
                case HeroType.BroGummer:
                    return "Bro Gummer";
                case HeroType.ChevBrolios:
                    return "Chev Brolios";
                case HeroType.DemolitionBro:
                    return "Demolition Bro";
            }
            return type.ToString();
        }
    }
}
