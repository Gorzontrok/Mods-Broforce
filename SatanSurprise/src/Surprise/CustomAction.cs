using System;
using UnityEngine;

namespace Surprise
{
    class CustomAction : MonoBehaviour
    {
        public int maxActionCount = 2;
        public float actionInterval = 2.5f;

        protected TestVanDammeAnim actionBy;
        protected int actionCount;
        protected bool isDoingAction;
        protected float lastTimeActionWasDone;


        protected virtual void Awake()
        {

        }

        public virtual void Call(TestVanDammeAnim callBy, params object[] objects)
        {
            actionBy = callBy;
            actionCount = 0;
            isDoingAction = true;
        }

        public virtual void Stop()
        {
            actionCount = 0;
            isDoingAction = false;
        }

        protected virtual void DoAction()
        {
            actionCount++;
            lastTimeActionWasDone = Time.time;
        }

        protected virtual void Update()
        {
            if (isDoingAction && (lastTimeActionWasDone -= Time.time) < actionInterval && actionCount < maxActionCount)
            {
                DoAction();
            }

            if (actionCount == maxActionCount)
            {
                Stop();
            }
        }
    }
}
