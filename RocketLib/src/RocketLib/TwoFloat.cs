using System;
using UnityEngine;

[Serializable]
public class TwoFloat
{
    public float x;
    public float y;

    public TwoFloat(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public Vector2Int ToVector2Int()
    {
        return new Vector2Int((int)x, (int)y);
    }

    public Vector2 ToVector2()
    {
        return new Vector2(x, y);
    }
}
