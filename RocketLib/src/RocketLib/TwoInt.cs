using System;
using UnityEngine;


[Serializable]
public class TwoInt
{
    public int x;
    public int y;

    public TwoInt(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Vector2Int ToVector2Int()
    {
        return new Vector2Int(x, y);
    }

    public Vector2 ToVector2()
    {
        return new Vector2(x, y);
    }
}
