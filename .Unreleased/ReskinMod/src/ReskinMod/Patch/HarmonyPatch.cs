using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using ReskinMod.Skins;

namespace ReskinMod.Patch
{
    /*
     SkinCollection skinCollection = SkinCollection.GetSkinCollection(__instance.GetType().Name.ToLower());
     if(skinCollection != null)
     {

     }
     */
    [HarmonyPatch(typeof(TestVanDammeAnim), "Awake")]
    static class ReSkin_Patch
    {
        static void Postfix(TestVanDammeAnim __instance)
        {
            try
            {
                TestVanDammeAnim inst = __instance;
                //Main.bmod.Log(inst.GetType().Name);
                SkinCollection skinCollection = SkinCollection.GetSkinCollection(inst.GetType().Name.ToLower());
                if(skinCollection != null)
                {
                    Skin characterSkin = skinCollection.GetSkin(SkinType.Character, 0);
                    Skin gunSkin = skinCollection.GetSkin(SkinType.Gun, 0);

                    if (characterSkin != null)
                    {
                        SpriteSM sprite = inst.gameObject.GetComponent<SpriteSM>();
                        sprite.meshRender.sharedMaterial.SetTexture("_MainTex", characterSkin.texture);
                    }
                    if (gunSkin != null)
                    {
                        inst.gunSprite.GetComponent<Renderer>().material.mainTexture = gunSkin.texture;
                        inst.gunSprite.GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex", gunSkin.texture);
                    }
                }
            }
            catch(Exception ex)
            {
                Main.bmod.Log(ex);
            }
        }
    }

    [HarmonyPatch(typeof(BroBase), "Start")]
    static class ReskinBro_Patch
    {
        static void Postfix(BroBase __instance)
        {
            if (!Main.enabled) return;

            try
            {
                BroBase inst = __instance;
                //Main.bmod.Log(inst.GetType().Name);
                SkinCollection skinCollection = SkinCollection.GetSkinCollection(inst.GetType().Name.ToLower());
                HeroType heroType = inst.heroType;
                if(skinCollection != null)
                {
                    Skin characterSkin = skinCollection.GetSkin(SkinType.Character, 0);
                    Skin avatarSkin = skinCollection.GetSkin(SkinType.Avatar, 0);
                    Skin gun2 = skinCollection.GetSkin(SkinType.Gun, 1);
                    Skin character2 = skinCollection.GetSkin(SkinType.Character, 1);
                    Skin armless = skinCollection.GetSkin(SkinType.Armless, 0);

                    if (avatarSkin != null)
                    {
                        __instance.player.hud.avatar.meshRender.sharedMaterial.SetTexture("_MainTex", avatarSkin.texture);
                    }

                    if ((__instance as DoubleBroSeven) && inst.GetType().Name == "DoubleBroSeven")
                    {
                        DoubleBroSeven bro = __instance as DoubleBroSeven;
                        if(character2 != null)
                        {
                            bro.balaclavaMaterial.mainTexture = character2.texture;
                        }

                        Traverse.Create(bro).Field("normalMaterial").GetValue<Material>().mainTexture = characterSkin.texture;
                    }
                    else if ((__instance as IndianaBrones) && inst.GetType().Name == "IndianaBrones")
                    {
                        IndianaBrones bro = __instance as IndianaBrones;
                        if (armless != null)
                        {
                            bro.materialArmless.mainTexture = armless.texture;
                        }
                        Traverse.Create(bro).Field("materialNormal").GetValue<Material>().mainTexture = characterSkin.texture;
                    }
                    else if ((__instance as Predabro) && inst.GetType().Name == "Predabro")
                    {
                        Predabro bro = __instance as Predabro;
                        if(character2 != null)
                        {
                            bro.stealthMaterial.mainTexture = character2.texture;
                        }
                        if(gun2 != null)
                        {
                            bro.stealthGunMaterial.mainTexture = gun2.texture;
                        }
                    }
                }
            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
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
                Mook inst = __instance;
                //Main.bmod.Log(inst.GetType().Name);
                SkinCollection skinCollection = SkinCollection.GetSkinCollection(inst.GetType().Name.ToLower());
                if(skinCollection != null)
                {
                    Skin character2 = skinCollection.GetSkin(SkinType.Character, 1);

                    if (__instance as MookRiotShield && inst.GetType().Name == "MookRiotShield")
                    {
                        try
                        {
                            MookRiotShield mook = __instance as MookRiotShield;
                            if(character2 != null)
                            {
                                mook.unarmedMaterial.mainTexture = character2.texture;
                            }
                        }
                        catch (Exception ex) { Main.bmod.logger.ExceptionLog("Shield", ex); }
                    }
                    else if (__instance as MookDog && inst.GetType().Name == "MookDog")
                    {
                        try
                        {
                            MookDog mook = __instance as MookDog;
                            if(character2 != null)
                            {
                                mook.upgradedMaterial.mainTexture = character2.texture;
                            }
                        }
                        catch (Exception ex) { Main.bmod.logger.ExceptionLog("Dog", ex); }
                    }
                }
            }
            catch(Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
        }
    }
    [HarmonyPatch(typeof(ThemeHolder), "DisableAllBlockPrefabs")]
    static class ReskinVillager_Patch2
    {
        static void Postfix(ThemeHolder __instance)
        {
            try
            {
                /*Main.bmod.Log("dd");
                Villager villagerMale = __instance.villager1[0] as Villager;
                {
                    Villager_Reskin.MaleVillager = new ReskinInfo("MaleVillager", Utility.Other_Character_Folder, "_armed");
                    var cInfo = Villager_Reskin.MaleVillager;
                    cInfo.CheckTex();
                    cInfo.CheckSecondTex(Traverse.Create(villagerMale).Field("armedMaterial").GetValue<Material>());
                    cInfo.ApplyReskin(villagerMale);
                    Traverse.Create(villagerMale).Field("armedMaterial").SetValue(cInfo.GetCharacterTex2(Traverse.Create(villagerMale).Field("armedMaterial").GetValue<Material>()));
                }*/
            }catch(Exception ex) { Main.bmod.logger.ExceptionLog("Villager", ex); }

        }
    }
}
