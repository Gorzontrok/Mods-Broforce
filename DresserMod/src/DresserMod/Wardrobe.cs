using System;
using System.Collections.Generic;

namespace DresserMod
{
    public class Wardrobe : IEquatable<Wardrobe>
    {
        public string wearers = string.Empty;
        public List<Attire> attires = new List<Attire>();

        public Attire this[int index]
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

        public void AddAttire(Attire attire)
        {
            attires.Add(attire);
        }

        public bool Equals(Wardrobe other)
        {
            if (other == null) return false;
            return other.wearers == wearers;
        }

        public Attire RandomAttire()
        {
            if(Main.settings.canUseDefaultSkin)
            {
                int val = UnityEngine.Random.Range(0, attires.Count + 1);
                if (val >= attires.Count)
                    return null;
                return attires[val];
            }
            return attires.RandomElement();
        }

        public void SetRandomAttire(object obj)
        {
            if(attires.Count == 0) return;
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
