using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RocketLib0;
using HarmonyLib;

namespace TweaksFromPigs
{
    internal class Compatibility
    {

        internal static IsThisModTFP ForBralef;
        internal static IsThisModTFP FilteredBros;
        internal static IsThisModTFP ExpendablesBros;
        internal static IsThisModTFP _007_Patch;
        internal static IsThisModTFP AvatarFaceHugger;
        internal static IsThisModTFP SkeletonDeadFace ;
        internal static IsThisModTFP MapDataController;

        internal static List<IsThisModTFP> BroforceModsList = new List<IsThisModTFP>();
        internal static void Load()
        {
            ForBralef = new IsThisModTFP("ForBralef");
            FilteredBros = new IsThisModTFP("FilteredBrosMod");
            ExpendablesBros = new IsThisModTFP("ExpendaBrosInGame");
            _007_Patch = new IsThisModTFP("007_Patch");
            AvatarFaceHugger = new IsThisModTFP("AvatarFaceHuggerMod");
            SkeletonDeadFace = new IsThisModTFP("SkeletonDeadFaceMod");
            MapDataController = new IsThisModTFP("MapDataControllerMod");
        }

        internal static bool GetCompatibilityBool(string id)
        {
            switch(id)
            {
                case "ForBralef":
                    return Main.settings.ForBralef_Compatibility;
                case "FilteredBrosMod":
                    return Main.settings.FilteredBros_Compatibility;
                case "ExpendaBrosInGame":
                    return Main.settings.ExpendablesBros_Compatibility;
                case "007_Patch":
                    return Main.settings._007Patch_Compatibility;
                case "AvatarFaceHuggerMod":
                    return Main.settings.AvatarFaceHugger_Compatibility;
                case "SkeletonDeadFaceMod":
                    return Main.settings.SkeletonDeadFace_Compatibility;
            }
            return false;
        }

        internal class IsThisModTFP
        {
            public IsThisMod i;
            public IsThisModTFP(string _ID)
            {
                this.i = new IsThisMod(_ID);
                BroforceModsList.Add(this);
            }
        }
    }
}
