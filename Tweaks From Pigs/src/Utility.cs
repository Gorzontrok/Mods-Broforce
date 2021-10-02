using System;
using System.Collections.Generic;
using System.Reflection;
using RocketLib0;
using UnityEngine;
using HarmonyLib;

namespace TweaksFromPigs
{
    public class Utility
    {
        public static System.Random rand = new System.Random();

        public static List<HeroType> HeroList = RocketLib._HeroUnlockController.HeroTypeFullList;
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


        public static Texture2D CreateTexFromMat(string ImagePath, Material origMat)
        {
            return RocketLib.CreateTexFromMat(Main.ResFolder + ImagePath, origMat);
        }
        public static Texture2D CreateTexFromSpriteSM(string ImagePath, SpriteSM sprite)
        {
            return RocketLib.CreateTexFromSpriteSM(Main.ResFolder + ImagePath, sprite);
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
    }
}
