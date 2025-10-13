using System;
using System.Collections.Generic;
using RocketLib.Menus.Core;
using RocketLib.Menus.Elements;
using UnityEngine;

namespace RocketLib.Menus.Layout
{
    /// <summary>
    /// Container that manages multiple grid pages with navigation and smooth transitions
    /// </summary>
    public class PaginatedGridContainer : LayoutContainer
    {
        // Grid configuration
        public int Columns { get; set; } = 5;
        public int Rows { get; set; } = 3;
        public float GridSpacing { get; set; } = 10f;
        public float GridPadding { get; set; } = 15f;

        // Pagination state
        public int CurrentPage { get; private set; } = 0;
        public int TotalPages { get; private set; } = 1;

        // Animation settings
        public float TransitionDuration { get; set; } = 0.5f;
        public bool EnableTransitions { get; set; } = true;

        // Navigation buttons
        private ActionButton previousButton;
        private ActionButton nextButton;

        // Grid pages
        private readonly List<GridLayoutContainer> gridPages = new List<GridLayoutContainer>();
        private List<LayoutElement> allItems = new List<LayoutElement>();

        // Animation state
        private bool isTransitioning = false;
        private float transitionProgress = 0f;
        private int transitionFromPage = -1;
        private int transitionToPage = -1;
        private float transitionDirection = 0f; // -1 for left, 1 for right

        // Events
        public Action<int> OnPageChanged { get; set; }

        // Parent menu reference for navigation refresh
        private FlexMenu parentMenu;

        // Layout zones
        private const float BUTTON_WIDTH = 40f;  // Smaller width
        private const float BUTTON_HEIGHT = 30f;  // Smaller height
        private const float BUTTON_MARGIN = 5f;  // Much less margin

        public PaginatedGridContainer(string name = "PaginatedGridContainer") : base(name)
        {
        }

        /// <summary>
        /// Set the parent menu for navigation refresh
        /// </summary>
        public void SetParentMenu(FlexMenu menu)
        {
            parentMenu = menu;
        }

        /// <summary>
        /// Set all items to be paginated
        /// </summary>
        public void SetItems(List<LayoutElement> items)
        {
            // Clear existing pages
            foreach (var grid in gridPages)
            {
                grid.Cleanup();
            }
            gridPages.Clear();

            allItems = new List<LayoutElement>(items);

            // Calculate pagination
            int itemsPerPage = Columns * Rows;
            TotalPages = Mathf.Max(1, Mathf.CeilToInt(allItems.Count / (float)itemsPerPage));

            // Create grid pages
            for (int pageIndex = 0; pageIndex < TotalPages; pageIndex++)
            {
                var grid = CreateGridPage(pageIndex, itemsPerPage);
                gridPages.Add(grid);
            }

            // Set initial visibility
            UpdatePageVisibility();
            UpdateNavigationButtons();
        }

        private GridLayoutContainer CreateGridPage(int pageIndex, int itemsPerPage)
        {
            var grid = new GridLayoutContainer
            {
                Name = $"GridPage_{pageIndex}",
                Columns = Columns,
                Rows = Rows,
                Spacing = GridSpacing,
                Padding = GridPadding,
                WidthMode = SizeMode.Fill,
                HeightMode = SizeMode.Fill,
                IsVisible = (pageIndex == CurrentPage),
                IsFocusable = false  // Grid containers themselves should never be focusable
            };

            // Add items to this page
            int startIndex = pageIndex * itemsPerPage;
            int endIndex = Mathf.Min(startIndex + itemsPerPage, allItems.Count);

            for (int i = startIndex; i < endIndex; i++)
            {
                grid.AddChild(allItems[i]);
            }

            AddChild(grid);
            return grid;
        }

