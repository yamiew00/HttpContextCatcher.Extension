using MongoDB.Bson;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HttpContextCatcher.Extension.Converters
{
    public class BsonValueJsonConverter : JsonConverter<BsonValue>
    {
        public override BsonValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException("Read is not supported for this converter.");
        }

        public override void Write(Utf8JsonWriter writer, BsonValue value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, BsonTypeMapper.MapToDotNetValue(value), options);
        }
    }
}
