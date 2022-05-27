using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using RocketLib0;

namespace ReskinMod
{
    public class SkinCollection
    {
        public static List<SkinCollection> skinCollections = new List<SkinCollection>();

        public readonly string name;
        public List<Skin> skins = new List<Skin>();

        public SkinCollection(string n)
        {
            name = n;
        }

        public static void Init()
        {
            skinCollections.Clear();
            BrowseDirectory(Main.assetsFolderPath);
        }

        public static SkinCollection GetSkinCollection(string name)
        {
            foreach(SkinCollection skinCollection in skinCollections)
            {
                if(skinCollection.name == name)
                {
                    return skinCollection;
                }
            }
            return null;
        }

        private static void BrowseDirectory(string directory)
        {
            foreach (string file in Directory.GetFiles(directory))
            {
                string fileName = file.Split('\\').Last();
                string fileNameNoExtension = fileName.Split('.')[0].ToLower();
                string skinCollectionName = fileNameNoExtension.Split('_')[0].ToLower();

                SkinCollection skinCollection = GetSkinCollection(skinCollectionName);
                if(skinCollection == null)
                {
                    skinCollection = new SkinCollection(skinCollectionName);
                    skinCollections.Add(skinCollection);
                }
                skinCollection.AddNewSkin(file);
            }
            foreach (string d in Directory.GetDirectories(directory))
            {
                BrowseDirectory(d);
            }
        }

        public bool AddNewSkin(string path)
        {
            Skin skin = new Skin(path);
            if(skin.skinType == Skin.SkinType.None || skin.texture == null)
            {
                Main.ErrorLog($"Failed Create skin for the file '{path}'");
                return false;
            }
            else
            {
                Skin skin1 = GetSkin(skin.skinType);
                if (skin1 != null)
                {
                    Main.WarningLog($"File conflict :\t{skin.path}\n\t{skin1.path}");
                    return false;
                }
                else
                {
                    skins.Add(skin);
                    return true;
                }
            }
        }

        public Skin GetSkin(Skin.SkinType skinType)
        {
            foreach(Skin skin in skins)
            {
                if(skin.skinType == skinType)
                {
                    return skin;
                }
            }
            return null;
        }
    }

    public class Skin
    {
        public Texture texture;
        public readonly SkinType skinType;
        public readonly string path;

        public Skin(string p)
        {
            path = p;
            string fileName = path.Split('\\').Last();
            skinType = GetSkinType(fileName);
            texture = CreateTexture();
        }

        private SkinType GetSkinType(string fileName)
        {
            string fileNameNoExtension = fileName.Split('.')[0].ToLower();

            if(fileNameNoExtension.Contains("_gun_anim2"))
            {
                return SkinType.Gun2;
            }
            else if (fileNameNoExtension.Contains("_gun_anim"))
            {
                return SkinType.Gun;
            }
            else if (fileNameNoExtension.Contains("_armless_anim"))
            {
                return SkinType.Armless;
            }
            else if (fileNameNoExtension.Contains("_decapitated_anim"))
            {
                return SkinType.Decapitated;
            }
            else if (fileNameNoExtension.Contains("_anim2"))
            {
                return SkinType.Character2;
            }
            else if (fileNameNoExtension.Contains("_anim"))
            {
                return SkinType.Character;
            }
            else if (fileNameNoExtension.Contains("_avatar2"))
            {
                return SkinType.Avatar2;
            }
            else if (fileNameNoExtension.Contains("_avatar"))
            {
                return SkinType.Avatar;
            }
            else
            {
                return SkinType.None;
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

        public enum SkinType
        {
            None,
            Character,
            Gun,
            Armless,
            Decapitated,
            Character2,
            Gun2,
            Avatar,
            Avatar2
        }
    }
}
