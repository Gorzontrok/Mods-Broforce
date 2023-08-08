using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TheGeneralsTraining.Patches.Bros
{
    [HarmonyPatch(typeof(TestVanDammeAnim), "CoverInAcidRPC")]
    static class SethBrondle_GorzonSpecialIdea_Patch
    {
        static bool Prefix(TestVanDammeAnim __instance)
        {
            if (!Main.CanUsePatch || !Main.settings.noAcidCoverIfSpecial) return true;

            try
            {
                Traverse tr = Traverse.Create(__instance);
                if (!(__instance is Alien))
                {
                    if (__instance.SpecialAmmo >= 1 && __instance as BrondleFly)
                    {
                        BrondleFly bro = __instance as BrondleFly;
                        SpriteSM spriteSM = UnityEngine.Object.Instantiate<SpriteSM>(bro.teleportOutAnimation);
                        spriteSM.transform.position = bro.transform.position;

                        SpriteSM spriteSM2 = UnityEngine.Object.Instantiate<SpriteSM>(bro.teleportInAnimation);
                        spriteSM2.transform.parent = bro.transform;
                        spriteSM2.transform.localPosition = -Vector3.forward;

                        __instance.SpecialAmmo--;
                        return false;
                    }
                    if (!__instance.hasBeenCoverInAcid)
                    {
                        __instance.hasBeenCoverInAcid = true;
                        if (__instance is Alien)
                        {
                            if (__instance.health < 10)
                            {
                                __instance.Stun(1f);
                                __instance.yI += 80f;
                            }
                        }
                        else
                        {
                            if (__instance is Mook)
                            {
                                __instance.Panic((int)Mathf.Sign(UnityEngine.Random.Range(-1f, 1f)), 4f, true);
                            }
                            __instance.gunSprite.GetComponent<Renderer>().enabled = false;
                            if (__instance is HellLostSoul)
                            {
                                __instance.CreateSkeleton();
                            }
                            else if (__instance is MookHellBoomer)
                            {
                                __instance.GetComponent<Renderer>().material = EffectsController.instance.hellBoomerCoveredInAcidMaterial;
                                tr.GetFieldValue<SpriteSM>("sprite").RecalcTexture();
                            }
                            else if (__instance is MookBigGuy || (__instance is MookSuicide && __instance.width > 10f))
                            {
                                __instance.GetComponent<Renderer>().material = EffectsController.instance.bigMookCoveredInAcidMaterial;
                                tr.GetFieldValue<SpriteSM>("sprite").RecalcTexture();
                            }
                            else if (__instance is MookGrenadier)
                            {
                                __instance.GetComponent<Renderer>().material = EffectsController.instance.grenadierMookCoveredInAcidMaterial;
                                tr.Field("sprite").GetValue<SpriteSM>().RecalcTexture();
                            }
                            else if (__instance is MookDog)
                            {
                                MookDog mookDog = __instance as MookDog;
                                if (mookDog.isMegaDog)
                                {
                                    __instance.GetComponent<Renderer>().material = EffectsController.instance.bigDogCoveredInAcidMaterial;
                                    tr.GetFieldValue<SpriteSM>("sprite").RecalcTexture();
                                }
                                else
                                {
                                    __instance.GetComponent<Renderer>().material = EffectsController.instance.dogCoveredInAcidMaterial;
                                    tr.GetFieldValue<SpriteSM>("sprite").RecalcTexture();
                                }
                            }
                            else if (tr.GetFieldValue<bool>("isHero"))
                            {
                                tr.Field("doingMelee").SetValue(false);
                                tr.Field("usingPockettedSpecial").SetValue(false);
                                tr.Field("usingSpecial").SetValue(false);
                                __instance.GetComponent<Renderer>().material = EffectsController.instance.broCoveredInAcidMaterial;
                                tr.GetFieldValue<SpriteSM>("sprite").RecalcTexture();
                            }
                            else
                            {
                                __instance.GetComponent<Renderer>().material = EffectsController.instance.mookCoveredInAcidMaterial;
                                tr.GetFieldValue<SpriteSM>("sprite").RecalcTexture();
                            }
                        }
                    }
                    tr.Field("acidMeltTimer").SetValue(1f);
                }
                return false;
            }
            catch(Exception e)
            {
                Main.Log(e);
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(BrondleFly), "TeleFrag")]
    static class Brondlefly_TeleFrag_Patch
    {
        static bool Prefix(BrondleFly __instance)
        {
            if(Main.CanUsePatch && Main.settings.leaveBeesBehind)
            {
                Traverse t = Traverse.Create(__instance);
                try
                {
                    t.SetFieldValue("teleportPos", __instance.transform.position);
                    t.SetFieldValue("teleportCamLerp", 0f);

                   Vector3 vector = ((!__instance.left) ? Vector3.zero : Vector3.left) + ((!__instance.right) ? Vector3.zero : Vector3.right) + ((!__instance.up) ? Vector3.zero : Vector3.up) + ((!__instance.down) ? Vector3.zero : Vector3.down);
                    if (vector != Vector3.zero)
                    {
                        vector.Normalize();
                    }
                    else
                    {
                        vector = new Vector3(__instance.transform.localScale.x, 0f, 0f);
                    }
                    float num = 80f;
                    Vector3 vector2 = __instance.transform.position + vector * num;

                    DoodadBeehive b = UnityEngine.Object.Instantiate<Block>(Map.Instance.activeTheme.blockBeeHive, vector, Quaternion.identity) as DoodadBeehive;

                    b.Collapse(0, 0, 10);

                    List<Unit> unitsInRange = Map.GetUnitsInRange(num, num, __instance.X, __instance.Y, false);
                    float num2 = 50f;
                    Unit unit = null;
                    foreach (Unit unit2 in unitsInRange)
                    {
                        if (unit2.IsEnemy && !(unit2 is Tank))
                        {
                            Vector3 from = unit2.transform.position - __instance.transform.position;
                            float num3 = Vector3.Angle(from, vector);
                            if (num3 < num2)
                            {
                                num2 = num3;
                                unit = unit2;
                            }
                        }
                    }
                    bool flag = false;
                    if (unit == null)
                    {
                        flag = t.Method("SearchForOpenSpot", new object[] { vector2, vector }).GetValue<bool>();
                        unitsInRange = Map.GetUnitsInRange(32f, 32f, vector2.x, vector2.y, false);
                        if (unitsInRange.Count > 0)
                        {
                            foreach (Unit unit3 in unitsInRange)
                            {
                                if (unit3.IsEnemy && !(unit3 is Tank))
                                {
                                    unit = unit3;
                                    flag = true;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        flag = true;
                    }
                    Debug.Log("Found suitable spot " + flag);
                    if (unit != null)
                    {
                        vector2 = unit.transform.position;
                        unit.Damage(unit.health, DamageType.Normal, 500f, 500f, __instance.Direction, __instance, unit.X, unit.Y);
                        unit.GibNow(DamageType.Crush, 0f, 100f);
                        t.SetFieldValue("defaultMaterial", __instance.coveredInBloodMaterial);
                        t.SetFieldValue("isBloody", true);
                        __instance.GetComponent<Renderer>().material = t.Field("defaultMaterial").GetValue<Material>();
                    }
                    SpriteSM spriteSM = UnityEngine.Object.Instantiate<SpriteSM>(__instance.teleportOutAnimation);
                    spriteSM.transform.position = __instance.transform.position;
                    if (flag)
                    {
                        if (Mathf.Abs(vector2.x - __instance.X) > 16f || Mathf.Abs(vector2.y - __instance.Y) > 16f)
                        {
                            __instance.SpecialAmmo--;
                        }
                        t.SetFieldValue("pressSpecialFacingDirection", 0);
                        __instance.SetXY(vector2.x, vector2.y);
                        __instance.SetPosition();
                    }
                    __instance.xI = 0f;
                    if (__instance.up)
                    {
                        t.SetFieldValue("jumpTime", Time.time);
                        __instance.yI = 120f;
                    }
                    else
                    {
                        __instance.yI = 0f;
                    }
                    SpriteSM spriteSM2 = UnityEngine.Object.Instantiate<SpriteSM>(__instance.teleportInAnimation);
                    spriteSM2.transform.parent = __instance.transform;
                    spriteSM2.transform.localPosition = -Vector3.forward;
                    Sound.GetInstance().PlayAudioClip(__instance.teleportSound, __instance.transform.position, 0.5f, UnityEngine.Random.Range(0.8f, 1.2f), false, false, 0f, false, false);
                    t.SetFieldValue("usingSpecial", false);
                    return false;
                }
                catch(Exception ex)
                {
                    Main.ExceptionLog(ex);
                }
            }
            return true;
        }
    }
}
