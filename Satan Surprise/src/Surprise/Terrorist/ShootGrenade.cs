using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Surprise.Terrorist
{
    class ShootGrenade : MonoBehaviour
    {
        public int shootCounter;
        Grenade grenade;

        void Awake()
        {
            grenade = (Map.Instance.activeTheme.mookGrenadier as MookGrenadier).specialGrenade;
        }

        public void ThrowGrenade(MonoBehaviour firedBy, float x, float y, float xI, float yI, int playerNum)
        {
            ProjectileController.SpawnGrenadeOverNetwork(grenade, firedBy, x, y , 0.001f, 0.011f, xI, yI, playerNum, 1f);
        }
    }
}
