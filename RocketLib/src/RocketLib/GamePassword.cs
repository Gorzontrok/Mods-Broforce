using System;
using System.Collections.Generic;
using System.Linq;

namespace RocketLib
{
    /// <summary>
    /// Add password to the game
    /// </summary>
    public class GamePassword
    {
        public static GamePassword[] Passwords
        {
            get { return _passwords; }
        }
        private static GamePassword[] _passwords = new GamePassword[0];

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
            AddPassword(this);
        }

        /// <summary>
        /// Return password
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return password;
        }

        private static void AddPassword(GamePassword password)
        {
            _passwords.Append(password);
        }
    }

    [Obsolete]
    public static class GamePasswordController
    {
        [Obsolete("Use 'GamePassword.Passwords' instead.")]
        public static List<GamePassword> GamePasswords
        {
            get
            {
                return GamePassword.Passwords.ToList();
            }
        }
    }
}
