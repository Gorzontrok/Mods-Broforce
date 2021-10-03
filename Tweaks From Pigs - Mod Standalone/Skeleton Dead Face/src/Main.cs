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
            mod = modEntry;

            modEntry.OnToggle = OnToggle;
            var harmony = new Harmony(modEntry.Info.Id);
            var assembly = Assembly.GetExecutingAssembly();
            harmony.PatchAll(assembly);

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

        public static Texture2D CreateTexFromSpriteSM(string ImagePath, SpriteSM sprite)
        {
            if (!File.Exists(ImagePath)) throw new IOException();

            Texture2D tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            tex.LoadImage(File.ReadAllBytes(ImagePath));
            tex.wrapMode = TextureWrapMode.Clamp;

            Texture orig = sprite.meshRender.sharedMaterial.GetTexture("_MainTex");

            tex.anisoLevel = orig.anisoLevel;
            tex.filterMode = orig.filterMode;
            tex.mipMapBias = orig.mipMapBias;
            tex.wrapMode = orig.wrapMode;

            return tex;
        }

    }

    [HarmonyPatch(typeof(PlayerHUD), "SetAvatarDead")]
    static class SetAvatarDead_Patch
    {
        static void Prefix(PlayerHUD __instance)
        {
            if (!Main.enabled)
                return;

            SpriteSM sprite = __instance.avatar.gameObject.GetComponent<SpriteSM>();
            sprite.meshRender.sharedMaterial.SetTexture("_MainTex", Main.CreateTexFromSpriteSM("SkeletonDeadFace.png", sprite));

            Traverse.Create(typeof(PlayerHUD)).Field("avatar").SetValue(sprite); //Change the avatar to the skeleton
            Traverse.Create(typeof(PlayerHUD)).Field("secondAvatar").SetValue(sprite); //Change the second avatar to the skeleton
        }
    }

}
