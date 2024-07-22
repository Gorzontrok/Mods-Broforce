using UnityEngine;

namespace RocketLib
{
    public static class BroforceObjectExtensions
    {
        public static void SetRendererTexture<T>(this T anim, Texture texture) where T : BroforceObject
        {
            anim.GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex", texture);
        }

        public static void SetRendererMaterial<T>(this T anim, Material mat) where T : BroforceObject
        {
            anim.GetComponent<Renderer>().sharedMaterial = mat;
        }

        #region Map
        public static void HurtWildLife<T>(this T unit, float range) where T : BroforceObject
        {
            Map.HurtWildLife(unit.X, unit.Y, range);
        }
        #region Attract
        public static void AttractAliens<T>(this T unit, Vector2 range) where T : NetworkedUnit
        {
            AttractAliens(unit, range.x, range.y);
        }
        public static void AttractAliens<T>(this T unit, float xRange, float yRange) where T : NetworkedUnit
        {
            Map.AttractAliens(unit.X, unit.Y, xRange, yRange);
        }
        public static void AttractMooks<T>(this T unit, Vector2 range) where T : NetworkedUnit
        {
            AttractMooks(unit, range.x, range.y);
        }
        public static void AttractMooks<T>(this T unit, float xRange, float yRange) where T : NetworkedUnit
        {
            Map.AttractMooks(unit.X, unit.Y, xRange, yRange);
        }
        #endregion

        #endregion
    }
}
