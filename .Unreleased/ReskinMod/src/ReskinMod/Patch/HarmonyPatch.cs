using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace ReskinMod.Patch
{
    [HarmonyPatch(typeof(BroBase), "Start")]
    static class ReskinBro_Patch
    {
        static void Postfix(BroBase __instance)
        {
            if (!Main.enabled) return;

            try
            {
                HeroType CurrentHero = __instance.heroType;
                if (CurrentHero == HeroType.BoondockBros) return;

                var cInfo = Utility.GetBroReskinInfo(CurrentHero);
                cInfo.CheckTex(CurrentHero, __instance);

                /*if (cInfo.CharacterTex != null)
                {
                    __instance.gameObject.GetComponent<SpriteSM>().meshRender.sharedMaterial.SetTexture("_MainTex", cInfo.CharacterTex);
                }
                if (cInfo.GunTex != null)
                {
                    __instance.gunSprite.GetComponent<Renderer>().material.mainTexture = cInfo.GunTex;
                    __instance.gunSprite.GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex", cInfo.GunTex);
                }
                if (cInfo.AvatarTex != null)
                {
                    __instance.player.hud.avatar.meshRender.sharedMaterial.SetTexture("_MainTex", cInfo.AvatarTex);
                }*/
                __instance = cInfo.ApplyReskin(__instance) as BroBase;

                if (CurrentHero == HeroType.DoubleBroSeven && (__instance as DoubleBroSeven))
                {
                    DoubleBroSeven bro = __instance as DoubleBroSeven;
                    cInfo.CheckSecondTex(bro.balaclavaMaterial);
                    bro.balaclavaMaterial.mainTexture = cInfo.GetCharacterTex2(bro.balaclavaMaterial);

                    Material normalMat = Material.Instantiate(bro.balaclavaMaterial);
                    normalMat.mainTexture = cInfo.CharacterTex;
                    Traverse.Create(bro).Field("normalMaterial").SetValue(normalMat);
                }
                else if (CurrentHero == HeroType.IndianaBrones && (__instance as IndianaBrones))
                {
                    IndianaBrones bro = __instance as IndianaBrones;
                    cInfo.CheckSecondTex(bro.materialArmless);
                    bro.materialArmless.mainTexture = cInfo.GetCharacterTex2(bro.materialArmless);

                    Material materialNormal = Material.Instantiate(bro.materialArmless);
                    materialNormal.mainTexture = cInfo.CharacterTex;
                    Traverse.Create(bro).Field("materialNormal").SetValue(materialNormal);
                }
                else if(CurrentHero == HeroType.Predabro && (__instance as Predabro))
                {
                    Predabro bro = __instance as Predabro;
                    cInfo.CheckSecondTex(bro.stealthMaterial, bro.stealthGunMaterial);

                    bro.stealthMaterial.mainTexture = cInfo.GetCharacterTex2(bro.stealthMaterial);
                    bro.stealthGunMaterial.mainTexture = cInfo.GetGunTex2(bro.stealthGunMaterial);
                }
            }
            catch (Exception ex) { Main.bmod.ExceptionLog(ex); }
        }
    }

    // MOOK
    [HarmonyPatch(typeof(Mook), "Awake")]
    static class ReskinMook_Patch
    {
        static void Postfix(Mook __instance)
        {
            if (!Main.enabled) return;

            try
            {
                //if (__instance as Satan) return;

                var cInfo = Utility.GetMookReskinInfo(__instance);
                cInfo.CheckTex(__instance);
                __instance = cInfo.ApplyReskin(__instance) as Mook;

                // FOR OTHER MATERIAL
                if (__instance as MookRiotShield)
                {
                    try
                    {
                        MookRiotShield mook = __instance as MookRiotShield;
                        cInfo.CheckSecondTex(mook.unarmedMaterial);
                        mook.unarmedMaterial.mainTexture = cInfo.GetCharacterTex2(mook.unarmedMaterial);
                    }
                    catch (Exception ex) { Main.bmod.ExceptionLog("Shield", ex); }
                }
                else if (__instance as MookDog)
                {
                    try
                    {
                        if (!(__instance as HellDog) && !(__instance as Alien))
                        {
                            MookDog mook = __instance as MookDog;
                            cInfo.CheckSecondTex(mook.upgradedMaterial);
                            mook.upgradedMaterial.mainTexture = cInfo.GetCharacterTex2(mook.upgradedMaterial);
                        }
                    }
                    catch(Exception ex) { Main.bmod.ExceptionLog("Dog", ex); }
                }
            }
            catch(Exception ex) { Main.bmod.ExceptionLog(ex); }
        }
    }
    [HarmonyPatch(typeof(ThemeHolder), "DisableAllBlockPrefabs")]
    static class ReskinVillager_Patch2
    {
        static void Postfix(ThemeHolder __instance)
        {
            try
            {
                Main.bmod.Log("dd");
                Villager villagerMale = __instance.villager1[0] as Villager;
                {
                    Villager_Reskin.MaleVillager = new ReskinInfo("MaleVillager", Utility.Other_Character_Folder, "_armed");
                    var cInfo = Villager_Reskin.MaleVillager;
                    cInfo.CheckTex();
                    cInfo.CheckSecondTex(Traverse.Create(villagerMale).Field("armedMaterial").GetValue<Material>());
                    cInfo.ApplyReskin(villagerMale);
                    Traverse.Create(villagerMale).Field("armedMaterial").SetValue(cInfo.GetCharacterTex2(Traverse.Create(villagerMale).Field("armedMaterial").GetValue<Material>()));
                }
            }catch(Exception ex) { Main.bmod.ExceptionLog("Villager", ex); }

        }
    }
}
