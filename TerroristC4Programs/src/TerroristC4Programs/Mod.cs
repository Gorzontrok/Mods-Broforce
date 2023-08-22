using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;

namespace TerroristC4Programs
{
    public static class Mod
    {
        public static bool CanUsePatch
        {
            get
            {
                if (Main.enabled)
                {
                    if (LevelEditorGUI.IsActive || Map.isEditing || LevelSelectionController.IsCustomCampaign())
                    {
                        return Main.settings.patchInCustomsLevel;
                    }
                    return true;
                }
                return false;
            }
        }

        public static bool CantUsePatch
        {
            get
            {
                return !CanUsePatch;
            }
        }

        public static Settings Sett
        {
            get
            {
                return Main.settings;
            }
        }

    }
}
