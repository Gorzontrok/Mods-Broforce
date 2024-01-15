using HarmonyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace DresserMod
{
    public class FuturisticAttire
    {
        public string Name { get; set; }
        public string Wearer { get; set; }
        // variable name, image path
        public Dictionary<string, string> Clothes { get; set; } = new Dictionary<string, string>();
        // variable name, value
        public Dictionary<string, object> Stats { get; set; } = new Dictionary<string, object>();

        [JsonIgnore]
        public string directory = string.Empty;

        public FuturisticAttire(string name, string directory)
        {
            Name = name;
            this.directory = directory;
        }

        public static FuturisticAttire TryLoadFromJson(string file)
        {
            if (!File.Exists(file))
                throw new FileNotFoundException(file);

            string json = File.ReadAllText(file);
            if (json.IsNullOrEmpty())
                throw new Exception($"{file} is empty");

            FuturisticAttire futuristicAttire = JsonConvert.DeserializeObject<FuturisticAttire>(json);

            if (!futuristicAttire.Wearer.IsNullOrEmpty())
                throw new MissingFieldException("Wearer field is null or empty");

            if (futuristicAttire.Name.IsNullOrEmpty())
                futuristicAttire.Name = Path.GetFileName(file);

            futuristicAttire.directory = Path.GetDirectoryName(file);
            return futuristicAttire;
        }

        public void SuitUp(object obj)
        {
            Traverse traverse = obj.GetTraverse();
            string[] keys = Clothes.Keys.ToArray();

            // Change sprites
            VariableSetter.Dynamic(this, Clothes.ToDictionary((p) => p.Key, (p) => (object)p.Value), PutOnCloth);

            // Change the variables
            VariableSetter.Dynamic(this, Stats, TweakWearer);
        }

        private void PutOnCloth(Traverse field, string key, object value)
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
                var tex = ResourcesController.GetTexture(directory, choice);
                if (tex != null)
                {
                    sprite.SetTexture(tex);
                    field.SetValue(sprite);
                }
            }
            else if (fieldType == typeof(Material))
            {
                Material material = field.GetValue<Material>();
                var tex = ResourcesController.GetTexture(directory, choice);
                if (tex != null)
                {
                    material.mainTexture = (Texture)tex;
                    field.SetValue(material);
                }
            }
            else if (fieldType == typeof(Texture))
            {
                var tex = ResourcesController.GetTexture(directory, choice);
                if (tex != null)
                    field.SetValue((Texture)tex);
            }
            else if (fieldType == typeof(Texture2D))
            {
                var tex = ResourcesController.GetTexture(directory, choice);
                if (tex != null)
                    field.SetValue((Texture2D)tex);
            }
            else if (fieldType == typeof(Component) || fieldType.IsAssignableFrom(typeof(Component)))
            {
                Renderer renderer = field.GetValue<Component>().GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material material = renderer.material;
                    if (material != null)
                    {
                        var tex = ResourcesController.GetTexture(directory, choice);
                        if (tex != null)
                            material.mainTexture = tex;
                    }
                }
            }
            else if (fieldType == typeof(GameObject) || fieldType.IsAssignableFrom(typeof(GameObject)))
            {
                Renderer renderer = field.GetValue<GameObject>().GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material material = renderer.material;
                    if (material != null)
                    {
                        var tex = ResourcesController.GetTexture(directory, choice);
                        if (tex != null)
                            material.mainTexture = tex;
                    }
                }
            }
            else
            {
                throw new Exception($"Variable is type of {fieldType}, try to place it in {nameof(Stats)}.");
            }
        }

        private void TweakWearer(Traverse field, string key, object value)
        {
            try
            {
                Type fieldType = field.GetValueType();
                if (fieldType == typeof(Enum))
                {
                    if (value is string)
                    {
                        field.SetValue(Enum.Parse(fieldType, (string)value));
                    }
                    field.SetValue((int)value);
                }
                else if (fieldType == typeof(float))
                {
                    if (value is double)
                    {
                        value = Convert.ToSingle(value);
                    }
                    field.SetValue(value);
                }
                else if (fieldType == typeof(Int32))
                {
                    value = Convert.ToInt32(value);
                    field.SetValue(value);
                }
                else if (fieldType == typeof(Projectile) || fieldType.IsAssignableFrom(typeof(Projectile)))
                {
                    var name = (string)value;
                    Projectile projectile = ResourcesController.LoadAssetSync<Projectile>(name);
                    if (projectile != null)
                        field.SetValue(projectile);
                }
                else if (fieldType == typeof(Grenade) || fieldType.IsAssignableFrom(typeof(Grenade)))
                {
                    var name = (string)value;
                    Grenade grenade = ResourcesController.LoadAssetSync<Grenade>(name);
                    if (grenade != null)
                        field.SetValue(grenade);
                }
                else
                {
                    field.SetValue(value);
                }
            }
            catch (Exception ex)
            {
                Main.Log($"Key: {key} ; Value: {value}\n" + ex);
            }
        }
    }
}
