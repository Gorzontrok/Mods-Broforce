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
        // - Other
        public bool EnabledSickPigs;
        public bool MechDropDoesFumiginene;

        // Harmony Id
        public bool FilteredBrosIsHere = false; public bool UnpatchFilteredBros;
        public bool ExpendabrosModIsHere = false; public bool UnpatchExpendabrosMod;
        public bool ForBralefIsHere = false; public bool UnpatchForBralef;


        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
