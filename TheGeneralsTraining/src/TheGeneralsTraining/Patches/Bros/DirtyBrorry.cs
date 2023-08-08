using System;
using HarmonyLib;
using UnityEngine;

namespace TheGeneralsTraining.Patches.Bros.DirtyBrorry
{
    [HarmonyPatch(typeof(DirtyHarry), "PerformBaseBallBatHit")]
    static class DirtyHarry_ReloadOnPunch_Patch
    {
        static bool Prefix(DirtyHarry __instance)
        {
            if (!Main.CanUsePatch || !Main.settings.reloadOnPunch) return true;

            try
            {
                Main.Log("punch");
                Sound sound = __instance.Sound();
                bool meleeHasHit = __instance.GetBool("meleeHasHit");
                bool hasPlayedMissSound = __instance.GetBool("hasPlayedMissSound");

                float num = 8f;
                Vector3 vector = new Vector3(__instance.X + (float)__instance.Direction * (num + 7f), __instance.Y + 10f, 0f);
                bool flag;
                Map.DamageDoodads(3, DamageType.Melee, vector.x, vector.y, 0f, 0f, 6f, __instance.playerNum, out flag, null);
                __instance.CallMethod("KickDoors", 25f);
                if (Map.HitClosestUnit(__instance, __instance.playerNum, 4, DamageType.Melee, num, num * 2f, vector.x, vector.y, __instance.transform.localScale.x * 250f, 250f, true, false, __instance.IsMine, false, true))
                {
                    Main.Log("hit");
                    sound.PlaySoundEffectAt(__instance.soundHolder.alternateMeleeHitSound, 0.3f, __instance.transform.position, 0.6f, true, false, false, 0f);
                    sound.PlaySoundEffectAt(__instance.soundHolder.alternateMeleeHitSound2, 0.5f, __instance.transform.position, 0.6f, true, false, false, 0f);
                    meleeHasHit = true;
                    EffectsController.CreateProjectilePopWhiteEffect(__instance.X + (__instance.width + 10f) * __instance.transform.localScale.x, __instance.Y + __instance.height + 4f);
                    // Added
                    __instance.SetFieldValue("bulletCount", 0);
                }
                else
                {
                    if (!hasPlayedMissSound)
                    {
                        sound.PlaySoundEffectAt(__instance.soundHolder.missSounds, 0.15f, __instance.transform.position, 1f, true, false, false, 0f);
                    }
                    hasPlayedMissSound = true;
                }
                __instance.SetFieldValue("meleeChosenUnit", null);
                if (!meleeHasHit && __instance.CallMethod<bool>("TryMeleeTerrain", 0, 2))
                {
                    meleeHasHit = true;
                }

                __instance.SetFieldValue("meleeHasHit", meleeHasHit);
                __instance.SetFieldValue("hasPlayedMissSound", hasPlayedMissSound);
                return false;
            }
            catch(Exception e)
            {
                Main.Log(e);
                return true;
            }
        }
    }
}
