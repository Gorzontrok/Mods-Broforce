using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFBGames.Systems;
using UnityEngine;

namespace RocketLib.Extensions
{
    public static class PlayerHUDExtensions
    {
        public static void SetGrenadesMaterials(this PlayerHUD playerHUD, Material[] materials)
        {
            Material grenadeIcon = null;
            for (int i = 0; i < playerHUD.grenadeIcons.Length; i++)
            {

                if (i >= materials.Length)
                {
                    if (grenadeIcon == null)
                        grenadeIcon = GameSystems.ResourceManager.LoadAssetSync<Material>("sharedtextures:GrenadeIcon");
                    playerHUD.grenadeIcons[i].GetComponent<Renderer>().material = grenadeIcon;
                }
                else
                    playerHUD.grenadeIcons[i].GetComponent<Renderer>().material = materials[i];
            }
        }
    }
}
