using RocketLib.Menus.Core;
using RocketLib.Menus.Elements;
using RocketLib.Menus.Layout;
using System.Collections.Generic;
using UnityEngine;

namespace RocketLib.Menus.Tests
{
    public class PaginatedGridExample : FlexMenu
    {
        public override string MenuTitle => "PAGINATED GRID EXAMPLE";

        private PaginatedGridContainer paginatedGrid;
        private TextElement pageIndicator;

        public PaginatedGridExample()
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
                Name = "PaginatedTitle",
                Text = "SELECT AN ITEM",
                HeightMode = SizeMode.Fixed,
                Height = 40f,
                WidthMode = SizeMode.Fill,
                FontSize = 4f
            };
            rootContainer.AddChild(title);

            paginatedGrid = new PaginatedGridContainer("PaginatedGrid")
            {
                WidthMode = SizeMode.Fill,
                HeightMode = SizeMode.Fill,
                Columns = 4,
                Rows = 3,
                GridSpacing = 10f,
                GridPadding = 10f,
                EnableTransitions = true,
                TransitionDuration = 0.3f
            };
            paginatedGrid.SetParentMenu(this);
            rootContainer.AddChild(paginatedGrid);

            var previousButton = new ActionButton("PrevButton")
            {
                Name = "PreviousButton",
                Text = "<",
                WidthMode = SizeMode.Fixed,
                Width = 40f,
                HeightMode = SizeMode.Fixed,
                Height = 30f
            };

            var nextButton = new ActionButton("NextButton")
            {
                Name = "NextButton",
                Text = ">",
                WidthMode = SizeMode.Fixed,
                Width = 40f,
                HeightMode = SizeMode.Fixed,
                Height = 30f
            };

            paginatedGrid.SetNavigationButtons(previousButton, nextButton);

            var items = new List<LayoutElement>();
            for (int i = 1; i <= 25; i++)
            {
                int index = i;
                var button = new ActionButton($"Item{i}")
                {
                    Name = $"Item{i}",
                    Text = $"ITEM {i}",
                    WidthMode = SizeMode.Fill,
                    HeightMode = SizeMode.Fill,
                    OnClick = () => {
                        Main.logger.Log($"Item {index} selected!");
                    }
                };
                items.Add(button);
            }

            paginatedGrid.SetItems(items);

            pageIndicator = new TextElement("PageIndicator")
            {
                Name = "PageIndicator",
                Text = $"Page {paginatedGrid.CurrentPage + 1} of {paginatedGrid.TotalPages}",
                HeightMode = SizeMode.Fixed,
                Height = 30f,
                WidthMode = SizeMode.Fill,
                FontSize = 3f
            };
            rootContainer.AddChild(pageIndicator);

            paginatedGrid.OnPageChanged = (page) => {
                pageIndicator.Text = $"Page {page + 1} of {paginatedGrid.TotalPages}";
                Main.logger.Log($"Changed to page {page + 1}");
            };

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

        public static PaginatedGridExample Show(Menu parentMenu = null)
        {
            return FlexMenu.Show<PaginatedGridExample>(parentGame: parentMenu);
        }
    }
}
