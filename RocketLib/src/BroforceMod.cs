using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityModManagerNet;
using HarmonyLib;
using RocketLib0;
using RocketLibLoadMod;

namespace RocketLib0
{
    /// <summary>
    ///
    /// </summary>
    public class BroforceMod
    {
        /// <summary>
        /// The Unity mod of the class.
        /// </summary>
        protected readonly UnityModManager.ModEntry __mod;
        /// <summary>
        /// The ID of the mod.
        /// </summary>
        public readonly string ID = string.Empty;
        /// <summary>
        /// Is the mod is successfuly load.
        /// </summary>
        public readonly bool IsLoaded = false;


        // PREFIX
        /// <summary>
        /// Normal prefix.
        /// </summary>
        protected readonly string Prefix = string.Empty;
        /// <summary>
        /// Exception prefix.
        /// </summary>
        protected readonly string PrefixException = string.Empty;
        /// <summary>
        /// Information prefix.
        /// </summary>
        protected readonly string PrefixInformation = string.Empty;
        /// <summary>
        /// Warning prefix.
        /// </summary>
        protected readonly string PrefixWarning = string.Empty;
        /// <summary>
        /// Error Prefix.
        /// </summary>
        protected readonly string PrefixError = string.Empty;
        /// <summary>
        /// Debug prefix.
        /// </summary>
        protected readonly string PrefixDebug = "[DEBUG] ";
        /// <summary>
        /// the current time.
        /// </summary>
        protected string TimeNow = string.Empty;


        //
        /// <summary>
        /// Write log locally.
        /// </summary>
        public bool UseLocalLog = false;

        /// <summary>
        /// Show debug log.
        /// </summary>
        public bool UseDebugLog = false;

        /// <summary>
        /// </summary>
        protected bool HasStartLog = false;

        /// <summary>
        /// Path of the file with the mod log.
        /// </summary>
        protected string LogFilePath = string.Empty;

        /// <summary>
        /// Create the mod.
        /// </summary>
        /// <param name="mod">Mod given by UnityModManager</param>
        /// <param name="_UseLocalLog">Write local log stored in the mod's folder.</param>
        /// <param name="_UseDebugLog">Show debug log.</param>
        public BroforceMod(UnityModManager.ModEntry mod, bool _UseLocalLog = false, bool _UseDebugLog = false)
        {
            if(mod == null)
            {
                new Exception("The given mod is null.");
            }

            RocketLibLoadMod.Main.Log("Start loading Broforce mod : " + mod.Info.Id);
            try
            {
                this.__mod = mod;
                this.ID = mod.Info.Id;

                this.Prefix = $"[{this.ID}]";
                this.PrefixException = $"[{this.ID}] [Exception]";
                this.PrefixInformation = $"[{this.ID}] [Information]";
                this.PrefixWarning = $"[{this.ID}] [Warning]";
                this.PrefixError = $"[{this.ID}] [Error]";

                this.UseLocalLog = _UseLocalLog;
                this.UseDebugLog = _UseDebugLog;

                this.LogFilePath = Path.Combine(this.__mod.Path, $"{this.ID}_Log.txt");

                this.TimeNow = "[" + DateTime.Now.ToString("HH:mm:ss") + "] ";
                this.IsLoaded = true;
                RocketLibLoadMod.Main.BMOD_List.Add(this);
                this.Log($"Successful loaded the mod : '{this.ID}' ");
            }
            catch(Exception ex)
            {
                this.IsLoaded = false;
                RocketLibLoadMod.Main.Log($"Failed Loading : {mod.Info.Id}\n" + ex);
            }

        }

