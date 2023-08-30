using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RocketLib
{
    public struct Countdown
    {
        private float _maxTime;
        private float _time;

        public bool IsOver
        {
            get
            {
                return _time <= 0;
            }
        }

        public float Time
        {
            get
            {
                return _time;
            }
        }

        public Countdown(float max)
        {
            _maxTime = max;
            _time = 0;
        }

        public void Reset(float time)
        {
            _maxTime = time;
            _time = time;
        }

        public void Reset()
        {
            _time = _maxTime;
        }

        public void Update(float t)
        {
            _time -= t;
        }
    }
}
