using HarmonyLib;

namespace RocketLib.Utils
{
    /// <summary>
    /// Enumeration of all unit types in Broforce
    /// </summary>
    public enum UnitType
    {
        None = 0,
        Bro = 1,
        Mook = 2,
        SuicideMook = 3,
        Bruiser = 4,
        SuicideBruiser = 5,
        StrongBruiser = 6,
        EliteBruiser = 7,
        ScoutMook = 8,
        RiotShieldMook = 9,
        Mech = 10,
        BrownMech = 11,
        JetpackMook = 12,
        GrenadierMook = 13,
        BazookaMook = 14,
        JetpackBazookaMook = 15,
        NinjaMook = 16,
        TreasureMook = 17,
        AttackDog = 18,
        SkinnedMook = 19,
        MookGeneral = 20,
        Alarmist = 21,
        StrongMook = 22,
        ScientistMook = 23,
        Snake = 24,
        Satan = 25,
        Facehugger = 26,
        Xenomorph = 27,
        Brute = 28,
        Screecher = 29,
        Baneling = 30,
        XenomorphBrainbox = 31,
        Hellhound = 32,
        UndeadMook = 33,
        Warlock = 34,
        Boomer = 35,
        UndeadSuicideMook = 36,
        Executioner = 37,
        LostSoul = 38,
        SoulCatcher = 39,
        SatanMiniboss = 40,
        CR666 = 41,
        Pig = 42,
        RottenPig = 43,
        Villager = 44,
        RCCar = 45,
        // Non-TestVanDammeAnim Units
        StealthTank = 46,
        Terrorkopter = 47,
        Terrorbot = 48,
        Megacockter = 49,
        SandWorm = 50,
        Boneworm = 51,
        BonewormBehind = 52,
        AlienWorm = 53,
        AlienFacehuggerWorm = 54,
        AlienFacehuggerWormBehind = 55,
        LargeAlienWorm = 56,
        MookLauncherTank = 57,
        CannonTank = 58,
        RocketTank = 59,
        ArtilleryTruck = 60,
        Blimp = 61,
        DrillCarrier = 62,
        MookTruck = 63,
        Turret = 64,
        Motorbike = 65,
        MotorbikeNuclear = 66,
        DumpTruck = 67
    }

    /// <summary>
    /// Utility class for working with Broforce unit types, providing methods for identification, spawning, and querying unit properties.
    /// </summary>
    public static class UnitTypes
    {
        /// <summary>
        /// Array containing the display names of all TestVanDammeAnim-derived units.
        /// </summary>
        public static readonly string[] AllTestVanDammeAnimNames = new string[]
        {
            "Mook", "Suicide Mook", "Bruiser", "Suicide Bruiser", "Strong Bruiser", "Elite Bruiser", "Scout Mook", "Riot Shield Mook", "Mech", "Brown Mech", "Jetpack Mook", "Grenadier Mook", "Bazooka Mook", "Jetpack Bazooka Mook",
            "Ninja Mook", "Treasure Mook", "Attack Dog", "Skinned Mook", "Mook General", "Alarmist", "Strong Mook", "Scientist Mook", "Snake", "Satan", "Facehugger", "Xenomorph", "Brute", "Screecher", "Baneling", "Xenomorph Brainbox",
            "Hellhound", "Undead Mook", "Warlock", "Boomer", "Undead Suicide Mook", "Executioner", "Lost Soul", "Soul Catcher", "Satan Miniboss", "CR666", "Pig", "Rotten Pig", "Villager", "Remote Control Car",
        };

        /// <summary>
        /// Array containing the display names of all units in the game, including non-TestVanDammeAnim types.
        /// </summary>
        public static readonly string[] AllUnitNames = new string[]
        {
            "Mook", "Suicide Mook", "Bruiser", "Suicide Bruiser", "Strong Bruiser", "Elite Bruiser", "Scout Mook", "Riot Shield Mook", "Mech", "Brown Mech", "Jetpack Mook", "Grenadier Mook", "Bazooka Mook", "Jetpack Bazooka Mook",
            "Ninja Mook", "Treasure Mook", "Attack Dog", "Skinned Mook", "Mook General", "Alarmist", "Strong Mook", "Scientist Mook", "Snake", "Satan", "Facehugger", "Xenomorph", "Brute", "Screecher", "Baneling", "Xenomorph Brainbox",
            "Hellhound", "Undead Mook", "Warlock", "Boomer", "Undead Suicide Mook", "Executioner", "Lost Soul", "Soul Catcher", "Satan Miniboss", "CR666", "Pig", "Rotten Pig", "Villager", "Remote Control Car",
            "Stealth Tank", "Terrorkopter", "Terrorbot", "Megacockter", "Sand Worm", "Boneworm", "Boneworm Behind", "Alien Worm", "Alien Facehugger Worm", "Alien Facehugger Worm Behind", "Large Alien Worm",
            "Mook Launcher Tank", "Cannon Tank", "Rocket Tank", "Artillery Truck", "Blimp", "Drill Carrier", "Mook Truck", "Turret", "Motorbike", "Motorbike Nuclear", "Dump Truck"
        };

        /// <summary>
        /// Normal enemy units (regular mooks, bruisers, etc.)
        /// </summary>
        public static readonly UnitType[] NormalUnits = new UnitType[]
        {
            UnitType.Mook, UnitType.SuicideMook, UnitType.Bruiser, UnitType.SuicideBruiser,
            UnitType.StrongBruiser, UnitType.EliteBruiser, UnitType.ScoutMook, UnitType.RiotShieldMook,
            UnitType.Mech, UnitType.BrownMech, UnitType.JetpackMook, UnitType.GrenadierMook,
            UnitType.BazookaMook, UnitType.JetpackBazookaMook, UnitType.NinjaMook, UnitType.TreasureMook,
            UnitType.AttackDog, UnitType.SkinnedMook, UnitType.MookGeneral, UnitType.Alarmist,
            UnitType.StrongMook, UnitType.ScientistMook, UnitType.Snake, UnitType.Satan
        };

