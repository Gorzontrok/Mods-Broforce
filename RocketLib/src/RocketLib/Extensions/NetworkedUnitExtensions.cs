using RocketLib.Collections;
using UnityEngine;

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

        #region Map
        public static void DisturbWildLife<T>(this T unit, float range) where T : NetworkedUnit
        {
            Map.DisturbWildLife(unit.X, unit.Y, range, unit.playerNum);
        }
        public static bool DamageDoodads<T>(this T unit, int damage, DamageType damageType, float range, Vector2 force, out bool hitImpenetrableDoodad) where T : NetworkedUnit
        {
            return Map.DamageDoodads(damage, damageType, unit.X, unit.Y, force.x, force.y, range, unit.playerNum, out hitImpenetrableDoodad, unit);
        }
        #region AlertNearbyMooks
        public static void AlertNearbyMooks<T>(this T unit, Vector2 range) where T : NetworkedUnit
        {
            AlertNearbyMooks(unit, range.x, range.y);
        }
        public static void AlertNearbyMooks<T>(this T unit, float xRange, float yRange) where T : NetworkedUnit
        {
            Map.AlertNearbyMooks(unit.X, unit.Y, xRange, yRange, unit.playerNum);
        }
        public static void AlertNearbyMooks<T>(this T unit, Vector2 range, GridPoint startPoint) where T : NetworkedUnit
        {
            AlertNearbyMooks(unit, range.x, range.y, startPoint);
        }
        public static void AlertNearbyMooks<T>(this T unit, float xRange, float yRange, GridPoint startPoint) where T : NetworkedUnit
        {
            Map.AlertNearbyMooks(unit.X, unit.Y, xRange, yRange, unit.playerNum, startPoint);
        }
        #endregion
        public static void BlindUnits<T>(this T unit, float range, float blindTime = 9f) where T : NetworkedUnit
        {
            Map.BlindUnits(unit.playerNum, unit.X, unit.Y, range, blindTime);
        }
        public static void BotherNearbyMooks<T>(this T unit, Vector2 range) where T : NetworkedUnit
        {
            BotherNearbyMooks(unit, range.x, range.y);
        }
        public static void BotherNearbyMooks<T>(this T unit, float xRange, float yRange) where T : NetworkedUnit
        {
            Map.BotherNearbyMooks(unit.X, unit.Y, xRange, yRange, unit.playerNum);
        }

        public static void BurnUnitsAround_Local<T>(this T unit, int damage, float range, bool penetrates = false, bool setGroundAlight = false) where T : NetworkedUnit
        {
            Map.BurnUnitsAround_Local(unit, unit.playerNum, damage, range, unit.X, unit.Y, penetrates, setGroundAlight);
        }
        #endregion
    }
}
