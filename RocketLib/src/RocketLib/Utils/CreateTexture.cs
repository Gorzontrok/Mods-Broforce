using RocketLib.Loggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityModManagerNet;

namespace RocketLib.Utils
{
    /// <summary>
    /// Library of useful function made for Broforce. And an  Logger on screen.
    /// </summary>
    public static class CreateTexture
    {
        public static Texture WithColor(Color color)
        {
            var tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            Color[] colors = new Color[tex.width * tex.height];
            for(int i = 0; i < colors.Length; i++)
            {
                colors[i] = color;
            }
            tex.SetPixels(colors);
            return tex;
        }

        /// <summary>
        /// Create a Texture from a material. If the given path does not exist, it will return the given Texture.
        /// </summary>
        /// <param name="ImagePath">Image path</param>
        /// <param name="material">Original Material</param>
        /// <returns>A Texture</returns>
        public static Texture FromMat(string ImagePath, Material material)
        {
            return FromTexture(ImagePath, Material.Instantiate(material).mainTexture);
        }

        /// <summary>
        /// Create a Texture from SpriteSM. If the given path does not exist, it will return the given Texture.
        /// </summary>
        /// <param name="ImagePath">Image Path</param>
        /// <param name="sprite">Original Sprite</param>
        /// <returns>A Texture</returns>
        public static Texture FromSpriteSM(string ImagePath, SpriteSM sprite)
        {
            return FromTexture(ImagePath, Material.Instantiate(sprite.GetComponent<Renderer>().sharedMaterial).GetTexture("_MainTex"));
        }

        /// <summary>
        /// Create a Texture from a MeshRenderer. If the given path does not exist, it will return the given Texture.
        /// </summary>
        /// <param name="ImagePath"></param>
        /// <param name="meshRenderer"></param>
        /// <returns></returns>
        public static Texture FromMeshRenderer(string ImagePath, MeshRenderer meshRenderer)
        {
            return FromTexture(ImagePath, Material.Instantiate(meshRenderer.sharedMaterial).mainTexture);
        }

        /// <summary>
        /// Create a Texture from a Renderer. If the given path does not exist, it will return the given Texture.
        /// </summary>
        /// <param name="ImagePath"></param>
        /// <param name="Renderer"></param>
        /// <returns></returns>
        public static Texture FromRenderer(string ImagePath, Renderer Renderer)
        {
            return FromTexture(ImagePath, Material.Instantiate(Renderer.material).mainTexture);
        }

        /// <summary>
        /// Create a Texture from an existing one. If the given path does not exist, it will return the given Texture.
        /// </summary>
        /// <param name="ImagePath"></param>
        /// <param name="texture-"></param>
        /// <returns></returns>
        public static Texture FromTexture(string ImagePath, Texture texture)
        {
            try
            {
                if (File.Exists(ImagePath))
                {
                    var tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);

                    tex.LoadImage(File.ReadAllBytes(ImagePath));
                    tex.wrapMode = TextureWrapMode.Clamp;

                    tex.anisoLevel = texture.anisoLevel;
                    tex.filterMode = texture.filterMode;
                    tex.mipMapBias = texture.mipMapBias;
                    tex.wrapMode = texture.wrapMode;

                    return tex;
                }
            }
            catch (Exception ex) { ScreenLogger.Instance.ExceptionLog(ex); }
            return texture;
        }
    }
}