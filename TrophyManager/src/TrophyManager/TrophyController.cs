using System;
using UnityEngine;
using System.Collections.Generic;
using TrophyManager;


public class TrophyController
{
    private static List<Trophy> __AllTrophyList = new List<Trophy>();
    public static List<Trophy> AllTrophyList
    {
        get
        {
            return __AllTrophyList;
        }
    }

    public static void AddTrophy(Trophy trophy)
    {
        bool AlreadyIn = false;
        foreach(Trophy t in __AllTrophyList)
        {
            AlreadyIn = trophy.Name == t.Name;
        }
        if(!AlreadyIn)
        {
            __AllTrophyList.Add(trophy);
        }
        else
        {
            Main.Log(trophy.Name + " already existed in the trophy list.");
        }
    }

    internal static void Reset()
    {
        foreach (Trophy trophy in __AllTrophyList)
        {
            trophy.Reset();
        }
    }

    internal static void CheckIsDone()
    {
        foreach (Trophy trophy in __AllTrophyList)
        {
            trophy.CheckIsDone();
        }
    }

    public static Texture2D CreateTextureFromPath(string path)
    {
        if(System.IO.File.Exists(path))
        {
            Texture2D tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.LoadImage(System.IO.File.ReadAllBytes(path));
            return tex;
        }
        return null;
    }
}
