using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityModManagerNet;
using HarmonyLib;
using RocketLib.Loggers;

namespace RocketLib
{
    /// <summary>
    /// Class
    /// </summary>
    public class BroforceMod
    {
        /// <summary>
        /// The ID of the mod.
        /// </summary>
        public string ID
        {
            get
            {
                return this.__ID;
            }
        }
        /// <summary>
        /// The Unity mod of the class.
        /// </summary>
        protected UnityModManager.ModEntry __mod;
        /// <summary>
        /// The ID of the mod.
        /// </summary>
        protected string __ID = string.Empty;
        /// <summary>
        /// Is the mod is successful load.
        /// </summary>
        public bool Loaded { get; protected set; }

        /// <summary>
        /// Here you access Logger method.
        /// </summary>
        public Loggers.Logger logger { get; protected set; }

        /// <summary>
        /// Call when exiting game.
        /// </summary>
        public Action OnExitGame = null;
        /// <summary>
        /// Call when a level is finished.
        /// </summary>
        public Action OnLevelFinished = null;
        /// <summary>
        /// Call when all mod are loaded.
        /// </summary>
        public Action OnAfterLoadMods = null;

        /// <summary>
        /// Create the mod.
        /// </summary>
        public BroforceMod()
        { }

        /// <summary>
        /// Create the mod.
        /// </summary>
        /// <param name="mod"></param>
        public BroforceMod(UnityModManager.ModEntry mod)
        {
            Load(mod);
        }

        /// <summary>
        /// Function to call for loading the mod.
        /// </summary>
        /// <param name="mod">UnityModManager mod</param>
        public void Load(UnityModManager.ModEntry mod)
        {
            if(!Loaded)
            {
                if (mod == null)
                {
                    new Exception("The given mod is null.");
                }

                ScreenLogger.Instance.Log("Start loading Broforce mod : " + mod.Info.Id);
                try
                {
                    this.__mod = mod;
                    this.__ID = mod.Info.Id;

                    this.logger = new Loggers.Logger(this.__mod);

                    this.Loaded = true;
                    UnityModManager.Logger.Log($" Successful loaded the Broforce Mod", $"[{this.ID}]");
                    BroforceModController.AddBroforceMod(this);
                }
                catch (Exception ex)
                {
                    this.Loaded = false;
                    ScreenLogger.Instance.ExceptionLog($"Failed Loading : {mod.Info.Id}", ex);
                }
            }
            else
            {
                Log("Mod already load.");
            }
        }

        /// <summary>
        /// The Update function. Call it in Main.OnUpdate.
        /// </summary>
        public void OnUpdate()
        {
            if (!this.Loaded) return;

            this.logger.OnUpdate();
        }

        /// <summary>
        /// Faster method to write log.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="LogType"></param>
        /// <param name="Debug"></param>
        public void Log(object Message, RLogType LogType = RLogType.Log, bool Debug = false)
        {
            this.logger.Log(Message, LogType, Debug);
        }
    }
}
