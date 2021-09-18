using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using RocketLibLoadMod;

namespace RocketLib
{
    public partial class RocketLib
    {
        /// <summary>
        /// 
        /// </summary>
        public static class _HeroUnlockController
        {
            /// <summary>
            /// It's the HeroType  list of the Broforce bros.
            /// </summary>
            public static List<HeroType> HeroTypeList = new List<HeroType> { HeroType.Rambro, HeroType.Brommando, HeroType.BaBroracus, HeroType.BrodellWalker, HeroType.BroHard, HeroType.McBrover, HeroType.Blade, HeroType.BroDredd, HeroType.Brononymous, HeroType.DirtyHarry, HeroType.Brominator, HeroType.Brobocop, HeroType.IndianaBrones, HeroType.AshBrolliams, HeroType.Nebro, HeroType.BoondockBros, HeroType.Brochete, HeroType.BronanTheBrobarian, HeroType.EllenRipbro, HeroType.TheBrocketeer, HeroType.TimeBroVanDamme, HeroType.BroniversalSoldier, HeroType.ColJamesBroddock, HeroType.CherryBroling, HeroType.BroMax, HeroType.TheBrode, HeroType.DoubleBroSeven, HeroType.Predabro, HeroType.BroveHeart, HeroType.TheBrofessional, HeroType.Broden, HeroType.TheBrolander, HeroType.SnakeBroSkin, HeroType.TankBro, HeroType.BroLee };
            /// <summary>
            /// It's the HeroType List of the Expendabros bros.
            /// </summary>
            public static List<HeroType> ExpendabrosHeroTypeList = new List<HeroType> { HeroType.BroneyRoss, HeroType.LeeBroxmas, HeroType.BronnarJensen, HeroType.HaleTheBro, HeroType.TrentBroser, HeroType.Broc, HeroType.TollBroad };
            /// <summary>
            /// It's the full HeroType list of the Broforce bros. Include Expendabros and Brond Fly.
            /// </summary>
            public static List<HeroType> HeroTypeFullList = new List<HeroType> { HeroType.Rambro, HeroType.Brommando, HeroType.BaBroracus, HeroType.BrodellWalker, HeroType.BroHard, HeroType.McBrover, HeroType.Blade, HeroType.BroDredd, HeroType.Brononymous, HeroType.DirtyHarry, HeroType.Brominator, HeroType.Brobocop, HeroType.IndianaBrones, HeroType.AshBrolliams, HeroType.Nebro, HeroType.BoondockBros, HeroType.Brochete, HeroType.BronanTheBrobarian, HeroType.EllenRipbro, HeroType.TheBrocketeer, HeroType.TimeBroVanDamme, HeroType.BroniversalSoldier, HeroType.ColJamesBroddock, HeroType.CherryBroling, HeroType.BroMax, HeroType.TheBrode, HeroType.DoubleBroSeven, HeroType.Predabro, HeroType.BroveHeart, HeroType.TheBrofessional, HeroType.Broden, HeroType.TheBrolander, HeroType.SnakeBroSkin, HeroType.TankBro, HeroType.BroLee, HeroType.BroneyRoss, HeroType.LeeBroxmas, HeroType.BronnarJensen, HeroType.HaleTheBro, HeroType.TrentBroser, HeroType.Broc, HeroType.TollBroad, HeroType.BrondleFly };

            /// <summary>
            /// The list of unlock intervals of the bros.
            /// </summary>
            public static List<int> HeroUnlockIntervalsInt = new List<int> { 1, 3, 5, 8, 11, 15, 20, 25, 31, 37, 46, 56, 65, 75, 87, 99, 115, 132, 145, 160, 175, 193, 222, 249, 274, 300, 326, 350, 374, 400, 420, 440, 465, 460, 480 };

            /// <summary>
            /// Return the dictionnary of the Intervals for unlock bros.
            /// </summary>
            /// <returns></returns>
            public static Dictionary<int, HeroType> GetHeroUnlockIntervals()
            {
                return Traverse.Create(typeof(HeroUnlockController)).Field("_heroUnlockIntervals").GetValue() as Dictionary<int, HeroType>;
            }

            /// <summary>
            /// Set the Intervals for unlock bros.
            /// </summary>
            /// <param name="newHeroUnlockIntervals">The new intervall dictionary.</param>
            public static void SetHeroUnlockIntervals(Dictionary<int, HeroType> newHeroUnlockIntervals)
            {
                if (newHeroUnlockIntervals.Count <= 0) throw new Exception("The given Dictionary is null.");
                Traverse.Create(typeof(HeroUnlockController)).Field("_heroUnlockIntervals").SetValue(newHeroUnlockIntervals);
            }

            /// <summary>
            /// Create the HeroUnlockInterval dictionary.
            /// </summary>
            /// <param name="HeroList">The list of hero to use.</param>
            /// <returns></returns>
            public static Dictionary<int, HeroType> BuildHeroUnlockIntervalsDictionary(List<HeroType> HeroList)
            {
                return BuildHeroUnlockIntervalsDictionary(HeroList, HeroUnlockIntervalsInt);
            }

            /// <summary>
            /// Create the HeroUnlockIntervals dictionary.
            /// </summary>
            /// <param name="HeroList">The list of hero to use.</param>
            /// <param name="IntervalsInt">The list of custom intervals to use.</param>
            /// <returns>The HeroUnlockIntervals dictionnary</returns>
            public static Dictionary<int, HeroType> BuildHeroUnlockIntervalsDictionary(List<HeroType> HeroList, List<int> IntervalsInt)
            {
                Dictionary<int, HeroType> HeroDictionary = new Dictionary<int, HeroType>();
                if (IntervalsInt[0] != 1) throw new Exception("The first value of the given int list should be 1.");

                int i = 0;
                try
                {
                    foreach (HeroType Hero in HeroList)
                    {
                        HeroDictionary.Add(IntervalsInt[i], Hero);
                        i++;
                    }
                }
                catch (Exception ex) { throw new Exception("Failed to build Dictionnary ! \n\t" + ex);}

                return HeroDictionary;
            }

            /// <summary>
            /// Show Current Unlock order
            /// </summary>
            public static void ShowHeroUnlockIntervals()
            {
                Dictionary<int, HeroType> HeroUnlockIntervals = Traverse.Create(typeof(HeroUnlockController)).Field("_heroUnlockIntervals").GetValue() as Dictionary<int, HeroType>;

                Main.Log("Start to show intervals....");
                string Table = "\n\n int, bro\n";
                foreach (KeyValuePair<int, HeroType> Hero in HeroUnlockIntervals)
                {
                    Table += (" " + Hero.Key + ",  " + HeroController.GetHeroName(Hero.Value) + "\n");
                }
                Main.Log(Table);
                Main.Log("Finish to show unlock order");
            }
        }
        
    }
}
