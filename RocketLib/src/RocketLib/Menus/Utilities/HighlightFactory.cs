using System;
using System.IO;
using System.Reflection;
using RocketLib.Utils;
using UnityEngine;

namespace RocketLib.Menus.Utilities
{
    /// <summary>
    /// Factory class for creating menu highlight objects without dependencies
    /// </summary>
    /// <summary>
    /// Factory class for creating menu highlight objects without dependencies
    /// </summary>
    public static class HighlightFactory
    {
        /// <summary>
        /// Creates a complete highlight object with lens flare and box components
        /// </summary>
        public static MenuHighlightTween CreateHighlight(Transform parent, int layer)
        {
            GameObject highlightObject = new GameObject("MenuHighlight", new Type[] { typeof(MeshRenderer), typeof(MeshFilter), typeof(SpriteSM), typeof(MenuHighlightTween) });

            highlightObject.layer = layer;
            highlightObject.transform.SetParentAndResetScale(parent, new Vector3(0f, -27f, 15.5f));

            SpriteSM highlightSprite = highlightObject.GetComponent<SpriteSM>();

            string directoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string resourcesPath = Path.Combine(directoryPath, "Resources");

            Material lensFlareTexture = ResourcesController.GetMaterial(resourcesPath, "UI_MenuSelect_LensFlare.png");

            SetupSprite(highlightSprite, lensFlareTexture, new Vector2(3072f, 0f), new Vector2(512f, 64f), 478.5214f, 43.44803f, new Vector3(0f, 0f, 40f), SpriteBase.ANCHOR_METHOD.MIDDLE_CENTER);

            MenuHighlightTween highlightTween = highlightObject.GetComponent<MenuHighlightTween>();

            GameObject boxHorizontalsObject = CreateBoxComponent(highlightObject, "BoxHorizontals", new Vector3(0f, 2f, 0f), 144f, 28f, new Vector3(0, -3f, 0f), layer, SpriteBase.ANCHOR_METHOD.MIDDLE_CENTER);
            if (boxHorizontalsObject != null)
            {
                highlightTween.boxHorizontals = boxHorizontalsObject.GetComponent<SpriteSM>();
            }

            GameObject boxLeftObject = CreateBoxComponent(highlightObject, "BoxLeft", new Vector3(-72f, 2f, 0f), 7f, 28f, new Vector3(0, -3f, 0f), layer, SpriteBase.ANCHOR_METHOD.MIDDLE_RIGHT);
            if (boxLeftObject != null)
            {
                highlightTween.boxLeft = boxLeftObject.GetComponent<SpriteSM>();
            }

            GameObject boxRightObject = CreateBoxComponent(highlightObject, "BoxRight", new Vector3(72f, 2f, 0f), 7f, 28f, new Vector3(0, -3f, 0f), layer, SpriteBase.ANCHOR_METHOD.MIDDLE_LEFT);
            if (boxRightObject != null)
            {
                highlightTween.boxRight = boxRightObject.GetComponent<SpriteSM>();
            }

            highlightTween.bounceOnMove = true;
            highlightTween.bounceWidth = true;
            highlightTween.speed = 26f;
            highlightTween.highlightHeight = 28f;
            highlightTween.minWidth = 32f;

            highlightObject.SetActive(true);

            return highlightTween;
        }

