using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PolyplayAPI.Models.Chats
{
    public class GeneralChatMessage
    {
        [BsonId] // primary key, map to MongoDB collection
        [BsonRepresentation(BsonType.ObjectId)] // handles conversion from string to ObjectId
        public string? Id { get; set; }

        [BsonElement("UserId")] public string UserId { get; set; } = null!; // it's called "UserId" inside MongoDB

        public string Message { get; set; } = null!;
    }
}
