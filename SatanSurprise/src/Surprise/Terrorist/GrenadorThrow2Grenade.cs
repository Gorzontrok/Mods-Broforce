using System;
using UnityEngine;

namespace Surprise.Terrorist
{
    class GrenadorThrow2Grenade : CustomAction
    {
        public MookGrenadier thrower;

        public override void Call(TestVanDammeAnim callBy, params object[] objects)
        {
            base.Call(callBy, objects);
            thrower = (MookGrenadier)actionBy;
        }

        protected override void DoAction()
        {
            float num2 = 128f;
            float num3 = 32f;
            bool playerRange = thrower.enemyAI.GetPlayerRange(ref num2, ref num3);
            thrower.PlayThrowLightSound(0.25f);
            float num4 = 130f;
            float num5 = 130f;
            if (playerRange)
            {
                float num6 = Mathf.Clamp((thrower.grenadeTossDistanceSpeedMinValue + num2 * thrower.grenadeTossXRangeM + num3 * thrower.grenadeTossYRangeM) * thrower.grenadeTossDistanceSpeedM, 0.5f, 1.5f);
                num6 = num6 * (1f - thrower.grenadeTossV / 2f) + thrower.grenadeTossV * UnityEngine.Random.value;
                num4 *= num6;
                num5 *= num6;
            }
            if (thrower.IsMine)
            {
                ProjectileController.SpawnGrenadeOverNetwork(thrower.longFuseGrenade, thrower, thrower.X + Mathf.Sign(thrower.transform.localScale.x) * 8f, thrower.Y + 24f, 0.001f, 0.011f, Mathf.Sign(thrower.transform.localScale.x) * num4, num5, thrower.playerNum, 1f);
            }
            base.DoAction();
        }
    }
}
