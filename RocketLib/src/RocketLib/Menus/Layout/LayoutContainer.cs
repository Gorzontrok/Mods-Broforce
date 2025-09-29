using System;
using System.Collections.Generic;
using System.Linq;
using RocketLib.Menus.Elements;
using UnityEngine;

namespace RocketLib.Menus.Layout
{
    public enum LayoutMode
    {
        Vertical,    // Stack top to bottom
        Horizontal,  // Stack left to right
        Grid,        // Rows and columns
        Absolute     // Manual positioning
    }

    /// <summary>
    /// Abstract base class for all layout containers.
    /// Each layout type is implemented as a separate subclass.
    /// </summary>
    public abstract class LayoutContainer : LayoutElement
    {
        public List<LayoutElement> Children { get; }
        public float Padding { get; set; }     // World units
        public float Spacing { get; set; }     // World units between children

        protected LayoutContainer(string name) : base(name)
        {
            Children = new List<LayoutElement>();
            Padding = 10f;
            Spacing = 5f;

            IsFocusable = false;
        }

        public void AddChild(LayoutElement child)
        {
            if (child != null && !Children.Contains(child))
            {
                Children.Add(child);
                child.Parent = this;
            }
        }

        public void RemoveChild(LayoutElement child)
        {
            if (child != null && Children.Remove(child))
            {
                child.Parent = null;
                child.Cleanup();
            }
        }

        public void ClearChildren()
        {
            foreach (var child in Children)
            {
                child.Parent = null;
                child.Cleanup();
            }
            Children.Clear();
        }

        public override void UpdateLayout()
        {
            // For root container, set size based on camera
            if (Parent == null)
            {
                Camera mainCamera = Camera.main;
                if (mainCamera != null && mainCamera.orthographic)
                {
                    float cameraHeight = mainCamera.orthographicSize * 2f;
                    float cameraWidth = cameraHeight * mainCamera.aspect;

                    ActualSize = new Vector2(cameraWidth, cameraHeight);
                    ActualPosition = Vector2.zero; // Center of camera
                }
            }

            // Let subclass arrange children
            ArrangeChildren();

            // Pass menu Transform to all children if we have it
            if (menuTransform != null)
            {
                foreach (var child in Children)
                {
                    child.SetMenuTransform(menuTransform);
                }
            }

            // Update all children recursively
            foreach (var child in Children)
            {
                if (child.IsVisible)
                {
                    child.UpdateLayout();
                }
            }
        }

        /// <summary>
        /// Abstract method for subclasses to implement their specific layout algorithm
        /// </summary>
        protected abstract void ArrangeChildren();

        /// <summary>
        /// Gets the current camera screen bounds. Used for overflow detection.
        /// </summary>
        protected void GetCameraBounds(out float top, out float bottom, out float left, out float right)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                // Fallback to approximate values if camera not available
                top = 160f;
                bottom = -160f;
                left = -270f;
                right = 270f;
                return;
            }

