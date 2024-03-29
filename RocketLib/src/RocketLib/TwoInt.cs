﻿using System;
using UnityEngine;


[Serializable]
public struct TwoInt
{
    public static readonly TwoInt zero = new TwoInt(0, 0);

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
