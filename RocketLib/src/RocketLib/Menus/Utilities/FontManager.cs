using System.Collections.Generic;
using UnityEngine;

namespace RocketLib.Menus.Utilities
{
    public enum BroforceFont
    {
        // Primary UI Fonts
        Hudson,              // HUDSONNY_BROFORCE (default)
        HudsonIcons,        // HUDSONNY_BROFORCE_ICONS
        HudsonOutline,      // HudsonOutline

        // Akzidenz-Grotesk Family
        AkzidenzBoldCondensed,
        AkzidenzBoldCondensedHiRes,
        AkzidenzExtraBold,
        AkzidenzMedium,
        AkzidenzRegular,
        AkzidenzBold,

        // Pixel Fonts
        Pixel04B,
        Pixel8BitWonder,

        // Other Fonts
        KTypeNYC,
        AndrewFootit,
        SourceHanSans,
        Arial
    }

    public static class FontManager
    {
        private const float SCALE_FACTOR = 0.1f;
        private const int DYNAMIC_BASE_SIZE = 256;

        private static readonly Dictionary<string, Font> fontCache = new Dictionary<string, Font>();
        private static readonly Dictionary<BroforceFont, string> fontNameMap = new Dictionary<BroforceFont, string>
        {
            { BroforceFont.Hudson, "HUDSONNY_BROFORCE" },
            { BroforceFont.HudsonIcons, "HUDSONNY_BROFORCE_ICONS" },
            { BroforceFont.HudsonOutline, "HudsonOutline" },

            { BroforceFont.AkzidenzBoldCondensed, "akzidenz-grotesk-be-bold-condensed" },
            { BroforceFont.AkzidenzBoldCondensedHiRes, "akzidenz-grotesk-be-bold-condensed HiRes" },
            { BroforceFont.AkzidenzExtraBold, "akzidenz-grotesk-be-extra-bold-condensed" },
            { BroforceFont.AkzidenzMedium, "akzidenz-grotesk-be-medium-condensed" },
            { BroforceFont.AkzidenzRegular, "akzidenz-grotesk-be-regular" },
            { BroforceFont.AkzidenzBold, "akzidenz-grotesk-bold" },

            { BroforceFont.Pixel04B, "04B_11__" },
            { BroforceFont.Pixel8BitWonder, "White_Font_8bitWonder" },

            { BroforceFont.KTypeNYC, "K-TYPE - NYC" },
            { BroforceFont.AndrewFootit, "ANDREW FOOTIT - HUDSON NY" },
            { BroforceFont.SourceHanSans, "SourceHanSansJPHW-Bold" },
            { BroforceFont.Arial, "Arial" }
        };

        static FontManager()
        {
            LoadFonts();
        }

        private static void LoadFonts()
        {
            Font[] allFonts = Resources.FindObjectsOfTypeAll<Font>();
            foreach (var font in allFonts)
            {
                if (font != null && !string.IsNullOrEmpty(font.name) && !fontCache.ContainsKey(font.name))
                {
                    fontCache[font.name] = font;
                }
            }
        }

        public static void ApplyFont(TextMesh textMesh, BroforceFont font, float size)
        {
            if (textMesh == null) return;

            Font unityFont = GetFont(font);
            if (unityFont != null)
            {
                textMesh.font = unityFont;

                var renderer = textMesh.GetComponent<MeshRenderer>();
                if (renderer != null && unityFont.material != null)
                {
                    renderer.sharedMaterial = unityFont.material;
                }

                if (IsDynamicFont(unityFont))
                {
                    textMesh.fontSize = DYNAMIC_BASE_SIZE;
                    textMesh.characterSize = size * SCALE_FACTOR;
                }
                else
                {
                    float bakeSize = unityFont.fontSize > 0 ? unityFont.fontSize : 64;
                    textMesh.characterSize = size * SCALE_FACTOR * (DYNAMIC_BASE_SIZE / bakeSize);
                }
            }
        }

        public static Vector2 CalculateTextDimensions(BroforceFont font, string text, float size)
        {
            return new Vector2(
                CalculateTextWidth(font, text, size),
                CalculateTextHeight(font, text, size)
            );
        }

        public static float CalculateTextWidth(BroforceFont font, string text, float size)
        {
            if (string.IsNullOrEmpty(text)) return 0;

            Font unityFont = GetFont(font);
            if (unityFont == null)
            {
                RocketLib.Main.logger?.Error($"[FontManager] CalculateTextWidth: Failed to get font '{font}'. GetFont returned null.");
                return 0;
            }

            string[] lines = text.Split('\n');
            float maxWidth = 0;

            int requestSize = unityFont.dynamic ? DYNAMIC_BASE_SIZE : 0;

            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                unityFont.RequestCharactersInTexture(line, requestSize);

                float lineWidth = 0;
                CharacterInfo charInfo;

                foreach (char c in line)
                {
                    if (unityFont.GetCharacterInfo(c, out charInfo, requestSize))
                    {
                        lineWidth += charInfo.advance;
                    }
                    else if (unityFont.GetCharacterInfo(' ', out charInfo, requestSize))
                    {
                        lineWidth += charInfo.advance;
                    }
                }

                if (lineWidth > maxWidth)
                {
                    maxWidth = lineWidth;
                }
            }

