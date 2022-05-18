using System;
using UnityModManagerNet;

namespace TweaksFromPigs
{
    public class Settings : UnityModManager.ModSettings
    {
        public bool firstLaunch;

        // Framerate
        public bool useCustomFramerate;
        public int maxFramerate;

        // - Bro
        public bool fixIndianaAchievement;
        public bool teargasAtFeet;
        public bool lessAccurateDrunkSeven;
        public bool useFifthBondSpecial;
        // Animation
        public bool usePushingFrame;
        public bool useNewLadderFrame;
        public bool fixPushingAnimation;
        public bool brocheteAlternateSpecialAnim;
        // Bro Spawn
        public bool changeHeroUnlock;
        public bool spawnWithExpendabros;
        public bool spawnWithBrondleFly;
        // Global Fix
        public bool tbagEnabled;
        public bool rememberPockettedSpecial;
        public bool fixExpendabros;

        // - Doodads
        public bool christmasAmmoBox;

        // - Enemies
        public bool canTeargasedEveryone;
        public float zombieSpeedModifier;
        public bool fasterZombie;
        public bool customSkinned;

        // - Interface
        // HUD
        public bool showFacehuggerHUD;
        public bool skeletonDeadFace;
        // Menu
        public bool languageMenuEnabled;
        public bool fixMaxArcadeLevel;

        // - Map
        public float acidBarrelSpawnProbability;
        public bool fixHidingInGrass;
        public bool useAcidBarrel;
        public int arcadeIndex;

        // - Other
        public bool moreBroInDeathMatch;
        public bool mechDropDoesSmoke;
        public bool pigAreAlwaysTerror;
        public bool setCustomMouse; public bool customMouseIsSet;
        public string customPlayerName;
        public bool fixTakeScreenshots;

        // Danger Zone
        public bool dangerZoneOpen = false;


        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
