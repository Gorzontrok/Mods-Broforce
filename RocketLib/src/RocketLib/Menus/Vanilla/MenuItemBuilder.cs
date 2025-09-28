using System;
using UnityEngine;

namespace RocketLib.Menus.Vanilla
{
    /// <summary>
    /// Fluent builder for creating configured menu items.
    /// Provides a clean API for constructing MenuBarItem instances with all their properties.
    /// </summary>
    public class MenuItemBuilder
    {
        private string text = "MENU ITEM";
        private string methodName = null;
        private float size = 6f;
        private Color color = Color.white;
        private Action action = null;
        private Action<bool> toggleAction = null;
        private Func<bool> getCurrentState = null;
        private object customData = null;

        /// <summary>
        /// Set the display text for this menu item.
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <returns>This builder for chaining</returns>
        public MenuItemBuilder WithText(string text)
        {
            this.text = text;
            return this;
        }

        /// <summary>
        /// Set the action to execute when selected.
        /// </summary>
        /// <param name="action">The action to execute</param>
        /// <returns>This builder for chaining</returns>
        public MenuItemBuilder WithAction(Action action)
        {
            this.action = action;
            return this;
        }

        /// <summary>
        /// Set a specific method name to invoke (for compatibility with existing menu items).
        /// </summary>
        /// <param name="methodName">The method name to invoke</param>
        /// <returns>This builder for chaining</returns>
        public MenuItemBuilder WithMethodName(string methodName)
        {
            this.methodName = methodName;
            return this;
        }

        /// <summary>
        /// Set the text size (default is 6f).
        /// </summary>
        /// <param name="size">The text size</param>
        /// <returns>This builder for chaining</returns>
        public MenuItemBuilder WithSize(float size)
        {
            this.size = size;
            return this;
        }

        /// <summary>
        /// Set the text color.
        /// </summary>
        /// <param name="color">The text color</param>
        /// <returns>This builder for chaining</returns>
        public MenuItemBuilder WithColor(Color color)
        {
            this.color = color;
            return this;
        }

        /// <summary>
        /// Set a toggle action for left/right input.
        /// </summary>
        /// <param name="toggleAction">Action to execute when toggled</param>
        /// <param name="getCurrentState">Function to get current state</param>
        /// <returns>This builder for chaining</returns>
        public MenuItemBuilder WithToggle(Action<bool> toggleAction, Func<bool> getCurrentState)
        {
            this.toggleAction = toggleAction;
            this.getCurrentState = getCurrentState;
            return this;
        }

        /// <summary>
        /// Set custom data to store with this item.
        /// </summary>
        /// <param name="data">Custom data object</param>
        /// <returns>This builder for chaining</returns>
        public MenuItemBuilder WithData(object data)
        {
            this.customData = data;
            return this;
        }

        /// <summary>
        /// Build the configured menu item.
        /// </summary>
        /// <returns>The constructed MenuBarItem</returns>
        public MenuBarItem Build()
        {
            var item = new MenuBarItem
            {
                name = text,
                size = size,
                color = color
            };

            if (!string.IsNullOrEmpty(methodName))
            {
                item.invokeMethod = methodName;
            }
            else if (action != null)
            {
                item.invokeMethod = $"Dynamic_{text.Replace(" ", "_")}";

                StoreAction(item.invokeMethod, action);
            }
            else if (toggleAction != null)
            {
                item.invokeMethod = $"Toggle_{text.Replace(" ", "_")}";

                StoreToggleAction(item.invokeMethod, toggleAction, getCurrentState);
            }
            else
            {
                item.invokeMethod = "NoOp";
            }

            if (customData != null)
            {
                StoreCustomData(item.invokeMethod, customData);
            }

            return item;
        }

        /// <summary>
        /// Build a MenuBarItem specifically for SimpleSubmenu use.
        /// </summary>
        /// <returns>The constructed MenuBarItem</returns>
        public MenuBarItem BuildForSimpleSubmenu()
        {
            var item = new MenuBarItem
            {
                name = text,
                size = size,
                color = color,
                invokeMethod = methodName ?? "DynamicAction"
            };

            return item;
        }

        /// <summary>
        /// Get the action associated with this builder.
        /// </summary>
        /// <returns>The action or null</returns>
        public Action GetAction()
        {
            return action;
        }

        private static readonly System.Collections.Generic.Dictionary<string, Action> dynamicActions =
            new System.Collections.Generic.Dictionary<string, Action>();
        public class ToggleActionPair
        {
            public Action<bool> Toggle { get; set; }
            public Func<bool> GetState { get; set; }
        }

        private static readonly System.Collections.Generic.Dictionary<string, ToggleActionPair> toggleActions =
            new System.Collections.Generic.Dictionary<string, ToggleActionPair>();
        private static readonly System.Collections.Generic.Dictionary<string, object> customDataStorage =
            new System.Collections.Generic.Dictionary<string, object>();

        private void StoreAction(string methodName, Action action)
        {
            dynamicActions[methodName] = action;
        }

        private void StoreToggleAction(string methodName, Action<bool> toggle, Func<bool> getState)
        {
            toggleActions[methodName] = new ToggleActionPair { Toggle = toggle, GetState = getState };
        }

        private void StoreCustomData(string methodName, object data)
        {
            customDataStorage[methodName] = data;
        }

        /// <summary>
        /// Get a stored dynamic action by method name.
        /// </summary>
        public static Action GetDynamicAction(string methodName)
        {
            return dynamicActions.ContainsKey(methodName) ? dynamicActions[methodName] : null;
        }

        /// <summary>
        /// Get a stored toggle action by method name.
        /// </summary>
        public static ToggleActionPair GetToggleAction(string methodName)
        {
            return toggleActions.ContainsKey(methodName) ? toggleActions[methodName] : null;
        }

        /// <summary>
        /// Get stored custom data by method name.
        /// </summary>
        public static object GetCustomData(string methodName)
        {
            return customDataStorage.ContainsKey(methodName) ? customDataStorage[methodName] : null;
        }

        /// <summary>
        /// Clear all stored dynamic actions and data.
        /// </summary>
        public static void ClearDynamicStorage()
        {
            dynamicActions.Clear();
            toggleActions.Clear();
            customDataStorage.Clear();
        }
    }
}