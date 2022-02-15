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

        internal static System.Random rand = new System.Random();

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

            bmod = new BroforceMod(mod);
            bmod.Load(mod);

            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            settings.HellMode = GUILayout.Toggle(settings.HellMode, "Hell Mode");
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

        internal static void Log(object str)
        {
            mod.Logger.Log(str.ToString());
        }
    }

    public class Settings : UnityModManager.ModSettings
    {
        public bool HellMode;
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }

    [HarmonyPatch(typeof(PlayerHUD), "SwitchAvatarAndGrenadeMaterial")]
    static class SwitchSpooktoberAvatar_Patch
    {
        static void Postfix(PlayerHUD __instance)
        {
            if (!Main.enabled)
                return;
            try
            {
                SpriteSM sprite = __instance.avatar.gameObject.GetComponent<SpriteSM>();
                string FileToLoad = string.Empty;
                if (__instance.heroType == HeroType.BroLee) FileToLoad = "Spooktober_avatar_long2.png";
                else if (__instance.heroType == HeroType.BroveHeart) FileToLoad = "Spooktober_avatar_long.png";
                else if (__instance.heroType == HeroType.TheBrofessional) FileToLoad = "Spooktober_avatar_longer.png";
                else FileToLoad = "Spooktober_avatar.png";
                sprite.meshRender.sharedMaterial.SetTexture("_MainTex", RocketLib.CreateTexFromSpriteSM(Path.Combine(Main.mod.Path, FileToLoad), sprite));

                 __instance.avatar = sprite;
                 __instance.secondAvatar = sprite;
                __instance.Show();
            }
            catch (Exception ex) { Main.bmod.Log(ex); }
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

                       /* if(Main.settings.HellMode && Main.rand.NextDouble() < 0.093)
                        {
                            DoodadInfoList[i].type = DoodadType.HellBoss;
                        }*/
                    }
                    else if (dinfo.type == DoodadType.Mook)
                    {
                        if(!Main.settings.HellMode)
                        {
                            if (dinfo.variation == 0)
                            {
                                DoodadInfoList[i].type = DoodadType.HellEnemy;
                                DoodadInfoList[i].variation = 1;
                            }
                            else if (dinfo.variation == 1)
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
                        else
                        {
                            DoodadInfoList[i].type = DoodadType.HellEnemy;
                            switch(dinfo.variation)
                            {
                                case 0:
                                    DoodadInfoList[i].variation = 3;
                                    break;
                                case 1:
                                        DoodadInfoList[i].variation = 5;
                                    break;
                                case 3:
                                    DoodadInfoList[i].variation = 6;
                                    break;
                                case 5:
                                    DoodadInfoList[i].variation = 0;
                                    break;
                                case 6:
                                    DoodadInfoList[i].variation = 11;
                                    break;
                                case 7:
                                    DoodadInfoList[i].variation = 3;
                                    break;
                                case 8:
                                    DoodadInfoList[i].variation = 4;
                                    break;
                                case 10:
                                    DoodadInfoList[i].variation = 8;
                                    break;
                            }
                        }
                    }
                    i++;
                }

                __instance.DoodadList = new List<DoodadInfo>(DoodadInfoList);
            }catch(Exception ex) { Main.bmod.logger.ExceptionLog(ex); }
        }
    }
    [HarmonyPatch(typeof(TestVanDammeAnim), "GetFootPoofColor")]
    class ChangeFootColor_Patch
    {
        static bool Prefix(TestVanDammeAnim __instance, ref BloodColor __result)
        {
            __result = BloodColor.Red;
            return false;
        }
    }
}
