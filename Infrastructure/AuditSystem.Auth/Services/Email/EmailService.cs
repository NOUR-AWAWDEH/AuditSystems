using AuditSystem.Auth.Services.Email;
using System.Net;
using System.Net.Mail;

public class EmailService(IConfiguration configuration) : IEmailService
{
    public async Task SendEmailAsync(string recipient, string subject, string body)
    {
        var email = configuration.GetValue<string>("EMAIL_CONFIGURATION:EMAIL")
                    ?? throw new ArgumentNullException("EMAIL_CONFIGURATION:EMAIL is missing in configuration.");

        var password = configuration.GetValue<string>("EMAIL_CONFIGURATION:PASSWORD")
                       ?? throw new ArgumentNullException("EMAIL_CONFIGURATION:PASSWORD is missing in configuration.");

        var host = configuration.GetValue<string>("EMAIL_CONFIGURATION:HOST")
                   ?? throw new ArgumentNullException("EMAIL_CONFIGURATION:HOST is missing in configuration.");

        var port = configuration.GetValue<int?>("EMAIL_CONFIGURATION:PORT") ?? throw new ArgumentNullException("EMAIL_CONFIGURATION:PORT is missing in configuration.");

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

