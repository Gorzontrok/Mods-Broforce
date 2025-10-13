using RocketLib.Menus.Utilities;
using UnityEngine;

namespace RocketLib.Menus.Elements
{

    public class TextElement : LayoutElement
    {
        private string _text = "Text";
        public string Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    boundsAvailable = false;  // Force width recalculation
                    visualNeedsUpdate = true;
                }
            }
        }

        private Color _textColor = Color.white;
        public Color TextColor
        {
            get { return _textColor; }
            set
            {
                if (_textColor != value)
                {
                    _textColor = value;
                    visualNeedsUpdate = true;
                }
            }
        }

        private float _fontSize = 20f;
        public float FontSize
        {
            get { return _fontSize; }
            set
            {
                if (_fontSize != value)
                {
                    _fontSize = value;
                    boundsAvailable = false;  // Force width recalculation
                    visualNeedsUpdate = true;
                }
            }
        }

        private BroforceFont _font = BroforceFont.Hudson;
        public BroforceFont Font
        {
            get { return _font; }
            set
            {
                if (_font != value)
                {
                    _font = value;
                    boundsAvailable = false;  // Force width recalculation
                    visualNeedsUpdate = true;
                }
            }
        }

        // Using base class gameObject field instead
        private TextMesh textMesh;

        // Auto-width caching
        private float cachedAutoWidth = 100f;
        private bool boundsAvailable = false;

        // Track when visual properties need updating
        private bool visualNeedsUpdate = true;

        public TextElement(string name) : base(name)
        {
            // Fields are already initialized with default values
            IsFocusable = false;

            // Default size
            WidthMode = Layout.SizeMode.Fill;
            HeightMode = Layout.SizeMode.Fixed;
            Width = 1f;
            Height = 30f;
        }

        public override void Render()
        {
            // Create GameObject if needed
            if (gameObject == null)
            {
                CreateTextGameObject();
            }

            // Handle visibility for existing GameObject
            if (gameObject != null)
            {
                if (!IsVisible)
                {
                    // Hide the GameObject when not visible
                    gameObject.SetActive(false);
                    return;
                }

                // Update position based on ActualPosition/ActualSize set by parent
                // ActualPosition is already the center of the element
                gameObject.transform.localPosition = new Vector3(ActualPosition.x, ActualPosition.y, -1f);
                gameObject.transform.localScale = Vector3.one;

                // Update visual properties only when changed
                if (textMesh != null && visualNeedsUpdate)
                {
                    textMesh.text = Text;
                    textMesh.color = TextColor;
                    FontManager.ApplyFont(textMesh, Font, FontSize);
                    visualNeedsUpdate = false;
                }

                // Ensure GameObject is active when visible
                gameObject.SetActive(true);
            }
        }

        private void CreateTextGameObject()
        {
            gameObject = new GameObject(Name);
            gameObject.layer = LayerMask.NameToLayer("UI");

            // Parent to menu if we have the reference
            if (menuTransform != null)
            {
                gameObject.transform.SetParent(menuTransform, false);
            }

            var meshRenderer = gameObject.AddComponent<MeshRenderer>();
            textMesh = gameObject.AddComponent<TextMesh>();

            // Basic text setup
            textMesh.text = Text;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
            textMesh.color = TextColor;

            // Apply font using FontManager
            FontManager.ApplyFont(textMesh, Font, FontSize);

            meshRenderer.sortingOrder = -10;
        }

        private void MeasureAutoWidth()
        {
            if (boundsAvailable)
                return;

            // Use FontManager for measurement
            cachedAutoWidth = FontManager.CalculateTextWidth(Font, Text, FontSize);

            boundsAvailable = true;
        }



        public override float GetPreferredWidth()
        {
            if (WidthMode == Layout.SizeMode.Auto)
            {
                // Always measure if we haven't yet, even without a GameObject
                if (!boundsAvailable)
                {
                    MeasureAutoWidth();
                }
                return cachedAutoWidth;
            }
            return base.GetPreferredWidth();
        }

        public override float GetPreferredHeight()
        {
            if (HeightMode == Layout.SizeMode.Auto)
            {
                return MeasureTextHeight();
            }
            return base.GetPreferredHeight();
        }

        private float MeasureTextHeight()
        {
            // Use FontManager for height measurement
            return FontManager.CalculateTextHeight(Font, Text, FontSize);
        }

        public override void Cleanup()
        {
            textMesh = null;
            base.Cleanup();  // Base class handles GameObject destruction
        }


    }
}
