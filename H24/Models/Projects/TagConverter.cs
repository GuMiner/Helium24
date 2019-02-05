using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace H24.Models
{
    public class TagConverter : JsonConverter
    {
        /// <summary>
        /// TODO: Make this return a valid result to avoid mistakes in the future.
        /// </summary>
        public override bool CanConvert(Type objectType)
            => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            List<Tag> tags = new List<Tag>();
            if (reader.TokenType == JsonToken.StartArray)
            {
                JArray array = JArray.Load(reader);
                foreach (JToken obj in array.Children())
                {
                    tags.Add(Tag.Parse(obj.Value<string>()));
                }
            }

            return tags.ToArray();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
