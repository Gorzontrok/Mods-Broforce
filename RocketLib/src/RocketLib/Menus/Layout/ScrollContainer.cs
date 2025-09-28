using System;
using System.Collections.Generic;
using RocketLib.Menus.Elements;
using UnityEngine;

namespace RocketLib.Menus.Layout
{
    /// <summary>
    /// Container that enables scrolling for content exceeding viewport bounds.
    /// Automatically scrolls based on focus changes from NavigationManager.
    /// </summary>
    public class ScrollContainer : LayoutContainer
    {
        // Content sizing (developer sets these)
        public float ContentWidth { get; set; } = 300f;
        public float ContentHeight { get; set; } = 600f;

        // Viewport size (derived from ActualSize)
        public float ViewportWidth => ActualSize.x;
        public float ViewportHeight => ActualSize.y;

        // Scrolling properties
        public ScrollDirection Direction { get; set; } = ScrollDirection.Vertical;
        public float ScrollPosition { get; set; } = 0f; // Current scroll offset in world units
        public ScrollBehavior Behavior { get; set; } = ScrollBehavior.KeepVisible;
        public float ScrollPadding { get; set; } = 20f; // Buffer zone for visibility
        public float ScrollSpeed { get; set; } = 5f; // Animation speed multiplier

        // Clipping properties
        public ClipMode ClipMode { get; set; } = ClipMode.DisableOffscreen;
        public bool DisableInvisible { get; set; } = true;

        // Events
        public Action<float> OnScroll { get; set; }

        // Internal state
        private float targetScrollPosition = 0f;
        private LayoutContainer contentContainer; // The actual container we're scrolling
        private LayoutElement lastFocusedElement = null;


        public ScrollContainer(string name = "ScrollContainer") : base(name)
        {
        }

        /// <summary>
        /// Set the content container that will be scrolled
        /// </summary>
        public void SetContent(LayoutContainer container)
        {
            if (container is AbsoluteLayoutContainer)
            {
                throw new InvalidOperationException("ScrollContainer does not support AbsoluteLayoutContainer. Use Vertical, Horizontal, or Grid layouts.");
            }

            contentContainer = container;
            if (contentContainer != null)
            {
                contentContainer.Parent = this;
                Children.Clear();
                Children.Add(contentContainer);
            }
        }

        public override void UpdateLayout()
        {
            if (contentContainer == null)
            {
                return;
            }


            // Set content container size based on ContentWidth/ContentHeight properties
            float actualContentWidth = ContentWidth;
            float actualContentHeight = ContentHeight;

            // For scrolling, content should be at least as large as viewport
            if (Direction == ScrollDirection.Vertical || Direction == ScrollDirection.Both)
            {
                actualContentHeight = Mathf.Max(ContentHeight, ViewportHeight);
            }
            if (Direction == ScrollDirection.Horizontal || Direction == ScrollDirection.Both)
            {
                actualContentWidth = Mathf.Max(ContentWidth, ViewportWidth);
            }

            contentContainer.ActualSize = new Vector2(actualContentWidth, actualContentHeight);

            // Position content container so its top aligns with viewport top at ScrollPosition=0
            // Key insight: ActualPosition is the CENTER of the container
            float xOffset = 0f;
            float yOffset = 0f;

            if (Direction == ScrollDirection.Vertical || Direction == ScrollDirection.Both)
            {
                // Align top edges when ScrollPosition = 0
                // As ScrollPosition increases, content moves UP (negative Y) to show content below
                // BUT: Moving up means INCREASING Y offset (less negative), so we ADD ScrollPosition
                yOffset = -(actualContentHeight - ViewportHeight) / 2f + ScrollPosition;
            }

            if (Direction == ScrollDirection.Horizontal || Direction == ScrollDirection.Both)
            {
                // Align left edges when ScrollPosition = 0
                // As ScrollPosition increases, content moves LEFT (negative X) to show content to the right
                xOffset = -(actualContentWidth - ViewportWidth) / 2f - ScrollPosition;
            }

            contentContainer.ActualPosition = ActualPosition + new Vector2(xOffset, yOffset);


            // Pass menuTransform to content container (since we override UpdateLayout and don't call base)
            if (menuTransform != null)
            {
                contentContainer.SetMenuTransform(menuTransform);
            }

            // Update content layout
            contentContainer.UpdateLayout();

            // Update visibility of children based on ClipMode
            if (ClipMode == ClipMode.DisableOffscreen)
            {
                UpdateChildVisibility();
            }
            else
            {
            }
        }

