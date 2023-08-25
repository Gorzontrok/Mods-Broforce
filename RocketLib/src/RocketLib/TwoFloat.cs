using System;
using UnityEngine;

[Serializable]
public struct TwoFloat
{
    public static readonly TwoFloat zero = new TwoFloat(0, 0);

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
