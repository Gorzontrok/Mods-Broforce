using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;
using RocketLib0;

namespace TweaksFromPigs
{
    internal static class UMM_GUI
    {

        private static GUIStyle ToolTipStyle = new GUIStyle();
        private static GUIStyle TitleStyle = new GUIStyle();
        private static GUIStyle DangerZoneStyle = new GUIStyle();

        private static int TabSelected;
        private static int SaveSlotToDelete;
        private static Vector2 _scrollViewVector;

        private static Settings Sett
        {
            get
            {
                return Main.settings;
            }
        }

        internal static void Init()
        {
            ToolTipStyle = new GUIStyle();
            ToolTipStyle.fontStyle = FontStyle.Bold;
            ToolTipStyle.fontSize = 15;
            ToolTipStyle.normal.textColor = Color.white;

            DangerZoneStyle = new GUIStyle("button");
            DangerZoneStyle.fontStyle = FontStyle.Bold;
            DangerZoneStyle.fontSize = 15;
            DangerZoneStyle.normal.textColor = Color.red;

            TitleStyle = new GUIStyle();
            TitleStyle.fontStyle = FontStyle.Bold;
            TitleStyle.fontSize = 13;
            TitleStyle.normal.textColor = Color.white;
        }

        internal static void GlobalGUI()
        {
            List<string> tab = new List<string> { "Main", "Animal","Bros", "Dooodads", "Enemies", "Interface", "Levels", "Other" };
            TabSelected = RocketLib.RGUI.Tab(tab.ToArray(), TabSelected, 10, 100);

            GUILayout.Space(15);
            Rect ToolTipRect = GUILayoutUtility.GetLastRect();
            GUILayout.Space(5);

            _scrollViewVector = GUILayout.BeginScrollView(_scrollViewVector, GUILayout.Height(400));
            switch (TabSelected)
            {
                case 0: GUI_Main(TitleStyle, ToolTipRect); break;
                case 1: GUI_Animal(TitleStyle, ToolTipRect); break;
                case 2: GUI_Bros(TitleStyle, ToolTipRect); break;
                case 3: GUI_Doodads(TitleStyle, ToolTipRect); break;
                case 4: GUI_Enemies(TitleStyle, ToolTipRect); break;
                case 5: GUI_Interface(TitleStyle, ToolTipRect); break;
                case 6: GUI_Map(TitleStyle, ToolTipRect); break;
                case 7: GUI_Other(TitleStyle, ToolTipRect); break;
            }
            GUILayout.EndScrollView();
        }

        private static void GUI_Main(GUIStyle TitleStyle, Rect ToolTipRect)
        {
            GUILayout.BeginVertical("box");

            GUILayout.BeginHorizontal();
            Sett.useCustomFramerate = GUILayout.Toggle(Sett.useCustomFramerate, new GUIContent("Use custom Frame-rate"));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                GUILayout.Label("Max Frame-rate :", GUILayout.ExpandWidth(false));
                int.TryParse(GUILayout.TextField(Sett.maxFramerate.ToString(), GUILayout.Width(100)), out Sett.maxFramerate);
                GUILayout.EndHorizontal();
            }

            if(Main.GorzonBuild)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Player Name :", GUILayout.ExpandWidth(false));
                Sett.customPlayerName = GUILayout.TextField(Sett.customPlayerName, GUILayout.MinWidth(100));
                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            GUI.Label(ToolTipRect, GUI.tooltip, ToolTipStyle);
        }

