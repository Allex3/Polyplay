using Microsoft.AspNetCore.Mvc;
using PolyplayAPI.Models.Chats;
using PolyplayAPI.Services;

namespace PolyplayAPI.Controllers.Chat;

[ApiController]
[Route("api/[controller]")]
public class GeneralChatController(GeneralChatService generalChatService) : ControllerBase
{
    private readonly GeneralChatService _generalChatService = generalChatService;

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

