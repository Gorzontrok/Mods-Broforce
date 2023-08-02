using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DresserMod
{
    public static class StorageRoom
    {
        public static readonly string AssetDirectory;
        public static readonly Dictionary<string, string> defaultJson =  new Dictionary<string, string>()
        {
            { "Wearer", "" },
            { "sprite", "" }
        };

        static StorageRoom()
        {
            AssetDirectory = Path.Combine(Main.mod.Path, "assets");
        }

        public static Dictionary<string, Wardrobe> wardrobes = new Dictionary<string, Wardrobe>();

        public static void Init()
        {
            wardrobes.Clear();
            CheckDirectory();
            ReadFiles();
        }

        public static void ReadFiles()
        {
            string[] files = Directory.GetFiles(AssetDirectory, "*.json", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                try
                {
                    Attire attire = Attire.ReadJson(file);
                    if(attire == null) continue;
                    string key = attire.wearer;
                    if (!wardrobes.ContainsKey(key))
                    {
                        wardrobes.Add(key, new Wardrobe(key));
                    }
                    wardrobes[key].AddAttire(attire);
                }
                catch(Exception ex)
                {
                    Main.Log($"{file}:\n" + ex);
                }
            }
        }

        public static void CheckDirectory()
        {
            if(!Directory.Exists(AssetDirectory))
            {
                Directory.CreateDirectory(AssetDirectory);
            }
        }

        public static void CreateJsonFile(string fileName)
        {
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            File.WriteAllText(Path.Combine(AssetDirectory, fileName + ".json"), JsonConvert.SerializeObject(defaultJson, settings));
        }
    }
}
