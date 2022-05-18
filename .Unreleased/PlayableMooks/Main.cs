using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace PlayableMooks
{
    static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            modEntry.OnUpdate = OnUpdate;

            settings = Settings.Load<Settings>(modEntry);

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

            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
        }

        static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }

        public static void Log(object str)
        {
            mod.Logger.Log(str.ToString());
        }
        public static void Log(IEnumerable<object> str)
        {
            mod.Logger.Log(str.ToString());
        }

    }

    public class Settings : UnityModManager.ModSettings
    {

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

    }

    [HarmonyPatch(typeof(Player), "InstantiateHero")]
    static class Patch
    {
        /*static bool Prefix(Player __instance, ref TestVanDammeAnim __result, HeroType heroTypeEnum, int PlayerNum, int ControllerNum)
        {
            if (!Main.enabled || !GameModeController.ShowStandardHUDS()) return true;
            try
            {
                if (!__instance.IsMine)
                {
                    __result = null;
                }
                TestVanDammeAnim heroPrefab = Map.Instance.activeTheme.mook;
                TestVanDammeAnim testVanDammeAnim = Networking.Networking.InstantiateBuffered<TestVanDammeAnim>(heroPrefab, Vector3.zero, Quaternion.identity, new object[0], false);
                Networking.Networking.RPC<int, HeroType, bool>(PID.TargetAll, new RpcSignature<int, HeroType, bool>(testVanDammeAnim.SetUpHero), PlayerNum, heroTypeEnum, true, false);
                __result = testVanDammeAnim;

                return false;
            }catch(Exception ex) { Main.mod.Logger.Log(ex.ToString()); }
            return true;
        }*/
    }
    [HarmonyPatch(typeof(Player), "SpawnHero")]
    static class Patch2
    {
        static void Postfix(Player __instance)
        {
            if (!Main.enabled) return;

            try
            {
                TestVanDammeAnim mook = Map.Instance.activeTheme.mook;
                __instance.character = Networking.Networking.InstantiateBuffered<TestVanDammeAnim>(mook, Vector3.zero, Quaternion.identity, new object[0], false);
                Traverse.Create(__instance.character).Field("isHero").SetValue(true);
                __instance.character.playerNum = __instance.playerNum;
            }
            catch (Exception ex) { Main.mod.Logger.Log(ex.ToString()); }
        }
    }
}
