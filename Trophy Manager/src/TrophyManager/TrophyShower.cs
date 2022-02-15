using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TrophyManager;

public class TrophyShower : MonoBehaviour
{
    GUIStyle trophyStyle = new GUIStyle();

    internal float timeRemaining = 10;

    internal static Dictionary<Texture, string> redeem = new Dictionary<Texture, string>();

    private static List<Trophy> showedTrophy = new List<Trophy>();
    private static List<Trophy> waitingTrophy = new List<Trophy>();

    internal static bool Load()
    {
        try
        {
            new GameObject(typeof(TrophyShower).FullName, typeof(TrophyShower));

            return true;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        return false;
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
        trophyStyle.normal.textColor = new Color(1f, 0.701f, 0.101f); // "gold" color
    }
    void OnDestroy()
    {
        Main.Log("TrophyManager notification DESTROY");
    }
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            waitingTrophy.Remove(waitingTrophy.First());
            timeRemaining = 5;
        }
    }

    void OnGUI()
    {
        if (Main.settings.Notif)
        {

            /* foreach (KeyValuePair<Texture, string> obj in redeem)
             {*/
            Trophy trophy = waitingTrophy.First();
                GUILayout.Space(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height-400);
                GUILayout.BeginHorizontal("box");
                GUILayout.Label(trophy.TrophyTex);
                GUILayout.Label("\n\n" + trophy.Name, trophyStyle, GUILayout.ExpandWidth(true));
                GUILayout.EndHorizontal();
                //Main.Log(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height.ToString());
           // }
        }
    }

    internal static void AddRedeem(Trophy trophy)
    {
        waitingTrophy.Add(trophy);
    }

}
