using System;
using HarmonyLib;
using ReskinMod.Skins;
using UnityEngine;

namespace ReskinMod.Patch
{
    [HarmonyPatch(typeof(AlienXenomorph), "Start")]
    static class AlienXenomorph_Reskin_Patch
    {
        static void Prefix(AlienXenomorph __instance)
        {
            try
            {
                SkinCollection skinCollection = SkinCollection.GetSkinCollection(__instance.GetType().Name.ToLower());
                if (skinCollection != null)
                {
                    Skin character = skinCollection.GetSkin(SkinType.Character, 0);
                    Skin character2 = skinCollection.GetSkin(SkinType.Character, 1);
                    if (character2 != null && __instance.hasBrainBox)
                    {
                        SpriteSM sprite = __instance.gameObject.GetComponent<SpriteSM>();
                        sprite.meshRender.sharedMaterial.SetTexture("_MainTex", character.texture);
                    }
                    if (character != null)
                    {
                        __instance.noBrainBoxMaterial.mainTexture = character.texture;
                    }
                }
            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
        }
    }
}
