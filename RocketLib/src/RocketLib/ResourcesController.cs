using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;
//using RocketLibUMM;

namespace RocketLib
{
    public class ResourcesController
    {
        /// <summary>
        /// Particles/Alpha Blended
        /// </summary>
        public static Shader Particle_AlphaBlend
        {
            get
            {
                return Shader.Find("Particles/Alpha Blended");
            }
        }

        /// <summary>
        /// Unlit/Depth Cutout With ColouredImage
        /// </summary>
        public static Shader Unlit_DepthCutout
        {
            get
            {
                return Shader.Find("Unlit/Depth Cutout With ColouredImage");
            }
        }

        /// <summary>
        /// Particle/Additive
        /// </summary>
        public static Shader Particle
        {
            get
            {
                return Shader.Find("Particle/Additive");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string assetsFolder = string.Empty;
        /// <summary>
        ///
        /// </summary>
        public string resourcesFolder = string.Empty;

        /// <summary>
        ///
        /// </summary>
        /// <param name="_assetsFolder"></param>
        /// <param name="_resourcesFolder"></param>
        public ResourcesController(string _assetsFolder, string _resourcesFolder)
        {
            assetsFolder = _assetsFolder;
            resourcesFolder = _resourcesFolder;
        }

        private Dictionary<string, Material> materialResources = new Dictionary<string, Material>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="shader"></param>
        /// <returns></returns>
        public Material GetMaterialResource(string resourceName, Shader shader)
        {
            Material result;
            if (materialResources.ContainsKey(resourceName))
            {
                return materialResources[resourceName];
            }
            else
            {
                result = CreateMaterial(resourceName, shader);
                if (result != null)
                {
                    materialResources.Add(resourceName, result);
                }
            }
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="shader"></param>
        /// <returns></returns>
        public Material CreateMaterial(string imageName, Shader shader)
        {
            try
            {
                byte[] imageBytes = ExtractResource(imageName);
                string filePath = GetFilePath(imageName);
                if (File.Exists(filePath))
                {
                    imageBytes = File.ReadAllBytes(filePath);
                }
                if (imageBytes != null)
                {
                    Material mat = new Material(shader);
                    Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                    tex.LoadImage(imageBytes);
                    tex.filterMode = FilterMode.Point;
                    tex.anisoLevel = 1;
                    tex.mipMapBias = 0;
                    tex.wrapMode = TextureWrapMode.Repeat;

                    mat.mainTexture = tex;
                    return mat;
                }
            }
            catch (Exception ex)
            {
                //Main.ExceptionLog(ex);
            }
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public static Texture2D CreateTexture(string filePath)
        {
            byte[] imageBytes = ExtractResource(imageName);
            string filePath = GetFilePath(imageName); if (File.Exists(filePath))
            {
                imageBytes = File.ReadAllBytes(filePath);
            }
            if (imageBytes != null)
            {
                return CreateTexture(imageBytes);
            }
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <returns></returns>
        public static Texture2D CreateTexture(byte[] imageBytes)
        {
            Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            tex.LoadImage(imageBytes);
            return tex;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static byte[] ExtractResource(string filePath)
        {
            Assembly a = Assembly.GetExecutingAssembly();
            using (Stream resFilestream = a.GetManifestResourceStream(filePath))
            {
                if (resFilestream == null) return null;
                byte[] ba = new byte[resFilestream.Length];
                resFilestream.Read(ba, 0, ba.Length);
                return ba;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <returns></returns>
        public string GetFilePath(string resourcePath)
        {
            return Path.Combine(assetsFolder, ResourcePathToFilePath(resourcePath));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <returns></returns>
        public static string ResourcePathToFilePath(string resourcePath)
        {
            string[] s = resourcePath.Split('.');
            string path = string.Empty;

            for (int i = 0; i < s.Length - 2; i++)
            {
                Path.Combine(path, s[i]);
            }
            path = Path.Combine(path, s[s.Length - 2] + "." + s[s.Length - 1]);
            return path;
        }
    }
}



