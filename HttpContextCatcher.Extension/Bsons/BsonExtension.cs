using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;

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
                CostSecond = contextCatcher.CostSecond,
                Request = ConvertRequest(contextCatcher.Request),
                Response = ConvertResponse(contextCatcher.Response),
                StatusCode = contextCatcher.StatusCode,
                Exception = contextCatcher.Exception
            };
        }

        private static BsonValue ToPrettyBson(this string content, string contentType)
        {
            if (string.IsNullOrEmpty(content))
            {
                return BsonNull.Value;
            }

            if (contentType.StartsWith("application/json"))
            {
                // jsonString may be a object or array
                if (content.StartsWith("{"))
                {
                    // json is an object
                    try
                    {
                        return BsonDocument.Parse(content);
                    }
                    catch
                    {
                        return new BsonDocument { { "text", content } };
                    }
                }
                else if (content.StartsWith("["))
                {
                    // json is an array
                    try
                    {
                        return BsonSerializer.Deserialize<BsonArray>(content);
                    }
                    catch
                    {
                        return new BsonDocument { { "text", content } };
                    }
                }
                else
                {
                    // other format
                    return new BsonDocument { { "text", content } };
                }
            }
            else if (contentType.StartsWith("multipart/form-data"))
            {
                // Assuming content is the raw body of the multipart/form-data.
                try
                {
                    var boundaryIndex = contentType.IndexOf("boundary=");
                    if (boundaryIndex == -1)
                    {
                        return new BsonDocument { { "error", "Boundary not found in Content-Type." } };
                    }

                    var boundary = contentType.Substring(boundaryIndex + "boundary=".Length);

                    // Split the content by boundary
                    var parts = content.Split(new string[] { "--" + boundary }, StringSplitOptions.RemoveEmptyEntries);

                    var bsonDocument = new BsonDocument();

                    foreach (var part in parts)
                    {
                        // Skip empty or non-data parts
                        if (string.IsNullOrWhiteSpace(part) || part.StartsWith("--")) continue;

                        var headerEndIndex = part.IndexOf("\r\n\r\n");
                        if (headerEndIndex == -1) continue;

                        var headers = part.Substring(0, headerEndIndex).Trim();
                        var body = part.Substring(headerEndIndex + 4).Trim(); // Skipping the two new lines

                        var headerLines = headers.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                        var name = "unknown";
                        foreach (var line in headerLines)
                        {
                            if (line.StartsWith("Content-Disposition"))
                            {
                                var nameIndex = line.IndexOf("name=\"") + 6;
                                var nameEndIndex = line.IndexOf("\"", nameIndex);
                                name = line.Substring(nameIndex, nameEndIndex - nameIndex);
                                break;
                            }
                        }

                        bsonDocument.Add(name, body);
                    }

                    return bsonDocument;
                }
                catch (Exception ex)
                {
                    return new BsonDocument { { "error", ex.Message }, { "content", content } };
                }
            }
            else
            {
                // Other Content-Type, return original content as a text field
                return new BsonDocument { { "text", content } };
            }
        }


        private static RequestBson ConvertRequest(RequestCatcher request)
        {
            if (request == null) return default;
            return new RequestBson
            {
                Body = request.Body.ToPrettyBson(request.ContentType),
                Headers = request.Headers,
                Method = request.Method,
                Queries = request.Queries,
                Path = request.Path,
                ContentType = request.ContentType
            };
        }

        private static ResponseBson ConvertResponse(ResponseCatcher response)
        {
            if(response == null) return default;
            return new ResponseBson
            {
                Body = response.Body.ToPrettyBson(response.ContentType),
                ContentType = response.ContentType
            };
        }
    }
}
