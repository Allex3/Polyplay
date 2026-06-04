using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using NuGet.Common;
using PolyplayAPI.Controllers.Auth;
using PolyplayAPI.Models;
using PolyplayAPI.Models.Chats;
using PolyplayAPI.Models.Logging;
using PolyplayAPI.Services;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace PolyplayAPI.Controllers.Chat;

[ApiController]
[Route("api/[controller]")]
public class GeneralChatController(GeneralChatService generalChatService, PolyplayDbContext dbContext, ConcurrentDictionary<string, WebSocket> wsConnections) : ControllerBase
{
    private readonly GeneralChatService _generalChatService = generalChatService;
    private readonly PolyplayDbContext _dbContext = dbContext;
    private readonly ConcurrentDictionary<string, WebSocket> _wsConnections = wsConnections;

    [Route("~/ws/generalChat")] // ~ overrides default routing, so
    // instead of api/games/ws/... it will be just /ws/startTestGames
    public async Task EstablishGeneralChatWs()
    {   // HttpContext of the executing action
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            const string HeaderKeyName = "Sec-WebSocket-Protocol";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues token);
            if (AuthValidator.Authenticate(token) == string.Empty)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
            WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

            CancellationToken ct = HttpContext.RequestAborted;

            if (ct.IsCancellationRequested)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest; // idk
            }
            string webSocketId = Guid.NewGuid().ToString(); // global ID for this websocket connection

            _wsConnections.TryAdd(webSocketId, webSocket); // to broadcast later

            await StartGeneralChatForUser(webSocket, webSocketId);

        }

        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest; // not websocket request
        }
    }

    [Route("~/ws/generalChatTypingIndicators")] // ~ overrides default routing, so
    // instead of api/games/ws/... it will be just /ws/startTestGames
    public async Task EstablishGeneralChatTypingIndicatorsWs()
    {   // HttpContext of the executing action
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            const string HeaderKeyName = "Sec-WebSocket-Protocol";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues token);
            if (AuthValidator.Authenticate(token)==string.Empty)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

            CancellationToken ct = HttpContext.RequestAborted;

            if (ct.IsCancellationRequested)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest; // idk
            }

            do
            {
                var typingIndicators = await WsUtilities.ReadJsonAsync<TypingIndicators>(webSocket, ct);

                if (typingIndicators == null)
                    continue;

                if (typingIndicators.Active)
                {
                    WsUtilities.SendJsonAsync(webSocket, new UsersCount { Count = 1 });
                }
                else // remove user from typing
                {
                    WsUtilities.SendJsonAsync(webSocket, new UsersCount { Count = -1 });
                }

            } while (!ct.IsCancellationRequested);


        }

        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest; // not websocket request
        }
    }

    /*

    [Route("~/ws/generalChatDisconnectedUsers")] // ~ overrides default routing
    public async Task EstablishGeneralChatDisconnectedUsersWs()
    {   // HttpContext of the executing action
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

            CancellationToken ct = HttpContext.RequestAborted;

            if (ct.IsCancellationRequested)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest; // idk
            }

            var userExiting = await WsUtilities.ReadJsonAsync<UserExiting>(webSocket, ct);

            var user = await _dbContext.Users.FindAsync(userExiting.UserId);

            if (user == null)
                return;


            foreach (var connection in _wsConnections) // not working broadcast
            {
                if (connection.Value.State != WebSocketState.Open)
                {
                    continue;
                }

                await WsUtilities.SendJsonAsync(webSocket, new UserExiting { UserId = user.Id, Username = user.UserName }, ct);
            }
        }

        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest; // not websocket request
        }
    }
    */
    private async Task StartGeneralChatForUser(WebSocket webSocket, string webSocketId)
    {
        const int THIS_IS_NOT_A_MAGIC_NUMBER = 1024;

        CancellationToken ct = CancellationToken.None;

        var lastTime = DateTime.Now;
        int messagesSentInOneMinute = 0;

        do
        {
            // this while just runs and waits to receive messages, then broadcasts them to the other clients

            GeneralChatMessageDTO receivedMessage = await WsUtilities.ReadJsonAsync<GeneralChatMessageDTO>(webSocket, ct);
            if (receivedMessage == null)
            {
                if (webSocket.State != WebSocketState.Open)
                {
                    break;
                }

                continue; // act as if message doesn't exist
            }

            messagesSentInOneMinute += 1;

            // message received
            if ((DateTime.Now - lastTime).TotalMinutes <= 1) // check spam
            {
                if (messagesSentInOneMinute > 15)
                {
                    _dbContext.MaliciousActivityLog.AddAsync(new MaliciousActivity
                    {
                        ActivityTypeId = 5,
                        Info = "YOu are a bad person!",
                        IpAddress = "????",
                        UserName = receivedMessage.UserName
                    });
                    _dbContext.SaveChangesAsync();
                }
            }
            else
            {
                lastTime = DateTime.Now;
                messagesSentInOneMinute = 0;
            }


            await _generalChatService.CreateAsync(new GeneralChatMessage { Message = receivedMessage.Message, UserName = receivedMessage.UserName });

            foreach (var connection in _wsConnections) // not working broadcast
            {
                if (connection.Value.State != WebSocketState.Open)
                {
                    continue;
                }

                await WsUtilities.SendJsonAsync<GeneralChatMessageDTO>(connection.Value, receivedMessage, ct);
            }
        } while (!ct.IsCancellationRequested);

        WebSocket dummy;

        _wsConnections.TryRemove(webSocketId, out dummy);
        await webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "UserDisconnected", ct);

        webSocket.Dispose();

    }

    [HttpGet]
    public async Task<List<GeneralChatMessageDTO>> Get()
    {
        var messages = await _generalChatService.GetAsync();
        List<GeneralChatMessageDTO> realMessages = [];
        messages.ForEach(message => realMessages.Add(new GeneralChatMessageDTO
        {
            Message = message.Message,
            UserName = message.UserName
        }));

        return realMessages;
    }

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
    [Authorize(Roles="User")]
    public async Task<IActionResult> Post(GeneralChatMessage newMessage)
    {
        await _generalChatService.CreateAsync(newMessage);

        return CreatedAtAction(nameof(Get), new { id = newMessage.Id }, newMessage);
    }

    [HttpPut("{id:length(24)}")]
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
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

