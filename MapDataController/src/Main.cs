using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RocketLib0;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace MapDataController
{
    static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        internal static BroforceMod bmod;

        internal static AmbienceType[] AmbiencesType = { AmbienceType.Auto, AmbienceType.AlienCave, AmbienceType.BurningJungle, AmbienceType.Cave, AmbienceType.City, AmbienceType.HeavyRain, AmbienceType.Hell, AmbienceType.HellFire, AmbienceType.Jungle, AmbienceType.NoChange, AmbienceType.None, AmbienceType.Rain };
        internal static List<object> AmbiencesTypeList = new List<object>();

        internal static CameraFollowMode[] CameraFollowModes = { CameraFollowMode.Normal, CameraFollowMode.DescendInStages, CameraFollowMode.ForcedDescent, CameraFollowMode.ForcedHorizontal, CameraFollowMode.ForcedVertical, CameraFollowMode.Horizontal, CameraFollowMode.MapExtents, CameraFollowMode.MapWidth, CameraFollowMode.NoChange, CameraFollowMode.PanUpward, CameraFollowMode.SingleScreen, CameraFollowMode.Vertical, CameraFollowMode.Zoom };
        internal static List<object> CameraFollowModesList = new List<object>();

        internal static List<object> HeroTypeListObj = new List<object>();

        internal static HeroSpawnMode[] HeroSpawnModes = { HeroSpawnMode.Auto, HeroSpawnMode.Helicopter, HeroSpawnMode.Portal, HeroSpawnMode.SpawnPoint, HeroSpawnMode.Truck };
        internal static List<object> HeroSpawnModesList = new List<object>();

        internal static MusicType[] MusicTypes = { MusicType.Default, MusicType.Alien,  MusicType.Bossfight, MusicType.BroforceDrums, MusicType.Factory, MusicType.Hell, MusicType.IntensityTest, MusicType.JungleBlueSky, MusicType.JungleRedSky, MusicType.Silence };
        internal static List<object> MusicTypesList = new List<object>();

        internal static LevelTheme[] LevelThemes = { LevelTheme.Forest, LevelTheme.Jungle, LevelTheme.BurningJungle, LevelTheme.City, LevelTheme.Hell, LevelTheme.America };
        internal static List<object> LevelThemeList = new List<object>();

        internal static WeatherType[] WeatherTypes = { WeatherType.Day, WeatherType.NoChange, WeatherType.DampCave, WeatherType.Burning, WeatherType.Evil, WeatherType.Night, WeatherType.Overcast, WeatherType.None };
        internal static List<object> WeatherTypesList = new List<object>();

        private static GUIStyle LabelStyle = new GUIStyle();
        private static GUIStyle ActiveStateStyle;


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

            foreach (AmbienceType i in AmbiencesType) AmbiencesTypeList.Add(i);
            foreach (CameraFollowMode i in CameraFollowModes) CameraFollowModesList.Add(i);
            foreach (HeroSpawnMode i in HeroSpawnModes) HeroSpawnModesList.Add(i);
            foreach (MusicType i in MusicTypes) MusicTypesList.Add(i);
            foreach (LevelTheme i in LevelThemes) LevelThemeList.Add(i);
            foreach (WeatherType i in WeatherTypes) WeatherTypesList.Add(i);

            foreach (HeroType i in RocketLib._HeroUnlockController.HeroTypeFullList) HeroTypeListObj.Add(HeroController.GetHeroName(i));
            HeroTypeListObj.Insert(0, HeroType.Random);

            LabelStyle.fontSize = 18;
            LabelStyle.normal.textColor = Color.white;

            ActiveStateStyle = new GUIStyle("toggle");
            ActiveStateStyle.normal.background = MakeTex(2, 2, new Color(0.7f, 0f, 0f));
            ActiveStateStyle.hover.background = MakeTex(2, 2, new Color(1f, 0f, 0f));
            ActiveStateStyle.onNormal.background = MakeTex(2, 2, new Color(0f, 0.7f, 0f));
            ActiveStateStyle.onHover.background = MakeTex(2, 2, new Color(0f, 1f, 0f));
            ActiveStateStyle.onActive.background = MakeTex(2, 2, new Color(1f, 0.5f, 0f));
            ActiveStateStyle.active.background = MakeTex(2, 2, new Color(1f, 0.5f, 0f));


            bmod = new BroforceMod(mod);

            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.Label(new GUIContent(" = Enabled", MakeTex(15, 15, new Color(0f, 0.7f, 0f))));
            GUILayout.Label(new GUIContent(" = Disabled", MakeTex(15, 15, new Color(0.7f, 0f, 0f))));
            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            ProbabilityGUI();
            BoolGUI();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            ListGUI();
            FloatThingsGUI();
            GUILayout.EndHorizontal();
        }

        static void ProbabilityGUI()
        {
            GUILayout.BeginVertical("box");
            GUILayout.Label("Probability", LabelStyle);
            GUILayout.Space(10);

            settings.bAcidBarrel = GUIFloatBar(settings.bAcidBarrel, settings.acidBarrelSpawnProbability, "Acid barrel : ", out settings.acidBarrelSpawnProbability);
            settings.bAlienExplosiveBlock = GUIFloatBar(settings.bAlienExplosiveBlock, settings.alienExplosiveBlockSpawnProbability, "Alien Explosive Block : ", out settings.alienExplosiveBlockSpawnProbability);
            settings.bAmmoCrateRemoteCarProbability = GUIFloatBar(settings.bAmmoCrateRemoteCarProbability, settings.ammoCrateRemoteCarProbability, "Ammo Crate Remote Car : ", out settings.ammoCrateRemoteCarProbability);
            settings.bBigMookProbability = GUIFloatBar(settings.bBigMookProbability, settings.bigMookSpawnProbability, "Bruisers : ", out settings.bigMookSpawnProbability);
            settings.bMineFieldProbability = GUIFloatBar(settings.bMineFieldProbability, settings.mineFieldSpawnProbability, "Mine Field : ", out settings.mineFieldSpawnProbability);
            settings.bOilBarrelProbability = GUIFloatBar(settings.bOilBarrelProbability, settings.oilBarrelSpawnProbability, "Oil Barrel : ", out settings.oilBarrelSpawnProbability);
            settings.bPropaneTankProbability = GUIFloatBar(settings.bPropaneTankProbability, settings.propaneTankSpawnProbability, "Propane Tank : ", out settings.propaneTankSpawnProbability);
            settings.bRegularMookProbability = GUIFloatBar(settings.bRegularMookProbability, settings.regualrMookSpawnProbability, "Regular Terrorist : ", out settings.regualrMookSpawnProbability);
            settings.bShieldMookProbability = GUIFloatBar(settings.bShieldMookProbability, settings.riotShieldMookSpawnProbability, "Terrorist Shield :", out settings.riotShieldMookSpawnProbability);
            settings.bSpikeTrapProbability = GUIFloatBar(settings.bSpikeTrapProbability, settings.spikeTrapSpawnProbability, "Spike Trap : ", out settings.spikeTrapSpawnProbability);
            settings.bSuicideMookProbability = GUIFloatBar(settings.bSuicideMookProbability, settings.suicideMookSpawnProbability, "Suicide Terrorist : ", out settings.suicideMookSpawnProbability);
            settings.bCoconutProbability = GUIFloatBar(settings.bCoconutProbability, settings.coconutProbability, "Coconut Probability : ", out settings.coconutProbability);

            GUILayout.Space(10);
            GUILayout.EndVertical();
        }

        static void BoolGUI()
        {
            GUILayout.BeginVertical("box");
            GUILayout.Label("True/False", LabelStyle);

            settings.bShowBubbleMultiplayer = GUIBool(settings.bShowBubbleMultiplayer, settings.alwaysShowBubblesInMultiplayer, "Always Show Bulbbles in multiplayer : ", out settings.alwaysShowBubblesInMultiplayer);
            settings.bBloomEffect = GUIBool(settings.bBloomEffect, settings.bloomEffect, "Bloom Effect : ", out settings.bloomEffect);
            settings.bStartWithFullSpecial = GUIBool(settings.bStartWithFullSpecial, settings.brosStartWithFullSpecials, "Bros Start With Full Special : ", out settings.brosStartWithFullSpecials);
            settings.bHighFiveSlowLevel = GUIBool(settings.bHighFiveSlowLevel, settings.canHighFiveSlowDownThisLevel, "High five slow down this level : ", out settings.canHighFiveSlowDownThisLevel);
            settings.bConstantFireworks = GUIBool(settings.bConstantFireworks, settings.constantFireworks, "Constant Fireworks  : ", out settings.constantFireworks);
            settings.bForceHardMode = GUIBool(settings.bForceHardMode, settings.forceHardMode, "Force Hard Mode : ", out settings.forceHardMode);
            settings.bOnlyTriggerWin = GUIBool(settings.bOnlyTriggerWin, settings.onlyTriggersCanWinLevel, "Only Triggers can win this level : ", out settings.onlyTriggersCanWinLevel);
            settings.bRestartOnDeathHardcore = GUIBool(settings.bRestartOnDeathHardcore, settings.restartOnDeathInHardcore, "Restart on death in hardcore : ", out settings.restartOnDeathInHardcore);
            settings.bSpawnAmmoCrate = GUIBool(settings.bSpawnAmmoCrate, settings.spawnAmmoCrates, "Spawn Ammo Crate : ", out settings.spawnAmmoCrates);
            settings.bSupressAnnouncer = GUIBool(settings.bSupressAnnouncer, settings.suppressAnnouncer, "Supress Announcer : ", out settings.suppressAnnouncer);
            settings.bStandardDeathmatchCollapse = GUIBool(settings.bStandardDeathmatchCollapse, settings.useStandardDeathmatchCollapse, "Standard Collapse In Deathmatch", out settings.useStandardDeathmatchCollapse);
            settings.bVignettingEffect = GUIBool(settings.bVignettingEffect, settings.vignettingEffect, "Vignetting Effect : ", out settings.vignettingEffect);
            settings.bWaitForAllPlayer = GUIBool(settings.bWaitForAllPlayer, settings.waitForAllPlayersInOnline, "Wait for all player in online : ", out settings.waitForAllPlayersInOnline);

            GUILayout.Space(10);
            GUILayout.EndVertical();
        }

        static void ListGUI()
        {
            GUILayout.BeginVertical("box");
            GUILayout.Label("List Change", LabelStyle);

            settings.bAmbience = GUIList(settings.bShowBubbleMultiplayer, settings.ambience, "Ambience : ", AmbiencesTypeList, out settings.ambience);
            settings.bCameraFollow = GUIList(settings.bCameraFollow, settings.cameraFollowMode, "Camera follow mode : ", CameraFollowModesList, out settings.cameraFollowMode);
            settings.bForcedBro = GUIList(settings.bForcedBro, settings.forcedBro, "Forced Bro : ", HeroTypeListObj, out settings.forcedBro);
            settings.bHeroSpawnMode = GUIList(settings.bHeroSpawnMode, settings.heroSpawnMode, "Hero Spawn Mode : ", HeroSpawnModesList, out settings.heroSpawnMode);
            settings.bMusicType = GUIList(settings.bMusicType, settings.musicType, "Music Type : ", MusicTypesList, out settings.musicType);
            settings.bTheme = GUIList(settings.bTheme, settings.theme, "Level Theme : ", LevelThemeList, out settings.theme);
            settings.bWeatherType = GUIList(settings.bWeatherType, settings.weatherType, "Weather Type : ", WeatherTypesList, out settings.weatherType);

            GUILayout.Space(10);
            GUILayout.EndVertical();
        }

        static void FloatThingsGUI()
        {
            GUILayout.BeginVertical("box");
            GUILayout.Label("Float things :", LabelStyle);
            GUILayout.Space(10);

            settings.bAmmoCrateMultiplier = GUIFloatBar(settings.bAmmoCrateMultiplier, settings.ammoCrateFrequencyMultiplier, 10, "Ammo Crate Frequency Multiplier : ", out settings.ammoCrateFrequencyMultiplier);
            settings.bCameraSpeed = GUIFloatBar(settings.bCameraSpeed, settings.cameraSpeed, 100, "Camera Speed : ", out settings.cameraSpeed);
            settings.bCollapseInterval = GUIFloatBar(settings.bCollapseInterval, settings.collapseInterval, 10, "CollapseInterval : ", out settings.collapseInterval);
            settings.bBloomEffectM = GUIFloatBar(settings.bBloomEffectM, settings.bloomEffectM, 2, "Bloom Effect M : ", out settings.bloomEffectM);
            settings.bBloomThresholdM = GUIFloatBar(settings.bBloomThresholdM, settings.bloomThresholdM, 2, "Bloom Threshold M : ", out settings.bloomThresholdM);

            GUILayout.Space(10);
            GUILayout.EndVertical();
        }

        private static bool GUIFloatBar(bool bActive, float value, string name, out float rValue)
        {
            var ActiveStateStyle = new GUIStyle("toggle");
            ActiveStateStyle.normal.background = MakeTex(2, 2, new Color(0.7f, 0f, 0f));
            ActiveStateStyle.hover.background = MakeTex(2, 2, new Color(1f, 0f, 0f));
            ActiveStateStyle.onNormal.background = MakeTex(2, 2, new Color(0f, 0.7f, 0f));
            ActiveStateStyle.onHover.background = MakeTex(2, 2, new Color(0f, 1f, 0f));
            ActiveStateStyle.onActive.background = MakeTex(2, 2, new Color(1f, 0.5f, 0f));
            ActiveStateStyle.active.background = MakeTex(2, 2, new Color(1f, 0.5f, 0f));

            GUILayout.BeginHorizontal();
            bActive = GUILayout.Toggle(bActive, "", ActiveStateStyle);
            GUILayout.Label(name + value.ToString());
            GUILayout.FlexibleSpace();
            value = (float)GUILayout.HorizontalScrollbar(value, 0f, 0, 1f, GUILayout.MaxWidth(430));
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            rValue = value;

            return bActive;
        }
        private static bool GUIFloatBar(bool bActive, float value, float MaxRange, string name, out float rValue)
        {
            var ActiveStateStyle = new GUIStyle("toggle");
            ActiveStateStyle.normal.background = MakeTex(2, 2, new Color(0.7f, 0f, 0f));
            ActiveStateStyle.hover.background = MakeTex(2, 2, new Color(1f, 0f, 0f));
            ActiveStateStyle.onNormal.background = MakeTex(2, 2, new Color(0f, 0.7f, 0f));
            ActiveStateStyle.onHover.background = MakeTex(2, 2, new Color(0f, 1f, 0f));
            ActiveStateStyle.onActive.background = MakeTex(2, 2, new Color(1f, 0.5f, 0f));
            ActiveStateStyle.active.background = MakeTex(2, 2, new Color(1f, 0.5f, 0f));

            GUILayout.BeginHorizontal();
            bActive = GUILayout.Toggle(bActive, "", ActiveStateStyle);
            GUILayout.Label(name + value.ToString());
            GUILayout.FlexibleSpace();
            value = (float)GUILayout.HorizontalScrollbar(value, 0f, 0, MaxRange, GUILayout.MaxWidth(430));
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            rValue = value;

            return bActive;
        }

        private static bool GUIBool(bool bActive, bool value, string name, out bool rValue)
        {
            var ActiveStateStyle = new GUIStyle("toggle");
            ActiveStateStyle.normal.background = MakeTex(2, 2, new Color(0.7f, 0f, 0f));
            ActiveStateStyle.hover.background = MakeTex(2, 2, new Color(1f, 0f, 0f));
            ActiveStateStyle.onNormal.background = MakeTex(2, 2, new Color(0f, 0.7f, 0f));
            ActiveStateStyle.onHover.background = MakeTex(2, 2, new Color(0f, 1f, 0f));
            ActiveStateStyle.onActive.background = MakeTex(2, 2, new Color(1f, 0.5f, 0f));
            ActiveStateStyle.active.background = MakeTex(2, 2, new Color(1f, 0.5f, 0f));

            GUILayout.BeginHorizontal();
            bActive = GUILayout.Toggle(bActive, "", ActiveStateStyle);
            GUILayout.Label(name, GUILayout.Width(250));
            value = GUILayout.Toggle(value, "");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            rValue = value;

            return bActive;
        }

        private static bool GUIList(bool bActive, int value, string name, List<object> list, out int rValue)
        {
            var ActiveStateStyle = new GUIStyle("toggle");
            ActiveStateStyle.normal.background = MakeTex(2, 2, new Color(0.7f, 0f, 0f));
            ActiveStateStyle.hover.background = MakeTex(2, 2, new Color(1f, 0f, 0f));
            ActiveStateStyle.onNormal.background = MakeTex(2, 2, new Color(0f, 0.7f, 0f));
            ActiveStateStyle.onHover.background = MakeTex(2, 2, new Color(0f, 1f, 0f));
            ActiveStateStyle.onActive.background = MakeTex(2, 2, new Color(1f, 0.5f, 0f));
            ActiveStateStyle.active.background = MakeTex(2, 2, new Color(1f, 0.5f, 0f));

            GUILayout.BeginHorizontal();
            bActive = GUILayout.Toggle(bActive, "", ActiveStateStyle);
            GUILayout.Label(name, GUILayout.Width(150));
            value = RocketLib.RGUI.ArrowList(list, value, 200);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            rValue = value;

            return bActive;
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

        internal static void Log(object str)
        {
            bmod.Log(str);
        }

        internal static Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }



    [HarmonyPatch(typeof(MapData), "Deserialize")]
    class ChangeMapData_Patch
    {
        static void Postfix(MapData __instance)
        {
            if (!Main.enabled) return;

           /* Main.Log("Camera speed : "+__instance.cameraSpeed);
            Main.Log(__instance.ammoCrateFrequencyMultiplier);
            Main.Log(__instance.collapseInterval);
            Main.Log(__instance.bloomEffectM);
            Main.Log(__instance.bloomThresholdM);*/

            Settings sett = Main.settings;
            if (sett.bAcidBarrel) __instance.acidBarrelSpawnProbability = sett.acidBarrelSpawnProbability;
            if (sett.bAlienExplosiveBlock) __instance.alienExplosiveBlockSpawnProbability = sett.alienExplosiveBlockSpawnProbability;
            if (sett.bAmbience) __instance.ambience = Main.AmbiencesType[sett.ambience];
            if (sett.bAmmoCrateMultiplier) __instance.ammoCrateFrequencyMultiplier = sett.ammoCrateFrequencyMultiplier;
            if (sett.bAmmoCrateRemoteCarProbability) __instance.ammoCrateRemoteCarProbability = sett.ammoCrateRemoteCarProbability;
            if (sett.bBigMookProbability) __instance.bigMookSpawnProbability = sett.bigMookSpawnProbability;
            if (sett.bBloomEffect) __instance.bloomEffect = sett.bloomEffect;
            if (sett.bBloomEffectM) __instance.bloomEffectM = sett.bloomEffectM;
            if (sett.bBloomThresholdM) __instance.bloomThresholdM = sett.bloomThresholdM;
            if (sett.bCameraFollow) __instance.cameraFollowMode = Main.CameraFollowModes[sett.cameraFollowMode];
            if (sett.bCameraSpeed) __instance.cameraSpeed = sett.cameraSpeed;
            if (sett.bCoconutProbability) __instance.coconutProbability = sett.coconutProbability;
            if (sett.bCollapseInterval) __instance.collapseInterval = sett.collapseInterval;
            if (sett.bConstantFireworks) __instance.constantFireworks = sett.constantFireworks;
            if (sett.bForcedBro) __instance.forcedBro = (HeroType)Main.HeroTypeListObj[sett.forcedBro];
            if (sett.bForceHardMode) __instance.forceHardMode = sett.forceHardMode;
            if (sett.bHeroSpawnMode) __instance.heroSpawnMode = Main.HeroSpawnModes[sett.heroSpawnMode];
            if (sett.bHighFiveSlowLevel) __instance.canHighFiveSlowDownThisLevel = sett.canHighFiveSlowDownThisLevel;
            if (sett.bMineFieldProbability) __instance.mineFieldSpawnProbability = sett.mineFieldSpawnProbability;
            if (sett.bMusicType) __instance.musicType = Main.MusicTypes[sett.musicType];
            if (sett.bOilBarrelProbability) __instance.oilBarrelSpawnProbability = sett.oilBarrelSpawnProbability;
            if (sett.bOnlyTriggerWin) __instance.onlyTriggersCanWinLevel = sett.onlyTriggersCanWinLevel;
            if (sett.bPropaneTankProbability) __instance.propaneTankSpawnProbability = sett.propaneTankSpawnProbability;
            if (sett.bRegularMookProbability) __instance.regualrMookSpawnProbability = sett.regualrMookSpawnProbability;
            if (sett.bRestartOnDeathHardcore) __instance.restartOnDeathInHardcore = sett.restartOnDeathInHardcore;
            if (sett.bShieldMookProbability) __instance.riotShieldMookSpawnProbability = sett.riotShieldMookSpawnProbability;
            if (sett.bShowBubbleMultiplayer) __instance.alwaysShowBubblesInMultiplayer = sett.alwaysShowBubblesInMultiplayer;
            if (sett.bSpawnAmmoCrate) __instance.spawnAmmoCrates = sett.spawnAmmoCrates;
            if (sett.bSpikeTrapProbability) __instance.spikeTrapSpawnProbability = sett.spikeTrapSpawnProbability;
            if (sett.bStandardDeathmatchCollapse) __instance.useStandardDeathmatchCollapse = sett.useStandardDeathmatchCollapse;
            if (sett.bStartWithFullSpecial) __instance.brosStartWithFullSpecials = sett.brosStartWithFullSpecials;
            if (sett.bSuicideMookProbability) __instance.suicideMookSpawnProbability = sett.suicideMookSpawnProbability;
            if (sett.bSupressAnnouncer) __instance.suppressAnnouncer = sett.suppressAnnouncer;
            if (sett.bTheme) __instance.theme = Main.LevelThemes[sett.theme];
            if (sett.bVignettingEffect) __instance.vignettingEffect = sett.vignettingEffect;
            if (sett.bWaitForAllPlayer) __instance.waitForAllPlayersInOnline = sett.waitForAllPlayersInOnline;
            if (sett.bWeatherType) __instance.weatherType = Main.WeatherTypes[sett.weatherType];
        }
    }
}
