using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DresserMod
{
    public static class StorageRoom
    {
        [Obsolete("Use 'WardrobesDirectory' instead")]
        public static readonly string AssetDirectory;
        public static readonly string WardrobesDirectory;

        [Obsolete("Not use since 'FuturisticAttires")]
        public static readonly Dictionary<string, string> defaultJson =  new Dictionary<string, string>()
        {
            { "Wearer", "" },
            { "sprite", "" }
        };

        static StorageRoom()
        {
            AssetDirectory = Path.Combine(Main.mod.Path, "assets");
            WardrobesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "DM_Wardrobes");

            subscribers = new List<string>();
            subscribers.Add(AssetDirectory);
            subscribers.Add(WardrobesDirectory);
        }

        public static Dictionary<string, Wardrobe> Wardrobes
        {
            get => _wardrobes;
        }

        private static Dictionary<string, Wardrobe> _wardrobes = new Dictionary<string, Wardrobe>();

        private static List<string> subscribers = new List<string>();

        public static void Initialize()
        {
            _wardrobes.Clear();
            CheckDirectory();
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
                if (Path.GetFileName(file).ToLower() == "info.json")
                    continue;

                    IAttire attire = null;
                try
                {
                    if (file.EndsWith(".fa.json")) // It's a FuturisticAttire
                    {
                        attire = FuturisticAttire.TryLoadFromJson(file);
                    }
                    else if (file.EndsWith(".ac.json"))
                    {
                        AttireCollection collection = AttireCollection.TryLoadFromJson(file);
                        if (collection != null)
                        {
                            LoadAttiresFromCollection(collection);
                        }
                    }
                    else
                    {
                        attire = Attire.TryReadJson(file);
                    }
                }
                catch(Exception ex)
                {
                    Main.Log($"{file}:\n" + ex);
                }

                if (attire != null)
                {
                    RegisterAttire(attire);
                }
            }
        }

        private static void RegisterAttire(IAttire attire)
        {
            if (!_wardrobes.ContainsKey(attire.Wearer)) // Creat Wardrobes
            {
                _wardrobes.Add(attire.Wearer, new Wardrobe(attire.Wearer));
            }
            _wardrobes[attire.Wearer].AddAttire(attire); // Add Attire to wardrobe
        }

        private static void LoadAttiresFromCollection(AttireCollection attireCollection)
        {
            foreach(FuturisticAttire attire in attireCollection.Attires)
            {
                RegisterAttire(attire);
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
        public static void CreateFuturisticAttireJsonFile(string fileName, string directory)
        {
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            File.WriteAllText(Path.Combine(directory, fileName + ".fa.json"), JsonConvert.SerializeObject(new FuturisticAttire(fileName, directory), Formatting.Indented, settings));
        }
        public static void CreateAttireCollectionJsonFile(string fileName, string directory)
        {
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            File.WriteAllText(Path.Combine(directory, fileName + ".ac.json"), JsonConvert.SerializeObject(new AttireCollection(), Formatting.Indented, settings));
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
                Initialize();
            }
        }

        private static void CheckDirectory()
        {
            if (!Directory.Exists(AssetDirectory))
            {
                Directory.CreateDirectory(AssetDirectory);
            }
            if (!Directory.Exists(WardrobesDirectory))
            {
                Directory.CreateDirectory(WardrobesDirectory);
            }
        }
    }
}
