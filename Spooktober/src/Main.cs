using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;
using RocketLib0;

namespace SpooktoberMod
{
    static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        internal static BroforceMod bmod;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
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

            bmod = new BroforceMod(mod, _UseLocalLog: true); ;

            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
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

   /* [HarmonyPatch(typeof(PlayerHUD), "Setup")]
    static class SetSpooktoberAvatar_Patch
    {
        static void Postfix(PlayerHUD __instance)
        {
            if (!Main.enabled)
                return;
            try
            {
                Traverse inst = Traverse.Create(__instance);
                SpriteSM sprite = __instance.avatar.gameObject.GetComponent<SpriteSM>();
                sprite.meshRender.sharedMaterial.SetTexture("_MainTex", RocketLib.CreateTexFromSpriteSM(Main.mod.Path + "Spooktober_avatar.png", sprite));

                /* __instance.avatar = sprite;
                 __instance.secondAvatar = sprite;
                Traverse.Create(__instance).Field("avatar").SetValue(sprite); //Change the avatar to the skeleton
                Traverse.Create(__instance).Field("secondAvatar").SetValue(sprite); //Change the second avatar to the skeleton
            }
            catch(Exception ex) { Main.Log(ex); }
        }
    }*/

    [HarmonyPatch(typeof(PlayerHUD), "SwitchAvatarAndGrenadeMaterial")]
    static class SwitchSpooktoberAvatar_Patch
    {
        static void Postfix(PlayerHUD __instance)
        {
            if (!Main.enabled)
                return;
            Traverse inst = Traverse.Create(__instance);
            try
            {
                SpriteSM sprite = __instance.avatar.gameObject.GetComponent<SpriteSM>();
                sprite.meshRender.sharedMaterial.SetTexture("_MainTex", RocketLib.CreateTexFromSpriteSM(Path.Combine(Main.mod.Path, "Spooktober_avatar.png"), sprite));

                 __instance.avatar = sprite;
                 __instance.secondAvatar = sprite;
                /* Traverse.Create(__instance).Field("avatar").SetValue(sprite); //Change the avatar to the skeleton
                 Traverse.Create(__instance).Field("secondAvatar").SetValue(sprite); //Change the second avatar to the skeleton*/
                __instance.Show();
            }
            catch (Exception ex) { Main.Log(ex); }
        }
    }
    // MAP
    [HarmonyPatch(typeof(MapData), "Deserialize")]
    class ChangeMapData_Patch
    {
        static void Postfix(MapData __instance)
        {
            __instance.ambience = AmbienceType.HellFire;
            __instance.theme = LevelTheme.Hell;
            __instance.weatherType = WeatherType.Evil;
            __instance.musicType = MusicType.IntensityTest;
            __instance.heroSpawnMode = HeroSpawnMode.Portal;
            __instance.mineFieldSpawnProbability = 1f;

            int i = 0;
            try
            {
                List<DoodadInfo> DoodadInfoList = new List<DoodadInfo>(__instance.DoodadList);
                foreach (DoodadInfo dinfo in __instance.DoodadList)
                {
                    if (dinfo.type == DoodadType.PureEvil)
                    {
                        DoodadInfoList[i].variation = 0;
                    }
                    else if (dinfo.type == DoodadType.Mook)
                    {
                        if(dinfo.variation == 0)
                        {
                            DoodadInfoList[i].type = DoodadType.HellEnemy;
                            DoodadInfoList[i].variation = 1;
                        }
                        else if(dinfo.variation == 1)
                        {
                            DoodadInfoList[i].type = DoodadType.HellEnemy;
                            DoodadInfoList[i].variation = 5;
                        }
                        else if (dinfo.variation == 3)
                        {
                            DoodadInfoList[i].type = DoodadType.HellEnemy;
                            DoodadInfoList[i].variation = 6;
                        }
                        else if (dinfo.variation == 5)
                        {
                            DoodadInfoList[i].type = DoodadType.HellEnemy;
                            DoodadInfoList[i].variation = 0;
                        }
                        else if (dinfo.variation == 6)
                        {
                            DoodadInfoList[i].type = DoodadType.HellEnemy;
                            DoodadInfoList[i].variation = 11;
                        }
                        else if (dinfo.variation == 7)
                        {
                            DoodadInfoList[i].type = DoodadType.HellEnemy;
                            DoodadInfoList[i].variation = 3;
                        }
                        else if (dinfo.variation == 8)
                        {
                            DoodadInfoList[i].type = DoodadType.HellEnemy;
                            DoodadInfoList[i].variation = 4;
                        }
                        else if (dinfo.variation == 10)
                        {
                            DoodadInfoList[i].type = DoodadType.HellEnemy;
                            DoodadInfoList[i].variation = 8;
                        }
                    }
                    i++;
                }

                __instance.DoodadList = new List<DoodadInfo>(DoodadInfoList);
            }catch(Exception ex) { Main.bmod.ExceptionLog(ex); }
        }
    }
    [HarmonyPatch(typeof(TestVanDammeAnim), "GetFootPoofColor")]
    class e
    {
        static bool Prefix(TestVanDammeAnim __instance, ref BloodColor __result)
        {
            __result = BloodColor.Red;
            return false;
        }
    }

}
