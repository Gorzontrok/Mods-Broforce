using System;
using RocketLib.Menus.Layout;
using RocketLib.Menus.Utilities;
using UnityEngine;

namespace RocketLib.Menus.Elements
{
    /// <summary>
    /// UI element for displaying a bro in card format for grid view selection
    /// </summary>
    public class BroCard : LayoutElement
    {
        // Child GameObjects
        private GameObject avatarGO;
        private SpriteSM avatarSprite;
        private MeshRenderer avatarRenderer;

        private GameObject nameGO;
        private TextMesh nameText;

        private GameObject spawnIndicatorGO;
        private SpriteRenderer spawnIndicatorSprite;

        // Data properties
        public string BroName { get; set; }
        public Texture2D AvatarTexture { get; set; }
        public Material AvatarMaterial { get; set; }
        public bool IsLocked { get; set; }
        public bool IsSpawnEnabled { get; set; }

        // Text scaling
        public bool EnableDynamicTextScaling { get; set; } = true;
        public float MinimumTextScale { get; set; } = 0.6f;

        // Visual properties
        public Color LockedTintColor { get; set; } = new Color(0.15f, 0.15f, 0.15f, 1f);  // Much darker for locked
        public Color EnabledSpawnColor { get; set; } = new Color(0.3f, 1f, 0.3f, 1f);  // Bright green
        public Color DisabledSpawnColor { get; set; } = new Color(0.8f, 0.3f, 0.3f, 1f);  // Muted red/orange
        public Color LockedSpawnColor { get; set; } = new Color(0.4f, 0.4f, 0.4f, 0.6f);  // Dark gray, semi-transparent
        public float HoverBrightnessMultiplier { get; set; } = 1.2f;

        // Events
        public Action OnClick { get; set; }
        public Action OnHover { get; set; }

        // Layout configuration
        private const float AVATAR_SIZE_RATIO = 1.0f;  // Avatar takes 100% of card width
        private const float NAME_HEIGHT_RATIO = 0.2f;  // Name takes 20% of card height
        private const float INDICATOR_SIZE_RATIO = 0.08f; // Indicator is 8% of card size
        private const float NAME_FONT_SIZE = 4f; // Font size for bro names

        public BroCard(string broName) : base($"BroCard_{broName}")
        {
            IsFocusable = true;

            // Default sizing
            WidthMode = SizeMode.Fixed;
            HeightMode = SizeMode.Fixed;
            Width = 64f;
            Height = 80f;

            BroName = broName;
        }

        public override void Render()
        {
            // Create main GameObject if needed
            if (gameObject == null)
            {
                CreateCardGameObject();
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

                // Update position
                gameObject.transform.localPosition = new Vector3(ActualPosition.x, ActualPosition.y, -1f);
                UpdateVisualState();

                // Update spawn indicator animation
                UpdateSpawnIndicator();

                // Ensure GameObject is active when visible
                gameObject.SetActive(true);
            }
        }

        private void CreateCardGameObject()
        {
            try
            {
                gameObject = new GameObject(Name);
                gameObject.layer = LayerMask.NameToLayer("UI");

                if (menuTransform != null)
                {
                    gameObject.transform.SetParentAndResetScale(menuTransform);
                }

                // Create avatar display
                CreateAvatarDisplay();

                // Create name text
                CreateNameDisplay();

                // Create spawn indicator
                CreateSpawnIndicator();

            }
            catch (Exception)
            {
            }
        }

