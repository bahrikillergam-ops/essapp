using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EssTeamApi.Data;
using EssTeamApi.Models;

namespace EssTeamApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ManagersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/managers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Manager>>> GetManagers()
        {
            var managers = await _context.Managers
                .Include(m => m.Players)
                .Include(m => m.Matches)
                .Include(m => m.Trainings)
                .Include(m => m.Equipment)
                .ToListAsync();

            return Ok(managers);
        }

        // GET: api/managers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Manager>> GetManager(int id)
        {
            var manager = await _context.Managers
                .Include(m => m.Players)
                .Include(m => m.Matches)
                .Include(m => m.Trainings)
                .Include(m => m.Equipment)
                .FirstOrDefaultAsync(m => m.ManagerId == id);

            if (manager == null)
                return NotFound("Manager not found");

            return Ok(manager);
        }

        // POST: api/managers
        [HttpPost]
        public async Task<ActionResult<Manager>> CreateManager(Manager manager)
        {
            _context.Managers.Add(manager);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetManager),
                new { id = manager.ManagerId }, manager);
        }

        // PUT: api/managers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateManager(int id, Manager updatedManager)
        {
            if (id != updatedManager.ManagerId)
                return BadRequest("Manager ID mismatch");

            _context.Entry(updatedManager).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/managers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteManager(int id)
        {
            var manager = await _context.Managers
                .FirstOrDefaultAsync(m => m.ManagerId == id);

            if (manager == null)
                return NotFound("Manager not found");

            _context.Managers.Remove(manager);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/managers/search?name=Ahmed
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Manager>>> SearchManagers(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Name is required");

            var managers = await _context.Managers
                .Where(m =>
                    m.FirstName!.ToLower().Contains(name.ToLower()) ||
                    m.LastName!.ToLower().Contains(name.ToLower()))
                .ToListAsync();

            if (!managers.Any())
                return NotFound("No managers found");

            return Ok(managers);
        }
    }
}
