using System;
using Newtonsoft.Json;

namespace bSpin {
    internal class EasingJsonConverter : JsonConverter {
        public override bool CanConvert(Type objectType) {
            return objectType == typeof(EasingFunction.Ease);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer) {
            if (reader.TokenType == JsonToken.Null) return EasingFunction.Ease.Linear;

            EasingFunction.Ease outEase;
            Enum.TryParse((string)reader.Value, out outEase);
            return outEase;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            var c = (EasingFunction.Ease)value;
            var objValue = c.ToString();
            serializer.Serialize(writer, objValue);
        }
    }
}