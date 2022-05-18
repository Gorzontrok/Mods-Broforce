using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;
using HarmonyLib;
using RocketLib0;

namespace FilteredBros
{
    class BroToggle
    {
        public static List<BroToggle> BroToggles
        {
            get
            {
                var l = new List<BroToggle>(broTogglesBroforce);
                l.AddRange(broTogglesExpendabros);
                l.AddRange(broTogglesHide);
                return l;
            }
        }

        public static List<BroToggle> broTogglesBroforce = new List<BroToggle>();
        public static List<BroToggle> broTogglesExpendabros = new List<BroToggle>();
        public static List<BroToggle> broTogglesHide = new List<BroToggle>();
        public static int brosEnable;

        public readonly HeroType heroType;
        public readonly int unlockNumber;
        public readonly BroGroup group;

        public bool enabled;

        private bool lastValue;

        public BroToggle(HeroType _heroType, int _unlockNumber, BroGroup g)
        {
            heroType = _heroType;
            unlockNumber = _unlockNumber;
            group = g;

            if(group == BroGroup.Broforce)
            {
                broTogglesBroforce.Add(this);
            }
            else if (group == BroGroup.Expendabros)
            {
                broTogglesExpendabros.Add(this);
            }
            else if (group == BroGroup.Hide)
            {
                broTogglesHide.Add(this);
            }
        }

        public override string ToString()
        {
            return heroType.ToString() + " " + unlockNumber.ToString() + " " + enabled.ToString();
        }

        public void Toggle()
        {
            if(group == BroGroup.Hide && !Main.cheat)
            {
                enabled = GUILayout.Toggle(false, "<color=\"gray\">???</color>", GUILayout.ExpandWidth(false));
            }
            else if (!IsBroUnlocked() && !Main.cheat)
            {
                enabled = GUILayout.Toggle(true, "<color=\"gray\">???</color>", GUILayout.ExpandWidth(false));
            }
            else
            {
                enabled = GUILayout.Toggle(enabled, HeroController.GetHeroName(heroType), GUILayout.ExpandWidth(false));
            }

            if(lastValue != enabled)
            {
                if (lastValue)
                {
                    brosEnable--;
                }
                else
                {
                    brosEnable++;
                }
            }
            lastValue = enabled;
        }

        public bool IsBroUnlocked()
        {
            return unlockNumber <= PlayerProgress.Instance.freedBros;
        }

        public static BroToggle GetBroToggleFromHeroType(HeroType hero)
        {
            foreach(BroToggle b  in BroToggles)
            {
                if (b.heroType == hero)
                {
                    return b;
                }
            }
            return null;
        }

        public enum BroGroup
        {
            Hide,
            Broforce,
            Expendabros
        }
    }
}