        /// <summary>
        /// Alien enemy units
        /// </summary>
        public static readonly UnitType[] AlienUnits = new UnitType[]
        {
            UnitType.Facehugger, UnitType.Xenomorph, UnitType.Brute,
            UnitType.Screecher, UnitType.Baneling, UnitType.XenomorphBrainbox
        };

        /// <summary>
        /// Hell enemy units
        /// </summary>
        public static readonly UnitType[] HellUnits = new UnitType[]
        {
            UnitType.Hellhound, UnitType.UndeadMook, UnitType.Warlock, UnitType.Boomer,
            UnitType.UndeadSuicideMook, UnitType.Executioner, UnitType.LostSoul, UnitType.SoulCatcher
        };

        /// <summary>
        /// Boss units
        /// </summary>
        public static readonly UnitType[] BossUnits = new UnitType[]
        {
            UnitType.SatanMiniboss, UnitType.CR666,
            UnitType.StealthTank, UnitType.Terrorkopter, UnitType.Terrorbot, UnitType.Megacockter
        };

        /// <summary>
        /// Worm enemy units
        /// </summary>
        public static readonly UnitType[] WormUnits = new UnitType[]
        {
            UnitType.SandWorm, UnitType.Boneworm, UnitType.BonewormBehind,
            UnitType.AlienWorm, UnitType.AlienFacehuggerWorm, UnitType.AlienFacehuggerWormBehind,
            UnitType.LargeAlienWorm
        };

        /// <summary>
        /// Vehicle enemy units (tanks, trucks, etc.)
        /// </summary>
        public static readonly UnitType[] VehicleEnemies = new UnitType[]
        {
            UnitType.MookLauncherTank, UnitType.CannonTank, UnitType.RocketTank,
            UnitType.ArtilleryTruck, UnitType.Blimp, UnitType.DrillCarrier,
            UnitType.MookTruck, UnitType.Turret, UnitType.Motorbike,
            UnitType.MotorbikeNuclear, UnitType.DumpTruck
        };

        /// <summary>
        /// Civilian and animal units
        /// </summary>
        public static readonly UnitType[] FriendlyUnits = new UnitType[]
        {
            UnitType.Pig, UnitType.RottenPig, UnitType.Villager
        };

        /// <summary>
        /// Other units
        /// </summary>
        public static readonly UnitType[] OtherUnits = new UnitType[]
        {
            UnitType.RCCar
        };

        /// <summary>
        /// All unit types in the game (excluding None)
        /// </summary>
        public static readonly UnitType[] AllUnits = new UnitType[]
        {
            // Normal enemies
            UnitType.Mook, UnitType.SuicideMook, UnitType.Bruiser, UnitType.SuicideBruiser,
            UnitType.StrongBruiser, UnitType.EliteBruiser, UnitType.ScoutMook, UnitType.RiotShieldMook,
            UnitType.Mech, UnitType.BrownMech, UnitType.JetpackMook, UnitType.GrenadierMook,
            UnitType.BazookaMook, UnitType.JetpackBazookaMook, UnitType.NinjaMook, UnitType.TreasureMook,
            UnitType.AttackDog, UnitType.SkinnedMook, UnitType.MookGeneral, UnitType.Alarmist,
            UnitType.StrongMook, UnitType.ScientistMook, UnitType.Snake,
            // Alien enemies
            UnitType.Facehugger, UnitType.Xenomorph, UnitType.Brute,
            UnitType.Screecher, UnitType.Baneling, UnitType.XenomorphBrainbox,
            // Hell enemies
            UnitType.Hellhound, UnitType.UndeadMook, UnitType.Warlock, UnitType.Boomer,
            UnitType.UndeadSuicideMook, UnitType.Executioner, UnitType.LostSoul, UnitType.SoulCatcher,
            // Bosses
            UnitType.Satan, UnitType.SatanMiniboss, UnitType.CR666,
            UnitType.StealthTank, UnitType.Terrorkopter, UnitType.Terrorbot, UnitType.Megacockter,
            // Worms
            UnitType.SandWorm, UnitType.Boneworm, UnitType.BonewormBehind,
            UnitType.AlienWorm, UnitType.AlienFacehuggerWorm, UnitType.AlienFacehuggerWormBehind,
            UnitType.LargeAlienWorm,
            // Vehicles
            UnitType.MookLauncherTank, UnitType.CannonTank, UnitType.RocketTank,
            UnitType.ArtilleryTruck, UnitType.Blimp, UnitType.DrillCarrier,
            UnitType.MookTruck, UnitType.Turret, UnitType.Motorbike,
            UnitType.MotorbikeNuclear, UnitType.DumpTruck,
            // Civilians and animals
            UnitType.Pig, UnitType.RottenPig, UnitType.Villager,
            // Other
            UnitType.RCCar
        };

