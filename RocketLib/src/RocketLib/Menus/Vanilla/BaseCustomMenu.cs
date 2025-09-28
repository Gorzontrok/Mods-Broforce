using System;
using System.Collections.Generic;
using HarmonyLib;
using RocketLib.Menus.Utilities;
using UnityEngine;

namespace RocketLib.Menus.Vanilla
{
    /// <summary>
    /// Abstract base class for all custom menus in RocketLib.
    /// Provides core functionality for menu creation, item management, and scene persistence.
    /// </summary>
    public abstract class BaseCustomMenu : Menu
    {
        /// <summary>
        /// Unique identifier for this menu
        /// </summary>
        public abstract string MenuId { get; }

        /// <summary>
        /// Display title for this menu
        /// </summary>
        public abstract string MenuTitle { get; }

        /// <summary>
        /// Vertical spacing between menu items
        /// </summary>
        protected virtual float ItemSpacing => 26f;

        /// <summary>
        /// Compressed vertical spacing (used when moveHighlight is false)
        /// </summary>
        protected virtual float ItemSpacingCompressed => 13.5f;

        /// <summary>
        /// Initial vertical offset for menu items
        /// </summary>
        protected virtual float InitialVerticalOffset => 107f;

        /// <summary>
        /// Initialize the menu with references from parent menu
        /// </summary>
        /// <param name="parent">The parent menu to copy references from</param>
        public virtual void Initialize(Menu parent)
        {
            PrevMenu = parent;

            UnityEngine.Object.DontDestroyOnLoad(this);
        }

        /// <summary>
        /// Set up the menu items for this menu.
        /// Override this to define your menu's items.
        /// </summary>
        protected abstract void SetupMenuItems();

        /// <summary>
        /// Unity Awake - set up the menu when GameObject becomes active
        /// </summary>
        protected override void Awake()
        {
            if (PrevMenu == null)
            {
                PrevMenu = FindParentMenuInScene();
            }

            if (PrevMenu != null)
            {
                CopyReferencesFromParent();
            }

            SetupMenuItems();

            this.desktopItems = this.masterItems;

            base.Awake();
        }

        private void CopyReferencesFromParent()
        {
            if (PrevMenu.textPrefab != null)
            {
                this.textPrefab = PrevMenu.textPrefab;
            }

            if (PrevMenu.drumSounds != null)
            {
                this.drumSounds = PrevMenu.drumSounds;
            }

            // Create our own highlight instead of sharing with parent
            try
            {
                CreateCustomHighlight();
            }
            catch (Exception ex)
            {
                Main.logger.Log("error: " + ex.ToString());
            }


            // Inherit spacing values from parent menu instead of using hardcoded values
            this.verticalSpacing = PrevMenu.verticalSpacing;
            this.verticalSpacingCompressed = PrevMenu.verticalSpacingCompressed;
            this.initialVerticalOffset = PrevMenu.initialVerticalOffset;

            this.fadeItems = true;
            this.fadeDistance = 0;
            this.hideDistance = -1;
            this.moveHighlight = false;

            // Inherit font sizing from parent menu
            Traverse parentTraverse = Traverse.Create(PrevMenu);
            this.overrideIndivdualItemCharacterSizes = parentTraverse.Field<bool>("overrideIndivdualItemCharacterSizes").Value;
            this.characterSizes = parentTraverse.Field<float>("characterSizes").Value;
            this.lineSpacing = parentTraverse.Field<float>("lineSpacing").Value;
            this.deselectedTextScale = parentTraverse.Field<float>("deselectedTextScale").Value;
        }

        /// <summary>
        /// Create a fresh highlight GameObject from scratch using our own resources
        /// </summary>
        private void CreateCustomHighlight()
        {
            // Use the new HighlightFactory to create highlight
            this.menuHighlight = HighlightFactory.CreateHighlight(this.transform, this.gameObject.layer);
        }

        /// <summary>
        /// Called when the menu is opened
        /// </summary>
        public virtual void OnMenuOpened()
        {
            // Ensure highlight is visible when menu opens
            if (this.menuHighlight != null && this.menuHighlight.gameObject != null)
            {
                this.menuHighlight.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Called when the menu is closed
        /// </summary>
        public virtual void OnMenuClosed()
        {
            if (PrevMenu != null)
            {
                this.MenuActive = false;
                this.PrevMenu.MenuActive = true;
                this.PrevMenu.TransitionIn();
            }
            else
            {
                this.MenuActive = false;
            }
        }

        /// <summary>
        /// Find the parent menu in the current scene
        /// </summary>
        /// <returns>The parent menu if found, null otherwise</returns>
        protected virtual Menu FindParentMenuInScene()
        {
            var mainMenu = GameObject.FindObjectOfType<MainMenu>();
            if (mainMenu != null) return mainMenu;

            var pauseMenu = GameObject.FindObjectOfType<PauseMenu>();
            if (pauseMenu != null) return pauseMenu;

            var optionsMenu = GameObject.FindObjectOfType<OptionsMenu>();
            if (optionsMenu != null) return optionsMenu;

            return null;
        }

        /// <summary>
        /// Add a menu item to this menu
        /// </summary>
        /// <param name="displayText">Display text for the item</param>
        /// <param name="methodName">Method to invoke when selected</param>
        /// <param name="isToggle">Whether this item accepts left/right input</param>
        protected void AddMenuItem(string displayText, string methodName, bool isToggle = false)
        {
            // Use parent menu's font size if available, otherwise use appropriate default
            float itemSize = 3f; // Default MenuBarItem size
            if (PrevMenu != null)
            {
                // Try to get size from parent's first item
                Traverse parentTraverse = Traverse.Create(PrevMenu);
                var parentItems = parentTraverse.Field<MenuBarItem[]>("masterItems").Value;
                if (parentItems != null && parentItems.Length > 0)
                {
                    itemSize = parentItems[0].size;
                }
            }
            else if (PrevMenu is MainMenu)
            {
                itemSize = 6f; // MainMenu typically uses larger size
            }

            var item = new MenuBarItem
            {
                name = displayText,
                invokeMethod = methodName,
                size = itemSize,
                color = Color.white
            };

            if (isToggle)
            {
                item.invokeMethod = $"Toggle{methodName}";
            }

            if (this.masterItems == null)
            {
                this.masterItems = new MenuBarItem[] { item };
            }
            else
            {
                var list = new List<MenuBarItem>(this.masterItems);
                list.Add(item);
                this.masterItems = list.ToArray();
            }
        }

        /// <summary>
        /// Update the visual representation of menu items
        /// </summary>
        protected virtual void UpdateMenuItemVisuals()
        {
        }

        /// <summary>
        /// Override Update to handle Escape key like OptionsMenu does
        /// </summary>
        protected override void Update()
        {
            base.Update();

            if (this.menuActive && !this.activatedThisFrame &&
                (Input.GetKeyDown(KeyCode.Escape) || InputReader.GetControllerPressingCancel() > -1))
            {
                OnMenuClosed();
            }
        }

        /// <summary>
        /// Clean up when menu is destroyed
        /// </summary>
        protected override void OnDestroy()
        {
            PrevMenu = null;
        }
    }
}
