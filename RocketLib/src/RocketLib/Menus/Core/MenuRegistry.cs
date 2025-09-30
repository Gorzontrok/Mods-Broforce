using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace RocketLib.Menus.Core
{
    /// <summary>
    /// Central registry for managing menu registrations and menu item injections
    /// </summary>
    public static class MenuRegistry
    {
        // Single storage for all registrations
        private static readonly Dictionary<string, MenuRegistration> registeredMenus = new Dictionary<string, MenuRegistration>();
        private static readonly Dictionary<string, Action<Menu>> menuActions = new Dictionary<string, Action<Menu>>();

        /// <summary>
        /// Register a custom action (no menu, just code to run)
        /// </summary>
        public static void RegisterAction(
            string displayText,
            Action<Menu> onSelect,
            TargetMenu targetMenu = TargetMenu.MainMenu,
            PositionMode position = PositionMode.Auto,
            string positionReference = null,
            int priority = 100,
            Func<Menu, bool> isVisible = null)
        {
            var registration = new MenuRegistration(displayText, targetMenu)
            {
                Kind = MenuKind.Action,
                OnSelect = onSelect,
                Position = position,
                PositionReference = positionReference,
                Priority = priority,
                IsVisible = isVisible
            };

            registeredMenus[registration.MenuId] = registration;
        }

        /// <summary>
        /// Register any menu type by class
        /// </summary>
        public static void RegisterMenu<T>(
            string displayText,
            TargetMenu targetMenu = TargetMenu.MainMenu,
            PositionMode position = PositionMode.Auto,
            string positionReference = null,
            int priority = 100,
            Func<Menu, bool> isVisible = null) where T : class, new()
        {
            var registration = new MenuRegistration(displayText, targetMenu)
            {
                MenuType = typeof(T),
                Kind = typeof(Vanilla.BaseCustomMenu).IsAssignableFrom(typeof(T)) ? MenuKind.BaseCustom : MenuKind.Flex,
                Position = position,
                PositionReference = positionReference,
                Priority = priority,
                IsVisible = isVisible
            };

            registeredMenus[registration.MenuId] = registration;
        }

        /// <summary>
        /// Register any menu type by instance
        /// </summary>
        public static void RegisterMenu(
            object menuInstance,
            string displayText,
            TargetMenu targetMenu = TargetMenu.MainMenu,
            PositionMode position = PositionMode.Auto,
            string positionReference = null,
            int priority = 100,
            Func<Menu, bool> isVisible = null)
        {
            var registration = new MenuRegistration(displayText, targetMenu)
            {
                MenuType = menuInstance.GetType(),
                Position = position,
                PositionReference = positionReference,
                Priority = priority,
                IsVisible = isVisible
            };

            // Only BaseCustomMenu supports instances
            if (menuInstance is Vanilla.BaseCustomMenu customMenu)
            {
                registration.Kind = MenuKind.BaseCustom;
                registration.CustomInstance = customMenu;
            }
            else if (menuInstance is FlexMenu)
            {
                throw new NotSupportedException("FlexMenu doesn't support instance registration. Use RegisterMenu<T>() instead.");
            }
            else
            {
                throw new ArgumentException($"Unknown menu type: {menuInstance.GetType()}");
            }

            registeredMenus[registration.MenuId] = registration;
        }

        /// <summary>
        /// Get all registrations for a specific target menu
        /// </summary>
        public static IEnumerable<MenuRegistration> GetMenusForTarget(TargetMenu targetMenu)
        {
            return registeredMenus.Values.Where(r => r.TargetMenu == targetMenu);
        }

        /// <summary>
        /// Inject registered items into a game menu
        /// </summary>
        public static void InjectMenuItems(Menu menu)
        {
            if (menu == null) return;

            TargetMenu? targetType = GetTargetMenuType(menu);
            if (!targetType.HasValue) return;

            var menuTraverse = Traverse.Create(menu);
            var masterItems = menuTraverse.Field<MenuBarItem[]>("masterItems").Value ?? new MenuBarItem[0];

            // Get items for this target menu
            var itemsToInject = registeredMenus.Values
                .Where(r => r.TargetMenu == targetType.Value)
                .Where(r => r.IsVisible == null || r.IsVisible(menu))
                .Where(r => !masterItems.Any(existing => existing.name == r.DisplayText))
                .OrderBy(r => r.Priority)
                .ThenBy(r => r.Position == PositionMode.End ? int.MaxValue : 0)
                .ToList();

            if (itemsToInject.Count == 0) return;

            var newItems = new List<MenuBarItem>(masterItems);

            foreach (var registration in itemsToInject)
            {
                // Get appropriate font size from parent menu
                float itemSize = 3f; // Default
                if (masterItems.Length > 0)
                {
                    itemSize = masterItems[0].size; // Use existing menu's font size
                }

                var menuItem = new MenuBarItem
                {
                    name = registration.DisplayText,
                    invokeMethod = $"RocketLib_{registration.MenuId}",
                    size = itemSize,
                    color = Color.white
                };

                int insertPos = DetermineInsertPosition(newItems, registration);

                if (insertPos >= 0 && insertPos < newItems.Count)
                {
                    newItems.Insert(insertPos, menuItem);
                }
                else
                {
                    newItems.Add(menuItem);
                }

                // Register the action for this menu item
                RegisterMenuAction(menu, menuItem.invokeMethod, registration);
            }

            menuTraverse.Field<MenuBarItem[]>("masterItems").Value = newItems.ToArray();
        }

        /// <summary>
        /// Determine the insertion position for a menu item based on positioning settings
        /// </summary>
        private static int DetermineInsertPosition(List<MenuBarItem> items, MenuRegistration registration)
        {
            switch (registration.Position)
            {
                case PositionMode.Before:
                    if (!string.IsNullOrEmpty(registration.PositionReference))
                    {
                        for (int i = 0; i < items.Count; i++)
                        {
                            if (items[i].name.Equals(registration.PositionReference, StringComparison.OrdinalIgnoreCase))
                            {
                                return i;
                            }
                        }
                    }
                    return -1;

                case PositionMode.After:
                    if (!string.IsNullOrEmpty(registration.PositionReference))
                    {
                        for (int i = 0; i < items.Count; i++)
                        {
                            if (items[i].name.Equals(registration.PositionReference, StringComparison.OrdinalIgnoreCase))
                            {
                                return i + 1;
                            }
                        }
                    }
                    return -1;

                case PositionMode.End:
                    return -1;

                case PositionMode.Auto:
                default:
                    if (registration.TargetMenu == TargetMenu.MainMenu)
                    {
                        for (int i = 0; i < items.Count; i++)
                        {
                            if (items[i].name.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase))
                            {
                                return i + 1;
                            }
                        }
                    }
                    return -1;
            }
        }

        /// <summary>
        /// Get the target menu type for a menu instance
        /// </summary>
        private static TargetMenu? GetTargetMenuType(Menu menu)
        {
            if (menu is MainMenu) return TargetMenu.MainMenu;
            if (menu is PauseMenu) return TargetMenu.PauseMenu;
            if (menu is OptionsMenu) return TargetMenu.OptionsMenu;
            if (menu is InGameOptionsMenu) return TargetMenu.InGameOptionsMenu;
            // ModOptions is handled separately
            return null;
        }

        /// <summary>
        /// Register an action to be invoked when a menu item is selected
        /// </summary>
        private static void RegisterMenuAction(Menu menu, string methodName, MenuRegistration registration)
        {
            var key = $"{menu.GetInstanceID()}_{methodName}";
            menuActions[key] = (m) => registration.Open(m);
        }

        /// <summary>
        /// Invoke a registered menu action
        /// </summary>
        public static bool InvokeMenuAction(Menu menu, string methodName)
        {
            var key = $"{menu.GetInstanceID()}_{methodName}";
            if (menuActions.TryGetValue(key, out var action))
            {
                action?.Invoke(menu);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Clear all registrations (useful for mod unload)
        /// </summary>
        public static void ClearAll()
        {
            registeredMenus.Clear();
            menuActions.Clear();
        }
    }
}
