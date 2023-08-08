 using System;
using UnityEngine;

namespace TheGeneralsTraining
{
    internal static class GUI_UMM
    {
        private static GUIStyle toolTipStyle = new GUIStyle();
        private static GUIStyle titleStyle = new GUIStyle();
        private static int TabSelected;
        private static string[] tabs;
        private static Action[] actions;
        private static int playerNum;

        private static Settings Sett
        {
            get
            {
                return Main.settings;
            }
        }

        public static void Init()
        {
            toolTipStyle = new GUIStyle
            {
                fontStyle = FontStyle.Bold,
                fontSize = 15
            };
            toolTipStyle.normal.textColor = Color.white;

            titleStyle = new GUIStyle
            {
                fontStyle = FontStyle.Bold,
                fontSize = 13
            };
            titleStyle.normal.textColor = Color.white;


            tabs = new string[] { "Global", "Brade", "Brochete", "BroDredd", "BroHard", "BroHeart", "Buffy", "Casey Broback", "Chev Brolios", "Desperabro", "Dirty Brorry", "Double Bro Seven", "Rambro", "Scorpion Bro", "SethBrondle", "Xena" };
            actions = new Action[] { Global, Brade, Brochete, BroDredd, BroHard, BroHeart, Buffy, CaseyBroback, ChevBrolios, Desperabro, DirtyHarry, DoubleBroSeven, Rambro, ScorpionBro, SethBrondle, Xena };
        }

        public static void OnGUI()
        {
            TabSelected = GUILayout.SelectionGrid(TabSelected, tabs, 8);

            GUILayout.Space(15);
            Rect toolTipRect = GUILayoutUtility.GetLastRect();
            GUILayout.Space(10);
            Sett.patchInCustomsLevel = GUILayout.Toggle(Sett.patchInCustomsLevel, new GUIContent("Patch in custom levels", "Do the changes are applied in Customs Level and Editor (use at your own risk)"));
            GUILayout.Space(5);


            GUILayout.BeginVertical("box");
            actions[TabSelected].Invoke();
            GUILayout.EndVertical();
            GUI.Label(toolTipRect, GUI.tooltip, toolTipStyle);
        }

        private static void Global()
        {
            Sett.strongerThrow = GUILayout.Toggle(Sett.strongerThrow, new GUIContent("Stronger throw", "Depends of the bro and it's state"));
            Sett.faceHugger = GUILayout.Toggle(Sett.faceHugger, new GUIContent("Face Hugger", "Show a face hugger on the hud if you have one on you."));
            Sett.electricThrow = GUILayout.Toggle(Sett.electricThrow, new GUIContent("Electric Throw ", "Broden and The Brolander (if ammo > 2) make mook electric when throw"));
            Sett.rememberPockettedSpecial = GUILayout.Toggle(Sett.rememberPockettedSpecial, new GUIContent("Remember Pocketed Special", "When you swap your bro alive, you keep the special ammo (Time Slow, AirStrike, Remote Control Car, Mech Drop, ect.)"));
            Sett.ladderAnimation = GUILayout.Toggle(Sett.ladderAnimation, new GUIContent("Ladder Animation", "Animation of climbing a ladder"));
            Sett.pushAnimation = GUILayout.Toggle(Sett.pushAnimation, new GUIContent("Push Animation", "Animation of pushing a box"));
            Sett.grenadeExplodeIfNotVisible = GUILayout.Toggle(Sett.grenadeExplodeIfNotVisible, new GUIContent("Grenade explode if not visible", "The grenades explode if they are not visible by the camera."));
            Sett.goldenFlexBrosProjectile = GUILayout.Toggle(Sett.goldenFlexBrosProjectile, new GUIContent("Bro's projectile with golden light", "The grenades explode if they are not visible by the camera."));
            Sett.holyWaterReviveFlashBang = GUILayout.Toggle(Sett.holyWaterReviveFlashBang, new GUIContent("Flashbang on holy water revive", "The grenades explode if they are not visible by the camera."));
            GUILayout.FlexibleSpace();
        }

        private static void Brade()
        {
            Sett.bradeGlaive = GUILayout.Toggle(Sett.bradeGlaive, new GUIContent("Old Brade Glaive", "Put back the old brade's glaive sprite"));
            GUILayout.FlexibleSpace();
        }
        private static void Brochete()
        {
            Sett.alternateSpecialAnim = GUILayout.Toggle(Sett.alternateSpecialAnim, new GUIContent("Alternate special animation", ""));
            GUILayout.FlexibleSpace();
        }

        private static void BroDredd()
        {
            Sett.lessTazerHit = GUILayout.Toggle(Sett.lessTazerHit, new GUIContent("Less taser hit", "It tooks less hit for Bro Dredd to explode an enemy with his taser."));
            GUILayout.FlexibleSpace();
        }
        private static void BroHard()
        {
            Sett.broHardFasterWhenDucking = GUILayout.Toggle(Sett.broHardFasterWhenDucking, new GUIContent("Faster in enclosed spaces", "15% faster in enclosed spaces"));
            GUILayout.FlexibleSpace();
        }

