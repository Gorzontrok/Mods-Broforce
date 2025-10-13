using UnityEngine;

namespace RocketLib.Menus.Utilities
{
    /// <summary>
    /// Custom highlight for grid menus that supports dynamic width and height
    /// Uses 9-slice approach with 8 sprites (4 corners + 4 sides)
    /// </summary>
    public class GridMenuHighlight : MonoBehaviour
    {
        protected Vector3 targetPos = Vector3.zero;
        protected Vector3 targetSize = new Vector3(100f, 100f, 1f);

        protected float bouncingHighlightCounter;
        protected bool bouncingHighlight;

        public bool bounceOnMove = true;
        public float speed = 10f;

        public SpriteSM lensFlare;

        public SpriteSM cornerTopLeft;
        public SpriteSM cornerTopRight;
        public SpriteSM cornerBottomLeft;
        public SpriteSM cornerBottomRight;

        public SpriteSM sideTop;
        public SpriteSM sideBottom;
        public SpriteSM sideLeft;
        public SpriteSM sideRight;

        protected int animFrame;
        protected float frameDelay;
        protected const float frameRate = 0.05f;

        protected const float cornerSize = 21f;
        protected float zOffset = -10f;

        public float BorderThickness { get; set; } = 10f;
        public float BorderPadding { get; set; } = 0f;

        public Vector3 TargetPos
        {
            get { return targetPos; }
        }

        public bool HasReachedTarget
        {
            get { return transform.localPosition == targetPos; }
        }

        protected void Start()
        {
            targetPos = transform.localPosition;
            UpdateBoxLayout();
        }

        public void SetTargetPos(Vector3 target, bool bounce = true)
        {
            if (targetPos != target)
            {
                if (bounce)
                {
                    Bounce();
                }
                targetPos = target;
            }
        }

        public void SetTargetSize(Vector3 size, bool bounce = true)
        {
            if (targetSize != size)
            {
                targetSize = size;
                if (bounce)
                {
                    Bounce();
                }
                UpdateBoxLayout();
            }
        }

        public void SetBorderThickness(float thickness)
        {
            BorderThickness = thickness;
            UpdateBoxLayout();
        }

        public void SetTargetBounds(Rect bounds)
        {
            Vector3 center = new Vector3(bounds.center.x, bounds.center.y, 0f);
            Vector3 size = new Vector3(bounds.width + BorderPadding * 2, bounds.height + BorderPadding * 2, 1f);
            SetTargetPos(center, bounceOnMove);
            SetTargetSize(size, false);
        }

        public void SetPositionImmediate(Vector3 pos)
        {
            targetPos = pos;
            transform.localPosition = new Vector3(pos.x, pos.y, zOffset);
        }

        public void SetBoundsImmediate(Rect bounds)
        {
            Vector3 center = new Vector3(bounds.center.x, bounds.center.y, 0f);
            Vector3 size = new Vector3(bounds.width + BorderPadding * 2, bounds.height + BorderPadding * 2, 1f);
            targetPos = center;
            transform.localPosition = new Vector3(center.x, center.y, zOffset);
            SetTargetSize(size, false);
        }

        protected void Update()
        {
            frameDelay += Time.deltaTime;
            if (frameDelay > frameRate)
            {
                frameDelay = 0f;
                if (animFrame < 6)
                {
                    animFrame++;
                }
                else
                {
                    animFrame--;
                }
                SetFrame();
            }

            if (bouncingHighlight)
            {
                bouncingHighlightCounter += Time.deltaTime * 2f;
                float num = Mathf.Clamp((0.7f - bouncingHighlightCounter) * 3f, 0f, 1f);
                float num2 = 1f - num;
                float num3 = Mathf.Sin(bouncingHighlightCounter * 3f + 1f / (0.1f + bouncingHighlightCounter * bouncingHighlightCounter * 2f));
                transform.localScale = new Vector3((1f + num3 * 0.04f) * num + num2, (1f + num3 * 0.07f) * num + num2, 1f);
            }

            Vector3 newPos = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * speed);
            transform.localPosition = new Vector3(newPos.x, newPos.y, zOffset);
        }

        public void Bounce()
        {
            if (!bounceOnMove)
            {
                return;
            }
            animFrame = 0;
            SetFrame();
            bouncingHighlight = true;
            bouncingHighlightCounter = 0f;
        }

        protected void SetFrame()
        {
            if (lensFlare != null)
            {
                lensFlare.SetLowerLeftPixel((float)(animFrame * 512), 64f);
                lensFlare.UpdateUVs();
            }
        }

        protected void UpdateBoxLayout()
        {
            float halfWidth = targetSize.x * 0.5f;
            float halfHeight = targetSize.y * 0.5f;

            // Scale corners based on border thickness
            // Original corner size is 21x21 pixels, which matches a 10f border thickness
            float scaledCornerSize = (BorderThickness / 10f) * cornerSize;
            float halfCorner = scaledCornerSize * 0.5f;

            if (cornerTopLeft != null)
            {
                cornerTopLeft.transform.localPosition = new Vector3(-halfWidth + halfCorner, halfHeight - halfCorner, 0f);
                cornerTopLeft.SetSize(scaledCornerSize, scaledCornerSize);
            }

            if (cornerTopRight != null)
            {
                cornerTopRight.transform.localPosition = new Vector3(halfWidth - halfCorner, halfHeight - halfCorner, 0f);
                cornerTopRight.SetSize(scaledCornerSize, scaledCornerSize);
            }

            if (cornerBottomLeft != null)
            {
                cornerBottomLeft.transform.localPosition = new Vector3(-halfWidth + halfCorner, -halfHeight + halfCorner, 0f);
                cornerBottomLeft.SetSize(scaledCornerSize, scaledCornerSize);
            }

            if (cornerBottomRight != null)
            {
                cornerBottomRight.transform.localPosition = new Vector3(halfWidth - halfCorner, -halfHeight + halfCorner, 0f);
                cornerBottomRight.SetSize(scaledCornerSize, scaledCornerSize);
            }

            if (sideTop != null)
            {
                sideTop.transform.localPosition = new Vector3(0f, halfHeight - BorderThickness * 0.5f, 0f);
                sideTop.SetSize(targetSize.x - scaledCornerSize * 2f, BorderThickness);
            }

            if (sideBottom != null)
            {
                sideBottom.transform.localPosition = new Vector3(0f, -halfHeight + BorderThickness * 0.5f, 0f);
                sideBottom.SetSize(targetSize.x - scaledCornerSize * 2f, BorderThickness);
            }

            if (sideLeft != null)
            {
                sideLeft.transform.localPosition = new Vector3(-halfWidth + BorderThickness * 0.5f, 0f, 0f);
                sideLeft.SetSize(BorderThickness, targetSize.y - scaledCornerSize * 2f);
            }

            if (sideRight != null)
            {
                sideRight.transform.localPosition = new Vector3(halfWidth - BorderThickness * 0.5f, 0f, 0f);
                sideRight.SetSize(BorderThickness, targetSize.y - scaledCornerSize * 2f);
            }
        }
    }
}
