using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SpinMod
{
    public class Vector3Converter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var c = (Vector3)value;
            var objValue = new { c.x, c.y, c.z };
            serializer.Serialize(writer, objValue);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return new Vector3();
            }
            else
            {
                JObject obj = JObject.Load(reader);
                return new Vector3(obj.Value<float>("x"), obj.Value<float>("y"), obj.Value<float>("z"));
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Vector3));
        }
    }

}
