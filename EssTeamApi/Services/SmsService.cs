using Twilio;
using Twilio.Rest.Api.V2010.Account;
using EssTeamApi.Data;
using Microsoft.EntityFrameworkCore;

namespace EssTeamApi.Services
{
    public class SmsService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public SmsService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // This is the method Hangfire calls every morning at 9:00 AM
        public async Task SendScheduledReminders()
        {
            // Calculate "Tomorrow" relative to today
            var tomorrow = DateTime.UtcNow.Date.AddDays(1);

            // 1. Check for Matches happening tomorrow
            var matches = await _context.Matches
                .Where(m => m.MatchDate.Date == tomorrow)
                .ToListAsync();

            // 2. Check for Trainings happening tomorrow
            var trainings = await _context.Trainings
                .Where(t => t.TrainingDate.Date == tomorrow)
                .ToListAsync();

            // If we have events, get the player phone numbers
            if (matches.Any() || trainings.Any())
            {
                var players = await _context.Players.ToListAsync();
                
                foreach (var player in players)
                {
                    if (string.IsNullOrEmpty(player.Phone)) continue;

                    // Send Match Reminders
                    foreach (var match in matches)
                    {
                        await SendSmsAsync(player.Phone, 
                            $"⚽ Match Reminder: Tomorrow vs {match.Opponent} at {match.Location}!");
                    }

                    // Send Training Reminders
                    foreach (var training in trainings)
                    {
                        await SendSmsAsync(player.Phone, 
                            $"🏃 Training Reminder: Tomorrow at {training.Location}!");
                    }
                }
            }
        }

        // The actual call to the Twilio API
        private async Task SendSmsAsync(string toPhone, string message)
        {
            var sid = _config["Twilio:AccountSid"];
            var token = _config["Twilio:AuthToken"];
            var fromPhone = _config["Twilio:PhoneNumber"];

            // Initialize the Twilio Client
            TwilioClient.Init(sid, token);

            try 
            {
                await MessageResource.CreateAsync(
                    body: message,
                    from: new Twilio.Types.PhoneNumber(fromPhone),
                    to: new Twilio.Types.PhoneNumber(toPhone)
                );
            }
            catch (Exception ex)
            {
                // Logs the error to your console if the phone number is invalid
                Console.WriteLine($"SMS Error to {toPhone}: {ex.Message}");
            }
        }
    }
}