        /// <summary>
        /// Called by NavigationManager when focus changes
        /// </summary>
        public void OnFocusChanged(LayoutElement newFocus)
        {

            if (newFocus == null || contentContainer == null)
            {
                return;
            }

            // Check if the focused element is within our content
            if (!IsChildOfContent(newFocus))
            {
                return;
            }

            lastFocusedElement = newFocus;
            switch (Behavior)
            {
                case ScrollBehavior.KeepVisible:
                    EnsureElementVisible(newFocus);
                    break;
                case ScrollBehavior.KeepCentered:
                    CenterOnElement(newFocus);
                    break;
                case ScrollBehavior.EdgeTriggered:
                    ScrollIfAtEdge(newFocus);
                    break;
            }
        }

        /// <summary>
        /// Ensure an element is visible within the viewport
        /// </summary>
        public void EnsureVisible(LayoutElement element)
        {
            if (element == null || contentContainer == null) return;
            EnsureElementVisible(element);
        }

        /// <summary>
        /// Scroll to a specific position
        /// </summary>
        public void ScrollTo(float position)
        {
            targetScrollPosition = position;
            ClampScrollPosition();
        }

        /// <summary>
        /// Scroll to a specific element
        /// </summary>
        public void ScrollToElement(LayoutElement element)
        {
            if (element == null) return;
            CenterOnElement(element);
        }

        /// <summary>
        /// Scroll by a delta amount
        /// </summary>
        public void ScrollBy(float delta)
        {
            targetScrollPosition = ScrollPosition + delta;
            ClampScrollPosition();
        }

        protected override void ArrangeChildren()
        {
            // ScrollContainer doesn't arrange children directly - the content container handles that
            // We just apply scroll offsets after the content container arranges its children
            if (contentContainer != null)
            {
                ApplyScrollOffset();
                UpdateChildVisibility();
            }
        }

        public void Update()
        {

            // Animate scroll position
            if (Mathf.Abs(ScrollPosition - targetScrollPosition) > 0.1f)
            {
                float oldScrollPos = ScrollPosition;
                ScrollPosition = Mathf.Lerp(ScrollPosition, targetScrollPosition, Time.deltaTime * ScrollSpeed);

                // Trigger layout update if position changed significantly
                if (Mathf.Abs(ScrollPosition - oldScrollPos) > 0.01f)
                {
                    UpdateLayout();
                    OnScroll?.Invoke(ScrollPosition);
                }
            }
            else if (ScrollPosition != targetScrollPosition)
            {
                ScrollPosition = targetScrollPosition;
                UpdateLayout();
                OnScroll?.Invoke(ScrollPosition);
            }
        }

        // Private helper methods

        private void CalculateContentBounds()
        {
            if (contentContainer == null) return;

            // Get the bounds of all content
            float minY = float.MaxValue;
            float maxY = float.MinValue;
            float minX = float.MaxValue;
            float maxX = float.MinValue;

            foreach (var child in GetAllChildren(contentContainer))
            {
                if (!child.IsVisible) continue;

                var bounds = child.GetBounds();
                minY = Mathf.Min(minY, bounds.y);
                maxY = Mathf.Max(maxY, bounds.y + bounds.height);
                minX = Mathf.Min(minX, bounds.x);
                maxX = Mathf.Max(maxX, bounds.x + bounds.width);
            }

            // Content size is set explicitly via ContentHeight/ContentWidth properties
            // Not calculated from children bounds
        }

        private void ApplyScrollOffset()
        {
            if (contentContainer == null) return;

            // Recalculate position with current scroll offset
            float xOffset = 0f;
            float yOffset = 0f;

            if (Direction == ScrollDirection.Vertical || Direction == ScrollDirection.Both)
            {
                yOffset = -(ContentHeight - ViewportHeight) / 2f + ScrollPosition;
            }

            if (Direction == ScrollDirection.Horizontal || Direction == ScrollDirection.Both)
            {
                xOffset = -(ContentWidth - ViewportWidth) / 2f + ScrollPosition;
            }

            contentContainer.ActualPosition = ActualPosition + new Vector2(xOffset, yOffset);
        }

        private void UpdateChildPositions(LayoutContainer container, Vector2 offset)
        {
            foreach (var child in container.Children)
            {
                // Update the child's actual position with the scroll offset
                child.ActualPosition = child.Position + offset;

                // If it's a container, update its children recursively
                if (child is LayoutContainer childContainer)
                {
                    UpdateChildPositions(childContainer, offset);
                }
            }
        }

