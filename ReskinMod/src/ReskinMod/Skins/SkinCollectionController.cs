using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ReskinMod.Skins
{
    public static class SkinCollectionController
    {
        public static List<SkinCollection> skinCollections = new List<SkinCollection>();
        public static List<string> conflictsAndErrors = new List<string>();

        private static char[] _numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public static void Init()
        {
            try
            {
                skinCollections.Clear();
                BrowseDirectory(Main.assetsFolderPath);
            }
            catch (Exception ex)
            {
                Main.ErrorLog(ex);
            }
        }

        public static SkinCollection GetSkinCollection(string name)
        {
            List<SkinCollection> collections = new List<SkinCollection>();
            foreach (SkinCollection skinCollection in skinCollections)
            {
                if (skinCollection.name == name)
                {
                    collections.Add(skinCollection);
                }
            }
            if(collections.Count > 0)
            {
                return collections[UnityEngine.Random.Range(0, collections.Count)];
            }
            return null;
        }

        public static SkinCollection GetSkinCollection(string name, int i)
        {
            foreach (SkinCollection skinCollection in SkinCollectionController.skinCollections)
            {
                if (skinCollection.name == name && skinCollection.skinCollectionNumber == i)
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
                string fileName = file.Split('\\').Last();
                string fileNameNoExtension = fileName.Split('.')[0].ToLower();
                string skinCollectionName = fileNameNoExtension.Split('_')[0].ToLower();

                string skinNumber = GetStringNumberFromName(skinCollectionName);
                bool isMultiple = skinNumber != "";
                int skinColNum = isMultiple ? int.Parse(skinNumber) : 0;

                SkinCollection skinCollection = GetSkinCollection(skinCollectionName, skinColNum);
                if (skinCollection == null)
                {
                    skinCollection = new SkinCollection(skinCollectionName.Substring(0, skinCollectionName.Length - skinNumber.Length), skinColNum);
                    skinCollections.Add(skinCollection);
                }
                skinCollection.AddNewSkin(file);
            }
            foreach (string d in Directory.GetDirectories(directory))
            {
                BrowseDirectory(d);
            }
        }

        private static string GetStringNumberFromName(string name)
        {
            int numberOfChar = 0;
            for (int i = 1; i < name.Length; i++)
            {
                if (_numbers.Contains(name[name.Length - i]))
                {
                    numberOfChar++;
                }
            }
            return name.Substring(name.Length - numberOfChar);
        }
    }
}
