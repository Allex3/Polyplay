using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolyplayAPI.Models;
using PolyplayAPI.Models.Chats;
using PolyplayAPI.Services;
using System.Net.WebSockets;
using System.Text;

namespace PolyplayAPI.Controllers.Chat;

[ApiController]
[Route("api/[controller]")]
public class GeneralChatController(GeneralChatService generalChatService, ConcurrentDictionary<string, WebSocket> wsConnections) : ControllerBase
{
    private readonly GeneralChatService _generalChatService = generalChatService;
    private readonly ConcurrentDictionary<string, WebSocket> _wsConnections = wsConnections;

    [Route("~/ws/generalChat")] // ~ overrides default routing, so
    // instead of api/games/ws/... it will be just /ws/startTestGames
    public async Task EstablishGeneralChatWs()
    {   // HttpContext of the executing action
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

            CancellationToken ct = HttpContext.RequestAborted;

            if (ct.IsCancellationRequested)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest; // idk
            }
            string webSocketId = Guid.NewGuid().ToString(); // global ID for this websocket connection

            wsConnections.TryAdd(webSocketId, webSocket); // to broadcast later

            await StartGeneralChatForUser(webSocket, webSocketId);

        }

        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest; // not websocket request
        }
    }
    private async Task StartGeneralChatForUser(WebSocket webSocket, string webSocketId)
    {
        const int THIS_IS_NOT_A_MAGIC_NUMBER = 1024;

        CancellationToken ct = CancellationToken.None;

        do
        {

            // this while just runs and waits to receive messages, then broadcasts them to the other clients

            GeneralChatMessage receivedMessage = await WsUtilities.ReadJsonAsync<GeneralChatMessage>(webSocket, ct);
            if (receivedMessage == null)
            {
                if (webSocket.State != WebSocketState.Open)
                {
                    break;
                }

                continue; // act as if message doesn't exist
            }

            await _generalChatService.CreateAsync(receivedMessage);

            foreach (var connection in _wsConnections) // not working broadcast
            {
                if (connection.Value.State != WebSocketState.Open)
                {
                    continue;
                }

                await WsUtilities.SendJsonAsync<GeneralChatMessage>(connection.Value, receivedMessage, ct);
            }
        } while (!ct.IsCancellationRequested);

        WebSocket dummy;

        _wsConnections.TryRemove(webSocketId, out dummy);
        await webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "UserDisconnected", ct);

        webSocket.Dispose();

    }

    [HttpGet]
    public async Task<List<GeneralChatMessage>> Get() => await _generalChatService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<GeneralChatMessage>> Get(string id)
    {
        var message = await _generalChatService.GetAsync(id);

        if (message is null)
        {
            return NotFound();
        }

        return message;
    }

    [HttpPost]
    public async Task<IActionResult> Post(GeneralChatMessage newMessage)
    {
        await _generalChatService.CreateAsync(newMessage);

        return CreatedAtAction(nameof(Get), new { id = newMessage.Id }, newMessage);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, GeneralChatMessage updatedMessage)
    {
        var message = await _generalChatService.GetAsync(id);

        if (message is null)
        {
            return NotFound();
        }

        updatedMessage.Id = message.Id;

        await _generalChatService.UpdateAsync(id, updatedMessage);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var message = await _generalChatService.GetAsync(id);

        if (message is null)
        {
            return NotFound();
        }

        await _generalChatService.RemoveAsync(id);

        return NoContent();
    }
}

