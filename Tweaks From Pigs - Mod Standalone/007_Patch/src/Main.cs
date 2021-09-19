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

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

    }

    // Patch 007
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
    [HarmonyPatch(typeof(DoubleBroSeven), "UseSpecial")]
    static class DoubleBroSeven_TearGasAtFeet_Patch
    {
        static bool Prefix(DoubleBroSeven __instance)
        {
            if (!Main.enabled) return true;

            DoubleBroSevenSpecialType currentSpecialType = Traverse.Create(__instance).Field("currentSpecialType").GetValue<DoubleBroSevenSpecialType>();
            if (currentSpecialType == DoubleBroSevenSpecialType.TearGas)
            {
                Networking.Networking.RPC<float>(PID.TargetAll, new RpcSignature<float>(__instance.PlayThrowLightSound), 0.5f, false);
                if (__instance.IsMine)
                {
                    if (Traverse.Create(__instance).Field("ducking").GetValue<bool>() && __instance.down)
                    {
                        ProjectileController.SpawnGrenadeOverNetwork(__instance.tearGasGrenade, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 3f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 30f, 70f, __instance.playerNum);
                    }
                    else
                    {
                        ProjectileController.SpawnGrenadeOverNetwork(__instance.tearGasGrenade, __instance, __instance.X + Mathf.Sign(__instance.transform.localScale.x) * 6f, __instance.Y + 10f, 0.001f, 0.011f, Mathf.Sign(__instance.transform.localScale.x) * 200f, 150f, __instance.playerNum);
                    }
                }
                __instance.SpecialAmmo--;
                return false;
            }
            return true;
        }
    }

    // Patch the icon in the HUD
    [HarmonyPatch(typeof(PlayerHUD), "SetGrenadeMaterials", new Type[] { typeof(HeroType) })]
    static class AddTearGasIcon_Patch
    {
        static void Prefix(PlayerHUD __instance, HeroType type)
        {
            if (type == HeroType.DoubleBroSeven && __instance.doubleBroGrenades.Length < 5)
            {
                Material newIconForTearGas = Material.Instantiate(__instance.rambroIcon);
                newIconForTearGas.mainTexture = Main.CreateTexFromMat("Grenade_Tear_Gas.png", newIconForTearGas);
                newIconForTearGas.name = "007TearGas";
                List<Material> tempList = __instance.doubleBroGrenades.ToList();
                tempList.Add(newIconForTearGas);
                __instance.doubleBroGrenades = tempList.ToArray();
            }
        }
    }

    // Weird Martini from DragonightFury
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
    [HarmonyPatch(typeof(Mook), "TearGas")]
    static class ForgetPlayerInTearGas_Patch
    {
        static bool Prefix(Mook __instance, float time)
        {
            if (!Main.enabled) return true;
            if (__instance.canBeTearGased)
            {
                __instance.Stop();
                Traverse.Create(__instance).Field("stunTime").SetValue(time);
                if (__instance.mookType == MookType.Trooper) Traverse.Create(__instance).Field("tearGasChoking").SetValue(true);
                if (__instance.enemyAI != null)
                {
                    __instance.enemyAI.Blind(time + 0.5f);
                }
                Traverse.Create(__instance).Method("DeactivateGun").GetValue();
            }
            return false;
        }
    }
    [HarmonyPatch(typeof(Mook), "Awake")]
    static class Mook_Awake_Patch
    {
        static void Prefix(Mook __instance)
        {
            if (!Main.enabled) return;
            if (__instance.mookType != MookType.Devil || __instance.mookType != MookType.Vehicle) __instance.canBeTearGased = true;
        }
    }
}
