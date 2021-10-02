using System;
using System.Reflection;
using System.Xml;
using HarmonyLib;
using RocketLib0;
using RocketLibLoadMod;

namespace RocketLib0
{
    /// <summary>This constructor check if a mod is Here or is Enabled.
    /// <example>
    /// Example of call :
    /// <code>
    ///     var RocketLib_info = new IsThisModIs("RocketLib");
    /// </code>
    /// </example>
    /// </summary>
    public class IsThisMod
    {
        private static string xmlFilePath = RocketLib.GameDataPath + "/Managed/UnityModManager/Params.xml";

        /// <summary>
        /// Return if the mod is Here.
        /// </summary>
        public bool IsHere
        {
            get
            {
                return _IsHere;
            }
        }

        /// <summary>
        /// Return is the mod is Enabled, if the mod does not exist it will return false.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return _IsEnabled;
            }
        }


        private bool _IsHere;
        private bool _IsEnabled;
        /// <summary>This constructor check if a mod is Here or is Enabled. Actually it don't work.
        /// <example>
        /// Example of call :
        /// <code>
        ///     RocketLib.IsThisModIs RocketLib_info = new RocketLib.IsThisModIs("RocketLib");
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="ID">Id of the mod.</param>
        public IsThisMod(string ID) // Check if the mod is here and it's enabled
        {
            XmlDocument mod = new XmlDocument();
            mod.Load(xmlFilePath); // Initialize the XML Document

            XmlNode node = mod.SelectSingleNode("//ModParams");// Get the group <ModParams>
            _IsHere = false;
            _IsEnabled = false;
            while (true)
            {
                foreach (XmlNode mods in node) // Get each attribute of each <Mod Id="" Enabled="" />
                {
                    if (mods.Attributes["Id"].Value == ID)// Here we need only the ID of the mod
                    {
                        _IsHere = true;
                        if (mods.Attributes["Enabled"].Value == "true" && mods.Attributes["Id"].Value == ID)
                        {
                            _IsEnabled = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check if their is Harmony Instance of XXXXXX function in the game.
        /// </summary>
        /// <param name="HarmonyId">The ID of the mod</param>
        /// <param name="MethodInfo">Method Info</param>
        /// <param name="PatchType">The patch type</param>
        /// <returns>bool</returns>
        public static bool HasHarmonyInstance(string HarmonyId, MethodInfo MethodInfo, HarmonyPatchType PatchType)
        {
            // https://harmony.pardeike.net/articles/basics.html#checking-for-existing-patches

            // retrieve all patches
            var patches = Harmony.GetPatchInfo(MethodInfo);
            if (patches is null) return false; // not patched

            // get info about all Prefixes/Postfixes/Transpilers/Finalizer
            if (PatchType == HarmonyPatchType.Prefix)
            {
                foreach (var patch in patches.Prefixes)
                {
                    if (patch.owner == HarmonyId)
                    {
                        Main.Dbg(HarmonyId + " here");
                        return true;
                    }
                }
            }
            else if (PatchType == HarmonyPatchType.Postfix)
            {
                foreach (var patch in patches.Postfixes)
                {
                    if (patch.owner == HarmonyId)
                    {
                        Main.Dbg(HarmonyId + " here");
                        return true;
                    }
                }
            }
            else if (PatchType == HarmonyPatchType.Transpiler)
            {
                foreach (var patch in patches.Transpilers)
                {
                    if (patch.owner == HarmonyId)
                    {
                        Main.Dbg(HarmonyId + " here");
                        return true;
                    }
                }
            }
            else if (PatchType == HarmonyPatchType.Finalizer)
            {
                foreach (var patch in patches.Finalizers)
                {
                    if (patch.owner == HarmonyId)
                    {
                        Main.Dbg(HarmonyId + " here");
                        return true;
                    }
                }
            }
            else
                throw new Exception("The HarmonyPatchType need to be Prefix, Postfix, Transpiler or Finalizer");
            return false;
        }
    }
}
