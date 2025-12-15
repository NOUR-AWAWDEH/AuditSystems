using System.Net;
using System.Net.Mail;
using System.Text;
using AuditSystem.Auth.Services.Email.Abstractions;
using AuditSystem.Auth.Services.Email.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuditSystem.Auth.Services.Email.Services.Smtp;

public sealed class SmtpEmailSender : IEmailSender
{
    private readonly SmtpSettings _settings;
    private readonly ISmtpClientFactory _factory;
    private readonly ILogger<SmtpEmailSender> _logger;

    public SmtpEmailSender(
        IOptions<SmtpSettings> settings,
        ISmtpClientFactory factory,
        ILogger<SmtpEmailSender> logger)
    {
        _settings = settings.Value;
        _factory = factory;
        _logger = logger;
    }

    public async Task SendAsync(EmailMessage message)
    {
        if (message.To == null || message.To.Count == 0)
            throw new ArgumentException("Recipient list cannot be empty");

        using var mail = new MailMessage
        {
            From = new MailAddress(
                _settings.SenderAddress,
                _settings.SenderName),
            Subject = message.Subject,
            Body = message.Body,
            IsBodyHtml = message.IsHtml,
            BodyEncoding = Encoding.UTF8
        };

        foreach (var recipient in message.To.Where(r => !string.IsNullOrWhiteSpace(r)))
            mail.To.Add(recipient);

        if (mail.To.Count == 0)
            throw new ArgumentException("No valid recipients");

        using var client = _factory.CreateClient();

        try
        {
            await client.SendMailAsync(mail);
            _logger.LogInformation(
                "Email sent to {Recipients}",
                string.Join(", ", mail.To.Select(x => x.Address)));
        }
        catch (SmtpException ex)
        {
            throw new Exception("SMTP send failed", ex);
        }
    }
}
