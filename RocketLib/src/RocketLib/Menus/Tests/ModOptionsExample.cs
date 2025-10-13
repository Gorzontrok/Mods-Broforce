using RocketLib.Menus.Vanilla;
using UnityEngine;

namespace RocketLib.Menus.Tests
{
    public class ModOptionsExample : BaseCustomMenu
    {
        public override string MenuTitle => "MY MOD OPTIONS";

        private bool option1Enabled = true;
        private bool option2Enabled = false;
        private bool option3Enabled = true;

        protected override void SetupMenuItems()
        {
            AddMenuItem($"OPTION 1: {(option1Enabled ? "ON" : "OFF")}", "ToggleOption1");
            AddMenuItem($"OPTION 2: {(option2Enabled ? "ON" : "OFF")}", "ToggleOption2");
            AddMenuItem($"OPTION 3: {(option3Enabled ? "ON" : "OFF")}", "ToggleOption3");
            AddMenuItem("BACK", "GoBackToParent");
        }

        private void ToggleOption1()
        {
            option1Enabled = !option1Enabled;
            Main.logger.Log($"Option 1 toggled to {(option1Enabled ? "ON" : "OFF")}");
            RefreshMenuItems();
        }

        private void ToggleOption2()
        {
            option2Enabled = !option2Enabled;
            Main.logger.Log($"Option 2 toggled to {(option2Enabled ? "ON" : "OFF")}");
            RefreshMenuItems();
        }

        private void ToggleOption3()
        {
            option3Enabled = !option3Enabled;
            Main.logger.Log($"Option 3 toggled to {(option3Enabled ? "ON" : "OFF")}");
            RefreshMenuItems();
        }

        private void RefreshMenuItems()
        {
            if (items != null && items.Length >= 3)
            {
                items[0].text = $"OPTION 1: {(option1Enabled ? "ON" : "OFF")}";
                items[1].text = $"OPTION 2: {(option2Enabled ? "ON" : "OFF")}";
                items[2].text = $"OPTION 3: {(option3Enabled ? "ON" : "OFF")}";
            }
        }

        private void GoBackToParent()
        {
            OnMenuClosed();
        }

        public static ModOptionsExample Show(Menu parentMenu)
        {
            var existingMenu = GameObject.FindObjectOfType<ModOptionsExample>();
            if (existingMenu != null)
            {
                existingMenu.MenuActive = true;
                existingMenu.OnMenuOpened();
                return existingMenu;
            }

            var menuGameObject = new GameObject("ModOptionsExample");
            var menu = menuGameObject.AddComponent<ModOptionsExample>();
            menu.Initialize(parentMenu);

            if (parentMenu != null)
            {
                parentMenu.MenuActive = false;
            }

            menu.MenuActive = true;
            menu.TransitionIn();
            menu.OnMenuOpened();

            return menu;
        }
    }
}
