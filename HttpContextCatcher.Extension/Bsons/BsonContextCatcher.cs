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
        public ObjectId _id { get; internal set; }

        [BsonElement("time")]
        public DateTime Time { get; internal set; }

        [BsonElement("costSecond")]
        public decimal CostSecond { get; internal set; }

        [BsonElement("statusCode")]
        public int StatusCode { get; internal set; }

        [BsonElement("request")]
        public RequestBson Request { get; internal set; }

        [BsonElement("response")]
        public ResponseBson Response { get; internal set; }

        [BsonElement("exception")]
        public ExceptionCatcher Exception { get; internal set; }
    }
}
