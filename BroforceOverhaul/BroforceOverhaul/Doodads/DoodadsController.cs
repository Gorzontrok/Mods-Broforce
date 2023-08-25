using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace BroforceOverhaul.Doodads
{
    public static class DoodadsController
    {

        public static int GetVillagerCapturedVariation()
        {
            return UnityEngine.Random.Range(7, Map.MapData.theme == LevelTheme.Jungle || Map.MapData.theme == LevelTheme.Forest || Map.MapData.theme == LevelTheme.BurningJungle ? 9 : 10);
        }

        public static void CallBroHQ(float x, float num)
        {
            float value = (float)UnityEngine.Random.value;
            /*if(value < 0.1f)
            {
                Mook mech = MapController.SpawnMook_Networked(Map.Instance.sharedObjectsReference.Asset.mechDrop, x, num + UnityEngine.Random.value * 8f, 0f, 0f, false, true, true, false, false);
                MookArmouredGuy component2 = mech.GetComponent<MookArmouredGuy>();
                component2.SetAmericaMaterials();
                component2.SetCanParachute(true);
                component2.OpenParachute();
            }
            else if( value < 0.6f)
            {
                int[] ammocrateAllow = new int[] { 0, 3, 6 };
                for (int j = 0; j < 3; j++)
                {
                    int variation = ammocrateAllow[UnityEngine.Random.Range(0, ammocrateAllow.Length)];
                    GameObject gameobject = Map.Instance.PlaceDoodad(new DoodadInfo(new GridPoint(0, 0), DoodadType.AmmoCrate, variation));
                    gameobject.transform.position = new Vector3(x + ((float)j - 1.5f) * 32f, num + UnityEngine.Random.value * 8f, 0f);
                    gameobject.GetComponent<CrateBlock>().SetParachuteActive(true);
                    gameobject.GetComponent<CrateBlock>().parachhuteFallSpeed = 10;
                }
            }*/
            /*else
            {*/
                for (int j = 0; j < 4; j++)
                {
                    TestVanDammeAnim[] villager = Map.Instance.activeTheme.villager1;
                    int max = villager.Length;
                    Villager villager2 = villager[UnityEngine.Random.Range(0, max)] as Villager;
                    if (villager2 != null)
                    {
                        MapController.SpawnVillager_Networked(villager2, x + ((float)j - 1.5f) * 32f, num + UnityEngine.Random.value * 8f, (float)UnityEngine.Random.Range(-1, 2), 0f, false, false, true, false, false, -1);
                    }
                }
            //}
        }
    }
}
