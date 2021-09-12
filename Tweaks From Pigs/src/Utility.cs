using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using UnityEngine;
using HarmonyLib;

namespace TweaksFromPigs
{
    public class Utility
    {

        public static List<HeroType> HeroList = new List<HeroType> { HeroType.Rambro, HeroType.Brommando, HeroType.BaBroracus, HeroType.BrodellWalker, HeroType.BroHard, HeroType.McBrover, HeroType.Blade, HeroType.BroDredd, HeroType.Brononymous, HeroType.DirtyHarry, HeroType.Brominator, HeroType.Brobocop, HeroType.IndianaBrones, HeroType.AshBrolliams, HeroType.Nebro, HeroType.BoondockBros, HeroType.Brochete, HeroType.BronanTheBrobarian, HeroType.EllenRipbro, HeroType.TheBrocketeer, HeroType.TimeBroVanDamme, HeroType.BroniversalSoldier, HeroType.ColJamesBroddock, HeroType.CherryBroling, HeroType.BroMax, HeroType.TheBrode, HeroType.DoubleBroSeven, HeroType.Predabro, HeroType.BroveHeart, HeroType.TheBrofessional, HeroType.Broden, HeroType.TheBrolander, HeroType.SnakeBroSkin, HeroType.TankBro, HeroType.BroLee, HeroType.BroneyRoss, HeroType.LeeBroxmas, HeroType.BronnarJensen, HeroType.HaleTheBro, HeroType.TrentBroser, HeroType.Broc, HeroType.TollBroad, HeroType.BrondleFly };
        public static List<int> HeroInt = new List<int> { 1, 3, 5, 8, 11, 15, 20, 25, 31, 37, 46, 56, 65, 75, 87, 99, 115, 132, 145, 160, 175, 193, 222, 249, 274, 300, 326, 350, 374, 400, 420, 440, 465, 460, 480, 500, 510, 524, 534, 540, 548, 560, 600 };
        public static Dictionary<int, HeroType> BuildHeroDictionary()
        {
            Dictionary<int, HeroType> HeroDictionary = new Dictionary<int, HeroType>();

            for (int i = 0; i< HeroList.Count; i++)
            {
                HeroType hero = HeroList[i];
                if(HeroUnlockController.IsExpendaBro(hero) && Main.settings.SpawnWithExpendabros)
                {
                    /* Bronney Ross : 500
                     * LeeBroxmas : 510
                     * Bronnar Jensen : 524
                     * Bro Ceasar : 534
                     * Trent Broser : 540
                     * Broc : 548
                     * Toll broad : 560
                     */
                    HeroDictionary.Add(HeroInt[i], hero);
                }
                else if(hero == HeroType.BrondleFly && Main.settings.SpawnBrondeFly)
                {
                    // Brondle Fly : 600
                    HeroDictionary.Add(HeroInt[i], hero);
                }
                else
                {
                    // Basic Step
                    HeroDictionary.Add(HeroInt[i], hero);
                }
            }
            return HeroDictionary;
        }

        public static Texture2D CreateTexFromMat(string filename, Material origMat)
        {
            if (!File.Exists(Main.mod.Path + "/Ressource/" + filename)) throw new IOException();

            Texture2D tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            tex.LoadImage(File.ReadAllBytes(Main.mod.Path + "/Ressource/" + filename));
            tex.wrapMode = TextureWrapMode.Clamp;

            Texture orig = origMat.mainTexture;

            tex.anisoLevel = orig.anisoLevel;
            tex.filterMode = orig.filterMode;
            tex.mipMapBias = orig.mipMapBias;
            tex.wrapMode = orig.wrapMode;

            return tex;
        }

        public static Texture2D CreateTexFromSpriteSM(string filename, SpriteSM sprite)
        {
            if (!File.Exists(Main.mod.Path + "/Ressource/" + filename)) throw new IOException();

            Texture2D tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            tex.LoadImage(File.ReadAllBytes(Main.mod.Path + "/Ressource/" + filename));
            tex.wrapMode = TextureWrapMode.Clamp;

            Texture orig = sprite.meshRender.sharedMaterial.GetTexture("_MainTex");

            tex.anisoLevel = orig.anisoLevel;
            tex.filterMode = orig.filterMode;
            tex.mipMapBias = orig.mipMapBias;
            tex.wrapMode = orig.wrapMode;

            return tex;
        }

        public static SpriteSM CreateSpriteSMForAvatar(string filename, ref PlayerHUD PHUD) //Setup the sprite
        {
            if (!File.Exists(Main.mod.Path + "/Ressource/" + filename)) throw new IOException();

            Texture2D tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            tex.LoadImage(File.ReadAllBytes(Main.mod.Path + "/Ressource/" + filename));
            tex.wrapMode = TextureWrapMode.Clamp;

            SpriteSM sprite = PHUD.avatar.gameObject.GetComponent<SpriteSM>();

            Texture orig = sprite.meshRender.sharedMaterial.GetTexture("_MainTex");

            tex.anisoLevel = orig.anisoLevel;
            tex.filterMode = orig.filterMode;
            tex.mipMapBias = orig.mipMapBias;
            tex.wrapMode = orig.wrapMode;

            sprite.meshRender.sharedMaterial.SetTexture("_MainTex", tex);

            return sprite;
        }
    }
}
