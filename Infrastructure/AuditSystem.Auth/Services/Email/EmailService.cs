using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace AuditSystem.Auth.Services.Email;

public class EmailService(IConfiguration configuration) : IEmailService
{
    public async Task SendEmailAsync(string recipient, string subject, string body)
    {
        var email = configuration.GetValue<string>("EMAIL_CONFIGURATION:EMAIL")
                    ?? throw new ArgumentNullException("Email configuration is missing.");

        var password = configuration.GetValue<string>("EMAIL_CONFIGURATION:PASSWORD")
                       ?? throw new ArgumentNullException("Password configuration is missing.");

        var host = configuration.GetValue<string>("EMAIL_CONFIGURATION:HOST")
                   ?? throw new ArgumentNullException("Host configuration is missing.");

        var port = configuration.GetValue<int?>("EMAIL_CONFIGURATION:PORT")
                   ?? throw new ArgumentNullException("Port configuration is missing.");

        using var smtpClient = new SmtpClient(host, port)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(email, password)
        };

        using var message = new MailMessage(email, recipient, subject, body)
        {
            IsBodyHtml = true
        };

        await smtpClient.SendMailAsync(message);
    }
}

