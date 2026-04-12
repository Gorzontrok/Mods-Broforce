using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SmallerHUD
{
    public static class ModUI
    {
        private static Settings Sett
        {
            get
            {
                return Main.settings;
            }
        }

        public static void OnGUI()
        {
            GUILayout.Label("Small level: " + Mod.LevelName());
            if (Sett.scaleLevel != (Sett.scaleLevel = (int)GUILayout.HorizontalSlider(Sett.scaleLevel, 10, 0, GUILayout.Width(200))))
            {
                Mod.ResetScale();
            }
        }
    }
}
