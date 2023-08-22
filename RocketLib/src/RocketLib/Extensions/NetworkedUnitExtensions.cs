using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using RocketLib.Collections;

namespace RocketLib
{
    public static class NetworkedUnitExtensions
    {
        public static bool IsPlayer(this NetworkedUnit self)
        {
            return (self as TestVanDammeAnim) != null && self.As<TestVanDammeAnim>().player != null && self.playerNum >= Nums.PLAYER_MIN && self.playerNum < Nums.PLAYER_MAX;
        }

        public static bool IsPlayerSide(this NetworkedUnit self)
        {
            return self.playerNum >= Nums.PLAYER_MIN;
        }

        public static bool IsPlayerSideAndNotPlayer(this NetworkedUnit self)
        {
            return self.playerNum >= Nums.PLAYER_MAX;
        }

        public static bool IsTerrorist(this NetworkedUnit self)
        {
            return self.playerNum == Nums.TERRORIST && self as TestVanDammeAnim;
        }

        public static bool IsAlien(this NetworkedUnit self)
        {
            return self.playerNum == Nums.ALIENS && self as Alien;
        }

        public static bool IsEnemy(this NetworkedUnit self)
        {
            return self.IsEnemy && (IsTerrorist(self) || IsAlien(self));
        }
    }
}