        private void CreateAvatarDisplay()
        {
            try
            {
                // Create GameObject with SpriteSM components
                avatarGO = new GameObject("Avatar", new Type[] {
                    typeof(MeshRenderer),
                    typeof(MeshFilter),
                    typeof(SpriteSM)
                });

                // Position avatar in upper portion of card
                float avatarSize = ActualSize.x * AVATAR_SIZE_RATIO;
                float avatarY = ActualSize.y * 0.15f;  // A bit higher in the card
                float avatarX = 4f;  // Offset to the right for better visual centering
                avatarGO.transform.SetParentAndResetScale(gameObject.transform, new Vector3(avatarX, avatarY, 0.1f));

                // Get components
                avatarSprite = avatarGO.GetComponent<SpriteSM>();
                avatarRenderer = avatarGO.GetComponent<MeshRenderer>();

                if (avatarRenderer != null)
                {
                    avatarRenderer.sortingOrder = 101;
                }

                // Configure SpriteSM
                if (avatarSprite != null)
                {
                    avatarSprite.plane = SpriteBase.SPRITE_PLANE.XY;
                    avatarSprite.anchor = SpriteBase.ANCHOR_METHOD.MIDDLE_CENTER;
                    avatarSprite.offset = Vector3.zero;
                    avatarSprite.width = avatarSize / 2.0f;
                    avatarSprite.height = avatarSize;

                    // Set texture if available
                    if (AvatarTexture != null)
                    {
                        SetupAvatarTexture();
                    }
                    else if (AvatarMaterial != null)
                    {
                        avatarRenderer.material = AvatarMaterial;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void CreateNameDisplay()
        {
            try
            {
                nameGO = new GameObject("Name");
                nameGO.layer = LayerMask.NameToLayer("UI");

                float nameY = -(ActualSize.y * 0.38f);
                nameGO.transform.SetParentAndResetScale(gameObject.transform, new Vector3(0, nameY, 0.1f));

                var meshRenderer = nameGO.AddComponent<MeshRenderer>();
                nameText = nameGO.AddComponent<TextMesh>();

                nameText.text = BroName.ToUpper();
                nameText.anchor = TextAnchor.MiddleCenter;
                nameText.alignment = TextAlignment.Center;
                nameText.color = Color.white;

                FontManager.ApplyFont(nameText, BroforceFont.Hudson, NAME_FONT_SIZE);

                meshRenderer.sortingOrder = 102;

                ApplyTextScaling();
            }
            catch (Exception)
            {
            }
        }

        private float MeasureTextWidth(string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0f;

            return FontManager.CalculateTextWidth(BroforceFont.Hudson, text, NAME_FONT_SIZE);
        }

        private void ApplyTextScaling()
        {
            if (!EnableDynamicTextScaling || nameText == null || string.IsNullOrEmpty(BroName))
            {
                // Use default scaling based on card width
                float textScale = ActualSize.x / 80f;
                nameGO.transform.localScale = Vector3.one * Mathf.Clamp(textScale, 0.5f, 1.5f);
                return;
            }

            // Measure the actual text width
            float measuredWidth = MeasureTextWidth(BroName.ToUpper());
            float availableWidth = ActualSize.x - 2f; // Small padding on sides

            // Default scale based on card size
            float baseScale = ActualSize.x / 80f;
            baseScale = Mathf.Clamp(baseScale, 0.5f, 1.5f);

            // Check if we need to scale down
            if (measuredWidth > availableWidth)
            {
                // Calculate scale factor needed to fit
                float scaleFactor = availableWidth / measuredWidth;

                // Apply minimum scale limit
                scaleFactor = Mathf.Max(scaleFactor, MinimumTextScale);

                // Apply the scale 
                baseScale *= scaleFactor;
            }

            // Apply the final scale
            nameGO.transform.localScale = Vector3.one * baseScale;
        }

        private void CreateSpawnIndicator()
        {
            try
            {
                spawnIndicatorGO = new GameObject("SpawnIndicator");
                spawnIndicatorGO.layer = LayerMask.NameToLayer("UI");

                // Position indicator in top-right corner
                float indicatorSize = ActualSize.x * INDICATOR_SIZE_RATIO;
                float indicatorX = ActualSize.x * 0.35f;
                float indicatorY = ActualSize.y * 0.35f;
                spawnIndicatorGO.transform.SetParentAndResetScale(gameObject.transform, new Vector3(indicatorX, indicatorY, 0.05f));

                // Add sprite renderer
                spawnIndicatorSprite = spawnIndicatorGO.AddComponent<SpriteRenderer>();
                spawnIndicatorSprite.sortingOrder = 103;

                // Create a glowing circle sprite for the indicator
                Sprite circleSprite = CreateCircleSprite(32);  // Create at reasonable resolution
                if (circleSprite != null)
                {
                    spawnIndicatorSprite.sprite = circleSprite;
                    // Scale to desired world size - sprite is now 112 pixels (32 radius * 2 + 48 for glow)
                    // We want the core circle to be indicatorSize world units
                    float spriteSize = 112f;  // Updated for glow with rays
                    float pixelsPerUnit = 100f; // Unity default
                    float currentWorldSize = spriteSize / pixelsPerUnit; // 0.8 world units
                    float desiredScale = indicatorSize / currentWorldSize;
                    spawnIndicatorGO.transform.localScale = Vector3.one * desiredScale;

                    // Set sprite renderer material
                    Material mat = new Material(Shader.Find("Sprites/Default"));
                    spawnIndicatorSprite.material = mat;
                }

                UpdateSpawnIndicator();
            }
            catch (Exception)
            {
            }
        }

        private Sprite CreateCircleSprite(int radius)
        {
            try
            {
                int size = radius * 2 + 48; // Extra space for glow rays
                Texture2D texture = new Texture2D(size, size);
                Color[] colors = new Color[size * size];

                Vector2 center = new Vector2(size / 2f, size / 2f);
                float innerRadius = radius * 0.6f;  // Solid bright core
                float glowStart = radius * 0.85f;   // Where the glow begins
                float outerGlow = radius + 24f;     // Extended glow range

                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        float distance = Vector2.Distance(new Vector2(x, y), center);

                        if (distance <= innerRadius)
                        {
                            // Solid bright center
                            colors[y * size + x] = Color.white;
                        }
                        else if (distance <= glowStart)
                        {
                            // Solid colored area with slight fade at edge
                            float t = (distance - innerRadius) / (glowStart - innerRadius);
                            float alpha = 1f;
                            float brightness = Mathf.Lerp(1f, 0.85f, t * t); // Quadratic falloff
                            colors[y * size + x] = new Color(brightness, brightness, brightness, alpha);
                        }
                        else if (distance <= radius)
                        {
                            // Sharp edge transition to glow
                            float t = (distance - glowStart) / (radius - glowStart);
                            float alpha = Mathf.Lerp(0.9f, 0.3f, t * t * t); // Cubic falloff for sharp edge
                            colors[y * size + x] = new Color(1f, 1f, 1f, alpha);
                        }
                        else if (distance <= outerGlow)
                        {
                            // Actual glow effect - exponential falloff for realistic light
                            float t = (distance - radius) / (outerGlow - radius);
                            float alpha = 0.3f * Mathf.Exp(-3f * t); // Exponential decay
                            colors[y * size + x] = new Color(1f, 1f, 1f, alpha);
                        }
                        else
                        {
                            colors[y * size + x] = Color.clear;
                        }
                    }
                }

                texture.SetPixels(colors);
                texture.Apply();

                return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
            }
            catch
            {
                return null;
            }
        }

        private void SetupAvatarTexture()
        {
            if (avatarSprite == null || AvatarTexture == null) return;

            try
            {
                // Create a proper sprite material
                Shader shader = Shader.Find("Unlit/Transparent Colored") ??
                               Shader.Find("Sprites/Default") ??
                               Shader.Find("Unlit/Texture");

                if (shader != null)
                {
                    Material mat = new Material(shader);
                    mat.mainTexture = AvatarTexture;
                    avatarRenderer.material = mat;

                    avatarSprite.SetFieldValue("texture", AvatarTexture);

                    avatarSprite.lowerLeftPixel = new Vector2(0f, 64f);
                    avatarSprite.pixelDimensions = new Vector2(32f, 64f);

                    avatarSprite.RecalcTexture();
                    avatarSprite.CalcUVs();
                    avatarSprite.UpdateUVs();
                }
            }
            catch (Exception)
            {
            }
        }

        public void LoadBroData(string broName)
        {
            BroName = broName;

            // Update name text if it exists
            if (nameText != null)
            {
                nameText.text = broName.ToUpper();
                ApplyTextScaling(); // Apply scaling for new name
            }

            // TODO: Load actual bro data from BroMaker
            // For now, use placeholder values
            IsLocked = UnityEngine.Random.Range(0, 2) == 0;  // Random 50/50 for testing
            IsSpawnEnabled = !IsLocked && UnityEngine.Random.Range(0, 2) == 0;

            UpdateVisualState();
        }

        public void SetAvatarFromTexture(Texture2D texture)
        {
            AvatarTexture = texture;
            if (avatarSprite != null)
            {
                SetupAvatarTexture();
            }
            UpdateVisualState();
        }

        public void UpdateVisualState()
        {
            // Update avatar tinting based on lock state (no hover effect)
            if (avatarRenderer != null && avatarRenderer.material != null)
            {
                Color tint = IsLocked ? LockedTintColor : Color.white;
                // Don't apply hover brightness to avatar - looks weird
                avatarRenderer.material.color = tint;
            }

            // Update spawn indicator
            UpdateSpawnIndicator();

            // Update name color
            if (nameText != null)
            {
                nameText.color = IsLocked ? Color.gray : Color.white;
            }
        }

        private void UpdateSpawnIndicator()
        {
            if (spawnIndicatorSprite != null && spawnIndicatorGO != null)
            {
                // Always show indicator
                spawnIndicatorGO.SetActive(true);

                // Set color based on lock and spawn state
                Color targetColor;
                if (IsLocked)
                {
                    targetColor = LockedSpawnColor;  // Gray for locked
                }
                else
                {
                    targetColor = IsSpawnEnabled ? EnabledSpawnColor : DisabledSpawnColor;
                }

                // Add subtle pulse for enabled bros
                if (!IsLocked && IsSpawnEnabled)
                {
                    float pulse = Mathf.Sin(Time.time * 2f) * 0.1f + 0.9f; // Oscillate between 0.8 and 1.0
                    targetColor.a *= pulse;
                }

                spawnIndicatorSprite.color = targetColor;
            }
        }

        public override void UpdateLayout()
        {
            base.UpdateLayout();

            // Update child element sizes and positions when layout changes
            if (avatarGO != null && avatarSprite != null)
            {
                float avatarSize = ActualSize.x * AVATAR_SIZE_RATIO;
                avatarSprite.width = avatarSize;
                avatarSprite.height = avatarSize;

                float avatarY = ActualSize.y * 0.15f;  // A bit higher in the card
                float avatarX = 4f;  // Offset to the right for better visual centering
                avatarGO.transform.localPosition = new Vector3(avatarX, avatarY, 0.1f);
            }

            if (nameGO != null)
            {
                float nameY = -(ActualSize.y * 0.38f);  // Just a bit higher
                nameGO.transform.localPosition = new Vector3(0, nameY, 0.1f);

                // Reapply text scaling when layout changes
                ApplyTextScaling();
            }

            if (spawnIndicatorGO != null)
            {
                float indicatorSize = ActualSize.x * INDICATOR_SIZE_RATIO;
                float indicatorX = ActualSize.x * 0.35f;
                float indicatorY = ActualSize.y * 0.35f;
                spawnIndicatorGO.transform.localPosition = new Vector3(indicatorX, indicatorY, 0.05f);

                // Match the scale calculation from CreateSpawnIndicator
                float spriteSize = 112f;  // Updated for glow with rays
                float pixelsPerUnit = 100f;
                float currentWorldSize = spriteSize / pixelsPerUnit; // 1.12 world units
                float desiredScale = indicatorSize / currentWorldSize;
                spawnIndicatorGO.transform.localScale = Vector3.one * desiredScale;
            }
        }

        public override void OnFocusGained()
        {
            base.OnFocusGained();
            UpdateVisualState();

            if (OnHover != null)
            {
                OnHover();
            }
        }

        public override void OnFocusLost()
        {
            base.OnFocusLost();
            UpdateVisualState();
        }

        public override void OnActivated()
        {
            base.OnActivated();

            if (OnClick != null)
            {
                OnClick();
            }
        }

        public override void Cleanup()
        {
            avatarSprite = null;
            avatarRenderer = null;
            nameText = null;
            spawnIndicatorSprite = null;

            base.Cleanup();
        }
    }
}
