using System;
using System.Collections.Generic;
using System.Linq;
using UnityModManagerNet;

namespace MapDataController
{
    public class Settings : UnityModManager.ModSettings
    {
        // PROBABILITY
        public bool bAcidBarrel = false;
        public float acidBarrelSpawnProbability = 0;
        public bool bAlienExplosiveBlock = false;
        public float alienExplosiveBlockSpawnProbability = 0;
        public bool bAmmoCrateRemoteCarProbability = false;
        public float ammoCrateRemoteCarProbability = 0f;
        public bool bBigMookProbability = false;
        public float bigMookSpawnProbability = 0.15f;
        public bool bMineFieldProbability = false;
        public float mineFieldSpawnProbability = 0.4f;
        public bool bOilBarrelProbability = false;
        public float oilBarrelSpawnProbability = 0.5f;
        public bool bPropaneTankProbability = false;
        public float propaneTankSpawnProbability = 0.4f;
        public bool bRegularMookProbability = false;
        public float regualrMookSpawnProbability = 0.45f;
        public bool bShieldMookProbability = false;
        public float riotShieldMookSpawnProbability = 0.2f;
        public bool bSpikeTrapProbability = false;
        public float spikeTrapSpawnProbability = 0.4f;
        public bool bSuicideMookProbability = false;
        public float suicideMookSpawnProbability = 0.2f;
        public bool bCoconutProbability = false;
        public float coconutProbability = 0;


        //True/False
        public bool bShowBubbleMultiplayer = false;
        public bool alwaysShowBubblesInMultiplayer = false;
        public bool bBloomEffect = false;
        public bool bloomEffect = false;
        public bool bStartWithFullSpecial = false;
        public bool brosStartWithFullSpecials = false;
        public bool bHighFiveSlowLevel = false;
        public bool canHighFiveSlowDownThisLevel = false;
        public bool bConstantFireworks = false;
        public bool constantFireworks = false;
        public bool bForceHardMode = false;
        public bool forceHardMode = false;
        public bool bOnlyTriggerWin = false;
        public bool onlyTriggersCanWinLevel = false;
        public bool bRestartOnDeathHardcore = false;
        public bool restartOnDeathInHardcore = false;
        public bool bSpawnAmmoCrate = false;
        public bool spawnAmmoCrates = true;
        public bool bSupressAnnouncer = false;
        public bool suppressAnnouncer = false;
        public bool bStandardDeathmatchCollapse = false;
        public bool useStandardDeathmatchCollapse = true;
        public bool bVignettingEffect = false;
        public bool vignettingEffect = false;
        public bool bWaitForAllPlayer = false;
        public bool waitForAllPlayersInOnline = false;


        // LIST
        public bool bAmbience = false;
        public int ambience = 0;
        public bool bCameraFollow = false;
        public int cameraFollowMode = 0;
        public bool bForcedBro = false;
        public int forcedBro = 0;
        public bool bHeroSpawnMode = false;
        public int heroSpawnMode = 0;
        public bool bMusicType = false;
        public int musicType = 0;
        public bool bTheme = false;
        public int theme = 0;
        public bool bWeatherType = false;
        public int weatherType = 0;

        // Float variable
        public bool bAmmoCrateMultiplier = false;
        public float ammoCrateFrequencyMultiplier = 1f;
        public bool bCameraSpeed = false;
        public float cameraSpeed = 32f;
        public bool bCollapseInterval = false;
        public float collapseInterval = 3f;
        public bool bBloomEffectM = false;
        public float bloomEffectM = 1f;
        public bool bBloomThresholdM = false;
        public float bloomThresholdM = 1f;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

    }
}
