using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Surprise.Aliens
{
    class MelterMakeEffectDuringRolling : CustomAction
    {
        Projectile projectile;
        protected override void Awake()
        {
            base.Awake();
            projectile = HeroController.GetHeroPrefab(HeroType.BrondleFly).projectile;
        }
        protected override void DoAction()
        {
           /* EffectsController.CreateExplosionRangePop(actionBy.X, actionBy.Y + 6f, -1f, (actionBy as AlienMelter).explodeRange * (actionBy as AlienMelter).broHarmRangeM * 2.4f);
            EffectsController.CreateSlimeParticlesSpray(actionBy.bloodColor, actionBy.X, actionBy.Y + 6f, 1f, 34, 6f, 5f, 300f, actionBy.xI * 0.6f, actionBy.yI * 0.2f + 150f, 0.6f);
            EffectsController.CreateSlimeExplosion(actionBy.X, actionBy.Y, 15f, 15f, 140f, 0f, 0f, 0f, 0f, 0, 20, 120f, 0f, Vector3.up, BloodColor.Green);
            EffectsController.CreateSlimeCover(15, actionBy.X, actionBy.Y + 8f, 60f, false);*/
            base.DoAction();
            ProjectileController.SpawnProjectileOverNetwork(this.projectile, actionBy, actionBy.X, actionBy.Y, 0, 500, true, actionBy.playerNum, false, true, 0f);
        }
    }
}
