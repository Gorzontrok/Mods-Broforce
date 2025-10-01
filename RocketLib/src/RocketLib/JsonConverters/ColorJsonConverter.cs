using System;
using Newtonsoft.Json;
using UnityEngine;

namespace RocketLib.JsonConverters
{
    public class ColorJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(Color))
            {
                return true;
            }
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object obj = serializer.Deserialize(reader);
            Color color = JsonConvert.DeserializeObject<Color>(obj.ToString());
            return color;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Color r = (Color)value;

            writer.WriteStartObject();
            writer.WritePropertyName("r");
            writer.WriteValue(r.r);
            writer.WritePropertyName("g");
            writer.WriteValue(r.g);
            writer.WritePropertyName("b");
            writer.WriteValue(r.b);
            writer.WritePropertyName("a");
            writer.WriteValue(r.a);
            writer.WriteEndObject();
        }
    }
}
