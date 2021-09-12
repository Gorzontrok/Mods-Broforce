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
            if (!Main.enabled) return;
            if(Main.settings.ShowFacehuggerHUD)
            {
                __instance.showFaceHugger = true;
                __instance.avatar.SetLowerLeftPixel(new Vector2(__instance.faceHugger1.lowerLeftPixel.x, 1f)); //For some reason he makes the avatar transparent.
                __instance.faceHugger1.gameObject.SetActive(true);
            }
            
        }
    }
    
    // From Skeleton Dead Face
    [HarmonyPatch(typeof(PlayerHUD), "SetAvatarDead")]
    static class SetAvatarDead_Patch
    {
        static void Prefix(PlayerHUD __instance, bool useFirstAvatar)
        {
            if (!Main.enabled) return;
            if(Main.settings.SkeletonDeadFace)
            {
                bool isUsingSpecialFrame = Traverse.Create(typeof(PlayerHUD)).Field("isUsingSpecialFrame").GetValue<bool>(); //Get the "isUsingSpecialFrame" original Value

                if (isUsingSpecialFrame)
                {
                    __instance.StopUsingSpecialFrame();
                }
                Traverse.Create(typeof(PlayerHUD)).Field("SetToDead").SetValue(true); //Change the value "SetToDead" to true

                SpriteSM sprite = Utility.CreateSpriteSMForAvatar("SkeletonDeadFace.png", ref __instance); // SpriteSM require otherwise he won't work

                Traverse.Create(typeof(PlayerHUD)).Field("avatar").SetValue(sprite); //Change the avatar to the skeleton
                Traverse.Create(typeof(PlayerHUD)).Field("secondAvatar").SetValue(sprite); //Change the second avatar to the skeleton


                SpriteSM spriteSM = (!useFirstAvatar) ? __instance.secondAvatar : __instance.avatar;
                if (spriteSM != null)
                {
                    spriteSM.SetLowerLeftPixel(new Vector2(96f, spriteSM.lowerLeftPixel.y));
                }
            }
        }
    }

    [HarmonyPatch(typeof(PlayerHUD), "SetGrenadeMaterials", new Type[] {typeof(HeroType)})]
    static class AddTearGasIcon_Patch
    {
        static void Prefix(PlayerHUD __instance, HeroType type)
        {
            if (!Main.enabled) return;
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
