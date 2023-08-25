using System;
using System.Collections.Generic;
using RocketLib;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace BroforceOverhaul.Bros.Spawn
{
    [HarmonyPatch(typeof(HeroUnlockController), "heroUnlockIntervals", MethodType.Getter)]
    static class HeroUnlockController_AddExpendabros_Patch
    {
        static bool Prefix(HeroUnlockController __instance, ref Dictionary<int, HeroType> __result)
        {
            if(Main.enabled)
            {
                Traverse t = Traverse.Create(typeof(HeroUnlockController));
                Dictionary<int, HeroType> _heroUnlockIntervals = t.Field("_heroUnlockIntervals").GetValue<Dictionary<int, HeroType>>();
                if (_heroUnlockIntervals == null)
                {
                    _heroUnlockIntervals = new Dictionary<int, HeroType>()
                    {
                        { 0, HeroType.Rambro },
                        { 1, HeroType.Brommando },
                        { 3, HeroType.BaBroracus },
                        { 5, HeroType.BrodellWalker },
                        { 8, HeroType.BroHard },
                        { 11, HeroType.McBrover },
                        { 15, HeroType.Blade },
                        { 20, HeroType.BroDredd },
                        { 25, HeroType.Brononymous },
                        { 31, HeroType.DirtyHarry },
                        { 37, HeroType.Brominator },
                        { 46, HeroType.Brobocop },
                        { 56, HeroType.IndianaBrones },
                        { 65, HeroType.AshBrolliams },
                        { 75, HeroType.Nebro },
                        { 87, HeroType.BoondockBros },
                        { 99, HeroType.Brochete },
                        { 115, HeroType.BronanTheBrobarian },
                        { 132, HeroType.EllenRipbro },
                        { 145, HeroType.TheBrocketeer },
                        { 160, HeroType.TimeBroVanDamme },
                        { 175, HeroType.BroniversalSoldier },
                        { 193, HeroType.ColJamesBroddock },
                        { 222, HeroType.CherryBroling },
                        { 249, HeroType.BroMax },
                        { 274, HeroType.TheBrode },
                        { 300, HeroType.DoubleBroSeven },
                        { 326, HeroType.Predabro },
                        { 350, HeroType.BroveHeart },
                        { 374, HeroType.TheBrofessional },
                        { 400, HeroType.Broden },
                        { 420, HeroType.TheBrolander },
                        { 440, HeroType.SnakeBroSkin },
                        { 460, HeroType.TankBro },
                        { 480, HeroType.BroLee },
                        { 490, HeroType.BroneyRoss },
                        { 500, HeroType.LeeBroxmas },
                        { 510, HeroType.BronnarJensen },
                        { 520, HeroType.HaleTheBro },
                        { 530,HeroType.Broc },
                        { 540, HeroType.TollBroad },
                        { 550, HeroType.TrentBroser }
                    };
                }
                __result = _heroUnlockIntervals;
                return false;
            }
            return true;
        }
    }
}

