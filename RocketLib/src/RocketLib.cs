using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityModManagerNet;
using RocketLibLoadMod;

namespace RocketLib0
{

    /// <summary>
    /// Library of useful function made for Broforce. And an  Logger on screen.
    /// </summary>
    public partial class RocketLib
    {
        /// <summary>
        /// Give the '_Data' folder of the game.
        /// </summary>
        public static string GameDataPath = Application.dataPath;

        internal static void Load() // Function for load all of function we needed.
        {
            try
            {
                ScreenLogger._isSuccessfullyLoad = ScreenLogger.Load();
                ScreenLogger.AddStartLog("Successful loaded RocketLib !");
            }
            catch(Exception ex)
            {
                Main.Log("Failed to load RocketLib !" + ex);
            }
        }

        /// <summary>
        /// Create a Texture based on an existing material.
        /// </summary>
        /// <param name="ImagePath">Image path</param>
        /// <param name="origMat">Original Material</param>
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
        public static class RGUI
        {
            /// <summary>
            /// A custom GUILayout. You make a choice with arrow.
            /// </summary>
            /// <param name="ObjectList">The given list where you can choose a value.</param>
            /// <param name="Number">The number who make the choice.</param>
            /// <returns>Int for the list</returns>
            public static int ArrowList(List<object> ObjectList, int Number)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("<", GUILayout.ExpandWidth(false)))
                {
                    if (Number > 0)
                    {
                        Number--;
                    }
                }
                GUILayout.Label(ObjectList[Number].ToString(), GUILayout.ExpandWidth(false));
                if (GUILayout.Button(">", GUILayout.ExpandWidth(false)))
                {
                    if (Number < ObjectList.Count - 1)
                    {
                        Number++;
                    }
                }
                GUILayout.EndHorizontal();

                return Number;
            }

            /// <summary>
            /// A custom GUILayout. You make a choice with arrow.
            /// </summary>
            /// <param name="ObjectList">The given list where you can choose a value.</param>
            /// <param name="Number">The number who make the choice.</param>
            /// <param name="width"></param>
            /// <returns>Int for the list</returns>
            public static int ArrowList(List<object> ObjectList, int Number, int width)
            {
                GUILayout.BeginHorizontal(GUILayout.Width(width));
                if (GUILayout.Button("<", GUILayout.ExpandWidth(false)))
                {
                    if (Number > 0)
                    {
                        Number--;
                    }
                }
                GUILayout.FlexibleSpace();
                GUILayout.Label(ObjectList[Number].ToString());
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(">", GUILayout.ExpandWidth(false)))
                {
                    if (Number < ObjectList.Count - 1)
                    {
                        Number++;
                    }
                }
                GUILayout.EndHorizontal();

                return Number;
            }
        }
    }
}