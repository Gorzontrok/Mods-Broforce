
using Newtonsoft.Json;
using System.IO;
using System;

namespace DresserMod
{
    public class AttireCollection
    {
        public FuturisticAttire[] Attires = new FuturisticAttire[0];

        public static AttireCollection TryLoadFromJson(string file)
        {
            if (!File.Exists(file))
                throw new FileNotFoundException(file);

            string json = File.ReadAllText(file);
            if (json.IsNullOrEmpty())
                throw new Exception($"{file} is empty");

            AttireCollection attireCollection = JsonConvert.DeserializeObject<AttireCollection>(json);

            foreach(FuturisticAttire attire in attireCollection.Attires)
            {
                attire.Directory = Path.GetDirectoryName(file);
            }

            return attireCollection;
        }
    }
}