        private static void GUI_Animal(GUIStyle TitleStyle, Rect ToolTipRect)
        {
            GUILayout.BeginVertical("box");

            BeginVerticalBox($"- {SecretText("Pigs")} :");
            GUILayout.BeginHorizontal();
            Sett.pigAreAlwaysTerror = GUILayout.Toggle(Sett.pigAreAlwaysTerror, new GUIContent($"Terrorist {SecretText("Pigs")}", $"Terrorist {SecretText("Pigs")} are the best <3"));
            Sett.sickPigs = GUILayout.Toggle(Sett.sickPigs, new GUIContent($"Sick {SecretText("Pigs")}", $"{SecretText("Pigs")} explode when you finish to ride them."));
            Sett.sickPigsPobability = ProbalityScrollBar(Sett.sickPigsPobability, $"Sick {SecretText("Pigs")} probability");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.EndVertical();
        }
        private static void GUI_Bros(GUIStyle TitleStyle, Rect ToolTipRect)
        {
            GUILayout.BeginVertical("box");

            GUILayout.BeginHorizontal();
            Sett.fixIndianaAchievement = GUILayout.Toggle(Sett.fixIndianaAchievement, new GUIContent("Fix Front Row Ticket", "Fix the achievement for unlock it."));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            BeginVerticalBox("- Animation :");
            GUILayout.BeginHorizontal();
            Sett.usePushingFrame = GUILayout.Toggle(Sett.usePushingFrame, new GUIContent("Use Pushing Frame", "Animation when you push a block."));
            Sett.useNewLadderFrame = GUILayout.Toggle(Sett.useNewLadderFrame, new GUIContent("Use New Ladder Animation", "Animation when you climb/are on a ladder."));
            Sett.fixPushingAnimation = GUILayout.Toggle(Sett.fixPushingAnimation, new GUIContent("Fix Pushing Animation", "Fix animation bug of some bro."));
            Sett.brocheteAlternateSpecialAnim = GUILayout.Toggle(Sett.brocheteAlternateSpecialAnim, new GUIContent("Brochete Alternate Special Animation", "Change the Special Animation."));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            BeginVerticalBox("- Double Bro Seven :");
            GUILayout.BeginHorizontal();
            Sett.useFifthBondSpecial = GUILayout.Toggle(Sett.useFifthBondSpecial, new GUIContent("Use the 5th special of 007", "This is a Tear Gas"));
            Sett.teargasAtFeet = GUILayout.Toggle(Sett.teargasAtFeet, new GUIContent("Teargas at feet", "If you are ducking you throw the teargas at your feet."));
            Sett.lessAccurateDrunkSeven = GUILayout.Toggle(Sett.lessAccurateDrunkSeven, new GUIContent($"I'm {SecretText("drunk")}", $"007 is less accurate when {SecretText("drunk")}."));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            BeginVerticalBox("- Global Fix :");
            GUILayout.BeginHorizontal();
            Sett.tbagEnabled = GUILayout.Toggle(Sett.tbagEnabled, new GUIContent("Enabled T-Bag", "You have a good taste."));
            Sett.fixExpendabros = GUILayout.Toggle(Sett.fixExpendabros, new GUIContent("Fix Expendabros", "Some tweaks change for Expendabros."));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            BeginVerticalBox("- Hero Unlock Controller :");
            GUILayout.BeginHorizontal();
            Sett.changeHeroUnlock = GUILayout.Toggle(Sett.changeHeroUnlock, new GUIContent("Use custom HeroUnlock of this mod", "If you have active compatibility of a mod which change the Unlock Intervals, Tweaks From Pigs will do nothing."));
            Sett.spawnWithExpendabros = GUILayout.Toggle(Sett.spawnWithExpendabros, new GUIContent("Spawn With Expendabros"));
            if (Main.cheat)
            {
                Sett.spawnWithBrondleFly = SecretToggle(Sett.spawnWithBrondleFly, new GUIContent("Spawn With Brondle Fly"));
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            BeginVerticalBox("- Pocketed Special :");
            GUILayout.BeginHorizontal();
            Sett.rememberPockettedSpecial = GUILayout.Toggle(Sett.rememberPockettedSpecial, new GUIContent("Remember Pocketed Special", "When alive, if you change your bro, you keep pocketed special. (Work with Swap Bros Mod)"));
            Sett.steroidsThrowEveryone = GUILayout.Toggle(Sett.steroidsThrowEveryone, new GUIContent("Steroids can throw more mooks", "Bruisers and some other enemies can be thrown with steroids."));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();


            GUILayout.EndVertical();
            GUI.Label(ToolTipRect, GUI.tooltip, ToolTipStyle);
        }

        static void GUI_Doodads(GUIStyle TitleStyle, Rect ToolTipRect)
        {
            GUILayout.BeginVertical("box");

            GUILayout.BeginHorizontal();
            Sett.christmasAmmoBox = GUILayout.Toggle(Sett.christmasAmmoBox, new GUIContent("Christmas Ammo Box", ""));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUI.Label(ToolTipRect, GUI.tooltip, ToolTipStyle);
        }

        private static void GUI_Enemies(GUIStyle TitleStyle, Rect ToolTipRect)
        {
            GUILayout.BeginVertical("box");

            GUILayout.BeginHorizontal();
            Sett.fasterZombie = GUILayout.Toggle(Sett.fasterZombie, new GUIContent("Faster Zombie", "The zombie get a custom speed."));
            GUILayout.EndHorizontal();
            Sett.zombieSpeedModifier = ProbalityScrollBar(Sett.zombieSpeedModifier, " Zombie Speed modifier : ", 5);
            GUILayout.Space(15);

            GUILayout.BeginHorizontal();
            Sett.canTeargasedEveryone = GUILayout.Toggle(Sett.canTeargasedEveryone, new GUIContent("Tear Gas everyone", "Some enemies can't be stunt by tear gas."));
            Sett.customSkinned = GUILayout.Toggle(Sett.customSkinned, new GUIContent("Custom Skinned Mook", "When brodator melee a mook, the enemy have a \"unique\" skin."));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUI.Label(ToolTipRect, GUI.tooltip, ToolTipStyle);
        }

        private static void GUI_Interface(GUIStyle TitleStyle, Rect ToolTipRect)
        {
            GUILayout.BeginVertical("box");

            BeginVerticalBox("- HUD :");
            GUILayout.BeginHorizontal();
            Sett.showFacehuggerHUD = GUILayout.Toggle(Sett.showFacehuggerHUD, new GUIContent("Show FaceHugger", "Show a Face Hugger on the avatar when you have an alien on the head."));
            Sett.skeletonDeadFace = GUILayout.Toggle(Sett.skeletonDeadFace, new GUIContent("Show skeleton Dead Face", "Show the Skeleton head of Expendabros when you are dead"));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            BeginVerticalBox("- Menu :");
            GUILayout.BeginHorizontal();
            Sett.fixMaxArcadeLevel = GUILayout.Toggle(Sett.fixMaxArcadeLevel, new GUIContent("Fix Max Arcade Level", "On a different arcade campaign, fix the maximum of arcade level."));
            Sett.languageMenuEnabled = GUILayout.Toggle(Sett.languageMenuEnabled, new GUIContent("Enabled Language menu", "You can change language of the game in option menu."));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();


            GUILayout.EndVertical();
            GUI.Label(ToolTipRect, GUI.tooltip, ToolTipStyle);
        }

        private static void GUI_Map(GUIStyle TitleStyle, Rect ToolTipRect)
        {
            GUILayout.BeginVertical("box");

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Arcade Campaign :", "Play other arcade campaign. Work Online."));
            Sett.arcadeIndex = RocketLib.RGUI.ArrowList(Main.arcadeCampaigns, Sett.arcadeIndex, 200);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(15);
            GUILayout.BeginHorizontal();
            Sett.useAcidBarrel = GUILayout.Toggle(Sett.useAcidBarrel, new GUIContent("Enabled Acid barrel to spawn", "They are not used in Campaign, makes the spawn."));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            Sett.acidBarrelSpawnProbability = ProbalityScrollBar(Sett.acidBarrelSpawnProbability, "Acid Barrel spawn probability : ");

            GUILayout.EndVertical();
            GUI.Label(ToolTipRect, GUI.tooltip, ToolTipStyle);
        }

        private static void GUI_Other(GUIStyle TitleStyle, Rect ToolTipRect)
        {
            GUILayout.BeginVertical("box");

            GUILayout.Space(10);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            Sett.setCustomMouse = GUILayout.Toggle(Sett.setCustomMouse, new GUIContent("Cursor from level Editor", "Why ? Why not."));
            Sett.mechDropDoesSmoke = GUILayout.Toggle(Sett.mechDropDoesSmoke, new GUIContent("Mech drop smoke", "Grenade of mech drop does smoke like Tank Bro grenade."));
            Sett.fixTakeScreenshots = GUILayout.Toggle(Sett.fixTakeScreenshots, new GUIContent("Fix screenshots", "If you press F9 in game you crash, now not anymore."));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(20);
            DangerZoneGUI(ToolTipRect);

            GUILayout.EndVertical();
            GUI.Label(ToolTipRect, GUI.tooltip, ToolTipStyle);
        }

        private static void DangerZoneGUI(Rect ToolTipRect)
        {
            var BoxDangerStyle = new GUIStyle("box");
            BoxDangerStyle.normal.background = TFP_Utility.MakeTex(2, 2, new Color(0.5f, 0f, 0f, 0.5f));

            var RedTxt = new GUIStyle();
            RedTxt.normal.textColor = Color.red;


            GUILayout.Label("DANGER ZONE", GUILayout.ExpandWidth(false));
            GUILayout.BeginHorizontal(BoxDangerStyle);
            SaveSlotToDelete = (int)ProbalityScrollBar(SaveSlotToDelete, "Save slot to delete : ");
            if (GUILayout.Button(new GUIContent("DELETE", "BE CAREFUL")))
            {
                PlayerProgress.Instance.saveSlots[SaveSlotToDelete - 1] = null;
                PlayerProgress.Save(true);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUI.Label(ToolTipRect, GUI.tooltip, ToolTipStyle);
        }

        static float ProbalityScrollBar(float value, string Text, float maxValue=1f, float minValue=0.0f)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(Text, GUILayout.ExpandWidth(false));
            GUILayout.Label(value.ToString(), GUILayout.Width(70));
            value = (float)GUILayout.HorizontalScrollbar(value, 0f, minValue, maxValue, GUILayout.MaxWidth(500));
            GUILayout.EndHorizontal();
            return value;
        }

        static bool SecretToggle(bool value, GUIContent content)
        {
            GUIContent newContent = new GUIContent(SecretText(content.text), content.image, content.tooltip);
            return GUILayout.Toggle(value, newContent);
        }

        static void BeginVerticalBox(string Name)
        {
            GUILayout.Space(15);
            GUILayout.Label(Name, TitleStyle);
            GUILayout.BeginVertical("box");
        }

        static string SecretText(string source)
        {
            return "<color=\"yellow\">" + source + "</color>";
        }
    }
}
