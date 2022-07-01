using System;
using HarmonyLib;
using ReskinMod.Skins;
using UnityEngine;

namespace ReskinMod.Patches.Animals
{
    [HarmonyPatch(typeof(Animal), "Awake")]
    static class Animal_Reskin
    {
        static void Postfix(Animal __instance)
        {
            if (Main.CantPatch) return;
            if(__instance.fatAnimal)
            {
                SkinCollection skinCollection = SkinCollectionController.GetSkinCollection(__instance.GetType().Name.ToLower());
                if (__instance.isRotten && skinCollection != null)
                {
                    Skin character = skinCollection.GetSkin(SkinType.Character, 1);

                    if (character != null)
                    {
                        SpriteSM sprite = __instance.gameObject.GetComponent<SpriteSM>();
                        sprite.meshRender.sharedMaterial.SetTexture("_MainTex", character.texture);
                    }
                }
            }
        }
    }
}
