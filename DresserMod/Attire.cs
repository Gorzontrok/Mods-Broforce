using HarmonyLib;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace DresserMod
{
    [Obsolete("Use FuturisticAttire")]
    public class Attire : IAttire
    {
        public const string WEARER_KEY = "Wearer";
        public const string NAME_KEY = "Name";

        // IAttire variables
        public string Name { get; set; }
        public string Wearer { get; set; }
        [JsonIgnore]
        public string Directory { get; set; }
        [JsonIgnore]
        public bool Enabled { get; set; }
        [JsonIgnore]
        public string Id { get => Wearer + '-' + Name; }

        /// <summary>
        /// Key: Variable Name ; Value: image
        /// </summary>
        public Dictionary<string, string> Clothes { get; set; } = new Dictionary<string, string>();

        public Attire(string name, string directory)
        {
            this.Name = name;
            this.Directory = directory;

            this.Enabled = !Main.settings.unactiveFiles.Contains(Id);
        }

        public static Attire TryReadJson(string file)
        {
            var json = File.ReadAllText(file);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            if (dict == null || dict.Count == 0)
                throw new Exception("Json is empty");
            if (!dict.ContainsKey(WEARER_KEY))
                throw new Exception("Wearer field is missing");
            var name = Path.GetFileName(file);
            if (dict.ContainsKey(NAME_KEY))
                name = dict[NAME_KEY];
            var result = new Attire(name, Path.GetDirectoryName(file))
            {
                Clothes = dict,
                Wearer = dict[WEARER_KEY]
            };
            return result;
        }


        public void SuitUp(object obj)
        {
            Traverse traverse = obj.GetTraverse();
            string[] keys = Clothes.Keys.ToArray();

            obj.DynamicFieldsValueSetter(Clothes.ToDictionary((p) =>  p.Key, (p) => (object)p.Value), new string[] { WEARER_KEY, NAME_KEY }, PutOn);
        }

        private void PutOn(Traverse field, string key, object value)
        {
            string choice = (string)value;
            if (string.IsNullOrEmpty(choice))
            {
                return;
            }

            Type fieldType = field.GetValueType();
            if (fieldType == typeof(SpriteSM))
            {
                SpriteSM sprite = field.GetValue<SpriteSM>();
                sprite.SetTexture(CreateTexture(choice));
                field.SetValue(sprite);
            }
            else if (fieldType == typeof(Material))
            {
                Material material = field.GetValue<Material>();
                material.mainTexture = CreateTexture(choice);
                field.SetValue(material);
            }
            else if (fieldType == typeof(Texture))
            {
                field.SetValue(CreateTexture(choice));
            }
            else if (fieldType == typeof(Texture2D))
            {
                field.SetValue((Texture2D)CreateTexture(choice));
            }
            else
            {
                throw new Exception("Field value type is:" + fieldType.Name);
            }
        }

        private Texture CreateTexture(string path)
        {
            Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            tex.LoadImage(File.ReadAllBytes(Path.Combine(Directory, path)));
            tex.filterMode = FilterMode.Point;
            tex.anisoLevel = 1;
            tex.mipMapBias = 0;
            tex.wrapMode = TextureWrapMode.Repeat;

            return tex;
        }
    }
}
