using System;
using System.IO;
using UnityEngine;
using RocketLib0;

namespace ReskinMod
{

    internal static partial class Utility
    {
        // bool ? true : false

        internal static string Mook_Folder;
        internal static string Bro_Folder;
        internal static string HUD_Folder;
        internal static string Interface_Folder;
        internal static string Other_Character_Folder;

        internal static Texture GetTextureFromPath(string ImagePath, Material material)
        {
            if (File.Exists(ImagePath))
            {
                return RocketLib.CreateTexFromMat(ImagePath, material);
            }
            else
            {
                Main.bmod.WarningLog($"'{Path.GetFileName(ImagePath)}' not found.", true);
                return material.mainTexture;
            }
        }


        // BOSS
        public static bool MookIsBoss(Mook mook)
        {
            return (mook as DolphLundrenSoldier) || (mook as SatanMiniboss);
        }
        public static string GetMookBossName(Mook mook)
        {

            if (mook as DolphLundrenSoldier) return "Dolph_Lundren";
            else if (mook as SatanMiniboss) return "Satan_MiniBoss";
            else return "";
        }
        public static string GetMookBossSecondMat(string name)
        {
            switch(name)
            {
                case "Satan_MiniBoss":
                    return "_Stage2";
            }
            return "";
        }






        // VILLAGER - CITIZEN
        // Villager: num = 0 -> male
        // Villager: num = 1 -> female
    }
}

