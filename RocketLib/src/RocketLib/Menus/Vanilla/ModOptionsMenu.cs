using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RocketLib.Menus.Core;
using UnityEngine;

namespace RocketLib.Menus.Vanilla
{
    /// <summary>
    /// Centralized mod options menu that dynamically shows items registered with ModOptions target
    /// </summary>
    public class ModOptionsMenu : BaseCustomMenu
    {
        public override string MenuTitle => "MOD OPTIONS";

        private static bool isRegistered = false;

        /// <summary>
        /// Register the ModOptionsMenu in main menu and pause menu
        /// Called by RocketLib initialization
        /// </summary>
        internal static void Initialize()
        {
            if (!isRegistered)
            {
                // Register in main menu with visibility check
                MenuRegistry.RegisterMenu<ModOptionsMenu>(
                    displayText: "MOD OPTIONS",
                    targetMenu: TargetMenu.MainMenu,
                    position: PositionMode.After,
                    positionReference: "OPTIONS",
                    isVisible: (menu) => HasRegisteredModOptions()
                );

                // Register in pause menu with visibility check
                MenuRegistry.RegisterMenu<ModOptionsMenu>(
                    displayText: "MOD OPTIONS",
                    targetMenu: TargetMenu.PauseMenu,
                    position: PositionMode.After,
                    positionReference: "OPTIONS",
                    isVisible: (menu) => HasRegisteredModOptions()
                );

                isRegistered = true;
            }
        }

        /// <summary>
        /// Check if any mods have registered options
        /// </summary>
        private static bool HasRegisteredModOptions()
        {
            var modMenus = MenuRegistry.GetMenusForTarget(TargetMenu.ModOptions);
            return modMenus != null && modMenus.Any();
        }

        /// <summary>
        /// Dynamically builds menu items from registered mods
        /// </summary>
        protected override void SetupMenuItems()
        {
            var itemList = new List<MenuBarItem>();

            // Query all items registered with ModOptions target
            var modMenus = MenuRegistry.GetMenusForTarget(TargetMenu.ModOptions)
                .OrderBy(m => m.Priority)
                .ThenBy(m => m.DisplayText)
                .ToList();

            foreach (var registration in modMenus)
            {
                // Create menu item for this mod
                var item = new MenuBarItem
                {
                    name = registration.DisplayText.ToUpper(),
                    size = GetParentFontSize(),
                    color = Color.white,
                    invokeMethod = $"OpenMod_{registration.MenuId}"
                };

                itemList.Add(item);

                // Register the action for this menu item
                var capturedReg = registration; // Capture for closure
                RegisterMenuAction(item.invokeMethod, () => capturedReg.Open(this));
            }

            // Add back button
            itemList.Add(new MenuBarItem
            {
                name = "BACK",
                invokeMethod = "OnMenuClosed",
                size = GetParentFontSize(),
                color = Color.white
            });

            this.masterItems = itemList.ToArray();
        }

        /// <summary>
        /// Get the appropriate font size based on parent menu
        /// </summary>
        private float GetParentFontSize()
        {
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
            return itemSize;
        }

        /// <summary>
        /// Register an action for a menu item
        /// </summary>
        private void RegisterMenuAction(string methodName, Action action)
        {
            // Store action in a dictionary or use reflection to create dynamic methods
            // For simplicity, we'll use a static dictionary
            if (!menuActions.ContainsKey(this))
            {
                menuActions[this] = new Dictionary<string, Action>();
            }
            menuActions[this][methodName] = action;
        }

        // Static storage for menu actions
        private static readonly Dictionary<BaseCustomMenu, Dictionary<string, Action>> menuActions =
            new Dictionary<BaseCustomMenu, Dictionary<string, Action>>();

        protected override void RunInput()
        {
            if (!activatedThisFrame && accept && !Menu.acceptPrev
                && highlightIndex < masterItems.Length
                && masterItems[highlightIndex].invokeMethod.StartsWith("OpenMod_"))
            {
                string methodName = masterItems[highlightIndex].invokeMethod;
                if (menuActions.TryGetValue(this, out var actions) && actions.TryGetValue(methodName, out var action))
                {
                    action?.Invoke();
                    PlayDrumSound(1);
                    return;
                }
            }

            base.RunInput();
        }

        /// <summary>
        /// Clean up when menu is destroyed
        /// </summary>
        protected override void OnDestroy()
        {
            if (menuActions.ContainsKey(this))
            {
                menuActions.Remove(this);
            }
            base.OnDestroy();
        }
    }
}
