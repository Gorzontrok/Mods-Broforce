using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using RocketLibLoadMod; // for call Main.Log(); when i test function.

namespace RocketLib
{

    /// <summary>
    /// Library of useful function made for Broforce. And an OnScreen Logger.
    /// </summary>
    public static partial class RocketLib
    {
        /// <summary>
        /// Give the '_Data' folder of the game. Be careful ! If the mod is not load, it will be empty !
        /// </summary>
        public static string GameDataPath = "";

        internal static string GameDirectory = Directory.GetCurrentDirectory();

        internal static string RocketLibModFolderPath = Main.mod.Path;

        internal static void Load() // Function for load all of function we needed.
        {
            try
            {
                ScreenLogger.Load();
                GetGameDataPath();
                ScreenLogger.firstLaunch.Add(" Succesfully Load RocketLib !");
            }
            catch(Exception ex)
            {
                Main.Log("Failed to load RocketLib !\n" + ex);
            }
        }

        static void GetGameDataPath() // Get the Data folder of the game.
        {
            string[] dirs = Directory.GetDirectories(GameDirectory, "*_Data");
            foreach (string dir in dirs)
            {
                GameDataPath = dir;
            }
        }

        /// <summary>This constructor check if a mod is Here or is Enabled.
        /// <example>
        /// Example of call :
        /// <code>
        ///     RocketLib.IsThisModIs RocketLib_info = new RocketLib.IsThisModIs("RocketLib");
        /// </code>
        /// </example>
        /// </summary>
        public class IsThisModIs
        {
            private static string xmlFilePath = GameDataPath + "/Managed/UnityModManager/Params.xml";

            /// <summary>
            /// Return if the mod is Here.
            /// </summary>
            public bool Here;

            /// <summary>
            /// Return is the mod is Enabled, if the mod does not exist it will return false.
            /// </summary>
            public bool Enabled;

            /// <summary>This constructor check if a mod is Here or is Enabled.
            /// <example>
            /// Example of call :
            /// <code>
            ///     RocketLib.IsThisModIs RocketLib_info = new RocketLib.IsThisModIs("RocketLib");
            /// </code>
            /// </example>
            /// </summary>
            /// <param name="ID">Id of the mod.</param>
            public IsThisModIs(string ID) // Check if the mod is here and it's enabled
            {
                XmlDocument mod = new XmlDocument();
                mod.Load(xmlFilePath); // Initialize the XML Document

                XmlNode node = mod.SelectSingleNode("//ModParams");// Get the group <ModParams>
                Here = false;
                Enabled = false;
                while(true)
                {
                    foreach (XmlNode mods in node) // Get each attribute of each <Mod Id="" Enabled="" />
                    {
                        if (mods.Attributes["Id"].Value == ID)// Here we need only the ID of the mod
                        {
                            Here = true;
                            if (mods.Attributes["Enabled"].Value == "true" && mods.Attributes["Id"].Value == ID)
                            {
                                Enabled = true;
                            }
                        }
                    }
                }
            }
        }
        /*
        /// <summary>
        /// Convert an image file to a Texture2D.
        /// </summary>
        /// <param name="ImagePath">Path of the image.</param>
        private static Texture2D ImageToTexture2D(string ImagePath) // Convert a image to a Texture2D
        {
            Texture2D texture;
            byte[] fileData;

            fileData = File.ReadAllBytes(ImagePath);

            texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            texture.LoadImage(fileData);
            return texture;
        }*/

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