using System;
using System.Collections.Generic;
using RocketLib0;

namespace TweaksFromPigs
{
    internal static class Compatibility
    {

        internal static IsThisModTFP ForBralef;
        internal static IsThisModTFP FilteredBros;
        internal static IsThisModTFP ExpendablesBros;
        internal static IsThisModTFP _007_Patch;
        internal static IsThisModTFP AvatarFaceHugger;
        internal static IsThisModTFP MapDataController;

        internal static List<IsThisModTFP> BroforceModsList = new List<IsThisModTFP>();
        internal static void Load()
        {
            ForBralef = new IsThisModTFP("ForBralef");
            FilteredBros = new IsThisModTFP("FilteredBrosMod");
            ExpendablesBros = new IsThisModTFP("ExpendaBrosInGame");
            _007_Patch = new IsThisModTFP("007_Patch");
            AvatarFaceHugger = new IsThisModTFP("AvatarFaceHuggerMod");
            MapDataController = new IsThisModTFP("MapDataControllerMod");
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
