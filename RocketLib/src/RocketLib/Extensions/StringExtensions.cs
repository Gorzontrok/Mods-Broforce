using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace RocketLib
{
    public static class StringExtensions
    {
        public const string COLOR_START = "<color=";
        public const string COLOR_END = "</color>";

        /// <summary>
        /// Surround string with xml "color" tag
        /// </summary>
        public static string Dye(this string self, Color color)
        {
            return self.Dye(color.ToHex());
        }
        /// <summary>
		/// Surround string with xml "color" tag
		/// </summary>
        public static string Dye(this string self, string hex)
        {
            StringBuilder stringBuilder = new StringBuilder(COLOR_START)
                .Append(hex)
                .Append(self)
                .Append('>')
                .Append(COLOR_END);
            self = stringBuilder.ToString();
            return self;
        }
    }
}
