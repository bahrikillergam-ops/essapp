using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EssTeamApi.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        // 1. Method for Match Alerts (Blue Theme)
        public async Task SendMassMatchEmailAsync(List<string> recipients, string date, string opponent, string location)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ESS Team", _config["EmailSettings:Username"]));
            
            foreach (var email in recipients)
            {
                message.Bcc.Add(MailboxAddress.Parse(email));
            }

            message.Subject = $"⚽ Match Alert: vs {opponent}";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $@"
                <div style='font-family: Arial, sans-serif; border: 2px solid #0056b3; padding: 20px; border-radius: 10px;'>
                    <h2 style='color: #0056b3;'>New Match Scheduled!</h2>
                    <p><strong>Opponent:</strong> {opponent}</p>
                    <p><strong>Date:</strong> {date}</p>
                    <p><strong>Location:</strong> {location}</p>
                    <p style='color: #555;'>Please confirm your availability in the app.</p>
                </div>";

            message.Body = bodyBuilder.ToMessageBody();
            await SendAsync(message);
        }

        // 2. Method for Training Alerts (Red Theme)
        public async Task SendMassTrainingEmailAsync(List<string> recipients, string date, string time, string location, string focus)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ESS Team", _config["EmailSettings:Username"]));
            
            foreach (var email in recipients)
            {
                message.Bcc.Add(MailboxAddress.Parse(email));
            }

            message.Subject = $"🏃 Training Session: {date}";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $@"
                <div style='font-family: Arial, sans-serif; border: 2px solid #d9534f; padding: 20px; border-radius: 10px;'>
                    <h2 style='color: #d9534f;'>Training Update</h2>
                    <p><strong>Date:</strong> {date}</p>
                    <p><strong>Time:</strong> {time}</p>
                    <p><strong>Location:</strong> {location}</p>
                    <p><strong>Focus:</strong> {focus}</p>
                    <hr>
                    <p style='font-style: italic;'>Be on time and bring your full kit.</p>
                </div>";

            message.Body = bodyBuilder.ToMessageBody();
            await SendAsync(message);
        }

        // Private helper to connect to SMTP
        private async Task SendAsync(MimeMessage message)
        {
            using var client = new SmtpClient();
            try
            {
                // Connects using settings from your appsettings.json
                await client.ConnectAsync(
                    _config["EmailSettings:SmtpServer"], 
                    int.Parse(_config["EmailSettings:Port"]), 
                    true
                );

                await client.AuthenticateAsync(
                    _config["EmailSettings:Username"], 
                    _config["EmailSettings:Password"]
                );

                await client.SendAsync(message);
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
    }
}