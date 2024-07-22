using TFBGames.Systems;
using UnityEngine;

namespace RocketLib
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

        public static Material GetPockettedMaterial(this PlayerHUD hud, PockettedSpecialAmmoType type)
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
                    return null;
            }
        }
    }
}
