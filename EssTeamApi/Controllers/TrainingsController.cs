using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EssTeamApi.Data;
using EssTeamApi.Models; // 👈 This connects to your Match.cs
using EssTeamApi.Services; // 👈 This connects to your EmailService.cs
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EssTeamApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;

        public TrainingsController(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<ActionResult<Training>> CreateTraining(Training training)
        {
            // 1. Save Training to Postgres
            _context.Trainings.Add(training);
            await _context.SaveChangesAsync();

            try {
                // 2. Fetch all emails from both tables
                var managerEmails = await _context.Managers.Select(m => m.Email).ToListAsync();
                var playerEmails = await _context.Players.Select(p => p.Email).ToListAsync();
                
                var allRecipients = managerEmails.Concat(playerEmails)
                    .Where(e => !string.IsNullOrEmpty(e))
                    .Distinct()
                    .ToList();

                // 3. Send notification
                await _emailService.SendMassTrainingEmailAsync(
                    allRecipients,
                    training.TrainingDate.ToString("D"), // dddd, dd MMMM yyyy
                    training.Time.ToString(@"hh\:mm"),
                    training.Location,
                    training.Focus
                );
            }
            catch (Exception ex) {
                Console.WriteLine($"Database saved, but email failed: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetTraining), new { id = training.TrainingId }, training);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Training>> GetTraining(int id)
        {
            var training = await _context.Trainings.FindAsync(id);
            return training == null ? NotFound() : Ok(training);
        }
    }
}