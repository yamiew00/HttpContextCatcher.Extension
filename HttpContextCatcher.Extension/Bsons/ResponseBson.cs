using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HttpContextCatcher.Extension.Bsons
{
    public class ResponseBson
    {
        [BsonElement("statusCode")]
        public int? StatusCode { get; set; }

        [BsonElement("body")]
        public BsonValue Body { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        [BsonElement("resSecond")]
        public decimal ResSecond { get; set; }
    }
}