        private void UpdateChildVisibility()
        {
            if (contentContainer == null || ClipMode != ClipMode.DisableOffscreen) return;


            var viewportBounds = GetViewportBounds();

            foreach (var child in GetAllChildren(contentContainer))
            {
                var childBounds = child.GetBounds();

                // Check if child is within viewport
                bool isVisible = BoundsIntersect(viewportBounds, childBounds);


                // Set visibility on the element itself
                child.IsVisible = isVisible;
            }
        }

        private Rect GetViewportBounds()
        {
            // Our ActualPosition is the CENTER of the viewport
            // Rect constructor is (x, y, width, height) where x,y is TOP-LEFT corner
            // In our world space, Y increases upward
            float left = ActualPosition.x - (ViewportWidth / 2f);
            float bottom = ActualPosition.y - (ViewportHeight / 2f);  // Bottom in world space (lower Y)

            // Create rect with bottom-left corner for world-space comparisons
            Rect bounds = new Rect(left, bottom, ViewportWidth, ViewportHeight);
            return bounds;
        }

        private bool BoundsIntersect(Rect a, Rect b)
        {
            return !(b.xMax < a.xMin || b.xMin > a.xMax ||
                    b.yMax < a.yMin || b.yMin > a.yMax);
        }

        private void EnsureElementVisible(LayoutElement element)
        {

            if (Direction == ScrollDirection.Vertical || Direction == ScrollDirection.Both)
            {
                // Element bounds in world space (already includes scroll offset from content container)
                float elementTop = element.ActualPosition.y + element.ActualSize.y / 2;
                float elementBottom = element.ActualPosition.y - element.ActualSize.y / 2;

                // Viewport bounds FIXED in world space (no ScrollPosition!)
                float viewportTop = ActualPosition.y + ViewportHeight / 2;
                float viewportBottom = ActualPosition.y - ViewportHeight / 2;

                // Include padding for better UX
                float paddedTop = viewportTop - ScrollPadding;
                float paddedBottom = viewportBottom + ScrollPadding;


                if (elementTop > paddedTop)
                {
                    // Element is above viewport, scroll up (decrease ScrollPosition)
                    float scrollNeeded = elementTop - paddedTop;
                    targetScrollPosition = ScrollPosition - scrollNeeded;
                }
                else if (elementBottom < paddedBottom)
                {
                    // Element is below viewport, scroll down (increase ScrollPosition)
                    float scrollNeeded = paddedBottom - elementBottom;
                    targetScrollPosition = ScrollPosition + scrollNeeded;
                }
                else
                {
                }
            }

            if (Direction == ScrollDirection.Horizontal || Direction == ScrollDirection.Both)
            {
                // Element bounds in world space (already includes scroll offset)
                float elementLeft = element.ActualPosition.x - element.ActualSize.x / 2;
                float elementRight = element.ActualPosition.x + element.ActualSize.x / 2;

                // Viewport bounds FIXED in world space
                float viewportLeft = ActualPosition.x - ViewportWidth / 2;
                float viewportRight = ActualPosition.x + ViewportWidth / 2;

                // Include padding
                float paddedLeft = viewportLeft + ScrollPadding;
                float paddedRight = viewportRight - ScrollPadding;

                if (elementLeft < paddedLeft)
                {
                    // Element is left of viewport, scroll left (decrease ScrollPosition for horizontal)
                    float scrollNeeded = paddedLeft - elementLeft;
                    targetScrollPosition = ScrollPosition - scrollNeeded;
                }
                else if (elementRight > paddedRight)
                {
                    // Element is right of viewport, scroll right (increase ScrollPosition)
                    float scrollNeeded = elementRight - paddedRight;
                    targetScrollPosition = ScrollPosition + scrollNeeded;
                }
            }

            ClampScrollPosition();
        }

        private void CenterOnElement(LayoutElement element)
        {
            if (Direction == ScrollDirection.Vertical || Direction == ScrollDirection.Both)
            {
                // Element is already in world space with scroll transform applied
                float elementCenter = element.ActualPosition.y;
                float viewportCenter = ActualPosition.y;  // Fixed viewport center

                // Calculate how much we need to scroll to center the element
                float offset = elementCenter - viewportCenter;
                targetScrollPosition = ScrollPosition - offset;

            }

            if (Direction == ScrollDirection.Horizontal || Direction == ScrollDirection.Both)
            {
                float elementCenter = element.ActualPosition.x;
                float viewportCenter = ActualPosition.x;

                float offset = elementCenter - viewportCenter;
                targetScrollPosition = ScrollPosition - offset;
            }

            ClampScrollPosition();
        }

