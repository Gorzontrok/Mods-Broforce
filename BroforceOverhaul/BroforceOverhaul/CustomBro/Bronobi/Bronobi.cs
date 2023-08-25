using System;
using System.Collections.Generic;
using UnityEngine;
using BroMakerLib;

namespace BroforceOverhaul.CustomBro.Bronobi
{
    public class Bronobi : SwordBroBaseMaker
    {
        protected BronobiForceWave forceWave;
        public override void bm_SetupBro(Player player)
        {
            this.bm_DefaultMaterial = ResourcesController.GetMaterialResource("Bros.Bronobi_anim.png", ResourcesController.Unlit_DepthCutout);
            this.bm_DefaultGunMaterial = ResourcesController.GetMaterialResource("Bros.Bronobi_gun_anim.png", ResourcesController.Unlit_DepthCutout);

            base.bm_SetupBro(player);
        }

        protected override void Awake()
        {
            try
            {
                _jumpForce = 300;
                speed = 130;
                this.fireRate = 0.2f;
                fireDelay = 0.1f;
                base.Awake();
                this.soundHolder = HeroController.GetHeroPrefab(HeroType.Blade).soundHolder;
                isHero = true;
            }
            catch(Exception ex)
            {
                Main.ExceptionLog(ex);
            }
        }

        protected override void SetGunPosition(float xOffset, float yOffset)
        {
            this.gunSprite.transform.localPosition = new Vector3(xOffset + 4f, yOffset, -1f);
        }

        protected override void SetupThrownMookVelocity(out float XI, out float YI)
        {
            base.SetupThrownMookVelocity(out XI, out YI);
            XI *= 1.2f;
            YI *= 1.2f;
        }

        protected override void UseSpecial()
        {
            if(SpecialAmmo > 0)
            {
               /* DirectionEnum direction;
                if (this.right)
                {
                    direction = DirectionEnum.Right;
                }
                else if (this.left)
                {
                    direction = DirectionEnum.Left;
                }
                else if (base.transform.localScale.x > 0f)
                {
                    direction = DirectionEnum.Right;
                }
                else
                {
                    direction = DirectionEnum.Left;
                }*/
                try
                {
                    forceWave = new GameObject("BronobiForceWave", new Type[] { typeof(Transform) }).AddComponent<BronobiForceWave>();
                    forceWave.transform.position = this.transform.position;
                    forceWave.Setup(playerNum, this, DirectionEnum.Any);

                }
                catch(Exception ex)
                {
                    Main.ExceptionLog(ex);
                }
            }
            else
            {
                this.player.StopAvatarSpecialFrame();
                HeroController.FlashSpecialAmmo(base.playerNum);
            }
        }

    }
}
