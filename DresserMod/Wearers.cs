using System;

namespace DresserMod
{
    public static class Wearers
    {
        public static string GetWearerName(object obj)
        {
            if(obj is Citizen)
            {
                var citizen = obj as Citizen;
                if(citizen.gameObject.name.Contains(PRESIDANT_US_GAMEOBJECT))
                    return PRESIDANT_US;
            }
            else if(obj is MookTrooper)
            {
                var trooper = obj as MookTrooper;
                if(trooper.gameObject.name.Contains(SCIENTIST_GAMEOBJECT))
                    return SCIENTIST;
                else if(trooper.gameObject.name.Contains(MOOK_STRONG_GAMEOBJECT))
                    return MOOK_STRONG;
                else if(trooper.gameObject.name.Contains(AGENT_GAMEOBJECT))
                    return AGENT;
            }
            else if (obj is AlienXenomorph)
            {
                var xeno = obj as AlienXenomorph;
                if(xeno.gameObject.name.Contains(XENOMORPHE_BRAINBOX_GAMEOBJECT))
                    return XENOMORPHE_BRAINBOX;
            }
            else if(obj is MookGeneral)
            {
                var general = obj as MookGeneral;
                if (general.gameObject.name.Contains(ALARMIST_GAMEOBJECT))
                    return ALARMIST;
            }
            else if (obj is ScoutMook)
            {
                var scout = obj as ScoutMook;
                if(scout.gameObject.name.Contains(MOOK_TREASURE_GAMEOBJECT))
                    return MOOK_TREASURE;
            }
            else if (obj is MookBigGuy)
            {
                var bigGuy = obj as MookBigGuy;
                if(bigGuy.gameObject.name.Contains(BRUISER_STRONG_GAMEOBJECT))
                    return BRUISER_STRONG;
            }
            else if (obj is MookArmouredGuy)
            {
                var armouredGuy = obj as MookArmouredGuy;
                if(armouredGuy.gameObject.name.Contains(MECH_BROWN_GAMEOBJECT))
                    return MECH_BROWN;
            }
            else if (obj is MookSuicide)
            {
                var suicide = obj as MookSuicide;
                if(suicide.gameObject.name.Contains(SUICIDE_BRUISER_GAMEOBJECT))
                    return SUICIDE_BRUISER;
            }
            else if (obj is Animal)
            {
                var animal = obj as Animal;
                if(animal.gameObject.name.Contains(SICK_PIG_GAMEOBJECT))
                    return SICK_PIG;
            }
            else if (obj is AlienFaceHugger)
            {
                var animal = obj as AlienFaceHugger;
                if(animal.gameObject.name.Contains(SNAKE_GAMEOBJECT))
                    return SNAKE;
            }
            else if (obj is Villager)
            {
                var animal = obj as Villager;
                if(animal.gameObject.name.Contains(DENIZEN))
                    return DENIZEN;
            }
            return obj.GetType().Name;
        }
        #region Characters

        #region Mooks
        public const string SCIENTIST = "Scientist";
        private const string SCIENTIST_GAMEOBJECT = "ZMookScientist";
        public const string ALARMIST = "Alarmist";
        private const string ALARMIST_GAMEOBJECT = "ZMook Alarmist";
        public const string MOOK_STRONG = "MookStrong";
        private const string MOOK_STRONG_GAMEOBJECT = "ZMook Strong";
        public const string MOOK_TREASURE = "MookTreasure";
        private const string MOOK_TREASURE_GAMEOBJECT = "ZMookTreasure";
        public const string BRUISER_STRONG = "BruiserStrong";
        private const string BRUISER_STRONG_GAMEOBJECT = "ZMookBigGuyStrong";
        public const string MECH_BROWN = "MechBrown";
        private const string MECH_BROWN_GAMEOBJECT = "ZMookArmouredGuy Brown";
        public const string SUICIDE_BRUISER = "SuicideBruiser";
        private const string SUICIDE_BRUISER_GAMEOBJECT = "ZMookSuicideBigGuy";

        public const string SNAKE = "Snake";
        private const string SNAKE_GAMEOBJECT = "ZSnake";

        #region Alien
        public const string XENOMORPHE_BRAINBOX = "XenomorpheBrainbox";
        private const string XENOMORPHE_BRAINBOX_GAMEOBJECT = "ZAlienXenomorphBrainBox";
        #endregion

        #endregion

        // Citizens
        public const string PRESIDANT_US = "PresidantUS";
        private const string PRESIDANT_US_GAMEOBJECT = "Presidant_BillClinton";
        public const string DENIZEN = "Denizen";

        public const string AGENT = "AgentCIA";
        private const string AGENT_GAMEOBJECT = "Agent";
        #endregion
        #region Animals
        public const string SICK_PIG = "SickPig";
        private const string SICK_PIG_GAMEOBJECT = "Pig Rotten";
        #endregion
    }
}
