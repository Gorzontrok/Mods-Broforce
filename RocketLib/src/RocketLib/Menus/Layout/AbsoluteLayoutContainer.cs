using System.Collections.Generic;
using System.Linq;
using RocketLib.Menus.Elements;
using UnityEngine;

namespace RocketLib.Menus.Layout
{
    /// <summary>
    /// Container that positions children at specific absolute positions
    /// </summary>
    public class AbsoluteLayoutContainer : LayoutContainer
    {
        // Store absolute positions for each child
        private readonly Dictionary<LayoutElement, Vector2> childPositions = new Dictionary<LayoutElement, Vector2>();

        public AbsoluteLayoutContainer(string name = "AbsoluteContainer") : base(name)
        {
        }

        /// <summary>
        /// Sets the absolute position for a child element relative to container center
        /// </summary>
        public void SetChildPosition(LayoutElement child, Vector2 position)
        {
            if (Children.Contains(child))
            {
                childPositions[child] = position;
            }
        }

        /// <summary>
        /// Sets the absolute position for a child element
        /// </summary>
        public void SetChildPosition(LayoutElement child, float x, float y)
        {
            SetChildPosition(child, new Vector2(x, y));
        }

        protected override void ArrangeChildren()
        {
            if (Children.Count == 0) return;

            // Position ALL children regardless of visibility (per design Rule 1)
            var childrenToPosition = Children.ToList();
            if (childrenToPosition.Count == 0) return;

            float availableWidth = ActualSize.x - (Padding * 2);
            float availableHeight = ActualSize.y - (Padding * 2);

            foreach (var child in childrenToPosition)
            {
                // Get absolute position if set, otherwise use (0, 0)
                Vector2 relativePos = Vector2.zero;
                if (childPositions.ContainsKey(child))
                {
                    relativePos = childPositions[child];
                }

                // Calculate child size
                float childWidth = availableWidth;
                float childHeight = availableHeight;

                switch (child.WidthMode)
                {
                    case SizeMode.Fixed:
                        childWidth = child.Width;
                        break;
                    case SizeMode.Percentage:
                        childWidth = child.Width * availableWidth;
                        break;

                    case SizeMode.Fill:
                        // Already set to availableWidth
                        break;
                }

                switch (child.HeightMode)
                {
                    case SizeMode.Fixed:
                        childHeight = child.Height;
                        break;
                    case SizeMode.Percentage:
                        childHeight = child.Height * availableHeight;
                        break;

                    case SizeMode.Fill:
                        // Already set to availableHeight
                        break;
                }

                // Apply constraints
                if (child.MinSize.x > 0) childWidth = Mathf.Max(childWidth, child.MinSize.x);
                if (child.MaxSize.x > 0) childWidth = Mathf.Min(childWidth, child.MaxSize.x);
                if (child.MinSize.y > 0) childHeight = Mathf.Max(childHeight, child.MinSize.y);
                if (child.MaxSize.y > 0) childHeight = Mathf.Min(childHeight, child.MaxSize.y);

                // Set actual position (relative to container center)
                child.ActualPosition = ActualPosition + relativePos;
                child.ActualSize = new Vector2(childWidth, childHeight);

            }

            DetectOutOfBounds(childrenToPosition);
        }

        private void DetectOutOfBounds(List<LayoutElement> childrenToPosition)
        {
            if (childrenToPosition.Count == 0) return;

            // Get actual camera bounds dynamically
            float screenTop, screenBottom, screenLeft, screenRight;
            GetCameraBounds(out screenTop, out screenBottom, out screenLeft, out screenRight);

            // Container bounds
            float containerLeft = ActualPosition.x - (ActualSize.x / 2);
            float containerRight = ActualPosition.x + (ActualSize.x / 2);
            float containerTop = ActualPosition.y + (ActualSize.y / 2);
            float containerBottom = ActualPosition.y - (ActualSize.y / 2);

            List<string> offScreenElements = new List<string>();
            List<string> containerOverflow = new List<string>();

            foreach (var child in childrenToPosition)
            {
                float childLeft = child.ActualPosition.x - (child.ActualSize.x / 2);
                float childRight = child.ActualPosition.x + (child.ActualSize.x / 2);
                float childTop = child.ActualPosition.y + (child.ActualSize.y / 2);
                float childBottom = child.ActualPosition.y - (child.ActualSize.y / 2);

                string childName = string.IsNullOrEmpty(child.Name) ? child.GetType().Name : child.Name;

                // Check for off-screen (CRITICAL)
                if (childTop > screenTop || childBottom < screenBottom ||
                    childLeft < screenLeft || childRight > screenRight)
                {
                    string issue = "";
                    if (childTop > screenTop) issue = $"top {childTop:F0} > screen {screenTop:F0}";
                    if (childBottom < screenBottom)
                    {
                        if (!string.IsNullOrEmpty(issue)) issue += ", ";
                        issue += $"bottom {childBottom:F0} < screen {screenBottom:F0}";
                    }
                    if (childLeft < screenLeft)
                    {
                        if (!string.IsNullOrEmpty(issue)) issue += ", ";
                        issue += $"left {childLeft:F0} < screen {screenLeft:F0}";
                    }
                    if (childRight > screenRight)
                    {
                        if (!string.IsNullOrEmpty(issue)) issue += ", ";
                        issue += $"right {childRight:F0} > screen {screenRight:F0}";
                    }
                    offScreenElements.Add($"{child.GetType().Name} '{childName}' ({issue})");
                }
                // Check for container overflow (less critical)
                else
                {
                    float overflow = 0;
                    string direction = "";

                    if (childLeft < containerLeft)
                    {
                        overflow = Mathf.Max(overflow, containerLeft - childLeft);
                        direction += "left ";
                    }
                    if (childRight > containerRight)
                    {
                        overflow = Mathf.Max(overflow, childRight - containerRight);
                        direction += "right ";
                    }
                    if (childTop > containerTop)
                    {
                        overflow = Mathf.Max(overflow, childTop - containerTop);
                        direction += "top ";
                    }
                    if (childBottom < containerBottom)
                    {
                        overflow = Mathf.Max(overflow, containerBottom - childBottom);
                        direction += "bottom ";
                    }

                    if (overflow > 50f)
                    {
                        containerOverflow.Add($"{child.GetType().Name} '{childName}' extends {overflow:F0}px beyond container ({direction.Trim()})");
                    }
                    else if (overflow > 20f)
                    {
                        // Only log if debug enabled
                        RocketLib.Main.logger.Debug($"[DEBUG] AbsoluteLayoutContainer '{Name}': {child.GetType().Name} '{childName}' minor overflow {overflow:F0}px");
                    }
                }
            }

            // Log based on severity
            if (offScreenElements.Count > 0)
            {
                // CRITICAL: Elements are off-screen
                RocketLib.Main.logger.Error($"[ERROR] AbsoluteLayoutContainer '{Name}' has OFF-SCREEN elements!");
                foreach (string element in offScreenElements)
                {
                    RocketLib.Main.logger.Error($"  - {element}");
                }
            }

            if (containerOverflow.Count > 0)
            {
                // WARNING: Significant container overflow
                RocketLib.Main.logger.Warning($"[WARNING] AbsoluteLayoutContainer '{Name}' has elements with significant overflow!");
                foreach (string warning in containerOverflow)
                {
                    RocketLib.Main.logger.Warning($"  - {warning}");
                }
            }
        }

        public void AddChild(LayoutElement element, Vector2? position = null)
        {
            base.AddChild(element);

            if (position.HasValue)
            {
                SetChildPosition(element, position.Value);
            }
        }
    }
}