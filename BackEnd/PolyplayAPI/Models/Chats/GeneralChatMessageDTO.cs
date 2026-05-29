using System.Text.Json.Serialization;

namespace PolyplayAPI.Models.Chats
{
    public class GeneralChatMessageDTO
    {

        [JsonPropertyName("userName")] public string UserName { get; set; } = null!;

        [JsonPropertyName("message")] public string Message { get; set; } = null!;
    }
}
