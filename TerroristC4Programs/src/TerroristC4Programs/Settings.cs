using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityModManagerNet;

namespace TerroristC4Programs
{
    public class Settings : UnityModManager.ModSettings
    {
        public bool patchInCustomsLevel = false;
        // Global
        public bool zombiesDanceOnFlex = true;
        public bool betterSkinlessSprite = true;

        // Probability
        public Probability01 strongerTrooperProbability = new Probability01();
        public Probability01 strongerBruiserProbability = new Probability01();
        public Probability01 eliteBruiserProbability = new Probability01();
        public Probability01 suicideGetBigger = new Probability01();

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
