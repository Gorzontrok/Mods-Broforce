using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace TweaksFromPigs
{
    public class Utility
    {
        public static SpriteSM CreateSpriteSMForAvatar(string filename, ref PlayerHUD PHUD) //Setup the sprite
        {

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


        private static int BrondeflyStep = 621;
        public static void BuildHeroDictionary()
        {
            AddRemoveExpendabros();
            if (Main.settings.SpawnBrondeFly)
            {
                if (!Main.HeroDictionary.ContainsKey(BrondeflyStep)) Main.HeroDictionary.Add(BrondeflyStep, HeroType.BrondleFly);
            }
            else
            {
                if (Main.HeroDictionary.ContainsKey(BrondeflyStep)) Main.HeroDictionary.Remove(BrondeflyStep);
            }
        }

        static void AddRemoveExpendabros()
        {
            Dictionary<int, HeroType> ExpendablesBro_dico = new Dictionary<int, HeroType>() {
                { 42, HeroType.BroneyRoss },
                { 52, HeroType.LeeBroxmas },
                { 62, HeroType.BronnarJensen },
                { 72, HeroType.HaleTheBro },
                { 82, HeroType.TrentBroser },
                { 92, HeroType.Broc },
                { 102, HeroType.TollBroad }
            };

            if (Main.settings.SpawnWithExpendabros)
            {
                foreach (KeyValuePair<int, HeroType> hero in ExpendablesBro_dico)
                {
                    if (!Main.HeroDictionary.ContainsKey(hero.Key)) Main.HeroDictionary.Add(hero.Key, hero.Value);
                }
            }

            if (!Main.settings.SpawnWithExpendabros)
            {
                foreach (KeyValuePair<int, HeroType> hero in ExpendablesBro_dico)
                {
                    if (Main.HeroDictionary.ContainsKey(hero.Key)) Main.HeroDictionary.Remove(hero.Key);
                }
            }
        }

        public static Texture2D CreateTexFromMat(string filename, Material origMat)
        {
            Texture2D tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            tex.LoadImage(File.ReadAllBytes(Main.mod.Path + "/Ressource/" + filename));
            tex.wrapMode = TextureWrapMode.Clamp;


            // Texture orig = sprite.meshRender.sharedMaterial.GetTexture("_MainTex");
            Texture orig = origMat.mainTexture;

            tex.anisoLevel = orig.anisoLevel;
            tex.filterMode = orig.filterMode;
            tex.mipMapBias = orig.mipMapBias;
            tex.wrapMode = orig.wrapMode;

            return tex;
        }
    }
}
