using System.Collections.Generic;
using UnityEngine;

namespace FilteredBros
{
    public enum BroGroup
    {
        Campaign,
        Expendabros,
        Unused,
        Customs
    }

    public class BroToggle
    {
        public static List<BroToggle> All
        {
            get
            {
                if (_all == null)
                {
                    _all = new List<BroToggle>(Broforce);
                    _all.AddRange(Expendabros);
                    _all.AddRange(Secret);
                }
                return _all;
            }
        }
        private static List<BroToggle> _all = null;

        public static List<BroToggle> Broforce = new List<BroToggle>();
        public static List<BroToggle> Expendabros = new List<BroToggle>();
        public static List<BroToggle> Secret = new List<BroToggle>();
        public static List<BroToggle> Customs = new List<BroToggle>();

        public static int BrosEnabled { get; private set; } = 0;

        public readonly HeroType heroType;
        public readonly int unlockNumber;
        public readonly BroGroup group;

        public string Name {  get; private set; }
        public bool enabled;

        private bool _lastValue;

        static BroToggle()
        {
            Broforce = new List<BroToggle>();
            Expendabros = new List<BroToggle>();
            Secret = new List<BroToggle>();
        }

        public BroToggle(HeroType heroType, int unlockNumber, BroGroup group)
        {
            this.heroType = heroType;
            this.unlockNumber = unlockNumber;
            this.group = group;

            if(this.group == BroGroup.Campaign)
            {
                Broforce.Add(this);
            }
            else if (this.group == BroGroup.Expendabros)
            {
                Expendabros.Add(this);
            }
            else if (this.group == BroGroup.Unused)
            {
                Secret.Add(this);
            }

            Name = HeroController.GetHeroName(this.heroType);
        }

        public override string ToString()
        {
            return Name;
        }

        public void DrawToggle()
        {
            if(IsBroUnlocked())
            {
                enabled = GUILayout.Toggle(enabled, ToString(), GUILayout.Width(Main.settings.ui.toggleWidth));
            }
            else
            {
                enabled = GUILayout.Toggle(true, "<color=\"gray\">???</color>", GUILayout.Width(Main.settings.ui.toggleWidth));
            }

            if(_lastValue != enabled)
            {
                Mod.ShouldUpdateUnlockIntervals = true;
                if (_lastValue)
                {
                    BrosEnabled--;
                }
                else
                {
                    BrosEnabled++;
                }
            }
            _lastValue = enabled;
        }

        public bool IsBroUnlocked()
        {
            return group != BroGroup.Campaign || unlockNumber <= PlayerProgress.Instance.freedBros || Main.cheat;
        }
    }
}
