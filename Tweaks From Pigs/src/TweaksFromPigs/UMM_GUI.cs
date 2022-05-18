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

        }

        private static void GUI_Main(GUIStyle TitleStyle, Rect ToolTipRect)
        {
            GUILayout.BeginVertical("box");

            GUILayout.BeginHorizontal();
            Main.settings.useCustomFramerate = GUILayout.Toggle(Main.settings.useCustomFramerate, new GUIContent("Use custom Frame-rate"));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                GUILayout.Label("Max Frame-rate :", GUILayout.ExpandWidth(false));
                int.TryParse(GUILayout.TextField(Main.settings.maxFramerate.ToString(), GUILayout.Width(100)), out Main.settings.maxFramerate);
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("Player Name :", GUILayout.ExpandWidth(false));
            Main.settings.customPlayerName = GUILayout.TextField(Main.settings.customPlayerName, GUILayout.MinWidth(100));
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUI.Label(ToolTipRect, GUI.tooltip, ToolTipStyle);
        }

        private static void GUI_Animal(GUIStyle TitleStyle, Rect ToolTipRect)
        {
            GUILayout.BeginVertical("box");

            BeginVerticalBox($"- {SecretText("Pigs")} :");
            GUILayout.BeginHorizontal();
            Main.settings.pigAreAlwaysTerror = GUILayout.Toggle(Main.settings.pigAreAlwaysTerror, new GUIContent($"Terrorist {SecretText("Pigs")}", $"Terrorist {SecretText("Pigs")} are the best <3"));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.EndVertical();
        }
        private static void GUI_Bros(GUIStyle TitleStyle, Rect ToolTipRect)
        {
            GUILayout.BeginVertical("box");

            GUILayout.BeginHorizontal();
            Main.settings.fixIndianaAchievement = GUILayout.Toggle(Main.settings.fixIndianaAchievement, new GUIContent("Fix Front Row Ticket", "Fix the achievement for unlock it."));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            BeginVerticalBox("- Animation :");
            GUILayout.BeginHorizontal();
            Main.settings.usePushingFrame = GUILayout.Toggle(Main.settings.usePushingFrame, new GUIContent("Use Pushing Frame", "Animation when you push a block."));
            Main.settings.useNewLadderFrame = GUILayout.Toggle(Main.settings.useNewLadderFrame, new GUIContent("Use New Ladder Animation", "Animation when you climb/are on a ladder."));
            Main.settings.fixPushingAnimation = GUILayout.Toggle(Main.settings.fixPushingAnimation, new GUIContent("Fix Pushing Animation", "Fix animation bug of some bro."));
            Main.settings.brocheteAlternateSpecialAnim = GUILayout.Toggle(Main.settings.brocheteAlternateSpecialAnim, new GUIContent("Brochete Alternate Special Animation", "Change the Special Animation."));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            BeginVerticalBox("- Double Bro Seven :");
            GUILayout.BeginHorizontal();
            Main.settings.useFifthBondSpecial = GUILayout.Toggle(Main.settings.useFifthBondSpecial, new GUIContent("Use the 5th special of 007", "This is a Tear Gas"));
            Main.settings.teargasAtFeet = GUILayout.Toggle(Main.settings.teargasAtFeet, new GUIContent("Teargas at feet", "If you are ducking you throw the teargas at your feet."));
            Main.settings.lessAccurateDrunkSeven = GUILayout.Toggle(Main.settings.lessAccurateDrunkSeven, new GUIContent($"I'm {SecretText("drunk")}", $"007 is less accurate when {SecretText("drunk")}."));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            BeginVerticalBox("- Global Fix :");
            GUILayout.BeginHorizontal();
            Main.settings.tbagEnabled = GUILayout.Toggle(Main.settings.tbagEnabled, new GUIContent("Enabled T-Bag", "You have a good taste."));
            Main.settings.rememberPockettedSpecial = GUILayout.Toggle(Main.settings.rememberPockettedSpecial, new GUIContent("Remember Pocketed Special", "When alive, if you change your bro, you keep pocketed special. (Work with Swap Bros Mod)"));
            Main.settings.fixExpendabros = GUILayout.Toggle(Main.settings.fixExpendabros, new GUIContent("Fix Expendabros", "Some tweaks change for Expendabros."));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            BeginVerticalBox("- Hero Unlock Controller :");
            GUILayout.BeginHorizontal();
            Main.settings.changeHeroUnlock = GUILayout.Toggle(Main.settings.changeHeroUnlock, new GUIContent("Use custom HeroUnlock of this mod", "If you have active compatibility of a mod which change the Unlock Intervals, Tweaks From Pigs will do nothing."));
            Main.settings.spawnWithExpendabros = GUILayout.Toggle(Main.settings.spawnWithExpendabros, new GUIContent("Spawn With Expendabros"));
            if (Main.cheat)
            {
                Main.settings.spawnWithBrondleFly = SecretToggle(Main.settings.spawnWithBrondleFly, new GUIContent("Spawn With Brondle Fly"));
            }
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
            Main.settings.christmasAmmoBox = GUILayout.Toggle(Main.settings.christmasAmmoBox, new GUIContent("Christmas Ammo Box", ""));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUI.Label(ToolTipRect, GUI.tooltip, ToolTipStyle);
        }

        private static void GUI_Enemies(GUIStyle TitleStyle, Rect ToolTipRect)
        {
            GUILayout.BeginVertical("box");

            GUILayout.BeginHorizontal();
            Main.settings.fasterZombie = GUILayout.Toggle(Main.settings.fasterZombie, new GUIContent("Faster Zombie", "The zombie get a custom speed."));
            GUILayout.EndHorizontal();
            Main.settings.zombieSpeedModifier = ProbalityScrollBar(Main.settings.zombieSpeedModifier, " Zombie Speed modifier : ", 5);
            GUILayout.Space(15);

            GUILayout.BeginHorizontal();
            Main.settings.canTeargasedEveryone = GUILayout.Toggle(Main.settings.canTeargasedEveryone, new GUIContent("Tear Gas everyone", "Some enemies can't be stunt by tear gas."));
            Main.settings.customSkinned = SecretToggle(Main.settings.customSkinned, new GUIContent("Custom Skinned Mook", ""));
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
            Main.settings.showFacehuggerHUD = GUILayout.Toggle(Main.settings.showFacehuggerHUD, new GUIContent("Show FaceHugger", "Show a Face Hugger on the avatar when you have an alien on the head."));
            Main.settings.skeletonDeadFace = GUILayout.Toggle(Main.settings.skeletonDeadFace, new GUIContent("Show skeleton Dead Face", "Show the Skeleton head of Expendabros when you are dead"));

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            BeginVerticalBox("- Menu :");
            GUILayout.BeginHorizontal();
            Main.settings.fixMaxArcadeLevel = GUILayout.Toggle(Main.settings.fixMaxArcadeLevel, new GUIContent("Fix Max Arcade Level", "On a different arcade campaign, fix the maximum of arcade level."));
            Main.settings.languageMenuEnabled = GUILayout.Toggle(Main.settings.languageMenuEnabled, new GUIContent("Enabled Language menu", "You can change language of the game in option menu."));
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
            Main.settings.arcadeIndex = RocketLib.RGUI.ArrowList(Main.arcadeCampaigns, Main.settings.arcadeIndex, 200);
            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginHorizontal();
            Main.settings.useAcidBarrel = GUILayout.Toggle(Main.settings.useAcidBarrel, new GUIContent("Enabled Acid barrel to spawn", "They are not used in Campaign, makes the spawn."));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            Main.settings.acidBarrelSpawnProbability = ProbalityScrollBar(Main.settings.acidBarrelSpawnProbability, "Acid Barrel spawn probability : ");
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUI.Label(ToolTipRect, GUI.tooltip, ToolTipStyle);
        }

        private static void GUI_Other(GUIStyle TitleStyle, Rect ToolTipRect)
        {
            GUILayout.BeginVertical("box");

            GUILayout.Space(10);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            Main.settings.setCustomMouse = GUILayout.Toggle(Main.settings.setCustomMouse, new GUIContent("Cursor from level Editor", "Why ? Why not."));
            Main.settings.mechDropDoesSmoke = GUILayout.Toggle(Main.settings.mechDropDoesSmoke, new GUIContent("Mech drop smoke", "Grenade of mech drop does smoke like Tank Bro grenade."));
            Main.settings.fixTakeScreenshots = GUILayout.Toggle(Main.settings.fixTakeScreenshots, new GUIContent("Fix screenshots", "If you press F9 in game you crash, now not anymore."));
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
