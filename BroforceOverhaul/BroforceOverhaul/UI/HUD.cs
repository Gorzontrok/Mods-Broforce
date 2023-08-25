using System;
using System.Collections.Generic;
using System.Linq;
using RocketLib;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace BroforceOverhaul.UI.HUD
{
    // FaceHugger on avatar
    [HarmonyPatch(typeof(PlayerHUD), "ShowFaceHugger")]
    static class PlayerHUD_FaceHuggerOnAvatar_Patch
    {
        static void Prefix(PlayerHUD __instance)
        {
            if (Main.enabled)
            {
                __instance.showFaceHugger = true;
                __instance.faceHugger1.SetSize(Traverse.Create(__instance).Field("avatarFacingDirection").GetValue<int>() * __instance.faceHugger1.width, __instance.faceHugger1.height);
                __instance.avatar.SetLowerLeftPixel(new Vector2(__instance.faceHugger1.lowerLeftPixel.x, 1f)); //For some reason he makes the avatar transparent.
                __instance.faceHugger1.gameObject.SetActive(true);
            }
        }
    }

    // Icon for
    [HarmonyPatch(typeof(PlayerHUD), "Awake")]
    static class PlayerHUD_TeargasIcon_Patch
    {
        static void Postfix(PlayerHUD __instance)
        {
            Material material = ResourcesController.GetMaterialResource("HUD.Grenade_Tear_Gas.png", ResourcesController.Particle_AlphaBlend);
            if (Main.enabled && __instance.doubleBroGrenades.Length < 5 && material != null)
            {
                List<Material> tempList = __instance.doubleBroGrenades.ToList();
                tempList.Add(material);
                __instance.doubleBroGrenades = tempList.ToArray();
            }
        }
    }

    // Multiple pocketed special icon on huds
    [HarmonyPatch(typeof(BroBase), "SetPlayerHUDAmmo")]
    static class BroBase_MultiplePocketedSpecial_Patch
    {
        static Material GetGrenadeMaterials(PlayerHUD hud, PockettedSpecialAmmoType type)
        {
            switch (type)
            {
                case PockettedSpecialAmmoType.Airstrike:
                    return hud.brodellWalkerIcon;
                case PockettedSpecialAmmoType.Timeslow:
                    return hud.timebroIcon;
                case PockettedSpecialAmmoType.RemoteControlCar:
                    return hud.bronnarIcon;
                case PockettedSpecialAmmoType.MechDrop:
                    return hud.mechDropIcon;
                case PockettedSpecialAmmoType.AlienPheromones:
                    return hud.alienPheromones;
                case PockettedSpecialAmmoType.Steroids:
                    return hud.steroids;
                default:
                    return hud.rambroIcon;
            }
        }

        static bool Prefix(BroBase __instance)
        {
            if (Main.enabled)
            {
                try
                {
                    if (__instance.player != null)
                    {
                        PlayerHUD hud = __instance.player.hud;
                        int iMax = __instance.pockettedSpecialAmmo.Count;
                        if (__instance.pockettedSpecialAmmo.Count > 0)
                        {
                            int pocketedSpecialStartIndex = 0;
                            if(__instance.pockettedSpecialAmmo.Count > 6)
                            {
                                pocketedSpecialStartIndex = __instance.pockettedSpecialAmmo.Count - 6;
                            }
                            for (int i = 0; i < __instance.pockettedSpecialAmmo.Count; i++)
                            {
                                if(pocketedSpecialStartIndex + i < __instance.pockettedSpecialAmmo.Count)
                                {
                                    hud.grenadeIcons[i].GetComponent<Renderer>().material = GetGrenadeMaterials(hud, __instance.pockettedSpecialAmmo[pocketedSpecialStartIndex + i]);
                                }
                            }
                            hud.SetGrenades(__instance.pockettedSpecialAmmo.Count);
                        }
                        else
                        {
                            hud.SetGrenadeMaterials(__instance.heroType);
                            hud.SetGrenades(__instance.SpecialAmmo);
                        }
                    }
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
    // Icon for
    [HarmonyPatch(typeof(PlayerHUD), "SetupIcons")]
    static class PlayerHUD_MoreSpaceBetweenGrenades_Patch
    {
        static void Postfix(PlayerHUD __instance, ref SpriteSM[] icons, ref int direction, ref bool doubleAvatar)
        {
            if(Main.enabled)
            {
                try
                {
                    for (int i = 0; i < icons.Length; i++)
                    {
                        icons[i].transform.localPosition = new Vector3((float)(((!doubleAvatar) ? 0 : (direction * 6)) + direction * 15 * i + direction * 18), -0.1f, 2f);
                    }
                }
                catch(Exception ex)
                {
                    Main.ExceptionLog(ex);
                }
            }
        }
    }



}

