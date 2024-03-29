﻿using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RocketLib.Loggers;

namespace RocketLib
{
    /// <summary>
    /// Add password to the game
    /// </summary>
    public class GamePassword
    {
        public readonly string password = string.Empty;
        public readonly Action action;

        /// <summary>
        /// Create the game password.
        /// </summary>
        /// <param name="_password"></param>
        /// <param name="_action"></param>
        public GamePassword(string _password, Action _action)
        {
            password = _password.ToLower();
            action = _action;
            GamePasswordController.AddPassword(this);
        }

        /// <summary>
        /// Return password
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return password;
        }
    }

    public static class GamePasswordController
    {
        private static List<GamePassword> gamePasswords = new List<GamePassword>();

        internal static void AddPassword(GamePassword gp)
        {
            gamePasswords.Add(gp);
        }
        public static List<GamePassword> GamePasswords
        {
            get
            {
                return new List<GamePassword>(gamePasswords);
            }
        }

        [HarmonyPatch(typeof(MainMenu), "ProcessCharacter")]
        static class Password_ProcessCharacter_Patch
        {
            static void Postfix(MainMenu __instance)
            {
                foreach (GamePassword password in gamePasswords)
                {
                    try
                    {
                        if(__instance.CallMethod<bool>("CheckCheatString", new object[] {password.password}))
                        {
                            Sound sound7 = Sound.GetInstance();
                            sound7.PlaySoundEffect(__instance.drumSounds.specialSounds[0], 0.75f);
                            password.action();
                        }
                    }
                    catch (Exception ex)
                    {
                        ScreenLogger.Instance.ExceptionLog("Failed to apply the password: " + password.password, ex);
                    }
                }
            }
        }
    }
}
