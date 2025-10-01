using System;

namespace RocketLib.Collections
{
    public static class Pickups
    {
        public static PickupType[] All
        {
            get
            {
                if (_all == null || _all.Length <= 0)
                {
                    _all = (PickupType[])Enum.GetValues(typeof(PickupType));
                }
                return _all;
            }
        }
        private static PickupType[] _all;

        public static PickupType[] SpecialAmmo
        {
            get
            {
                return new PickupType[]  {
                    PickupType.Ammo,
                    PickupType.Airstrike,
                    PickupType.TimeSlow,
                    PickupType.RemotecontrolCar,
                    PickupType.MechDrop,
                    PickupType.AlienPheromones,
                    PickupType.Steroids
                };
            }
        }
        public static PickupType[] Rogueforce
        {
            get
            {

                return new PickupType[] { PickupType.Perk, PickupType.Dollars };
            }
        }

        public static PickupType[] Flexes
        {
            get
            {
                return new PickupType[] { PickupType.FlexTeleport, PickupType.FlexAirJump, PickupType.FlexAlluring, PickupType.FlexGoldenLight, PickupType.FlexInvulnerability };
            }
        }
    }
}
