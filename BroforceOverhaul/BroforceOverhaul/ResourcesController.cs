﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using RocketLib;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

    public static class ResourcesController
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


        private static string AssetsFolder
        {
            get
            {
                return Path.Combine(Main.mod.Path, "assets_override");
            }
        }

    public static string resourceFolder = "BroforceOverhaul.Assets";

        private static Dictionary<string, Material> materialResources = new Dictionary<string, Material>();

        public static Material GetMaterialResource(string resourceName, Shader shader)
        {
            Material result;
            if(materialResources.ContainsKey(resourceName))
            {
                return materialResources[resourceName];
            }
            else
            {
                result = CreateMaterial(resourceName, shader);
                if(result != null)
                {
                    materialResources.Add(resourceName, result);
                }
            }
            return result;
        }

        public static Material CreateMaterial(string imageName, Shader shader)
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
                Main.ExceptionLog(ex);
            }
            return null;
        }

        public static Texture2D CreateTexture(string imageName)
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
        public static Texture2D CreateTexture(byte[] imageBytes)
        {
            Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            tex.LoadImage(imageBytes);
            return tex;
        }

        public static byte[] ExtractResource(string filename)
        {
            Assembly a = Assembly.GetExecutingAssembly();
            using (Stream resFilestream = a.GetManifestResourceStream(resourceFolder + filename))
            {
                if (resFilestream == null) return null;
                byte[] ba = new byte[resFilestream.Length];
                resFilestream.Read(ba, 0, ba.Length);
                return ba;
            }
        }

        private static string GetFilePath(string imageName)
        {
            string[] s = imageName.Split('.');
            string path = AssetsFolder;

            for (int i = 0; i < s.Length - 2; i++)
            {
                Path.Combine(path, s[i]);
            }
            path = Path.Combine(path, s[s.Length - 2] + "." + s[s.Length - 1]);
            return path;
        }
    }


