using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using RocketLib0;
using UnityEngine;
using HarmonyLib;

namespace TweaksFromPigs
{
    public static class TFP_Utility
    {
        public static bool CanChangeMapValue
        {
            get
            {
                return !Map.isEditing || !LevelSelectionController.loadCustomCampaign || !LevelEditorGUI.IsActive;
            }
        }

        public static Material CreateMaterialFromFile(string ImagePath, Shader shader)
        {
            Material mat = new Material(shader);

            Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            tex.LoadImage(File.ReadAllBytes(Path.Combine(Main.assetsFolder, ImagePath+".png")));
            tex.filterMode = FilterMode.Point;
            tex.anisoLevel = 1;
            tex.mipMapBias = 0;
            tex.wrapMode = TextureWrapMode.Repeat;

            mat.mainTexture = tex;

            return mat;
        }

        public static Material CreateMaterialFromResources(string ImageName, Shader shader)
        {
            try
            {
                Material mat = new Material(shader);
                Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                tex.LoadImage(ExtractResource(ImageName));
                tex.filterMode = FilterMode.Point;
                tex.anisoLevel = 1;
                tex.mipMapBias = 0;
                tex.wrapMode = TextureWrapMode.Repeat;

                mat.mainTexture = tex;
                return mat;
            }
            catch(Exception ex)
            {
                Main.ExceptionLog(ex);
            }
            return CreateMaterialFromFile(ImageName, shader);

        }

        public static byte[] ExtractResource(String filename)
        {
            Assembly a = Assembly.GetExecutingAssembly();
            using (Stream resFilestream = a.GetManifestResourceStream("TweaksFromPigs.assets." + filename + ".png"))
            {
                if (resFilestream == null) return null;
                byte[] ba = new byte[resFilestream.Length];
                resFilestream.Read(ba, 0, ba.Length);
                return ba;
            }
        }

        public static Dictionary<int, HeroType> BuildHeroDictionary()
        {
            List<HeroType> HeroList = new List<HeroType>(RocketLib._HeroUnlockController.Full_HeroType);
            List<int> HeroInt = new List<int>(RocketLib._HeroUnlockController.Hero_Unlock_Intervals);
            HeroInt.AddRange(new int[] { 490, 500, 510, 520, 530, 540, 550, 560 });

            Dictionary<int, HeroType> HeroDictionary = new Dictionary<int, HeroType>();

            for (int i = 0; i< HeroList.Count; i++)
            {
                HeroType hero = HeroList[i];
                if(HeroUnlockController.IsExpendaBro(hero) && Main.settings.spawnWithExpendabros)
                {
                    /* Broney Ross : 490
                     * LeeBroxmas : 500
                     * Bronnar Jensen : 510
                     * Bro Ceasar : 520
                     * Trent Broser : 530
                     * Broc : 540
                     * Toll broad : 550
                     */
                    HeroDictionary.Add(HeroInt[i], hero);
                }
                else if (hero == HeroType.BrondleFly && Main.settings.spawnWithBrondleFly)
                {
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

        public static Vector3 GetBroGunVector3PositionWhenFinishPushing(HeroType hero)
        {
            Vector3 vector = new Vector3(0f, 0f, -0.001f);
            switch (hero)
            {
                case HeroType.Blade: vector = new Vector3(0f, 0f, -1f); break;
                case HeroType.BronanTheBrobarian: vector = new Vector3(3f, 0, -1f); break;
                case HeroType.Nebro: vector = new Vector3(4f, 0, -1f); break;
                case HeroType.TheBrolander: vector = new Vector3(3f, 0, -1f); break;
                case HeroType.HaleTheBro: vector = new Vector3(2f, 0, -1f); break;
                case HeroType.BroveHeart:
                    TestVanDammeAnim broheart = HeroController.GetHeroPrefab(hero);
                    if (!Traverse.Create(broheart).Field("disarmed").GetValue<bool>()) vector = new Vector3(5f, 4, -1f);
                    else vector = new Vector3(3, 0, -1);
                    break;
                case HeroType.BroneyRoss:
                    vector = new Vector3(0, 0, 0); break;
                case HeroType.LeeBroxmas:
                    vector = new Vector3(6, 0, -0.001f); break;
                case HeroType.TheBrode:
                    vector = new Vector3(4, 4, 1); break;
                case HeroType.Brochete:
                    vector = new Vector3(6, 0, 0.001f); break;
            }
            return vector;
        }
        public static Vector3 GetBroGunVector3PositionWhilePushing(HeroType hero)
        {
            Vector3 vector = new Vector3(0f, 0f, -0.001f);
            switch (hero)
            {
                case HeroType.Blade: vector = new Vector3(-4f, 0f, -1f); break;
                case HeroType.BronanTheBrobarian: vector = new Vector3(-3f, 0, -1f); break;
                case HeroType.Nebro: vector = new Vector3(-4f, 0, -1f); break;
                case HeroType.TheBrolander: vector = new Vector3(-3f, 0, -1f); break;
                case HeroType.HaleTheBro: vector = new Vector3(-2f, 0, -1f); break;
                case HeroType.BroveHeart:
                    TestVanDammeAnim broheart = HeroController.GetHeroPrefab(hero);
                    if (!Traverse.Create(broheart).Field("disarmed").GetValue<bool>()) vector = new Vector3(-5f, 4, -1f);
                    else vector = new Vector3(-3, 0, -1);
                    break;
                case HeroType.BroneyRoss:
                    vector = new Vector3(-2, 0, 0); break;
                case HeroType.LeeBroxmas:
                    vector = new Vector3(-5, 0, -0.001f); break;
                case HeroType.TheBrode:
                    vector = new Vector3(-4, 0, -1); break;
                case HeroType.Brochete:
                    vector = new Vector3(-6, 0, 0.001f); break;
            }
            return vector;
        }

        public static Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }
}