            float cameraHeight = mainCamera.orthographicSize * 2f;
            float cameraWidth = cameraHeight * mainCamera.aspect;
            top = mainCamera.orthographicSize;
            bottom = -mainCamera.orthographicSize;
            left = -(cameraWidth / 2f);
            right = cameraWidth / 2f;
        }

        protected virtual void DetectOverflow(
            string containerType,
            string dimension,
            List<LayoutElement> children,
            float availableSpace,
            float totalRequired,
            float totalSpacing,
            bool isVertical)
        {
            // Only run debug output if the active menu has debug enabled
            var activeMenu = Core.FlexMenu.activeMenu;
            if (activeMenu == null || !activeMenu.EnableDebugOutput) return;
            if (children.Count == 0) return;

            // Get actual camera bounds dynamically
            float screenTop, screenBottom, screenLeft, screenRight;
            GetCameraBounds(out screenTop, out screenBottom, out screenLeft, out screenRight);

            // Calculate overflow
            float overflow = totalRequired - availableSpace;

            // Build detailed space analysis
            RocketLib.Main.logger.Log($"[FlexMenu Debug] Space Analysis for '{Name}':");
            RocketLib.Main.logger.Log($"  Container: {containerType}");
            RocketLib.Main.logger.Log($"  Available {dimension}: {availableSpace:F1}px");
            RocketLib.Main.logger.Log($"  Padding: {Padding * 2:F1}px ({Padding:F1}px each side)");
            RocketLib.Main.logger.Log($"  Total Spacing: {totalSpacing:F1}px");
            RocketLib.Main.logger.Log("");
            RocketLib.Main.logger.Log($"  Child Distribution:");

            int fillCount = 0;
            
            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];
                string childName = string.IsNullOrEmpty(child.Name) ? child.GetType().Name : child.Name;
                SizeMode sizeMode = isVertical ? child.HeightMode : child.WidthMode;
                float childSize = isVertical ? child.ActualSize.y : child.ActualSize.x;
                
                if (sizeMode == SizeMode.Fill)
                {
                    fillCount++;
                    RocketLib.Main.logger.Log($"    {i + 1}. {child.GetType().Name} '{childName}' (Fill): {childSize:F1}px allocated");
                }
                else
                {
                    RocketLib.Main.logger.Log($"    {i + 1}. {child.GetType().Name} '{childName}' ({sizeMode}): {childSize:F1}px");
                }
                
                if (i < children.Count - 1)
                {
                    RocketLib.Main.logger.Log($"       + Spacing: {Spacing:F1}px");
                }
            }
            
            RocketLib.Main.logger.Log("");
            RocketLib.Main.logger.Log($"  Total Required: {totalRequired:F1}px");
            
            if (overflow > 1f)
            {
                RocketLib.Main.logger.Log($"  [!] OVERFLOW: {overflow:F1}px (needs {totalRequired:F1}px, has {availableSpace:F1}px)");
                
                if (fillCount > 0)
                {
                    float spaceAfterFixed = availableSpace - totalRequired;
                    if (spaceAfterFixed < 0)
                    {
                        RocketLib.Main.logger.Log($"  [!] Fixed content too large: {fillCount} Fill element(s) squeezed to 0px");
                        RocketLib.Main.logger.Log($"  [!] Need to reduce fixed content by {-spaceAfterFixed:F1}px or increase container size");
                    }
                    else if (fillCount > 1)
                    {
                        float spacePerFill = spaceAfterFixed / fillCount;
                        RocketLib.Main.logger.Log($"  [!] Note: {fillCount} Fill elements share remaining space ({spacePerFill:F1}px each)");
                    }
                }
                
                
            }
            else if (overflow > -1f)
            {
                RocketLib.Main.logger.Log($"  [OK] Fits perfectly ({totalRequired:F1}px used of {availableSpace:F1}px available)");
            }
            else
            {
                float remaining = availableSpace - totalRequired;
                RocketLib.Main.logger.Log($"  [OK] Fits with {remaining:F1}px remaining");
            }

            // Check for off-screen elements
            List<string> offScreenElements = new List<string>();
            foreach (var child in children)
            {
                if (isVertical)
                {
                    float childTop = child.ActualPosition.y + (child.ActualSize.y / 2);
                    float childBottom = child.ActualPosition.y - (child.ActualSize.y / 2);

                    if (childTop > screenTop || childBottom < screenBottom)
                    {
                        string childName = string.IsNullOrEmpty(child.Name) ? child.GetType().Name : child.Name;
                        string issue = "";
                        if (childTop > screenTop) issue = $"top {childTop:F0} > screen {screenTop:F0}";
                        if (childBottom < screenBottom)
                        {
                            if (!string.IsNullOrEmpty(issue)) issue += ", ";
                            issue += $"bottom {childBottom:F0} < screen {screenBottom:F0}";
                        }
                        offScreenElements.Add($"{child.GetType().Name} '{childName}' ({issue})");
                    }
                }
                else
                {
                    float childLeft = child.ActualPosition.x - (child.ActualSize.x / 2);
                    float childRight = child.ActualPosition.x + (child.ActualSize.x / 2);

                    if (childLeft < screenLeft || childRight > screenRight)
                    {
                        string childName = string.IsNullOrEmpty(child.Name) ? child.GetType().Name : child.Name;
                        string issue = "";
                        if (childLeft < screenLeft) issue = $"left {childLeft:F0} < screen {screenLeft:F0}";
                        if (childRight > screenRight)
                        {
                            if (!string.IsNullOrEmpty(issue)) issue += ", ";
                            issue += $"right {childRight:F0} > screen {screenRight:F0}";
                        }
                        offScreenElements.Add($"{child.GetType().Name} '{childName}' ({issue})");
                    }
                }
            }
            
            if (offScreenElements.Count > 0)
            {
                RocketLib.Main.logger.Log("");
                RocketLib.Main.logger.Log($"  [!!!] OFF-SCREEN ELEMENTS:");
                foreach (string element in offScreenElements)
                {
                    RocketLib.Main.logger.Log($"    - {element}");
                }
            }
            
            RocketLib.Main.logger.Log("");
        }

        public override void Render()
        {
            foreach (var child in Children)
            {
                child.Render();
            }
        }

        public override void OnVisibilityChanged()
        {
            if (Children == null)
            {
                return;
            }
            foreach (var child in Children)
            {
                child.IsVisible = this.IsVisible;
            }
        }

        public override void OnIsPositionedChanged()
        {
            if (Children == null)
            {
                return;
            }
            foreach (var child in Children)
            {
                child.IsVisible = this.IsVisible;
            }
        }

        public override void Cleanup()
        {
            ClearChildren();
            base.Cleanup();
        }

        public override LayoutElement GetElementAt(Vector2 position)
        {
            if (!IsVisible || !IsEnabled) return null;

            // Check children first (reverse order for top-most)
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                var found = Children[i].GetElementAt(position);
                if (found != null) return found;
            }

            // Containers themselves are not selectable
            return null;
        }

        public override List<LayoutElement> GetFocusableElements()
        {
            var elements = new List<LayoutElement>();

            // Note: We check IsEnabled but NOT IsVisible
            // This allows containers with invisible children (e.g., ScrollContainer) to return focusable elements
            if (!IsEnabled) return elements;

            foreach (var child in Children)
            {
                elements.AddRange(child.GetFocusableElements());
            }

            return elements;
        }

        public List<LayoutElement> GetChildrenToPosition()
        {
            return Children.Where(x => x.IsPositioned).ToList();
        }

        /// <summary>
        /// Factory method to create containers by type
        /// </summary>
        public static LayoutContainer Create(LayoutMode mode)
        {
            switch (mode)
            {
                case LayoutMode.Vertical:
                    return new VerticalLayoutContainer();

                case LayoutMode.Horizontal:
                    return new HorizontalLayoutContainer();

                case LayoutMode.Grid:
                    return new GridLayoutContainer();

                case LayoutMode.Absolute:
                    return new AbsoluteLayoutContainer();

                default:
                    throw new ArgumentException($"Unknown layout mode: {mode}");
            }
        }
    }
}
