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

    public static bool Load()
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
            redeem.Remove(redeem.Keys.First());
            timeRemaining = 10;
        }
    }

    void OnGUI()
    {
        if (Main.settings.Notif)
        {
            trophyStyle.normal.textColor = new Color(1f, 0.701f, 0.101f); // "gold" color

            foreach (KeyValuePair<Texture, string> obj in redeem)
            {
                GUILayout.Space(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height-400);
                GUILayout.BeginHorizontal("box");
                GUILayout.Label(obj.Key);
                GUILayout.Label("\n\n" + obj.Value, trophyStyle, GUILayout.ExpandWidth(true));
                GUILayout.EndHorizontal();
                //Main.Log(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height.ToString());
            }
        }
    }

    public static void AddRedeem(Texture image, string Name)
    {
        redeem.Add(image, Name);
    }

}
