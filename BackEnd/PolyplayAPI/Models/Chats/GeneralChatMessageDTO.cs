using System.Text.Json.Serialization;

namespace PolyplayAPI.Models.Chats
{
    public class GeneralChatMessageDTO
    {
        [JsonPropertyName("userId")] public string UserId { get; set; } = null!;

        [JsonPropertyName("username")] public string Username { get; set; } = null!;

        [JsonPropertyName("message")] public string Message { get; set; } = null!;
    }
}
