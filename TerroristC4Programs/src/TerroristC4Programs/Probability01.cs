using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TerroristC4Programs
{
    [Serializable]
    public class Probability01
    {
        public float Value
        {
            get
            {
                return enabled ? value : 0f;
            }
        }

        public float value = 0f;
        public bool enabled = false;
        public void ToGui(GUIContent guiContent, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label((enabled ? "Disable " : "Enable ") + guiContent.text, GUILayout.ExpandWidth(false));
            enabled = GUILayout.Toggle(enabled, GUIContent.none, options);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Probability: " + value, GUILayout.Width(150));
            value = GUILayout.HorizontalSlider(value, 0f, 1f, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, GUILayout.Width(100));
            GUILayout.EndHorizontal();
        }
    }
}
