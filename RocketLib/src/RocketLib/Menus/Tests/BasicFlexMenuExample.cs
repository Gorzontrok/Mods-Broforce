using RocketLib.Menus.Core;
using RocketLib.Menus.Elements;
using RocketLib.Menus.Layout;

namespace RocketLib.Menus.Tests
{
    public class BasicFlexMenuExample : FlexMenu
    {
        public override string MenuId => "RocketLib_BasicFlexMenuExample";
        public override string MenuTitle => "BASIC FLEX MENU";

        public BasicFlexMenuExample()
        {
            EnableDebugOutput = true;
        }

        protected override void InitializeContainer()
        {
            rootContainer = new VerticalLayoutContainer
            {
                Name = "MainContainer",
                WidthMode = SizeMode.Fill,
                HeightMode = SizeMode.Fill,
                Padding = 20f,
                Spacing = 10f
            };
        }

        protected override void Start()
        {
            base.Start();

            // Title
            var title = new TextElement("Title")
            {
                Name = "MenuTitle",
                Text = "FLEX MENU EXAMPLE",
                HeightMode = SizeMode.Fixed,
                Height = 50f,
                WidthMode = SizeMode.Fill,
                FontSize = 5f
            };
            rootContainer.AddChild(title);

            // Content container - takes all remaining space
            var contentContainer = new VerticalLayoutContainer("ContentContainer")
            {
                WidthMode = SizeMode.Fill,
                HeightMode = SizeMode.Fill,
                Spacing = 5f,
                Padding = 10f,
                ChildHorizontalAlignment = HorizontalAlignment.Center
            };
            rootContainer.AddChild(contentContainer);

            // Sample buttons
            var button1 = new ActionButton("Button1")
            {
                Name = "TestButton1",
                Text = "FIRST BUTTON",
                WidthMode = SizeMode.Fixed,
                Width = 250f,
                HeightMode = SizeMode.Fixed,
                Height = 40f,
                OnClick = () =>
                {
                    Main.logger.Log("First button clicked!");
                }
            };
            contentContainer.AddChild(button1);

            var button2 = new ActionButton("Button2")
            {
                Name = "TestButton2",
                Text = "SECOND BUTTON",
                WidthMode = SizeMode.Fixed,
                Width = 250f,
                HeightMode = SizeMode.Fixed,
                Height = 40f,
                OnClick = () =>
                {
                    Main.logger.Log("Second button clicked!");
                }
            };
            contentContainer.AddChild(button2);

            var button3 = new ActionButton("Button3")
            {
                Name = "TestButton3",
                Text = "THIRD BUTTON",
                WidthMode = SizeMode.Fixed,
                Width = 250f,
                HeightMode = SizeMode.Fixed,
                Height = 40f,
                OnClick = () =>
                {
                    Main.logger.Log("Third button clicked!");
                }
            };
            contentContainer.AddChild(button3);

            // Back button
            var backButton = new ActionButton("BackButton")
            {
                Name = "BackButton",
                Text = "BACK",
                WidthMode = SizeMode.Fixed,
                Width = 150f,
                HeightMode = SizeMode.Fixed,
                Height = 40f,
                OnClick = () =>
                {
                    GoBack();
                }
            };

            var buttonContainer = new HorizontalLayoutContainer("ButtonContainer")
            {
                WidthMode = SizeMode.Fill,
                HeightMode = SizeMode.Fixed,
                Height = 60f,
                ChildVerticalAlignment = VerticalAlignment.Center
            };
            buttonContainer.AddChild(backButton);
            rootContainer.AddChild(buttonContainer);

            // CRITICAL: Must refresh layout after adding all elements
            RefreshLayout();
        }

        // Convenience static method following the pattern
        public static BasicFlexMenuExample Show(Menu parentMenu = null)
        {
            return FlexMenu.Show<BasicFlexMenuExample>(parentGame: parentMenu);
        }
    }
}
