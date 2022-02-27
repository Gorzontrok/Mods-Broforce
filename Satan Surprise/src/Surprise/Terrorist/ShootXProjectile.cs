using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace Surprise.Terrorist
{
    class ShootXProjectile : CustomAction
    {
        Projectile projectile;

        public override void Call(TestVanDammeAnim callBy, params object[] objects)
        {
            projectile = (Projectile)objects[0];
            base.Call(callBy, objects);
        }
        protected override void DoAction()
        {
            Traverse trav = Traverse.Create(actionBy);
            trav.Field("gunFrame").SetValue(3);
            trav.Field("gunSprite").GetValue<SpriteSM>().SetLowerLeftPixel((float)(trav.Field("gunSpritePixelWidth").GetValue<int>() * 3), 32f);
            EffectsController.CreateMuzzleFlashEffect(actionBy.X + actionBy.transform.localScale.x * 10f, actionBy.Y + 8f, -25f, actionBy.transform.localScale.x * (float)(250 + ((!Demonstration.bulletsAreFast) ? 0 : 150)) * 0.01f, (float)(UnityEngine.Random.Range(0, 4) - 2) * ((!Demonstration.bulletsAreFast) ? 0.2f : 1f) * 0.01f, actionBy.transform);
            ProjectileController.SpawnProjectileLocally(projectile, actionBy, actionBy.X + actionBy.transform.localScale.x * 10f, actionBy.Y + 8f, actionBy.transform.localScale.x * (float)(250 + ((!Demonstration.bulletsAreFast) ? 0 : 150)), (float)(UnityEngine.Random.Range(0, 4) - 2) * ((!Demonstration.bulletsAreFast) ? 0.2f : 1f), trav.Field("firingPlayerNum").GetValue<int>());
            trav.Method("PlayAttackSound").GetValue();
            base.DoAction();
        }
    }
}
