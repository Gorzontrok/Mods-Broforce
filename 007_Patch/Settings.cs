using UnityEngine;
using UnityModManagerNet;

namespace DoubleBroSevenTrained
{
    public enum Its
    {
        No, Yes
    }

    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Draw("Mod Settings", Box = true)]
        public ModConfigs modConfigs = new ModConfigs();
        [Draw("Vanilla Settings", Collapsible = true), Space(10)]
        public VanillaConfigs vanillaConfigs = new VanillaConfigs();

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

        public void OnChange()
        { }
    }

    [DrawFields(DrawFieldMask.Public)]
    public class ModConfigs
    {
        [Draw("Use Tear Gas")]
        public bool useTearGas = true;
        [Draw("Show Balaclava On Avatar")]
        public bool showBalaclavaOnAvatar = true;
        [Draw("Not Accurate if Drunk", DrawType.ToggleGroup)]
        public Its hasDrunkShooting = Its.No;
        [Draw("Angle Range", Precision = 2, VisibleOn = "hasDrunkShooting|Yes")]
        public Vector2 drunkShootingAdditionalYSpeedRange = new Vector2(-25f, 25f);
    }

    [DrawFields(DrawFieldMask.Public)]
    public class VanillaConfigs
    {
        [Draw(Precision = 2, Label = "Muzzle Flash I")]
        public Vector2 muzzleFlashI = new Vector2(0.01f, 0.01f);

        [Header("Animation"), Space(5)]
        [Draw("Use Pushing Animation")]
        public bool usePushingAnimation = false;
        [Draw("Use Ladder Climbing Animation")]
        public bool useLadderClimbingAnimation = false;
        [Draw("Use Ladder Transition Animation")]
        public bool useLadderClimbingTransitionAnimation = false;
        [Draw(DrawType.Auto, Label = "Insemination Animation Position", Tooltip = "X is column and Y is row", Precision = 0)]
        public Vector2 inseminationAnimation = new Vector2(24, 6);

        [Header("Specials"), Space(5)]
        [Draw(DrawType.ToggleGroup, Tooltip = "This will override 'Use Tear Gas", Label = "Use Max Ammo")]
        public Its useMaxAmmo = Its.No;
        [Draw(DrawType.Slider, Label = "Max Ammo", Min = 0, Max = 5, VisibleOn = "useMaxAmmo|Yes")]
        public int maxAmmo = 2;

        [Header("Tear Gas"), Space(5)]
        [Draw("Spawn Tear Gas Offset", Precision = 2)]
        public Vector2 spawnTearGasPosition = new Vector2(6f, 10f);
        [Draw("Spawn Tear Gas Force", Precision = 2)]
        public Vector2 spawnTearGasPositionForce = new Vector2(200f, 150f);
        [Draw("Spawn Tear Gas At Feet Offset", Precision = 2)]
        public Vector2 spawnTearGasPositionAtFeet = new Vector2(6f, 3f);
        [Draw("Spawn Tear Gas At Feet Force", Precision = 2)]
        public Vector2 spawnTearGasPositionAtFeetForce = new Vector2(30f, 70f);

        [Header("Martinis"), Space(5)]
        [Draw(DrawType.Slider, Min = 0, Max = 10, Label = "Martini to drink to be drunk")]
        public int drunkAt = 2;
        [Draw(DrawType.Field, Label = "Drunk Animation Position", Tooltip = "X is column and Y is row", Box = true, Precision = 0)]
        public Vector2 drunkAnimationPosition = new Vector2(25, 7);
        [Draw("Drunk Animation Max Frames")]
        public int drunkAnimationMaxFrame = 7;
        [Header("Balaclava")]
        [Draw(DrawType.ToggleGroup, Label = "Change Balaclava Time")]
        public Its changeBalaclavaTime = Its.No;
        [Draw(Label = "Balaclava Time", VisibleOn = "changeBalaclavaTime|Yes")]
        public float balaclavaTime = 5f;
    }
}
