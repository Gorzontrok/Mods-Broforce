using System.Linq;
using UnityEngine;

namespace RocketLib
{
    public static class RGUI
    {
        /// <summary> Do not expand the GUILayout width and height </summary>
        public static GUILayoutOption[] Unexpanded
        {
            get
            {
                return new GUILayoutOption[] { GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false) };
            }
        }

        /// <summary>
        /// A custom GUILayout. You make a choice with arrow.
        /// </summary>
        /// <param name="StringsArray">The values that are show.</param>
        /// <param name="selected">The current index.</param>
        /// <returns>Index of the array</returns>
        public static int ArrowList(string[] StringsArray, int selected)
        {
            GUIStyle LeftArrowStyle = new GUIStyle("button");
            GUIStyle RightArrowStyle = new GUIStyle("button");

            GUILayout.BeginHorizontal();

            LeftArrowStyle = ChangeArrowStyle(LeftArrowStyle, selected > 0);

            if (GUILayout.Button("<", LeftArrowStyle, GUILayout.ExpandWidth(false)))
            {
                if (selected > 0)
                {
                    selected--;
                }
            }
            GUILayout.Label(StringsArray[selected].ToString(), GUILayout.ExpandWidth(false));

            RightArrowStyle = ChangeArrowStyle(RightArrowStyle, selected < StringsArray.Length - 1);

            if (GUILayout.Button(">", RightArrowStyle, GUILayout.ExpandWidth(false)))
            {
                if (selected < StringsArray.Length - 1)
                {
                    selected++;
                }
            }
            GUILayout.EndHorizontal();

            return selected;
        }

        /// <summary>
        /// A custom GUILayout. You make a choice with arrow.
        /// </summary>
        /// <param name="StringsArray">The values that are show.</param>
        /// <param name="selected">The current index.</param>
        /// <param name="Width"></param>
        /// <returns>Index of the array</returns>
        public static int ArrowList(string[] StringsArray, int selected, float Width)
        {
            GUIStyle LeftArrowStyle = new GUIStyle("button");
            GUIStyle RightArrowStyle = new GUIStyle("button");

            GUILayout.BeginHorizontal(GUILayout.Width(Width));

            LeftArrowStyle = ChangeArrowStyle(LeftArrowStyle, selected > 0);

            if (GUILayout.Button("<", LeftArrowStyle, GUILayout.ExpandWidth(false)))
            {
                if (selected > 0)
                {
                    selected--;
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.Label(StringsArray[selected].ToString());
            GUILayout.FlexibleSpace();

            RightArrowStyle = ChangeArrowStyle(RightArrowStyle, selected < StringsArray.Length - 1);

            if (GUILayout.Button(">", RightArrowStyle, GUILayout.ExpandWidth(false)))
            {
                if (selected < StringsArray.Length - 1)
                {
                    selected++;
                }
            }
            GUILayout.EndHorizontal();

            return selected;
        }

        /// <summary>
        /// A custom GUILayout. You make a choice with arrow.
        /// </summary>
        /// <param name="ObjectsArray">The values that are show.</param>
        /// <param name="selected">The current index.</param>
        /// <returns>Index of the array</returns>
        public static int ArrowList(object[] ObjectsArray, int selected)
        {
            string[] StrArray = ObjectsArray.Select(obj => obj.ToString()).ToArray();
            return ArrowList(StrArray, selected);
        }

        /// <summary>
        /// A custom GUILayout. You make a choice with arrow.
        /// </summary>
        /// <param name="ObjectsArray">The values that are show.</param>
        /// <param name="selected">The current index.</param>
        /// <param name="Width"></param>
        /// <returns>Index of the array</returns>
        public static int ArrowList(object[] ObjectsArray, int selected, float Width)
        {
            string[] StrArray = ObjectsArray.Select(obj => obj.ToString()).ToArray();
            return ArrowList(StrArray, selected, Width);
        }

        /// <summary>
        /// </summary>
        /// <param name="Strings"></param>
        /// <param name="Number"></param>
        /// <param name="Space"></param>
        /// <param name="TabWidth"></param>
        /// <returns></returns>
        public static int Tab(string[] Strings, int Number, int Space, int TabWidth)
        {
            var TabStyle = new GUIStyle("button");
            var ActiveTabStyle = new GUIStyle("button");
            ActiveTabStyle.normal.background = ActiveTabStyle.hover.background;
            GUILayout.BeginHorizontal();
            for (int i = 0; i < Strings.Length; i++)
            {
                if (GUILayout.Button(Strings[i], (i == Number ? ActiveTabStyle : TabStyle), GUILayout.Width(TabWidth))) return i;
                GUILayout.Space(Space);
            }
            GUILayout.EndHorizontal();
            return Number;
        }

        private static GUIStyle ChangeArrowStyle(GUIStyle Style, bool ToCheck)
        {
            if (ToCheck)
            {
                Style.normal.textColor = Color.white;
                Style.hover.textColor = Color.white;
                Style.active.textColor = Color.white;
            }
            else
            {
                Style.normal.textColor = Color.gray;
                Style.hover.textColor = Color.gray;
                Style.active.textColor = Color.gray;
            }
            return Style;
        }

        public static void LabelCenteredHorizontally(GUIContent content, GUIStyle style = null, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(content, style ?? GUI.skin.label, options);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public static float HorizontalSlider(string text, float value, float minValue, float maxValue, float sliderWidth = 500f)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(text, GUILayout.ExpandWidth(false));
            GUILayout.Label(value.ToString(), GUILayout.Width(70));
            value = (float)GUILayout.HorizontalSlider(value, minValue, maxValue, GUILayout.MaxWidth(sliderWidth));
            GUILayout.EndHorizontal();
            return value;
        }
        public static float HorizontalSlider(string text, string tooltip, float value, float minValue, float maxValue, float sliderWidth = 500f)
        {
            GUILayout.BeginHorizontal(new GUIContent(string.Empty, tooltip), GUIStyle.none, new GUILayoutOption[] { });
            GUILayout.Label(text, GUILayout.ExpandWidth(false));
            GUILayout.Label(value.ToString(), GUILayout.Width(70));
            value = (float)GUILayout.HorizontalSlider(value, minValue, maxValue, GUILayout.MaxWidth(sliderWidth));
            GUILayout.EndHorizontal();
            return value;
        }

        public static int HorizontalSliderInt(string text, int value, int minValue, int maxValue, float sliderWidth = 500f)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(text, GUILayout.ExpandWidth(false));
            GUILayout.Label(value.ToString(), GUILayout.Width(70));
            value = (int)GUILayout.HorizontalSlider(value, minValue, maxValue, GUILayout.MaxWidth(sliderWidth));
            GUILayout.EndHorizontal();
            return value;
        }
        public static int HorizontalSliderInt(string text, string tooltip, int value, int minValue, int maxValue, float sliderWidth = 500f)
        {
            GUILayout.BeginHorizontal(new GUIContent(string.Empty, tooltip), GUIStyle.none, new GUILayoutOption[] { });
            GUILayout.Label(text, GUILayout.ExpandWidth(false));
            GUILayout.Label(value.ToString(), GUILayout.Width(70));
            value = (int)GUILayout.HorizontalSlider(value, minValue, maxValue, GUILayout.MaxWidth(sliderWidth));
            GUILayout.EndHorizontal();
            return value;
        }
    }
}
