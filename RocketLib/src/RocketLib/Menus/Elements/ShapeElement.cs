using UnityEngine;

namespace RocketLib.Menus.Elements
{
    /// <summary>
    /// A LayoutElement that displays basic runtime-created shapes
    /// </summary>
    public class ShapeElement : LayoutElement
    {
        private GameObject spriteGO;
        private SpriteRenderer spriteRenderer;

        private Sprite _sprite;
        public Sprite Sprite
        {
            get => _sprite;
            set
            {
                _sprite = value;
                UpdateSprite();
            }
        }

        public ShapeScaleMode ScaleMode { get; set; } = ShapeScaleMode.Fit;
        public Color Tint { get; set; } = Color.white;

        public ShapeElement(string name) : base(name)
        {
            IsFocusable = false;
        }

        public override void Render()
        {
            try
            {
                // Ensure GameObject exists
                if (gameObject == null)
                {
                    gameObject = new GameObject(Name);
                    if (menuTransform != null)
                    {
                        gameObject.transform.SetParentAndResetScale(menuTransform);
                    }
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

                    // Position the element
                    gameObject.transform.position = ActualPosition;

                    // Create sprite GameObject if needed
                    if (spriteGO == null)
                    {
                        spriteGO = new GameObject("Sprite");
                        spriteGO.transform.SetParentAndResetScale(gameObject.transform, Vector3.zero);

                        spriteRenderer = spriteGO.AddComponent<SpriteRenderer>();
                        spriteRenderer.sortingOrder = 100; // Ensure it renders above background

                        UpdateSprite();
                    }

                    // Ensure GameObject is active when visible
                    gameObject.SetActive(true);
                }
            }
            catch (System.Exception)
            {
            }
        }

        private void UpdateSprite()
        {
            if (spriteRenderer != null && _sprite != null)
            {
                spriteRenderer.sprite = _sprite;
                spriteRenderer.color = Tint;

                // Scale sprite to fit the element size
                if (_sprite != null)
                {
                    ApplyScaling();
                }
            }
        }

        private void ApplyScaling()
        {
            if (spriteGO == null || _sprite == null) return;

            float spriteWidth = _sprite.bounds.size.x;
            float spriteHeight = _sprite.bounds.size.y;

            if (spriteWidth <= 0 || spriteHeight <= 0) return;

            float targetWidth = ActualSize.x;
            float targetHeight = ActualSize.y;

            float scaleX = 1f;
            float scaleY = 1f;

            switch (ScaleMode)
            {
                case ShapeScaleMode.Stretch:
                    // Stretch to fill entire size
                    scaleX = targetWidth / spriteWidth;
                    scaleY = targetHeight / spriteHeight;
                    break;

                case ShapeScaleMode.Fit:
                    // Scale uniformly to fit within bounds
                    float fitScale = Mathf.Min(targetWidth / spriteWidth, targetHeight / spriteHeight);
                    scaleX = scaleY = fitScale;
                    break;

                case ShapeScaleMode.Fill:
                    // Scale uniformly to fill bounds (may crop)
                    float fillScale = Mathf.Max(targetWidth / spriteWidth, targetHeight / spriteHeight);
                    scaleX = scaleY = fillScale;
                    break;

                case ShapeScaleMode.None:
                    // Use original sprite size
                    scaleX = scaleY = 1f;
                    break;
            }

            spriteGO.transform.localScale = new Vector3(scaleX, scaleY, 1f);
        }

        public override void UpdateLayout()
        {
            base.UpdateLayout();

            // Update sprite scaling when layout changes
            if (spriteGO != null && _sprite != null)
            {
                ApplyScaling();
            }
        }



        public override void Cleanup()
        {
            if (spriteGO != null)
            {
                UnityEngine.Object.Destroy(spriteGO);
                spriteGO = null;
                spriteRenderer = null;
            }

            base.Cleanup();
        }

    }

    public enum ShapeScaleMode
    {
        None,     // Original size
        Stretch,  // Stretch to fill (distorts)
        Fit,      // Scale uniformly to fit within bounds
        Fill      // Scale uniformly to fill bounds (may crop)
    }
}