using RocketLib.Menus.Vanilla;
using UnityEngine;

namespace RocketLib.Menus.Tests
{
    public class VanillaSubmenuExample : BaseCustomMenu
    {
        public override string MenuTitle => "VANILLA SUBMENU";

        protected override void SetupMenuItems()
        {
            AddMenuItem("OPTION 1", "SelectOption1");
            AddMenuItem("OPTION 2", "SelectOption2");
            AddMenuItem("OPTION 3", "SelectOption3");
            AddMenuItem("BACK", "GoBackToParent");
        }

        private void SelectOption1()
        {
            Main.logger.Log("Option 1 selected!");
        }

        private void SelectOption2()
        {
            Main.logger.Log("Option 2 selected!");
        }

        private void SelectOption3()
        {
            Main.logger.Log("Option 3 selected!");
        }

        private void GoBackToParent()
        {
            OnMenuClosed();
        }

        public static VanillaSubmenuExample Show(Menu parentMenu)
        {
            var existingMenu = GameObject.FindObjectOfType<VanillaSubmenuExample>();
            if (existingMenu != null)
            {
                existingMenu.MenuActive = true;
                existingMenu.OnMenuOpened();
                return existingMenu;
            }

            var menuGameObject = new GameObject("VanillaSubmenuExample");
            var menu = menuGameObject.AddComponent<VanillaSubmenuExample>();
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