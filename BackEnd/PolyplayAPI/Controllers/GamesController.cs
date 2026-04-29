using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolyplayAPI.Filters;
using PolyplayAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace PolyplayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly GameDbContext _context;
        private FakeData _fakeData;

        public GamesController(GameDbContext context)
        {
            _context = context;
            _fakeData = new FakeData();
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames([FromQuery] PaginationParameters paginationParams)
        {
            var gamesQuery = _context.Games.AsQueryable();
            var totalGames = await gamesQuery.CountAsync();
            var games = await gamesQuery.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            var paginatedResponse = new PaginatedResponse<Game>(games, paginationParams.PageNumber, paginationParams.PageSize, totalGames);

            return Ok(paginatedResponse); // 200 OK, page number, page size, total records, and data
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(long id)
        {
            var game = await _context.Games.FindAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(long id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid) // for some reason filter doesn't check as it does when POSTing
            {
                var errorsList = ModelState.SelectMany(property => property.Value.Errors)
                    .Select(error => error.ErrorMessage)
                    .ToList();
                return BadRequest(error: errorsList); // return this object, which is a list of strings
            }


            _context.Entry(game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, game);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(long id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GameExists(long id)
        {
            return _context.Games.Any(e => e.Id == id);
        }

        // for web sockets... but still related to games, so I put it here
        [Route("~/ws/startTestGames")] // ~ overrides default routing, so
        // instead of api/games/ws/... it will be just /ws/startTestGames
        public async Task GetTestWs()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

                while (true)
                {
                    List<Game> games = _fakeData.GenerateGames(3);
                    foreach(var game in games)
                    {
                        _context.Games.Add(game); // add each game
                    }
                    await _context.SaveChangesAsync(); // save changes

                    var message = "horse";
                    var bytes = Encoding.UTF8.GetBytes(message);
                    var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
                    await webSocket.SendAsync(arraySegment, // announce that games were inserted
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None);

                    Thread.Sleep(3000); //generate 5 games every 3 seconds
                }
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest; // not websocket request
            }
        }
    }
}
