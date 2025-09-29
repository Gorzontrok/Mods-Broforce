using System;
using System.Collections.Generic;
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

        public override void Render()
        {
            foreach (var child in Children)
            {
                child.Render();
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
