using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;

namespace AuditSystem.Auth.Services.Email;

public class EmailService(
    IOptions<SMTPConfigModel> smtpConfig,
    ILogger<EmailService> logger) 
    : IEmailService
{
    private readonly SMTPConfigModel _config = smtpConfig.Value;

    public async Task SendEmailAsync(EmailRequest emailRequest)
    {
        if (emailRequest == null)
            throw new ArgumentNullException(nameof(emailRequest));

        var body = GetEmailBody(emailRequest.TemplateName);

        foreach (var placeholder in emailRequest.Placeholders)
        {
            body = body.Replace($"{{{placeholder.Key}}}", placeholder.Value);
        }

        await SendEmailInternalAsync(
            emailRequest.ToEmails,
            emailRequest.Subject,
            body,
            isHtml: true);
    }

    public async Task SendEmailAsync(string email, string subject, string body, bool isHtml = true)
    {
        await SendEmailAsync(new List<string> { email }, subject, body, isHtml);
    }

    public async Task SendEmailAsync(List<string> recipients, string subject, string body, bool isHtml = true)
    {
        await SendEmailInternalAsync(recipients, subject, body, isHtml);
    }

    private async Task SendEmailInternalAsync(List<string> recipients, string subject, string body, bool isHtml)
    {
        if (recipients == null || recipients.Count == 0)
            throw new ArgumentException("Recipient email list cannot be empty.", nameof(recipients));

        using MailMessage mailMessage = new()
        {
            From = new MailAddress(_config.SenderAddress, _config.SenderDisplayName),
            IsBodyHtml = isHtml,
            Subject = subject,
            Body = body,
            BodyEncoding = Encoding.UTF8
        };

        foreach (var recipient in recipients)
        {
            if (!string.IsNullOrWhiteSpace(recipient))
            {
                mailMessage.To.Add(recipient);
            }
        }

        if (mailMessage.To.Count == 0)
            throw new ArgumentException("No valid email addresses provided.", nameof(recipients));

        using SmtpClient smtpClient = new()
        {
            Host = _config.Host,
            Port = _config.Port,
            EnableSsl = _config.EnableSSL,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_config.UserName, _config.Password),
            DeliveryMethod = SmtpDeliveryMethod.Network
        };

        try
        {
            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (SmtpException ex)
        {
            throw new EmailSendException("Failed to send email", ex);
        }
    }
    
    private string GetEmailBody(string templateName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"AuditSystem.Auth.Services.Email.EmailTemplates.{templateName}.html";

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            throw new FileNotFoundException($"Embedded resource not found: {resourceName}");
        }
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}