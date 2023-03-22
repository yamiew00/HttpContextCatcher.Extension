using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace HttpContextCatcher.Extension.Bsons
{
    [BsonIgnoreExtraElements]
    public class RequestBson
    {
        [BsonElement("path")]
        public string Path { get; set; }

        [BsonElement("method")]
        public string Method { get; set; }

        [BsonElement("body")]
        public BsonValue Body { get; set; }

        [BsonElement("queries")]
        public Dictionary<string, string> Queries { get; set; }

        [BsonElement("headers")]
        public Dictionary<string, string> Headers { get; set; }
    }
}
