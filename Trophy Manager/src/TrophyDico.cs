using System;
using System.Collections.Generic;
using TrophyManager;


public class TrophyDico
{
    // old
    private static string trophyFolderPath = Main.mod.Path + "Trophy/";

    private static object[] DoYouLikeMyMuscle       =   { "Make 50 enemies blind.",     trophyFolderPath + "DoYouLikeMyMuscle.png",         false,  50      };
    private static object[] DoorKill                =   { "Kill Someone with a door",   trophyFolderPath + "DoorKill.png",                  false,  1       };

    private static Dictionary<String, object[]> trophyIntObjective = new Dictionary<String, object[]>()
    {
        {"Do you like my muscle ?",         DoYouLikeMyMuscle       },
        { "D-D-D-DOOR KILL !",              DoorKill                }
    };
    //========================

    private static List<Trophy> __AllTrophyList = new List<Trophy>();
    public static List<Trophy> AllTrophyList
    {
        get
        {
            return __AllTrophyList;
        }
    }

    public static void UpdateTrophyValue(Trophy trophy)
    {
        __AllTrophyList[__AllTrophyList.FindIndex(ind => ind.Name.Equals(trophy.Name))] = trophy;
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
        List<Trophy> temp = __AllTrophyList;
        foreach (Trophy trophy in temp)
        {
            trophy.Reset();
            UpdateTrophyValue(trophy);
        }
    }

    internal static void CheckIsDone()
    {
        List<Trophy> temp = __AllTrophyList;
        foreach (Trophy trophy in temp)
        {
            trophy.CheckIsDone();
            UpdateTrophyValue(trophy);
        }
    }
}