        /// <summary>
        /// Gets the UnitType of a TestVanDammeAnim character based on its MookType and specific class type.
        /// </summary>
        /// <param name="character">The TestVanDammeAnim character to identify.</param>
        /// <returns>The UnitType of the character.</returns>
        public static UnitType GetUnitType(this TestVanDammeAnim character)
        {
            switch (character.GetMookType())
            {
                case MookType.None:
                    if (character is BroBase)
                        return UnitType.Bro;
                    if (character is HellLostSoul)
                        return UnitType.LostSoul;
                    if (character is Villager)
                        return UnitType.Villager;
                    Animal animal = character as Animal;
                    // 2 different units use the Animal class
                    if (animal != null)
                    {
                        if (animal.isRotten)
                            return UnitType.RottenPig;
                        else
                            return UnitType.Pig;
                    }
                    RemoteControlExplosiveCar car = character as RemoteControlExplosiveCar;
                    if (car != null)
                    {
                        return UnitType.RCCar;
                    }
                    break;
                case MookType.Trooper:
                    if (character is MookJetpack)
                        return UnitType.JetpackMook;
                    if (character is MookNinja)
                        return UnitType.NinjaMook;
                    // 3 different enemies use the MookTrooper class
                    if (character is MookTrooper)
                    {
                        Traverse trav = Traverse.Create(character);
                        if ((bool)trav.GetFieldValue("randomizeDancingFramesRow"))
                        {
                            return UnitType.Mook;
                        }
                        int dancingFrames = (int)trav.GetFieldValue("dancingFrames");
                        if (dancingFrames == 11)
                            return UnitType.StrongMook;
                        else
                            return UnitType.ScientistMook;
                    }
                    if (character is SkinnedMook)
                        return UnitType.SkinnedMook;
                    if (character is MookGeneral)
                        return UnitType.Alarmist;
                    break;
                case MookType.Suicide:
                    return UnitType.SuicideMook;
                case MookType.BigGuy:
                    if (character is MookSuicide)
                        return UnitType.SuicideBruiser;
                    if (character is MookBigGuyElite)
                        return UnitType.EliteBruiser;
                    if (character is SatanMiniboss)
                        return UnitType.SatanMiniboss;
                    if (character is DolphLundrenSoldier)
                        return UnitType.CR666;
                    // 2 different enemies use the MookBigGuy class
                    if (character is MookBigGuy)
                    {
                        if (character.maxHealth == 25 || (character.maxHealth == -1 && character.health == 25))
                            return UnitType.Bruiser;
                        else
                            return UnitType.StrongBruiser;
                    }
                    break;
                case MookType.Scout:
                    Mook mook = character as Mook;
                    if (mook != null)
                    {
                        if (!mook.canLandOnFace)
                            return UnitType.ScoutMook;
                        else
                            return UnitType.TreasureMook;
                    }
                    break;
                case MookType.Dog:
                    return UnitType.AttackDog;
                case MookType.Devil:
                    return UnitType.Satan;
                case MookType.RiotShield:
                    return UnitType.RiotShieldMook;
                case MookType.Alien:
                    if (character is AlienBrute)
                        return UnitType.Brute;
                    // 2 different enemies use the AlienXenomorph class
                    AlienXenomorph xenomorph = character as AlienXenomorph;
                    if (xenomorph != null)
                    {
                        if (!xenomorph.hasBrainBox)
                            return UnitType.Xenomorph;
                        else
                            return UnitType.XenomorphBrainbox;
                    }
                    break;
                case MookType.Grenadier:
                    return UnitType.GrenadierMook;
                case MookType.Villager:
                    // Unused by villagers in the game normally
                    return UnitType.Villager;
                case MookType.General:
                    return UnitType.MookGeneral;
                case MookType.Bazooka:
                    if (character is MookJetpackBazooka)
                        return UnitType.JetpackBazookaMook;
                    if (character is MookBazooka)
                        return UnitType.BazookaMook;
                    break;
                case MookType.FaceHugger:
                    AlienFaceHugger facehugger = character as AlienFaceHugger;
                    if (facehugger != null)
                    {
                        if (facehugger.layEggsInsideBros)
                            return UnitType.Facehugger;
                        else
                            return UnitType.Snake;
                    }
                    break;
                case MookType.Melter:
                    if (character is AlienMelter)
                        return UnitType.Screecher;
                    if (character is AlienMosquito)
                        return UnitType.Baneling;
                    break;
                case MookType.UndeadTrooper:
                    return UnitType.UndeadMook;
                case MookType.UndeadSuicide:
                    return UnitType.UndeadSuicideMook;
                case MookType.Warlock:
                    return UnitType.Warlock;
                case MookType.Boomer:
                    if (character is MookHellSoulCatcher)
                        return UnitType.SoulCatcher;
                    if (character is MookHellBoomer)
                        return UnitType.Boomer;
                    break;
                case MookType.HellDog:
                    return UnitType.Hellhound;
                case MookType.HellBigGuy:
                    return UnitType.Executioner;
                case MookType.ArmouredGuy:
                    if (character.maxHealth == 65 || (character.maxHealth == -1 && character.health == 65))
                        return UnitType.BrownMech;
                    else
                        return UnitType.Mech;
                case MookType.Vehicle:
                    // No mooks use this in game
                    return UnitType.None;
                default:
                    return UnitType.None;
            }
            return UnitType.None;
        }

