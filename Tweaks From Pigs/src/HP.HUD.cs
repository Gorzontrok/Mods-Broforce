using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace TweaksFromPigs
{
    // From Show avatar facehugger
    [HarmonyPatch(typeof(PlayerHUD), "ShowFaceHugger")]
    static class Avatar_FaceHugger_Patch
    {
        static void Prefix(PlayerHUD __instance)
        {
            if (!Main.enabled || ( Main.settings.AvatarFaceHugger_Compatibility && Compatibility.AvatarFaceHugger.i.IsEnabled)) return;
            if(Main.settings.ShowFacehuggerHUD)
            {
                __instance.showFaceHugger = true;
                    __instance.faceHugger1.SetSize(Traverse.Create(__instance).Field("avatarFacingDirection").GetValue<int>() * __instance.faceHugger1.width, __instance.faceHugger1.height);
                    __instance.avatar.SetLowerLeftPixel(new Vector2(__instance.faceHugger1.lowerLeftPixel.x, 1f)); //For some reason he makes the avatar transparent.
                    __instance.faceHugger1.gameObject.SetActive(true);
            }
        }
    }


    // From Skeleton Dead Face
    [HarmonyPatch(typeof(PlayerHUD), "SetAvatarDead")]
    static class SetAvatarDead_Patch
    {
        static void Prefix(PlayerHUD __instance)
        {
            if (!Main.enabled || (Main.settings.SkeletonDeadFace_Compatibility && Compatibility.SkeletonDeadFace.i.IsEnabled)) return;
            if(Main.settings.SkeletonDeadFace)
            {
                SpriteSM sprite = __instance.avatar.gameObject.GetComponent<SpriteSM>();
                sprite.meshRender.sharedMaterial.SetTexture("_MainTex", Utility.CreateTexFromSpriteSM("SkeletonDeadFace.png", sprite));

                Traverse.Create(typeof(PlayerHUD)).Field("avatar").SetValue(sprite); //Change the avatar to the skeleton
                Traverse.Create(typeof(PlayerHUD)).Field("secondAvatar").SetValue(sprite); //Change the second avatar to the skeleton
            }
        }
    }

    [HarmonyPatch(typeof(PlayerHUD), "SetGrenadeMaterials", new Type[] {typeof(HeroType)})]
    static class AddTearGasIcon_Patch
    {
        static void Prefix(PlayerHUD __instance, HeroType type)
        {
            if (!Main.enabled || (Main.settings._007Patch_Compatibility && Compatibility._007_Patch.i.IsEnabled)) return;
            if (type == HeroType.DoubleBroSeven && __instance.doubleBroGrenades.Length < 5)
            {
                Material newIconForTearGas = Material.Instantiate(__instance.rambroIcon);
                newIconForTearGas.mainTexture = Utility.CreateTexFromMat("Grenade_Tear_Gas.png", newIconForTearGas);
                newIconForTearGas.name = "007TearGas";
                List<Material> tempList = __instance.doubleBroGrenades.ToList();
                tempList.Add(newIconForTearGas);
                __instance.doubleBroGrenades = tempList.ToArray();
            }
        }
    }
}
