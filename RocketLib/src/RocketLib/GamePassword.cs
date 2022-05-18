using System;
using System.Collections.Generic;
using System.Linq;
using RocketLibLoadMod;
using HarmonyLib;

namespace RocketLib0
{
    /// <summary>
    /// Add password to the game
    /// </summary>
    public class GamePassword
    {
        /// <summary>
        ///
        /// </summary>
        public readonly string password = string.Empty;
        /// <summary>
        ///
        /// </summary>
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
        /// ToString method
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return password;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public static class GamePasswordController
    {
        private static List<GamePassword> gamePasswords = new List<GamePassword>();

        internal static void AddPassword(GamePassword gp)
        {
            gamePasswords.Add(gp);
        }
        /// <summary>
        ///
        /// </summary>
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
                if (!Main.enabled) return;
                foreach (GamePassword password in gamePasswords)
                {
                    try
                    {
                        if(Traverse.Create(__instance).Method("CheckCheatString", new object[] {password.password}).GetValue<bool>())
                        {
                            Sound sound7 = Sound.GetInstance();
                            sound7.PlaySoundEffect(__instance.drumSounds.specialSounds[0], 0.75f);
                            password.action();
                        }
                    }
                    catch (Exception ex) { Main.bmod.logger.ExceptionLog("Failed apply the password: " + password.password, ex); }
                }
            }
        }
    }
}
