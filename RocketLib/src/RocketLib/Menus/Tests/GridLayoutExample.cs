using RocketLib.Menus.Core;
using RocketLib.Menus.Elements;
using RocketLib.Menus.Layout;
using UnityEngine;

namespace RocketLib.Menus.Tests
{
    public class GridLayoutExample : FlexMenu
    {
        public override string MenuTitle => "GRID LAYOUT EXAMPLE";

        public GridLayoutExample()
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

            var title = new TextElement("Title")
            {
                Name = "GridTitle",
                Text = "SELECT AN OPTION",
                HeightMode = SizeMode.Fixed,
                Height = 40f,
                WidthMode = SizeMode.Fill,
                FontSize = 4f
            };
            rootContainer.AddChild(title);

            var gridContainer = new GridLayoutContainer("GridContainer")
            {
                WidthMode = SizeMode.Fill,
                HeightMode = SizeMode.Fill,
                Columns = 3,
                ColumnSpacing = 10f,
                RowSpacing = 10f,
                Padding = 10f,
                CellHorizontalAlignment = HorizontalAlignment.Center,
                CellVerticalAlignment = VerticalAlignment.Center
            };
            rootContainer.AddChild(gridContainer);

            for (int i = 1; i <= 9; i++)
            {
                int index = i;
                var button = new ActionButton($"GridButton{i}")
                {
                    Name = $"Option{i}",
                    Text = $"OPTION {i}",
                    WidthMode = SizeMode.Fill,
                    HeightMode = SizeMode.Fill,
                    OnClick = () => {
                        Main.logger.Log($"Grid option {index} selected!");
                    }
                };
                gridContainer.AddChild(button);
            }

            var backButton = new ActionButton("BackButton")
            {
                Name = "BackButton",
                Text = "BACK",
                WidthMode = SizeMode.Fixed,
                Width = 150f,
                HeightMode = SizeMode.Fixed,
                Height = 40f,
                OnClick = () => {
                    GoBack();
                }
            };

            var buttonContainer = new HorizontalLayoutContainer("ButtonContainer")
            {
                WidthMode = SizeMode.Fill,
                HeightMode = SizeMode.Fixed,
                Height = 60f,
                Spacing = 10f
            };
            buttonContainer.AddChild(backButton);
            rootContainer.AddChild(buttonContainer);

            RefreshLayout();
        }

        public static GridLayoutExample Show(Menu parentMenu = null)
        {
            return FlexMenu.Show<GridLayoutExample>(parentGame: parentMenu);
        }
    }
}
