using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BroforceOverhaul.Scenes.IronBroGameOver
{
    public class RestartIronBroGUI : MonoBehaviour
    {
        private void OnGui()
        {
            if(SceneManager.GetActiveScene().name == "CutsceneGameOver")
            {
                GUILayout.BeginArea(new Rect((int)Screen.width / 2, (int)Screen.height / 2, 200, 200));
                //GUILayout.Button();
                GUILayout.EndArea();
            }
        }
    }
}

