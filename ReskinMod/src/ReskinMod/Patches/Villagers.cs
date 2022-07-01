using System;
using HarmonyLib;
using ReskinMod.Skins;
using UnityEngine;

namespace ReskinMod.Patches.Villagers
{
    [HarmonyPatch(typeof(Villager), "Awake")]
    static class Villager_Reskin_Patch
    {
        static void Postfix(Villager __instance)
        {
            if (Main.CantPatch || (__instance as Citizen)) return;

            SkinCollection skinCollection = SkinCollectionController.GetSkinCollection(__instance.GetType().Name.ToLower());
             if(skinCollection != null && UnityEngine.Random.value < Main.settings.citizenVillagerProb)
            {
                Skin character = skinCollection.GetSkin(SkinType.Character, 0);
                Skin characterArmed = skinCollection.GetSkin(SkinType.Character, 1);
                Skin gun = skinCollection.GetSkin(SkinType.Gun, 0);

                if (character != null)
                {
                    __instance.disarmedGunMaterial.mainTexture = character.texture;
                    if (!__instance.hasGun)
                    {
                        SpriteSM sprite = __instance.gameObject.GetComponent<SpriteSM>();
                        sprite.meshRender.sharedMaterial.SetTexture("_MainTex", character.texture);
                    }
                }
                if (gun != null)
                {
                    __instance.gunSprite.GetComponent<Renderer>().material.mainTexture = gun.texture;
                    __instance.gunSprite.GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex", gun.texture);
                }
                if (characterArmed != null)
                {
                    Traverse.Create(__instance).Field("armedMaterial").GetValue<Material>().mainTexture = characterArmed.texture;
                    if(__instance.hasGun)
                    {
                        SpriteSM sprite = __instance.gameObject.GetComponent<SpriteSM>();
                        sprite.meshRender.sharedMaterial.SetTexture("_MainTex", characterArmed.texture);
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(Citizen), "Start")]
    static class Citizen_Reskin_Patch
    {
        static void Postfix(Citizen __instance)
        {
            if (Main.CantPatch) return;
            string skinCollectionName = __instance.runRightAfterHighFive ? "clinton" : ( __instance.becomeTerroristOnGunfire ? "agent" : __instance.GetType().Name.ToLower());
            SkinCollection skinCollection = SkinCollectionController.GetSkinCollection(skinCollectionName);
            if (skinCollection != null && UnityEngine.Random.value < Main.settings.citizenVillagerProb)
            {
                Main.ErrorLog(1);
                Skin character = skinCollection.GetSkin(SkinType.Character, 0);

                if (character != null)
                {
                    SpriteSM sprite = __instance.gameObject.GetComponent<SpriteSM>();
                    sprite.meshRender.sharedMaterial.SetTexture("_MainTex", character.texture);
                }
            }
        }
    }
}