        /// <summary>
        /// The Update function. Call it in Main.OnUpdate.
        /// </summary>
        public virtual void OnUpdate()
        {
            if (!this.IsLoaded) return;

            this.TimeNow = "[" + DateTime.Now.ToString("HH:mm:ss") + "] ";
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void StartLog()
        {
            if (!this.IsLoaded) return;

            if(!this.HasStartLog)
            {
                DateTime dateTimeNow = DateTime.Now;
                string thing = new string('=', 24);
                WriteLogLocally(thing + dateTimeNow + thing, "");
                this.HasStartLog = true;
            }
        }

        // ======= Normal Log =======
        /// <summary>
        /// Write Log.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="LogType"></param>
        /// <param name="Debug"></param>
        public virtual void Log(IEnumerable<object> Message, RLogType LogType = RLogType.Log, bool Debug = false)
        {
            if (!this.IsLoaded) return;

            this.Log(Message.ToString(), LogType, Debug);
        }
        /// <summary>
        /// Write Exception Log.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="Debug"></param>
        public virtual void Log(Exception exception, bool Debug = false)
        {
            if (!this.IsLoaded) return;

            this.ExceptionLog(exception, Debug);
        }
        /// <summary>
        /// Write Exception Log.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="exception"></param>
        /// <param name="Debug"></param>
        public virtual void Log(string Message, Exception exception, bool Debug = false)
        {
            this.ExceptionLog(Message + "\n" + exception, Debug);
        }
        /// <summary>
        /// Write log.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="LogType"></param>
        /// <param name="Debug"></param>
        public virtual void Log(object Message, RLogType LogType = RLogType.Log, bool Debug = false)
        {
            if (!this.IsLoaded) return;

            this.StartLog();

            if((!Debug && !this.UseDebugLog) || (Debug && this.UseDebugLog))
            {
                string LocalPrefix = string.Empty;

                switch (LogType)
                {
                    case RLogType.Error:
                        LocalPrefix = this.TimeNow + (Debug && this.UseDebugLog ? this.PrefixDebug : "") + this.PrefixError;
                        break;
                    case RLogType.Exception:
                        LocalPrefix = this.TimeNow + (Debug && this.UseDebugLog ? this.PrefixDebug : "") + this.PrefixException;
                        break;
                    case RLogType.Information:
                        LocalPrefix = this.TimeNow + (Debug && this.UseDebugLog ? this.PrefixDebug : "") + this.PrefixInformation;
                        break;
                    case RLogType.Warning:
                        LocalPrefix = this.TimeNow + (Debug && this.UseDebugLog ? this.PrefixDebug : "") + this.PrefixWarning;
                        break;
                    default:
                        LocalPrefix = this.TimeNow + (Debug && this.UseDebugLog ? this.PrefixDebug : "") + this.Prefix;
                        break;
                }

                if(string.IsNullOrEmpty(LocalPrefix))
                {
                    LocalPrefix = this.TimeNow + (Debug && this.UseDebugLog ? this.PrefixDebug + this.Prefix : this.Prefix);
                }

                if (RocketLib.ScreenLogger.isSuccessfullyLoad)
                {
                    RocketLib.ScreenLogger.Log(Message, LocalPrefix);
                }
                else
                {
                    UnityModManager.Logger.Log(Message.ToString(), LocalPrefix);
                }
                WriteLogLocally(Message.ToString(), LocalPrefix);
            }
        }
        //======================

        // ======= Exception Log =======
        /// <summary>
        /// Write Exception Log.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="Debug"></param>
        public virtual void ExceptionLog(Exception exception, bool Debug = false)
        {
            if (!this.IsLoaded) return;

            this.ExceptionLog(exception.ToString(), Debug);
        }
        /// <summary>
        /// Write Exception Log.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="exception"></param>
        /// <param name="Debug"></param>
        public virtual void ExceptionLog(string Message, Exception exception, bool Debug = false)
        {
            this.ExceptionLog(Message + "\n" + exception, Debug);
        }
        /// <summary>
        /// Write Exception Log.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Debug"></param>
        public virtual void ExceptionLog(IEnumerable<object> Message, bool Debug = false)
        {
            if (!this.IsLoaded) return;

            this.ExceptionLog(Message.ToString(), Debug);
        }
        /// <summary>
        /// Write Exception Log.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Debug"></param>
        public virtual void ExceptionLog(object Message, bool Debug = false)
        {
            if (!this.IsLoaded) return;

            this.Log(Message, RLogType.Exception, Debug);
        }
        //======================

        //======= Warning Log ========
        /// <summary>
        /// Write Warning Log.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Debug"></param>
        public virtual void WarningLog(object Message, bool Debug = false)
        {
            if (!this.IsLoaded) return;

            this.Log(Message, RLogType.Warning, Debug);
        }
        /// <summary>
        /// Write Warning Log.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Debug"></param>
        public virtual void WarningLog(IEnumerable<object> Message, bool Debug = false)
        {
            this.WarningLog(Message.ToString(), Debug);
        }
        // ==============================

        // ======== Information Log ==========
        /// <summary>
        /// Write information Log.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Debug"></param>
        public virtual void InformationLog(object Message, bool Debug = false)
        {
            if (!this.IsLoaded) return;

            this.Log(Message, RLogType.Information, Debug);
        }
        /// <summary>
        /// Write information Log.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Debug"></param>
        public virtual void InformationLog(IEnumerable<object> Message, bool Debug = false)
        {
            this.InformationLog(Message.ToString(), Debug);
        }
        // ==============================

        // ====== Error Log =========
        /// <summary>
        /// Write Error Log.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Debug"></param>
        public virtual void ErrorLog(object Message, bool Debug = false)
        {
            this.Log(Message, RLogType.Error, Debug);
        }
        /// <summary>
        /// Write Error Log.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Debug"></param>
        public virtual void ErrorLog(IEnumerable<object> Message, bool Debug = false)
        {
            this.ErrorLog(Message.ToString(), Debug);
        }
        /// <summary>
        /// Write Error Log.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="Debug"></param>
        public virtual void ErrorLog(Exception exception, bool Debug = false)
        {
            if (!this.IsLoaded) return;

            this.ExceptionLog(exception.ToString(), Debug);
        }
        /// <summary>
        /// Write Error Log.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="exception"></param>
        /// <param name="Debug"></param>
        public virtual void ErrorLog(string Message, Exception exception, bool Debug = false)
        {
            this.ExceptionLog(Message + "\n" + exception, Debug);
        }
        // ==============================


        // ========= Debug Log ==========
        /// <summary>
        /// Write log only in debug mode.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="LogType"></param>
        public virtual void DebugLog(object Message, RLogType LogType = RLogType.Log)
        {
            if (!this.IsLoaded) return;

            this.Log(Message, LogType, true);
        }
        /// <summary>
        /// Write log only in debug mode.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="LogType"></param>
        public virtual void DebugLog(IEnumerable<object> Message, RLogType LogType = RLogType.Log)
        {
            this.DebugLog(Message.ToString(), LogType);
        }
        /// <summary>
        /// Write Exception Debug Log.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="LogType"></param>
        public virtual void DebugLog(Exception exception, RLogType LogType = RLogType.Log)
        {
            if (!this.IsLoaded) return;

            this.Log(exception.ToString(), LogType);
        }
        /// <summary>
        /// Write Exception Debug Log.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="exception"></param>
        /// <param name="LogType"></param>
        public virtual void DebugLog(string Message, Exception exception, RLogType LogType = RLogType.Log)
        {
            this.Log(Message + "\n" + exception, LogType, true) ;
        }
        // ==============================

        /// <summary>
        /// Write log in the current mod folder.
        /// </summary>
        /// <param name="Message">Message to write in the log.</param>
        /// <param name="prefix">The prefix</param>
        protected virtual void WriteLogLocally(string Message, string prefix)
        {
            if (!this.IsLoaded) return;

            try
            {
                if(this.UseLocalLog)
                {

                    if (!File.Exists(LogFilePath))
                    {
                        using (File.Create(LogFilePath)); // Ignore warning.
                    }
                    if (!string.IsNullOrEmpty(Message))
                    {
                        string LogToWrite = prefix + " : " + Message;


                        using (StreamWriter writer = File.AppendText(LogFilePath))
                        {
                            writer.WriteLine(LogToWrite);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                this.ExceptionLog("Failed write log locally\n" + ex);
            }
        }

        /// <summary>
        /// Delete all lines in the log file.
        /// </summary>
        public virtual void ClearFileLog()
        {
             if (File.Exists(LogFilePath))
                {
                    try
                    {
                        File.Delete(LogFilePath);
                    }
                    catch (Exception ex)
                    {
                        Main.bmod.ExceptionLog(ex);
                    }
                }

                if (!File.Exists(LogFilePath))
                {
                    using (File.Create(LogFilePath)) ; // Ignore warning.
                }
        }
    }
}
