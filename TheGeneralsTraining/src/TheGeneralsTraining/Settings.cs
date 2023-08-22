using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityModManagerNet;

namespace TheGeneralsTraining
{
    public class Settings : UnityModManager.ModSettings
    {
        public bool patchInCustomsLevel;

        // HUD & GUI
        /// <summary>
        /// HUD
        /// </summary>
        public bool rememberPockettedSpecial;
        /// <summary>
        /// Avatars
        /// </summary>
        public bool faceHugger;

        // PROJECTILES
        /// <summary>
        /// Grenades
        /// </summary>
        public bool grenadeExplodeIfNotVisible;
        public bool goldenFlexBrosProjectile;

        // BROS
        /// <summary>
        /// Bros
        /// </summary>
        public bool ladderAnimation;
        /// <summary>
        /// Bros
        /// </summary>
        public bool pushAnimation;
        /// <summary>
        /// Panic Revive
        /// </summary>
        public bool holyWaterPanicUnits;
        /// <summary>
        ///
        /// </summary>
        public bool flexIfReviveSourceFlex;

        /// <summary>
        /// Brolander & Broden
        /// </summary>
        public bool electricThrow;
        /// <summary>
        /// Broniversal Soldier & Brominator & Steroids
        /// </summary>
        public bool strongerThrow;

        /// <summary>
        /// 007
        /// </summary>
        public bool fifthBondSpecial;
        /// <summary>
        /// 007
        /// </summary>
        public bool drunkSeven;

        /// <summary>
        /// Brade
        /// </summary>
        public bool bradeGlaive;

        /// <summary>
        /// Bro Hard
        /// </summary>
        public bool broHardFasterWhenDucking;

        /// <summary>
        /// Brochete
        /// </summary>
        public bool alternateSpecialAnim;

        /// <summary>
        /// BroDredd
        /// </summary>
        public bool lessTazerHit;

        /// <summary>
        /// BroveHeart
        /// </summary>
        public bool retrieveSwordInAmmo;

        /// <summary>
        /// Buffy
        /// </summary>
        public bool hollywaterMookToVillager;
        /// <summary>
        /// Buffy
        /// </summary>
        public bool betterKick;

        /// <summary>
        /// Casey Broback
        /// </summary>
        public bool strongerMelee;
        /// <summary>
        /// Casey Broback
        /// </summary>
        public bool pigGrenade;

        /// <summary>
        /// Chev Brolios
        /// </summary>
        public bool carBattery;
        /// <summary>
        /// Chev Brolios
        /// </summary>
        public bool noRecoil;

        /// <summary>
        /// Desperabros
        /// </summary>
        public bool mariachisPlayMusic;

        /// <summary>
        /// Dirty Harry
        /// </summary>
        public bool reloadOnPunch;

        /// <summary>
        /// Scorpion Bro
        /// </summary>
        public bool stealthier;

        /// <summary>
        /// Seth Brondle
        /// </summary>
        public bool noAcidCoverIfSpecial;
        /// <summary>
        /// Seth Brondle
        /// </summary>
        public bool leaveBeesBehind;

        /// <summary>
        /// Xena
        /// </summary>
        public bool betterChakram;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
