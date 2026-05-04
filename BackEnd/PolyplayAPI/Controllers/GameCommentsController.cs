using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolyplayAPI.Filters;
using PolyplayAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolyplayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameCommentsController : ControllerBase
    {
        private readonly PolyplayDbContext _context;

        public GameCommentsController(PolyplayDbContext context)
        {
            _context = context;
        }

        // GET: api/GameComments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameComment>>> GetGameComments()
        {
            return await _context.GameComments.ToListAsync();
        }

        // GET: api/GameComments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameComment>> GetGameComment(long id)
        {
            var gameComment = await _context.GameComments.FindAsync(id);

            if (gameComment == null)
            {
                return NotFound();
            }

            return gameComment;
        }

        // PUT: api/GameComments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGameComment(long id, GameComment gameComment)
        {
            if (id != gameComment.Id)
            {
                return BadRequest();
            }

            _context.Entry(gameComment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameCommentExists(id))
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

        // POST: api/GameComments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<GameComment>> PostGameComment(GameComment gameComment)
        {
            _context.GameComments.Add(gameComment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGameComment", new { id = gameComment.Id }, gameComment);
        }

        // DELETE: api/GameComments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGameComment(long id)
        {
            var gameComment = await _context.GameComments.FindAsync(id);
            if (gameComment == null)
            {
                return NotFound();
            }

            _context.GameComments.Remove(gameComment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GameCommentExists(long id)
        {
            return _context.GameComments.Any(e => e.Id == id);
        }
    }
}
