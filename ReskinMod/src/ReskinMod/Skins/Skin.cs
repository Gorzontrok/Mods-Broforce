using System;
using System.Linq;
using System.IO;
using UnityEngine;

namespace ReskinMod.Skins
{
    public class Skin
    {
        public Texture texture;
        public SkinType skinType;
        public readonly string path;
        public int skinNumber = 0;

        public Skin(string p)
        {
            path = p;
            GetSkinType();
            texture = CreateTexture();
        }

        private void GetSkinType()
        {
            string fileNameNoExtension = Path.GetFileNameWithoutExtension(path).ToLower();

            if (fileNameNoExtension.Contains("_gun_anim"))
            {
                skinType = SkinType.Gun;
            }
            else if (fileNameNoExtension.Contains("_armless_anim"))
            {
                skinType = SkinType.Armless;
            }
            else if (fileNameNoExtension.Contains("_decapitated_anim"))
            {
                skinType = SkinType.Decapitated;
            }
            else if (fileNameNoExtension.Contains("_anim"))
            {
                skinType = SkinType.Character;
            }
            else if (fileNameNoExtension.Contains("_avatar"))
            {
                skinType = SkinType.Avatar;
            }
            else
            {
                skinType = SkinType.None;
            }

            if (skinType != SkinType.None)
            {
                skinNumber = fileNameNoExtension.Last() - '0';
            }
        }

        private Texture CreateTexture()
        {
            Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            tex.LoadImage(File.ReadAllBytes(path));
            tex.filterMode = FilterMode.Point;
            tex.anisoLevel = 1;
            tex.mipMapBias = 0;
            tex.wrapMode = TextureWrapMode.Repeat;

            return tex;
        }

        public override string ToString()
        {
            return skinType.ToString() + " " + skinNumber.ToString();
        }
    }

    public enum SkinType
    {
        None,
        Character,
        Gun,
        Armless,
        Decapitated,
        Avatar,
    }
}
