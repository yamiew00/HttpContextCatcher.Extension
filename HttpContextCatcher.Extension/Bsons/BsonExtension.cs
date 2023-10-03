using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace HttpContextCatcher.Extension.Bsons
{
    public static class BsonExtension
    {
        /// <summary>
        /// Converts a given ContextCatcher object into a BsonContextCatcher object.
        /// </summary>
        /// <param name="contextCatcher">The source ContextCatcher object containing JSON string values.</param>
        /// <returns>A new BsonContextCatcher object with BsonValue representations for the respective JSON string properties of the original ContextCatcher.</returns>
        /// <remarks>
        /// This method facilitates the transformation of JSON strings within a ContextCatcher object into BsonValue types 
        /// that are more suitable for MongoDB storage. 
        /// </remarks>
        public static BsonContextCatcher ToBsonType(this ContextCatcher contextCatcher)
        {
            return new BsonContextCatcher
            {
                Time = contextCatcher.Time,
                Request = ConvertRequest(contextCatcher.Request),
                Response = ConvertResponse(contextCatcher.Response),
                Exception = contextCatcher.Exception
            };
        }

        private static BsonValue ToPrettyBson(this string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
            {
                return BsonNull.Value;
            }

            // Remove leading and trailing white-spaces
            jsonString = jsonString.Trim();

            // jsonString may be a object or array
            if (jsonString.StartsWith("{"))
            {
                //json is a object
                try
                {
                    return BsonDocument.Parse(jsonString);
                }
                catch
                {
                    return new BsonDocument { { "text", jsonString } };
                }
            }
            else if (jsonString.StartsWith("["))
            {
                //json is an array
                try
                {
                    return BsonSerializer.Deserialize<BsonArray>(jsonString);
                }
                catch
                {
                    return new BsonDocument { { "text", jsonString } };
                }
            }
            else
            {
                // other format
                return new BsonDocument { { "text", jsonString } };
            }
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
    }
}
