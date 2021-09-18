using System;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace _007_Patch
{
    static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            var harmony = new Harmony(modEntry.Info.Id);
            settings = Settings.Load<Settings>(modEntry);
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);
            }
            catch (Exception ex)
            {
                mod.Logger.Log(ex.ToString());
            }

            mod = modEntry;

            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();
            settings.UseWeirdMartini = GUILayout.Toggle(settings.UseWeirdMartini, "Use weird martini behaviour");
            settings.PatchTearGas = GUILayout.Toggle(settings.PatchTearGas, "Everyone can be teargased (BUG)");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }

        public static void Log(object str)
        {
            mod.Logger.Log(str.ToString());
        }

        public static Texture2D CreateTexFromMat(string filename, Material origMat)
        {
            if (!File.Exists(Main.mod.Path +  filename)) throw new IOException();

            Texture2D tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            tex.LoadImage(File.ReadAllBytes(Main.mod.Path + filename));
            tex.wrapMode = TextureWrapMode.Clamp;

            Texture orig = origMat.mainTexture;

            tex.anisoLevel = orig.anisoLevel;
            tex.filterMode = orig.filterMode;
            tex.mipMapBias = orig.mipMapBias;
            tex.wrapMode = orig.wrapMode;

            return tex;
        }
    }

    public class Settings : UnityModManager.ModSettings
    {
        public bool UseWeirdMartini;
        public bool PatchTearGas;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

    }

    [HarmonyPatch(typeof(DoubleBroSeven), "Awake")]
    static class AddMoreSpecial_Patch
    {
        static void Postfix(DoubleBroSeven __instance)
        {
            if (!Main.enabled) return;
            Traverse.Create(typeof(DoubleBroSeven)).Field("_specialAmmo").SetValue(5);
            __instance.SpecialAmmo = 5;
            __instance.originalSpecialAmmo = 5;
        }
    }

    // Patch the icon in the HUD
    [HarmonyPatch(typeof(PlayerHUD), "SetGrenadeMaterials", new Type[] { typeof(HeroType) })]
    static class AddTearGasIcon_Patch
    {
        static void Prefix(PlayerHUD __instance, HeroType type)
        {
            Material newIconForTearGas = __instance.rambroIcon;
            if (type == HeroType.DoubleBroSeven)
            {
                newIconForTearGas.mainTexture = Main.CreateTexFromMat("Grenade_Tear_Gas.png", newIconForTearGas);
                List<Material> tempList = __instance.doubleBroGrenades.ToList();
                tempList.Add(newIconForTearGas);
                __instance.doubleBroGrenades = tempList.ToArray();
            }
        }
    }

    // Weird Martini
    [HarmonyPatch(typeof(MartiniGlass), "Death")]
    static class WeirdMartini_Patch
    {
        static bool Prefix(MartiniGlass __instance)
        {
            if(Main.settings.UseWeirdMartini)
            {
                Traverse.Create(__instance).Method("MakeEffects").GetValue();
                return false;
            }
            return true;
        }
    }

    // Mook patch
    [HarmonyPatch(typeof(MookSuicide), "Start")]
    static class MookSuicide_Patch
    {
        static void Postfix(MookSuicide __instance)
        {
            if (Main.settings.PatchTearGas) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }
    [HarmonyPatch(typeof(MookBigGuy), "Awake")]
    static class MookBigGuy_Patch
    {
        static void Postfix(MookBigGuy __instance)
        {
            if (Main.settings.PatchTearGas) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }
    [HarmonyPatch(typeof(MookDog), "Update")]
    static class MookDog_Patch
    {
        static void Postfix(MookDog __instance)
        {
            if (Main.settings.PatchTearGas) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }
    [HarmonyPatch(typeof(MookGrenadier), "Start")]
    static class MookGrenadier_Patch
    {
        static void Postfix(MookGrenadier __instance)
        {
            if (Main.settings.PatchTearGas) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }
    [HarmonyPatch(typeof(MookJetpack), "Start")]
    static class MookJetpack_Patch
    {
        static void Postfix(MookJetpack __instance)
        {
            if (Main.settings.PatchTearGas) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }
    [HarmonyPatch(typeof(MookNinja), "Start")]
    static class MookNinja_Patch
    {
        static void Postfix(MookNinja __instance)
        {
            if (Main.settings.PatchTearGas) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }
    [HarmonyPatch(typeof(MookRiotShield), "Update")]
    static class MookRiotShield_Patch
    {
        static void Postfix(MookRiotShield __instance)
        {
            if (Main.settings.PatchTearGas) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }
    [HarmonyPatch(typeof(MookTrooper), "Start")]
    static class MookTrooper_Patch
    {
        static void Postfix(MookTrooper __instance)
        {
            if (Main.settings.PatchTearGas) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }
    [HarmonyPatch(typeof(ScoutMook), "Awake")]
    static class ScoutMook_Patch
    {
        static void Postfix(ScoutMook __instance)
        {
            if (Main.settings.PatchTearGas) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }
    [HarmonyPatch(typeof(MookGeneral), "IsEvil")]
    static class MookGeneral_Patch
    {
        static void Postfix(MookGeneral __instance)
        {
            if (Main.settings.PatchTearGas) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }

    // Patch alien
    [HarmonyPatch(typeof(Alien), "Start")]
    static class Alien_Patch
    {
        static void Postfix(Alien __instance)
        {
            if (Main.settings.PatchTearGas) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }
    [HarmonyPatch(typeof(AlienFaceHugger), "Start")]
    static class AlienFaceHugger_Patch
    {
        static void Postfix(AlienFaceHugger __instance)
        {
            if (Main.settings.PatchTearGas) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }
    [HarmonyPatch(typeof(AlienMelter), "Start")]
    static class AlienMelter_Patch
    {
        static void Postfix(AlienMelter __instance)
        {
            if (Main.settings.PatchTearGas) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }
    [HarmonyPatch(typeof(AlienMosquito), "Start")]
    static class AlienMosquito_Patch
    {
        static void Postfix(AlienMosquito __instance)
        {
            if (Main.settings.PatchTearGas) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }
    [HarmonyPatch(typeof(AlienXenomorph), "Start")]
    static class AlienXenomorph_Patch
    {
        static void Postfix(AlienXenomorph __instance)
        {
            if (Main.settings.PatchTearGas) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }

    //Patch Hell
    [HarmonyPatch(typeof(MookSuicideUndead), "Start")]
    static class MookSuicideUndead_Patch
    {
        static void Postfix(MookSuicideUndead __instance)
        {
            if (Main.settings.PatchTearGas) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }
    [HarmonyPatch(typeof(HellDog), "Awake")]
    static class HellDog_Patch
    {
        static void Postfix(HellDog __instance)
        {
            if (Main.settings.PatchTearGas) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }
    [HarmonyPatch(typeof(UndeadTrooper), "Start")]
    static class UndeadTrooper_Patch
    {
        static void Postfix(UndeadTrooper __instance)
        {
            if (Main.settings.PatchTearGas) __instance.canBeTearGased = true;
            else __instance.canBeTearGased = false;
        }
    }

}
