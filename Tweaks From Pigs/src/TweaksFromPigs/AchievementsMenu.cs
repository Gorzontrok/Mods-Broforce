using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using HarmonyLib;
using UnityEngine;
using Steamworks;
using Localisation;

namespace TweaksFromPigs
{
    public class AchievementsMenu : Menu
    {
        private CSteamID steamID;
        private List<SteamAchievement> AchievementsList = new List<SteamAchievement>();
        private Dictionary<SteamAchievement, bool> AchievementsStates = new Dictionary<SteamAchievement, bool>();

        private MainMenu mainMenu = MainMenu.instance;

        private Transform transform1;

        public AchievementsMenu(MainMenu _mainMenu, Transform _transform)
        {
            new GameObject(typeof(AchievementsMenu).FullName, typeof(AchievementsMenu));

            this.mainMenu = _mainMenu;

            this.transform1 = _transform;

            steamID = SteamUser.GetSteamID();
            AchievementsList.Add(SteamAchievement.finish_campaign);
            AchievementsList.Add(SteamAchievement.finish_campaign_hard);
            AchievementsList.Add(SteamAchievement.finish_hardcore);
            AchievementsList.Add(SteamAchievement.finish_hardcore_hard);
            AchievementsList.Add(SteamAchievement.campaign_no_deaths);
            AchievementsList.Add(SteamAchievement.hot_brotato);
            AchievementsList.Add(SteamAchievement.bronald_bradman);
            AchievementsList.Add(SteamAchievement.martini);
            AchievementsList.Add(SteamAchievement.turkey_dinner);
            AchievementsList.Add(SteamAchievement.rest_in_pieces);
            AchievementsList.Add(SteamAchievement.throw_satan);
            AchievementsList.Add(SteamAchievement.broadway);
            AchievementsList.Add(SteamAchievement.noticket);
            AchievementsList.Add(SteamAchievement.gunshow);
            AchievementsList.Add(SteamAchievement.yourefired);
            AchievementsList.Add(SteamAchievement.illbeback);
            AchievementsList.Add(SteamAchievement.stevenseagull);


            this.textPrefab = this.mainMenu.textPrefab;
            this.menuHighlight = this.mainMenu.menuHighlight;

            this.initialVerticalOffset = 0.0f;
            this.verticalSpacing = 16.0f;
            this.verticalSpacingCompressed = 15.0f;

            this.overrideIndivdualItemCharacterSizes = true;

            this.characterSizes = 4.0f;
            this.lineSpacing = 0.4f;
            this.deselectedTextScale = 1.0f;

            this.fadeItems = true;
            this.fadeDistance = 0;
            this.hideDistance = -1;

            this.menuHolder = this.mainMenu.menuHolder;

            this.PrevMenu = this.mainMenu;

            this.drumSounds = this.mainMenu.drumSounds;

            this.PrevMenu = _mainMenu;


            this.Awake();
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void SetupItems()
        {
            base.SetupItems();
            List<MenuBarItem> list = new List<MenuBarItem>();
            list.Insert(0, new MenuBarItem
            {
                name = "BACK",
                localisedKey = "MENU_BACK",
                invokeMethod = "GoBackToMainMenu",
                size = 16,
                color = Color.white
            });
            foreach (SteamAchievement achievement in AchievementsList)
            {
                list.Insert(0, new MenuBarItem
                {
                    color = Color.white,
                    size = this.characterSizes,
                    name = achievement.ToString(),
                    invokeMethod = "Nothing"
                });

            }
            this.masterItems = list.ToArray();
        }

        private void GoBackToMainMenu()
        {
            this.MenuActive = false;
            this.mainMenu.MenuActive = true;
            this.mainMenu.TransitionIn();
        }
        public override void InstantiateItems()
        {
            base.DestroyItems();
            this.SetupItems();
            this.items = new MenuBarItemUI[this.masterItems.Length];
            this.itemEnabled = new bool[this.masterItems.Length];
            for (int i = 0; i < this.masterItems.Length; i++)
            {
                this.items[i] = UnityEngine.Object.Instantiate<MenuBarItemUI>(this.textPrefab);
                LocalisedString component = this.items[i].GetComponent<LocalisedString>();
                if (component != null)
                {
                    string localisedKey = this.masterItems[i].localisedKey;
                    if (!string.IsNullOrEmpty(localisedKey))
                    {
                        component.SetKey(localisedKey);
                        this.items[i].LocalisedKey = localisedKey;
                        this.items[i].text = component.StringValue;
                    }
                    else
                    {
                        component.enabled = !component.SetTextValue;
                        this.items[i].text = this.masterItems[i].name;
                    }
                }
                else
                {
                    this.items[i].text = this.masterItems[i].name;
                }


                this.items[i].Id = this.masterItems[i].name;

                this.items[i].transform.SetParent(this.transform1);
                this.items[i].transform.localPosition = Vector3.down * this.initialVerticalOffset + Vector3.down * this.verticalSpacing * (float)i;

                this.items[i].characterSize = this.masterItems[i].size;
                this.items[i].lineSpacing = this.lineSpacing;
                base.SetMenuItemColor(i, this.masterItems[i].color);
                this.itemEnabled[i] = true;

                TextMesh backdropText = this.items[i].BackdropText;
                backdropText.lineSpacing = this.lineSpacing;
                backdropText.transform.localPosition = new Vector3(0f, -3f, 25f);
                backdropText.characterSize = this.masterItems[i].size;
                if (backdropText.GetComponent<Renderer>().material.HasProperty("_TintColor"))
                {
                    backdropText.GetComponent<Renderer>().material.SetColor("_TintColor", Color.black);
                }
            }
        }

        private void Nothing()
        { }
    }
}
