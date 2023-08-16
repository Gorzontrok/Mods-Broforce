using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using UnityEngine;

namespace SmallerHUD
{
    public static class Mod
    {
        public static bool CanUsePatch
        {
            get
            {
                return Main.enabled;
            }
        }
        private static Settings Sett
        {
            get
            {
                return Main.settings;
            }
        }
        public static string LevelName()
        {
            switch (Sett.scaleLevel)
            {
                case 10:
                case 9:
                    return "No Change";
                case 8:
                case 7:
                    return "A little smaller";
                case 6:
                case 5:
                    return "Smaller";
                case 4:
                case 3:
                    return "That's Small";
                case 2:
                case 1:
                    return "Too small";
                case 0:
                    return "I'm sure i can see it";
            }
            return "You broke the scale";
        }

        public static Vector3 LevelVector()
        {
            return new Vector3((float)Sett.scaleLevel / 10, (float)Sett.scaleLevel / 10, 1f);
        }

        public static void ResetScale()
        {
            foreach (var player in HeroController.players)
            {
                if (player != null && player.hud != null)
                {
                    player.hud.transform.localScale = LevelVector();
                }
            }
        }
    }
}
