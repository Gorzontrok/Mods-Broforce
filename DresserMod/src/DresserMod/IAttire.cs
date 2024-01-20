using Newtonsoft.Json;
using System.Collections.Generic;

namespace DresserMod
{
    /// <summary>
    /// Attires Interface
    /// </summary>
    public interface IAttire
    {
        string Name { get; set; }
        /// <summary>
        /// The object name which wear the Attire
        /// </summary>
        string Wearer { get; set; }

        string Directory { get; set; }
        bool Enabled { get; set; }
        string Id { get; }


        Dictionary<string, string> Clothes { get; set; }

        void SuitUp(object obj);
    }
}