        protected override void ArrangeChildren()
        {
            // Position navigation buttons
            if (previousButton != null)
            {
                previousButton.ActualPosition = new Vector2(
                    ActualPosition.x - (ActualSize.x / 2) + BUTTON_WIDTH / 2 + BUTTON_MARGIN,
                    ActualPosition.y
                );
                previousButton.ActualSize = new Vector2(BUTTON_WIDTH, BUTTON_HEIGHT);
            }

            if (nextButton != null)
            {
                nextButton.ActualPosition = new Vector2(
                    ActualPosition.x + (ActualSize.x / 2) - BUTTON_WIDTH / 2 - BUTTON_MARGIN,
                    ActualPosition.y
                );
                nextButton.ActualSize = new Vector2(BUTTON_WIDTH, BUTTON_HEIGHT);
            }

            // Calculate grid area (excluding smaller button zones)
            float gridAreaLeft = ActualPosition.x - (ActualSize.x / 2) + BUTTON_WIDTH + BUTTON_MARGIN;
            float gridAreaRight = ActualPosition.x + (ActualSize.x / 2) - BUTTON_WIDTH - BUTTON_MARGIN;
            float gridAreaWidth = gridAreaRight - gridAreaLeft;
            float gridCenterX = gridAreaLeft + gridAreaWidth / 2;

            // Position grids
            for (int i = 0; i < gridPages.Count; i++)
            {
                var grid = gridPages[i];

                // Base position
                Vector2 basePosition = new Vector2(gridCenterX, ActualPosition.y);

                // Apply transition offset if animating
                if (isTransitioning)
                {
                    if (i == transitionFromPage)
                    {
                        // Outgoing page slides away
                        float offsetX = gridAreaWidth * transitionProgress * transitionDirection;
                        grid.ActualPosition = basePosition + new Vector2(offsetX, 0);
                    }
                    else if (i == transitionToPage)
                    {
                        // Incoming page slides in
                        float offsetX = gridAreaWidth * (1f - transitionProgress) * -transitionDirection;
                        grid.ActualPosition = basePosition + new Vector2(offsetX, 0);
                    }
                    else
                    {
                        // Hidden pages stay off-screen
                        grid.ActualPosition = basePosition + new Vector2(gridAreaWidth * 2, 0);
                    }
                }
                else
                {
                    // Static positioning
                    if (i == CurrentPage)
                    {
                        grid.ActualPosition = basePosition;
                    }
                    else
                    {
                        // Keep hidden pages off-screen
                        grid.ActualPosition = basePosition + new Vector2(gridAreaWidth * 2, 0);
                    }
                }

                grid.ActualSize = new Vector2(gridAreaWidth, ActualSize.y);

                // Force grid to update its children's layout
                grid.UpdateLayout();
            }
        }

        public override void Render()
        {
            // Update animation
            if (isTransitioning && EnableTransitions)
            {
                transitionProgress += Time.deltaTime / TransitionDuration;

                if (transitionProgress >= 1f)
                {
                    // Animation complete
                    transitionProgress = 1f;
                    isTransitioning = false;
                    CurrentPage = transitionToPage;
                    UpdatePageVisibility();
                    UpdateNavigationButtons();
                    OnPageChanged?.Invoke(CurrentPage);

                    // Refresh navigation after page change
                    if (parentMenu != null)
                    {
                        parentMenu.RefreshLayout();
                    }
                }

                // Force re-layout during animation
                ArrangeChildren();
            }

            base.Render();
        }

        /// <summary>
        /// Navigate to the next page
        /// </summary>
        public void NextPage()
        {
            if (isTransitioning || CurrentPage >= TotalPages - 1) return;

            StartTransition(CurrentPage, CurrentPage + 1, -1f);
        }

        /// <summary>
        /// Navigate to the previous page
        /// </summary>
        public void PreviousPage()
        {
            if (isTransitioning || CurrentPage <= 0) return;

            StartTransition(CurrentPage, CurrentPage - 1, 1f);
        }

        /// <summary>
        /// Jump to a specific page without animation
        /// </summary>
        public void GoToPage(int pageIndex)
        {
            if (pageIndex < 0 || pageIndex >= TotalPages) return;

            CurrentPage = pageIndex;
            UpdatePageVisibility();
            UpdateNavigationButtons();
            OnPageChanged?.Invoke(CurrentPage);

            // Refresh navigation for instant transitions
            if (parentMenu != null)
            {
                parentMenu.RefreshLayout();
            }
        }