        /// <summary>
        /// Gets the UnitType of any Unit, including both TestVanDammeAnim and non-TestVanDammeAnim types.
        /// </summary>
        /// <param name="unit">The Unit to identify.</param>
        /// <returns>The UnitType of the unit.</returns>
        public static UnitType GetUnitType(this Unit unit)
        {
            TestVanDammeAnim character = unit as TestVanDammeAnim;
            if (character != null)
                return character.GetUnitType();

            // Handle non-TestVanDammeAnim enemies using type matching
            switch (unit)
            {
                case TankBig _:
                    return UnitType.StealthTank;
                case MammothKopter _:
                    return UnitType.Megacockter;
                case Mookopter _:
                    return UnitType.Terrorkopter;
                case GoliathMech _:
                    return UnitType.Terrorbot;
                case AlienGiantSandWorm _:
                    return UnitType.SandWorm;
                case HellBoneWormMiniboss boneworm:
                    if (boneworm.name.Contains("Behind") || boneworm.activationOffset.x == 250)
                    {
                        return UnitType.BonewormBehind;
                    }
                    else
                    {
                        return UnitType.Boneworm;
                    }
                case AlienWormFacehuggerLauncher facehuggerLauncher:
                    if (facehuggerLauncher.name.Contains("Behind") || facehuggerLauncher.activationOffset.x == 250)
                    {
                        return UnitType.AlienFacehuggerWormBehind;
                    }
                    else
                    {
                        return UnitType.AlienFacehuggerWorm;
                    }
                case AlienGiantBossSandworm _:
                    return UnitType.LargeAlienWorm;
                case AlienMinibossSandWorm _:
                    return UnitType.AlienWorm;
                case MookDumpTruck _:
                    return UnitType.DumpTruck;
                case MookArtilleryTruck _:
                    return UnitType.ArtilleryTruck;
                case MookBlimp _:
                    return UnitType.Blimp;
                case DrillCarrier _:
                    return UnitType.DrillCarrier;
                case MookTruck _:
                    return UnitType.MookTruck;
                case MookMotorBike motorbike:
                    if (motorbike.nuclearBombSprite != null)
                    {
                        return UnitType.MotorbikeNuclear;
                    }
                    else
                    {
                        return UnitType.Motorbike;
                    }
                case MookGunplacement _:
                    return UnitType.Turret;
                case Tank tank:
                    switch (tank.weapon)
                    {
                        case TankrocketBattery _:
                            return UnitType.RocketTank;
                        case TankCannon _:
                            return UnitType.CannonTank;
                        case TankMookLauncher _:
                            return UnitType.MookLauncherTank;
                    }
                    return UnitType.None;
                default:
                    return UnitType.None;
            }
        }

