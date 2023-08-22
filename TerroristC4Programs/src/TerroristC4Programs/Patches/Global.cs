using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TerroristC4Programs.Extensions;
using UnityEngine;

namespace TerroristC4Programs.PatchesGlobal
{
    [HarmonyPatch(typeof(TestVanDammeAnim), "CalculateZombieInput")]
    static class MookDanceOnBroFlex_Patch
    {
        public static float dancingTime = 0.1f;
        static void Postfix(TestVanDammeAnim __instance)
        {
            if (Mod.CanUsePatch && Mod.Sett.zombiesDanceOnFlex && __instance as Mook)
            {
                var reviveSource = __instance.GetFieldValue<TestVanDammeAnim>("reviveSource");
                if (reviveSource == null) return;

                if (reviveSource.IsGesturing())
                    __instance.Dance(dancingTime);
                else
                    __instance.Dance(0f);
            }
        }
    }

    [HarmonyPatch(typeof(TestVanDammeAnim), "ReplaceWithSkinnedInstance")]
    static class BetterSkinlessSprites_Patch
    {
        static void Prefix(TestVanDammeAnim __instance, ref Unit skinnedInstance)
        {
            if(Mod.CantUsePatch && !Mod.Sett.betterSkinlessSprite) return;

            if(__instance as MookGeneral)
            {
                var tex = TextureManager.GetTexture("mookGeneral_skinless.png");
                if(tex != null)
                    skinnedInstance.GetComponent<SpriteSM>().SetTexture(tex);
            }
            else if (__instance as MookJetpack)
            {
                var tex = TextureManager.GetTexture("mookJetpack_skinless.png");
                if (tex != null)
                    skinnedInstance.GetComponent<SpriteSM>().SetTexture(tex);
            }
            else if (__instance as UndeadTrooper)
            {
                var tex = TextureManager.GetTexture("mookUndead_skinless.png");
                if (tex != null)
                    skinnedInstance.GetComponent<SpriteSM>().SetTexture(tex);
            }
            else
            {
                var tex = TextureManager.GetTexture("mook_skinless.png");
                if (tex != null)
                    skinnedInstance.GetComponent<SpriteSM>().SetTexture(tex);
            }
        }
    }

    [HarmonyPatch(typeof(Unit), "IsHeavy")]
    static class Unit_IsHeavy_Patch
    {
        static bool Prefix(Unit __instance, ref bool __result)
        {
            if(Mod.CanUsePatch && __instance is MookGrenadier)
            {
                __result = true;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(Unit), "HeadShot")]
    static class Unit_HeadSHot_Patch
    {
        static bool Prefix(Unit __instance, int damage, DamageType damageType, float xI, float yI, int direction, float xHit, float yHit, MonoBehaviour damageSender)
        {
            if (Mod.CantUsePatch) return true;

            var mook = __instance as Mook;
            if (mook == null) return true;

            try
            {
                if(mook is ScoutMook)
                {
                    if (mook.GetBool("decapitated"))
                    {
                        mook.Damage(damage, damageType, xI, yI, direction, damageSender, xHit, yHit);
                    }
                    else if ((damageType == DamageType.Bullet || damageType == DamageType.SilencedBullet || damageType == DamageType.Melee || damageType == DamageType.Knifed || damageType == DamageType.Normal || damageType == DamageType.Blade) && mook.health > 0 && damage * 3 >= mook.health)
                    {
                        mook.Decapitate(damage, damageType, xI, yI, direction, xHit, yHit, damageSender);
                    }
                    else
                    {
                        mook.Damage(damage, damageType, xI, yI, direction, damageSender, xHit, yHit);
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                Main.Log(e);
            }
            return true;
        }
    }
}
