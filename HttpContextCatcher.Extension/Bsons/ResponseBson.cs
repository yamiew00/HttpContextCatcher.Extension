using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HttpContextCatcher.Extension.Bsons
{
    public class ResponseBson
    {
        [BsonElement("body")]
        public BsonValue Body { get; set; }

        [BsonElement("contentType")]
        public string ContentType { get; set; }
    }
}
