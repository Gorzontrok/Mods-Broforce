using HarmonyLib;
using UnityEngine;

namespace RocketLib.Utils
{
    public static class TestVanDammeAnimTypes
    {
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
            Villager = 44
        }

        public static readonly string[] allUnitNames = new string[]
        {
            "Mook", "Suicide Mook", "Bruiser", "Suicide Bruiser", "Strong Bruiser", "Elite Bruiser", "Scout Mook", "Riot Shield Mook", "Mech", "Brown Mech", "Jetpack Mook", "Grenadier Mook", "Bazooka Mook", "Jetpack Bazooka Mook",
            "Ninja Mook", "Treasure Mook", "Attack Dog", "Skinned Mook", "Mook General", "Alarmist", "Strong Mook", "Scientist Mook", "Snake", "Satan", "Facehugger", "Xenomorph", "Brute", "Screecher", "Baneling", "Xenomorph Brainbox",
            "Hellhound", "Undead Mook", "Warlock", "Boomer", "Undead Suicide Mook", "Executioner", "Lost Soul", "Soul Catcher", "Satan Miniboss", "CR666", "Pig", "Rotten Pig", "Villager"
        };

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
                        Vector2Int dancingFrames = (Vector2Int)trav.GetFieldValue("originalDancingFrames");
                        if (dancingFrames.x == 0)
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
                        if ( character.maxHealth == 25 )
                            return UnitType.Bruiser;
                        else
                            return UnitType.StrongBruiser;
                    }
                    break;
                case MookType.Scout:
                    Mook mook = character as Mook;
                    if ( mook != null )
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
                    if ( facehugger != null )
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
                    if (character.maxHealth == 65)
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

        public static TestVanDammeAnim GetUnitPrefab(this UnitType type, int villagerNum = -1, bool startDead = false)
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
                    if ( !startDead )
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
                    if ( villagerNum == -1 )
                        return Map.Instance.activeTheme.villager1[UnityEngine.Random.Range(0, 1)];
                    else
                        return Map.Instance.activeTheme.villager1[villagerNum];
            }
            return null;
        }

        public static string ToString(this UnitType type)
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
            }
            return "None";
        }

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
            }
            return UnitType.None;
        }

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

        public static bool HasSpecial3(this UnitType type)
        {
            switch (type)
            {
                case UnitType.CR666:
                    return true;
            }
            return false;
        }
    }
}
