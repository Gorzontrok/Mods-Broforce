using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheGeneralsTraining.Patches.Bros.Buffy
    {
    [HarmonyPatch(typeof(Broffy), "PerformKnifeMeleeAttack")]
    static class Broffy_PerformKnifeMeleeAttack_Patch
    {
        static bool Prefix(Broffy __instance)
        {
            if (Main.CanUsePatch && Main.settings.betterKick)
            {
                try
                {
                    Traverse t = __instance.GetTraverse();
                    Sound sound = t.GetFieldValue<Sound>("sound");

                    DamageType damageType = !t.GetFieldValue<bool>("dashingMelee") ? DamageType.SilencedBullet : DamageType.Melee;
                    Map.DamageDoodads(3, DamageType.Knifed, __instance.X + (float)(__instance.Direction * 4), __instance.Y, 0f, 0f, 6f, __instance.playerNum, out bool flag, null);
                    t.CallMethod("KickDoors", 24f);
                    bool knock = false;
                    float xI = 0f;
                    float yI = 0f;
                    int damage = 4;
                    if (t.GetFieldValue<bool>("dashingMelee"))
                    {
                        knock = true;
                        xI = (float)(__instance.Direction * 350);
                        yI = 300;
                        damage = 8;
                    }
                    if (Map.HitClosestUnit(__instance, __instance.playerNum, damage, damageType, 14f, 24f, __instance.X + __instance.transform.localScale.x * 8f, __instance.Y + 8f, xI, yI, knock, true, __instance.IsMine, false))
                    {
                        sound.PlaySoundEffectAt(__instance.soundHolder.meleeHitSound, 1f, __instance.transform.position);
                        t.SetFieldValue("meleeHasHit", true);
                    }
                    else if (t.GetFieldValue<bool>("playMissSound"))
                    {
                        sound.PlaySoundEffectAt(__instance.soundHolder.missSounds, 0.3f, __instance.transform.position);
                    }
                    t.SetFieldValue<Unit>("meleeChosenUnit", null);
                    if (t.GetFieldValue<bool>("shouldTryHitTerrain") && t.Method("TryMeleeTerrain", new object[] { 0, 2 }).GetValue<bool>())
                    {
                        t.SetFieldValue("meleeHasHit", true);
                    }
                }
                catch(Exception e)
                {
                    Main.Log(e);
                }
            }
            return true;
        }
    }

}
