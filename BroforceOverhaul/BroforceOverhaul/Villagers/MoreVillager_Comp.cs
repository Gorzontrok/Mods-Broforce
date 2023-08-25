using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace BroforceOverhaul.Villagers
{
    public class MoreVillager_Comp : MonoBehaviour
    {
        public VillagerType villagerType = VillagerType.Normal;
        void Awake()
        {
            float value = UnityEngine.Random.value;
            if(value < 0.5f)
            {
                villagerType = VillagerType.Thrower;
            }
        }
    }
    public enum VillagerType
    {
        Normal,
        Thrower
    }
}
