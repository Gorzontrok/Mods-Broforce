using System.Collections.Generic;
using RocketLib.Menus.Elements;
using UnityEngine;

namespace RocketLib.Menus.Layout
{
    /// <summary>
    /// Container that arranges children horizontally from left to right
    /// </summary>
    public class HorizontalLayoutContainer : LayoutContainer
    {
        // Controls vertical alignment of children
        public VerticalAlignment ChildVerticalAlignment { get; set; } = VerticalAlignment.Center;

        public HorizontalLayoutContainer(string name = "HorizontalContainer") : base(name)
        {
        }

        protected override void ArrangeChildren()
        {
            if (Children.Count == 0) return;

            var childrenToPosition = this.GetChildrenToPosition();
            if (childrenToPosition.Count == 0) return;

            float availableWidth = ActualSize.x - (Padding * 2);
            float availableHeight = ActualSize.y - (Padding * 2);

            float totalSpacing = Spacing * Mathf.Max(0, childrenToPosition.Count - 1);

            // Phase 1: Calculate widths for all non-Fill children
            List<float> childWidths = new List<float>();
            List<int> fillChildIndices = new List<int>();
            float totalFixedWidth = 0;

            for (int i = 0; i < childrenToPosition.Count; i++)
            {
                var child = childrenToPosition[i];
                float width = 0;

                switch (child.WidthMode)
                {
                    case SizeMode.Fixed:
                        width = child.Width;
                        totalFixedWidth += width;
                        break;

                    case SizeMode.Percentage:
                        width = (child.Width / 100f) * availableWidth;
                        totalFixedWidth += width;
                        break;

                    case SizeMode.Auto:
                        width = child.GetPreferredWidth();
                        totalFixedWidth += width;
                        break;

                    case SizeMode.Fill:
                        fillChildIndices.Add(i);
                        break;
                }

                childWidths.Add(width);
            }

            // Phase 2: Calculate and distribute remaining space to Fill children
            float remainingWidth = availableWidth - totalFixedWidth - totalSpacing;

            if (fillChildIndices.Count > 0)
            {
                if (remainingWidth > 0)
                {
                    float fillWidth = remainingWidth / fillChildIndices.Count;
                    foreach (int index in fillChildIndices)
                    {
                        childWidths[index] = fillWidth;
                    }
                }
                else
                {
                    foreach (int index in fillChildIndices)
                    {
                        childWidths[index] = 0;
                    }
                }
            }

            // Phase 3: Position all children with calculated widths
            float containerLeft = ActualPosition.x - (ActualSize.x / 2);
            float currentX = containerLeft + Padding;

            for (int i = 0; i < childrenToPosition.Count; i++)
            {
                var child = childrenToPosition[i];
                float width = childWidths[i];

                // Apply width constraints
                if (child.MinSize.x > 0) width = Mathf.Max(width, child.MinSize.x);
                if (child.MaxSize.x > 0) width = Mathf.Min(width, child.MaxSize.x);

                // Calculate height based on mode
                float height = availableHeight;
                switch (child.HeightMode)
                {
                    case SizeMode.Fixed:
                        height = child.Height;
                        break;

                    case SizeMode.Percentage:
                        height = (child.Height / 100f) * availableHeight;
                        break;

                    case SizeMode.Auto:
                        height = child.GetPreferredHeight();
                        break;

                    case SizeMode.Fill:
                        // Already set to availableHeight
                        break;
                }

                // Apply height constraints
                if (child.MinSize.y > 0) height = Mathf.Max(height, child.MinSize.y);
                if (child.MaxSize.y > 0) height = Mathf.Min(height, child.MaxSize.y);

                // Calculate Y position based on alignment
                VerticalAlignment alignment = child.VerticalAlignmentOverride ?? ChildVerticalAlignment;
                float childY = ActualPosition.y; // Default center

                switch (alignment)
                {
                    case VerticalAlignment.Top:
                        childY = ActualPosition.y + (ActualSize.y / 2) - Padding - (height / 2);
                        break;
                    case VerticalAlignment.Center:
                        childY = ActualPosition.y;
                        break;
                    case VerticalAlignment.Bottom:
                        childY = ActualPosition.y - (ActualSize.y / 2) + Padding + (height / 2);
                        break;
                }

                // Set position (centered in X, aligned in Y based on setting)
                child.ActualPosition = new Vector2(
                    currentX + (width / 2),
                    childY
                );
                child.ActualSize = new Vector2(width, height);

                // Move to next position
                currentX += width + Spacing;
            }

            // Detect overflow
            DetectOverflow(childrenToPosition, totalFixedWidth, totalSpacing, availableWidth);
        }