        /// <summary>
        /// Gets the TestVanDammeAnim prefab for the specified unit type.
        /// </summary>
        /// <param name="type">The unit type to get the prefab for.</param>
        /// <param name="villagerNum">For villagers, specifies which villager variant to return. -1 for random.</param>
        /// <param name="startDead">For undead mooks, whether to get the lying-down variant.</param>
        /// <returns>The TestVanDammeAnim prefab, or null if the type doesn't have one.</returns>
        public static TestVanDammeAnim GetTestVanDammeAnimPrefab(this UnitType type, int villagerNum = -1, bool startDead = false)
        {
            switch (type)
            {
                case UnitType.Mook:
                    return Map.Instance.activeTheme.mook;
                case UnitType.SuicideMook:
                    return Map.Instance.activeTheme.mookSuicide;
                case UnitType.Bruiser:
                    return Map.Instance.activeTheme.mookBigGuy;
                case UnitType.SuicideBruiser:
                    return Map.Instance.activeTheme.mookSuicideBigGuy;
                case UnitType.StrongBruiser:
                    return Map.Instance.sharedObjectsReference.Asset.mookBigGuyStrong;
                case UnitType.EliteBruiser:
                    return Map.Instance.activeTheme.mookBigGuyElite;
                case UnitType.ScoutMook:
                    return Map.Instance.activeTheme.mookScout;
                case UnitType.RiotShieldMook:
                    return Map.Instance.activeTheme.mookRiotShield;
                case UnitType.Mech:
                    return Map.Instance.activeTheme.mookArmoured;
                case UnitType.BrownMech:
                    return Map.Instance.sharedObjectsReference.Asset.mechBrown;
                case UnitType.JetpackMook:
                    return Map.Instance.sharedObjectsReference.Asset.mookJetpack;
                case UnitType.GrenadierMook:
                    return Map.Instance.activeTheme.mookGrenadier;
                case UnitType.BazookaMook:
                    return Map.Instance.activeTheme.mookBazooka;
                case UnitType.JetpackBazookaMook:
                    return Map.Instance.activeTheme.mookJetpackBazooka;
                case UnitType.NinjaMook:
                    return Map.Instance.activeTheme.mookNinja;
                case UnitType.TreasureMook:
                    return Map.Instance.sharedObjectsReference.Asset.treasureMook;
                case UnitType.AttackDog:
                    return Map.Instance.activeTheme.mookDog;
                case UnitType.SkinnedMook:
                    return Map.Instance.activeTheme.skinnedMook;
                case UnitType.MookGeneral:
                    return Map.Instance.activeTheme.mookGeneral;
                case UnitType.Alarmist:
                    return Map.Instance.activeTheme.mookAlarmist;
                case UnitType.StrongMook:
                    return Map.Instance.activeTheme.mookStrong;
                case UnitType.ScientistMook:
                    return Map.Instance.activeTheme.mookScientist;
                case UnitType.Snake:
                    return Map.Instance.activeTheme.snake;
                case UnitType.Satan:
                    return Map.Instance.activeTheme.satan;
                case UnitType.Facehugger:
                    return Map.Instance.activeTheme.alienFaceHugger;
                case UnitType.Xenomorph:
                    return Map.Instance.activeTheme.alienXenomorph;
                case UnitType.Brute:
                    return Map.Instance.activeTheme.alienBrute;
                case UnitType.Screecher:
                    return Map.Instance.activeTheme.alienBaneling;
                case UnitType.Baneling:
                    return Map.Instance.activeTheme.alienMosquito;
                case UnitType.XenomorphBrainbox:
                    return Map.Instance.activeTheme.mookXenomorphBrainbox;
                case UnitType.Hellhound:
                    return Map.Instance.sharedObjectsReference.Asset.hellEnemies[0].GetComponent<TestVanDammeAnim>();
                case UnitType.UndeadMook:
                    if (!startDead)
                        return Map.Instance.sharedObjectsReference.Asset.hellEnemies[1].GetComponent<TestVanDammeAnim>();
                    else
                        return Map.Instance.sharedObjectsReference.Asset.hellEnemies[2].GetComponent<TestVanDammeAnim>();
                case UnitType.Warlock:
                    return Map.Instance.sharedObjectsReference.Asset.hellEnemies[3].GetComponent<TestVanDammeAnim>();
                case UnitType.Boomer:
                    return Map.Instance.sharedObjectsReference.Asset.hellEnemies[4].GetComponent<TestVanDammeAnim>();
                case UnitType.UndeadSuicideMook:
                    return Map.Instance.sharedObjectsReference.Asset.hellEnemies[5].GetComponent<TestVanDammeAnim>();
                case UnitType.Executioner:
                    return Map.Instance.sharedObjectsReference.Asset.hellEnemies[6].GetComponent<TestVanDammeAnim>();
                case UnitType.LostSoul:
                    return Map.Instance.sharedObjectsReference.Asset.hellEnemies[8].GetComponent<TestVanDammeAnim>();
                case UnitType.SoulCatcher:
                    return Map.Instance.sharedObjectsReference.Asset.hellEnemies[10].GetComponent<TestVanDammeAnim>();
                case UnitType.SatanMiniboss:
                    return Map.Instance.sharedObjectsReference.Asset.satanMiniboss as TestVanDammeAnim;
                case UnitType.CR666:
                    return Map.Instance.activeTheme.mookDolfLundgren;
                case UnitType.Pig:
                    return Map.Instance.activeTheme.animals[0].GetComponent<TestVanDammeAnim>();
                case UnitType.RottenPig:
                    return Map.Instance.activeTheme.animals[2].GetComponent<TestVanDammeAnim>();
                case UnitType.Villager:
                    if (villagerNum == -1)
                        return Map.Instance.activeTheme.villager1[UnityEngine.Random.Range(0, 1)];
                    else
                        return Map.Instance.activeTheme.villager1[villagerNum];
                case UnitType.RCCar:
                    return Map.Instance.remoteCarPrefabReference.Asset;
                // Non-TestVanDammeAnim enemies don't have TestVanDammeAnim prefabs
                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets the Unit prefab for any unit type, including both TestVanDammeAnim and non-TestVanDammeAnim types.
        /// </summary>
        /// <param name="type">The unit type to get the prefab for.</param>
        /// <param name="villagerNum">For villagers, specifies which villager variant to return. -1 for random.</param>
        /// <param name="startDead">For undead mooks, whether to get the lying-down variant.</param>
        /// <returns>The Unit prefab for spawning.</returns>
        public static Unit GetUnitPrefab(this UnitType type, int villagerNum = -1, bool startDead = false)
        {
            TestVanDammeAnim testVanDammeAnimPrefab = type.GetTestVanDammeAnimPrefab(villagerNum, startDead);
            if (testVanDammeAnimPrefab != null)
                return testVanDammeAnimPrefab;

            // Handle non-TestVanDammeAnim enemies
            switch (type)
            {
                case UnitType.StealthTank:
                    return Map.Instance.activeTheme.mookMammothTank;
                case UnitType.Terrorkopter:
                    return Map.Instance.activeTheme.mookKopterMiniBoss;
                case UnitType.Terrorbot:
                    return Map.Instance.activeTheme.goliathMech;
                case UnitType.Megacockter:
                    return Map.Instance.activeTheme.mookKopterMammoth;
                case UnitType.SandWorm:
                    return Map.Instance.sharedObjectsReference.Asset.hellEnemies[7].GetComponent<Unit>();
                case UnitType.Boneworm:
                    return Map.Instance.sharedObjectsReference.Asset.hellEnemies[12].GetComponent<Unit>();
                case UnitType.BonewormBehind:
                    return Map.Instance.sharedObjectsReference.Asset.hellEnemies[13].GetComponent<Unit>();
                case UnitType.AlienWorm:
                    return Map.Instance.sharedObjectsReference.Asset.alienMinibossSandWorm;
                case UnitType.AlienFacehuggerWorm:
                    return Map.Instance.sharedObjectsReference.Asset.alienSandWormFacehuggerSpitter;
                case UnitType.AlienFacehuggerWormBehind:
                    return Map.Instance.sharedObjectsReference.Asset.alienSandWormFacehuggerSpitterBehind;
                case UnitType.LargeAlienWorm:
                    return Map.Instance.sharedObjectsReference.Asset.alienGiantSandWormBoss;
                case UnitType.MookLauncherTank:
                    return Map.Instance.activeTheme.mookTankMookLauncher;
                case UnitType.CannonTank:
                    return Map.Instance.activeTheme.mookTankCannon;
                case UnitType.RocketTank:
                    return Map.Instance.activeTheme.mookTankRockets;
                case UnitType.ArtilleryTruck:
                    return Map.Instance.activeTheme.mookArtilleryTruck;
                case UnitType.Blimp:
                    return Map.Instance.activeTheme.mookBlimp;
                case UnitType.DrillCarrier:
                    return Map.Instance.activeTheme.mookDrillCarrier;
                case UnitType.MookTruck:
                    return Map.Instance.activeTheme.mookTruck;
                case UnitType.Turret:
                    return Map.Instance.activeTheme.sandbag;
                case UnitType.Motorbike:
                    return Map.Instance.sharedObjectsReference.Asset.mookMotorBike;
                case UnitType.MotorbikeNuclear:
                    return Map.Instance.sharedObjectsReference.Asset.mookMotorBikeNuclear;
                case UnitType.DumpTruck:
                    return Map.Instance.sharedObjectsReference.Asset.mookDumpTruck;

                default:
                    return null;
            }
        }

        /// <summary>
        /// Converts a UnitType to its human-readable string representation.
        /// </summary>
        /// <param name="type">The UnitType to convert.</param>
        /// <returns>The display name of the unit type.</returns>
        public static string ToDisplayString(this UnitType type)
        {
            switch (type)
            {
                case UnitType.None:
                    return "None";
                case UnitType.Bro:
                    return "Bro";
                case UnitType.Mook:
                    return "Mook";
                case UnitType.SuicideMook:
                    return "Suicide Mook";
                case UnitType.Bruiser:
                    return "Bruiser";
                case UnitType.SuicideBruiser:
                    return "Suicide Bruiser";
                case UnitType.StrongBruiser:
                    return "Strong Bruiser";
                case UnitType.EliteBruiser:
                    return "Elite Bruiser";
                case UnitType.ScoutMook:
                    return "Scout Mook";
                case UnitType.RiotShieldMook:
                    return "Riot Shield Mook";
                case UnitType.Mech:
                    return "Mech";
                case UnitType.BrownMech:
                    return "Brown Mech";
                case UnitType.JetpackMook:
                    return "Jetpack Mook";
                case UnitType.GrenadierMook:
                    return "Grenadier Mook";
                case UnitType.BazookaMook:
                    return "Bazooka Mook";
                case UnitType.JetpackBazookaMook:
                    return "Jetpack Bazooka Mook";
                case UnitType.NinjaMook:
                    return "Ninja Mook";
                case UnitType.TreasureMook:
                    return "Treasure Mook";
                case UnitType.AttackDog:
                    return "Attack Dog";
                case UnitType.SkinnedMook:
                    return "Skinned Mook";
                case UnitType.MookGeneral:
                    return "Mook General";
                case UnitType.Alarmist:
                    return "Alarmist";
                case UnitType.StrongMook:
                    return "Strong Mook";
                case UnitType.ScientistMook:
                    return "Scientist Mook";
                case UnitType.Snake:
                    return "Snake";
                case UnitType.Satan:
                    return "Satan";
                case UnitType.Facehugger:
                    return "Facehugger";
                case UnitType.Xenomorph:
                    return "Xenomorph";
                case UnitType.Brute:
                    return "Brute";
                case UnitType.Screecher:
                    return "Screecher";
                case UnitType.Baneling:
                    return "Baneling";
                case UnitType.XenomorphBrainbox:
                    return "Xenomorph Brainbox";
                case UnitType.Hellhound:
                    return "Hellhound";
                case UnitType.UndeadMook:
                    return "Undead Mook";
                case UnitType.Warlock:
                    return "Warlock";
                case UnitType.Boomer:
                    return "Boomer";
                case UnitType.UndeadSuicideMook:
                    return "Undead Suicide Mook";
                case UnitType.Executioner:
                    return "Executioner";
                case UnitType.LostSoul:
                    return "Lost Soul";
                case UnitType.SoulCatcher:
                    return "Soul Catcher";
                case UnitType.SatanMiniboss:
                    return "Satan Miniboss";
                case UnitType.CR666:
                    return "CR666";
                case UnitType.Pig:
                    return "Pig";
                case UnitType.RottenPig:
                    return "Rotten Pig";
                case UnitType.Villager:
                    return "Villager";
                case UnitType.RCCar:
                    return "Remote Control Car";
                case UnitType.StealthTank:
                    return "Stealth Tank";
                case UnitType.Terrorkopter:
                    return "Terrorkopter";
                case UnitType.Terrorbot:
                    return "Terrorbot";
                case UnitType.Megacockter:
                    return "Megacockter";
                case UnitType.SandWorm:
                    return "Sand Worm";
                case UnitType.Boneworm:
                    return "Boneworm";
                case UnitType.BonewormBehind:
                    return "Boneworm Behind";
                case UnitType.AlienWorm:
                    return "Alien Worm";
                case UnitType.AlienFacehuggerWorm:
                    return "Alien Facehugger Worm";
                case UnitType.AlienFacehuggerWormBehind:
                    return "Alien Facehugger Worm Behind";
                case UnitType.LargeAlienWorm:
                    return "Large Alien Worm";
                case UnitType.MookLauncherTank:
                    return "Mook Launcher Tank";
                case UnitType.CannonTank:
                    return "Cannon Tank";
                case UnitType.RocketTank:
                    return "Rocket Tank";
                case UnitType.ArtilleryTruck:
                    return "Artillery Truck";
                case UnitType.Blimp:
                    return "Blimp";
                case UnitType.DrillCarrier:
                    return "Drill Carrier";
                case UnitType.MookTruck:
                    return "Mook Truck";
                case UnitType.Turret:
                    return "Turret";
                case UnitType.Motorbike:
                    return "Motorbike";
                case UnitType.MotorbikeNuclear:
                    return "Motorbike Nuclear";
                case UnitType.DumpTruck:
                    return "Dump Truck";
            }
            return "None";
        }

        /// <summary>
        /// Gets the display name for a UnitType. This is a static method alternative to the extension method.
        /// </summary>
        /// <param name="type">The UnitType to get the display name for.</param>
        /// <returns>The display name of the unit type.</returns>
        public static string GetDisplayName(UnitType type)
        {
            return type.ToDisplayString();
        }

        /// <summary>
        /// Converts a string representation to its corresponding UnitType.
        /// </summary>
        /// <param name="type">The string name of the unit type.</param>
        /// <returns>The corresponding UnitType, or UnitType.None if not found.</returns>
        public static UnitType ToUnitType(string type)
        {
            switch (type)
            {
                case "None":
                    return UnitType.None;
                case "Bro":
                    return UnitType.Bro;
                case "Mook":
                    return UnitType.Mook;
                case "Suicide Mook":
                    return UnitType.SuicideMook;
                case "Bruiser":
                    return UnitType.Bruiser;
                case "Suicide Bruiser":
                    return UnitType.SuicideBruiser;
                case "Strong Bruiser":
                    return UnitType.StrongBruiser;
                case "Elite Bruiser":
                    return UnitType.EliteBruiser;
                case "Scout Mook":
                    return UnitType.ScoutMook;
                case "Riot Shield Mook":
                    return UnitType.RiotShieldMook;
                case "Mech":
                    return UnitType.Mech;
                case "Brown Mech":
                    return UnitType.BrownMech;
                case "Jetpack Mook":
                    return UnitType.JetpackMook;
                case "Grenadier Mook":
                    return UnitType.GrenadierMook;
                case "Bazooka Mook":
                    return UnitType.BazookaMook;
                case "Jetpack Bazooka Mook":
                    return UnitType.JetpackBazookaMook;
                case "Ninja Mook":
                    return UnitType.NinjaMook;
                case "Treasure Mook":
                    return UnitType.TreasureMook;
                case "Attack Dog":
                    return UnitType.AttackDog;
                case "Skinned Mook":
                    return UnitType.SkinnedMook;
                case "Mook General":
                    return UnitType.MookGeneral;
                case "Alarmist":
                    return UnitType.Alarmist;
                case "Strong Mook":
                    return UnitType.StrongMook;
                case "Scientist Mook":
                    return UnitType.ScientistMook;
                case "Snake":
                    return UnitType.Snake;
                case "Satan":
                    return UnitType.Satan;
                case "Facehugger":
                    return UnitType.Facehugger;
                case "Xenomorph":
                    return UnitType.Xenomorph;
                case "Brute":
                    return UnitType.Brute;
                case "Screecher":
                    return UnitType.Screecher;
                case "Baneling":
                    return UnitType.Baneling;
                case "Xenomorph Brainbox":
                    return UnitType.XenomorphBrainbox;
                case "Hellhound":
                    return UnitType.Hellhound;
                case "Undead Mook":
                    return UnitType.UndeadMook;
                case "Warlock":
                    return UnitType.Warlock;
                case "Boomer":
                    return UnitType.Boomer;
                case "Undead Suicide Mook":
                    return UnitType.UndeadSuicideMook;
                case "Executioner":
                    return UnitType.Executioner;
                case "Lost Soul":
                    return UnitType.LostSoul;
                case "Soul Catcher":
                    return UnitType.SoulCatcher;
                case "Satan Miniboss":
                    return UnitType.SatanMiniboss;
                case "CR666":
                    return UnitType.CR666;
                case "Pig":
                    return UnitType.Pig;
                case "Rotten Pig":
                    return UnitType.RottenPig;
                case "Villager":
                    return UnitType.Villager;
                case "Remote Control Car":
                    return UnitType.RCCar;
                case "Stealth Tank":
                    return UnitType.StealthTank;
                case "Terrorkopter":
                    return UnitType.Terrorkopter;
                case "Terrorbot":
                    return UnitType.Terrorbot;
                case "Megacockter":
                    return UnitType.Megacockter;
                case "Sand Worm":
                    return UnitType.SandWorm;
                case "Boneworm":
                    return UnitType.Boneworm;
                case "Boneworm Behind":
                    return UnitType.BonewormBehind;
                case "Alien Worm":
                    return UnitType.AlienWorm;
                case "Alien Facehugger Worm":
                    return UnitType.AlienFacehuggerWorm;
                case "Alien Facehugger Worm Behind":
                    return UnitType.AlienFacehuggerWormBehind;
                case "Large Alien Worm":
                    return UnitType.LargeAlienWorm;
                case "Mook Launcher Tank":
                    return UnitType.MookLauncherTank;
                case "Cannon Tank":
                    return UnitType.CannonTank;
                case "Rocket Tank":
                    return UnitType.RocketTank;
                case "Artillery Truck":
                    return UnitType.ArtilleryTruck;
                case "Blimp":
                    return UnitType.Blimp;
                case "Drill Carrier":
                    return UnitType.DrillCarrier;
                case "Mook Truck":
                    return UnitType.MookTruck;
                case "Turret":
                    return UnitType.Turret;
                case "Motorbike":
                    return UnitType.Motorbike;
                case "Motorbike Nuclear":
                    return UnitType.MotorbikeNuclear;
                case "Dump Truck":
                    return UnitType.DumpTruck;
            }
            return UnitType.None;
        }

        /// <summary>
        /// Determines if the unit type has a special ability (first special slot).
        /// </summary>
        /// <param name="type">The unit type to check.</param>
        /// <returns>True if the unit has a special ability.</returns>
        public static bool HasSpecial(this UnitType type)
        {
            switch (type)
            {
                case UnitType.Mech:
                case UnitType.BrownMech:
                case UnitType.AttackDog:
                case UnitType.MookGeneral:
                case UnitType.Alarmist:
                case UnitType.Snake:
                case UnitType.Satan:
                case UnitType.Facehugger:
                case UnitType.Hellhound:
                case UnitType.UndeadSuicideMook:
                case UnitType.SatanMiniboss:
                case UnitType.CR666:
                case UnitType.Villager:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if the unit type has a second special ability.
        /// </summary>
        /// <param name="type">The unit type to check.</param>
        /// <returns>True if the unit has a second special ability.</returns>
        public static bool HasSpecial2(this UnitType type)
        {
            switch (type)
            {
                case UnitType.SuicideMook:
                case UnitType.SuicideBruiser:
                case UnitType.SatanMiniboss:
                case UnitType.CR666:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if the unit type has a third special ability.
        /// </summary>
        /// <param name="type">The unit type to check.</param>
        /// <returns>True if the unit has a third special ability.</returns>
        public static bool HasSpecial3(this UnitType type)
        {
            switch (type)
            {
                case UnitType.CR666:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if the unit type can perform a dance/taunt animation.
        /// </summary>
        /// <param name="type">The unit type to check.</param>
        /// <returns>True if the unit can dance.</returns>
        public static bool CanDance(this UnitType type)
        {
            switch (type)
            {
                case UnitType.EliteBruiser:
                case UnitType.BrownMech:
                case UnitType.JetpackBazookaMook:
                case UnitType.Pig:
                case UnitType.RottenPig:
                case UnitType.Villager:
                case UnitType.RCCar:
                // Boss and vehicle enemies can't dance
                case UnitType.StealthTank:
                case UnitType.Terrorkopter:
                case UnitType.Terrorbot:
                case UnitType.SandWorm:
                case UnitType.Boneworm:
                case UnitType.BonewormBehind:
                case UnitType.AlienWorm:
                case UnitType.AlienFacehuggerWorm:
                case UnitType.AlienFacehuggerWormBehind:
                case UnitType.LargeAlienWorm:
                case UnitType.MookLauncherTank:
                case UnitType.CannonTank:
                case UnitType.RocketTank:
                case UnitType.ArtilleryTruck:
                case UnitType.Blimp:
                case UnitType.DrillCarrier:
                case UnitType.MookTruck:
                case UnitType.Turret:
                case UnitType.Motorbike:
                case UnitType.MotorbikeNuclear:
                case UnitType.DumpTruck:
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Determines if the unit type is a suicide unit.
        /// </summary>
        /// <param name="type">The unit type to check.</param>
        /// <returns>True if the unit is a suicide type.</returns>
        public static bool IsSuicideUnit(this UnitType type)
        {
            switch (type)
            {
                case UnitType.Baneling:
                case UnitType.LostSoul:
                case UnitType.Screecher:
                case UnitType.SuicideBruiser:
                case UnitType.SuicideMook:
                case UnitType.UndeadSuicideMook:
                case UnitType.RCCar:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines if the unit type derives from the TestVanDammeAnim base class.
        /// </summary>
        /// <param name="type">The unit type to check.</param>
        /// <returns>True if the unit type derives from TestVanDammeAnim, false for vehicles, bosses, and worms.</returns>
        public static bool IsTestVanDammeAnimType(this UnitType type)
        {
            switch (type)
            {
                // Non-TestVanDammeAnim types
                case UnitType.StealthTank:
                case UnitType.Terrorkopter:
                case UnitType.Terrorbot:
                case UnitType.Megacockter:
                case UnitType.SandWorm:
                case UnitType.Boneworm:
                case UnitType.BonewormBehind:
                case UnitType.AlienWorm:
                case UnitType.AlienFacehuggerWorm:
                case UnitType.AlienFacehuggerWormBehind:
                case UnitType.LargeAlienWorm:
                case UnitType.MookLauncherTank:
                case UnitType.CannonTank:
                case UnitType.RocketTank:
                case UnitType.ArtilleryTruck:
                case UnitType.Blimp:
                case UnitType.DrillCarrier:
                case UnitType.MookTruck:
                case UnitType.Turret:
                case UnitType.Motorbike:
                case UnitType.MotorbikeNuclear:
                case UnitType.DumpTruck:
                    return false;
                case UnitType.None:
                    return false;
                default:
                    // All other types derive from TestVanDammeAnim
                    return true;
            }
        }

        /// <summary>
        /// Gets the sprite width for the specified unit type.
        /// </summary>
        /// <param name="type">The unit type to get sprite width for.</param>
        /// <returns>The width of the unit's sprite in pixels.</returns>
        public static float GetSpriteWidth(this UnitType type)
        {
            switch (type)
            {
                case UnitType.Bruiser:
                case UnitType.EliteBruiser:
                case UnitType.Executioner:
                case UnitType.StrongBruiser:
                case UnitType.SuicideBruiser:
                case UnitType.Boomer:
                case UnitType.SoulCatcher:
                    return 36f;
                case UnitType.Mech:
                case UnitType.BrownMech:
                case UnitType.CR666:
                case UnitType.Xenomorph:
                case UnitType.XenomorphBrainbox:
                    return 48f;
                case UnitType.Brute:
                case UnitType.SatanMiniboss:
                    return 64;
                default:
                    return 32f;
            }
        }

        /// <summary>
        /// Gets the sprite height for the specified unit type.
        /// </summary>
        /// <param name="type">The unit type to get sprite height for.</param>
        /// <returns>The height of the unit's sprite in pixels.</returns>
        public static float GetSpriteHeight(this UnitType type)
        {
            switch (type)
            {
                case UnitType.Boomer:
                case UnitType.SoulCatcher:
                    return 36f;
                case UnitType.Mech:
                case UnitType.BrownMech:
                case UnitType.CR666:
                case UnitType.Xenomorph:
                case UnitType.XenomorphBrainbox:
                    return 48f;
                case UnitType.Brute:
                case UnitType.SatanMiniboss:
                    return 64;
                default:
                    return 32f;
            }
        }
    }
}
