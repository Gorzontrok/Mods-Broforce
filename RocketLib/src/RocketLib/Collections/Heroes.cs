using System;
using System.Collections.Generic;
using System.Linq;

namespace RocketLib.Collections
{
    public static class Heroes
    {
        public static HeroType[] All
        {
            get
            {
                if (_all == null || _all.Length <= 0)
                {
                    _all = (HeroType[])Enum.GetValues(typeof(HeroType));
                }
                return _all;
            }
        }
        private static HeroType[] _all;

        /// <summary>
        /// All playable heroes. (read only)
        /// </summary>
        public static HeroType[] Playables
        {
            get
            {
                if (_playables == null || _playables.Length <= 0)
                {
                    var temp = All.ToList();
                    temp.Remove(HeroType.None);
                    temp.Remove(HeroType.Random);
                    temp.Remove(HeroType.TankBroTank);
                    temp.Remove(HeroType.Final);
                    _playables = temp.ToArray();
                }
                return _playables;
            }
        }
        private static HeroType[] _playables;

        /// <summary>
        /// All heroes playables in campaign. (read only)
        /// </summary>
        public static HeroType[] CampaignBro
        {
            get
            {
                if (_campaignBro == null || _campaignBro.Length <= 0)
                {
                    var temp = Playables.ToList();
                    temp.RemoveAll(item => Expendabros.Contains(item) || Unused.Contains(item));
                    _campaignBro = temp.ToArray();
                }
                return _campaignBro;
            }
        }
        private static HeroType[] _campaignBro;

        /// <summary>
        /// Expendabros heroes array. (read only)
        /// </summary>
        public static HeroType[] Expendabros
        {
            get
            {
                return new HeroType[] { HeroType.BroneyRoss, HeroType.LeeBroxmas, HeroType.BronnarJensen, HeroType.HaleTheBro, HeroType.Broc, HeroType.TollBroad, HeroType.TrentBroser };
            }
        }

        /// <summary>
        /// Unused Heroes. Some are playble some aren't. (read only)
        /// </summary>
        public static HeroType[] Unused
        {
            get
            {
                return new HeroType[] { HeroType.ChevBrolios, HeroType.CaseyBroback, HeroType.ScorpionBro, HeroType.SuicideBro };
            }
        }

        /// <summary>
        /// Heroes from the Forever Update in unlock order. (read only)
        /// </summary>
        public static HeroType[] ForeverUpdate
        {
            get
            {
                return new HeroType[] { HeroType.Desperabro, HeroType.Xebro, HeroType.DemolitionBro, HeroType.DemolitionBro, HeroType.Broffy, HeroType.BrondleFly, HeroType.BroGummer };
            }
        }

        /// <summary>
        /// The original unlock intervals dictionary. (read only)
        /// </summary>
        public static Dictionary<int, HeroType> OriginalUnlockIntervals
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
                    { 480, HeroType.BroLee },
                    { 500, HeroType.Desperabro },
                    { 520, HeroType.Xebro },
                    { 540, HeroType.DemolitionBro },
                    { 560, HeroType.Broffy },
                    { 580, HeroType.BrondleFly },
                    { 600, HeroType.BroGummer }
                };
            }
        }

        /// <summary>
        /// The original array of hero save number interval. (read only)
        /// </summary>
        public static int[] HeroSaveInterval
        {
            get
            {
                return new int[] { 0, 1, 3, 5, 8, 11, 15, 20, 25, 31, 37, 46, 56, 65, 75, 87, 99, 115, 132, 145, 160, 175, 193, 222, 249, 274, 300, 326, 350, 374, 400, 420, 440, 460, 480, 500, 520, 540, 560, 580, 600 };
            }
        }
    }
}