        private void DetectOverflow(List<LayoutElement> childrenToPosition, float totalFixedWidth, float totalSpacing, float availableWidth)
        {
            if (childrenToPosition.Count == 0) return;

            // Get actual camera bounds dynamically
            float screenTop, screenBottom, screenLeft, screenRight;
            GetCameraBounds(out screenTop, out screenBottom, out screenLeft, out screenRight);

            // Thresholds for different warning levels
            const float WARNING_THRESHOLD = 50f;  // Significant overflow
            const float DEBUG_THRESHOLD = 20f;    // Minor overflow

            // Calculate container overflow
            float totalRequired = totalFixedWidth + totalSpacing;
            float containerOverflow = totalRequired - availableWidth;

            // Check for off-screen elements (CRITICAL)
            List<string> offScreenElements = new List<string>();
            foreach (var child in childrenToPosition)
            {
                float childLeft = child.ActualPosition.x - (child.ActualSize.x / 2);
                float childRight = child.ActualPosition.x + (child.ActualSize.x / 2);
                float childTop = child.ActualPosition.y + (child.ActualSize.y / 2);
                float childBottom = child.ActualPosition.y - (child.ActualSize.y / 2);

                if (childLeft < screenLeft || childRight > screenRight ||
                    childTop > screenTop || childBottom < screenBottom)
                {
                    string childName = string.IsNullOrEmpty(child.Name) ? child.GetType().Name : child.Name;
                    string issue = "";
                    if (childLeft < screenLeft) issue = $"left {childLeft:F0} < screen {screenLeft:F0}";
                    if (childRight > screenRight)
                    {
                        if (!string.IsNullOrEmpty(issue)) issue += ", ";
                        issue += $"right {childRight:F0} > screen {screenRight:F0}";
                    }
                    if (childTop > screenTop)
                    {
                        if (!string.IsNullOrEmpty(issue)) issue += ", ";
                        issue += $"top {childTop:F0} > screen {screenTop:F0}";
                    }
                    if (childBottom < screenBottom)
                    {
                        if (!string.IsNullOrEmpty(issue)) issue += ", ";
                        issue += $"bottom {childBottom:F0} < screen {screenBottom:F0}";
                    }
                    offScreenElements.Add($"{child.GetType().Name} '{childName}' ({issue})");
                }
            }

            // Log based on severity
            if (offScreenElements.Count > 0)
            {
                // CRITICAL: Elements are off-screen
                RocketLib.Main.logger.Error($"HorizontalLayoutContainer '{Name}' has OFF-SCREEN elements!");
                foreach (string element in offScreenElements)
                {
                    RocketLib.Main.logger.Error($"  - {element}");
                }
            }
            else if (containerOverflow > WARNING_THRESHOLD)
            {
                // SIGNIFICANT: Large overflow that may indicate a design problem
                RocketLib.Main.logger.Warning($"HorizontalLayoutContainer '{Name}' overflow: {containerOverflow:F0}px");
                RocketLib.Main.logger.Warning($"  - Required: {totalRequired:F0}px, Available: {availableWidth:F0}px");
            }
            else if (containerOverflow > DEBUG_THRESHOLD)
            {
                // MINOR: Small overflow, only log if debug is enabled
                RocketLib.Main.logger.Debug($"HorizontalLayoutContainer '{Name}' minor overflow: {containerOverflow:F0}px");
            }
            // SILENT: Overflow < 20px is normal and ignored
        }
    }
}
