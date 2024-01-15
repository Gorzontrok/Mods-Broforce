using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TFBGames.Systems;

namespace DresserMod
{
    public static class ResourcesController
    {
        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public static Texture2D GetTexture(string path, string fileName)
        {
            Texture2D tex = null;
            textures.TryGetValue(fileName, out tex);
            if (tex != null)
                return tex;

            if (fileName.Contains(":"))
            {
                try
                {
                    tex = LoadAssetSync<Texture2D>(fileName);
                }
                catch (Exception ex)
                {
                    Main.Log(ex);
                }
            }
            else
            {
                tex = CreateTexture(path, fileName);
            }

            if (tex != null)
            {
                textures.Add(fileName, tex);
            }
            return tex;
        }

        public static Texture2D CreateTexture(string path, string fileName)
        {
            return CreateTexture(Path.Combine(path, fileName));
        }

        public static Texture2D CreateTexture(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found", filePath);

            return CreateTexture(File.ReadAllBytes(filePath));
        }

        public static Texture2D CreateTexture(byte[] imageBytes)
        {
            if (imageBytes.IsNullOrEmpty())
                throw new ArgumentException("Is null or empty", nameof(imageBytes));

            Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            tex.LoadImage(imageBytes);
            tex.filterMode = FilterMode.Point;
            tex.anisoLevel = 1;
            tex.mipMapBias = 0;
            tex.wrapMode = TextureWrapMode.Repeat;
            return tex;
        }

        public static T LoadAssetSync<T>(string name)
            where T : UnityEngine.Object
        {
            return GameSystems.ResourceManager.LoadAssetSync<T>(name);
        }
    }
}


