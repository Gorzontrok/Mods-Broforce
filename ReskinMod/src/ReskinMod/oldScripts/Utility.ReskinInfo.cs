using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReskinMod
{
   /* internal static partial class Utility
    {
        internal static RI_Mook GetMookReskinInfo(Mook mook)
        {
            if (mook as MookTrooper)
            {
                if (mook as MookBazooka)
                {
                    return Mook_Reskin.Bazooka;
                }
                else if (mook as MookJetpack)
                {
                    return Mook_Reskin.Jetpack;
                }
                else if (mook as UndeadTrooper)
                {
                    return Mook_Reskin.UndeadTrooper;
                }
                return Mook_Reskin.MookTrooper;
            }
            else if (mook as MookSuicide)
            {
                return Mook_Reskin.Suicide;
            }
            else if (mook as MookBigGuy)
            {
                if (mook as MookArmouredGuy)
                {
                    return Mook_Reskin.ArmouredGuy;
                }
                else if (mook as MookHellBigGuy)
                {
                    return Mook_Reskin.Hell_Bruisers;
                }
                else if (mook as MookHellArmouredBigGuy)
                {
                    return Mook_Reskin.Hell_Armoured_BigGuy;
                }
                return Mook_Reskin.Bruisers;
            }
            else if (mook as SkinnedMook)
            {
                return Mook_Reskin.SkinlessMook;
            }
            else if (mook as MookHellBoomer)
            {
                if (mook as MookHellSoulCatcher)
                {
                    return Mook_Reskin.SoulCatcher;
                }
                return Mook_Reskin.Boomer;
            }
            else if (mook as MookSuicideUndead)
            {
                return Mook_Reskin.UndeadSuicide;
            }
            else if (mook as MookRiotShield)
            {
                return Mook_Reskin.RiotShield;
            }
            else if (mook as ScoutMook)
            {
                return Mook_Reskin.Scout;
            }
            else if (mook as MookDog)
            {
                if (mook as HellDog)
                {
                    return Mook_Reskin.HellDog;
                }
                else if (mook as Alien)
                {
                    if (mook as AlienBrute)
                    {
                        return Mook_Reskin.Alien_Brute;
                    }
                    else if (mook as AlienClimber)
                    {
                        if (mook as AlienFaceHugger)
                        {
                            return Mook_Reskin.FaceHugger;
                        }
                        else if (mook as AlienMelter)
                        {
                            return Mook_Reskin.Melter;
                        }
                        else if (mook as AlienXenomorph)
                        {
                            return Mook_Reskin.Xenomorphe;
                        }
                        return Mook_Reskin.Xenomorphe;
                    }
                }
                return Mook_Reskin.Dog;
            }
            else if (mook as AlienMosquito)
            {
                if (mook as HellLostSoul)
                {
                    return Mook_Reskin.LostSoul;
                }
                return Mook_Reskin.Mosquito;
            }
            else if (mook as MookGeneral)
            {
                return Mook_Reskin.General;
            }
            else if (mook as MookGrenadier)
            {
                return Mook_Reskin.Grenadier;
            }
            else if (mook as Satan)
            {
                return Mook_Reskin.Satan;
            }
            else if (mook as Warlock)
            {
                return Mook_Reskin.Warlock;
            }
            return Mook_Reskin.MookTrooper;
        }

        internal static RI_Bro GetBroReskinInfo(HeroType broName)
        {
            switch (broName)
            {
                case HeroType.AshBrolliams: return Bro_Reskin.AshBrolliams;
                case HeroType.BaBroracus: return Bro_Reskin.BaBroracus;
                case HeroType.Blade: return Bro_Reskin.Brade;
                case HeroType.BoondockBros: return Bro_Reskin.BoondockBros;
                case HeroType.Brobocop: return Bro_Reskin.Brobocop;
                case HeroType.Broc: return Bro_Reskin.Broc;
                case HeroType.Brochete: return Bro_Reskin.Brochete;
                case HeroType.BrodellWalker: return Bro_Reskin.BrodellWalker;
                case HeroType.Broden: return Bro_Reskin.Broden;
                case HeroType.BroDredd: return Bro_Reskin.BroDredd;
                case HeroType.BroHard: return Bro_Reskin.BroHard;
                case HeroType.BroLee: return Bro_Reskin.BroLee;
                case HeroType.BroMax: return Bro_Reskin.BroMax;
                case HeroType.Brominator: return Bro_Reskin.Brominator;
                case HeroType.Brommando: return Bro_Reskin.Brommando;
                case HeroType.BronanTheBrobarian: return Bro_Reskin.BronanTheBrobarian;
                case HeroType.BrondleFly: return Bro_Reskin.BrondleFly;
                case HeroType.BroneyRoss: return Bro_Reskin.BroneyRoss;
                case HeroType.BroniversalSoldier: return Bro_Reskin.BroniversalSoldier;
                case HeroType.BronnarJensen: return Bro_Reskin.BronnarJensen;
                case HeroType.Brononymous: return Bro_Reskin.BroInBlack;
                case HeroType.BroveHeart: return Bro_Reskin.BroveHeart;
                case HeroType.CherryBroling: return Bro_Reskin.CherryBroling;
                case HeroType.ColJamesBroddock: return Bro_Reskin.ColJamesBroddock;
                case HeroType.DirtyHarry: return Bro_Reskin.DirtyBrorry;
                case HeroType.DoubleBroSeven: return Bro_Reskin.DoubleBroSeven;
                case HeroType.EllenRipbro: return Bro_Reskin.EllenRipbro;
                case HeroType.HaleTheBro: return Bro_Reskin.BroCaesar;
                case HeroType.IndianaBrones: return Bro_Reskin.IndianaBrones;
                case HeroType.LeeBroxmas: return Bro_Reskin.LeeBroxmas;
                case HeroType.McBrover: return Bro_Reskin.McBrover;
                case HeroType.Nebro: return Bro_Reskin.NeoBro;
                case HeroType.Predabro: return Bro_Reskin.Predabro;
                case HeroType.Rambro: return Bro_Reskin.Rambro;
                case HeroType.SnakeBroSkin: return Bro_Reskin.SnakeBroSkin;
                case HeroType.TimeBroVanDamme: return Bro_Reskin.TimeBro;
                case HeroType.TollBroad: return Bro_Reskin.TollBroad;
                case HeroType.TankBro: return Bro_Reskin.TankBro;
                case HeroType.TheBrocketeer: return Bro_Reskin.TheBrocketeer;
                case HeroType.TheBrode: return Bro_Reskin.TheBrode;
                case HeroType.TheBrofessional: return Bro_Reskin.TheBrofessional;
                case HeroType.TheBrolander: return Bro_Reskin.TheBrolander;
                case HeroType.TrentBroser: return Bro_Reskin.TrentBroser;
            }
            return Bro_Reskin.Rambro;
        }

    }*/
}
