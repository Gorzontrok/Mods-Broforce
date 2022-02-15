using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using RocketLib0;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace Pigstini
{
    static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;

        internal static BroforceMod bmod;

        internal static Grenade pigstini;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnToggle = OnToggle;

            mod = modEntry;

            var harmony = new Harmony(modEntry.Info.Id);
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);
            }
            catch (Exception ex)
            {
                mod.Logger.Log("Failed to Patch Harmony !\n" + ex.ToString());
            }

            bmod = new BroforceMod(mod);
            bmod.Load(mod);

            return true;
        }
        internal static void MakePigstini()
        {
            try
            {
                pigstini = (HeroController.GetHeroPrefab(HeroType.DoubleBroSeven) as DoubleBroSeven).martiniGlass;
                pigstini.gameObject.GetComponent<MeshRenderer>().material.mainTexture = RocketLib.CreateTexFromMat(mod.Path + "pigstini.png", pigstini.gameObject.GetComponent<MeshRenderer>().material);
            }
            catch (Exception ex) { bmod.logger.ExceptionLog("Failed to create Pistini :(", ex); }
        }
        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }
    }
    [HarmonyPatch(typeof(Player), "Start")]
    static class MakePigstini_Patch
    {
        static void Postfix()
        {
            Main.MakePigstini();
        }
    }

    // Rambro Patch
    [HarmonyPatch(typeof(Rambro), "FireWeapon")]
    static class Rambro_FireWeapon_Patch
    {
        static bool Prefix(Rambro __instance, float x, float y, float xSpeed, float ySpeed)
        {
            try
            {
                EffectsController.CreateShrapnel(__instance.bulletShell, x + __instance.transform.localScale.x * -15f, y + 3f, 1f, 30f, 1f, -__instance.transform.localScale.x * 80f, 170f);
                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch(Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // Brommando Patch
    [HarmonyPatch(typeof(Brommando), "FireWeapon")]
    static class Brommando_FireWeapon_Patch
    {
        static bool Prefix(Brommando __instance, float x, float y, float xSpeed, float ySpeed)
        {
            try
            {
                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // BaBroracus Patch
    [HarmonyPatch(typeof(BaBroracus), "FireWeapon")]
    static class BaBroracus_FireWeapon_Patch
    {
        static bool Prefix(BaBroracus __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // BrodellWalker Patch
    [HarmonyPatch(typeof(BrodellWalker), "FireWeapon")]
    static class BrodellWalker_FireWeapon_Patch
    {
        static bool Prefix(BrodellWalker __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // BroHard Patch
    [HarmonyPatch(typeof(BroHard), "FireWeapon")]
    static class BroHard_FireWeapon_Patch
    {
        static bool Prefix(BroHard __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // McBrover Patch
    [HarmonyPatch(typeof(McBrover), "FireWeapon")]
    static class McBrover_FireWeapon_Patch
    {
        static bool Prefix(McBrover __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // Blade Patch
    [HarmonyPatch(typeof(Blade), "FireWeapon")]
    static class Blade_FireWeapon_Patch
    {
        static bool Prefix(Blade __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // BroDredd Patch
    [HarmonyPatch(typeof(BroDredd), "FireWeapon")]
    static class BroDredd_FireWeapon_Patch
    {
        static bool Prefix(BroDredd __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // Bro In Black Patch
    [HarmonyPatch(typeof(Brononymous), "FireWeapon")]
    static class Brononymous_FireWeapon_Patch
    {
        static bool Prefix(Brononymous __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // SnakeBroskin Patch
    [HarmonyPatch(typeof(SnakeBroskin), "FireWeapon")]
    static class SnakeBroskin_FireWeapon_Patch
    {
        static bool Prefix(SnakeBroskin __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // Brominator Patch
    [HarmonyPatch(typeof(Brominator), "FireWeapon")]
    static class Brominator_FireWeapon_Patch
    {
        static bool Prefix(Brominator __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // Brobocop Patch
    [HarmonyPatch(typeof(Brobocop), "FireWeapon")]
    static class Brobocop_FireWeapon_Patch
    {
        static bool Prefix(Brobocop __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // IndianaBrones Patch
    [HarmonyPatch(typeof(IndianaBrones), "FireWeapon")]
    static class IndianaBrones_FireWeapon_Patch
    {
        static bool Prefix(IndianaBrones __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // AshBrolliams Patch
    [HarmonyPatch(typeof(AshBrolliams), "FireWeapon")]
    static class AshBrolliams_FireWeapon_Patch
    {
        static bool Prefix(AshBrolliams __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // Mr Anderbro Patch
    [HarmonyPatch(typeof(Nebro), "FireWeapon")]
    static class Nebro_FireWeapon_Patch
    {
        static bool Prefix(Nebro __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // BoondockBro Patch
    [HarmonyPatch(typeof(BoondockBro), "FireWeapon")]
    static class BoondockBro_FireWeapon_Patch
    {
        static bool Prefix(BoondockBro __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // Brochete Patch
    [HarmonyPatch(typeof(Brochete), "FireWeapon")]
    static class Brochete_FireWeapon_Patch
    {
        static bool Prefix(Brochete __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // BronanTheBrobarian Patch
    [HarmonyPatch(typeof(BronanTheBrobarian), "FireWeapon")]
    static class BronanTheBrobarian_FireWeapon_Patch
    {
        static bool Prefix(BronanTheBrobarian __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // EllenRipbro Patch
    [HarmonyPatch(typeof(EllenRipbro), "FireWeapon")]
    static class EllenRipbro_FireWeapon_Patch
    {
        static bool Prefix(EllenRipbro __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // TimeBroVanDamme Patch
    [HarmonyPatch(typeof(TimeBroVanDamme), "FireWeapon")]
    static class TimeBroVanDamme_FireWeapon_Patch
    {
        static bool Prefix(TimeBroVanDamme __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // BroniversalSoldier Patch
    [HarmonyPatch(typeof(BroniversalSoldier), "FireWeapon")]
    static class BroniversalSoldier_FireWeapon_Patch
    {
        static bool Prefix(BroniversalSoldier __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // ColJamesBrodock Patch
    [HarmonyPatch(typeof(ColJamesBrodock), "FireWeapon")]
    static class ColJamesBrodock_FireWeapon_Patch
    {
        static bool Prefix(ColJamesBrodock __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // CherryBroling Patch
    [HarmonyPatch(typeof(CherryBroling), "FireWeapon")]
    static class CherryBroling_FireWeapon_Patch
    {
        static bool Prefix(CherryBroling __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // BroMax Patch
    [HarmonyPatch(typeof(TollBroad), "FireWeapon")]
    static class BroMax_FireWeapon_Patch
    {
        static bool Prefix(BroMax __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }
    // DoubleBroSeven Patch
    [HarmonyPatch(typeof(DoubleBroSeven), "FireWeapon")]
    static class DoubleBroSeven_FireWeapon_Patch
    {
        static bool Prefix(DoubleBroSeven __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // Predabro Patch
    [HarmonyPatch(typeof(Predabro), "FireWeapon")]
    static class Predabro_FireWeapon_Patch
    {
        static bool Prefix(Predabro __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // TheBrofessional Patch
    [HarmonyPatch(typeof(TheBrofessional), "FireWeapon")]
    static class TheBrofessional_FireWeapon_Patch
    {
        static bool Prefix(TheBrofessional __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // BrondleFly Patch
    [HarmonyPatch(typeof(BrondleFly), "FireWeapon")]
    static class BrondleFly_FireWeapon_Patch
    {
        static bool Prefix(BrondleFly __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // BroveHeart Patch
    [HarmonyPatch(typeof(BroveHeart), "FireWeapon")]
    static class BroveHeart_FireWeapon_Patch
    {
        static bool Prefix(BroveHeart __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // TheBrolander Patch
    [HarmonyPatch(typeof(TheBrolander), "FireWeapon")]
    static class TheBrolander_FireWeapon_Patch
    {
        static bool Prefix(TheBrolander __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // DirtyHarry Patch
    [HarmonyPatch(typeof(DirtyHarry), "FireWeapon")]
    static class DirtyHarry_FireWeapon_Patch
    {
        static bool Prefix(DirtyHarry __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // TollBroad Patch
    [HarmonyPatch(typeof(TollBroad), "FireWeapon")]
    static class TollBroad_FireWeapon_Patch
    {
        static bool Prefix(TollBroad __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // Broc Patch
    [HarmonyPatch(typeof(BrocSnipes), "FireWeapon", new Type[] { typeof(float), typeof(float), typeof(float), typeof(float) })]
    static class BrocSnipes_FireWeapon_Patch
    {
        static bool Prefix(BrocSnipes __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }
    [HarmonyPatch(typeof(BrocSnipes), "FireWeapon", new Type[] { typeof(float), typeof(float), typeof(Vector3), typeof(float), typeof(float) })]
    static class BrocSnipes_FireWeapon2_Patch
    {
        static bool Prefix(BrocSnipes __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // BroneyRoss Patch
    [HarmonyPatch(typeof(BroneyRoss), "FireWeapon")]
    static class BroneyRoss_FireWeapon_Patch
    {
        static bool Prefix(BroneyRoss __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // LeeBroxmas Patch
    [HarmonyPatch(typeof(LeeBroxmas), "FireWeapon")]
    static class LeeBroxmas_FireWeapon_Patch
    {
        static bool Prefix(LeeBroxmas __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // BronnarJensen Patch
    [HarmonyPatch(typeof(BronnarJensen), "FireWeapon")]
    static class BronnarJensen_FireWeapon_Patch
    {
        static bool Prefix(BronnarJensen __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }


    // BroCeasar Patch
    [HarmonyPatch(typeof(BroCeasar), "FireWeapon")]
    static class BroCeasar_FireWeapon_Patch
    {
        static bool Prefix(BroCeasar __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }

    // TrentBroser Patch
    [HarmonyPatch(typeof(TestVanDammeAnim), "FireWeapon")]
    static class TrentBroser_FireWeapon_Patch
    {
        static bool Prefix(TestVanDammeAnim __instance)
        {
            try
            {

                ProjectileController.SpawnGrenadeOverNetwork(Main.pigstini, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 110f, __instance.playerNum);
                return false;

            }
            catch (Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
            return true;
        }
    }
}
