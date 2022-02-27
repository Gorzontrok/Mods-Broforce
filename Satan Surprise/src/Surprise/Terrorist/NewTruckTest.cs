using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Surprise.Terrorist
{
    class NewTruckTest : MonoBehaviour
    {
        public int maximumSpawnMook;

        int spawnMookCount = 0;
        float fireDelay = 0.6f;

        public Mook SpawnMook(float x, float y, float xI, float yI, bool isAltert)
        {
            if((fireDelay -= 0.1f) < 0 && spawnMookCount < maximumSpawnMook)
            {
                Mook mookToSpawn = GetMookToSpawn();
                spawnMookCount++;
                AssignFireDelay();
                if (mookToSpawn != null)
                {
                    Mook mook = MapController.SpawnMook_Networked(mookToSpawn, x, y, xI, yI, true, false, false, false, isAltert);
                    return mook;
                }
            }
            return null;
        }

        Mook GetMookToSpawn()
        {
            try
            {
                TestVanDammeAnim mook = activeTheme.mook;
                if (spawnMookCount < 2)
                {
                    mook = activeTheme.mookDog;
                }
                else if (spawnMookCount < 4)
                {
                    mook = activeTheme.mookRiotShield;
                }
                else if (Main.PartyIsHardMode && spawnMookCount < 5)
                {
                    mook = activeTheme.mookBigGuy;
                }

                return (Mook)mook;
            }
            catch(Exception ex)
            {
                Main.Log(ex);
                return null;
            }
        }

        void AssignFireDelay()
        {
            if (spawnMookCount % 2 == 0 || (Main.PartyIsHardMode && spawnMookCount == 5))
            {
                fireDelay = 4f;
            }
            else
            {
                fireDelay = 0.6f;
            }
        }

        ThemeHolder activeTheme
        {
            get
            {
                return Map.Instance.activeTheme;
            }
        }
    }
}
