using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using RocketLib0;

namespace ReskinMod.Skins
{
    public class SkinCollection
    {
        public static List<SkinCollection> skinCollections = new List<SkinCollection>();
        public static List<SkinCollection> villagerSkinCollections = new List<SkinCollection>();
        public static List<string> conflictsAndErrors = new List<string>();

        public readonly string name;
        public readonly int skinCollectionNumber;
        public List<Skin> skins = new List<Skin>();

        public SkinCollection(string n, int i)
        {
            name = n;
            skinCollectionNumber = i;
        }

        public static void Init()
        {
            try
            {
                skinCollections.Clear();
                BrowseDirectory(Main.assetsFolderPath);
            }
            catch(Exception ex)
            {
                Main.ErrorLog(ex);
            }
        }

        public static SkinCollection GetSkinCollection(string name, bool isVillager = false)
        {
            if(isVillager && villagerSkinCollections.Count > 0)
            {
                return villagerSkinCollections[UnityEngine.Random.Range(0, villagerSkinCollections.Count)];
            }
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
            foreach (string file in Directory.GetFiles(directory, "*.png"))
            {
                Main.WarningLog(file);
                string fileName = file.Split('\\').Last();
                string fileNameNoExtension = fileName.Split('.')[0].ToLower();
                string skinCollectionName = fileNameNoExtension.Split('_')[0].ToLower();
                bool isVillager = skinCollectionName.Remove(skinCollectionName.Length - 1) == "villager";

                SkinCollection skinCollection = GetSkinCollection(skinCollectionName);
                if(skinCollection == null)
                {
                    skinCollection = new SkinCollection(skinCollectionName, 0);
                    skinCollections.Add(skinCollection);
                    if(isVillager)
                    {
                        villagerSkinCollections.Add(skinCollection);
                    }
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
            Skin skin = null;
            try
            {
                skin = new Skin(path);
            }
            catch(Exception ex)
            {
                Main.ErrorLog(ex);
            }

            if(skin == null || skin.skinType == SkinType.None || skin.texture == null)
            {
                string msg = $"Failed Create skin for the file '{path}'";
                conflictsAndErrors.Add("<color=\"yellow\">" + msg + "</color>");
                Main.ErrorLog(msg);
                return false;
            }
            else
            {
                Skin skin1 = GetSkin(skin.skinType, skin.skinNumber);
                if (skin1 != null)
                {
                    string msg = $"File conflict :\t{skin.path}\n\t{skin1.path}\nSecond file has been choses";
                    conflictsAndErrors.Add("<color=\"red\">" + msg + "</color>");
                    Main.WarningLog(msg);
                    return false;
                }
                else
                {
                    skins.Add(skin);
                    return true;
                }
            }
        }

        public Skin GetSkin(SkinType skinType, int skinNumber)
        {
            foreach(Skin skin in skins)
            {
                if(skin.skinType == skinType && skin.skinNumber == skinNumber)
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

            if(skinType != SkinType.None)
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
