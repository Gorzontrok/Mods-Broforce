using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using RocketLibLoadMod;

namespace RocketLib0
{

    /// <summary>
    /// Library of useful function made for Broforce. And an  Logger on screen.
    /// </summary>
    public partial class RocketLib
    {
        /// <summary>
        /// Give the '_Data' folder of the game. Be careful ! If the mod is not load, it will be empty !
        /// </summary>
        public static string GameDataPath = "/Broforce_beta_Data";

        internal static string GameDirectory = Directory.GetCurrentDirectory();

        internal static void Load() // Function for load all of function we needed.
        {
            try
            {
                ScreenLogger._isSuccessfullyLoad = ScreenLogger.Load();
                ScreenLogger.AddStartLog("Succesfully Load RocketLib !");
            }
            catch(Exception ex)
            {
                Main.Log("Failed to load RocketLib !\n" + ex);
            }
        }

        /// <summary>
        /// Create a Texture based on an existing material.
        /// </summary>
        /// <param name="ImagePath">Image path</param>
        /// <param name="origMat">Original Materail</param>
        /// <returns>A Texture</returns>
        public static Texture2D CreateTexFromMat(string ImagePath, Material origMat)
        {
            if (!File.Exists(ImagePath)) throw new IOException();

            Texture2D tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            tex.LoadImage(File.ReadAllBytes(ImagePath));
            tex.wrapMode = TextureWrapMode.Clamp;

            Texture orig = origMat.mainTexture;

            tex.anisoLevel = orig.anisoLevel;
            tex.filterMode = orig.filterMode;
            tex.mipMapBias = orig.mipMapBias;
            tex.wrapMode = orig.wrapMode;

            return tex;
        }

        /// <summary>
        /// Create a Texture from SpriteSM
        /// </summary>
        /// <param name="ImagePath">Image Path</param>
        /// <param name="sprite">Original Sprite</param>
        /// <returns>A Texture</returns>
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

        /// <summary>
        /// This class contains my GUILayout.
        /// </summary>
        public static class GUI
        {
            /// <summary>
            /// A custom GUILayout. You make a choice with arrow.
            /// </summary>
            /// <param name="list">The given list where you can choose a value.</param>
            /// <param name="Nbr">The number who make the choice.</param>
            /// <returns>Int for the list</returns>
            public static int ArrowList(List<string> list, int Nbr)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("<", GUILayout.ExpandWidth(false)))
                {
                    if (Nbr > 0)
                    {
                        Nbr--;
                    }
                }
                GUILayout.Label(list[Nbr].ToString(), GUILayout.ExpandWidth(false));
                if (GUILayout.Button(">", GUILayout.ExpandWidth(false)))
                {
                    if (Nbr < list.Count - 1)
                    {
                        Nbr++;
                    }
                }
                GUILayout.EndHorizontal();

                return Nbr;
            }
        }
    }
}