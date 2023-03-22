using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using System;

namespace HttpContextCatcher.Extension.Bsons
{
    [BsonIgnoreExtraElements]
    public class BsonContextCatcher
    {
        [BsonId]
        [BsonElement("_id")]
        [BsonIgnoreIfDefault]
        [JsonIgnore]
        public ObjectId _id { get; set; }

        [BsonElement("time")]
        public DateTime Time { get; set; }

        [BsonElement("request")]
        public RequestBson Request { get; set; }

        [BsonElement("response")]
        public ResponseBson Response { get; set; }

        [BsonElement("exception")]
        public ExceptionCatcher Exception { get; set; }

        [BsonElement("item")]
        [BsonIgnoreIfDefault]
        public ItemBson Item { get; set; }
    }
}
