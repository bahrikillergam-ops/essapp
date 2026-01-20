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

        // GET: api/players/search?name=Ali
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Player>>> SearchPlayers(string name)
        {
            var players = await _context.Players
                .Where(p =>
                    p.FirstName.ToLower().Contains(name.ToLower()) ||
                    p.LastName.ToLower().Contains(name.ToLower()))
                .ToListAsync();

            if (!players.Any())
                return NotFound("No players found");

            return players;
        }
    }
}
