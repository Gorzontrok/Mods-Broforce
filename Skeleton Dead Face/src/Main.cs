using System;
using System.Reflection;
using HarmonyLib;
using System.IO;
using UnityEngine;
using UnityModManagerNet;

namespace SkeletonDeadFace
{
    public class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnToggle = OnToggle;
            var harmony = new Harmony(modEntry.Info.Id);
            var assembly = Assembly.GetExecutingAssembly();
            harmony.PatchAll(assembly);
            mod = modEntry;

            return true;
        }
        
        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }

        public static void Log(object str)
        {
            mod.Logger.Log(str.ToString());
        }

        public static SpriteSM Setup(string filePath, ref PlayerHUD PHUD) //Setup the sprite
        {
            Texture2D tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            tex.LoadImage(File.ReadAllBytes(filePath));
            tex.wrapMode = TextureWrapMode.Clamp;

            SpriteSM sprite = PHUD.avatar.gameObject.GetComponent<SpriteSM>();

            Texture orig = sprite.meshRender.sharedMaterial.GetTexture("_MainTex");

            tex.anisoLevel = orig.anisoLevel;
            tex.filterMode = orig.filterMode;
            tex.mipMapBias = orig.mipMapBias;
            tex.wrapMode = orig.wrapMode;

            sprite.meshRender.sharedMaterial.SetTexture("_MainTex", tex);

            return sprite;
        }

    }
     
    [HarmonyPatch(typeof(PlayerHUD), "SetAvatarDead")]
    static class SetAvatarDead_Patch
    {
        static void Prefix(PlayerHUD __instance, ref bool useFirstAvatar)
        {
            if (!Main.enabled)
                return;

            try
            {
                bool isUsingSpecialFrame = Traverse.Create(typeof(PlayerHUD)).Field("isUsingSpecialFrame").GetValue<bool>(); //Get the "isUsingSpecialFrame" original Value
                
                if (isUsingSpecialFrame)
                {
                    __instance.StopUsingSpecialFrame();
                }
                Traverse.Create(typeof(PlayerHUD)).Field("SetToDead").SetValue(true); //Change the value "SetToDead" to true

                //Set sprite
                string filePath = mod.Path + "/skeletonFace.png";

                SpriteSM sprite = Main.Setup(filePath, ref __instance); // SpriteSM require otherwise he won't work
                
                Traverse.Create(typeof(PlayerHUD)).Field("avatar").SetValue(sprite); //Change the avatar to the skeleton
                Traverse.Create(typeof(PlayerHUD)).Field("secondAvatar").SetValue(sprite); //Change the second avatar to the skeleton
                   

                SpriteSM spriteSM = (!useFirstAvatar) ? __instance.secondAvatar : __instance.avatar;
                if (spriteSM != null)
                {
                    spriteSM.SetLowerLeftPixel(new Vector2(96f, spriteSM.lowerLeftPixel.y)); 
                }
            }
            catch(Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }
    }

}
