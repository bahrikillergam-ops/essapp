using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EssTeamApi.Data;
using EssTeamApi.Models;

namespace EssTeamApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlayersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            var players = await _context.Players
                .Include(p => p.Manager)
                .ToListAsync();

            return Ok(players);
        }

        // GET: api/players/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            var player = await _context.Players
                .Include(p => p.Manager)
                .FirstOrDefaultAsync(p => p.PlayerId == id);

            if (player == null)
                return NotFound("Player not found");

            return Ok(player);
        }

        // POST: api/players
        [HttpPost]
        public async Task<ActionResult<Player>> CreatePlayer(Player player)
        {
            // Check manager exists
            var managerExists = await _context.Managers
                .AnyAsync(m => m.ManagerId == player.ManagerId);

            if (!managerExists)
                return BadRequest("Manager does not exist");

            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlayer),
                new { id = player.PlayerId }, player);
        }

        // PUT: api/players/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayer(int id, Player updatedPlayer)
        {
            if (id != updatedPlayer.PlayerId)
                return BadRequest("Player ID mismatch");

            _context.Entry(updatedPlayer).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/players/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _context.Players
                .FirstOrDefaultAsync(p => p.PlayerId == id);

            if (player == null)
                return NotFound("Player not found");

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/players/search?name=Ali
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Player>>> SearchPlayers(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Name is required");

            var players = await _context.Players
                .Where(p =>
                    p.FirstName!.ToLower().Contains(name.ToLower()) ||
                    p.LastName!.ToLower().Contains(name.ToLower()))
                .Include(p => p.Manager)
                .ToListAsync();

            if (!players.Any())
                return NotFound("No players found");

            return Ok(players);
        }
    }
}
