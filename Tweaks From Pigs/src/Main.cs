using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;
using RocketLib0;

namespace TweaksFromPigs
{
    public static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        public static Dictionary<int, HeroType> HeroDictionary = new Dictionary<int, HeroType>();

        public static bool origDicIsSet = true;
        public static bool origUnpatchFilteredBros;

        public static string ResFolder;

        public static Harmony harmony;
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            modEntry.OnUpdate = OnUpdate;
            modEntry.OnHideGUI = OnHideGUI;
            settings = Settings.Load<Settings>(modEntry);

            mod = modEntry;

            try
            {
                harmony = new Harmony(modEntry.Info.Id);
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);
            }
            catch (Exception ex)
            {
                mod.Logger.Log("Failed to Patch Harmony !\n" + ex.ToString());
            }


            try { Start();  }
            catch(Exception ex) { mod.Logger.Log(ex.ToString()); }

            if (!settings.getFirstLaunch) FirstLaunch();

            return true;
        }
        static void Start()
        {
            var IsAvailableInC_Patch = typeof(HeroUnlockController).GetMethod("IsAvailableInCampaign");
            settings.FilteredBrosIsHere = CheckIfThisModIsHereAndUnpatchPrefix(IsAvailableInC_Patch, "FilteredBrosMod", settings.UnpatchFilteredBros);
            settings.ExpendabrosModIsHere = CheckIfThisModIsHereAndUnpatchPrefix(IsAvailableInC_Patch, "ExpendaBrosInGame", settings.UnpatchExpendabrosMod);
            settings.ForBralefIsHere = CheckIfThisModIsHereAndUnpatchPrefix(IsAvailableInC_Patch, "ForBralef", settings.UnpatchForBralef);
            if (settings.ExpendabrosModIsHere && settings.UnpatchExpendabrosMod) harmony.UnpatchAll("ExpendaBrosInGame");

            if (settings.SetCustomMouse)
            {
                ShowMouseController.SetCursorToArrow(true);
                settings.CustomMouseIsSet = true;
            }
            else settings.CustomMouseIsSet = false;
            origUnpatchFilteredBros = settings.UnpatchFilteredBros;

            ResFolder = mod.Path + "/Ressource/";

            mod.Logger.Log(PlayerProgress.Instance.freedBros.ToString());
        }

        static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            if (!Main.enabled) return;
            HeroDictionary = Utility.BuildHeroDictionary();

            if (settings.TbagEnabled) TestVanDammeAnim.teaBagCheatEnabled = true;
            else TestVanDammeAnim.teaBagCheatEnabled = false;

            if (settings.UseCustomFramerate)
            { Application.targetFrameRate = settings.MaxFramerate; QualitySettings.vSyncCount = 0; }
            else QualitySettings.vSyncCount = 1;

            if (settings.ShowAdvancedOption) BtnAdvancedOptionTxt = "Hide advanced settings";
            else BtnAdvancedOptionTxt = "Show advanced settings";

            settings.NeedReload = TheseVarHaveChangeValue();

            PlaytomicController.hasChosenBossRushDemo = true;
            if(settings.SetCustomMouse)
            {
                if(!settings.CustomMouseIsSet)
                {
                    ShowMouseController.SetCursorToArrow(true);
                    settings.CustomMouseIsSet = true;
                }
                if (Input.GetMouseButton(0)) ShowMouseController.HilightCursor();
            }
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            var TitleStyle = new GUIStyle();
            TitleStyle.fontStyle = FontStyle.Bold;
            TitleStyle.fontSize = 13;
            TitleStyle.normal.textColor = Color.white;

            var ReloadStyle = new GUIStyle();
            ReloadStyle.fontSize = 15;
            ReloadStyle.normal.textColor = Color.yellow;

            /*if (GUILayout.Button("df"))
            {
                List<int> inn = new List<int>();
                for (int i = 30; i < 65; i++)inn.Add(i);
                PlayerProgress.Instance.CompletedArcadeMissions.AddRange(inn);
                    foreach (int i in PlayerProgress.Instance.CompletedArcadeMissions) Main.Log(i);
            }*/

            GUILayout.BeginHorizontal();
            if(settings.NeedReload)
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("If you want change being applied, you need to reload !", ReloadStyle);
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(30);

            GUILayout.BeginHorizontal();
            settings.UseCustomFramerate = GUILayout.Toggle(settings.UseCustomFramerate, new GUIContent("Use custom Framerate"));
            GUILayout.EndHorizontal();
            if (settings.UseCustomFramerate)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                GUILayout.Label("Max Framerate :", GUILayout.ExpandWidth(false));
                int.TryParse(GUILayout.TextField(settings.MaxFramerate.ToString(), GUILayout.Width(100)), out settings.MaxFramerate);
                GUILayout.EndHorizontal();
            }
                GUILayout.Space(10);
                GUILayout.Label("- Sensitive variable :", TitleStyle);
                GUILayout.BeginVertical("box");
                GUILayout.BeginHorizontal();
                if (settings.FilteredBrosIsHere) settings.UnpatchFilteredBros = GUILayout.Toggle(settings.UnpatchFilteredBros, new GUIContent("Unpatch Filtered Bros Mod on start", "This is for avoiding bug with the Hero Unlock Intervals from this Mods."));
                if (settings.ExpendabrosModIsHere) settings.UnpatchExpendabrosMod = GUILayout.Toggle(settings.UnpatchExpendabrosMod, new GUIContent("Unpatch Expendabros In Game Mod on start", "This is for avoiding bug with the Hero Unlock Intervals and the expendabros fixes from this Mods."));
                if (settings.ForBralefIsHere) settings.UnpatchForBralef = GUILayout.Toggle(settings.UnpatchForBralef, new GUIContent("Unpatch ForBralef Mod on start", "This is for avoiding bug with the Hero Unlock Intervals from this Mods."));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();


            GUILayout.Space(10);
            GUILayout.Label("- Animation :", TitleStyle);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            settings.UsePushingFrame = GUILayout.Toggle(settings.UsePushingFrame, new GUIContent("Use Pushing Frame", "Animation when you push a block."));
            settings.UseNewLadderFrame = GUILayout.Toggle(settings.UseNewLadderFrame, new GUIContent("Use New Ladder Animation", "Animation when you climb/are on a ladder."));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(10);
            GUILayout.Label("- Hero Unlock Controller :", TitleStyle);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            settings.ChangeHeroUnlock = GUILayout.Toggle(settings.ChangeHeroUnlock, new GUIContent("Use custom HeroUnlock of this mod", "If you have another mod who change the Unlock Intervals, don't use it or disable it. Unless you have disable the mod."));
            settings.SpawnWithExpendabros = GUILayout.Toggle(settings.SpawnWithExpendabros, new GUIContent("Spawn With Expendabros"));
            settings.SpawnBrondeFly = GUILayout.Toggle(settings.SpawnBrondeFly, new GUIContent("Spawn With Brondle Fly"));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(10);
            GUILayout.Label("- HUD :", TitleStyle);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            settings.ShowFacehuggerHUD = GUILayout.Toggle(settings.ShowFacehuggerHUD, new GUIContent("Show FaceHugger on HUD", "Show a facehugger on the avatar when you have an alien on the head."));
            settings.SkeletonDeadFace = GUILayout.Toggle(settings.SkeletonDeadFace, new GUIContent("Show skeleton Dead Face", "Show the Skeleton head of Expendabros when you are dead"));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(10);
            GUILayout.Label("- Bro :", TitleStyle);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            settings.TbagEnabled = GUILayout.Toggle(settings.TbagEnabled, new GUIContent("Enabled T-Bag", "This have a good taste"));
            settings.UseFifthBondSpecial = GUILayout.Toggle(settings.UseFifthBondSpecial, new GUIContent("Use the 5th special of 007", "This is a Tear Gas"));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(10);
            GUILayout.Label("- Ennemies :", TitleStyle);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            settings.FasterZombie = GUILayout.Toggle(settings.FasterZombie, new GUIContent("Faster Zombie", "The zombie are the bro and ennemies who are revive by Bronniversal Soldier"));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(10);
            GUILayout.Label("- Map :", TitleStyle);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            settings.UseAcidBarrel = GUILayout.Toggle(settings.UseAcidBarrel, new GUIContent("Enabled Acid barrel to spawn", "They are unused in Campaign i just enabled them."));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(10);
            GUILayout.Label("- Other :", TitleStyle);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            settings.PigAreAlwaysTerror = GUILayout.Toggle(settings.PigAreAlwaysTerror, new GUIContent("Terrorist Pigs", "Terrorist pigs are the best <3"));
            settings.SetCustomMouse = GUILayout.Toggle(settings.SetCustomMouse, new GUIContent("Cursor from level Editor", "Why ? Why not."));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            var ToolTipStyle = new GUIStyle();
            ToolTipStyle.fontStyle = FontStyle.Bold;
            ToolTipStyle.fontSize = 15;
            ToolTipStyle.normal.textColor = Color.white;

            GUILayout.BeginHorizontal();
            Rect lastRect = GUILayoutUtility.GetLastRect();
            lastRect.y -= 350;
            //lastRect.x += 250;
            lastRect.width += 300;
            GUI.Label(lastRect, GUI.tooltip, ToolTipStyle);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            var AdvancedStyle = new GUIStyle("button");
            AdvancedStyle.fontStyle = FontStyle.Bold;
            AdvancedStyle.fontSize = 15;
            AdvancedStyle.normal.textColor = Color.yellow;

            GUILayout.BeginHorizontal();
            GUILayout.Space(100);
            settings.ShowAdvancedOption = GUILayout.Toggle(settings.ShowAdvancedOption, BtnAdvancedOptionTxt, AdvancedStyle);
            GUILayout.Space(100);
            GUILayout.EndHorizontal();
            if (settings.ShowAdvancedOption) AdvancedOptionGUI();
        }
        private static string BtnAdvancedOptionTxt = "Show Advanced settings";

        static void AdvancedOptionGUI()
        {
            var TitleStyle = new GUIStyle();
            TitleStyle.fontStyle = FontStyle.Bold;
            TitleStyle.fontSize = 13;
            TitleStyle.normal.textColor = Color.white;

            var InfoStyle = new GUIStyle();
            InfoStyle.normal.textColor = Color.yellow;

            GUILayout.BeginVertical("box");
            settings.CloseAdvancedOptionOnExit = GUILayout.Toggle(settings.CloseAdvancedOptionOnExit, "Close this when exit");
            GUILayout.EndVertical();

            GUILayout.Space(10);
            GUILayout.Label("- Animation :", TitleStyle);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            settings.FixPushingAnimation = GUILayout.Toggle(settings.FixPushingAnimation, new GUIContent("Fix Pushing Animation", "Fix animation bug of some bro."));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(10);
            GUILayout.Label("- Bro :", TitleStyle);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            settings.FixIndianaAchievement = GUILayout.Toggle(settings.FixIndianaAchievement, new GUIContent("Fix Front Row Ticket", "Fix the achievement for unlock it."));
            settings.TeargasAtFeet = GUILayout.Toggle(settings.TeargasAtFeet, new GUIContent("Teargas at feet", "If you are ducking you throw the teargas at your feet."));
            settings.LessAccurateDrunkSeven = GUILayout.Toggle(settings.LessAccurateDrunkSeven, new GUIContent("I'm drunk", "007 is less accurate when drunk."));
            settings.FixExpendabros = GUILayout.Toggle(settings.FixExpendabros, new GUIContent("Fix Expendabros", "Some tweaks change for Expendabros."));
            settings.LessOPBroniversalRevive = GUILayout.Toggle(settings.LessOPBroniversalRevive, new GUIContent("Slower Broniversal special", "Wait ~0.5sec before reuse special."));
            settings.BroniversalAutoRevive = GUILayout.Toggle(settings.BroniversalAutoRevive, new GUIContent("Broniversal Auto-Revive", "If dead Broniversal will automaticaly revive."));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(10);
            GUILayout.Label("- Ennemies :", TitleStyle);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            GUILayout.Label("Zombie Speed modifier : " + settings.ZombieSpeedModifier.ToString(), GUILayout.ExpandWidth(false));
            settings.ZombieSpeedModifier = (float)GUILayout.HorizontalScrollbar(settings.ZombieSpeedModifier, 0.5f, 1f, 5, GUILayout.MaxWidth(500));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            settings.CanTeargasedEveryone = GUILayout.Toggle(settings.CanTeargasedEveryone, new GUIContent("TearGas everyone", "Some ennemies can't be teargased.-"));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(10);
            GUILayout.Label("- Map :", TitleStyle);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
                GUILayout.Label("Acid Barrel spawn rate : " + settings.AcidBarrelSpawnChance.ToString(), GUILayout.ExpandWidth(false));
                settings.AcidBarrelSpawnChance = (float)GUILayout.HorizontalScrollbar(settings.AcidBarrelSpawnChance, 0f, 0.5f, 1f, GUILayout.MaxWidth(500));
                GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(10);
            GUILayout.Label("- Other :", TitleStyle);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            settings.EnabledSickPigs = GUILayout.Toggle(settings.EnabledSickPigs, new GUIContent("Sick Pigs", "Enabled sick pigs to spawn."));
            settings.MechDropDoesFumiginene = GUILayout.Toggle(settings.MechDropDoesFumiginene, new GUIContent("Mech drop smoke", "Grenade of mech drop does smoke like Tank Bro grenade."));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        public static bool CheckIfThisModIsHereAndUnpatchPrefix(MethodInfo methodInfo, string HarmonyId, bool unpatchThis, HarmonyPatchType patchType = HarmonyPatchType.Prefix)
        {
            // https://harmony.pardeike.net/articles/basics.html#checking-for-existing-patches

            // retrieve all patches
            var patches = Harmony.GetPatchInfo(methodInfo);
            if (patches is null) return false; // not patched

            // get info about all Prefixes/Postfixes/Transpilers
            foreach (var patch in patches.Prefixes)
            {
                if (patch.owner == HarmonyId)
                {
                    Main.Log(HarmonyId + " here", RLogType.Information);
                    if(unpatchThis)
                    {
                        harmony.Unpatch(methodInfo, patchType, HarmonyId);
                        Main.Log(HarmonyId + " Unpatch !", RLogType.Information);
                    }
                    return true;
                }
            }
            return false;
        }

        static bool TheseVarHaveChangeValue()
        {
            if (settings.UnpatchFilteredBros != origUnpatchFilteredBros) return true;
            if (!settings.SetCustomMouse && settings.CustomMouseIsSet) return true;
            return false;
        }

        static void FirstLaunch()
        {
            settings.getFirstLaunch = true;
            settings.MaxFramerate = 10;

            // Bros addon/fix
            settings.UseFifthBondSpecial = true;

            // HUD
            settings.ShowFacehuggerHUD = true;
            settings.SkeletonDeadFace = true;

            // Map
            settings.UseAcidBarrel = true;

            // - Advanced :
            // Animation
            settings.FixPushingAnimation = true;
            // Bro
            settings.FixIndianaAchievement = true;
            settings.TeargasAtFeet = true;
            settings.LessOPBroniversalRevive = true;
            settings.LessAccurateDrunkSeven = true;
            settings.BroniversalAutoRevive = true;
            settings.FixExpendabros = true;
            //Enemies
            settings.CanTeargasedEveryone = true;
            settings.ZombieSpeedModifier = 1.2f;
            // Map
            settings.AcidBarrelSpawnChance = 0.4f;
            // other
            settings.EnabledSickPigs = true;
            settings.MechDropDoesFumiginene = true;
        }

        static void OnHideGUI(UnityModManager.ModEntry modEntry)
        {
            if (settings.CloseAdvancedOptionOnExit) settings.ShowAdvancedOption = false;
           /* if(!LevelEditorGUI.IsActive) ShowMouseController.ShowMouse = false;
            Cursor.lockState = CursorLockMode.None;*/
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
        public static void Log(object str, RLogType type = RLogType.Log)
        {
            if (!RocketLib.ScreenLogger.isSuccessfullyLoad) mod.Logger.Log(str.ToString());
            else
            {
                RocketLib.ScreenLogger.ModId = mod.Info.Id;
                RocketLib.ScreenLogger.Log(str, type);
            }
        }
        public static void Log(IEnumerable<object> str, RLogType type = RLogType.Log)
        {
            if (!RocketLib.ScreenLogger.isSuccessfullyLoad) mod.Logger.Log(str.ToString());
            else
            {
                RocketLib.ScreenLogger.ModId = mod.Info.Id;
                RocketLib.ScreenLogger.Log(str, type);
            }
        }
    }
}
