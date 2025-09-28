using UnityEngine;

namespace RocketLib.Extensions
{
    public static class ProjectileExtensions
    {
        /// <summary>
        /// Prints the values that have been changed in this object from the default values for the Projectile class.
        /// </summary>
        /// <param name="projectile">Object to compare to the default projectile.</param>
        public static void CompareToDefaultProjectile(this Projectile projectile)
        {
            Projectile defaultProjectile = new GameObject("TemporaryProjectile", typeof(Transform), typeof(MeshFilter), typeof(MeshRenderer), typeof(SpriteSM), typeof(Projectile)).GetComponent<Projectile>();
            defaultProjectile.PrintDifferences(projectile);
            UnityEngine.Object.Destroy(defaultProjectile);
        }
    }
}
