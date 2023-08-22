using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TerroristC4Programs
{
    internal static class ModUI
    {
        public static Settings Sett
        {
            get
            {
                return Main.settings;
            }
        }


        public static readonly string[] tabs;
        public static readonly Action[] actions;

        private static int _selectedTab = 0;
        private static Rect _toolTip;

        static ModUI()
        {
            tabs = new string[] { "Global", "Troopers", "Bruisers", "Suicides" };
            actions = new Action[] { Global, Troopers, Bruisers, Suicides };
        }

        public static void MainUI()
        {
            _selectedTab = GUILayout.SelectionGrid(_selectedTab, tabs, 8, GUILayout.ExpandWidth(false));
            _toolTip = GUILayoutUtility.GetLastRect();
            GUILayout.Space(10);
            Sett.patchInCustomsLevel = GUILayout.Toggle(Sett.patchInCustomsLevel, new GUIContent("Patch in custom levels", "Do the changes are applied in Customs Level and Editor (use at your own risk)"));

            GUILayout.BeginVertical("box");
            actions[_selectedTab].Invoke();
            GUILayout.EndVertical();

            GUI.Label(_toolTip, GUI.tooltip);
        }

        private static void Global()
        {
            Sett.zombiesDanceOnFlex = GUILayout.Toggle(Sett.zombiesDanceOnFlex, new GUIContent("Zombies dance on Flex", "Zombies will dance if the revive source is flexing"));
            Sett.betterSkinlessSprite = GUILayout.Toggle(Sett.betterSkinlessSprite, new GUIContent("Better skinless sprite", ""));
        }

        private static void Troopers()
        {
            Sett.strongerTrooperProbability.ToGui(new GUIContent("Stronger Troopers"));
        }
        private static void Bruisers()
        {
            Sett.strongerBruiserProbability.ToGui(new GUIContent("Stronger Bruisers"));
            Sett.eliteBruiserProbability.ToGui(new GUIContent("Elites Bruisers"));
        }
        private static void Suicides()
        {
            Sett.suicideGetBigger.ToGui(new GUIContent("Suicides Get Bigger"));
        }
    }
}
