using System.Collections.Generic;
using RocketLib.Menus.Elements;
using UnityEngine;

namespace RocketLib.Menus.Layout
{
    /// <summary>
    /// Container that stacks children vertically from top to bottom
    /// </summary>
    public class VerticalLayoutContainer : LayoutContainer
    {
        // Controls horizontal alignment of children
        public HorizontalAlignment ChildHorizontalAlignment { get; set; } = HorizontalAlignment.Left;

        public VerticalLayoutContainer(string name = "VerticalContainer") : base(name)
        {
        }

        protected override void ArrangeChildren()
        {
            if (Children.Count == 0) return;

            var childrenToPosition = this.GetChildrenToPosition();
            if (childrenToPosition.Count == 0) return;

            float availableHeight = ActualSize.y - (Padding * 2);
            float availableWidth = ActualSize.x - (Padding * 2);
            float totalSpacing = Spacing * (childrenToPosition.Count - 1);

            // Phase 1: Calculate heights for all non-Fill children
            List<float> childHeights = new List<float>();
            List<int> fillChildIndices = new List<int>();
            float totalFixedHeight = 0;

            for (int i = 0; i < childrenToPosition.Count; i++)
            {
                var child = childrenToPosition[i];
                float height = 0;

                switch (child.HeightMode)
                {
                    case SizeMode.Fixed:
                        height = child.Height;
                        totalFixedHeight += height;
                        break;

                    case SizeMode.Percentage:
                        height = (child.Height / 100f) * availableHeight;
                        totalFixedHeight += height;
                        break;

                    case SizeMode.Auto:
                        height = child.GetPreferredHeight();
                        totalFixedHeight += height;
                        break;

                    case SizeMode.Fill:
                        fillChildIndices.Add(i);
                        break;
                }

                childHeights.Add(height);
            }

            // Phase 2: Calculate and distribute remaining space to Fill children
            float remainingHeight = availableHeight - totalFixedHeight - totalSpacing;

            if (fillChildIndices.Count > 0)
            {
                if (remainingHeight > 0)
                {
                    float fillHeight = remainingHeight / fillChildIndices.Count;
                    foreach (int index in fillChildIndices)
                    {
                        childHeights[index] = fillHeight;
                    }
                }
                else
                {
                    // No space remaining, Fill children get 0 height
                    foreach (int index in fillChildIndices)
                    {
                        childHeights[index] = 0;
                    }
                }
            }

            // Phase 3: Position all children with calculated heights
            float currentY = ActualPosition.y + (ActualSize.y / 2) - Padding;

            for (int i = 0; i < childrenToPosition.Count; i++)
            {
                var child = childrenToPosition[i];
                float height = childHeights[i];

                // Apply min/max constraints
                if (child.MinSize.y > 0) height = Mathf.Max(height, child.MinSize.y);
                if (child.MaxSize.y > 0) height = Mathf.Min(height, child.MaxSize.y);

                // Calculate width
                float width = CalculateChildWidth(child, availableWidth);

                // Apply width constraints
                if (child.MinSize.x > 0) width = Mathf.Max(width, child.MinSize.x);
                if (child.MaxSize.x > 0) width = Mathf.Min(width, child.MaxSize.x);

                // Calculate X position based on alignment
                HorizontalAlignment alignment = child.HorizontalAlignmentOverride ?? ChildHorizontalAlignment;
                float childX = ActualPosition.x; // Default center

                switch (alignment)
                {
                    case HorizontalAlignment.Left:
                        childX = ActualPosition.x - (ActualSize.x / 2) + Padding + (width / 2);
                        break;
                    case HorizontalAlignment.Center:
                        childX = ActualPosition.x;
                        break;
                    case HorizontalAlignment.Right:
                        childX = ActualPosition.x + (ActualSize.x / 2) - Padding - (width / 2);
                        break;
                }

                // Set actual position (center of element)
                child.ActualPosition = new Vector2(childX, currentY - (height / 2));
                child.ActualSize = new Vector2(width, height);

                // Move to next position
                currentY -= (height + Spacing);
            }

            // Detect overflow
            DetectOverflow(childrenToPosition, totalFixedHeight, totalSpacing, availableHeight);
        }

        private void DetectOverflow(List<LayoutElement> childrenToPosition, float totalFixedHeight, float totalSpacing, float availableHeight)
        {
            base.DetectOverflow(
                containerType: "VerticalLayoutContainer",
                dimension: "Height",
                children: childrenToPosition,
                availableSpace: availableHeight,
                totalRequired: totalFixedHeight + totalSpacing,
                totalSpacing: totalSpacing,
                isVertical: true
            );
        }

        private float CalculateChildWidth(LayoutElement child, float availableWidth)
        {
            switch (child.WidthMode)
            {
                case SizeMode.Fixed:
                    return child.Width;

                case SizeMode.Percentage:
                    return (child.Width / 100f) * availableWidth;

                case SizeMode.Fill:
                    return availableWidth;

                case SizeMode.Auto:
                    return child.GetPreferredWidth();

                default:
                    return availableWidth;
            }
        }
    }
}
