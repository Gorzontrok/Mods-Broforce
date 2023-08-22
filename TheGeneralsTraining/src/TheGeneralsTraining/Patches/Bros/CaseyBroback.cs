using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGeneralsTraining.Components;
using UnityEngine;

namespace TheGeneralsTraining.Patches.Bros.CaseyBroback0
{
    [HarmonyPatch(typeof(CaseyBroback), "PerformKnifeMeleeAttack")]
    static class CaseyBroback_PerformKnifeMeleeAttack_Patch
    {
        static bool Prefix(CaseyBroback __instance, bool shouldTryHitTerrain, bool playMissSound)
        {
            if(Main.CanUsePatch && Main.settings.strongerMelee)
            {
                try
                {
                    Traverse t = __instance.GetTraverse();
                    Sound sound = t.GetValue<Sound>("sound");
                    Unit unit = Map.HitClosestUnit(__instance, __instance.playerNum, 0, DamageType.Melee, 14f, 24f, __instance.X + __instance.transform.localScale.x * 8f, __instance.Y + 8f, __instance.transform.localScale.x * 400 * (float)t.GetFieldValue<int>("meleeDirection"), 750, true, false, __instance.IsMine, false, true);
                    if (unit != null)
                    {
                        Mook mook = unit as Mook;
                        if (mook != null)
                        {
                            mook.PlayFallSound(0.3f);
                        }
                        sound.PlaySoundEffectAt(__instance.soundHolder.meleeHitSound, 1f, __instance.transform.position, 1.5f, true, false, false, 0f);
                        t.SetFieldValue("meleeHasHit", true);
                    }
                    else if (playMissSound)
                    {
                        sound.PlaySoundEffectAt(__instance.soundHolder.missSounds, 0.3f, __instance.transform.position, 1f, true, false, false, 0f);
                    }
                    t.SetFieldValue<Unit>("meleeChosenUnit", null);
                    if (shouldTryHitTerrain && t.Method("TryMeleeTerrain", new object[] { 0, 2 }).GetValue<bool>())
                    {
                        t.SetFieldValue("meleeHasHit", true);
                    }
                    t.SetFieldValue("meleeDirection", t.GetFieldValue<int>("meleeDirection") * -1);
                    return false;
                }
                catch(Exception e)
                {
                    Main.Log(e);
                }
            }
return true;
        }
    }
    [HarmonyPatch(typeof(CaseyBroback), "Awake")]
    static class CaseyBroback_Awake_Patch
    {
        static void Prefix(CaseyBroback __instance)
        {
            try
            {
                __instance.originalSpecialAmmo = 3;
            }
            catch(Exception e)
            {
                Main.Log(e);
            }
        }

       /* static void Postfix(CaseyBroback __instance)
        {
            if(Main.CanUsePatch && Main.settings.pigGrenade)
            {
                try
                {
                    //__instance.specialGrenade = ProjectileController.GetMechDropGrenadePrefab();
                    __instance.specialGrenade.gameObject.AddComponent<PigGrenade_Comp>();

                    Material material = ResourcesController.GetMaterialResource("pigGrenade.png", ResourcesController.Unlit_DepthCutout);
                    __instance.specialGrenade.GetComponent<Renderer>().sharedMaterial = material;
                }
                catch(Exception ex)
                {
                    Main.Log(ex);
                }
            }
        }*/
    }
}
