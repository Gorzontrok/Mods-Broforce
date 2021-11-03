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

        internal static BroforceMod bmod;

        internal static Dictionary<int, HeroType> HeroDictionary = new Dictionary<int, HeroType>();

        internal static bool origDicIsSet = true;

        internal static string ResFolder;

        internal static Harmony harmony;

        internal static string[] ArcadeCampaign = new string[] { "Normal", "Expendabros", "TWITCHCON", "Alien Demo", "Boss Rush", "Hell Arcade" };
        internal static string CurentArcade;

        private static string BtnAdvancedOptionTxt = string.Empty;
        private static string BtnDangerZoneTxt = string.Empty;

        private static int SaveSlotToDelete = 1;

        private static GUIStyle ToolTipStyle = new GUIStyle();
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

            try
            {
                bmod = new BroforceMod(mod, _UseLocalLog: true);
            }
            catch(Exception ex)
            {
                mod.Logger.Log("Failed create BroforceMod :\n" + ex);
            }

            try
            {
                Start();
            }
            catch(Exception ex)
            {
                mod.Logger.Log(ex.ToString());
            }

            if (!settings.getFirstLaunch) FirstLaunch();

            return true;
        }
        static void Start()
        {
            Compatibility.Load();
            for(int i = 0; i < Compatibility.BroforceModsList.Count; i++)
            {
                Compatibility.IsThisModTFP broforcemod = Compatibility.BroforceModsList[i];
                if (broforcemod.i.IsHere)
                {
                    string end = broforcemod.i.IsEnabled ? " and is active." : ".";
                    bmod.InformationLog("'" + broforcemod.i.ID + "' is here" + end, true);

                    if(!Compatibility.GetCompatibilityBool(broforcemod.i.ID))
                    {
                        bmod.WarningLog($"Compatibility with the mod : '{broforcemod.i.ID}' is off.", false);
                    }
                }
            }

            // Set Mouse of the game
            if (settings.SetCustomMouse)
            {
                ShowMouseController.SetCursorToArrow(true);
                settings.CustomMouseIsSet = true;
                bmod.InformationLog("Custom mouse is set.", true);
            }
            else settings.CustomMouseIsSet = false;

            ResFolder = mod.Path + "/Ressource/";

            ToolTipStyle = new GUIStyle();
            ToolTipStyle.fontStyle = FontStyle.Bold;
            ToolTipStyle.fontSize = 15;
            ToolTipStyle.normal.textColor = Color.white;
        }

        static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            if (!Main.enabled) return;
            // Build Unlock Intervals
            HeroDictionary = Utility.BuildHeroDictionary();

            // Set T-Bag
            if (settings.TbagEnabled) TestVanDammeAnim.teaBagCheatEnabled = true;
            else TestVanDammeAnim.teaBagCheatEnabled = false;

            //Set custom Frame rate
            if (settings.UseCustomFramerate)
            { Application.targetFrameRate = settings.MaxFramerate; QualitySettings.vSyncCount = 0; }
            else QualitySettings.vSyncCount = 1;

            // Change string of Advanced Btn
            if (settings.ShowAdvancedOption) BtnAdvancedOptionTxt = "Hide advanced settings";
            else BtnAdvancedOptionTxt = "Show advanced settings";
            // Change txt of danger btn
            if (settings.DangerZoneOpen) BtnDangerZoneTxt = "CLOSE DANGER ZONE";
            else BtnDangerZoneTxt = "OPEN DANGER ZONE";

            settings.NeedReload = TheseVarHaveChangeValue();

            // Custom Mouse change color
            if(settings.SetCustomMouse)
            {
                if(!settings.CustomMouseIsSet)
                {
                    ShowMouseController.SetCursorToArrow(true);
                    settings.CustomMouseIsSet = true;
                }
                if (Input.GetMouseButton(0)) ShowMouseController.HilightCursor();
            }


            // Arcade choice
            CurentArcade = ArcadeCampaign[settings.ArcadeIndex];
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

            GUILayout.Space(15);
            Rect ToolTipRect = GUILayoutUtility.GetLastRect();
            GUILayout.Space(15);

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
                GUILayout.Label("- Makes X mod compatible :", TitleStyle);
                GUILayout.BeginVertical("box");
                GUILayout.BeginHorizontal();
                if (Compatibility.FilteredBros.i.IsHere) settings.FilteredBros_Compatibility = GUILayout.Toggle(settings.FilteredBros_Compatibility, new GUIContent("Filtered Bros", "Makes the mod compatible with Filtered Bros Mod."));
                if (Compatibility.ExpendablesBros.i.IsHere) settings.ExpendablesBros_Compatibility= GUILayout.Toggle(settings.ExpendablesBros_Compatibility, new GUIContent("Expendabros In Game", "Makes the mod compatible with Expendalbes Bros In Game mod."));
                if (Compatibility._007_Patch.i.IsHere) settings._007Patch_Compatibility= GUILayout.Toggle(settings._007Patch_Compatibility, new GUIContent("007 Patch", "Makes the mod compatible with Expendalbes Bros In Game mod."));
                if (Compatibility.ForBralef.i.IsHere) settings.ForBralef_Compatibility = GUILayout.Toggle(settings.ForBralef_Compatibility, new GUIContent("ForBralef", "Makes the mod compatible with ForBralef mod."));
                if (Compatibility.AvatarFaceHugger.i.IsHere) settings.AvatarFaceHugger_Compatibility = GUILayout.Toggle(settings.AvatarFaceHugger_Compatibility, new GUIContent("Avatar FaceHugger", "Makes the mod compatible with Avatar FaceHugger mod."));
                if (Compatibility.SkeletonDeadFace.i.IsHere) settings.SkeletonDeadFace_Compatibility = GUILayout.Toggle(settings.SkeletonDeadFace_Compatibility, new GUIContent("Skeleton Dead Face", "Makes the mod compatible with Skeleton Dead Face mod."));
                if (Compatibility.MapDataController.i.IsHere) settings.MapDataController_Compatibility = GUILayout.Toggle(settings.MapDataController_Compatibility, new GUIContent("Map Data Controller", "Makes the mod compatible with Map Data Controller mod."));
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
            settings.ChangeHeroUnlock = GUILayout.Toggle(settings.ChangeHeroUnlock, new GUIContent("Use custom HeroUnlock of this mod", "If you have active compatibility of a mod which change the Unlock Intervals, Tweaks From Pigs will do nothing."));
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
            settings.TbagEnabled = GUILayout.Toggle(settings.TbagEnabled, new GUIContent("Enabled T-Bag", "You have a good taste."));
            settings.UseFifthBondSpecial = GUILayout.Toggle(settings.UseFifthBondSpecial, new GUIContent("Use the 5th special of 007", "This is a Tear Gas"));
            settings.RememberPockettedSpecial = GUILayout.Toggle(settings.RememberPockettedSpecial, new GUIContent("Remember Pocketted Special", "When you are alive and tou change your bro, you keep pocketted special. (Work with Swap Bros Mod)"));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(10);
            GUILayout.Label("- Ennemies :", TitleStyle);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            settings.FasterZombie = GUILayout.Toggle(settings.FasterZombie, new GUIContent("Faster Zombie", "The zombie are faster. You can change the value in Advanced Section"));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(10);
            GUILayout.Label("- Map :", TitleStyle);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Arcade Type :", "When choose, play Arcade level 1. You can play Online."));
            settings.ArcadeIndex = RocketLib.RGUI.ArrowList(new List<object>(ArcadeCampaign), settings.ArcadeIndex, 200);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
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

            GUI.Label(ToolTipRect, GUI.tooltip, ToolTipStyle);

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
            Rect ToolTipRect = GUILayoutUtility.GetLastRect();
            GUILayout.Space(10);

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
            GUILayout.Label(" Zombie Speed modifier : ", GUILayout.ExpandWidth(false));
            GUILayout.Label(settings.ZombieSpeedModifier.ToString(), GUILayout.Width(55));
            settings.ZombieSpeedModifier = (float)GUILayout.HorizontalScrollbar(settings.ZombieSpeedModifier, 0f, 1f, 5, GUILayout.MaxWidth(500));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            settings.CanTeargasedEveryone = GUILayout.Toggle(settings.CanTeargasedEveryone, new GUIContent("TearGas everyone", "Some ennemies can't be teargased."));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(10);
            GUILayout.Label("- Map :", TitleStyle);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
                GUILayout.Label(" Acid Barrel spawn rate : ", GUILayout.ExpandWidth(false));
            GUILayout.Label(settings.AcidBarrelSpawnChance.ToString(), GUILayout.Width(70));
                settings.AcidBarrelSpawnChance = (float)GUILayout.HorizontalScrollbar(settings.AcidBarrelSpawnChance, 0f, 0.0f, 1f, GUILayout.MaxWidth(500));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(10);
            GUILayout.Label("- Menu :", TitleStyle);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            settings.MaxArcadeLevelEnabled = GUILayout.Toggle(settings.MaxArcadeLevelEnabled, new GUIContent("Max Arcade Level", "On different arcade, block the level at the max level playable."));
            settings.LanguageMenuEnabled = GUILayout.Toggle(settings.LanguageMenuEnabled, new GUIContent("Re-Enabled Language menu", "You can change language of the game in option menu."));
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

            GUI.Label(ToolTipRect, GUI.tooltip, ToolTipStyle);

            GUILayout.Space(10);
            var DangerZoneTxt = new GUIStyle("button");
            DangerZoneTxt.fontStyle = FontStyle.Bold;
            DangerZoneTxt.fontSize = 15;
            DangerZoneTxt.normal.textColor = Color.red;

            GUILayout.BeginHorizontal();
            GUILayout.Space(100);
            settings.DangerZoneOpen = GUILayout.Toggle(settings.DangerZoneOpen, BtnDangerZoneTxt, DangerZoneTxt);
            GUILayout.Space(100);
            GUILayout.EndHorizontal();
            if (settings.DangerZoneOpen) DangerZoneGUI();
        }

        static void DangerZoneGUI()
        {
            var BoxDangerStyle = new GUIStyle("box");
            BoxDangerStyle.normal.background = Utility.MakeTex(2, 2, new Color(0.5f, 0f, 0f, 0.5f));

            var RedTxt = new GUIStyle();
            RedTxt.normal.textColor = Color.red;

            GUILayout.Space(30);

            Rect ToolTipRect = GUILayoutUtility.GetLastRect();

            GUILayout.BeginHorizontal(BoxDangerStyle);
            GUILayout.Label("Save slot to delete : ", GUILayout.ExpandWidth(false));
            GUILayout.Label(SaveSlotToDelete.ToString(), GUILayout.ExpandWidth(false));
            SaveSlotToDelete = (int)GUILayout.HorizontalScrollbar(SaveSlotToDelete, 0, 1, 5, GUILayout.MaxWidth(100));
            if (GUILayout.Button(new GUIContent("DELETE", "BE CAREFUL")))
            {
                PlayerProgress.Instance.saveSlots[SaveSlotToDelete - 1] = null;
                PlayerProgress.Save(true);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUI.Label(ToolTipRect, GUI.tooltip, ToolTipStyle);
        }

        static bool TheseVarHaveChangeValue()
        {
            if (!settings.SetCustomMouse && settings.CustomMouseIsSet) return true;
            return false;
        }

        static void FirstLaunch()
        {
            settings.getFirstLaunch = true;
            settings.MaxFramerate = 30;

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
            settings.AcidBarrelSpawnChance = 0.2f;
            //Menu
            settings.MaxArcadeLevelEnabled = true;
            settings.LanguageMenuEnabled = true;
            // other
            settings.EnabledSickPigs = true;
            settings.MechDropDoesFumiginene = true;

            OnSaveGUI(mod);
        }

        static void OnHideGUI(UnityModManager.ModEntry modEntry)
        {
            settings.DangerZoneOpen = false;
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
    }
}
