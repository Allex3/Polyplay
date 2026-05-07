using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace PolyplayAPI.Models.Chats
{
    public class GeneralChatMessageDTO
    {
        [JsonPropertyName("userId")]
        public long UserId { get; set; }

         [JsonPropertyName("username")] public string Username { get; set; } = null!;

         [JsonPropertyName("message")]  public string Message { get; set; } = null!;
    }
}
