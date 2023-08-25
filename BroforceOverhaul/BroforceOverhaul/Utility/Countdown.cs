using System;
using System.Collections.Generic;
using System.Linq;
using RocketLib;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;
namespace BroforceOverhaul.Utility
{
    public class Countdown : MonoBehaviour
    {
        public float Value
        {
            get
            {
                return _value;
            }
        }

        public bool IsFinish
        {
            get
            {
                return _value < 0;
            }
        }

        public float timeMax = 0.5f;
        public bool active;
        public float timeRemoved = 0.1f;

        private float _value;

        public void ResetTimer()
        {
            _value = timeMax;
        }

        private void Update()
        {
            if (active)
            {
                if (_value > 0)
                {
                    _value -= timeRemoved;
                }
                else
                {
                    active = false;
                }
            }
        }

    }
}

