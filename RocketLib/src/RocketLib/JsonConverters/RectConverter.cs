using System;
using Newtonsoft.Json;
using UnityEngine;

namespace RocketLib.JsonConverters
{
    public class RectConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(Rect))
            {
                return true;
            }
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var t = serializer.Deserialize(reader);
            var iv = JsonConvert.DeserializeObject<Rect>(t.ToString());
            return iv;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Rect r = (Rect)value;

            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(r.x);
            writer.WritePropertyName("y");
            writer.WriteValue(r.y);
            writer.WritePropertyName("width");
            writer.WriteValue(r.width);
            writer.WritePropertyName("height");
            writer.WriteValue(r.height);
            writer.WriteEndObject();
        }
    }
}
