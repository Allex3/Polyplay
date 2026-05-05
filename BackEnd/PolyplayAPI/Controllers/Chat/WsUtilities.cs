using System.Net.WebSockets;
using System.Text.Json;
using PolyplayAPI.Models.Chats;

namespace PolyplayAPI.Controllers.Chat
{
    public static class WsUtilities
    {
        private const int THIS_IS_NOT_A_MAGIC_NUMBER = 1024;
        public static async Task<T?> ReadJsonAsync<T>(WebSocket ws, CancellationToken ct = default)
        {
            var buffer = new ArraySegment<byte>(new byte[THIS_IS_NOT_A_MAGIC_NUMBER*16]); //16kb

            using MemoryStream ms = new MemoryStream();
            WebSocketReceiveResult receivedResult;

            
            do
            {
                try
                {

                    ct.ThrowIfCancellationRequested();
                }
                catch (System.Net.WebSockets.WebSocketException e)
                {
                    return default;
                }

                receivedResult = await ws.ReceiveAsync(buffer, ct);

                ms.Write(buffer.Array, buffer.Offset, receivedResult.Count);

            } while (!receivedResult.EndOfMessage);


            ms.Seek(0, SeekOrigin.Begin); // Changing stream position to cover whole message


            if (receivedResult.MessageType != WebSocketMessageType.Text)
                return default;

            using var reader = new StreamReader(ms, System.Text.Encoding.UTF8);
            string jsonSerialized = await reader.ReadToEndAsync(ct);
            return JsonSerializer.Deserialize <T>(jsonSerialized, options: new JsonSerializerOptions(JsonSerializerDefaults.Web)); // decoding message
        }

        public static Task SendJsonAsync<T>(WebSocket ws, T data, CancellationToken ct = default)
        {
            var buffer = System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));
            var segment = new ArraySegment<byte>(buffer);
            return ws.SendAsync(segment, WebSocketMessageType.Text, true, ct);
        }
    }
}
