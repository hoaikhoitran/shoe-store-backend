using ShoeStore.API.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace ShoeStore.API.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendVerificationEmailAsync(string email, string username, string verificationToken)
        {
            try
            {
                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
                var smtpUsername = _configuration["EmailSettings:Username"];
                var smtpPassword = _configuration["EmailSettings:Password"];
                var fromEmail = _configuration["EmailSettings:FromEmail"];
                var fromName = _configuration["EmailSettings:FromName"] ?? "ShoeStore";
                var baseUrl = _configuration["AppSettings:BaseUrl"] ?? "https://localhost:5001";

                var verificationLink = $"{baseUrl}/api/auth/verify-email?token={verificationToken}";

                using var client = new SmtpClient(smtpServer, smtpPort)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(smtpUsername, smtpPassword)
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail!, fromName),
                    Subject = "Verify Your Email - ShoeStore",
                    Body = $@"
                        <html>
                        <body>
                            <h2>Welcome to ShoeStore, {username}!</h2>
                            <p>Thank you for registering. Please verify your email address by clicking the link below:</p>
                            <p><a href='{verificationLink}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Verify Email</a></p>
                            <p>Or copy and paste this link into your browser:</p>
                            <p>{verificationLink}</p>
                            <p>This link will expire in 24 hours.</p>
                            <p>If you did not register for this account, please ignore this email.</p>
                        </body>
                        </html>",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);

                await client.SendMailAsync(mailMessage);
                _logger.LogInformation($"Verification email sent to {email}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send verification email to {email}");
                throw;
            }
        }
    }
}
