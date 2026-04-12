using System;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;
using System.Reflection;

namespace AvatarFaceHuggerMod
{
    public static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            mod = modEntry;
            modEntry.OnToggle = OnToggle;

            var harmony = new Harmony(modEntry.Info.Id);
            var assembly = Assembly.GetExecutingAssembly();
            harmony.PatchAll(assembly);
            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }
    }

    [HarmonyPatch(typeof(PlayerHUD), "ShowFaceHugger")]
    static class Avatar_FaceHugger_Patch
    {
        static void Prefix(PlayerHUD __instance)
        {
            if (!Main.enabled) return;

            __instance.showFaceHugger = true;
            __instance.faceHugger1.SetSize(Traverse.Create(__instance).Field("avatarFacingDirection").GetValue<int>() * __instance.faceHugger1.width, __instance.faceHugger1.height);
            __instance.avatar.SetLowerLeftPixel(new Vector2(__instance.faceHugger1.lowerLeftPixel.x, 1f));
            __instance.faceHugger1.gameObject.SetActive(true);
        }
    }
    [HarmonyPatch(typeof(PlayerHUD), "Start")]
    static class Start_Patch
    {
        static void Postfix(PlayerHUD __instance)
        {
            if (!Main.enabled) return;

            var shader = __instance.avatar.MeshRenderer.material.shader;
            if (shader != null)
            {
                __instance.faceHugger1.meshRender.material.shader = shader;
                __instance.faceHugger1.meshRender.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 0.5f));

                __instance.faceHugger2.meshRender.material.shader = shader;
                __instance.faceHugger2.meshRender.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 0.5f));
            }
        }
    }
}