        private void StartTransition(int fromPage, int toPage, float direction)
        {
            if (!EnableTransitions)
            {
                // Instant transition
                GoToPage(toPage);
                return;
            }

            isTransitioning = true;
            transitionProgress = 0f;
            transitionFromPage = fromPage;
            transitionToPage = toPage;
            transitionDirection = direction;

            // Make both pages visible during transition
            if (fromPage >= 0 && fromPage < gridPages.Count)
            {
                gridPages[fromPage].IsVisible = true;
            }
            if (toPage >= 0 && toPage < gridPages.Count)
            {
                gridPages[toPage].IsVisible = true;
            }
        }

        private void UpdatePageVisibility()
        {
            for (int i = 0; i < gridPages.Count; i++)
            {
                bool isCurrentPage = (i == CurrentPage);
                var grid = gridPages[i];

                // Set visibility
                grid.IsVisible = isCurrentPage;

                // The grid container itself should never be focusable
                grid.IsFocusable = false;

                // Set focusability for all children
                foreach (var child in grid.Children)
                {
                    child.IsFocusable = isCurrentPage && (child is BroCard || child is ActionButton);
                }
            }
        }

        private void UpdateNavigationButtons()
        {
            if (previousButton != null)
            {
                previousButton.IsVisible = CurrentPage > 0;
                previousButton.IsFocusable = CurrentPage > 0;
            }

            if (nextButton != null)
            {
                nextButton.IsVisible = CurrentPage < TotalPages - 1;
                nextButton.IsFocusable = CurrentPage < TotalPages - 1;
            }
        }

        /// <summary>
        /// Initialize navigation buttons (called from menu using this container)
        /// </summary>
        public void SetNavigationButtons(ActionButton previous, ActionButton next)
        {
            previousButton = previous;
            nextButton = next;

            if (previousButton != null)
            {
                AddChild(previousButton);
                previousButton.OnClick = PreviousPage;
            }

            if (nextButton != null)
            {
                AddChild(nextButton);
                nextButton.OnClick = NextPage;
            }

            UpdateNavigationButtons();
        }

        public override List<LayoutElement> GetFocusableElements()
        {
            var elements = new List<LayoutElement>();

            if (!IsEnabled) return elements;

            // Only get focusable elements from the CURRENT page's grid
            if (CurrentPage >= 0 && CurrentPage < gridPages.Count)
            {
                var currentGrid = gridPages[CurrentPage];
                elements.AddRange(currentGrid.GetFocusableElements());
            }

            // Also include navigation buttons if they're visible/focusable
            if (previousButton != null && previousButton.IsFocusable)
            {
                elements.Add(previousButton);
            }
            if (nextButton != null && nextButton.IsFocusable)
            {
                elements.Add(nextButton);
            }

            return elements;
        }

        public override void Cleanup()
        {
            foreach (var grid in gridPages)
            {
                grid.Cleanup();
            }
            gridPages.Clear();
            allItems.Clear();

            base.Cleanup();
        }

        /// <summary>
        /// Get the currently focused element across all pages
        /// </summary>
        public LayoutElement GetFocusedElement()
        {
            if (CurrentPage >= 0 && CurrentPage < gridPages.Count)
            {
                var currentGrid = gridPages[CurrentPage];
                foreach (var child in currentGrid.Children)
                {
                    if (child.IsFocused)
                        return child;
                }
            }
            return null;
        }

        /// <summary>
        /// Focus the first focusable element on the current page
        /// </summary>
        public void FocusFirstElement()
        {
            if (CurrentPage >= 0 && CurrentPage < gridPages.Count)
            {
                var currentGrid = gridPages[CurrentPage];
                foreach (var child in currentGrid.Children)
                {
                    if (child.IsFocusable)
                    {
                        child.IsFocused = true;
                        return;
                    }
                }
            }
        }
    }
}