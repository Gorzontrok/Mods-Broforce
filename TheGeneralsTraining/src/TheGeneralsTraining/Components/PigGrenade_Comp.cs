using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TheGeneralsTraining.Components
{
    internal class PigGrenade_Comp : MonoBehaviour
    {
        public void Explode(Grenade grenade)
        {
            MapController.SpawnTestVanDamme_Networked(Map.Instance.activeTheme.animals[0].GetComponent<TestVanDammeAnim>(), grenade.X, grenade.Y, 0f, 0f, false, false, false, false);
            MakeEffects(grenade);
        }

        public void MakeEffects(Grenade grenade)
        {
            EffectsController.CreateShrapnel(grenade.shrapnel, grenade.X, grenade.Y, 3f, 140f, 20f, 0f, 150f);
            Randomf random = Traverse.Create(grenade).Field("random").GetValue<Randomf>();
            Vector3 a = random.insideUnitCircle;
            EffectsController.CreateEffect(grenade.smoke1, grenade.X + a.x * grenade.range * 0.25f, grenade.Y + a.y * grenade.range * 0.25f, 0f, random.value * 0.5f, a * grenade.range * 3f, null);
            a = random.insideUnitCircle;
            EffectsController.CreateEffect(grenade.smoke1, grenade.X + a.x * grenade.range * 0.25f, grenade.Y + a.y * grenade.range * 0.25f, 0f, random.value * 0.5f, a * grenade.range * 3f, null);
            EffectsController.CreateEffect(grenade.smoke1, grenade.X + a.x * grenade.range * 0.25f, grenade.Y + a.y * grenade.range * 0.25f, 0f, random.value * 0.2f, random.insideUnitSphere * grenade.range * 3f, null);
            a = random.insideUnitCircle;
            EffectsController.CreateEffect(grenade.smoke2, grenade.X + a.x * grenade.range * 0.25f, grenade.Y + a.y * grenade.range * 0.25f, 0f, random.value * 0.5f, a * grenade.range * 3f, null);
            a = random.insideUnitCircle;
            EffectsController.CreateEffect(grenade.smoke2, grenade.X + a.x * grenade.range * 0.25f, grenade.Y + a.y * grenade.range * 0.25f, 0f, random.value * 0.5f, a * grenade.range * 3f, null);
            a = random.insideUnitCircle;
            EffectsController.CreateEffect(grenade.smoke2, grenade.X + a.x * grenade.range * 0.25f, grenade.Y + a.y * grenade.range * 0.25f, 0f, random.value * 0.2f, a * grenade.range * 3f, null);
            //Map.DisturbWildLife(grenade.X, grenade.Y, 40f, 5);
        }
    }
}
