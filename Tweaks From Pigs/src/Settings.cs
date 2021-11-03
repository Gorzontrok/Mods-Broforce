using System;
using UnityModManagerNet;

namespace TweaksFromPigs
{
    public class Settings : UnityModManager.ModSettings
    {
        public bool getFirstLaunch;
        public bool NeedReload = false;

        // Animation
        public bool UsePushingFrame;
        public bool UseNewLadderFrame;

        // Bros addon/fix
        public bool TbagEnabled;
        public bool UseFifthBondSpecial;
        public bool RememberPockettedSpecial;

        // Bro Spawn
        public bool ChangeHeroUnlock;
        public bool SpawnWithExpendabros;
        public bool SpawnBrondeFly;

        // Enemies
        public bool FasterZombie;

        // Framerate
        public bool UseCustomFramerate;
        public int MaxFramerate;

        // HUD
        public bool ShowFacehuggerHUD;
        public bool SkeletonDeadFace;

        // Map
        public bool UseAcidBarrel;
        public int ArcadeIndex;

        // Other
        public bool PigAreAlwaysTerror;
        public bool SetCustomMouse; public bool CustomMouseIsSet;

        // AdvancedOption
        public bool ShowAdvancedOption;
        public bool CloseAdvancedOptionOnExit;
        // - Animation
        public bool FixPushingAnimation;
        // - Bro
        public bool FixIndianaAchievement;
        public bool TeargasAtFeet;
        public bool LessAccurateDrunkSeven;
        public bool FixExpendabros;
        public bool LessOPBroniversalRevive;
        public bool BroniversalAutoRevive;
        // - Enemies
        public bool CanTeargasedEveryone;
        public float ZombieSpeedModifier;
        // - Map
        public float AcidBarrelSpawnChance;
        public bool FixHidingInGrass;
        // - Menu
        public bool LanguageMenuEnabled;
        public bool MaxArcadeLevelEnabled;
        // - Other
        public bool EnabledSickPigs;
        public bool MechDropDoesFumiginene;

        // Harmony Id
        public bool FilteredBros_Compatibility;
        public bool ExpendablesBros_Compatibility;
        public bool ForBralef_Compatibility;
        public bool _007Patch_Compatibility;
        public bool AvatarFaceHugger_Compatibility;
        public bool SkeletonDeadFace_Compatibility;
        public bool MapDataController_Compatibility;

        // Danger Zone
        public bool DangerZoneOpen = false;
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
