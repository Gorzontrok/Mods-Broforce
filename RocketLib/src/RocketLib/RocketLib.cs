using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityModManagerNet;
using RocketLibLoadMod;

namespace RocketLib0
{

    /// <summary>
    /// Library of useful function made for Broforce. And an  Logger on screen.
    /// </summary>
    public static partial class RocketLib
    {
        /// <summary>
        /// Know if RocketLib is Loaded
        /// </summary>
        public static bool Loaded { get; private set; }

        internal static void Load()
        {
            if(!Loaded)
            {
                try
                {
                   ScreenLogger.IsLoaded = ScreenLogger.Load();
                }
                catch (Exception ex)
                {
                    Main.Log("Error while loading RocketLib" + ex);
                }
                Loaded = true;
            }
            else
            {
                Main.Log("Cancel Load, already Started");
            }
        }

        /// <summary>
        /// Create a Texture from a material. If the given path does not exist, it will return the given Texture.
        /// </summary>
        /// <param name="ImagePath">Image path</param>
        /// <param name="material">Original Material</param>
        /// <returns>A Texture</returns>
        public static Texture CreateTexFromMat(string ImagePath, Material material)
        {
            return CreateTexFromTexture(ImagePath, Material.Instantiate(material).mainTexture);
        }

        /// <summary>
        /// Create a Texture from SpriteSM. If the given path does not exist, it will return the given Texture.
        /// </summary>
        /// <param name="ImagePath">Image Path</param>
        /// <param name="sprite">Original Sprite</param>
        /// <returns>A Texture</returns>
        public static Texture CreateTexFromSpriteSM(string ImagePath, SpriteSM sprite)
        {
            return CreateTexFromTexture(ImagePath, Material.Instantiate(sprite.GetComponent<Renderer>().sharedMaterial).GetTexture("_MainTex"));
        }

        /// <summary>
        /// Create a Texture from a MeshRenderer. If the given path does not exist, it will return the given Texture.
        /// </summary>
        /// <param name="ImagePath"></param>
        /// <param name="meshRenderer"></param>
        /// <returns></returns>
        public static Texture CreateTexFromMeshRenderer(string ImagePath, MeshRenderer meshRenderer)
        {
            return CreateTexFromTexture(ImagePath, Material.Instantiate(meshRenderer.sharedMaterial).mainTexture);
        }

        /// <summary>
        /// Create a Texture from a Renderer. If the given path does not exist, it will return the given Texture.
        /// </summary>
        /// <param name="ImagePath"></param>
        /// <param name="Renderer"></param>
        /// <returns></returns>
        public static Texture CreateTexFromRenderer(string ImagePath, Renderer Renderer)
        {
            return CreateTexFromTexture(ImagePath, Material.Instantiate(Renderer.material).mainTexture);
        }

        /// <summary>
        /// Create a Texture from an existing one. If the given path does not exist, it will return the given Texture.
        /// </summary>
        /// <param name="ImagePath"></param>
        /// <param name="texture-"></param>
        /// <returns></returns>
        public static Texture CreateTexFromTexture(string ImagePath, Texture texture)
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
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return texture;
        }