        private void ScrollIfAtEdge(LayoutElement element)
        {
            if (Direction == ScrollDirection.Vertical || Direction == ScrollDirection.Both)
            {
                // Element bounds in world space (already includes scroll offset)
                float elementTop = element.ActualPosition.y + element.ActualSize.y / 2;
                float elementBottom = element.ActualPosition.y - element.ActualSize.y / 2;

                // Viewport bounds FIXED in world space
                float viewportTop = ActualPosition.y + ViewportHeight / 2;
                float viewportBottom = ActualPosition.y - ViewportHeight / 2;

                // Only scroll if element is at the edge
                float edgeThreshold = 5f; // Small threshold for "at edge"

                if (elementTop > viewportTop - edgeThreshold)
                {
                    // At top edge, scroll up to show more content above
                    float scrollNeeded = elementTop - viewportTop + ViewportHeight * 0.3f; // Scroll by 30% of viewport
                    targetScrollPosition = ScrollPosition - scrollNeeded;
                }
                else if (elementBottom < viewportBottom + edgeThreshold)
                {
                    // At bottom edge, scroll down to show more content below
                    float scrollNeeded = viewportBottom - elementBottom + ViewportHeight * 0.3f;
                    targetScrollPosition = ScrollPosition + scrollNeeded;
                }
            }

            if (Direction == ScrollDirection.Horizontal || Direction == ScrollDirection.Both)
            {
                float elementLeft = element.ActualPosition.x - element.ActualSize.x / 2;
                float elementRight = element.ActualPosition.x + element.ActualSize.x / 2;

                float viewportLeft = ActualPosition.x - ViewportWidth / 2;
                float viewportRight = ActualPosition.x + ViewportWidth / 2;

                float edgeThreshold = 5f;

                if (elementLeft < viewportLeft + edgeThreshold)
                {
                    // At left edge, scroll left
                    float scrollNeeded = viewportLeft - elementLeft + ViewportWidth * 0.3f;
                    targetScrollPosition = ScrollPosition - scrollNeeded;
                }
                else if (elementRight > viewportRight - edgeThreshold)
                {
                    // At right edge, scroll right
                    float scrollNeeded = elementRight - viewportRight + ViewportWidth * 0.3f;
                    targetScrollPosition = ScrollPosition + scrollNeeded;
                }
            }

            ClampScrollPosition();
        }

        private void ClampScrollPosition()
        {
            if (Direction == ScrollDirection.Vertical || Direction == ScrollDirection.Both)
            {
                float maxScroll = Mathf.Max(0, ContentHeight - ViewportHeight);
                targetScrollPosition = Mathf.Clamp(targetScrollPosition, 0, maxScroll);
            }

            if (Direction == ScrollDirection.Horizontal || Direction == ScrollDirection.Both)
            {
                float maxScroll = Mathf.Max(0, ContentWidth - ViewportWidth);
                targetScrollPosition = Mathf.Clamp(targetScrollPosition, 0, maxScroll);
            }
        }

        private bool IsChildOfContent(LayoutElement element)
        {
            if (contentContainer == null) return false;

            // Check if element is a descendant of our content container
            var parent = element.Parent;
            while (parent != null)
            {
                if (parent == contentContainer) return true;
                parent = parent.Parent;
            }

            return false;
        }

        private List<LayoutElement> GetAllChildren(LayoutContainer container)
        {
            var allChildren = new List<LayoutElement>();
            if (container != null)
            {
                CollectChildren(container, allChildren);
            }
            return allChildren;
        }

        private void CollectChildren(LayoutContainer container, List<LayoutElement> collection)
        {
            foreach (var child in container.Children)
            {
                collection.Add(child);
                if (child is LayoutContainer childContainer)
                {
                    CollectChildren(childContainer, collection);
                }
            }
        }

        public override void Cleanup()
        {
            contentContainer?.Cleanup();
            base.Cleanup();
        }
    }

    public enum ScrollDirection
    {
        Vertical,
        Horizontal,
        Both
    }

    public enum ScrollBehavior
    {
        KeepVisible,      // Minimal scrolling to keep focused item in view
        KeepCentered,     // Always center the focused item
        EdgeTriggered     // Only scroll when navigating past viewport edge
    }

    public enum ClipMode
    {
        None,             // No clipping - content scrolls off screen edges
        DisableOffscreen  // Disable GameObjects that are fully outside viewport
    }
}