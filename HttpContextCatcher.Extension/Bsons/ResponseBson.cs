using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HttpContextCatcher.Extension.Bsons
{
    public class ResponseBson
    {
        [BsonRepresentation(BsonType.Int32)]
        [BsonElement("statusCode")]
        public int? StatusCode { get; set; }

        [BsonElement("body")]
        public BsonValue Body { get; set; }
    }
}