            float characterSize = size * SCALE_FACTOR;
            float calculatedWidth = maxWidth * characterSize * SCALE_FACTOR;

            float correctionFactor = GetFontWidthCorrectionFactor(font);
            calculatedWidth *= correctionFactor;

            return calculatedWidth;
        }

        public static float CalculateTextHeight(BroforceFont font, string text, float size, float lineSpacing = 1.0f)
        {
            float baseHeight = size * 2.3f;

            float correctionFactor = GetFontHeightCorrectionFactor(font);
            baseHeight *= correctionFactor;

            int lineCount = 1;
            if (!string.IsNullOrEmpty(text))
            {
                foreach (char c in text)
                {
                    if (c == '\n') lineCount++;
                }
            }

            if (lineCount == 1)
            {
                return baseHeight;
            }

            float multiLineHeight = baseHeight + ((lineCount - 1) * baseHeight * lineSpacing);

            float multiLineCorrectionFactor = GetMultiLineHeightCorrectionFactor(font);
            multiLineHeight *= multiLineCorrectionFactor;

            return multiLineHeight;
        }

        private static float GetFontWidthCorrectionFactor(BroforceFont font)
        {
            switch (font)
            {
                case BroforceFont.Pixel8BitWonder:
                    return 2.50000f;
                case BroforceFont.KTypeNYC:
                    return 6.09524f;
                case BroforceFont.HudsonIcons:
                case BroforceFont.HudsonOutline:
                    return 4.00000f;
                default:
                    return 1.00000f;
            }
        }

        private static float GetFontHeightCorrectionFactor(BroforceFont font)
        {
            switch (font)
            {
                case BroforceFont.Pixel8BitWonder:
                    return 0.01739f;
                case BroforceFont.KTypeNYC:
                    return 1.45252f;
                case BroforceFont.AndrewFootit:
                    return 1.15978f;
                case BroforceFont.SourceHanSans:
                    return 1.64731f;
                case BroforceFont.Arial:
                    return 1.00761f;
                case BroforceFont.Hudson:
                    return 1.00174f;
                case BroforceFont.HudsonIcons:
                case BroforceFont.HudsonOutline:
                    return 2.22609f;
                case BroforceFont.AkzidenzBoldCondensed:
                case BroforceFont.AkzidenzBoldCondensedHiRes:
                    return 0.98282f;
                case BroforceFont.AkzidenzExtraBold:
                    return 0.99395f;
                case BroforceFont.AkzidenzMedium:
                    return 0.94831f;
                case BroforceFont.AkzidenzRegular:
                    return 1.03624f;
                case BroforceFont.AkzidenzBold:
                    return 0.97280f;
                case BroforceFont.Pixel04B:
                    return 1.11304f;
                default:
                    return 1.00000f;
            }
        }

        private static float GetMultiLineHeightCorrectionFactor(BroforceFont font)
        {
            switch (font)
            {
                case BroforceFont.Hudson:
                    return 1.07407f;
                case BroforceFont.HudsonIcons:
                case BroforceFont.HudsonOutline:
                case BroforceFont.Pixel04B:
                case BroforceFont.Pixel8BitWonder:
                case BroforceFont.KTypeNYC:
                case BroforceFont.SourceHanSans:
                    return 1.00000f;
                case BroforceFont.AkzidenzBoldCondensed:
                case BroforceFont.AkzidenzBoldCondensedHiRes:
                    return 1.04983f;
                case BroforceFont.AkzidenzExtraBold:
                    return 1.05002f;
                case BroforceFont.AkzidenzMedium:
                    return 1.05008f;
                case BroforceFont.AkzidenzRegular:
                    return 1.05013f;
                case BroforceFont.AkzidenzBold:
                    return 1.05034f;
                case BroforceFont.AndrewFootit:
                    return 1.06936f;
                case BroforceFont.Arial:
                    return 1.18015f;
                default:
                    return 1.00000f;
            }
        }

        private static Font GetFont(BroforceFont font)
        {
            if (!fontNameMap.ContainsKey(font))
            {
                RocketLib.Main.logger?.Error($"[FontManager] Font {font} not found in fontNameMap");
                return null;
            }

            string fontName = fontNameMap[font];
            if (!fontCache.ContainsKey(fontName))
            {
                LoadFonts();
                if (!fontCache.ContainsKey(fontName))
                {
                    RocketLib.Main.logger?.Error($"[FontManager] Font '{fontName}' not found in Resources");
                    return null;
                }
            }

            return fontCache[fontName];
        }

        private static bool IsDynamicFont(Font font)
        {
            return font != null && font.dynamic;
        }
    }
}
