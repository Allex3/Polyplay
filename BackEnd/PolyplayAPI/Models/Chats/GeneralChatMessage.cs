using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace PolyplayAPI.Models.Chats
{
    public class GeneralChatMessage
    {
        [BsonId] // primary key, map to MongoDB collection
        [BsonRepresentation(BsonType.ObjectId)] // handles conversion from string to ObjectId
        public string? Id { get; set; }
        // the JSON representation will still be userId, camelCase, not PascalCase
        [BsonElement("UserId")] public long UserId { get; set; } // it's called "UserId" inside MongoDB

        public string Message { get; set; } = null!;
    }
}