        /// <summary>
        /// Creates a grid highlight with dynamic width and height support
        /// </summary>
        public static GridMenuHighlight CreateGridHighlight(Transform parent, int layer, float borderThickness = 10f)
        {
            GameObject highlightObject = new GameObject("GridMenuHighlight", new Type[] { typeof(GridMenuHighlight) });

            highlightObject.layer = layer;
            highlightObject.transform.SetParentAndResetScale(parent, Vector3.zero);

            GridMenuHighlight gridHighlight = highlightObject.GetComponent<GridMenuHighlight>();

            string directoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string resourcesPath = Path.Combine(directoryPath, "Resources");

            Material selectorTexture = ResourcesController.GetMaterial(resourcesPath, "UI_MainMenu_Selector.png");
            Material lensFlareTexture = ResourcesController.GetMaterial(resourcesPath, "UI_MenuSelect_LensFlare.png");

            GameObject lensFlareObject = new GameObject("LensFlare", new Type[] { typeof(MeshRenderer), typeof(MeshFilter), typeof(SpriteSM) });
            lensFlareObject.layer = layer;
            lensFlareObject.transform.SetParentAndResetScale(highlightObject.transform, new Vector3(0f, 0f, 40f));

            SpriteSM lensFlareSprite = lensFlareObject.GetComponent<SpriteSM>();
            SetupSprite(lensFlareSprite, lensFlareTexture, new Vector2(3072f, 0f), new Vector2(512f, 64f), 478.5214f, 43.44803f, Vector3.zero, SpriteBase.ANCHOR_METHOD.MIDDLE_CENTER);
            gridHighlight.lensFlare = lensFlareSprite;
            lensFlareObject.SetActive(false);

            gridHighlight.cornerTopLeft = CreateGridBoxSprite(highlightObject, "CornerTopLeft", selectorTexture, new Vector2(0f, 21f), new Vector2(21f, 21f), layer, SpriteBase.ANCHOR_METHOD.MIDDLE_CENTER);

            gridHighlight.cornerTopRight = CreateGridBoxSprite(highlightObject, "CornerTopRight", selectorTexture, new Vector2(68f, 21f), new Vector2(21f, 21f), layer, SpriteBase.ANCHOR_METHOD.MIDDLE_CENTER);

            gridHighlight.cornerBottomLeft = CreateGridBoxSprite(highlightObject, "CornerBottomLeft", selectorTexture, new Vector2(0f, 89f), new Vector2(21f, 21f), layer, SpriteBase.ANCHOR_METHOD.MIDDLE_CENTER);

            gridHighlight.cornerBottomRight = CreateGridBoxSprite(highlightObject, "CornerBottomRight", selectorTexture, new Vector2(68f, 89f), new Vector2(21f, 21f), layer, SpriteBase.ANCHOR_METHOD.MIDDLE_CENTER);

            gridHighlight.sideTop = CreateGridBoxSprite(highlightObject, "SideTop", selectorTexture, new Vector2(21f, 10f), new Vector2(47f, 10f), layer, SpriteBase.ANCHOR_METHOD.MIDDLE_CENTER);

            gridHighlight.sideBottom = CreateGridBoxSprite(highlightObject, "SideBottom", selectorTexture, new Vector2(21f, 89f), new Vector2(47f, 10f), layer, SpriteBase.ANCHOR_METHOD.MIDDLE_CENTER);

            gridHighlight.sideLeft = CreateGridBoxSprite(highlightObject, "SideLeft", selectorTexture, new Vector2(0f, 68f), new Vector2(10f, 47f), layer, SpriteBase.ANCHOR_METHOD.MIDDLE_CENTER);

            gridHighlight.sideRight = CreateGridBoxSprite(highlightObject, "SideRight", selectorTexture, new Vector2(79f, 68f), new Vector2(10f, 47f), layer, SpriteBase.ANCHOR_METHOD.MIDDLE_CENTER);

            gridHighlight.bounceOnMove = true;
            gridHighlight.speed = 12.5f;
            gridHighlight.BorderThickness = borderThickness;

            highlightObject.SetActive(true);

            return gridHighlight;
        }

        private static SpriteSM CreateGridBoxSprite(GameObject parent, string name, Material material,
            Vector2 lowerLeftPixel, Vector2 pixelDimensions, int layer, SpriteBase.ANCHOR_METHOD anchor)
        {
            GameObject boxObject = new GameObject(name, new Type[] { typeof(MeshRenderer), typeof(MeshFilter), typeof(SpriteSM) });

            boxObject.layer = layer;
            boxObject.transform.SetParentAndResetScale(parent.transform, Vector3.zero);

            SpriteSM boxSprite = boxObject.GetComponent<SpriteSM>();

            float width = pixelDimensions.x;
            float height = pixelDimensions.y;

            SetupSprite(boxSprite, material, lowerLeftPixel, pixelDimensions, width, height, Vector3.zero, anchor);

            return boxSprite;
        }

        private static GameObject CreateBoxComponent(GameObject parent, string name, Vector3 localPos, float width, float height, Vector3 spriteOffset, int layer, SpriteBase.ANCHOR_METHOD anchor)
        {
            string directoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string resourcesPath = Path.Combine(directoryPath, "Resources");

            Material selectorTexture = ResourcesController.GetMaterial(resourcesPath, "UI_MainMenu_Selector.png");

            GameObject boxObject = new GameObject(name, new Type[] { typeof(MeshRenderer), typeof(MeshFilter), typeof(SpriteSM) });

            boxObject.layer = layer;
            boxObject.transform.SetParentAndResetScale(parent.transform, localPos);

            SpriteSM boxSprite = boxObject.GetComponent<SpriteSM>();

            Vector2 lowerLeftPixel = Vector2.zero;
            Vector2 pixelDimensions = Vector2.zero;

            if (name == "BoxHorizontals")
            {
                lowerLeftPixel = new Vector2(21f, 89f);
                pixelDimensions = new Vector2(47f, 89f);
            }
            else if (name == "BoxLeft")
            {
                lowerLeftPixel = new Vector2(0f, 89f);
                pixelDimensions = new Vector2(21f, 89f);
            }
            else if (name == "BoxRight")
            {
                lowerLeftPixel = new Vector2(68f, 89f);
                pixelDimensions = new Vector2(21f, 89f);
            }

            SetupSprite(boxSprite, selectorTexture, lowerLeftPixel, pixelDimensions, width, height, spriteOffset, anchor);

            return boxObject;
        }

        private static void SetupSprite(SpriteSM sprite, Material material, Vector2 lowerLeftPixel, Vector2 pixelDimensions, float width, float height, Vector3 offset, SpriteBase.ANCHOR_METHOD anchor)
        {
            sprite.lowerLeftPixel = lowerLeftPixel;
            sprite.pixelDimensions = pixelDimensions;
            sprite.plane = SpriteBase.SPRITE_PLANE.XY;
            sprite.width = width;
            sprite.height = height;
            sprite.offset = offset;
            sprite.anchor = anchor;

            sprite.GetComponent<MeshRenderer>().material = material;
            sprite.SetFieldValue("texture", material.mainTexture);

            sprite.CalcUVs();
            sprite.UpdateUVs();
        }
    }
}
