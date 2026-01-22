using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EssTeamApi.Data;
using EssTeamApi.Models;
using EssTeamApi.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EssTeamApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;

        public MatchesController(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<ActionResult<Match>> CreateMatch(Match match)
        {
            // Ensure date is UTC for PostgreSQL compatibility
            match.MatchDate = System.DateTime.SpecifyKind(match.MatchDate, System.DateTimeKind.Utc);

            _context.Matches.Add(match);
            await _context.SaveChangesAsync();

            try 
            {
                var managerEmails = await _context.Managers.Select(m => m.Email).ToListAsync();
                var playerEmails = await _context.Players.Select(p => p.Email).ToListAsync();

                var allRecipients = managerEmails.Concat(playerEmails)
                    .Where(e => !string.IsNullOrWhiteSpace(e))
                    .Distinct()
                    .ToList();

                await _emailService.SendMassMatchEmailAsync(
                    allRecipients,
                    match.MatchDate.ToString("dddd, dd MMMM yyyy"), 
                    match.Opponent ?? "Unknown Opponent",
                    match.Location ?? "TBA"
                );
            }
            catch (System.Exception ex) 
            {
                System.Console.WriteLine("Email error: " + ex.Message);
            }

            return CreatedAtAction(nameof(GetMatch), new { id = match.MatchId }, match);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Match>> GetMatch(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            return match == null ? NotFound() : Ok(match);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Match>>> GetMatches()
        {
            return await _context.Matches.ToListAsync();
        }

        // --- NEW: UPDATE METHOD ---
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMatch(int id, Match match)
        {
            if (id != match.MatchId) return BadRequest("ID mismatch");

            // Fix date for PostgreSQL
            match.MatchDate = System.DateTime.SpecifyKind(match.MatchDate, System.DateTimeKind.Utc);

            _context.Entry(match).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Matches.Any(e => e.MatchId == id)) return NotFound();
                else throw;
            }

            return NoContent(); // Success (204)
        }

        // --- NEW: DELETE METHOD ---
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match == null) return NotFound();

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();

            return NoContent(); // Success (204)
        }
    }
}