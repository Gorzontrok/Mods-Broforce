using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityModManagerNet;
using HarmonyLib;
using RocketLibLoadMod;

namespace RocketLib0
{
        /// <summary>
        /// On screen logger.
        /// </summary>
        public class ScreenLogger : MonoBehaviour
        {
            /// <summary>
            /// Instance of ScreenLogger
            /// </summary>
            public static ScreenLogger Instance
            {
                get
                {
                    return instance;
                }
            }
            /// <summary>
            ///
            /// </summary>
            public static bool IsLoaded { get; internal set; }

            internal List<string> FullLogList = new List<string>();

            private float TimeRemaining = Main.settings.logTimer;
            private List<string> LogsOnScreen = new List<string>();
            private List<string> LogsForTXT = new List<string>();
            private int UMM_NumberOfLogs;
            private string LogFilePath = Main.mod.Path + "Logs\\";
            private static ScreenLogger instance;

            internal static bool Load()
            {
                try
                {
                    var d = new GameObject(typeof(ScreenLogger).FullName, typeof(ScreenLogger));
                    IsLoaded = true;
                    return true;
                }
                catch (Exception ex)
                {
                    Main.Log(ex);
                }
                return false;
            }

            internal void AddSuccessLog(string str)
            {
                string nstr = "<color=green>[" + DateTime.Now.ToString("HH:mm:ss") + "] " + str + "</color>\n";
                LogsOnScreen.Add(nstr);
                LogsForTXT.Add(nstr);
                FullLogList.Add(nstr);
            }

            /// <summary>
            /// Clear the log on screen.
            /// </summary>
            public void Clear()
            {
                LogsOnScreen = new List<string>();
            }
            private void ClearTXTFiles()
            {
                string FileNameToday = DateTime.UtcNow.Date.ToString("yyyy'-'MM'-'dd") + ".txt";
                foreach (string file in Directory.GetFiles(LogFilePath))
                {
                    if (file != FileNameToday)
                    {
                        File.Delete(Path.Combine(LogFilePath, file));
                    }
                }
            }

            /// <summary>
            /// Add log to the screen.
            /// </summary>
            /// <param name="str">Log Message</param>
            /// <param name="type">RLogType</param>
            public void Log(object str, RLogType type = RLogType.Log)
            {
                string prefix = $"[{DateTime.Now.ToString("HH:mm:ss")}]" + (type == RLogType.Log ? "" : "[" + type.ToString() + "]");
                Log(str, prefix);
            }

            /// <summary>
            /// Add log to the screen. (BroforceMod edition)
            /// </summary>
            /// <param name="str">Log Message</param>
            /// <param name="prefix">Prefix of the log</param>
            public void Log(object str, string prefix)
            {
                AddLog($"{prefix} : " + str.ToString());
            }

            void AddLog(string log)
            {
                LogsOnScreen.Add("\n" + log);
                LogsForTXT.Add(log);
                FullLogList.Add(log);
            }


            private void OnDestroy()
            {
                IsLoaded = false;
                Main.mod.Logger.Error("ScreenLogger DESTROY");
            }

            private void Awake()
            {
                DontDestroyOnLoad(this);
                //ClearTXTFiles();
                IsLoaded = true;
                instance = this;
            }

            private void Start()
            {
                /*ModId = "RocketLibTest";
                Log("TEST Log", RLogType.Log);
                Log("TEST Warning", RLogType.Warning);
                Log("TEST ERROR", RLogType.Error);
                Log("TEST Exception", RLogType.Exception);
                Log("TEST Information", RLogType.Information);*/
                ClearTXTFiles();
                string thing = new string('=', 24);
                LogsForTXT.Add(thing + DateTime.Now.ToString("HH:mm:ss") + thing);
            }

            private void Update()
            {
                if (Input.GetKeyDown(KeyCode.F7)) Clear();

                if (LogsOnScreen.Count > 0)
                {
                    if (TimeRemaining > 0)
                    {
                        TimeRemaining -= Time.deltaTime;
                    }
                    else
                    {
                        LogsOnScreen.Remove(LogsOnScreen.First());
                        TimeRemaining = Main.settings.logTimer;
                    }
                }
                if (LogsOnScreen.Count > 30) LogsOnScreen.Remove(LogsOnScreen.First());
                CheckUMMLog();
                WriteLogTXT();
            }


            private void OnGUI()
            {
                if (Main.settings.OnScreenLog && LogsOnScreen.Count > 0)
                {
                        GUILayout.BeginVertical("box");
                        var LogStyle = new GUIStyle();
                        foreach (string log in LogsOnScreen)
                        {
                            LogStyle.normal.textColor = WhichColor(log);
                            GUILayout.Label(log, LogStyle);
                        }
                        GUILayout.EndVertical();
                }
            }

            internal static Color WhichColor(string LogMsg)
            {
                LogMsg = LogMsg.ToLower();
                if (LogMsg.Contains("error") || LogMsg.Contains("exception"))
                {
                    return Color.red;
                }
                else if (LogMsg.Contains("warning"))
                {
                    return Color.yellow;
                }
                else if (LogMsg.Contains("[information]"))
                {
                    return Color.blue;
                }
                else if (LogMsg.Contains("successful loaded"))
                {
                    return Color.green;
                }
                else
                {
                    return Color.white;
                }
            }

            private void CheckUMMLog()
            {
                List<string> ummLog = Traverse.Create(typeof(UnityModManager.Logger)).Field("history").GetValue<List<string>>();
                if (ummLog.Count > UMM_NumberOfLogs)
                {
                    if (Main.settings.ShowManagerLog)
                    {
                        for (var str = UMM_NumberOfLogs; str < ummLog.Count; str++)
                        {
                            string dateTimeNow = DateTime.Now.ToString("HH:mm:ss");
                            LogsOnScreen.Add("\n[" + dateTimeNow + "] " + ummLog[str]);
                        }
                    }
                    UMM_NumberOfLogs = ummLog.Count;
                }
            }
            private void WriteLogTXT()
            {
                try
                {
                    if (!Directory.Exists(LogFilePath))
                    {
                        Directory.CreateDirectory(LogFilePath); // Ignore warning.
                    }

                    DateTime date = DateTime.UtcNow.Date;
                    string FilePathToday = Path.Combine(LogFilePath, date.ToString("yyyy'-'MM'-'dd") + ".txt");

                    if (!File.Exists(FilePathToday))
                    {
                        using (File.Create(FilePathToday)) ; // Ignore warning.
                    }
                    if (LogsForTXT.Count > 0)
                    {
                        using (StreamWriter writer = File.AppendText(FilePathToday))
                        {
                            foreach (var str in LogsForTXT)
                            {
                                writer.WriteLine(str);
                            }
                        }
                        LogsForTXT = new List<string>();
                    }
                }
                catch (Exception ex)
                {
                    Main.bmod.logger.ExceptionLog(ex);
                }
            }

        }
}

/// <summary>
/// Type of log for the log. They each have a "custom" color.
/// </summary>
public enum RLogType
{
    /// <summary>
    /// White
    /// </summary>
    Log,
    /// <summary>
    /// Yellow
    /// </summary>
    Warning,
    /// <summary>
    /// Red
    /// </summary>
    Error,
    /// <summary>
    /// Red
    /// </summary>
    Exception,
    /// <summary>
    /// Blue
    /// </summary>
    Information
}

