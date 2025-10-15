using System;
using UnityEngine;

namespace RocketLib.Menus.Elements
{
    /// <summary>
    /// A LayoutElement that displays game textures using Broforce's SpriteSM system
    /// </summary>
    public class ImageElement : LayoutElement
    {
        public enum ImageScaleMode
        {
            None,
            Stretch,
            Fit,
            Fill
        }

        private GameObject spriteGO;
        private SpriteSM spriteSM;
        private MeshRenderer meshRenderer;
        private Material _material;
        private bool _needsUpdate = true;

        private Material _imageMaterial;
        public Material ImageMaterial
        {
            get => _imageMaterial;
            set
            {
                _imageMaterial = value;
                _needsUpdate = true;
                if (spriteSM != null)
                {
                    UpdateSpriteSM();
                }
            }
        }

        private Texture2D _texture;
        public Texture2D Texture
        {
            get => _texture;
            set
            {
                _texture = value;
                _needsUpdate = true;
                if (spriteSM != null)
                {
                    UpdateSpriteSM();
                }
            }
        }

        public ImageScaleMode ScaleMode { get; set; } = ImageScaleMode.Fit;

        private Color _tint = Color.white;
        public Color Tint
        {
            get => _tint;
            set
            {
                _tint = value;
                UpdateTint();
            }
        }

        public int PixelsPerUnit { get; set; } = 1;

        public Vector2? LowerLeftPixel { get; set; }

        public Vector2? PixelDimensions { get; set; }

        public Vector2? SpriteOffset { get; set; }

        public ImageElement(string name) : base(name)
        {
            IsFocusable = false;
        }

        public override void Render()
        {
            try
            {
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

                    gameObject.transform.position = ActualPosition;

                    if (spriteGO == null)
                    {
                        try
                        {
                            // Create GameObject with proper components following HighlightFactory pattern
                            spriteGO = new GameObject("SpriteSM", new Type[] {
                                typeof(MeshRenderer),
                                typeof(MeshFilter),
                                typeof(SpriteSM)
                            });
                            spriteGO.transform.SetParentAndResetScale(gameObject.transform, Vector3.zero);

                            spriteSM = spriteGO.GetComponent<SpriteSM>();
                            meshRenderer = spriteGO.GetComponent<MeshRenderer>();

                            if (meshRenderer != null)
                            {
                                meshRenderer.sortingOrder = 100;
                            }

                            // Configure SpriteSM with default values
                            if (spriteSM != null)
                            {
                                spriteSM.plane = SpriteBase.SPRITE_PLANE.XY;
                                spriteSM.anchor = SpriteBase.ANCHOR_METHOD.MIDDLE_CENTER;
                                spriteSM.offset = Vector3.zero;
                            }

                            if (_needsUpdate)
                            {
                                UpdateSpriteSM();
                            }
                        }
                        catch (System.Exception)
                        {
                        }
                    }

                    // Ensure GameObject is active when visible
                    gameObject.SetActive(true);
                }
            }
            catch (System.Exception)
            {
            }
        }

        private void UpdateSpriteSM()
        {
            if (spriteSM == null || meshRenderer == null) return;

            // Set material
            if (_imageMaterial != null)
            {
                meshRenderer.material = _imageMaterial;
                _material = _imageMaterial;

                // Get texture from material
                Texture mainTex = _imageMaterial.mainTexture;
                if (mainTex != null && mainTex is Texture2D)
                {
                    SetupSpriteFromTexture((Texture2D)mainTex);
                }
            }
            else if (_texture != null)
            {
                // Create material if needed
                if (_material == null)
                {
                    // Use default shader for sprites
                    Shader shader = Shader.Find("Sprites/Default");
                    if (shader == null)
                        shader = Shader.Find("Unlit/Texture");
                    if (shader == null)
                        shader = meshRenderer.sharedMaterial?.shader;

                    if (shader != null)
                    {
                        _material = new Material(shader);
                    }
                }

                if (_material != null)
                {
                    _material.mainTexture = _texture;
                    meshRenderer.material = _material;
                    SetupSpriteFromTexture(_texture);
                }
            }

            UpdateTint();
            _needsUpdate = false;
        }

