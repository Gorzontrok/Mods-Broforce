using System;

namespace RocketLib.Collections
{
    public static class PockettedSpecial
    {
        public static PockettedSpecialAmmoType[] All
        {
            get
            {
                if (_all == null || _all.Length <= 0)
                {
                    _all = (PockettedSpecialAmmoType[])Enum.GetValues(typeof(PockettedSpecialAmmoType));
                }
                return _all;
            }
        }
        private static PockettedSpecialAmmoType[] _all;

        public static PockettedSpecialAmmoType[] SpecialAmmo
        {
            get
            {
                return new PockettedSpecialAmmoType[]  {
                    PockettedSpecialAmmoType.Standard,
                    PockettedSpecialAmmoType.Airstrike,
                    PockettedSpecialAmmoType.Timeslow,
                    PockettedSpecialAmmoType.RemoteControlCar,
                    PockettedSpecialAmmoType.MechDrop,
                    PockettedSpecialAmmoType.AlienPheromones,
                    PockettedSpecialAmmoType.Steroids
                };
            }
        }
        public static PockettedSpecialAmmoType[] Rogueforce
        {
            get
            {

                return new PockettedSpecialAmmoType[] {
                    PockettedSpecialAmmoType.Perk,
                    PockettedSpecialAmmoType.Dollars,
                    PockettedSpecialAmmoType.Revive,
                    PockettedSpecialAmmoType.Damage
                };
            }
        }

    }
}
