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
    public class UserActivitiesController : ControllerBase
    {
        private readonly PolyplayDbContext _context;

        public UserActivitiesController(PolyplayDbContext context)
        {
            _context = context;
        }

        // GET: api/UserActivities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserActivity>>> GetUserActivityLog()
        {
            // TODO MAKE THIS MORE EFFICIENT CUZ WTF
            var userActivities = await _context.UserActivityLog.ToListAsync();
            userActivities.ForEach(ua => ua.User = _context.Users.Find(ua.UserId));
            userActivities.ForEach(ua => ua.ActivityType = _context.ActivityTypes.Find(ua.ActivityTypeId));

            return userActivities;
        }

        [HttpGet("malicious")]
        public async Task<ActionResult<IEnumerable<UserActivity>>> GetUserActivityLogMalicious()
        {
            /*
            // TODO MAKE THIS MORE EFFICIENT CUZ WTF
            var userActivities = await _context.UserActivityLog.AsQueryable().Where(ua => ua.ActivityTypeId == 5)
                .Where(ua => (DateTime.Now - ua.ActivityTimestamp).Value.TotalMinutes < 2);

            userActivities.ForEach(ua => ua.User = _context.Users.Find(ua.UserId));
            userActivities.ForEach(ua => ua.ActivityType = _context.ActivityTypes.Find(ua.ActivityTypeId));

            return userActivities;
            */
            return null;
        }

        // GET: api/UserActivities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserActivity>> GetUserActivity(long id)
        {
            var userActivity = await _context.UserActivityLog.FindAsync(id);
            userActivity?.ActivityType = await _context.ActivityTypes.FindAsync(userActivity.ActivityTypeId) ?? new ActivityType
            {
                Id = 0, Info = "M Activity"
            };
            userActivity?.User = await _context.Users.FindAsync(userActivity.UserId);

            if (userActivity == null)
            {
                return NotFound();
            }

            return userActivity;
        }

        // PUT: api/UserActivities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserActivity(long id, UserActivity userActivity)
        {
            if (id != userActivity.Id)
            {
                return BadRequest();
            }

            _context.Entry(userActivity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserActivityExists(id))
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

        // POST: api/UserActivities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserActivity>> PostUserActivity(UserActivity userActivity)
        {
            _context.UserActivityLog.Add(userActivity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserActivity", new { id = userActivity.Id }, userActivity);
        }

        // DELETE: api/UserActivities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserActivity(long id)
        {
            var userActivity = await _context.UserActivityLog.FindAsync(id);
            if (userActivity == null)
            {
                return NotFound();
            }

            _context.UserActivityLog.Remove(userActivity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserActivityExists(long id)
        {
            return _context.UserActivityLog.Any(e => e.Id == id);
        }
    }
}