        /// <summary>
        /// This class contains my GUILayout.
        /// </summary>
        public static class RGUI
        {
            /// <summary>
            /// A custom GUILayout. You make a choice with arrow.
            /// </summary>
            /// <param name="StringsArray">The values that are show.</param>
            /// <param name="Number">The current index.</param>
            /// <returns>Index of the array</returns>
            public static int ArrowList(string[] StringsArray, int Number)
            {
                GUIStyle LeftArrowStyle = new GUIStyle("button");
                GUIStyle RightArrowStyle = new GUIStyle("button");

                GUILayout.BeginHorizontal();

                LeftArrowStyle = ChangeArrowStyle(LeftArrowStyle, Number > 0);

                if (GUILayout.Button("<", LeftArrowStyle, GUILayout.ExpandWidth(false)))
                {
                    if (Number > 0)
                    {
                        Number--;
                    }
                }
                GUILayout.Label(StringsArray[Number].ToString(), GUILayout.ExpandWidth(false));

                RightArrowStyle = ChangeArrowStyle(RightArrowStyle, Number < StringsArray.Length - 1);

                if (GUILayout.Button(">", RightArrowStyle, GUILayout.ExpandWidth(false)))
                {
                    if (Number < StringsArray.Length - 1)
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
            /// <param name="StringsArray">The values that are show.</param>
            /// <param name="Number">The current index.</param>
            /// <param name="Width"></param>
            /// <returns>Index of the array</returns>
            public static int ArrowList(string[] StringsArray, int Number, int Width)
            {
                GUIStyle LeftArrowStyle = new GUIStyle("button");
                GUIStyle RightArrowStyle = new GUIStyle("button");

                GUILayout.BeginHorizontal(GUILayout.Width(Width));

                LeftArrowStyle = ChangeArrowStyle(LeftArrowStyle, Number > 0);

                if (GUILayout.Button("<", LeftArrowStyle, GUILayout.ExpandWidth(false)))
                {
                    if (Number > 0)
                    {
                        Number--;
                    }
                }
                GUILayout.FlexibleSpace();
                GUILayout.Label(StringsArray[Number].ToString());
                GUILayout.FlexibleSpace();

                RightArrowStyle = ChangeArrowStyle(RightArrowStyle, Number < StringsArray.Length - 1);

                if (GUILayout.Button(">", RightArrowStyle, GUILayout.ExpandWidth(false)))
                {
                    if (Number < StringsArray.Length - 1)
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
            /// <param name="ObjectsArray">The values that are show.</param>
            /// <param name="Number">The current index.</param>
            /// <returns>Index of the array</returns>
            public static int ArrowList(object[] ObjectsArray, int Number)
            {
                string[] StrArray = ObjectsArray.Select(obj => obj.ToString()).ToArray();
                return ArrowList(StrArray, Number);
            }

            /// <summary>
            /// A custom GUILayout. You make a choice with arrow.
            /// </summary>
            /// <param name="ObjectsArray">The values that are show.</param>
            /// <param name="Number">The current index.</param>
            /// <param name="Width"></param>
            /// <returns>Index of the array</returns>
            public static int ArrowList(object[] ObjectsArray, int Number, int Width)
            {
                string[] StrArray = ObjectsArray.Select(obj => obj.ToString()).ToArray();
                return ArrowList(StrArray, Number, Width);
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="Strings"></param>
            /// <param name="Number"></param>
            /// <param name="Space"></param>
            /// <param name="TabWidth"></param>
            /// <returns></returns>
            public static int Tab(string[] Strings, int Number, int Space, int TabWidth)
            {
                var TabStyle = new GUIStyle("button");
                var ActiveTabStyle = new GUIStyle("button");
                ActiveTabStyle.normal.background = ActiveTabStyle.hover.background;
                GUILayout.BeginHorizontal();
                for(int i = 0; i < Strings.Length; i++)
                {
                    if (GUILayout.Button(Strings[i],(i == Number ? ActiveTabStyle : TabStyle), GUILayout.Width(TabWidth))) return i;
                    GUILayout.Space(Space);
                }
                GUILayout.EndHorizontal();
                return Number;
            }

            private static GUIStyle ChangeArrowStyle(GUIStyle Style, bool ToCheck)
            {
                if (ToCheck)
                {
                    Style.normal.textColor = Color.white;
                    Style.hover.textColor = Color.white;
                    Style.active.textColor = Color.white;
                }
                else
                {
                    Style.normal.textColor = Color.gray;
                    Style.hover.textColor = Color.gray;
                    Style.active.textColor = Color.gray;
                }
                return Style;
            }
            /*
            /// <summary>
            ///
            /// </summary>
            /// <param name="width"></param>
            /// <param name="height"></param>
            /// <param name="options"></param>
            /// <param name="setIndexNumber"></param>
            /// <returns></returns>
            public static int DropdownList(float width, float height, string[] options, int setIndexNumber)
            {
                var dropdown = new Dropdown(width, height, options, setIndexNumber);
                dropdown.OnGUI();
                return dropdown.IndexNumber;
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="width"></param>
            /// <param name="height"></param>
            /// <param name="options"></param>
            /// <param name="setIndexNumber"></param>
            /// <returns></returns>
            public static int DropdownList(float width, float height, List<string> options, int setIndexNumber)
            {
                var dropdown = new Dropdown(width, height, options.ToArray(), setIndexNumber);
                dropdown.OnGUI();
                return dropdown.IndexNumber;
            }

            /// <summary>
            ///
            /// </summary>
            public class Dropdown
            {
                // Original by Alexneargarder
                // https://github.com/alexneargarder/BroforceMods/blob/master/Utility%20Mod/Utility%20Mod/Main.cs#L657

                /// <summary>
                /// The index to return.
                /// </summary>
                public int IndexNumber = 0;

                private Vector2 scrollViewVector = Vector2.zero;
                private string[] OptionsArray = new string[] { };

                private float Width = 0f;
                private float Height = 0f;

                private bool Show = false;

                /// <summary>
                /// Create a new Dropdown
                /// </summary>
                /// <param name="width"></param>
                /// <param name="height"></param>
                /// <param name="options"></param>
                /// <param name="setIndexNumber"></param>
                /// <param name="setShow"></param>
                public Dropdown(float width, float height, string[] options, int setIndexNumber, bool setShow = false)
                {
                    Width = width;
                    Height = height;
                    OptionsArray = options;
                    IndexNumber = setIndexNumber;
                    Show = setShow;
                }

                /// <summary>
                ///
                /// </summary>
                public void OnGUI()
                {
                    if (GUILayout.Button(OptionsArray[IndexNumber], GUILayout.Width(Width)))
                    {
                        if (Show)
                        {
                            Show = false;
                            Main.Log("hide");
                        }
                        else
                        {
                            Show = true;
                            Main.Log("show");
                        }
                    }

                    if (Show)
                    {
                        GUILayout.BeginVertical("box");
                        scrollViewVector = GUILayout.BeginScrollView(scrollViewVector, GUILayout.Height(Height));

                        for (int index = 0; index < OptionsArray.Length; index++)
                        {
                            Main.Log(index);
                            if (GUILayout.Button(OptionsArray[index], GUILayout.Width(Width)))
                            {
                                Show = false;
                                Main.Log("couco");
                                IndexNumber = index;
                            }
                        }
                        GUI.EndScrollView();
                        GUILayout.EndVertical();
                    }
                }
            }*/
        }
    }
}