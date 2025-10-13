using RocketLib.Menus.Core;
using RocketLib.Menus.Elements;
using RocketLib.Menus.Layout;
using UnityEngine;

namespace RocketLib.Menus.Tests
{
    /// <summary>
    /// Test menu for demonstrating transition animations
    /// </summary>
    public class TransitionTestMenu : FlexMenu
    {
        private readonly TextElement titleText;
        private TextElement statusText;
        private ActionButton transitionToggle;

        public override string MenuId => "TransitionTestMenu";
        public override string MenuTitle => "Transition Test";

        protected override void InitializeContainer()
        {
            base.InitializeContainer();

            EnableTransition = true;

            rootContainer = new VerticalLayoutContainer
            {
                Name = "MainContainer",
                WidthMode = SizeMode.Fill,
                HeightMode = SizeMode.Fill,
                Spacing = 15f,
                Padding = 10f
            };
        }

        protected override void Start()
        {
            base.Start();

            var title = new TextElement("Title")
            {
                Text = "TRANSITION TEST MENU",
                TextColor = Color.white,
                FontSize = 7f,
                WidthMode = SizeMode.Fill,
                HeightMode = SizeMode.Fixed,
                Height = 35f
            };
            rootContainer.AddChild(title);

            var topSpacer = new SpacerElement("TopSpacer")
            {
                HeightMode = SizeMode.Fixed,
                Height = 10f
            };
            rootContainer.AddChild(topSpacer);

            statusText = new TextElement("StatusText")
            {
                Text = "Transitions are DISABLED",
                TextColor = Color.green,
                FontSize = 4.5f,
                WidthMode = SizeMode.Fill,
                HeightMode = SizeMode.Fixed,
                Height = 25f
            };
            rootContainer.AddChild(statusText);

            var contentArea = new VerticalLayoutContainer
            {
                Name = "ContentArea",
                WidthMode = SizeMode.Fixed,
                Width = 280f,
                HeightMode = SizeMode.Fixed,
                Height = 180f,
                Spacing = 8f
            };

            var centeringWrapper = new VerticalLayoutContainer
            {
                Name = "CenteringWrapper",
                WidthMode = SizeMode.Fill,
                HeightMode = SizeMode.Fill
            };

            var contentTopSpacer = new SpacerElement("ContentTopSpacer")
            {
                HeightMode = SizeMode.Fixed,
                Height = -20f
            };
            centeringWrapper.AddChild(contentTopSpacer);

            var horizontalCenter = new HorizontalLayoutContainer
            {
                Name = "HorizontalCenter",
                WidthMode = SizeMode.Fill,
                HeightMode = SizeMode.Fixed,
                Height = 180f
            };

            var leftSpacer = new SpacerElement("LeftSpacer")
            {
                WidthMode = SizeMode.Fill
            };
            horizontalCenter.AddChild(leftSpacer);

            horizontalCenter.AddChild(contentArea);

            var rightSpacer = new SpacerElement("RightSpacer")
            {
                WidthMode = SizeMode.Fill
            };
            horizontalCenter.AddChild(rightSpacer);

            centeringWrapper.AddChild(horizontalCenter);

            var contentBottomSpacer = new SpacerElement("ContentBottomSpacer")
            {
                HeightMode = SizeMode.Fill
            };
            centeringWrapper.AddChild(contentBottomSpacer);

            rootContainer.AddChild(centeringWrapper);

            var btn1 = new ActionButton("TestButton1")
            {
                Text = "TEST OPTION 1",
                FontSize = 4.5f,
                WidthMode = SizeMode.Fill,
                HeightMode = SizeMode.Fixed,
                Height = 30f,
                OnClick = () => TestAction("Option 1")
            };
            contentArea.AddChild(btn1);

            var btn2 = new ActionButton("TestButton2")
            {
                Text = "TEST OPTION 2",
                FontSize = 4.5f,
                WidthMode = SizeMode.Fill,
                HeightMode = SizeMode.Fixed,
                Height = 30f,
                OnClick = () => TestAction("Option 2")
            };
            contentArea.AddChild(btn2);

            var btn3 = new ActionButton("TestButton3")
            {
                Text = "TEST OPTION 3",
                FontSize = 4.5f,
                WidthMode = SizeMode.Fill,
                HeightMode = SizeMode.Fixed,
                Height = 30f,
                OnClick = () => TestAction("Option 3")
            };
            contentArea.AddChild(btn3);

            transitionToggle = new ActionButton("TransitionToggle")
            {
                Text = "TOGGLE TRANSITIONS",
                FontSize = 4.5f,
                WidthMode = SizeMode.Fill,
                HeightMode = SizeMode.Fixed,
                Height = 30f,
                OnClick = () => ToggleTransitions()
            };
            contentArea.AddChild(transitionToggle);

            var backBtn = new ActionButton("BackButton")
            {
                Text = "BACK",
                FontSize = 4.5f,
                WidthMode = SizeMode.Fill,
                HeightMode = SizeMode.Fixed,
                Height = 30f,
                OnClick = () => GoBack()
            };
            contentArea.AddChild(backBtn);

            RefreshLayout();
        }

        private void TestAction(string option)
        {
            Main.logger?.Log($"[TransitionTestMenu] {option} triggered!");
        }

        private void ToggleTransitions()
        {
            EnableTransition = !EnableTransition;

            statusText.Text = EnableTransition ? "Transitions are ENABLED" : "Transitions are DISABLED";
            statusText.TextColor = EnableTransition ? Color.green : Color.red;

            Main.logger?.Log($"[TransitionTestMenu] Transitions: {EnableTransition}");
        }
    }
}
