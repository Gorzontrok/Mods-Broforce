using System;
using UnityEngine;

namespace RocketLib.Menus.Core
{
    /// <summary>
    /// Target menus where menu items can be injected
    /// </summary>
    public enum TargetMenu
    {
        MainMenu,           // Game's main menu
        PauseMenu,          // In-game pause menu
        OptionsMenu,        // Options submenu (from MainMenu)
        InGameOptionsMenu,  // Options submenu (from PauseMenu)
        ModOptions          // Mod options submenu
    }

    /// <summary>
    /// Type of menu being registered
    /// </summary>
    public enum MenuKind
    {
        BaseCustom,    // Vanilla-style menu
        Flex,          // FlexMenu with layout system
        Action         // Custom action (no menu, just runs code)
    }

    /// <summary>
    /// Positioning modes for menu items
    /// </summary>
    public enum PositionMode
    {
        Auto,        // Find best position automatically
        Before,      // Insert before the reference item
        After,       // Insert after the reference item
        End          // Add at the end
    }

    /// <summary>
    /// Unified menu registration that handles both menu instance and menu item
    /// </summary>
    public class MenuRegistration
    {
        // Required constructor - ensures minimum required fields
        public MenuRegistration(string displayText, TargetMenu targetMenu)
        {
            if (string.IsNullOrEmpty(displayText))
                throw new ArgumentNullException(nameof(displayText));

            DisplayText = displayText;
            TargetMenu = targetMenu;

            // Auto-generate ID (internal use only)
            MenuId = Guid.NewGuid().ToString();

            // Defaults
            Position = PositionMode.Auto;
            Priority = 100;
        }

        // Core identification (MenuId is internal only)
        internal string MenuId { get; }
        public string DisplayText { get; }
        public TargetMenu TargetMenu { get; }

        // Menu type information
        public MenuKind Kind { get; set; }
        public Type MenuType { get; set; }

        // Instance storage (only for BaseCustomMenu)
        public Vanilla.BaseCustomMenu CustomInstance { get; set; }
        // Note: FlexMenu doesn't support instances - always created via type

        // Action support (only for Action kind)
        public Action<Menu> OnSelect { get; set; }

        // Menu item placement
        public PositionMode Position { get; set; }      // Ignored for ModOptions
        public string PositionReference { get; set; }   // Ignored for ModOptions
        public int Priority { get; set; }               // Primary for ModOptions, secondary for others

        // Visibility control
        public Func<Menu, bool> IsVisible { get; set; }

        /// <summary>
        /// Opens the menu or executes the action
        /// </summary>
        public void Open(Menu parentMenu)
        {
            if (Kind == MenuKind.BaseCustom)
            {
                OpenCustomMenu(parentMenu);
            }
            else if (Kind == MenuKind.Flex)
            {
                // FlexMenu always uses type-based creation
                // Use reflection to call the generic Show<T> method
                var showMethod = typeof(FlexMenu).GetMethod("Show", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                var genericMethod = showMethod.MakeGenericMethod(MenuType);
                genericMethod.Invoke(null, new object[] { null, parentMenu, "default" });
            }
            else if (Kind == MenuKind.Action)
            {
                // Just run the action, no menu to open
                OnSelect?.Invoke(parentMenu);
            }
        }

        private void OpenCustomMenu(Menu parentMenu)
        {
            Vanilla.BaseCustomMenu menuToOpen = null;

            if (CustomInstance != null)
            {
                // Use the pre-configured instance
                menuToOpen = CustomInstance;

                // Ensure it has a GameObject
                if (menuToOpen.gameObject == null)
                {
                    var menuGO = new GameObject($"RocketLib_Menu_{MenuId}");
                    menuGO.transform.SetParent(parentMenu.transform.parent);
                    menuGO.transform.position = parentMenu.transform.position;
                    menuGO.SetActive(false);

                    // Move the component to the new GameObject
                    // This is complex, so for new instances we'll create fresh
                    menuToOpen = menuGO.AddComponent(MenuType) as Vanilla.BaseCustomMenu;
                }
            }
            else if (MenuType != null)
            {
                // Create a new instance from type
                var menuGO = new GameObject($"RocketLib_Menu_{MenuId}");
                menuGO.transform.SetParent(parentMenu.transform.parent);
                menuGO.transform.position = parentMenu.transform.position;
                menuGO.SetActive(false);

                menuToOpen = menuGO.AddComponent(MenuType) as Vanilla.BaseCustomMenu;
            }

            if (menuToOpen != null)
            {
                // Initialize and show the menu
                menuToOpen.Initialize(parentMenu);
                parentMenu.MenuActive = false;
                menuToOpen.gameObject.SetActive(true);
                menuToOpen.MenuActive = true;
                menuToOpen.TransitionIn();
            }
            else
            {
                Main.logger.Error($"Failed to open custom menu: {DisplayText}");
            }
        }
    }
}