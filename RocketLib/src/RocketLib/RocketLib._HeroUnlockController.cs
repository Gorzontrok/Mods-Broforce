using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RocketLibLoadMod;

namespace RocketLib0
{
    public partial class RocketLib
    {
        /// <summary>
        ///
        /// </summary>
        public static class _HeroUnlockController
        {
            /// <summary>
            /// It's the HeroType  list of the Broforce bros. (read only)
            /// </summary>
            public static HeroType[] HeroTypes_Intervals
            {
                get
                {
                    return new HeroType[] { HeroType.Rambro, HeroType.Brommando, HeroType.BaBroracus, HeroType.BrodellWalker, HeroType.BroHard, HeroType.McBrover, HeroType.Blade, HeroType.BroDredd, HeroType.Brononymous, HeroType.DirtyHarry, HeroType.Brominator, HeroType.Brobocop, HeroType.IndianaBrones, HeroType.AshBrolliams, HeroType.Nebro, HeroType.BoondockBros, HeroType.Brochete, HeroType.BronanTheBrobarian, HeroType.EllenRipbro, HeroType.TheBrocketeer, HeroType.TimeBroVanDamme, HeroType.BroniversalSoldier, HeroType.ColJamesBroddock, HeroType.CherryBroling, HeroType.BroMax, HeroType.TheBrode, HeroType.DoubleBroSeven, HeroType.Predabro, HeroType.BroveHeart, HeroType.TheBrofessional, HeroType.Broden, HeroType.TheBrolander, HeroType.SnakeBroSkin, HeroType.TankBro, HeroType.BroLee };
                }
            }
            /// <summary>
            /// It's the HeroType List of the Expendabros bros. (read only)
            /// </summary>
            public static HeroType[] Expendabros_HeroTypes_Intervals
            {
                get
                {
                    return new HeroType[] { HeroType.BroneyRoss, HeroType.LeeBroxmas, HeroType.BronnarJensen, HeroType.HaleTheBro, HeroType.Broc, HeroType.TollBroad, HeroType.TrentBroser };
                }
            }
            /// <summary>
            /// It's the HeroType List of the Expendabros bros. (read only)
            /// </summary>
            public static HeroType[] Other_Bros_HeroTypes
            {
                get
                {
                    return new HeroType[] { HeroType.BrondleFly, HeroType.SuicideBro };
                }
            }
            /// <summary>
            /// It's the full HeroType list of the Broforce bros. Include Expendabros and Brondle Fly. (read only)
            /// </summary>
            public static HeroType[] Full_HeroType
            {
                get
                {
                    List<HeroType> heroTypes = new List<HeroType>(HeroTypes_Intervals);
                    heroTypes.AddRange(Expendabros_HeroTypes_Intervals);
                    return heroTypes.ToArray();
                }
            }
            /// <summary>
            /// The list of unlock intervals of the bros. (read only)
            /// </summary>
            public static int[] Hero_Unlock_Intervals
            {
                get
                {
                    return new int[] { 0, 1, 3, 5, 8, 11, 15, 20, 25, 31, 37, 46, 56, 65, 75, 87, 99, 115, 132, 145, 160, 175, 193, 222, 249, 274, 300, 326, 350, 374, 400, 420, 440, 460, 480 };
                }
            }

            /// <summary>
            /// The original dictionary of the unlock intervals. (read only)
            /// </summary>
            public static Dictionary<int, HeroType> Original_Unlock_Intervals
            {
                get
                {
                    return new Dictionary<int, HeroType>(){
                        { 0, HeroType.Rambro },
                        { 1, HeroType.Brommando },
                        { 3, HeroType.BaBroracus },
                        { 5, HeroType.BrodellWalker },
                        { 8, HeroType.BroHard },
                        { 11, HeroType.McBrover },
                        { 15, HeroType.Blade },
                        { 20, HeroType.BroDredd },
                        { 25, HeroType.Brononymous },
                        { 31, HeroType.DirtyHarry },
                        { 37, HeroType.Brominator },
                        { 46, HeroType.Brobocop },
                        { 56, HeroType.IndianaBrones },
                        { 65, HeroType.AshBrolliams },
                        { 75, HeroType.Nebro },
                        { 87, HeroType.BoondockBros },
                        { 99, HeroType.Brochete },
                        { 115, HeroType.BronanTheBrobarian },
                        { 132, HeroType.EllenRipbro },
                        { 145, HeroType.TheBrocketeer },
                        { 160, HeroType.TimeBroVanDamme },
                        { 175, HeroType.BroniversalSoldier },
                        { 193, HeroType.ColJamesBroddock },
                        { 222, HeroType.CherryBroling },
                        { 249, HeroType.BroMax },
                        { 274, HeroType.TheBrode },
                        { 300, HeroType.DoubleBroSeven },
                        { 326, HeroType.Predabro },
                        { 350, HeroType.BroveHeart },
                        { 374, HeroType.TheBrofessional },
                        { 400, HeroType.Broden },
                        { 420, HeroType.TheBrolander },
                        { 440, HeroType.SnakeBroSkin },
                        { 460, HeroType.TankBro },
                        { 480, HeroType.BroLee }
                    };
                }
            }

            /// <summary>
            /// Set the Intervals for unlock bros.
            /// </summary>
            /// <param name="newHeroUnlockIntervals">The new interval dictionary.</param>
            public static void SetHeroUnlockIntervals(Dictionary<int, HeroType> newHeroUnlockIntervals)
            {
                if (newHeroUnlockIntervals.Count <= 0) throw new Exception("The given Dictionary is null.");
                Traverse.Create(typeof(HeroUnlockController)).Field("_heroUnlockIntervals").SetValue(newHeroUnlockIntervals);
            }

            /// <summary>
            /// Show Current Unlock order
            /// </summary>
            public static void ShowHeroUnlockIntervals()
            {
                UnityModManagerNet.UnityModManager.Logger.Log( "\n\n int, bro\n", "");
                foreach (KeyValuePair<int, HeroType> Hero in HeroUnlockController.heroUnlockIntervals)
                {
                    UnityModManagerNet.UnityModManager.Logger.Log(" " + Hero.Key + ",  " + HeroController.GetHeroName(Hero.Value) + "\n", "");
                }
            }

            /// <summary>
            /// List of all PockettedSpecialAmmoType. (read only)
            /// </summary>
            public static PockettedSpecialAmmoType[] Pocketed_Special_AmmoTypes
            {
                get
                {
                    return new PockettedSpecialAmmoType[] { PockettedSpecialAmmoType.Standard, PockettedSpecialAmmoType.Airstrike, PockettedSpecialAmmoType.Timeslow, PockettedSpecialAmmoType.RemoteControlCar, PockettedSpecialAmmoType.MechDrop, PockettedSpecialAmmoType.AlienPheromones, PockettedSpecialAmmoType.Steroids };
                }
            }
        }
    }
}
