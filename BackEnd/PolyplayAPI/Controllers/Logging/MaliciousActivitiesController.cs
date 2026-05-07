using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolyplayAPI.Models;
using PolyplayAPI.Models.Logging;

namespace PolyplayAPI.Controllers.Logging
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaliciousActivitiesController : ControllerBase
    {
        private readonly PolyplayDbContext _context;

        public MaliciousActivitiesController(PolyplayDbContext context)
        {
            _context = context;
        }

        // GET: api/MaliciousActivities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaliciousActivity>>> GetMaliciousActivityLog()
        {
            var maliciousActivities = await _context.MaliciousActivityLog.ToListAsync();
            maliciousActivities.ForEach(ua => ua.User = _context.Users.Find(ua.UserId));
            maliciousActivities.ForEach(ua => ua.ActivityType = _context.ActivityTypes.Find(ua.ActivityTypeId));

            return maliciousActivities;
        }

        // GET: api/MaliciousActivities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MaliciousActivity>> GetMaliciousActivity(long id)
        {
            var maliciousActivity = await _context.MaliciousActivityLog.FindAsync(id);

            if (maliciousActivity == null)
            {
                return NotFound();
            }

            return maliciousActivity;
        }

        // PUT: api/MaliciousActivities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaliciousActivity(long id, MaliciousActivity maliciousActivity)
        {
            if (id != maliciousActivity.Id)
            {
                return BadRequest();
            }

            _context.Entry(maliciousActivity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaliciousActivityExists(id))
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

        // POST: api/MaliciousActivities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MaliciousActivity>> PostMaliciousActivity(MaliciousActivity maliciousActivity)
        {
            _context.MaliciousActivityLog.Add(maliciousActivity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMaliciousActivity", new { id = maliciousActivity.Id }, maliciousActivity);
        }

        // DELETE: api/MaliciousActivities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaliciousActivity(long id)
        {
            var maliciousActivity = await _context.MaliciousActivityLog.FindAsync(id);
            if (maliciousActivity == null)
            {
                return NotFound();
            }

            _context.MaliciousActivityLog.Remove(maliciousActivity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MaliciousActivityExists(long id)
        {
            return _context.MaliciousActivityLog.Any(e => e.Id == id);
        }
    }
}