        private static void BroHeart()
        {
            Sett.retrieveSwordInAmmo = GUILayout.Toggle(Sett.retrieveSwordInAmmo, new GUIContent("Retrieve lost sword in ammo pickups"));
            GUILayout.FlexibleSpace();
        }

        private static void Buffy()
        {
            Sett.hollywaterMookToVillager = GUILayout.Toggle(Sett.hollywaterMookToVillager, new GUIContent("Mook to villager with Holy Water", "Convert basic Mooks into villager with holy water"));
            GUILayout.FlexibleSpace();
        }

        private static void CaseyBroback()
        {
            Sett.strongerMelee = GUILayout.Toggle(Sett.strongerMelee, new GUIContent("Stronger Melee"));
            Sett.pigGrenade = GUILayout.Toggle(Sett.pigGrenade, new GUIContent("Pig Grenade", "Replace the special with a pig grenade."));
            GUILayout.FlexibleSpace();
        }
        private static void ChevBrolios()
        {
            Sett.carBattery = GUILayout.Toggle(Sett.carBattery, new GUIContent("Car Battery", "During adrenaline, press the special button again to use up one of his lives to infuse chev with fire and electricity"));
            Sett.noRecoil = GUILayout.Toggle(Sett.noRecoil, new GUIContent("No Recoil", "No recoil on shoot"));
            GUILayout.FlexibleSpace();
        }

        private static void Desperabro()
        {
            Sett.mariachisNoLongerStealLivesFromDesperabro = GUILayout.Toggle(Sett.mariachisNoLongerStealLivesFromDesperabro, new GUIContent("Mariachis has no lives", "Mariachis no longer steal lives from Desperabro"));
            //Sett.mariachisPlayMusic = GUILayout.Toggle(Sett.mariachisPlayMusic, new GUIContent("Mariachis Play Music", "When they have no enemies to shoot at, they play music."));
            GUILayout.FlexibleSpace();
        }
        private static void DirtyHarry()
        {
            Sett.reloadOnPunch = GUILayout.Toggle(Sett.reloadOnPunch, new GUIContent("Reload on punch"));
            GUILayout.FlexibleSpace();
        }

        private static void DoubleBroSeven()
        {
            Sett.fifthBondSpecial = GUILayout.Toggle(Sett.fifthBondSpecial, new GUIContent("Fifth Bond's Special", "Add the fifth special of 007, a tear gas"));
            Sett.drunkSeven = GUILayout.Toggle(Sett.drunkSeven, new GUIContent("Don't drink", "007 is less accurate when he has drink more than 3 cocktails"));
            GUILayout.FlexibleSpace();
        }

        private static void Rambro()
        {
            playerNum = (int)GUILayout.HorizontalSlider(playerNum, 0, 4);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Point", new GUILayoutOption[] { GUILayout.Width(100) }))
            {
                HeroController.players[playerNum].character.SetGestureAnimation(GestureElement.Gestures.Point);
            }
            if (GUILayout.Button("KneeDrop", new GUILayoutOption[] { GUILayout.Width(100) }))
            {
                HeroController.players[playerNum].character.SetGestureAnimation(GestureElement.Gestures.KneeDrop);
            }
            if (GUILayout.Button("Wave", new GUILayoutOption[] { GUILayout.Width(100) }))
            {
                HeroController.players[playerNum].character.SetGestureAnimation(GestureElement.Gestures.Wave);
            }
            if (GUILayout.Button("Salute", new GUILayoutOption[] { GUILayout.Width(100) }))
            {
                HeroController.players[playerNum].character.SetGestureAnimation(GestureElement.Gestures.Salute);
            }
            if (GUILayout.Button("Shhh", new GUILayoutOption[] { GUILayout.Width(100) }))
            {
                HeroController.players[playerNum].character.SetGestureAnimation(GestureElement.Gestures.Shhh);
            }
            if (GUILayout.Button("Thrust", new GUILayoutOption[] { GUILayout.Width(100) }))
            {
                HeroController.players[playerNum].character.SetGestureAnimation(GestureElement.Gestures.Thrust);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private static void ScorpionBro()
        {
            Sett.stealthier = GUILayout.Toggle(Sett.stealthier, new GUIContent("Better Special", "Can use melee in stealth mode and become invisible if player is ducking."));
            GUILayout.FlexibleSpace();
        }

        private static void SethBrondle()
        {
            Sett.noAcidCoverIfSpecial = GUILayout.Toggle(Sett.noAcidCoverIfSpecial, new GUIContent("No acid cover with special", "Seth Brondle will not be cover by acid if he has at least one special left."));
            GUILayout.FlexibleSpace();
        }

        private static void Xena()
        {
            Sett.betterChakram = GUILayout.Toggle(Sett.betterChakram, new GUIContent("Better chakram"));
            GUILayout.FlexibleSpace();
        }

        //Utility
        private static void StartCategory(string categoryName)
        {
            GUILayout.Space(15);
            GUILayout.Label(categoryName, titleStyle);
            GUILayout.BeginVertical("box");
        }
        private static void EndCategory()
        {
            GUILayout.EndVertical();
        }
    }
}