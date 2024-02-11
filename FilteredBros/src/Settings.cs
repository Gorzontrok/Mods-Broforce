using System.Collections.Generic;
using UnityEngine;
using UnityModManagerNet;

namespace FilteredBros
{
    public class Settings : UnityModManager.ModSettings
    {
        public UISettings ui = new UISettings();

        public ModSettings mod = new ModSettings();

        public List<bool> brosEnable;
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }

    [DrawFields(DrawFieldMask.Public)]
    public class UISettings : IDrawable
    {
        [Header("UI Settings")]
        [Draw(Type = DrawType.Slider, Label = "Bros per line", Min = 1, Max = 15)]
        public int numberOfBroPerLine = 6;
        [Draw(Type = DrawType.Slider, Label = "Names Width", Min = 10, Max = 500)]
        public int toggleWidth = 130;

        public void OnChange()
        { }
    }

    [DrawFields(DrawFieldMask.Public)]
    public class ModSettings : IDrawable
    {
        [Header("Mod Settings")]
        [Draw(Type = DrawType.Toggle, Label = "Patch In Custom Levels")]
        public bool patchInCustomsLevel = false;
        [Draw(Type = DrawType.Toggle, Label = "Use in Level Editor")]
        public bool useInLevelEditor = false;
        [Draw(Type = DrawType.Toggle, Label = "Ignore Forced Bros in levels")]
        public bool ignoreForcedBros = false;
        [Draw(Type = DrawType.Field, Label = "Maximum number of lives (0 to disabled)", Width = 80)]
        public int maxLifeNumber = 0;

        public void OnChange()
        { }
    }
}
