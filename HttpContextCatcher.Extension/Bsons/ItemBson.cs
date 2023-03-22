using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace HttpContextCatcher.Extension.Bsons
{
    [BsonIgnoreExtraElements]
    public class ItemBson
    {
        [BsonElement("items")]
        public Dictionary<string, BsonValue> Items { get; set; }
    }
}
