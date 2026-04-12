using System;
using System.Collections.Generic;
using System.Linq;

namespace DresserMod
{
    public class Wardrobe : IEquatable<Wardrobe>
    {
        public string wearers = string.Empty;
        public List<IAttire> attires = new List<IAttire>();

        public IAttire this[int index]
        {
            get
            {
                if (index < 0 || index > attires.Count)
                    throw new ArgumentOutOfRangeException();
                return attires[index];
            }
        }

        public Wardrobe(string wearers)
        {
            this.wearers = wearers;
        }

        public void AddAttire(IAttire attire)
        {
            attires.Add(attire);
        }

        public bool Equals(Wardrobe other)
        {
            if (other == null) return false;
            return other.wearers == wearers;
        }

        public IAttire RandomAttire()
        {
            IAttire[] activeAttires = attires.Where(a => a.Enabled).ToArray();
            if (activeAttires.Length <= 0)
                return null;

            if(Main.settings.canUseDefaultSkin)
            {
                int val = UnityEngine.Random.Range(0, activeAttires.Length + 1);
                if (val >= activeAttires.Length)
                    return null;
                return activeAttires[val];
            }
            return activeAttires.RandomElement();
        }

        public void SetRandomAttire(object obj)
        {
            if(attires.Count == 0)
                return;
            try
            {
                var attire = RandomAttire();
                if(attire != null)
                    attire.SuitUp(obj);
            }
            catch (Exception e)
            {
                Main.Log(e);
            }
        }
    }
}
