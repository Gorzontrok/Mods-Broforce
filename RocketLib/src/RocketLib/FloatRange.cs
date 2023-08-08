using Newtonsoft.Json;
using System;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public struct FloatRange
{
    public static FloatRange ZeroOne
    {
        get { return new FloatRange(0f, 1f); }
    }

    public float AverageValue
    {
        get
        {
            return (_min + _max) * 0.5f;
        }
    }

    public float Min
    {
        get
        {
            return _min;
        }
        set
        {
            _min = value;
            _chosenValue = null;
        }
    }

    public float Max
    {
        get
        {
            return _max;
        }
        set
        {
            _max = value;
            _chosenValue = null;
        }
    }

    public float ChosenValue
    {
        get
        {
            if (_chosenValue == null)
            {
                _chosenValue = new float?(UnityEngine.Random.Range(_min, _max));
            }
            return _chosenValue.Value;
        }
    }

    public float ChooseRandom
    {
        get
        {
            return UnityEngine.Random.Range(_min, _max);
        }
    }

    public float Length
    {
        get
        {
            return _max - _min;
        }
    }

    [SerializeField, Delayed]
    private float _min;
    [SerializeField, Delayed]
    private float _max;
    [JsonIgnore, XmlIgnore]
    private float? _chosenValue;

    public FloatRange(float min, float max)
    {
        _min = min;
        _max = max;
        _chosenValue = null;
    }

    public bool InRange(float value)
    {
        return value >= _min && value <= _max;
    }

    public bool InRange(int value)
    {
        return (float)value >= _min && (float)value <= _max;
    }

    public float GetValue(float proportion)
    {
        return Mathf.Lerp(_min, _max, proportion);
    }

    public int Clamp(int value)
    {
        return (int)Mathf.Clamp((float)value, _min, _max);
    }

    public float Clamp(float value)
    {
        return Mathf.Clamp(value, _min, _max);
    }
}