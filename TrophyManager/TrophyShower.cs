using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TrophyManager;

public class AchievementShower : MonoBehaviour
{
    public static Rect windowRect = new Rect(20, 20, 120, 50);

    // I want to made a notification that show on screen

    public static void trophyWindows(int windowID)
    {
        windowRect = GUILayout.Window(0, windowRect, trophyWindows, "My Window");
        if (GUILayout.Button("Hello World"))
        {
            Main.Log("Got a click");
        }
    }
}
