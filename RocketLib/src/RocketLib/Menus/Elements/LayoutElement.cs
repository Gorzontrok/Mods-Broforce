using System;
using System.Collections.Generic;
using RocketLib.Menus.Layout;
using UnityEngine;

namespace RocketLib.Menus.Elements
{
    public abstract class LayoutElement
    {
        private readonly string id;
        public string Id { get { return id; } }
        public string Name { get; set; }

        public LayoutContainer Parent { get; set; }

        // Size specification using new SizeMode API
        public Layout.SizeMode WidthMode { get; set; }
        public Layout.SizeMode HeightMode { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        // Constraints
        public Vector2 MinSize { get; set; }
        public Vector2 MaxSize { get; set; }

        // Alignment overrides (optional)
        public HorizontalAlignment? HorizontalAlignmentOverride { get; set; }
        public VerticalAlignment? VerticalAlignmentOverride { get; set; }

        public Vector2 ActualPosition { get; set; }
        public Vector2 ActualSize { get; set; }

        // Visibility and interaction
        /// <summary>
        /// This determines whether the element and its children are visible on screen.
        /// </summary>
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                this.OnVisibilityChanged();
            }
        }
        private bool _isVisible;
        /// <summary>
        /// This determines whether the element and its children should take up space and be positioned by Layout Containers
        /// </summary>
        public bool IsPositioned
        {
            get => _isPositioned;
            set
            {
                _isPositioned = value;
                this.OnIsPositionedChanged();
            }
        }
        private bool _isPositioned;
        public bool IsVisibleAndPositioned
        {
            get => IsVisible && IsPositioned;
            set => IsVisible = IsPositioned = value;
        }

        public bool IsEnabled { get; set; }
        public bool IsFocusable { get; set; }
        public bool IsFocused { get; set; }

        // User data
        public object Tag { get; set; }

        // Set by child if it creates one
        protected GameObject gameObject;
        // Reference to parent menu
        protected Transform menuTransform;

        protected LayoutElement(string name)
        {
            id = Guid.NewGuid().ToString();
            Name = name;

            // Default to fill parent
            WidthMode = Layout.SizeMode.Fill;
            HeightMode = Layout.SizeMode.Fill;
            Width = 1f;
            Height = 1f;

            MinSize = Vector2.zero;
            MaxSize = Vector2.zero;

            _isVisible = true;
            _isPositioned = true;
            IsEnabled = true;
            IsFocusable = false;
            IsFocused = false;
        }

        public virtual void UpdateLayout()
        {
            // Base implementation does nothing - parent controls everything
            // Only root containers handle their own sizing
        }

        public virtual float GetPreferredWidth()
        {
            switch (WidthMode)
            {
                case Layout.SizeMode.Fixed:
                    return Width;
                case Layout.SizeMode.Percentage:
                    return Parent != null ? (Width / 100f) * Parent.ActualSize.x : Width;
                case Layout.SizeMode.Auto:
                    return Width; // Default for elements that don't implement Auto
                case Layout.SizeMode.Fill:
                default:
                    return 0; // Fill takes remaining space
            }
        }

        public virtual float GetPreferredHeight()
        {
            switch (HeightMode)
            {
                case Layout.SizeMode.Fixed:
                    return Height;
                case Layout.SizeMode.Percentage:
                    return Parent != null ? (Height / 100f) * Parent.ActualSize.y : Height;
                case Layout.SizeMode.Auto:
                    return Height; // Default for elements that don't implement Auto
                case Layout.SizeMode.Fill:
                default:
                    return 0; // Fill takes remaining space
            }
        }

        public virtual void SetMenuTransform(Transform transform)
        {
            menuTransform = transform;
            if (gameObject != null && menuTransform != null)
            {
                gameObject.transform.SetParentAndResetScale(menuTransform);
            }
        }

        public abstract void Render();

        public virtual void Cleanup()
        {
            // Cleanup GameObject if it exists
            if (gameObject != null)
            {
                UnityEngine.Object.Destroy(gameObject);
                gameObject = null;
            }
        }

        public virtual bool ContainsPoint(Vector2 point)
        {
            float halfWidth = ActualSize.x / 2;
            float halfHeight = ActualSize.y / 2;

            return point.x >= ActualPosition.x - halfWidth &&
                   point.x <= ActualPosition.x + halfWidth &&
                   point.y >= ActualPosition.y - halfHeight &&
                   point.y <= ActualPosition.y + halfHeight;
        }

        public virtual LayoutElement GetElementAt(Vector2 position)
        {
            if (!IsVisible || !IsEnabled) return null;

            if (IsFocusable && ContainsPoint(position))
            {
                return this;
            }

            return null;
        }

        public virtual List<LayoutElement> GetFocusableElements()
        {
            var elements = new List<LayoutElement>();

            if (!IsEnabled) return elements;

            if (IsFocusable)
            {
                elements.Add(this);
            }

            return elements;
        }

        public virtual Rect GetBounds()
        {
            return new Rect(
                ActualPosition.x - (ActualSize.x / 2),
                ActualPosition.y - (ActualSize.y / 2),
                ActualSize.x,
                ActualSize.y
            );
        }

        public virtual void OnFocusGained()
        {
            IsFocused = true;
        }

        public virtual void OnFocusLost()
        {
            IsFocused = false;
        }

        public virtual void OnActivated()
        {
        }

        public virtual void OnVisibilityChanged()
        {
        }

        public virtual void OnIsPositionedChanged()
        {

        }

        public LayoutElement FindById(string id)
        {
            if (Id == id) return this;

            var container = this as LayoutContainer;
            if (container != null)
            {
                foreach (var child in container.Children)
                {
                    var found = child.FindById(id);
                    if (found != null) return found;
                }
            }

            return null;
        }

        public LayoutElement FindByName(string name)
        {
            if (Name == name) return this;

            var container = this as LayoutContainer;
            if (container != null)
            {
                foreach (var child in container.Children)
                {
                    var found = child.FindByName(name);
                    if (found != null) return found;
                }
            }

            return null;
        }

        public T FindByType<T>() where T : LayoutElement
        {
            if (this is T) return (T)this;

            var container = this as LayoutContainer;
            if (container != null)
            {
                foreach (var child in container.Children)
                {
                    var found = child.FindByType<T>();
                    if (found != null) return found;
                }
            }

            return null;
        }

        public List<T> FindAllByType<T>() where T : LayoutElement
        {
            var results = new List<T>();

            if (this is T) results.Add((T)this);

            var container = this as LayoutContainer;
            if (container != null)
            {
                foreach (var child in container.Children)
                {
                    results.AddRange(child.FindAllByType<T>());
                }
            }

            return results;
        }

    }
}
