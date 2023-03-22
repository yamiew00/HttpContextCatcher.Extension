using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace HttpContextCatcher.Extension.Bsons
{
    public static class BsonExtension
    {
        private static BsonValue ToPrettyBson(this string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
            {
                return BsonNull.Value;
            }

            object @object = default;
            //jsonString may fail to deserialize
            try
            {
                @object = JsonConvert.DeserializeObject(jsonString);
            }
            catch
            {
                @object = new
                {
                    text = jsonString
                };
            }

            return BsonDocument.Parse(JObject.FromObject(@object).ToString());
        }

        private static BsonValue ToPrettyBson(this object @object)
        {
            if (@object == null)
            {
                return BsonNull.Value;
            }

            return BsonDocument.Parse(JObject.FromObject(@object).ToString());
        }

        public static BsonContextCatcher ToBsonType(this ContextCatcher contextCatcher)
        {
            return new BsonContextCatcher
            {
                Time = contextCatcher.Time,
                Request = ConvertRequest(contextCatcher.Request),
                Response = ConvertResponse(contextCatcher.Response),
                Exception = contextCatcher.Exception,
                Item = ConvertItem(contextCatcher.Items)
            };
        }

        private static RequestBson ConvertRequest(RequestCatcher request)
        {
            if (request == null) return default;
            return new RequestBson
            {
                Body = request.Body.ToPrettyBson(),
                Headers = request.Headers,
                Method = request.Method,
                Queries = request.Queries,
                Path = request.Path,
            };
        }

        private static ResponseBson ConvertResponse(ResponseCatcher response)
        {
            if(response == null) return default;
            return new ResponseBson
            {
                Body = response.Body.ToPrettyBson(),
                ResSecond = response.ResSecond,
                StatusCode = response.StatusCode
            };
        }

        private static ItemBson ConvertItem(ItemCatcher items)
        {
            if(items == null) return default;
            return new ItemBson
            {
                Items = (items == null) ?
                            null :
                            items.Items.ToDictionary(keyValue => keyValue.Key,
                                                     keyValue => keyValue.Value.ToPrettyBson())

            };
        }
    }
}