        private void SetupSpriteFromTexture(Texture2D texture)
        {
            if (spriteSM == null || texture == null) return;

            // Set pixel coordinates to show entire texture or specified region
            // Use custom LowerLeftPixel if provided, otherwise default to bottom-left corner
            spriteSM.lowerLeftPixel = LowerLeftPixel ?? new Vector2(0, texture.height);

            // Use custom PixelDimensions if provided (for animated sprites), otherwise use full texture
            Vector2 actualPixelDimensions = PixelDimensions ?? new Vector2(texture.width, texture.height);
            spriteSM.pixelDimensions = actualPixelDimensions;

            // Set world size based on actual pixel dimensions (may be a single frame)
            float worldWidth = actualPixelDimensions.x / (float)PixelsPerUnit;
            float worldHeight = actualPixelDimensions.y / (float)PixelsPerUnit;

            spriteSM.width = worldWidth;
            spriteSM.height = worldHeight;

            spriteSM.offset = SpriteOffset ?? new Vector2(0, 0);

            // Set texture reference using reflection like HighlightFactory does
            spriteSM.SetFieldValue("texture", texture);

            // Calculate and update UVs
            spriteSM.CalcUVs();
            spriteSM.UpdateUVs();

            // Apply scaling after basic setup
            ApplyScaling();
        }

        private void UpdateTint()
        {
            if (spriteSM != null)
            {
                spriteSM.SetColor(_tint);
            }
        }

        private void ApplyScaling()
        {
            if (spriteSM == null || (_texture == null && _imageMaterial?.mainTexture == null)) return;

            Texture2D tex = _texture ?? (_imageMaterial.mainTexture as Texture2D);
            if (tex == null) return;

            // Use actual pixel dimensions (may be a single frame) for scaling
            Vector2 actualPixelDimensions = PixelDimensions ?? new Vector2(tex.width, tex.height);
            float textureWidth = actualPixelDimensions.x / (float)PixelsPerUnit;
            float textureHeight = actualPixelDimensions.y / (float)PixelsPerUnit;

            if (textureWidth <= 0 || textureHeight <= 0) return;

            float targetWidth = ActualSize.x;
            float targetHeight = ActualSize.y;

            float newWidth = textureWidth;
            float newHeight = textureHeight;

            switch (ScaleMode)
            {
                case ImageScaleMode.Stretch:
                    newWidth = targetWidth;
                    newHeight = targetHeight;
                    break;

                case ImageScaleMode.Fit:
                    float fitScale = Mathf.Min(targetWidth / textureWidth, targetHeight / textureHeight);
                    newWidth = textureWidth * fitScale;
                    newHeight = textureHeight * fitScale;
                    break;

                case ImageScaleMode.Fill:
                    float fillScale = Mathf.Max(targetWidth / textureWidth, targetHeight / textureHeight);
                    newWidth = textureWidth * fillScale;
                    newHeight = textureHeight * fillScale;
                    break;

                case ImageScaleMode.None:
                    // Keep original size
                    break;
            }

            // Set the size directly on SpriteSM
            spriteSM.width = newWidth;
            spriteSM.height = newHeight;
        }

        public override void UpdateLayout()
        {
            base.UpdateLayout();

            if (spriteSM != null && (_texture != null || _imageMaterial != null))
            {
                ApplyScaling();
            }
        }



        public override void Cleanup()
        {
            if (_material != null && _imageMaterial == null)
            {
                UnityEngine.Object.Destroy(_material);
                _material = null;
            }

            if (spriteGO != null)
            {
                UnityEngine.Object.Destroy(spriteGO);
                spriteGO = null;
                spriteSM = null;
                meshRenderer = null;
            }

            base.Cleanup();
        }
    }
}
