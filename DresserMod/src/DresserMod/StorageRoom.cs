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

        private static List<string> subscribers = new List<string>();

        public static void Init()
        {
            wardrobes.Clear();
            CheckDirectory();
            ReadFiles(AssetDirectory, SearchOption.AllDirectories);
            foreach(var subscriber in subscribers)
            {
                ReadFiles(subscriber, SearchOption.AllDirectories);
            }
        }

        public static void ReadFiles(string directory, SearchOption searchOption = SearchOption.AllDirectories)
        {
            string[] files = Directory.GetFiles(directory, "*.json", searchOption);
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

        public static void CreateJsonFile(string fileName, string directory)
        {
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            File.WriteAllText(Path.Combine(directory, fileName + ".json"), JsonConvert.SerializeObject(defaultJson, settings));
        }

        public static void AddSubscriber(string path)
        {
            if (!subscribers.Contains(path))
            {
                subscribers.Add(path);
                ReadFiles(path);
            }
        }
        public static void RemoveSubscriber(string path)
        {
            if(subscribers.Contains(path))
            {
                subscribers.Remove(path);
                Init();
            }
        }

        private static void CheckDirectory()
        {
            if (!Directory.Exists(AssetDirectory))
            {
                Directory.CreateDirectory(AssetDirectory);
            }
        }

    }
}
