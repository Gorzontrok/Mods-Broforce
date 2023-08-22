using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TerroristC4Programs.Extensions
{
    public static class MookExtensions
    {
        public static void Decapitate(this Mook mook, int damage, DamageType damageType, float xI, float yI, int direction, float xHit, float yHit, MonoBehaviour damageSender)
        {
            mook.SetFieldValue("decapitated", true);
            mook.SetFieldValue("assasinated",  false);
            mook.SetFieldValue("disemboweled", false);
            mook.canBeAssasinated = false;
            mook.health = 1;
            EffectsController.CreateBloodParticles(mook.bloodColor, mook.X, mook.Y + 16f, 10, 3f, 2f, 50f, xI * 0.5f + (float)(direction * 50), yI * 0.4f + 80f);
            EffectsController.CreateGibs(mook.GetFieldValue<GibHolder>("decapitationGib"), mook.GetComponent<Renderer>().sharedMaterial, mook.X, mook.Y, 100f, 100f, xI * 0.25f, yI * 0.4f + 60f);
            mook.CallMethod("PlayDecapitateSound");
            mook.CallMethod("DeactivateGun");
            mook.GetComponent<Renderer>().sharedMaterial = mook.GetFieldValue<Material>("decapitatedMaterial");
            if (UnityEngine.Random.value > 0f)
            {
                mook.Panic(UnityEngine.Random.Range(0, 2) * 2 - 1, 2.5f, false);
                mook.SetFieldValue("decapitationCounter", 0.3f + UnityEngine.Random.value * 0.4f);
            }
            else
            {
                mook.Damage(mook.health, damageType, xI, yI, direction, damageSender, xHit, yHit);
            }
            if (yI > 400f)
            {
                mook.Knock(DamageType.Knock, xI, yI, false);
                mook.yI = Mathf.Max(mook.yI, yI * 0.5f);
                mook.BackSomersault(true);
            }
            mook.CallMethod("TryAssignHeroThatKilledMe", damageSender);
        }
    }
}
